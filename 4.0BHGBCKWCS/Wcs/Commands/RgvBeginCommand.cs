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
    /// Rgv任务开始命令
    /// </summary>
    public class RgvBeginCommand : CommandBase<bool, string>
    {
        private RgvCommunication _rgvCommunication = null;

        private RgvCommunication RgvCommunication =>
            _rgvCommunication ?? (_rgvCommunication = RgvCommunication.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private IRgvContract _rgvContract = null;
        private IRgvContract RgvContract => _rgvContract ?? (_rgvContract = RgvService.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());

        protected override void ExecuteContent()
        {
            try
            {
                var wmsRgvTasks = WmsTaskContract.GetRgvWmsTasks();
                if (wmsRgvTasks.Count > 0)
                {
                    LogContract.InsertWcsLog($"收到Rgv空闲信息");
                    var rTask = wmsRgvTasks[0];
                    var writeResult = RgvCommunication.WriteTask(int.Parse(rTask.FromPosition), int.Parse(rTask.ToPosition),
                        rTask.PilerNo.Value);
                    if (!writeResult)
                    {
                        LogContract.InsertWcsLog($"写入rgv任务失败：任务号：{rTask.TaskId}，{rTask.FromPosition}->{rTask.ToPosition}");
                        return;
                    }

                    rTask.TaskStatus = 20;
                    rTask.StartTime = DateTime.Now;
                    var ret = WmsTaskContract.UpdateWmsTask(rTask);
                    if (!ret)
                    {
                        Sleep(20);
                        ret = WmsTaskContract.UpdateWmsTask(rTask);
                        
                    }
                    LogContract.InsertWcsLog($"写入rgv任务成功：任务号：{rTask.TaskId}，{rTask.FromPosition}->{rTask.ToPosition}");
                    return;
                }

                var rgvTasks = RgvContract.GetUnBeingRgvTasks();
                if (rgvTasks.Count > 0)
                {
                    LogContract.InsertWcsLog($"收到Rgv空闲信息");
                    var rTask = rgvTasks[0];
                    var writeResult = RgvCommunication.WriteTask(rTask.FromPosition.Value, rTask.ToPosition.Value, rTask.RTaskId);
                    if (!writeResult)
                    {
                        LogContract.InsertWcsLog($"写入rgv任务失败：任务号：{rTask.RTaskId}，{rTask.FromPosition}->{rTask.ToPosition}");
                        return;
                    }
                    rTask.Status = 20;
                    rTask.StartTime = DateTime.Now;
                    var ret = RgvContract.UpdateRgvTask(rTask);
                    if (!ret)
                    {
                        Sleep(20);
                        ret = RgvContract.UpdateRgvTask(rTask);
                    }

                    LogContract.InsertWcsLog($"写入rgv任务成功：任务号：{rTask.RTaskId}，{rTask.FromPosition}->{rTask.ToPosition}");
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }

        protected override bool LoadRequest(string baseArg)
        {
            return RgvCommunication.IsFree;
        }

        public RgvBeginCommand(string baseArg= "Rgv任务开始") : base("Rgv")
        {
            Validating += RgvCommand_Validating;
        }

        private void RgvCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }
    }
}
