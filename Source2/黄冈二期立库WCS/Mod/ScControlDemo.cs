using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.DataBase;
using WCS.model;

namespace WCS.Mod
{
    public class ScControlDemo
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

                //Step2: 获取任务号，找到对应的任务
                var seqid = OPCHelper.ReadSCTaskId(ddjNo);

                //从任务列表中获取到对应的任务
                var taskinfo = WcsSqlB.GetTaskInfoBySeqId(seqid);

                if (taskinfo == null)
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjNo}]DDJFinsh，无法获从任务列表中取到任务，任务号：[{seqid}]", 2);
                    OPCHelper.WCSFeedBack(ddjNo, 1);
                    return;
                }

                if (taskinfo.NordID == 1 || taskinfo.NordID == 5)
                {
                    //入库
                    WcsSqlB.InsertLog($"堆垛机[{ddjNo}]：入库完成，任务号：[{seqid}]", 1, taskinfo.NPalletID);
                    WcsSqlB.UpdateTaskStatus(taskinfo.SeqID, 98);

                    //--------------------------------------------------------------------------------------------------------------
                    WcsSqlB.CreateOutTask(taskinfo);  //测试代码。创建出库任务
                    //--------------------------------------------------------------------------------------------------------------
                }
                else if (taskinfo.NordID == 3 || taskinfo.NordID == 6)
                {
                    //出库                  
                    WcsSqlB.UpdateTaskStatus(taskinfo.SeqID, 31);
                    WcsSqlB.InsertLog($"堆垛机[{ddjNo}]：出库完成，任务号：[{seqid}]", 1, taskinfo.NPalletID);

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

                    //--------------------------------------------------------------------------------------------------------------
                    //var rlt = WcsSqlB.CreateInTask(taskinfo);  //测试代码。创建入库任务 出到同一排下一个货架位
                    //--------------------------------------------------------------------------------------------------------------
                }

                //4.清除堆垛机任务状态
                OPCHelper.WCSFeedBack(ddjNo, 1);
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"堆垛机[{sc.ScNo}]：完成任务异常：{ex.Message}", 2);
            }
        }
        #endregion

        #region 堆垛机出库
        public static void DdjOutAll()
        {
            DdjOut(sc1);
            DdjOut(sc2);
            DdjOut(sc3);
            DdjOut(sc4);
            DdjOut(sc5);
            DdjOut(sc6);
            DdjOut(sc7);
            DdjOut(sc8);
            DdjOut(sc9);
            DdjOut(sc10);
            DdjOut(sc11);
        }

        static void DdjOut(Wcs_DdjInfo sc)
        {
            var ddjno = sc.ScNo;
            try
            {
                //Step1:堆垛机在自动，空闲，激活的状态下，才能接收任务
                if (!OPCHelper.IsDdjCanRun(ddjno)) { return; }

                if (OPCHelper.ReadSCTStatus(ddjno) != 6)
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]状态错误！", 2);
                    OPCHelper.WriteSix(ddjno);
                    //OPCHelper.WCSFeedBack(ddjno, 1);
                    return;
                }

                //if (sc.ScNo == 11)
                //{
                var tid1 = OPCHelper.ReadTaskId(sc.StationOut1No);
                //   if (tid1 > 0)
                //    { return; }
                //}
                //else
                //{
                var tid2 = OPCHelper.ReadTaskId(sc.StationOut2No);
                //    if (tid2 > 0)
                //    { return; }
                //}

                //Step3:从数据库中获取一条可以出库的任务
                Wcs_Task task = WcsSqlB.GetNextTask(ddjno);
                if (task == null) { return; }

                if (task.NoptStation > 2000 && tid2 > 0) { return; }
                if (task.NoptStation < 2000 && tid1 > 0) { return; }

                //Step4:写任务给堆垛机
                if (OPCHelper.WriteTaskToSC(task))
                {
                    WcsSqlB.UpdateTaskStatus(task.SeqID, 31);
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]：出库开始，任务号：[{task.SeqID}]", 1, task.NPalletID);
                }
                else
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]：出库接收失败，任务号：[{task.SeqID}]", 2, task.NPalletID);
                }
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"堆垛机[{ddjno}]出库异常：{ex.Message}", 2);
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
                //堆垛机是否可运行
                if (!OPCHelper.IsDdjCanRun(ddjno)) { return; }

                //Step1: 读取电气是否有入库请求
                if (OPCHelper.ReadTarget(sc.StationInNo) != 999) { return; }

                if (OPCHelper.ReadSCTStatus(ddjno) != 6)
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]状态错误！", 2);
                    OPCHelper.WriteSix(ddjno);
                    return;
                }

                //Step2: 读取任务号
                var seqid = OPCHelper.ReadTaskId(sc.StationInNo);
                if (seqid == 0)
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]DdjIn，无法从入库站台获取到任务号！", 2);
                    return;
                }

                //Step3: 根据垛号在数据库中找到对应的任务
                var taskInfo = WcsSqlB.GetTaskInfoBySeqId(seqid);
                if (taskInfo == null)
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]DdjIn，无法从任务列表中获取到入库任务，任务号：[{seqid}]", 2);
                    return;
                }

                //Step4:把任务写给堆垛机
                if (OPCHelper.WriteTaskToSC(taskInfo))
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]：入库开始，任务号：[{taskInfo.SeqID}]", 1, taskInfo.NPalletID);
                    WcsSqlB.UpdateTaskStatus(taskInfo.SeqID, 21);  //更改任务状态
                }
                else
                {
                    WcsSqlB.InsertLog($"堆垛机[{ddjno}]：入库接收失败，任务号：[{taskInfo.SeqID}]", 2, taskInfo.NPalletID);
                }
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"堆垛机[{ddjno}]入库异常：{ex.Message}", 2);
            }
        }
        #endregion
    }
}
