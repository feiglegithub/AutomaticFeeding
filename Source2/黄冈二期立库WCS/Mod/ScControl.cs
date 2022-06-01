using System;
using System.Collections.Generic;
using WCS.DataBase;
using WCS.model;

namespace WCS
{
    public class ScControl
    {
        static Wcs_DdjInfo sc1 = new Wcs_DdjInfo(1, 1072, 5079, 5036);
        static Wcs_DdjInfo sc2 = new Wcs_DdjInfo(2, 1065, 5087, 5044);
        static Wcs_DdjInfo sc3 = new Wcs_DdjInfo(3, 1058, 5095, 5052);
        static Wcs_DdjInfo sc4 = new Wcs_DdjInfo(4, 1051, 5103, 5060);
        static Wcs_DdjInfo sc5 = new Wcs_DdjInfo(5, 1044, 5111, 5068);
        static Wcs_DdjInfo sc6 = new Wcs_DdjInfo(6, 1037, 5119, 5076);
        static Wcs_DdjInfo sc7 = new Wcs_DdjInfo(7, 1029, 4122, 4037);
        static Wcs_DdjInfo sc8 = new Wcs_DdjInfo(8, 1022, 4131, 4030);
        static Wcs_DdjInfo sc9 = new Wcs_DdjInfo(9, 1015, 4139, 4023);
        static Wcs_DdjInfo sc10 = new Wcs_DdjInfo(10, 1008, 4147, 4016);
        static Wcs_DdjInfo sc11 = new Wcs_DdjInfo(11, 1001, 4155, 4009);

        #region 堆垛机完成
        public static void DDJFinishAll()
        {
            DDJFinsh(sc1);
            DDJFinsh(sc2);
            DDJFinsh(sc3);
            DDJFinsh(sc4);
            DDJFinsh(sc5);
            DDJFinsh(sc6);
            DDJFinsh(sc7);
            DDJFinsh(sc8);
            DDJFinsh(sc9);
            DDJFinsh(sc10);
            DDJFinsh(sc11);
        }

        static void DDJFinsh(Wcs_DdjInfo sc)
        {
            try
            {
                var ddjNo = sc.ScNo;
                //Step1: 堆垛机发任务完成信号
                if (!OPCHelper.IsFinishTask(ddjNo)) return;

                //Step2: 获取托盘号，找到对应的任务
                var seqid = OPCHelper.ReadSCTaskId(ddjNo);

                //从任务列表中获取到对应的任务
                var taskinfo = WcsSqlB.GetTaskInfoBySeqId(seqid);

                if (taskinfo == null)
                {
                    WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机DDJFinsh，无法获从任务列表中取到任务，任务号：[{seqid}]", 2);
                    OPCHelper.WCSFeedBack(ddjNo, 1);
                    return;
                }

                if (taskinfo.NordID == 1 || taskinfo.NordID == 5)
                {
                    //入库
                    WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机：入库完成，托盘号：[{taskinfo.NPalletID}]", 1, taskinfo.NPalletID);
                    WcsSqlB.UpdateTaskStatus(taskinfo.SeqID, 98);
                }
                else if (taskinfo.NordID == 3 || taskinfo.NordID == 6)
                {
                    //出库                  
                    WcsSqlB.UpdateTaskStatus(taskinfo.SeqID, 31);
                    WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机：出库完成，托盘号：[{taskinfo.NPalletID}]", 1, taskinfo.NPalletID);

                    //给线体写任务数据
                    if (taskinfo.NoptStation < 2000)
                    {
                        //一楼出库
                        OPCHelper.WriteStationOutTask(taskinfo, sc.StationOut1No);
                    }
                    else
                    {
                        //二楼出库
                        OPCHelper.WriteStationOutTask(taskinfo, sc.StationOut2No);
                    }
                }

                //4.清除堆垛机任务状态
                OPCHelper.WCSFeedBack(ddjNo, 1);
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"[{sc.ScNo}]号堆垛机：完成任务异常：{ex.Message}", 2);
            }
        }
        #endregion

        #region 堆垛机出库
        public static void DdjOutAll()
        {
            DdjOutM(sc1);
            DdjOutM(sc2);
            DdjOutM(sc3);
            DdjOutM(sc4);
            DdjOutM(sc5);
            DdjOutM(sc6);
            DdjOutM(sc7);
            DdjOutM(sc8);
            DdjOutM(sc9);
            DdjOutM(sc10);
            DdjOutM(sc11);
        }

