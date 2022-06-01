using System;
using System.Collections.Generic;
using System.Diagnostics;
using NJIS.Common;
using WcsModel;
using WCS.Communications;
using WCS.DataBase;
using WCS.Interfaces;
using WCS.OPC;

namespace WCS
{
    public class ScControl:Singleton<ScControl>
    {
        private static readonly Dictionary<EPiler, IPiler> dictionaryPiler = new Dictionary<EPiler, IPiler>
        {
            {EPiler.First,new PilerCommunication(EPiler.First) },
            {EPiler.Second,new PilerCommunication(EPiler.Second) },
            {EPiler.Third,new PilerCommunication(EPiler.Third) },
            {EPiler.Fourth,new PilerCommunication(EPiler.Fourth) },
        };

        private static readonly List<EPiler> Pilers = new List<EPiler>(){EPiler.First,EPiler.Second,EPiler.Third, EPiler.Fourth };

        private InStockStationCommunication _inStockStationCommunication = null;
        /// <summary>
        /// 入库站台交互
        /// </summary>
        private InStockStationCommunication InStockStationCommunication =>
            _inStockStationCommunication ?? (_inStockStationCommunication = InStockStationCommunication.GetInstance());

        private OutStockStationCommunication _outStockStationCommunication = null;
        /// <summary>
        /// 出库站台交互
        /// </summary>
        private OutStockStationCommunication OutStockStationCommunication =>
            _outStockStationCommunication ??
            (_outStockStationCommunication = OutStockStationCommunication.GetInstance());

        private ScControl() { }

        #region 堆垛机完成
        public void DdjFinishAll()
        {
            Pilers.ForEach(piler=> DdjFinish(piler));
        }

         void DdjFinish(EPiler piler)
        {
            try
            {
                var iPiler = dictionaryPiler[piler];
                //Step1: 堆垛机翻出任务完成信号
                //堆垛机是否给出完成信号
                if (!iPiler.IsFinished) return;

                //Step2: 获取垛号，找到对应的任务
                //读取垛号
                var pilerNo = int.Parse(iPiler.PilerStackName);

                //从任务列表中获取到对应的任务
                var taskInfo = WCSSql.GetTaskByPilerNo(pilerNo);

                if (taskInfo == null)
                {
                    WCSSql.InsertLog($"堆垛机[{(int)piler}]DDJFinish，无法获从任务列表中取到任务，垛号：[{pilerNo}]", "ERROR", pilerNo);
                    //OpcSc.ClearTaskFinished(ddjNo);
                    iPiler.ClearTaskFinished();
                    return;
                }

                if (taskInfo.TaskType == 1)
                {
                    //更新为完成状态
                    //入库
                    WCSSql.InsertLog($"堆垛机[{(int)piler}]：入库完成，垛号：[{pilerNo}]", "LOG", pilerNo);
                    WCSSql.UpdateTaskStatus(taskInfo.TaskId, 98, "finish");
                }
                else if (taskInfo.TaskType == 2)
                {
                    //出库                  
                    //给线体写任务数据
                    //OpcHsc.WriteSCStationOutTask(ddjNo, pilerNo, taskInfo.target); //*******如果线体连接中断，抛出异常*****
                    //OutStockStationCommunication.WriteTask(piler, pilerNo, taskInfo);
                    WCSSql.UpdateTaskStatus(taskInfo.TaskId, 31, "ddj");
                    WCSSql.InsertLog($"堆垛机[{(int)piler}]：出库完成，垛号：[{pilerNo}]", "LOG", pilerNo);
                }
                else
                {
                    //其它
                    WCSSql.InsertLog($"堆垛机[{(int)piler}]：任务完成，垛号：[{pilerNo}]", "LOG", pilerNo);
                }

                //4.清除堆垛机任务状态
                //给堆垛机反馈
                iPiler.ClearTaskFinished();
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog($"堆垛机[{(int)piler}]：完成任务失败：{ex.Message}", "ERROR", (int)piler);
            }
        }
        #endregion

