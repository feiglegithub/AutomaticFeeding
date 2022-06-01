using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message;
using Telerik.WinForms.Documents.Layout;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class ChainBufferControlPresenter:PresenterBase
    {
        public ChainBufferControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<ChainBufferArgs>(EmqttSettings.Current.PcsChainBufferRep, ExecuteReciveDatas, EExecutionMode.Asynchronization,true);
        }

        private void ExecuteReciveDatas(ChainBufferArgs data)
        {
            Send(ChainBufferControl.BindingDatas,data);
        }
    }
}
