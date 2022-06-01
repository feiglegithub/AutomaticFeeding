using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticsTest.Presenters
{
    public class StackPresenter:SearchPresenterBase
    {
        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());
        public StackPresenter()
        {
            Register<Tuple<DateTime, bool>> (GetData, (sender, planDate) =>
            {
                ExecuteBase(this, sender, planDate, (o, arg) => arg.Item2?Contract.GetStacksByPlanDate(arg.Item1): StackManager.GetStacksByPlanDate(arg.Item1), data => Send(BindingData, sender, data));
            });
        }
    }
}
