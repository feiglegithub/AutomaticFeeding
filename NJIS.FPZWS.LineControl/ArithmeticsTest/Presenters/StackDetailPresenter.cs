using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticsTest.Presenters
{
    public class StackDetailPresenter:SearchPresenterBase
    {
        public StackDetailPresenter()
        {
            Register<DateTime>(GetData, (sender, planDate) =>
            {
                ExecuteBase(this, sender, planDate, (o, arg) => Contract.GetStackDetailsByPlanDate(arg), data => Send(BindingData, sender, data));
            });
        }
    }
}
