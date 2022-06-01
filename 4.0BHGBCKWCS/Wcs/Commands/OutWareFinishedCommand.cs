using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using WcsModel;
using WcsService;
using WCS.Communications;

namespace WCS.Commands
{
    /// <summary>
    /// 出库到开料锯完成命令
    /// </summary>
    public class OutWareFinishedCommand:CommandBase<bool,ECuttingStation>
    {
        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());


        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        public OutWareFinishedCommand(ECuttingStation baseArg) : base(baseArg)
        {
            this.Validating += OutWareFinishedCommand_Validating;
        }

        private void OutWareFinishedCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !RequestData;
        }

        protected override bool LoadRequest(ECuttingStation baseArg)
        {
            return Line.IsDone(baseArg);
        }

        protected override void ExecuteContent()
        {
            try
            {
                var stackNo = Line.ReadStackNo(BaseArg);
                LogContract.InsertWcsLog($"收到出库完成请求：垛号：{stackNo},到达站台：【{Convert.ToInt32(BaseArg)}】");
                var tasks = WmsTaskContract.GetWmsTasksByStackNo(stackNo, 2);
                if (tasks.Count == 0)
                {
                    LogContract.InsertWcsErrorLog($"开料站台：【{Convert.ToInt32(BaseArg)}】找不到任务，垛号：{stackNo}");
                    Line.ClearPlcRequest(BaseArg);
                    return;
                }

                var task = tasks[0];

                Line.WriteUpToCutStation(BaseArg, task.HasUpProtect.Value);
                task.TaskStatus = 98;
                task.FinishTime = DateTime.Now;
                WmsTaskContract.UpdateWmsTask(task);
                Line.ClearPlcRequest(BaseArg);
                LogContract.InsertWcsLog($"出库完成：任务号：{task.TaskId},目标站台：{task.ToPosition}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
