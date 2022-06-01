using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class SearchPresenterBase:PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string BindingData = nameof(BindingData);

        protected ILineControlCuttingContractPlus _contract = null;

        protected ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());
    }
}
