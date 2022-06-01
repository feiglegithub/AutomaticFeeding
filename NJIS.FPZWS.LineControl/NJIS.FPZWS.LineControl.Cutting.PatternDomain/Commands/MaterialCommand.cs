using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using System;
using System.Collections.Generic;

namespace ArithmeticsTest.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("已废弃，不建议使用",true)]
    public class MaterialCommand:CommandBase<List<CuttingStackProductionList>,string>
    {
        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = new LineControlCuttingService());

        public MaterialCommand(string baseArg="更新wms上料状态") : base(baseArg)
        {
            this.Validating += UpdatedCommand_Validating;
        }

        private void UpdatedCommand_Validating(object arg1, Args.CancelEventArg<List<CuttingStackProductionList>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<CuttingStackProductionList> LoadRequest(string baseArg)
        {
            var loadeds = Contract.GetCuttingStackProductionListsByStatus(RequestLoadingStatus.Loaded);
            var loadings = Contract.GetCuttingStackProductionListsByStatus(RequestLoadingStatus.WmsLoading);
            loadings.AddRange(loadeds);
            return loadings;
        }

        protected override void ExecuteContent()
        {
            var loadings = RequestData.FindAll(item => item.Status == Convert.ToInt32(RequestLoadingStatus.WmsLoading));
            var loadeds = RequestData.FindAll(item => item.Status == Convert.ToInt32(RequestLoadingStatus.Loaded));

            List < Stack > stacks = new List<Stack>();
            foreach (var loaded in loadeds)
            {
                var tStacks = Contract.GetStacksByStackName(loaded.StackName);
                foreach (var stack in tStacks)
                {
                    if (stack.Status < Convert.ToInt32(StackStatus.LoadedMaterial))
                    {
                        stack.Status = Convert.ToInt32(StackStatus.LoadedMaterial);
                        stack.UpdatedTime = DateTime.Now;
                        stacks.Add(stack);
                    }
                }
            }

            foreach (var loaded in loadings)
            {
                var tStacks = Contract.GetStacksByStackName(loaded.StackName);

                foreach (var stack in tStacks)
                {
                    if (stack.Status < Convert.ToInt32(StackStatus.LoadingMaterial))
                    {
                        stack.Status = Convert.ToInt32(StackStatus.LoadingMaterial);
                        stack.UpdatedTime = DateTime.Now;
                        stacks.Add(stack);
                    }
                }
            }
            if(stacks.Count==0) return;
            Contract.BulkUpdatedStacks(stacks);


        }
    }
}