        #region 堆垛机出库
        public  void DdjOutAll()
        {
            foreach (var piler in Pilers)
            {
                try
                {
                    var iPiler = dictionaryPiler[piler];

                    //Strp1:堆垛机在自动，空闲，激活的状态下，才能接收任务
                    if (!iPiler.IsFree)
                        continue;

                    //St2p2:堆垛机出库站台是否有板
                    if (!OutStockStationCommunication.OutStationIsFree(iPiler.Piler))
                    {
                        DdjOut(piler);
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog($"堆垛机[{ (int)piler}]出库失败：{ ex.Message}", "ERROR");
                    continue;
                }
            }
        }

         void DdjOut(EPiler piler)
        {
            var iPiler = dictionaryPiler[piler];
            //Step3:从数据库中获取一条可以出库的任务
            var task = WCSSql.Demo_GetOutTask((int)piler);

            if (task == null) { return; }

            //Step4:写任务给堆垛机
            //if (iPiler.WritePilerOutStockTask(task))
            //{
            //    WCSSql.InsertLog($"堆垛机[{(int)piler}]：出库开始，垛号：[{task.PilerNo}]", "LOG", task.PilerNo);
            //    WCSSql.UpdateTaskStatus(task.TaskId, 30, "start");
            //}
            //else
            //{
            //    WCSSql.InsertLog($"堆垛机[{(int)piler}]：出库接收失败，垛号：[{task.PilerNo}]", "ERROR", task.PilerNo);
            //}
        }
        #endregion

        #region 堆垛机入库
        public  void DdjAllIn()
        {
            foreach (var piler in Pilers)
            {

                var iPiler = dictionaryPiler[piler];
                try
                {
                    if (iPiler.IsFree)
                    {
                        DdjIn(piler);
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog($"堆垛机[{(int)piler}]入库错误：{ex.Message}", "ERROR");
                    continue;
                }
            }
        }

        //入库
         void DdjIn(EPiler piler)
        {
            var iPiler = dictionaryPiler[piler];
            //Step1: 读取电气是否有入库请求
            //是否有入库请求
            if (!InStockStationCommunication.IsRequestInStock(piler)) { return; }


            //Step2: 如果有，先读取垛号
            //读取垛号
            var pilerNo = InStockStationCommunication.StackNo(piler);

            if (pilerNo == 0)
            {
                WCSSql.InsertLog($"堆垛机[{(int)piler}]DdjIn，无法从入库站台获取到垛号！", "ERROR");
                return;
            }

            //Step3: 根据垛号在数据库中找到对应的任务
            //根据垛号查找任务
            var taskInfo = WCSSql.GetTaskByPilerNo(pilerNo, 1);

            if (taskInfo == null)
            {
                WCSSql.InsertLog($"堆垛机[{(int)piler}]DdjIn，无法从任务列表中获取到入库任务，垛号：[{pilerNo}]", "ERROR", pilerNo);
                return;
            }

            //Step4:把任务写给堆垛机
            //给堆垛机写任务
            //if (iPiler.WritePilerInStockTask(taskInfo))
            //{
            //    WCSSql.InsertLog($"堆垛机[{(int)piler}]：入库开始，垛号：[{taskInfo.PilerNo}]", "LOG", taskInfo.PilerNo);
            //    WCSSql.UpdateTaskStatus(taskInfo.TaskId, 21, "ddj");  //更改任务状态
            //}
            //else
            //{
            //    WCSSql.InsertLog($"堆垛机[{(int)piler}]：入库接收失败，垛号：[{taskInfo.PilerNo}]", "ERROR", taskInfo.PilerNo);
            //}
        }
        #endregion

        #region old code

        //#region 堆垛机完成
        //public static void DDJFinishAll()
        //{

        //    for (int i =1; i <= 4; i++)
        //    {
        //        if (!AppCommon.IsDdjRun(i))
        //            continue;
        //          DDJFinsh(i);
        //    }
        //}

        //static void DDJFinsh(int ddjNo)
        //{
        //    try
        //    {
        //        //Step1: 堆垛机翻出任务完成信号
        //        //堆垛机是否给出完成信号
        //        if (!OpcSc.IsFinished(ddjNo)) return;

        //        //Step2: 获取垛号，找到对应的任务
        //        //读取垛号
        //        var pilerNo = int.Parse(OpcSc.RTask(ddjNo));

        //        //从任务列表中获取到对应的任务
        //        var taskinfo = WCSSql.GetTaskByPilerNo(pilerNo);

        //        if (taskinfo == null)
        //        {
        //            WCSSql.InsertLog($"堆垛机[{ddjNo}]DDJFinsh，无法获从任务列表中取到任务，垛号：[{pilerNo}]", "ERROR", pilerNo);
        //            OpcSc.ClearTaskFinished(ddjNo);
        //            return;
        //        }

        //        if (taskinfo.TaskType == 1)
        //        {
        //            //更新为完成状态
        //            //入库
        //            WCSSql.InsertLog($"堆垛机[{ddjNo}]：入库完成，垛号：[{pilerNo}]", "LOG", pilerNo);
        //            WCSSql.UpdateTaskStatus(taskinfo.TaskId, 98, "finish");

        //            //--------------------------------------------------------------------------------------------------------------
        //            //WCSSql.CreateDemoTask2(ddjNo, taskinfo.ToPosition);  //测试代码。创建出库任务
        //            //--------------------------------------------------------------------------------------------------------------
        //        }
        //        else if (taskinfo.TaskType == 2)
        //        {
        //            //出库                  
        //            //给线体写任务数据
        //            OpcHsc.WriteSCStationOutTask(ddjNo, pilerNo, taskinfo.target); //*******如果线体连接中断，抛出异常*****
        //            WCSSql.UpdateTaskStatus(taskinfo.TaskId, 31, "ddj");
        //            WCSSql.InsertLog($"堆垛机[{ddjNo}]：出库完成，垛号：[{pilerNo}]", "LOG", pilerNo);

        //            //--------------------------------------------------------------------------------------------------------------
        //            //var to = TestDdj(ddjNo, pilerNo, taskinfo.FromRow); //测试代码
        //            //OpcHs.WriteStationOutTask(ddjNo, pilerNo, to);//测试代码
        //            //if (to == -1)
        //            //{
        //            //    WCSSql.InsertLog($"堆垛机[{ddjNo}]：入库循环测试结束！", "LOG", pilerNo);
        //            //    return;
        //            //}
        //            //--------------------------------------------------------------------------------------------------------------
        //        }
        //        else
        //        {
        //            //其它
        //            WCSSql.InsertLog($"堆垛机[{ddjNo}]：任务完成，垛号：[{pilerNo}]", "LOG", pilerNo);
        //        }

        //        //4.清除堆垛机任务状态
        //        //给堆垛机反馈
        //        OpcSc.ClearTaskFinished(ddjNo);
        //    }
        //    catch(Exception ex)
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddjNo}]：完成任务失败：{ex.Message}", "ERROR", ddjNo);
        //    }
        //}

