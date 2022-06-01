using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class TaskLogControlPresenter:PresenterBase
    {
        public const string GetTaskLog = nameof(GetTaskLog);
        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService());
        public TaskLogControlPresenter()
        {
            Register<DateTime>(GetTaskLog,ExecuteGetTaskLog);
        }

        private void ExecuteGetTaskLog(object sender, DateTime planDate)
        {
            var recipient = sender;
            var result = Contract.GetCuttingTaskLogs(planDate.Date);
            Send(TaskLogControl.BindingDatas,recipient,result);
        }
    }
}
