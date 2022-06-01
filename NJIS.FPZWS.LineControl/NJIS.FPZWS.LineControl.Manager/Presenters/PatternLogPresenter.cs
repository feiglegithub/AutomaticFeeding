using System;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class PatternLogPresenter:SearchPresenterBase
    {
        public PatternLogPresenter()
        {
            Register<DateTime>(GetData,(sender,planDate)=>ExecuteBase(this,sender,planDate,
                (s,p)=>Contract.GetPatternLogsByPlanDate(p),logs=> Send(BindingData, sender, logs)
            ));
        }
    }
}
