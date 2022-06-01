using System;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class StackPresenter:SearchPresenterBase
    {
        //private IStackManage _stackManage = null;
        //private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());
        public StackPresenter()
        {
            Register<DateTime> (GetData, (sender, planDate) =>
            {
                //ExecuteBase(this, sender, planDate, (o, arg) => arg.Item2 ? Contract.GetStacksByPlanDate(arg.Item1) : StackManager.GetStacksByPlanDate(arg.Item1), data => Send(BindingData, sender, data));

                ExecuteBase(this, sender, planDate, (o, arg) => Contract.GetStacksByPlanDate(arg), data => Send(BindingData, sender, data));
            });
        }
    }
}
