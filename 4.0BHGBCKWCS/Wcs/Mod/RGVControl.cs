using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WCS.Communications;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS.Mod
{
    /// <summary>
    /// RGV控制类
    /// </summary>
    public class RGVControl:Singleton<RGVControl>
    {
        private RGVControl() { }

        private RgvCommunication _rgvCommunication = null;

        private RgvCommunication RgvCommunication =>
            _rgvCommunication ?? (_rgvCommunication = RgvCommunication.GetInstance());

        private LineCommunication _lineCommunication = null;

        private LineCommunication LineCommunication =>
            _lineCommunication ?? (_lineCommunication = LineCommunication.GetInstance());

        /// <summary>
        /// RGV任务完成
        /// </summary>
        public void RgvFinishTask()
        {
            try
            {
                //收到完成信号
                if (!RgvCommunication.IsFinished) return;

                //找垛号
                var pilerNo = RgvCommunication.StackNo;

                var manualTask = WCSSql.GetCutting67ManualTask(31, pilerNo);
                if (manualTask != null)
                {
                    WCSSql.UpdateCutting67ManualTask(manualTask.TaskId, 99);
                    OpcHsc.FeedBackRGV();
                    return;
                }

                WCSSql.InsertRGVRequestLog(pilerNo,99);
                //根据垛号在RGV_Task找任务
                var rgvTask = WCSSql.GetRGVTaskByPilerNo(pilerNo);

                if (rgvTask == null)
                {
                    //根据垛号在WMS_Task找上料任务
                    TaskInfo wmsCutTask = WCSSql.GetTaskByPilerNo(pilerNo, 4);
                    if (wmsCutTask == null)
                    {
                        WCSSql.InsertLog($"从任务列表中获取RGV任务失败[RGVFnishTask]，垛号：{pilerNo}", "ERROR", pilerNo);
                    }
                    else
                    {
                        //这是一个上料的任务，直接存在WMS任务表，不在RGV任务表
                        WCSSql.InsertLog($"RGV上料已完成！垛号：{pilerNo}，起始位：{wmsCutTask.FromPosition}，目标位：{wmsCutTask.ToPosition}", "LOG", pilerNo);
                        WCSSql.UpdateTaskStatus(wmsCutTask.TaskId, 98, "finish");
                    }

                    RgvCommunication.FeedBackRgv();
                    return;
                }
               
                //非上料任务
                var wmsBufferTask = WCSSql.GetTaskByPilerNo(pilerNo);
                if (wmsBufferTask == null)
                {
                    WCSSql.InsertLog($"从WMS任务列表中获取任务失败[RGVFnishTask]，垛号：{pilerNo}", "ERROR", pilerNo);
                    RgvCommunication.FeedBackRgv();
                    return;
                }

                var targetStation = rgvTask.ToPosition;
                if (targetStation == 3013)
                {
                    //如果RGV把货物放到GT218,则需要同时把垛号与目标号写给线体
                    LineCommunication.WriteStackNo("GT218", pilerNo, wmsBufferTask.target);
                }
                else if (targetStation == 4030)
                {
                    //胶水，封边带出库 (暂不处理)
                }
                else if (targetStation >= 4001)
                {
                    //RGV把货运到缓存区，任务已完成
                    WCSSql.UpdateTaskStatus(wmsBufferTask.TaskId, 98, "finish");
                }

                //清任务状态,更新状态，写日记             
                WCSSql.UpdateRGVTaskStatus(98, rgvTask.RTaskId);
                WCSSql.InsertLog($"RGV任务已完成！垛号：{pilerNo}，起始位：{rgvTask.FromPosition}，目标位：{targetStation}", "LOG", pilerNo);
                RgvCommunication.FeedBackRgv();
                WCSSql.UpdateTaskStatus(wmsBufferTask.TaskId, 98, "finish");
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("RGV任务完成异常，" + ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 给RGV写任务
        /// </summary>
        public void WriteRgvTask()
        {
            try
            {
                //判断RGV可动
                if (!RgvCommunication.IsFree) { return; }

                var manualTask = WCSSql.GetCutting67ManualTask(29);
                if (manualTask != null)
                {
                    OpcHsc.WriteRGVTask(Convert.ToInt32(manualTask.FromPosition), Convert.ToInt32(manualTask.ToPosition), manualTask.PilerNo.Value);
                    WCSSql.UpdateCutting67ManualTask(manualTask.TaskId, 31);
                    return;
                }
                
                //获取任务
                var rTask = WCSSql.GetNextRGVTask();

                if (rTask == null) { return; }

                //写任务（从哪来，到哪去，垛号)
                var rlt = RgvCommunication.WriteTask(rTask.FromPosition, rTask.ToPosition, rTask.PilerNo);

                //改状态，写日记
                if (rlt)
                {
                    WCSSql.UpdateRGVTaskStatus(20, rTask.RTaskId);
                    WCSSql.InsertRGVRequestLog(rTask.PilerNo, 1, rTask.FromPosition.ToString(), rTask.ToPosition.ToString());
                    WCSSql.InsertLog($"RGV已接收任务！垛号：{rTask.PilerNo}，起始位：{rTask.FromPosition}，目标位：{rTask.ToPosition}", "LOG");
                }
                else
                {
                    WCSSql.InsertLog($"RGV接收任务失败！垛号：{rTask.PilerNo}，起始位：{rTask.FromPosition}，目标位：{rTask.ToPosition}", "ERROR");
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("给RGV写任务出错，" + ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 上料任务由WMS下发，所以RGV直接从WMS任务表中获取上料任务
        /// </summary>
        public void WriteWmsTaskToRgv()
        {
            try
            {
                //判断RGV可动
                if (!RgvCommunication.IsFree) { return; }

                var rTask = WCSSql.GetRGVTaskFromWMS();

                if (rTask == null) { return; }

                //写任务（从哪来，到哪去，垛号)
                var rlt = RgvCommunication.WriteTask(rTask.FromPosition, rTask.ToPosition, rTask.PilerNo);

                //改状态，写日记
                if (rlt)
                {
                    WCSSql.UpdateTaskStatus(rTask.RTaskId, 20, "start");
                    WCSSql.InsertRGVRequestLog(rTask.PilerNo, 1,rTask.FromPosition.ToString(),rTask.ToPosition.ToString());
                    WCSSql.InsertLog($"RGV已接收上料任务！垛号：{rTask.PilerNo}，起始位：{rTask.FromPosition}，目标位：{rTask.ToPosition}", "LOG");
                }
                else
                {
                    WCSSql.InsertLog($"RGV接收上料任务失败！垛号：{rTask.PilerNo}，起始位：{rTask.FromPosition}，目标位：{rTask.ToPosition}", "ERROR");
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("给RGV写上料任务出错，" + ex.Message, "ERROR");
            }
        }
    }

    #region Old Code

    //public class RGVControl
    //{

    //    /// <summary>
    //    /// RGV任务完成
    //    /// </summary>
    //    public static void RGVFnishTask()
    //    {
    //        try
    //        {
    //            //收到完成信号
    //            if (!OpcHsc.ReadRGVFinished()) { return; }

    //            //找垛号
    //            var pilerNo = OpcHsc.ReadRGVPilerNo();
    //            WCSSql.InsertRGVRequestLog(pilerNo, 99);
    //            //根据垛号在RGV_Task找任务
    //            var rgv_task = WCSSql.GetRGVTaskByPilerNo(pilerNo);

    //            if (rgv_task == null)
    //            {
    //                //根据垛号在WMS_Task找上料任务
    //                TaskInfo wms_cut_task = WCSSql.GetTaskByPilerNo(pilerNo, 4);
    //                if (wms_cut_task == null)
    //                {
    //                    WCSSql.InsertLog($"从任务列表中获取RGV任务失败[RGVFnishTask]，垛号：{pilerNo}", "ERROR", pilerNo);
    //                }
    //                else
    //                {
    //                    //这是一个上料的任务，直接存在WMS任务表，不在RGV任务表
    //                    WCSSql.InsertLog($"RGV上料已完成！垛号：{pilerNo}，起始位：{wms_cut_task.FromPosition}，目标位：{wms_cut_task.ToPosition}", "LOG", pilerNo);
    //                    WCSSql.UpdateTaskStatus(wms_cut_task.TaskId, 98, "finish");
    //                }

    //                OpcHsc.FeedBackRGV();
    //                return;
    //            }

    //            //非上料任务
    //            var wms_buffer_task = WCSSql.GetTaskByPilerNo(pilerNo);
    //            if (wms_buffer_task == null)
    //            {
    //                WCSSql.InsertLog($"从WMS任务列表中获取任务失败[RGVFnishTask]，垛号：{pilerNo}", "ERROR", pilerNo);
    //                OpcHsc.FeedBackRGV();
    //                return;
    //            }

    //            var targetStation = rgv_task.ToPosition;
    //            if (targetStation == 3013)
    //            {
    //                //如果RGV把货物放到GT218,则需要同时把垛号与目标号写给线体
    //                OpcHsc.WriteDeviceData("GT218", pilerNo, wms_buffer_task.target);
    //            }
    //            else if (targetStation == 4030)
    //            {
    //                //胶水，封边带出库 (暂不处理)
    //            }
    //            else if (targetStation >= 4001)
    //            {
    //                //RGV把货运到缓存区，任务已完成
    //                WCSSql.UpdateTaskStatus(wms_buffer_task.TaskId, 98, "finish");
    //            }

    //            //清任务状态,更新状态，写日记             
    //            WCSSql.UpdateRGVTaskStatus(98, rgv_task.RTaskId);
    //            WCSSql.InsertLog($"RGV任务已完成！垛号：{pilerNo}，起始位：{rgv_task.FromPosition}，目标位：{targetStation}", "LOG", pilerNo);
    //            OpcHsc.FeedBackRGV();
    //            WCSSql.UpdateTaskStatus(wms_buffer_task.TaskId, 98, "finish");
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog("RGV任务完成异常，" + ex.Message, "ERROR");
    //        }
    //    }

    //    /// <summary>
    //    /// 给RGV写任务
    //    /// </summary>
    //    public static void WriteRGVTask()
    //    {
    //        try
    //        {
    //            //判断RGV可动
    //            if (!OpcHsc.RGVCanDo()) { return; }

    //            //获取任务
    //            var r_task = WCSSql.GetNextRGVTask();

    //            if (r_task == null) { return; }

    //            //写任务（从哪来，到哪去，垛号)
    //            var rlt = OpcHsc.WriteRGVTask(r_task.FromPosition, r_task.ToPosition, r_task.PilerNo);

    //            //改状态，写日记
    //            if (rlt)
    //            {
    //                WCSSql.UpdateRGVTaskStatus(20, r_task.RTaskId);
    //                WCSSql.InsertRGVRequestLog(r_task.PilerNo, 1, r_task.FromPosition.ToString(), r_task.ToPosition.ToString());
    //                WCSSql.InsertLog($"RGV已接收任务！垛号：{r_task.PilerNo}，起始位：{r_task.FromPosition}，目标位：{r_task.ToPosition}", "LOG");
    //            }
    //            else
    //            {
    //                WCSSql.InsertLog($"RGV接收任务失败！垛号：{r_task.PilerNo}，起始位：{r_task.FromPosition}，目标位：{r_task.ToPosition}", "ERROR");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog("给RGV写任务出错，" + ex.Message, "ERROR");
    //        }
    //    }

    //    /// <summary>
    //    /// 上料任务由WMS下发，所以RGV直接从WMS任务表中获取上料任务
    //    /// </summary>
    //    public static void WriteWMSTaskToRGV()
    //    {
    //        try
    //        {
    //            //判断RGV可动
    //            if (!OpcHsc.RGVCanDo()) { return; }

    //            var r_task = WCSSql.GetRGVTaskFromWMS();

    //            if (r_task == null) { return; }

    //            //写任务（从哪来，到哪去，垛号)
    //            var rlt = OpcHsc.WriteRGVTask(r_task.FromPosition, r_task.ToPosition, r_task.PilerNo);

    //            //改状态，写日记
    //            if (rlt)
    //            {
    //                WCSSql.UpdateTaskStatus(r_task.RTaskId, 20, "start");
    //                WCSSql.InsertRGVRequestLog(r_task.PilerNo, 1, r_task.FromPosition.ToString(), r_task.ToPosition.ToString());
    //                WCSSql.InsertLog($"RGV已接收上料任务！垛号：{r_task.PilerNo}，起始位：{r_task.FromPosition}，目标位：{r_task.ToPosition}", "LOG");
    //            }
    //            else
    //            {
    //                WCSSql.InsertLog($"RGV接收上料任务失败！垛号：{r_task.PilerNo}，起始位：{r_task.FromPosition}，目标位：{r_task.ToPosition}", "ERROR");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            WCSSql.InsertLog("给RGV写上料任务出错，" + ex.Message, "ERROR");
    //        }
    //    }
    //}

    #endregion
}
