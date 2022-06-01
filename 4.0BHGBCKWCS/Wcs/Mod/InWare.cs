using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NJIS.Common;
using WCS.Common;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS
{
    public class InWare:Singleton<InWare>
    {
        private InWare() { }

        public void AllInWare()
        {
            BCInWare();
            RGVRequest();
        }

        //105入口板材入库
        public void BCInWare()
        {
            try
            {
                //Step1:检测入口有没有入库请求
                if (!OpcHsc.ReadPLCRequest(105)) { return; }

                //OpcHsc.WriteDeviceData("GT102", 999, 1001);
                //return;

                //检查是否有半自动上料到6 7 电子锯
                var manualTask = WCSSql.GetCutting67ManualTask(0,0);
                if (manualTask != null)
                {
                    OpcHsc.WriteDeviceData("GT102", manualTask.PilerNo.Value, Convert.ToInt32(manualTask.ToPosition));
                    WCSSql.UpdateCutting67ManualTask(manualTask.TaskId, 20);
                    return;
                }
                var msg = "";
                //Step2: 从WMS获取到任务
                var task = WCSSql.GetInWareTaskFromWMS(ref msg);
                if (msg.Length > 0)
                {
                    WCSSql.InsertLog($"入库口105：入库失败，{msg}", "ERROR");
                    //退回
                    //OpcHs.InWareWriteBack();
                    return;
                }

                //Step3.把任务数据写给线体，入库 
                OpcHsc.WriteDeviceData("GT102", task.PilerNo, task.target);

                //Step4.日志记录
                WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
                WCSSql.InsertLog($"入库口105：入库成功！垛号：{task.PilerNo}", "LOG");
            }
            catch(Exception ex)
            {
                WCSSql.InsertLog($"入口105[BCInWare]：入库异常！{ex.Message}", "ERROR");
                //退回
                //OpcHs.InWareWriteBack();
            }
        }

        //3011 GT307     ,3012  GT317 RGV任务请求,开料锯空垫板入库
        int[] array = { 3011, 3012 };
        public void RGVRequest()
        {
            try
            {
                foreach (int stationNo in array)
                {
                    //地面线是否有请求
                    if (!OpcHsc.ReadPLCRequest(stationNo)) { continue; }

                    //空垫板入库申请，满垛42块板
                    var request = new RequestInfo();
                    request.ReqType = 3;
                    request.Amount = 42;
                    request.FromPosition = stationNo.ToString();
                    var rlt = WCSSql.RequestTask(request);

                    if (rlt.Status == 200)
                    {
                        //WMS反馈成功
                        var msg = "";
                        var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
                        if (task == null)
                        {
                            WCSSql.InsertLog($"空垫板入库申请失败！{msg}", "ERROR");
                            continue;
                        }

                        WCSSql.InsertLog($"WCS任务申请成功！请求编号：{rlt.ReqId}，类型：空垫板入库，垛号：{task.PilerNo}，起始位：{stationNo}，目标位：{task.ToPosition}", "LOG");
                        //创建RGV任务
                        WCSSql.CreateRGVTask(new RGVTask() { TaskType = 2, FromPosition = stationNo, ToPosition = 3013, PilerNo = task.PilerNo });
                        WCSSql.InsertLog($"创建RGV任务成功！垛号：{task.PilerNo}，起始位：{stationNo}，目标位：3013", "LOG");
                        //清除地面线请求
                        OpcHsc.ClearPLCRequest(stationNo);
                    }
                    else
                    {
                        WCSSql.InsertLog(rlt.Message, "ERROR");
                    }
                }
            }
            catch(Exception ex)
            {
                WCSSql.InsertLog("3011/3012请求入库失败[RGVRequest]：" + ex.Message, "ERROR");
            }
        }
    }

    #region Old Code

    //public class InWare
    //{
    //    public static void AllInWare()
    //    {
    //        BCInWare();
    //        RGVRequest();
    //    }

    //    //105入口板材入库
    //    public static void BCInWare()
    //    {
    //        try
    //        {
    //            //Step1:检测入口有没有入库请求
    //            if (!OpcHsc.ReadPLCRequest(105)) { return; }

    //            //OpcHsc.WriteDeviceData("GT102", 999, 1001);
    //            //return;

    //            var msg = "";
    //            //Step2: 从WMS获取到任务
    //            var task = WCSSql.GetInWareTaskFromWMS(ref msg);
    //            if (msg.Length > 0)
    //            {
    //                WCSSql.InsertLog($"入库口105：入库失败，{msg}", "ERROR");
    //                //退回
    //                //OpcHs.InWareWriteBack();
    //                return;
    //            }

    //            //Step3.把任务数据写给线体，入库 
    //            OpcHsc.WriteDeviceData("GT102", task.PilerNo, task.target);

    //            //Step4.日志记录
    //            WCSSql.UpdateTaskStatus(task.TaskId, 20, "start");
    //            WCSSql.InsertLog($"入库口105：入库成功！垛号：{task.PilerNo}", "LOG");
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog($"入口105[BCInWare]：入库异常！{ex.Message}", "ERROR");
    //            //退回
    //            //OpcHs.InWareWriteBack();
    //        }
    //    }

    //    //3011 GT307     ,3012  GT317 RGV任务请求,开料锯空垫板入库
    //    static int[] array = { 3011, 3012 };
    //    public static void RGVRequest()
    //    {
    //        try
    //        {
    //            foreach (int stationNo in array)
    //            {
    //                //地面线是否有请求
    //                if (!OpcHsc.ReadPLCRequest(stationNo)) { continue; }

    //                //空垫板入库申请，满垛42块板
    //                var request = new RequestInfo();
    //                request.ReqType = 3;
    //                request.Amount = 42;
    //                request.FromPosition = stationNo.ToString();
    //                var rlt = WCSSql.RequestTask(request);

    //                if (rlt.Status == 200)
    //                {
    //                    //WMS反馈成功
    //                    var msg = "";
    //                    var task = WCSSql.GetTaskByReqId(rlt.ReqId, ref msg);
    //                    if (task == null)
    //                    {
    //                        WCSSql.InsertLog($"空垫板入库申请失败！{msg}", "ERROR");
    //                        continue;
    //                    }

    //                    WCSSql.InsertLog($"WCS任务申请成功！请求编号：{rlt.ReqId}，类型：空垫板入库，垛号：{task.PilerNo}，起始位：{stationNo}，目标位：{task.ToPosition}", "LOG");
    //                    //创建RGV任务
    //                    WCSSql.CreateRGVTask(new RGVTask() { TaskType = 2, FromPosition = stationNo, ToPosition = 3013, PilerNo = task.PilerNo });
    //                    WCSSql.InsertLog($"创建RGV任务成功！垛号：{task.PilerNo}，起始位：{stationNo}，目标位：3013", "LOG");
    //                    //清除地面线请求
    //                    OpcHsc.ClearPLCRequest(stationNo);
    //                }
    //                else
    //                {
    //                    WCSSql.InsertLog(rlt.Message, "ERROR");
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog("3011/3012请求入库失败[RGVRequest]：" + ex.Message, "ERROR");
    //        }
    //    }
    //}


    #endregion
}