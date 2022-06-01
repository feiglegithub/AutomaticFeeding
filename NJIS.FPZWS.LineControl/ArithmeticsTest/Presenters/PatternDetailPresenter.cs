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
    public class PatternDetailPresenter: SearchPresenterBase
    {

        public PatternDetailPresenter()
        {
            Register<string>(GetData, (sender, batchName) =>
            {
                ExecuteBase(this,sender,batchName,(o,arg)=> Contract.GetPatternDetailsByBatchName(batchName),data=>Send(BindingData,sender,data));
            });
        }
    }
}