        static void DdjOutM(Wcs_DdjInfo sc)
        {
            var ddjno = sc.ScNo;
            try
            {
                ////////////////////////////////////////////////一楼出库///////////////////////////////////////////////////////////////////////
                //Step1:堆垛机在自动，空闲，激活的状态下，才能接收任务
                if (!OPCHelper.IsDdjCanRun(ddjno)) { return; }
                if (OPCHelper.ReadSCTStatus(ddjno) != 6)
                {
                    WcsSqlB.InsertLog($"[{ddjno}]号堆垛机状态错误！", 2);
                    OPCHelper.WriteSix(ddjno);
                    return;
                }

                //Step2:堆垛机一楼出库站台是否占用
                var tid = OPCHelper.ReadTaskId(sc.StationOut1No);
                if (tid == 0)
                {
                    DdjOut(ddjno, 1);
                }

                ////////////////////////////////////////////////二楼出库///////////////////////////////////////////////////////////////////////
                //堆垛机在自动，空闲，激活的状态下，才能接收任务
                if (!OPCHelper.IsDdjCanRun(ddjno)) { return; }

                //Step3:堆垛机二楼出库站台是否占用
                var tid2 = OPCHelper.ReadTaskId(sc.StationOut2No);
                if (tid2 == 0)
                {
                    DdjOut(ddjno, 2);
                }
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"[{ddjno}]号堆垛机出库异常：{ex.Message}", 2);
            }
        }

        static void DdjOut(int ddjNo, int layer)
        {
            //Step3:从数据库中获取一条可以出库的任务
            Wcs_Task task = null;
            if (layer == 1)
            {
                //一楼出库
                task = WcsSqlB.GetOutTask1(ddjNo);
            }
            else if (layer == 2)
            {
                //二楼出库
                task = WcsSqlB.GetOutTask2(ddjNo);
            }

            if (task == null) { return; }

            //Step4:写任务给堆垛机
            if (OPCHelper.WriteTaskToSC(task))
            {
                var rlt = WcsSqlB.UpdateWMSTask(task.SeqID.ToString(), 10, 2);
                if (rlt.Length > 0)
                {
                    //出库失败
                    WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机：出库失败，托盘号：[{task.NPalletID}]{rlt}", 2, task.NPalletID); 
                }
                else
                {
                    WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机：出库开始，托盘号：[{task.NPalletID}]", 1, task.NPalletID);
                }
            }
            else
            {
                WcsSqlB.InsertLog($"[{ddjNo}]号堆垛机：出库接收失败，托盘号：[{task.NPalletID}]，任务号：{task.SeqID}", 2, task.NPalletID);
            }
        }
        #endregion

        #region 堆垛机入库
        public static void DdjAllIn()
        {
            DdjIn(sc1);
            DdjIn(sc2);
            DdjIn(sc3);
            DdjIn(sc4);
            DdjIn(sc5);
            DdjIn(sc6);
            DdjIn(sc7);
            DdjIn(sc8);
            DdjIn(sc9);
            DdjIn(sc10);
            DdjIn(sc11);
        }

        //入库
        static void DdjIn(Wcs_DdjInfo sc)
        {
            var ddjno = sc.ScNo;
            try
            {
                //Step1: 读取电气是否有入库请求
                var target = OPCHelper.ReadTarget(sc.StationInNo);
                if (target != 999) { return; }

                //堆垛机是否可运行
                if (!OPCHelper.IsDdjCanRun(ddjno)) { return; }

                if (OPCHelper.ReadSCTStatus(ddjno) != 6)
                {
                    WcsSqlB.InsertLog($"[{ddjno}]号堆垛机状态错误！", 2);
                    OPCHelper.WriteSix(ddjno);
                    return;
                }

                //Step2: 读取任务号在数据库中找到对应的任务
                var seqid = OPCHelper.ReadTaskId(sc.StationInNo);
                //var rPallet = OPCHelper.ReadSCPallet(ddjno);
                var taskInfo = WcsSqlB.GetTaskInfoBySeqId(seqid);
                if (taskInfo == null)
                {
                    WcsSqlB.InsertLog($"[{ddjno}]号堆垛机DdjIn，无法从任务列表中获取到入库任务，任务号：[{seqid}]", 2);
                    return;
                }

                //Step3:把任务写给堆垛机
                if (OPCHelper.WriteTaskToSC(taskInfo))
                {
                    WcsSqlB.InsertLog($"[{ddjno}]号堆垛机：入库开始，托盘号：[{taskInfo.NPalletID}]，任务SeqId:{taskInfo.SeqID}", 1, taskInfo.NPalletID);
                    WcsSqlB.UpdateTaskStatus(taskInfo.SeqID, 21);  //更改任务状态
                }
                else
                {
                    WcsSqlB.InsertLog($"[{ddjno}]号堆垛机：入库接收失败，托盘号：[{taskInfo.NPalletID}]，任务号：{taskInfo.SeqID}", 2, taskInfo.NPalletID);
                }
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"[{ddjno}]号堆垛机入库异常：{ex.Message}", 2);
            }
        }
        #endregion
    }
}
