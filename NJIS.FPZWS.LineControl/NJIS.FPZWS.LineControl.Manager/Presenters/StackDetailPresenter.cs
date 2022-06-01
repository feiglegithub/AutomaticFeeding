using System;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
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
