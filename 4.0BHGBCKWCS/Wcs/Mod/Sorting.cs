using Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using NJIS.Common;
using WCS.Common;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;
using WcsService;
using WcsModel;

namespace WCS.Mod
{

    public class Sorting:Singleton<Sorting>
    {
        private Sorting() { }

        private static ISortingContract iContract = SortingService.GetInstance();

         long sort_taskid = 0; //保存当前拣选任务ID
         string msg = "";
         int[] arr = { 2001, 2002, 2004 };
         int[] arr2 = { 2001, 2002, 2003, 2004, 2005 };
         Dictionary<string, SortStation> buffer = new Dictionary<string, SortStation>(); //只保存有板(>=2)的的拣选位
         Dictionary<string, SortStation> buffer2 = new Dictionary<string, SortStation>(); //只保存有板(>=1)的的拣选位
         List<int> free = new List<int>();  //只保存无板(<=1)的拣选位

        private List<SortingStationInfo> sortingStationInfos = null;

        private List<SortingStationInfo> SortingStationInfos =>
            (sortingStationInfos ?? (sortingStationInfos = iContract.GetSortingStationInfos()));

        //分配指令
        public void DoCmd()
        {
            try
            {
                //保险起见，先清除WCS写的抓板数据
                //OpcHsc.ClearMainpulatorTask();

                //保险起见，先验证2001,2002,2003,2004拣选工位PLC计算的板件数量与WCS计算的板件数量是否一致
                if (OpcHsc.RMCanDo(1))
                {
                    foreach (var sortingStationInfo in SortingStationInfos)
                    {
                        if(sortingStationInfo.StationType == 0) continue;
                        if (CheckWCSPLCAmount(sortingStationInfo.StationNo, sortingStationInfo.BookCount))
                        {
                            WCSSql.InsertLog($"拣选工位{sortingStationInfo.StationNo}：PLC存储的数量与WCS存储的数量不一致！", "ERROR");
                            return;
                        }
                    }
                }

                //将拣选完后的剩下最后一块空垫板，从2001,2002,2004自动抓到2005
                if (PutLastEmpty())
                {
                    return;
                }

                //获取当前正在拣选的任务
                msg = WCSSql.GetCurrentSortTaskId(out sort_taskid);
                if (msg.Length > 0)
                {
                    WCSSql.InsertLog(msg, "ERROR");
                    return;
                }

                if (sort_taskid == 0)
                {
                    var count2003 = OpcHsc.ReadBoradsCount(2003);
                    if (count2003 <= 1)
                    {
                        //开始一个新的拣选任务
                        StartSortedTask();
                    }
                    return;
                }

                //如果机械手已接收指令，不往下执行
                if (!OpcHsc.RMCanDo(1)) { return; }

                int plc_amount = OpcHsc.ReadBoradsCount(2003);  //从机械手PLC读取的拣选工位的板件数量
                int rlt = WCSSql.CheckSortTaskIsFinished(sort_taskid, plc_amount);
                if (rlt == 1)
                {
                    //结束当前执行的拣选任务
                    FinishSortedTask(sort_taskid);
                }
                else if (rlt == 0)
                {
                    //拣选未完成
                    SendCmd(sort_taskid);
                }
                else if (rlt == 998)
                {
                    //PLC记录的数量与WCS记录的不一致
                    WCSSql.InsertLog($"[CheckSortTaskIsFinished]拣选任务：{sort_taskid}PLC记录的拣选数量与WCS记录的拣选数量不一致！请核查！", "ERROR");
                }
                else
                {
                    //异常
                    WCSSql.InsertLog($"[CheckSortTaskIsFinished]拣选任务：{sort_taskid}执行异常！", "ERROR");
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("机械手拣选错误！[DoCmd]" + ex.Message, "ERROR");
            }
        }

        //抓最后一块空垫板
        private  bool PutLastEmpty()
        {
            //2001,2002,2004
            foreach (int no in arr)
            {
                if (OpcHsc.ReadBoradsCount(no) != 1) { continue; }

                //如果2003没有板，直接放板
                if (OpcHsc.ReadBoradsCount(2003) == 0)
                {
                    if (OpcHsc.RMCanDo(1))
                    {
                        var rlt2 = OpcHsc.WriteToMainpulator(no, 2003);
                        if (rlt2)
                        {
                            WCSSql.InsertLog($"机械手[1]抓取最后一块空垫板，From：{no}，To：2003，花色：空垫板", "LOG");
                        }
                    }

                    return true;
                }

                if (OpcHsc.ReadBoradsCount(2005) >= 42)
                {
                    if (OpcHsc.ReadEmptyBuffersCount() >= 2)
                    {
                        //防止重复向WMS发出申请
                        if (OpcHsc.ReadInWareCmd(2005)) { return true; }

                        //向WMS请求空垫板入库  
                        //给2005工位写清除指令
                        //给2005工位写空垫板入库指令,执行入库
                        var request = new RequestInfo();
                        request.ReqType = 3;
                        request.Amount = 42;
                        request.FromPosition = "2005";
                        var rlt = WCSSql.RequestTask(request);
                        if (rlt.Status == 200)
                        {
                            var msg = "";
                            var task = WCSSql.GetTaskByReqId(request.ReqId, ref msg);
                            if (task == null)
                            {
                                WCSSql.InsertLog($"WCS获取任务失败[GetTaskByReqId]，请求ID：{request.ReqId}，{msg}", "ERROR");
                            }
                            else
                            {
                                //给线体任务                               
                                OpcHsc.WriteDeviceData("GT217", task.PilerNo, task.target);
                                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                                WCSSql.InsertLog($"空垫板开始入库！垛号：{task.PilerNo}，起始位：{task.FromPosition}，目标位：{task.ToPosition}", "LOG");
                            }
                        }
                    }
                    else
                    {
                        if (!OpcHsc.ReadGT217GoBackCmd())
                        {
                            OpcHsc.WriteGT217GoBackCmd();     //退回指令      
                        }
                        WCSSql.InsertLog($"正在向PLC请求退回空垫板：GT217-->GT216", "LOG");
                        Thread.Sleep(5 * 1000);  //等待5s...
                    }
                }
                else
                {
                    if (OpcHsc.RMCanDo(1))
                    {
                        var rlt = OpcHsc.WriteToMainpulator(no, 2005);
                        if (rlt)
                        {
                            WCSSql.InsertLog($"机械手[1]抓取最后一块空垫板，From：{no}，To：2005，花色：空垫板", "LOG");
                        }
                    }
                }

                return true;

            }

            return false;
        }

        //更新余料的数量信息，供WMS获取统计
        private  void SetSortedBuffers()
        {
            var dic = GetCurrentBuffFromPLC();
            var sortbuffers = "";
            foreach (SortStation ss in dic.Values)
            {
                sortbuffers += $"{ss.ProductCode}|{ss.Amount};";
            }

            WCSSql.SetWcsConfig("SortBuffer", sortbuffers);
        }

        /// <summary>
        /// 验证拣选工位所剩下的板材数与数据库的板材数是否一致
        /// </summary>
        /// <param name="stationNo">拣选工位</param>
        /// <param name="count">数据库剩下的数量</param>
        /// <returns></returns>
        private bool CheckWCSPLCAmount(int stationNo, int count)
        {
            return count == OpcHsc.ReadBoradsCount(stationNo);
        }

        /// <summary>
        /// 验证拣选工位所剩下的板材数与数据库的板材数是否一致
        /// </summary>
        /// <param name="sortingStationInfos"></param>
        /// <returns></returns>
        private string CheckWCSPLCAmount(List<SortingStationInfo> sortingStationInfos)
        {
            bool flag = true;
            foreach (var sortingStationInfo in sortingStationInfos)
            {
                if (sortingStationInfo.StationType == 0) //空垫板
                {
                    continue;
                }

                flag &= sortingStationInfo.BookCount == OpcHsc.ReadBoradsCount(sortingStationInfo.StationNo);
                if (!flag)
                {
                    return $"拣选工位{sortingStationInfo.StationNo}：PLC存储的数量与WCS存储的数量不一致！";
                }
            }

            return string.Empty;
            //if (WCSSql.CheckSortCount(2001, OpcHsc.ReadBoradsCount(2001)) == false)
            //{
            //    return "拣选工位2001：PLC存储的数量与WCS存储的数量不一致！";
            //}
            //if (WCSSql.CheckSortCount(2002, OpcHsc.ReadBoradsCount(2002)) == false)
            //{
            //    return "拣选工位2002：PLC存储的数量与WCS存储的数量不一致！";
            //}
            //if (WCSSql.CheckSortCount(2003, OpcHsc.ReadBoradsCount(2003)) == false)
            //{
            //    return "拣选工位2003：PLC存储的数量与WCS存储的数量不一致！";
            //}
            //if (WCSSql.CheckSortCount(2004, OpcHsc.ReadBoradsCount(2004)) == false)
            //{
            //    return "拣选工位2004：PLC存储的数量与WCS存储的数量不一致！";
            //}

            //return "";
        }

        //手动回库指令
        public  void ReturnByHand(int station)
        {
            if (!OpcHsc.RMCanDo(1))
            {
                WCSSql.InsertLog("【ReturnByHand】在机械手空闲的状态下才能做手动回库的操作！", "ERROR");
                return;
            }

            if (station == 2003)
            {

            }
            else if (station == 2001 || station == 2002 || station == 2004)
            {
                //   2.2如果是3001,3002,3004 调用WMS接口【参数：ReqType=4,ReqId=10006,FromStation=3001,ProductColor='903S',Count=12】
                var plc_ProductCode = OpcHsc.ReadStaionProductCode(station); //从线体PLC读取到花色信息
                var plc_Amount = OpcHsc.ReadBoradsCount(station);  //从机械手PLC读取到到数量
                var pilerno = OpcHsc.ReadPilerNoByStationNo(station);

                if (plc_ProductCode == "")
                {
                    WCSSql.InsertLog($"【ReturnByHand】从站台{station}获取不到花色信息！", "ERROR");
                    return;
                }

                if (plc_Amount <= 1)
                {
                    WCSSql.InsertLog($"【ReturnByHand】站台{station}没有余料！", "ERROR");
                    return;
                }

                if (pilerno == 0)
                {
                    WCSSql.InsertLog($"【ReturnByHand】从站台{station}获取不到垛号！", "ERROR");
                    return;
                }

                var req = new RequestInfo()
                {
                    ReqType = 4,  //余料回库
                    FromPosition = station.ToString(),
                    Amount = plc_Amount - 1,
                    PilerNo = pilerno
                };


                var wms_rlt = WCSSql.RequestTask(req);

                if (wms_rlt.Status == 200)
                {
                    WCSSql.InsertLog($"余料入库[手动]申请成功！请求编号：{wms_rlt.ReqId}，工位：{req.FromPosition}，数量：{req.Amount}", "LOG");

                    var msg = "";
                    var task = WCSSql.GetTaskByReqId(wms_rlt.ReqId, ref msg);
                    if (task == null)
                    {
                        WCSSql.InsertLog($"余料入库[手动]申请失败！错误信息：{msg}", "ERROR");
                        //return false;
                    }

                    //写任务给线体,清掉板件数量
                    OpcHsc.WriteDeviceData(station, task.PilerNo, task.target);
                    WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                    WCSSql.InsertLog($"余料入库开始[手动]！垛号：{task.PilerNo}，起始位：{station}，目标位：{task.ToPosition}，花色：{task.ProductCode}", "LOG");
                }
                else
                {
                    WCSSql.InsertLog($"余料入库[手动]申请失败！错误信息：{wms_rlt.Message}", "ERROR");
                }
            }
            else if (station == 2005)
            {
                //   2.3如果是2005 调用WMS接口【参数：ReqType=3,ReqId=10007,FromStation=3005,Count=39】
                var plc_empty_count = OpcHsc.ReadBoradsCount(2005); //从机械手PLC获取空垫板数量

                if (plc_empty_count <= 5)
                {
                    LogHelper.LogErrorInfo($"【ReturnByHand】空垫板数量少于6块不能回！");
                    return;
                }

                var req = new RequestInfo()
                {
                    ReqType = 3,
                    Amount = plc_empty_count,
                    FromPosition = "2005"
                };
                //var NewReqId = WCSSql.CreateRequest(req);

                //if (NewReqId == 0)
                //{
                //    LogHelper.LogErrorInfo($"【ReturnByHand】WCS创建空垫板请求执行异常！");
                //    return;
                //}

                //调用WMS接口

                //给机械手PLC写清除指令
                //给线体PLC写入库任务，入库！！！
            }
        }

        //开始拣选时，如果2003工位没有空垫板， 则 A：从2001，2002或2004上抓上保护板； B：从2005抓空垫板
        private  void PutFirstEmptyTo2003(long sortid)
        {
            if (!OpcHsc.RMCanDo(1)) return;

            var lst = GetCurrentBuffFromPLC();
            var ret = false;

            //A计划
            foreach (var item in lst)
            {
                if (item.Value.Amount == 42)
                {
                    ret = OpcHsc.WriteToMainpulator(item.Value.Code, 2003);
                    if (ret)
                    {
                        WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务：{sortid}，FromStation：{item.Value.Code}，ToStation：2003", "LOG");
                        return;
                    }
                }
            }

            //B计划
            var count2005 = OpcHsc.ReadBoradsCount(2005);
            if (count2005 == 0)
            {
                if (!OpcHsc.ReadGT216GoHeadCmd() && OpcHsc.ReadBoradsCount(2006) > 0)
                {
                    OpcHsc.WriteGT216GoBackCmd();
                }
                WCSSql.InsertLog($"拣选任务：{sortid} 正在向PLC请求前进空垫板：GT216-->GT217", "LOG");
                Thread.Sleep(10 * 1000);  //等待5s...
            }
            else
            {
                ret = OpcHsc.WriteToMainpulator(2005, 2003);
                if (ret)
                {
                    WCSSql.InsertLog($" 拣选任务放置第一块空垫板：{sortid}，FromStation：{2005}，ToStation：2003", "LOG");
                    return;
                }
            }

            if (ret == false)
            {
                WCSSql.InsertLog(" 机械手拣选放置第一块空垫板任务失败！[PutFirstEmptyTo2003]", "ERROR");
            }
        }

        //执行抓板命令
        private  void SendCmd(long SortId)
        {
            int count2003 = OpcHsc.ReadBoradsCount(2003); //从机械手PLC获取当前拣选工位的板件数量

            //拣选时，先放置第一块空垫板
            if (count2003 == 0)
            {
                PutFirstEmptyTo2003(SortId);
                return;
            }

            var Next_Productcode = WCSSql.GetNextProductCode(SortId, count2003);

            if (Next_Productcode == "")
            {
                WCSSql.InsertLog($"[SendCmd]拣选任务：[{SortId}]获取第{count2003}块板花色错误！", "ERROR");
                return;
            }

            //4.从电气PLC获取2001，2002，2004花色，从龙门PLC获取2001，2002，2004剩余的数量
            var dicbuffer = GetCurrentBuffFromPLC();
            var rt = false;
            var stationNo = 0;
            //5.如果有可以抓取的花色，则写命令给龙门PLC抓板
            if (dicbuffer.ContainsKey(Next_Productcode))
            {
                stationNo = dicbuffer[Next_Productcode].Code;

                //有上保护板，先去掉上保护板
                if (dicbuffer[Next_Productcode].Amount == 42)
                {
                    if (OpcHsc.ReadBoradsCount(2005) >= 42)
                    {
                        var pilerscount = OpcHsc.ReadEmptyBuffersCount();  //获取缓存的空垫板垛数
                        if (pilerscount >= 2)
                        {
                            //避免重复申请
                            if (OpcHsc.ReadInWareCmd(2005)) { return; }
                            //向WMS请求空垫板入库  
                            //给2005工位写清除指令
                            //给2005工位写空垫板入库指令,执行入库
                            //空垫板入库申请，满垛42块板
                            var request = new RequestInfo();
                            request.ReqType = 3;
                            request.Amount = 42;
                            request.FromPosition = "2005";
                            var rlt = WCSSql.RequestTask(request);

                            if (rlt.Status == 200)
                            {
                                //WMS反馈成功
                                var msg = "";
                                var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
                                if (task == null)
                                {
                                    WCSSql.InsertLog($"空垫板入库申请失败！{msg}", "ERROR");
                                    return;
                                }

                                WCSSql.InsertLog($"WCS任务申请成功！请求编号：{rlt.ReqId}，类型：空垫板入库，垛号：{task.PilerNo}，起始位：2005，目标位：{task.ToPosition}", "LOG");
                                OpcHsc.WriteDeviceData(2005, task.PilerNo, task.target);
                                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                                WCSSql.InsertLog($"空垫板入库开始！垛号：{task.PilerNo}，起始位：2005，目标位：{task.ToPosition}", "LOG");
                            }
                            else
                            {
                                WCSSql.InsertLog(rlt.Message, "ERROR");
                                return;
                            }
                        }
                        else
                        {
                            //给PLC写退回指令 Action=1  GT217-->GT216
                            if (!OpcHsc.ReadGT217GoBackCmd())
                            {
                                OpcHsc.WriteGT217GoBackCmd();
                            }

                            WCSSql.InsertLog($"拣选任务：{SortId} 正在向PLC请求退回空垫板：GT217-->GT216", "LOG");
                            Thread.Sleep(10 * 1000);  //等待5s...
                        }
                        return;
                    }

                    //给机械手写抓取空垫板指令 FromStation：dicbuffer[Next_Productcode].Code,,ToStaion:3005
                    if (OpcHsc.RMCanDo(1))
                    {
                        rt = OpcHsc.WriteToMainpulator(stationNo, 2005);
                        if (rt)
                        {
                            //OpcHs.SetHaveUpProduct(stationNo, 0);
                            WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务：{SortId}，FromStation：{stationNo}，ToStation：2005", "LOG");
                        }
                        else
                        {
                            WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务失败：{SortId}，FromStation：{stationNo}，ToStation：2005", "ERROR");
                        }
                    }

                    return;
                }

                if (dicbuffer[Next_Productcode].Amount >= 2 && OpcHsc.ReadBoradsCount(2003) > 0)
                {
                    //给机械手写抓取指令  FromStation: lst1[Next_Productcode]      ToStation:2003
                    if (OpcHsc.RMCanDo(1))
                    {
                        rt = OpcHsc.WriteToMainpulator(stationNo, 2003);
                        if (rt)
                        {
                            WCSSql.InsertLog($"机械手[1]抓板，拣选任务：{SortId}，StackIndex：{count2003}，花色：{Next_Productcode}，From：{stationNo}，To：2003 ！", "LOG");
                            WCSSql.SetSortInfoStatus(SortId, count2003); //更新拣选明细
                        }
                        else
                        {
                            WCSSql.InsertLog($"机械手[1]抓板失败！", "ERROR");
                        }
                    }
                }
            }
            else
            {
                //可能需要的花色还在来的线体上，等待10秒钟
                WCSSql.InsertLog($"机械手[1]拣选等待原料！拣选任务：{SortId}，StackIndex：{count2003}，花色：{Next_Productcode}，From：未知，To：2003  没有花色，静等20s...", "LOG");
                Thread.Sleep(1000 * 20);
            }
        }

        //从PLC获取2001,2002,2004工位当前暂存的花色，数量信息
        public  Dictionary<string, SortStation> GetCurrentBuffFromPLC()
        {
            buffer.Clear();

            foreach (int no in arr)
            {
                var count = OpcHsc.ReadBoradsCount(no);
                var pcode = "";
                if (count >= 2)
                {
                    pcode = OpcHsc.ReadStaionProductCode(no);
                    buffer.Add(pcode,
                           new SortStation()
                           {
                               Amount = count,
                               Code = no,
                               ProductCode = pcode
                               //HaveUpProtect = OpcHs.IsHaveUpProduct(no)
                           });
                }
            }

            return buffer;
        }

        public  Dictionary<string, SortStation> GetCurrentBuffFromPLC2()
        {
            buffer2.Clear();

            foreach (int no in arr2)
            {
                var count = OpcHsc.ReadBoradsCount(no);
                var code = "";
                if (count >= 1)
                {
                    code = OpcHsc.ReadStaionProductCode(no);
                    buffer2.Add(code,
                           new SortStation()
                           {
                               Amount = count,
                               Code = no,
                               ProductCode = code
                               //HaveUpProtect = OpcHs.IsHaveUpProduct(no)
                           });
                }
            }

            return buffer2;
        }

        //从PLC获取2001,2002,2004空闲的工位
        private  List<int> GetFreeSortStation()
        {
            free.Clear();
            foreach (int no in arr)
            {
                var count = OpcHsc.ReadBoradsCount(no);
                if (count <= 1)
                {
                    free.Add(no);
                }
            }

            return free;
        }

        //余料回库申请
        private  bool LessReturnBack(SortStation ss, long sortId)
        {
            //该工位的余料回库已经申请过了
            //if (WCSSql.GetSortStationTaskId2(ss.Code) == sortId) { return true; }
            if (OpcHsc.ReadInWareCmd(ss.Code)) { return true; }

            //余料回库请求
            var req = new RequestInfo();
            req.ReqType = 4;
            req.Amount = ss.Amount;
            req.FromPosition = ss.Code.ToString();
            var pilerNo = OpcHsc.ReadPilerNoByStationNo(ss.Code);
            req.PilerNo = pilerNo;

            var wms_rlt = WCSSql.RequestTask(req);

            if (wms_rlt.Status == 200)
            {
                WCSSql.InsertLog($"余料自动入库申请成功！请求编号：{wms_rlt.ReqId}，垛号：{pilerNo}，工位：{req.FromPosition}，数量：{req.Amount}", "LOG");

                var msg = "";
                var task = WCSSql.GetTaskByReqId(wms_rlt.ReqId, ref msg);
                if (task == null)
                {
                    WCSSql.InsertLog($"余料自动入库申请失败！请求编号：{wms_rlt.ReqId}，垛号：{pilerNo}，工位：{req.FromPosition}，错误信息：{msg}", "ERROR");
                    return false;
                }

                OpcHsc.WriteDeviceData(ss.Code, task.PilerNo, task.target);
                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                WCSSql.InsertLog($"余料自动入库开始！垛号：{task.PilerNo}，起始位：{ss.Code}，目标位：{task.ToPosition}，花色：{task.ProductCode}", "LOG");
                return true;
            }
            else
            {
                WCSSql.InsertLog($"余料自动入库申请失败[999]！工位：{req.FromPosition}，错误信息：{wms_rlt.Message}", "ERROR");
                return false;
            }
        }

        //余料自动回库计算
        private  bool AutoReteurn(DataTable dt, long sortId)
        {
            var buffer = GetCurrentBuffFromPLC();  //当前存储的花色信息  2001,2002,2004
            var free = GetFreeSortStation();  //当前空闲的拣选工位   2001,2002,2004

            if (buffer.Count + free.Count != 3)
            {
                WCSSql.InsertLog("[AutoReteurn]PLC存储的花色和数量信息有误！", "LOG");
                return false;
            }

            if (free.Count >= dt.Rows.Count) { return true; }

            var UNeed = new List<string>();  //保存当前工位有 但是 拣选任务不需要的花色   A2,A3
            var Need = new List<string>();   //保存拣选任务需要 但是 当前工位没有的花色    A4

            foreach (string item in buffer.Keys)
            {
                var rows = dt.Select($"  ProductCode='{item}' ");
                if (rows.Count() == 0)
                {
                    UNeed.Add(item);
                }
            }

            foreach (DataRow it in dt.Rows)
            {
                var productcode = it["ProductCode"].ToString();
                if (!buffer.ContainsKey(productcode))
                {
                    Need.Add(productcode);
                }
            }

            //如果需要新的花色，并且需要新花色的数量大于空闲工位数量，则需要退料
            int N = Need.Count - free.Count;
            if (N == 1)
            {
                //退一垛余料
                if (UNeed.Count >= 1)
                {
                    //这里以后根据实际优化一下，把实际最不常用的花色退掉    这是紧急不重要的
                    var ss = buffer[GetLessProductCode(UNeed)];

                    if (LessReturnBack(ss, sortId) == false) { return false; }

                }
            }
            else if (N == 2)
            {
                //退两垛余料
                if (UNeed.Count >= 2)
                {
                    var ss1 = buffer[UNeed[0]];
                    if (LessReturnBack(ss1, sortId) == false) { return false; }

                    var ss2 = buffer[UNeed[1]];
                    if (LessReturnBack(ss2, sortId) == false) { return false; }
                }
            }
            else if (N == 3)
            {
                //退三垛余料
                if (UNeed.Count == 3)
                {
                    var ss3 = buffer[UNeed[0]];
                    if (LessReturnBack(ss3, sortId) == false) { return false; }

                    var ss4 = buffer[UNeed[1]];
                    if (LessReturnBack(ss4, sortId) == false) { return false; }

                    var ss5 = buffer[UNeed[2]];
                    if (LessReturnBack(ss5, sortId) == false) { return false; }
                }
            }
            else
            {
                //无需退料
                return true;
            }

            return false;
        }

        //根据拣选任务请求要料/补料
        private  bool AutoRequest(DataTable dt, long sortId)
        {
            var buffer_station = GetCurrentBuffFromPLC();  //获取当前有余料的工位  
            var free_station = GetFreeSortStation();  //获取当前没有余料的工位   
            var index = 0;
            if (buffer_station.Count + free_station.Count != 3)
            {
                WCSSql.InsertLog("[AutoRequest]PLC存储的花色和数量信息有误！", "ERROR");
                return false;
            }
            string productcode_need = "";
            int amount_need = 0;
            var wms_rlt = new WMSFeedBack();

            //拣选任务需要的花色
            foreach (DataRow dr in dt.Rows)
            {
                //4.判断是否需要向WMS申请要料 【参数：ReqType=2，ReqId=10002，ProductCode='903S'，NeedCound=20，ToStation=3001】
                productcode_need = dr["ProductCode"].ToString();
                amount_need = Convert.ToInt32(dr["Amount"]);
                if (buffer_station.ContainsKey(productcode_need))
                {
                    var diff = buffer_station[productcode_need].Amount - 1 - amount_need;

                    if (diff < 0)
                    {
                        //把工位与任务号绑定
                        if (WCSSql.GetSortStationTaskId(buffer_station[productcode_need].Code) == sortId) { continue; }

                        // 要料-补料：要补到对应的花色的工位
                        var req = new RequestInfo()
                        {
                            ReqType = 2,
                            ProductCode = productcode_need,
                            Amount = diff * -1,
                            ToPosition = buffer_station[productcode_need].Code.ToString()
                        };

                        //向WMS发出要料请求
                        wms_rlt = WCSSql.RequestTask(req);

                        if (wms_rlt.Status == 200)
                        {
                            WCSSql.BindSortIdToStation(req.ToPosition, sortId);
                            WCSSql.InsertLog($"申请拣选要料(补料)成功！请求编号：{wms_rlt.ReqId}，花色：{productcode_need}，需要数量：{req.Amount}，目标位：{req.ToPosition}", "LOG");
                        }
                        else
                        {
                            WCSSql.InsertLog($"申请拣选要料(补料)失败！错误信息：{wms_rlt.Message}", "ERROR");
                            return false;
                        }
                    }
                }
                else
                {
                    //花色没有，申请要料
                    //1.分配一个空的拣选位置
                    if (free_station.Count >= index + 1)
                    {
                        if (WCSSql.GetSortStationTaskId(free_station[index]) == sortId)
                        {
                            //已经分配了，避免工位重复被分配
                            index++;
                            continue;
                        }
                        // 要新的料
                        var req = new RequestInfo()
                        {
                            ReqType = 2,
                            ProductCode = productcode_need,
                            Amount = amount_need,
                            ToPosition = free_station[index].ToString()
                        };

                        index++;

                        //向WMS发出要料请求
                        wms_rlt = WCSSql.RequestTask(req);

                        if (wms_rlt.Status == 200)
                        {
                            WCSSql.BindSortIdToStation(req.ToPosition, sortId);
                            WCSSql.InsertLog($"申请拣选要料成功！请求编号：{wms_rlt.ReqId}，花色：{productcode_need}，需要数量：{req.Amount}，目标位：{req.ToPosition}", "LOG");
                        }
                        else
                        {
                            WCSSql.InsertLog($"申请拣选要料失败！错误信息：{wms_rlt.Message}", "ERROR");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //得到使用率最小的不需要的余料
        private  string GetLessProductCode(List<string> lst)
        {
            if (lst.Count == 1)
            {
                return lst[0];
            }

            return WCSSql.GetLessUseProduct(lst);
        }

        //在每个拣选任务刚开始时，先判断一下能否根据现场余料的情况直接拣选入库的可能性
        private  bool FirstReturn(DataTable dt)
        {
            var dics = GetCurrentBuffFromPLC();
            if (dt.Rows.Count == 1)
            {
                //单花色
                var productcode = dt.Rows[0]["ProductCode"].ToString();
                var amount = Convert.ToInt32(dt.Rows[0]["Amount"]);
                long NewSortTask = Convert.ToInt64(dt.Rows[0]["TaskId"]);
                if (dics.ContainsKey(productcode))
                {
                    if (dics[productcode].Amount - 1 == amount)
                    {
                        //刚好可用余料来作为拣选入库
                        FinishSortedTask(NewSortTask, dics[productcode].Code);
                        return true;
                    }
                }
            }
            return false;
        }

        private  void StartSortedTask()
        {
            //2.按队列顺序找一个【已分配=1】的拣选任务,获取需要的花色，数量信息  903
            var dt = WCSSql.GetNewSortNeedInfo();
            if (dt.Rows.Count == 0)
            {
                //没有要执行的拣选任务
                return;
            }

            long NewSortTask = Convert.ToInt64(dt.Rows[0]["TaskId"]);
            if (dt.Rows.Count > 3)
            {
                //一次拣选任务超过3种花色
                WCSSql.InsertLog($"[StartSortedTask]拣选任务：{NewSortTask}，拣选花色不能超过3种！", "ERROR");
                return;
            }

            //拣选明细数据核查
            var rlt = WCSSql.CheckSortDetail(NewSortTask);
            if (rlt.Length > 0)
            {
                WCSSql.InsertLog($"[StartSortedTask]{rlt}", "ERROR");
                return;
            }

            //在拣选开始时，先判断2001，2002或2004上的余料是否直接满足拣选需求；满足则直接回库，拣选任务完成
            if (FirstReturn(dt))
            {
                return;
            }

            //余料自动回库计算
            if (!AutoReteurn(dt, NewSortTask))
            {
                return;
            }

            //拣选要料/补料自动申请
            if (!AutoRequest(dt, NewSortTask))
            {
                return;
            }

            //5.将拣任务改为【执行状态=20】
            WCSSql.UpdateTaskStatus(NewSortTask, 20, "start");
            WCSSql.InsertLog($"拣选任务：{NewSortTask}开始执行！", "LOG");
        }

        private  void FinishSortedTask(long SortId, int fromStation = 2003)
        {
            var req = new RequestInfo()
            {
                ReqType = 1,
                TaskId = SortId,
                FromPosition = fromStation.ToString()
            };

            //调用WMS接口
            var rlt = WCSSql.RequestTask(req);

            //如果返回成功
            if (rlt.Status == 200)
            {
                //WCSSql.InsertLog($"拣选任务[{taskid}]向WMS申请拣选入库成功[200]！请求编号：{req.ReqId}", "LOG");
                //更新拣选任务为【完成状态=98】
                WCSSql.UpdateTaskStatus(SortId, 98, "finish");
                WCSSql.InsertLog($"拣选任务[{SortId}]已完成！", "LOG");

                //获取WMS产生的入库任务
                var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
                if (task == null)
                {
                    WCSSql.InsertLog($"[FinishSortedTask]拣选入库申请失败！请求编号[{rlt.ReqId}]：{msg}", "ERROR");
                    return;
                }

                //WCS给龙门PLC发出工位清除指令
                //OpcHs.ClearSortStation(fromStation);
                //WCS给线体PLC写入库任务：新的垛号，目标值   入库！！！
                WCSSql.SetSortStationCount(fromStation, 0);
                OpcHsc.WriteDeviceData(fromStation, task.PilerNo, task.target);
                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                WCSSql.InsertLog($"拣选入库开始！任务ID：{task.TaskId}，垛号：{task.PilerNo}，起始位：{fromStation}，目标位：{task.ToPosition}", "LOG");

                //将余料缓存信息同步给WMS
                SetSortedBuffers();
            }
            else
            {
                //记录错误日志，并报警，人工处理
                WCSSql.InsertLog($"拣选完成出错！[FinishSortedTask]任务ID：{SortId}，错误信息：{rlt.Status}{rlt.Message}", "ERROR");
            }
        }
    }

    //#region old code

    //public class Sorting
    //{
    //    private static IContract iContract = Service.GetInstance();

    //    static long sort_taskid = 0; //保存当前拣选任务ID
    //    static string msg = "";
    //    static int[] arr = { 2001, 2002, 2004 };
    //    static int[] arr2 = { 2001, 2002, 2003, 2004, 2005 };
    //    static Dictionary<string, SortStation> buffer = new Dictionary<string, SortStation>(); //只保存有板(>=2)的的拣选位
    //    static Dictionary<string, SortStation> buffer2 = new Dictionary<string, SortStation>(); //只保存有板(>=1)的的拣选位
    //    static List<int> free = new List<int>();  //只保存无板(<=1)的拣选位

    //    //分配指令
    //    public static void DoCmd()
    //    {
    //        try
    //        {
    //            //保险起见，先清除WCS写的抓板数据
    //            //OpcHsc.ClearMainpulatorTask();

    //            //保险起见，先验证2001,2002,2003,2004拣选工位PLC计算的板件数量与WCS计算的板件数量是否一致
    //            if (OpcHsc.RMCanDo(1))
    //            {
    //                var diff = CheckWCSPLCAmount();
    //                if (diff.Length > 0)
    //                {
    //                    WCSSql.InsertLog(diff, "ERROR");
    //                    return;
    //                }
    //            }

    //            //将拣选完后的剩下最后一块空垫板，从2001,2002,2004自动抓到2005
    //            if (PutLastEmpty())
    //            {
    //                return;
    //            }

    //            //获取当前正在拣选的任务
    //            msg = WCSSql.GetCurrentSortTaskId(out sort_taskid);
    //            if (msg.Length > 0)
    //            {
    //                WCSSql.InsertLog(msg, "ERROR");
    //                return;
    //            }

    //            if (sort_taskid == 0)
    //            {
    //                var count2003 = OpcHsc.ReadBoradsCount(2003);
    //                if (count2003 <= 1)
    //                {
    //                    //开始一个新的拣选任务
    //                    StartSortedTask();
    //                }
    //                return;
    //            }

    //            //如果机械手已接收指令，不往下执行
    //            if (!OpcHsc.RMCanDo(1)) { return; }

    //            int plc_amount = OpcHsc.ReadBoradsCount(2003);  //从机械手PLC读取的拣选工位的板件数量
    //            int rlt = WCSSql.CheckSortTaskIsFinished(sort_taskid, plc_amount);
    //            if (rlt == 1)
    //            {
    //                //结束当前执行的拣选任务
    //                FinishSortedTask(sort_taskid);
    //            }
    //            else if (rlt == 0)
    //            {
    //                //拣选未完成
    //                SendCmd(sort_taskid);
    //            }
    //            else if (rlt == 998)
    //            {
    //                //PLC记录的数量与WCS记录的不一致
    //                WCSSql.InsertLog($"[CheckSortTaskIsFinished]拣选任务：{sort_taskid}PLC记录的拣选数量与WCS记录的拣选数量不一致！请核查！", "ERROR");
    //            }
    //            else
    //            {
    //                //异常
    //                WCSSql.InsertLog($"[CheckSortTaskIsFinished]拣选任务：{sort_taskid}执行异常！", "ERROR");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog("机械手拣选错误！[DoCmd]" + ex.Message, "ERROR");
    //        }
    //    }

    //    //抓最后一块空垫板
    //    private static bool PutLastEmpty()
    //    {
    //        //2001,2002,2004
    //        foreach (int no in arr)
    //        {
    //            if (OpcHsc.ReadBoradsCount(no) != 1) { continue; }

    //            //如果2003没有板，直接放板
    //            if (OpcHsc.ReadBoradsCount(2003) == 0)
    //            {
    //                if (OpcHsc.RMCanDo(1))
    //                {
    //                    var rlt2= OpcHsc.WriteToMainpulator(no, 2003);
    //                    if (rlt2)
    //                    {
    //                        WCSSql.InsertLog($"机械手[1]抓取最后一块空垫板，From：{no}，To：2003，花色：空垫板", "LOG");
    //                    }
    //                }

    //                return true;     
    //            }

    //            if (OpcHsc.ReadBoradsCount(2005) >= 42)
    //            {
    //                if (OpcHsc.ReadEmptyBuffersCount() >= 2)
    //                {
    //                    //防止重复向WMS发出申请
    //                    if (OpcHsc.ReadInWareCmd(2005)) { return true; }

    //                    //向WMS请求空垫板入库  
    //                    //给2005工位写清除指令
    //                    //给2005工位写空垫板入库指令,执行入库
    //                    var request = new RequestInfo();
    //                    request.ReqType = 3;
    //                    request.Amount = 42;
    //                    request.FromPosition = "2005";
    //                    var rlt = WCSSql.RequestTask(request);
    //                    if (rlt.Status == 200)
    //                    {
    //                        var msg = "";
    //                        var task = WCSSql.GetTaskByReqId(request.ReqId, ref msg);
    //                        if (task == null)
    //                        {
    //                            WCSSql.InsertLog($"WCS获取任务失败[GetTaskByReqId]，请求ID：{request.ReqId}，{msg}", "ERROR");
    //                        }
    //                        else
    //                        {
    //                            //给线体任务                               
    //                            OpcHsc.WriteDeviceData("GT217", task.PilerNo, task.target);
    //                            WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //                            WCSSql.InsertLog($"空垫板开始入库！垛号：{task.PilerNo}，起始位：{task.FromPosition}，目标位：{task.ToPosition}", "LOG");                            
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (!OpcHsc.ReadGT217GoBackCmd())
    //                    {
    //                        OpcHsc.WriteGT217GoBackCmd();     //退回指令      
    //                    }        
    //                    WCSSql.InsertLog($"正在向PLC请求退回空垫板：GT217-->GT216", "LOG");
    //                    Thread.Sleep(5 * 1000);  //等待5s...
    //                }
    //            }
    //            else
    //            {
    //                if (OpcHsc.RMCanDo(1))
    //                {
    //                    var rlt = OpcHsc.WriteToMainpulator(no, 2005);
    //                    if (rlt)
    //                    {
    //                        WCSSql.InsertLog($"机械手[1]抓取最后一块空垫板，From：{no}，To：2005，花色：空垫板", "LOG");
    //                    }
    //                }
    //            }

    //            return true;

    //        }

    //        return false;
    //    }

    //    //更新余料的数量信息，供WMS获取统计
    //    private static void SetSortedBuffers()
    //    {
    //        var dic = GetCurrentBuffFromPLC();
    //        var sortbuffers = "";
    //        foreach (SortStation ss in dic.Values)
    //        {
    //            sortbuffers += $"{ss.ProductCode}|{ss.Amount};";
    //        }

    //        WCSSql.SetWcsConfig("SortBuffer", sortbuffers);
    //    }

    //    //2001,2002,2003,2004
    //    private static string CheckWCSPLCAmount()
    //    {
    //        if (WCSSql.CheckSortCount(2001, OpcHsc.ReadBoradsCount(2001)) == false)
    //        {
    //            return "拣选工位2001：PLC存储的数量与WCS存储的数量不一致！";
    //        }
    //        if (WCSSql.CheckSortCount(2002, OpcHsc.ReadBoradsCount(2002)) == false)
    //        {
    //            return "拣选工位2002：PLC存储的数量与WCS存储的数量不一致！";
    //        }
    //        if (WCSSql.CheckSortCount(2003, OpcHsc.ReadBoradsCount(2003)) == false)
    //        {
    //            return "拣选工位2003：PLC存储的数量与WCS存储的数量不一致！";
    //        }
    //        if (WCSSql.CheckSortCount(2004, OpcHsc.ReadBoradsCount(2004)) == false)
    //        {
    //            return "拣选工位2004：PLC存储的数量与WCS存储的数量不一致！";
    //        }

    //        return "";
    //    }

    //    //手动回库指令
    //    public static void ReturnByHand(int station)
    //    {
    //        if (!OpcHsc.RMCanDo(1))
    //        {
    //            WCSSql.InsertLog("【ReturnByHand】在机械手空闲的状态下才能做手动回库的操作！", "ERROR");
    //            return;
    //        }

    //        if (station == 2003)
    //        {
                
    //        }
    //        else if (station == 2001 || station == 2002 || station == 2004)
    //        {
    //            //   2.2如果是3001,3002,3004 调用WMS接口【参数：ReqType=4,ReqId=10006,FromStation=3001,ProductColor='903S',Count=12】
    //            var plc_ProductCode =OpcHsc.ReadStaionProductCode(station); //从线体PLC读取到花色信息
    //            var plc_Amount = OpcHsc.ReadBoradsCount(station);  //从机械手PLC读取到到数量
    //            var pilerno = OpcHsc.ReadPilerNoByStationNo(station);

    //            if (plc_ProductCode == "")
    //            {
    //                WCSSql.InsertLog($"【ReturnByHand】从站台{station}获取不到花色信息！", "ERROR");
    //                return;
    //            }

    //            if (plc_Amount <= 1)
    //            {
    //                WCSSql.InsertLog($"【ReturnByHand】站台{station}没有余料！", "ERROR");
    //                return;
    //            }

    //            if (pilerno == 0)
    //            {
    //                WCSSql.InsertLog($"【ReturnByHand】从站台{station}获取不到垛号！", "ERROR");
    //                return;
    //            }

    //            var req = new RequestInfo()
    //            {
    //                ReqType = 4,  //余料回库
    //                FromPosition = station.ToString(),
    //                Amount = plc_Amount - 1,
    //                PilerNo = pilerno
    //            };


    //            var wms_rlt = WCSSql.RequestTask(req);

    //            if (wms_rlt.Status == 200)
    //            {
    //                WCSSql.InsertLog($"余料入库[手动]申请成功！请求编号：{wms_rlt.ReqId}，工位：{req.FromPosition}，数量：{req.Amount}", "LOG");

    //                var msg = "";
    //                var task = WCSSql.GetTaskByReqId(wms_rlt.ReqId, ref msg);
    //                if (task == null)
    //                {
    //                    WCSSql.InsertLog($"余料入库[手动]申请失败！错误信息：{msg}", "ERROR");
    //                    //return false;
    //                }

    //                //写任务给线体,清掉板件数量
    //                OpcHsc.WriteDeviceData(station, task.PilerNo, task.target);
    //                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //                WCSSql.InsertLog($"余料入库开始[手动]！垛号：{task.PilerNo}，起始位：{station}，目标位：{task.ToPosition}，花色：{task.ProductCode}", "LOG");
    //            }
    //            else
    //            {
    //                WCSSql.InsertLog($"余料入库[手动]申请失败！错误信息：{wms_rlt.Message}", "ERROR");
    //            }
    //        }
    //        else if (station == 2005)
    //        {
    //            //   2.3如果是2005 调用WMS接口【参数：ReqType=3,ReqId=10007,FromStation=3005,Count=39】
    //            var plc_empty_count = OpcHsc.ReadBoradsCount(2005); //从机械手PLC获取空垫板数量

    //            if (plc_empty_count <= 5)
    //            {
    //                LogHelper.LogErrorInfo($"【ReturnByHand】空垫板数量少于6块不能回！");
    //                return;
    //            }

    //            var req = new RequestInfo()
    //            {
    //                ReqType = 3,
    //                Amount = plc_empty_count,
    //                FromPosition = "2005"
    //            };
    //            //var NewReqId = WCSSql.CreateRequest(req);

    //            //if (NewReqId == 0)
    //            //{
    //            //    LogHelper.LogErrorInfo($"【ReturnByHand】WCS创建空垫板请求执行异常！");
    //            //    return;
    //            //}

    //            //调用WMS接口

    //            //给机械手PLC写清除指令
    //            //给线体PLC写入库任务，入库！！！
    //        }
    //    }
        
    //    //开始拣选时，如果2003工位没有空垫板， 则 A：从2001，2002或2004上抓上保护板； B：从2005抓空垫板
    //    private static void PutFirstEmptyTo2003(long sortid)
    //    {
    //        if (!OpcHsc.RMCanDo(1)) return;

    //        var lst = GetCurrentBuffFromPLC();
    //        var ret = false;

    //        //A计划
    //        foreach (var item in lst)
    //        {
    //            if (item.Value.Amount == 42)
    //            {
    //                ret = OpcHsc.WriteToMainpulator(item.Value.Code, 2003);
    //                if (ret)
    //                {
    //                    WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务：{sortid}，FromStation：{item.Value.Code}，ToStation：2003", "LOG");
    //                    return;
    //                }
    //            }
    //        }

    //        //B计划
    //        var count2005= OpcHsc.ReadBoradsCount(2005);
    //        if (count2005 == 0)
    //        {
    //            if (!OpcHsc.ReadGT216GoHeadCmd() && OpcHsc.ReadBoradsCount(2006) > 0)
    //            {
    //                OpcHsc.WriteGT216GoBackCmd();                 
    //            }
    //            WCSSql.InsertLog($"拣选任务：{sortid} 正在向PLC请求前进空垫板：GT216-->GT217", "LOG");
    //            Thread.Sleep(10 * 1000);  //等待5s...
    //        }
    //        else
    //        {
    //            ret = OpcHsc.WriteToMainpulator(2005, 2003);
    //            if (ret)
    //            {
    //                WCSSql.InsertLog($" 拣选任务放置第一块空垫板：{sortid}，FromStation：{2005}，ToStation：2003", "LOG");
    //                return;
    //            }
    //        }

    //        if (ret == false)
    //        {
    //            WCSSql.InsertLog(" 机械手拣选放置第一块空垫板任务失败！[PutFirstEmptyTo2003]", "ERROR");
    //        }
    //    }

    //    //执行抓板命令
    //    private static void SendCmd(long SortId)
    //    {
    //        int count2003 = OpcHsc.ReadBoradsCount(2003); //从机械手PLC获取当前拣选工位的板件数量

    //        //拣选时，先放置第一块空垫板
    //        if (count2003 == 0)
    //        {
    //            PutFirstEmptyTo2003(SortId);
    //            return;
    //        }
            
    //        var Next_Productcode = WCSSql.GetNextProductCode(SortId, count2003);

    //        if (Next_Productcode == "")
    //        {
    //            WCSSql.InsertLog($"[SendCmd]拣选任务：[{SortId}]获取第{count2003}块板花色错误！", "ERROR");
    //            return;
    //        }

    //        //4.从电气PLC获取2001，2002，2004花色，从龙门PLC获取2001，2002，2004剩余的数量
    //        var dicbuffer = GetCurrentBuffFromPLC();
    //        var rt = false;
    //        var stationNo = 0;
    //        //5.如果有可以抓取的花色，则写命令给龙门PLC抓板
    //        if (dicbuffer.ContainsKey(Next_Productcode))
    //        {
    //            stationNo = dicbuffer[Next_Productcode].Code;

    //            //有上保护板，先去掉上保护板
    //            if (dicbuffer[Next_Productcode].Amount == 42)
    //            {
    //                if (OpcHsc.ReadBoradsCount(2005) >= 42)
    //                {
    //                    var pilerscount = OpcHsc.ReadEmptyBuffersCount();  //获取缓存的空垫板垛数
    //                    if (pilerscount >= 2)
    //                    {
    //                        //避免重复申请
    //                        if (OpcHsc.ReadInWareCmd(2005)) { return; }
    //                        //向WMS请求空垫板入库  
    //                        //给2005工位写清除指令
    //                        //给2005工位写空垫板入库指令,执行入库
    //                        //空垫板入库申请，满垛42块板
    //                        var request = new RequestInfo();
    //                        request.ReqType = 3;
    //                        request.Amount = 42;
    //                        request.FromPosition = "2005";
    //                        var rlt = WCSSql.RequestTask(request);

    //                        if (rlt.Status == 200)
    //                        {
    //                            //WMS反馈成功
    //                            var msg = "";
    //                            var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
    //                            if (task == null)
    //                            {
    //                                WCSSql.InsertLog($"空垫板入库申请失败！{msg}", "ERROR");
    //                                return;
    //                            }

    //                            WCSSql.InsertLog($"WCS任务申请成功！请求编号：{rlt.ReqId}，类型：空垫板入库，垛号：{task.PilerNo}，起始位：2005，目标位：{task.ToPosition}", "LOG");
    //                            OpcHsc.WriteDeviceData(2005, task.PilerNo, task.target);
    //                            WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //                            WCSSql.InsertLog($"空垫板入库开始！垛号：{task.PilerNo}，起始位：2005，目标位：{task.ToPosition}", "LOG");
    //                        }
    //                        else
    //                        {
    //                            WCSSql.InsertLog(rlt.Message, "ERROR");
    //                            return;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        //给PLC写退回指令 Action=1  GT217-->GT216
    //                        if (!OpcHsc.ReadGT217GoBackCmd())
    //                        {
    //                            OpcHsc.WriteGT217GoBackCmd();
    //                        }

    //                        WCSSql.InsertLog($"拣选任务：{SortId} 正在向PLC请求退回空垫板：GT217-->GT216", "LOG");
    //                        Thread.Sleep(10 * 1000);  //等待5s...
    //                    }
    //                    return;
    //                }

    //                //给机械手写抓取空垫板指令 FromStation：dicbuffer[Next_Productcode].Code,,ToStaion:3005
    //                if (OpcHsc.RMCanDo(1))
    //                {
    //                    rt = OpcHsc.WriteToMainpulator(stationNo, 2005);
    //                    if (rt)
    //                    {
    //                        //OpcHs.SetHaveUpProduct(stationNo, 0);
    //                        WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务：{SortId}，FromStation：{stationNo}，ToStation：2005", "LOG");
    //                    }
    //                    else
    //                    {
    //                        WCSSql.InsertLog($"机械手[1]执行摘除上保护板！ 拣选任务失败：{SortId}，FromStation：{stationNo}，ToStation：2005", "ERROR");
    //                    }
    //                }

    //                return;
    //            }

    //            if (dicbuffer[Next_Productcode].Amount >= 2 && OpcHsc.ReadBoradsCount(2003) > 0)
    //            {
    //                //给机械手写抓取指令  FromStation: lst1[Next_Productcode]      ToStation:2003
    //                if (OpcHsc.RMCanDo(1))
    //                {
    //                    rt = OpcHsc.WriteToMainpulator(stationNo, 2003);
    //                    if (rt)
    //                    {
    //                        WCSSql.InsertLog($"机械手[1]抓板，拣选任务：{SortId}，StackIndex：{count2003}，花色：{Next_Productcode}，From：{stationNo}，To：2003 ！", "LOG");
    //                        WCSSql.SetSortInfoStatus(SortId, count2003); //更新拣选明细
    //                    }
    //                    else
    //                    {
    //                        WCSSql.InsertLog($"机械手[1]抓板失败！", "ERROR");
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //可能需要的花色还在来的线体上，等待10秒钟
    //            WCSSql.InsertLog($"机械手[1]拣选等待原料！拣选任务：{SortId}，StackIndex：{count2003}，花色：{Next_Productcode}，From：未知，To：2003  没有花色，静等20s...", "LOG");
    //            Thread.Sleep(1000 * 20);
    //        }
    //    }

    //    //从PLC获取2001,2002,2004工位当前暂存的花色，数量信息
    //    public static Dictionary<string, SortStation> GetCurrentBuffFromPLC()
    //    {
    //        buffer.Clear();

    //        foreach (int no in arr)
    //        {
    //            var count = OpcHsc.ReadBoradsCount(no);
    //            var pcode = "";
    //            if (count >= 2)
    //            {
    //                pcode = OpcHsc.ReadStaionProductCode(no);
    //                buffer.Add(pcode,
    //                       new SortStation()
    //                       {
    //                           Amount = count,
    //                           Code = no,
    //                           ProductCode = pcode
    //                           //HaveUpProtect = OpcHs.IsHaveUpProduct(no)
    //                       });
    //            }
    //        }

    //        return buffer;
    //    }

    //    public static Dictionary<string, SortStation> GetCurrentBuffFromPLC2()
    //    {
    //        buffer2.Clear();

    //        foreach (int no in arr2)
    //        {
    //            var count = OpcHsc.ReadBoradsCount(no);
    //            var code = "";
    //            if (count >= 1)
    //            {
    //                code = OpcHsc.ReadStaionProductCode(no);
    //                buffer2.Add(code,
    //                       new SortStation()
    //                       {
    //                           Amount = count,
    //                           Code = no,
    //                           ProductCode = code
    //                           //HaveUpProtect = OpcHs.IsHaveUpProduct(no)
    //                       });
    //            }
    //        }

    //        return buffer2;
    //    }

    //    //从PLC获取2001,2002,2004空闲的工位
    //    private static List<int> GetFreeSortStation()
    //    {
    //        free.Clear();
    //        foreach (int no in arr)
    //        {
    //            var count = OpcHsc.ReadBoradsCount(no);
    //            if (count <= 1)
    //            {
    //                free.Add(no);
    //            }
    //        }

    //        return free;
    //    }

    //    //余料回库申请
    //    private static bool LessReturnBack(SortStation ss,long sortId)
    //    {
    //        //该工位的余料回库已经申请过了
    //        //if (WCSSql.GetSortStationTaskId2(ss.Code) == sortId) { return true; }
    //        if (OpcHsc.ReadInWareCmd(ss.Code)) { return true; }

    //        //余料回库请求
    //        var req = new RequestInfo();
    //        req.ReqType = 4;
    //        req.Amount = ss.Amount;
    //        req.FromPosition = ss.Code.ToString();
    //        var pilerNo = OpcHsc.ReadPilerNoByStationNo(ss.Code);
    //        req.PilerNo = pilerNo;

    //        var wms_rlt = WCSSql.RequestTask(req);

    //        if (wms_rlt.Status == 200)
    //        {
    //            WCSSql.InsertLog($"余料自动入库申请成功！请求编号：{wms_rlt.ReqId}，垛号：{pilerNo}，工位：{req.FromPosition}，数量：{req.Amount}", "LOG");

    //            var msg = "";
    //            var task = WCSSql.GetTaskByReqId(wms_rlt.ReqId, ref msg);
    //            if (task == null)
    //            {
    //                WCSSql.InsertLog($"余料自动入库申请失败！请求编号：{wms_rlt.ReqId}，垛号：{pilerNo}，工位：{req.FromPosition}，错误信息：{msg}", "ERROR");
    //                return false;
    //            }

    //            OpcHsc.WriteDeviceData(ss.Code, task.PilerNo, task.target);
    //            WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //            WCSSql.InsertLog($"余料自动入库开始！垛号：{task.PilerNo}，起始位：{ss.Code}，目标位：{task.ToPosition}，花色：{task.ProductCode}", "LOG");
    //            return true;
    //        }
    //        else
    //        {
    //            WCSSql.InsertLog($"余料自动入库申请失败[999]！工位：{req.FromPosition}，错误信息：{wms_rlt.Message}", "ERROR");
    //            return false;
    //        }
    //    }

    //    //余料自动回库计算
    //    private static bool AutoReteurn(DataTable dt, long sortId)
    //    {
    //        var buffer = GetCurrentBuffFromPLC();  //当前存储的花色信息  2001,2002,2004
    //        var free = GetFreeSortStation();  //当前空闲的拣选工位   2001,2002,2004

    //        if (buffer.Count + free.Count != 3)
    //        {
    //            WCSSql.InsertLog("[AutoReteurn]PLC存储的花色和数量信息有误！", "LOG");
    //            return false;
    //        }

    //        if (free.Count >= dt.Rows.Count) { return true; }

    //        var UNeed = new List<string>();  //保存当前工位有 但是 拣选任务不需要的花色   A2,A3
    //        var Need = new List<string>();   //保存拣选任务需要 但是 当前工位没有的花色    A4

    //        foreach (string item in buffer.Keys)
    //        {
    //            var rows = dt.Select($"  ProductCode='{item}' ");
    //            if (rows.Count() == 0)
    //            {
    //                UNeed.Add(item);
    //            }
    //        }

    //        foreach (DataRow it in dt.Rows)
    //        {
    //            var productcode = it["ProductCode"].ToString();
    //            if (!buffer.ContainsKey(productcode))
    //            {
    //                Need.Add(productcode);
    //            }
    //        }

    //        //如果需要新的花色，并且需要新花色的数量大于空闲工位数量，则需要退料
    //        int N = Need.Count - free.Count;
    //        if (N == 1)
    //        {
    //            //退一垛余料
    //            if (UNeed.Count >= 1)
    //            {
    //                //这里以后根据实际优化一下，把实际最不常用的花色退掉    这是紧急不重要的
    //                var ss = buffer[GetLessProductCode(UNeed)];

    //                if (LessReturnBack(ss, sortId) == false) { return false; }

    //            }
    //        }
    //        else if (N == 2)
    //        {
    //            //退两垛余料
    //            if (UNeed.Count >= 2)
    //            {
    //                var ss1 = buffer[UNeed[0]];
    //                if (LessReturnBack(ss1, sortId) == false) { return false; }

    //                var ss2 = buffer[UNeed[1]];
    //                if (LessReturnBack(ss2, sortId) == false) { return false; }
    //            }
    //        }
    //        else if (N == 3)
    //        {
    //            //退三垛余料
    //            if (UNeed.Count == 3)
    //            {
    //                var ss3 = buffer[UNeed[0]];
    //                if (LessReturnBack(ss3, sortId) == false) { return false; }

    //                var ss4 = buffer[UNeed[1]];
    //                if (LessReturnBack(ss4, sortId) == false) { return false; }

    //                var ss5 = buffer[UNeed[2]];
    //                if (LessReturnBack(ss5, sortId) == false) { return false; }
    //            }
    //        }
    //        else
    //        {
    //            //无需退料
    //            return true;
    //        }

    //        return false;
    //    }

    //    //根据拣选任务请求要料/补料
    //    private static bool AutoRequest(DataTable dt, long sortId)
    //    {
    //        var buffer_station = GetCurrentBuffFromPLC();  //获取当前有余料的工位  
    //        var free_station = GetFreeSortStation();  //获取当前没有余料的工位   
    //        var index = 0;
    //        if (buffer_station.Count + free_station.Count != 3)
    //        {
    //            WCSSql.InsertLog("[AutoRequest]PLC存储的花色和数量信息有误！", "ERROR");
    //            return false;
    //        }
    //        string productcode_need = "";
    //        int amount_need = 0;
    //        var wms_rlt = new WMSFeedBack();

    //        //拣选任务需要的花色
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            //4.判断是否需要向WMS申请要料 【参数：ReqType=2，ReqId=10002，ProductCode='903S'，NeedCound=20，ToStation=3001】
    //            productcode_need = dr["ProductCode"].ToString();
    //            amount_need = Convert.ToInt32(dr["Amount"]);
    //            if (buffer_station.ContainsKey(productcode_need))
    //            {
    //                var diff = buffer_station[productcode_need].Amount - 1 - amount_need;

    //                if (diff < 0)
    //                {
    //                    //把工位与任务号绑定
    //                    if (WCSSql.GetSortStationTaskId(buffer_station[productcode_need].Code) == sortId) { continue; }

    //                    // 要料-补料：要补到对应的花色的工位
    //                    var req = new RequestInfo()
    //                    {
    //                        ReqType = 2,
    //                        ProductCode = productcode_need,
    //                        Amount = diff * -1,
    //                        ToPosition = buffer_station[productcode_need].Code.ToString()
    //                    };

    //                    //向WMS发出要料请求
    //                    wms_rlt = WCSSql.RequestTask(req);

    //                    if (wms_rlt.Status == 200)
    //                    {
    //                        WCSSql.BindSortIdToStation(req.ToPosition, sortId);
    //                        WCSSql.InsertLog($"申请拣选要料(补料)成功！请求编号：{wms_rlt.ReqId}，花色：{productcode_need}，需要数量：{req.Amount}，目标位：{req.ToPosition}", "LOG");
    //                    }
    //                    else
    //                    {
    //                        WCSSql.InsertLog($"申请拣选要料(补料)失败！错误信息：{wms_rlt.Message}", "ERROR");
    //                        return false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                //花色没有，申请要料
    //                //1.分配一个空的拣选位置
    //                if (free_station.Count >= index + 1)
    //                {
    //                    if (WCSSql.GetSortStationTaskId(free_station[index]) == sortId)
    //                    {
    //                        //已经分配了，避免工位重复被分配
    //                        index++;
    //                        continue;
    //                    }
    //                    // 要新的料
    //                    var req = new RequestInfo()
    //                    {
    //                        ReqType = 2,
    //                        ProductCode = productcode_need,
    //                        Amount = amount_need,
    //                        ToPosition = free_station[index].ToString()
    //                    };

    //                    index++;

    //                    //向WMS发出要料请求
    //                    wms_rlt = WCSSql.RequestTask(req);

    //                    if (wms_rlt.Status == 200)
    //                    {
    //                        WCSSql.BindSortIdToStation(req.ToPosition, sortId);
    //                        WCSSql.InsertLog($"申请拣选要料成功！请求编号：{wms_rlt.ReqId}，花色：{productcode_need}，需要数量：{req.Amount}，目标位：{req.ToPosition}", "LOG");
    //                    }
    //                    else
    //                    {
    //                        WCSSql.InsertLog($"申请拣选要料失败！错误信息：{wms_rlt.Message}", "ERROR");
    //                        return false;
    //                    }
    //                }
    //            }
    //        }
    //        return true;
    //    }

    //    //得到使用率最小的不需要的余料
    //    private static string GetLessProductCode(List<string> lst)
    //    {
    //        if (lst.Count == 1)
    //        {
    //            return lst[0];
    //        }

    //        return WCSSql.GetLessUseProduct(lst);
    //    }

    //    //在每个拣选任务刚开始时，先判断一下能否根据现场余料的情况直接拣选入库的可能性
    //    private static bool FirstReturn(DataTable dt)
    //    {
    //        var dics = GetCurrentBuffFromPLC();
    //        if (dt.Rows.Count == 1)
    //        {
    //            //单花色
    //            var productcode = dt.Rows[0]["ProductCode"].ToString();
    //            var amount = Convert.ToInt32(dt.Rows[0]["Amount"]);
    //            long NewSortTask = Convert.ToInt64(dt.Rows[0]["TaskId"]);
    //            if (dics.ContainsKey(productcode))
    //            {
    //                if (dics[productcode].Amount - 1 == amount)
    //                {
    //                    //刚好可用余料来作为拣选入库
    //                    FinishSortedTask(NewSortTask, dics[productcode].Code);
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }

    //    private static void StartSortedTask()
    //    {
    //        //2.按队列顺序找一个【已分配=1】的拣选任务,获取需要的花色，数量信息  903
    //        var dt = WCSSql.GetNewSortNeedInfo();
    //        if (dt.Rows.Count == 0)
    //        {
    //            //没有要执行的拣选任务
    //            return;
    //        }

    //        long NewSortTask = Convert.ToInt64(dt.Rows[0]["TaskId"]);
    //        if (dt.Rows.Count > 3)
    //        {
    //            //一次拣选任务超过3种花色
    //            WCSSql.InsertLog($"[StartSortedTask]拣选任务：{NewSortTask}，拣选花色不能超过3种！", "ERROR");
    //            return;
    //        }

    //        //拣选明细数据核查
    //        var rlt = WCSSql.CheckSortDetail(NewSortTask);
    //        if (rlt.Length > 0)
    //        {
    //            WCSSql.InsertLog($"[StartSortedTask]{rlt}", "ERROR");
    //            return;
    //        }

    //        //在拣选开始时，先判断2001，2002或2004上的余料是否直接满足拣选需求；满足则直接回库，拣选任务完成
    //        if (FirstReturn(dt))
    //        {
    //            return;
    //        }

    //        //余料自动回库计算
    //        if (!AutoReteurn(dt, NewSortTask))
    //        {
    //            return;
    //        }

    //        //拣选要料/补料自动申请
    //        if (!AutoRequest(dt, NewSortTask))
    //        {
    //            return;
    //        }

    //        //5.将拣任务改为【执行状态=20】
    //        WCSSql.UpdateTaskStatus(NewSortTask, 20, "start");
    //        WCSSql.InsertLog($"拣选任务：{NewSortTask}开始执行！", "LOG");
    //    }

    //    private static void FinishSortedTask(long SortId, int fromStation = 2003)
    //    {
    //        var req = new RequestInfo()
    //        {
    //            ReqType = 1,
    //            TaskId = SortId,
    //            FromPosition = fromStation.ToString()
    //        };

    //        //调用WMS接口
    //        var rlt = WCSSql.RequestTask(req);

    //        //如果返回成功
    //        if (rlt.Status == 200)
    //        {
    //            //WCSSql.InsertLog($"拣选任务[{taskid}]向WMS申请拣选入库成功[200]！请求编号：{req.ReqId}", "LOG");
    //            //更新拣选任务为【完成状态=98】
    //            WCSSql.UpdateTaskStatus(SortId, 98, "finish");
    //            WCSSql.InsertLog($"拣选任务[{SortId}]已完成！", "LOG");

    //            //获取WMS产生的入库任务
    //            var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
    //            if (task == null)
    //            {
    //                WCSSql.InsertLog($"[FinishSortedTask]拣选入库申请失败！请求编号[{rlt.ReqId}]：{msg}", "ERROR");
    //                return;
    //            }

    //            //WCS给龙门PLC发出工位清除指令
    //            //OpcHs.ClearSortStation(fromStation);
    //            //WCS给线体PLC写入库任务：新的垛号，目标值   入库！！！
    //            WCSSql.SetSortStationCount(fromStation, 0);
    //            OpcHsc.WriteDeviceData(fromStation, task.PilerNo, task.target);
    //            WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //            WCSSql.InsertLog($"拣选入库开始！任务ID：{task.TaskId}，垛号：{task.PilerNo}，起始位：{fromStation}，目标位：{task.ToPosition}", "LOG");

    //            //将余料缓存信息同步给WMS
    //            SetSortedBuffers();
    //        }
    //        else 
    //        {
    //            //记录错误日志，并报警，人工处理
    //            WCSSql.InsertLog($"拣选完成出错！[FinishSortedTask]任务ID：{SortId}，错误信息：{rlt.Status}{rlt.Message}", "ERROR");
    //        }
    //    }
    //}

    //#endregion

}
