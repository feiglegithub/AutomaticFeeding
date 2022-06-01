using System;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using System.Collections.Generic;
using System.Linq;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    public class StackDetailCommand:CommandBase<List<StackDetail>, string>
    {
        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        public StackDetailCommand(string baseArg=nameof(StackDetailCommand)) : base(baseArg)
        {
            this.Validating += StackDetailCommand_Validating;
        }

        private void StackDetailCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<StackDetail>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<StackDetail> LoadRequest(string baseArg)
        {
            var list = StackManager.GetStackDetailsByStatus(BookStatus.DistributedPattern).Concat(StackManager.GetStackDetailsByStatus(BookStatus.Cutting)).ToList();
            return list;
        }

        protected override void ExecuteContent()
        {
            var stackDetails = RequestData;
            List<StackDetail> finishedDetails = new List<StackDetail>();
            List<Stack> finishedStacks = new List<Stack>();
            var groups = stackDetails.GroupBy(item => new {item.StackName, item.PatternId,item.ActualBatchName});
            foreach (var group in groups)
            {
                
                var patterns = Distribute.GetPatternsByBatchName(group.Key.ActualBatchName);
                var pattern = patterns.FirstOrDefault(item => item.PatternId == group.Key.PatternId && item.IsEnable && item.Status == PatternStatus.Cut.GetHashCode());
                if (pattern != null)
                {
                    var tList = group.ToList();
                    foreach (var stackDetail in tList)
                    {
                        stackDetail.Status = BookStatus.Cut.GetHashCode();
                        stackDetail.UpdatedTime = pattern.UpdatedTime;
                        finishedDetails.Add(stackDetail);
                    }
                }
            }

            if (finishedDetails.Count > 0)
            {
                var stackGroups = finishedDetails.GroupBy(item => item.StackName);
                foreach (var stackGroup in stackGroups)
                {
                    var stackName = stackGroup.Key;
                    var tStackDetails = StackManager.GetStackDetailsByStackName(stackName);
                    if (!tStackDetails.Exists(item => item.Status < BookStatus.Cut.GetHashCode()))
                    {
                        var stack = StackManager.GetStackByStackName(stackName);
                        if (stack != null)
                        {
                            stack.Status = StackStatus.Cut.GetHashCode();
                            stack.UpdatedTime = DateTime.Now;
                            finishedStacks.Add(stack);
                        }
                    }
                }
            }

            if (finishedDetails.Count > 0)
            {
                StackManager.SaveStackDetails(finishedDetails);
            }

            if (finishedStacks.Count > 0)
            {
                StackManager.SaveStacks(finishedStacks);
            }


        }
    }
}
