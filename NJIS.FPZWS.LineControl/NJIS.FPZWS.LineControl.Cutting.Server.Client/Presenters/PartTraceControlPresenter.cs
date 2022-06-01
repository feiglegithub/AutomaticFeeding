using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class PartTraceControlPresenter:PresenterBase
    {
        public const string Refresh = nameof(Refresh);
        public PartTraceControlPresenter()
        {
            Register<string>(Refresh, ExecuteRefresh);
            Register<List<PartInfoPositionArgs>>(EmqttSettings.Current.PcsPartInfoPositionRep, ExecuteGetPartPositions, EExecutionMode.Asynchronization,true);
            
        }

        private void ExecuteRefresh(object sender,string str)
        {

        }

        private void ExecuteGetPartPositions(List<PartInfoPositionArgs> partInfoPositionArgses)
        {
            Send(PartTraceControl.ReceiveDatas,partInfoPositionArgses);
        }

        
    }
}
