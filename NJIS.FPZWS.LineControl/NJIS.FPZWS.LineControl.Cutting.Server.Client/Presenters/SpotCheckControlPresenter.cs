using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class SpotCheckControlPresenter:PresenterBase
    {
        public const string GetPartInfos = nameof(GetPartInfos);

        private ILineControlCuttingContract _lineControlCutting = null;

        private ILineControlCuttingContract LineControlCuttingService =>
            _lineControlCutting ?? (_lineControlCutting = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService());

        public SpotCheckControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<string>(GetPartInfos, ExecuteGetPartInfos);
        }

        private void ExecuteGetPartInfos(object sender,string partId)
        {
            var recipient = sender;
            var datas = LineControlCuttingService.GetCutPartInfoCollectors(partId);
            Send(SpotCheckControl.ReciveCanCheckPartInfos,recipient,datas);
        }
    }
}
