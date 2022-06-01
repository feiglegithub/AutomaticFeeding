using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace ArithmeticsTest.Presenters
{
    public class SearchPresenterBase:PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string BindingData = nameof(BindingData);

        protected ILineControlCuttingContract _contract = null;

        protected ILineControlCuttingContract Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContract>());
    }
}
