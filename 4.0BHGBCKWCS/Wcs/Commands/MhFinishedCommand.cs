using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using WcsService;
using WCS.Communications;

namespace WCS.Commands
{
    public class MhFinishedCommand : CommandBase<bool, string>
    {
        private MachineHandCommunication _handCommunication = null;

        private MachineHandCommunication Hand =>
            _handCommunication ?? (_handCommunication = MachineHandCommunication.GetInstance());
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());
        private IMachineHandTaskLogContract _contract = null;
        private IMachineHandTaskLogContract Contract => _contract ?? (_contract = MachineHandTaskLogService.GetInstance());


        public MhFinishedCommand(string baseArg= "拣选完成") : base(baseArg)
        {
            this.Validating += SortingFinishedCommand_Validating;
        }

        private void SortingFinishedCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(string baseArg)
        {
            return Hand.IsFinished;
        }

        protected override void ExecuteContent()
        {
            try
            {
                //执行完成动作相关
                var tasks =Contract.GetMachineHandTaskLogs();
                //没有任务则无需处理，不需要回写Plc
                if (!tasks.Exists(item=>item.Status==1))
                {
                    return;
                }
                var task = tasks.First(item => item.Status == 1);
                LogContract.InsertWcsLog($"收到机械手完成任务，机械手任务号：{task.Id},{task.FromStation}->{task.ToStation},花色:{task.ProductCode}");
                var resultMsg = Contract.FinishedMachineHandTask(task.Id);
                if (!resultMsg.Result)
                {
                    resultMsg = Contract.FinishedMachineHandTask(task.Id);
                    Thread.Sleep(20);
                }
                LogContract.InsertWcsLog($"收到机械手完成任务{(resultMsg.Result?"成功":("失败，"+ resultMsg.Msg))},机械手任务号：{task.Id}");

            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
