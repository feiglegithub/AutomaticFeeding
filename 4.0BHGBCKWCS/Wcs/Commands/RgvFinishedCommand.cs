using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using WcsService;
using WCS.Communications;
using static System.Threading.Thread;

namespace WCS.Commands
{
    /// <summary>
    /// Rgv任务完成命令
    /// </summary>
    public class RgvFinishedCommand:CommandBase<bool,object>
    {

        private RgvCommunication _rgvCommunication = null;

        private RgvCommunication RgvCommunication =>
            _rgvCommunication ?? (_rgvCommunication = RgvCommunication.GetInstance());

        private IRgvContract _rgvContract = null;
        private IRgvContract RgvContract => _rgvContract ?? (_rgvContract = RgvService.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());


        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private LineCommunication _lineCommunication = null;

        private LineCommunication LineCommunication =>
            _lineCommunication ?? (_lineCommunication = LineCommunication.GetInstance());

        public RgvFinishedCommand(string baseArg= "Rgv任务完成") : base(baseArg)
        {
            this.Validating += RgvFinishedCommand_Validating;
        }

        private void RgvFinishedCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(object baseArg)
        {
            return RgvCommunication.IsFinished;
        }

        protected override void ExecuteContent()
        {
            try
            {
                var taskId = RgvCommunication.StackNo;
                
                LogContract.InsertWcsLog($"收到rgv完成任务信号:任务号：{taskId}");
                var rTasks = RgvContract.GetRgvTasksByTaskId(taskId);
                if (rTasks.Count > 0)
                {
                    var rTask = rTasks[0];
                    var stackNo = rTask.PilerNo.Value;
                    var wmsTasks = WmsTaskContract.GetWmsTasksByStackNo(stackNo);
                    if (wmsTasks.Count == 0)
                    {
                        LogContract.InsertWcsErrorLog($"找不到Rgv任务，垛号：{stackNo}");
                        RgvCommunication.FeedBackRgv();
                        return;
                    }
                    var wmsTask = wmsTasks[0];
                    var targetStation = rTask.ToPosition;
                    if (targetStation == 3013)
                    {
                        int target = wmsTask.TaskType == 2
                            ? int.Parse(wmsTask.ToPosition)
                            : 105 - wmsTask.DdjNo.Value;
                        //如果RGV把货物放到GT218,则需要同时把垛号与目标号写给线体
                        var r = LineCommunication.WriteStackNo("GT218", stackNo, target);
                        if (!r)
                        {
                            LogContract.InsertWcsErrorLog($"Rgv写入任务失败，任务号：{wmsTask.TaskId}，垛号：{stackNo},目标站台：{target}");
                        }
                    }
                    else if (targetStation >= 4001)
                    {
                        //RGV把货运到缓存区，任务已完成
                        wmsTask.TaskStatus = 98;
                        wmsTask.FinishTime = DateTime.Now;
                        var r = WmsTaskContract.UpdateWmsTask(wmsTask);
                        if (!r)
                        {
                            Sleep(20);
                            r = WmsTaskContract.UpdateWmsTask(wmsTask);
                            
                        }
                    }
                    else if (targetStation>=3001 && targetStation<=3007)
                    {
                        wmsTask.TaskStatus = 98;
                        wmsTask.FinishTime = DateTime.Now;
                        var r = WmsTaskContract.UpdateWmsTask(wmsTask);
                        if (!r)
                        {
                            Sleep(20);
                            r = WmsTaskContract.UpdateWmsTask(wmsTask);

                        }
                    }
                    rTask.FinishTime = DateTime.Now;
                    rTask.Status = 98;
                    if (!RgvContract.UpdateRgvTask(rTask))
                    {
                        Sleep(20);
                        RgvContract.UpdateRgvTask(rTask);
                        
                    }

                    if (!RgvCommunication.FeedBackRgv())
                    {
                        Sleep(20);
                        RgvCommunication.FeedBackRgv();

                    }

                    LogContract.InsertWcsLog($"Rgv任务完成，任务号：{rTask.TaskId}");
                    return;
                }

                //var wmsRgvTasks = WmsTaskContract.GetRgvWmsTasksByStackNo(stackNo);
                //if (wmsRgvTasks.Count > 0)
                //{
                //    var rTask = wmsRgvTasks[0];
                //    rTask.TaskStatus = 98;
                //    rTask.FinishTime = DateTime.Now;
                //    if (!WmsTaskContract.UpdateWmsTask(rTask))
                //    {
                //        Sleep(20);
                //        WmsTaskContract.UpdateWmsTask(rTask);
                //    }

                //    if (!RgvCommunication.FeedBackRgv())
                //    {
                //        Sleep(20);
                //        RgvCommunication.FeedBackRgv();
                //    }
                //    LogContract.InsertWcsLog($"Rgv任务完成，任务号：{rTask.TaskId}");
                //    return;
                //}

                LogContract.InsertWcsErrorLog($"找不到Rgv任务，任务号：{taskId}");
                RgvCommunication.FeedBackRgv();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