        ////循环测试堆垛机
        //static int TestDdj(int scno, int pilerNo, int row)
        //{
        //    //创建一条入库任务
        //    var task = WCSSql.Demo_GetLastTask(scno, row);
        //    int r = 0, c = 0, l = 0;
        //    if (task.ToPosition.Equals($"{scno * 2}.21.11") || task.ToPosition.Equals($"{scno * 2 - 1}.21.11"))
        //    {
        //        return -1;
        //    }

        //    if (task.ToLayer < 3)
        //    {
        //        l = task.ToLayer + 1;
        //        c = task.ToColumn;
        //        r = task.ToRow;
        //    }
        //    else
        //    {
        //        if (task.ToColumn < 21)
        //        {
        //            l = 1;
        //            c = task.ToColumn + 1;
        //            r = task.ToRow;
        //            if (c == 4 || c == 10 || c == 16)
        //            {
        //                c++;
        //            }
        //        }
        //        //else
        //        //{
        //        //    if (task.ToRow < scno * 2)
        //        //    {
        //        //        c = 1;
        //        //        l = 1;
        //        //        r = task.ToRow + 1;
        //        //    }
        //        //}
        //    }

        //    WCSSql.CreateDemoTask(scno, $"{r}.{c}.{l}", pilerNo);

        //    return 105 - scno;
        //}
        //#endregion

