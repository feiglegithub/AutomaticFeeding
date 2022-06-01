using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class BatchGroupPresenter:SearchPresenterBase
    {
        public const string Save = nameof(Save);
        private DateTime CurPlanDate { get; set; } = DateTime.Today;
        public BatchGroupPresenter()
        {
            Register<DateTime>(GetData, (sender,planDate)=>ExecuteBase(this,sender,planDate,(s,arg)=>
            {
                CurPlanDate = arg;
                var data = Contract.GetBatchGroupsByPlanDate(CurPlanDate);
                Send(BindingData,s,data);
            }));

            Register<List<BatchGroup>>(Save,(sender,args)=>ExecuteBase(this,sender,args, (s, planDate) =>
                {
                    var ret = Contract.BulkUpdatedBatchGroupsLoadTime(args);
                    this.Send(BindingData, ret);
                    if (ret)
                    {
                        var data = Contract.GetBatchGroupsByPlanDate(CurPlanDate);
                        Send(BindingData, s, data);
                    }
                }));
        }
    }
}
