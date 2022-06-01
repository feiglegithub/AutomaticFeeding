using System;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class PatternPresenter: SearchPresenterBase
    {
        //private IPatternDistribute _distribute = null;
        //private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());
        public PatternPresenter()
        {
            Register<DateTime>(GetData, (sender, planDate) =>
            {
                ExecuteBase(this, sender, planDate, (o, arg) => Contract.GetPatternsByPlanDate(arg), data => Send(BindingData, sender, data));
            });
        }
    }
}