        //#region 堆垛机出库
        //public static void DdjOutAll()
        //{
        //    for (int ddj = 1; ddj <= 4; ddj++)
        //    {
        //        try
        //        {
        //            if (!AppCommon.IsDdjRun(ddj))
        //                continue;

        //            //Strp1:堆垛机在自动，空闲，激活的状态下，才能接收任务
        //            if (!OpcSc.CanRun(ddj))
        //                continue;

        //            //St2p2:堆垛机出库站台是否有板
        //            if (!OpcHsc.IsOutStationFree(ddj))
        //            {
        //                DdjOut(ddj);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WCSSql.InsertLog($"堆垛机[{ ddj}]出库失败：{ ex.Message}", "ERROR");
        //            continue;
        //        }
        //    }
        //}

        //static void DdjOut(int ddjNo)
        //{
        //    //Step3:从数据库中获取一条可以出库的任务
        //    var task = WCSSql.Demo_GetOutTask(ddjNo);

        //    if (task == null) { return; }

        //    //Step4:写任务给堆垛机
        //    if (OpcSc.WTaskOut(task, ddjNo))
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddjNo}]：出库开始，垛号：[{task.PilerNo}]", "LOG", task.PilerNo);
        //        WCSSql.UpdateTaskStatus(task.TaskId, 30, "start");
        //    }
        //    else
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddjNo}]：出库接收失败，垛号：[{task.PilerNo}]", "ERROR", task.PilerNo);
        //    }       
        //}
        //#endregion

        //#region 堆垛机入库
        //public static void DdjAllIn()
        //{
        //    for (int i = 1; i <= 4; i++)
        //    {
        //        if (!AppCommon.IsDdjRun(i))
        //            continue;
        //        try
        //        {
        //            if (OpcSc.CanRun(i))
        //            {
        //                DdjIn(i);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WCSSql.InsertLog($"堆垛机[{i}]入库错误：{ex.Message}", "ERROR");
        //            continue;
        //        }
        //    }
        //}

        ////入库
        //static void DdjIn(int ddj)
        //{
        //    //Step1: 读取电气是否有入库请求
        //    //是否有入库请求
        //    if (!OpcHsc.IsStationInRequest(ddj)) { return; }


        //    //Step2: 如果有，先读取垛号
        //    //读取垛号
        //    var pilerNo = OpcHsc.ReadStationPiler(ddj);

        //    if (pilerNo == 0)
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddj}]DdjIn，无法从入库站台获取到垛号！", "ERROR");
        //        return;
        //    }

        //    //Step3: 根据垛号在数据库中找到对应的任务
        //    //根据垛号查找任务
        //    var taskInfo = WCSSql.GetTaskByPilerNo(pilerNo, 1);

        //    if (taskInfo == null)
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddj}]DdjIn，无法从任务列表中获取到入库任务，垛号：[{pilerNo}]", "ERROR", pilerNo);
        //        return;
        //    }

        //    //Step4:把任务写给堆垛机
        //    //给堆垛机写任务
        //    if (OpcSc.WTaskIn(taskInfo, ddj))
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddj}]：入库开始，垛号：[{taskInfo.PilerNo}]", "LOG", taskInfo.PilerNo);
        //        WCSSql.UpdateTaskStatus(taskInfo.TaskId, 21, "ddj");  //更改任务状态
        //    }
        //    else
        //    {
        //        WCSSql.InsertLog($"堆垛机[{ddj}]：入库接收失败，垛号：[{taskInfo.PilerNo}]", "ERROR", taskInfo.PilerNo);
        //    }           
        //}
        //#endregion

        #endregion


    }
}
