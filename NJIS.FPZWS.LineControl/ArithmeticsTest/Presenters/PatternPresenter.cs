using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.UI.Common.Message;

namespace ArithmeticsTest.Presenters
{
    public class PatternPresenter: SearchPresenterBase
    {
        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());
        public PatternPresenter()
        {
            Register<Tuple<DateTime, bool>>(GetData, (sender, planDate) =>
            {
                ExecuteBase(this, sender, planDate, (o, arg) => arg.Item2?Contract.GetPatternsByPlanDate(arg.Item1): Distribute.GetPatternsByPlanDate(arg.Item1), data => Send(BindingData, sender, data));
            });
        }
    }
}
