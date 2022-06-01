using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.UI.Common.Message;

namespace ArithmeticsTest.Commands
{
    /// <summary>
    /// 更新锯切图的状态命令
    /// </summary>
    public class PatternDetailCommand:CommandBase<List<Pattern>,string>
    {
        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContract>());

        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());
        public PatternDetailCommand(string baseArg="Updated PatternDetail Command") : base(baseArg)
        {
            this.Validating += PatternDetailCommand_Validating;
        }

        private void PatternDetailCommand_Validating(object arg1, Args.CancelEventArg<List<Pattern>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<Pattern> LoadRequest(string baseArg)
        {
            return Distribute.CuttingPatterns;
        }

        protected override void ExecuteContent()
        {
            var cuttingPatterns = RequestData;
            var updatedPatterns = new List<Pattern>();
            var updatedPatternDetails = new List<PatternDetail>();
            var updatedStacks = new List<Stack>();
            var updatedStackDetails = new List<StackDetail>();
            var groups = cuttingPatterns.GroupBy(item => item.BatchName);
            foreach (var group in groups)
            {
                var batchName = group.Key; 
                 var patterns = group.ToList();

                var stacks = StackManager.GetStacksByPlanDate(patterns[0].PlanDate);
                var stackDetails = StackManager.GetStackDetailsByPlanDate(patterns[0].PlanDate);

                //var stacks = Contract.GetStacksByPlanDate(patterns[0].PlanDate);
                //var stackDetails = Contract.GetStackDetailsByPlanDate(patterns[0].PlanDate);
                var details = Distribute.GetPatternDetailsByBatchName(batchName);

                details = patterns.Join(details, pattern => pattern.PatternId, detail => detail.PatternId,
                    (pattern, detail) => detail).ToList().FindAll(item=> !item.IsFinished && !item.IsOffPart).ToList();
                var feedBacks = Contract.GetPartFeedBacksByBatchName(batchName);

                var tDetails = details.Join(feedBacks, detail => detail.PartId, feedBack => feedBack.PartId,
                    (detail, feedBack) =>new{Detail = detail ,FeedBack=feedBack}).ToList();
                if(tDetails.Count==0) continue;
                foreach (var detail in tDetails)
                {
                    detail.Detail.FinishedPartCount = 1;
                    detail.Detail.IsFinished = true;
                    detail.Detail.UpdatedTime = detail.FeedBack.CreatedTime;
                    updatedPatternDetails.Add(detail.Detail);
                }
                
                var detailGroups = tDetails.GroupBy(item => item.Detail.PatternId);
                foreach (var detailGroup in detailGroups)
                {
                    var patternId = detailGroup.Key;

                    if (!patterns.Exists(item => item.PatternId == patternId))
                    {
                        BroadcastMessage.Send("StackName", $"{batchName}找不到锯切图号{patternId}");
                        return;
                    }

                    var pattern = patterns.First(item => item.PatternId == patternId);

                    var tStackDetails = stackDetails.FindAll(item => item.PatternId == patternId && item.Status< Convert.ToInt32(BookStatus.Cut)).OrderByDescending(item=>item.Status).ToList();
                    if (tStackDetails.Count==0)
                    {
                        BroadcastMessage.Send("StackName", $"{batchName}找不到锯切图号{patternId}的垛明细");
                        return;
                    }
                    if (!stacks.Exists(item => item.StackName == tStackDetails[0].StackName))
                    {
                        BroadcastMessage.Send("StackName", $"找不到垛号{tStackDetails[0].StackName}");
                        return;
                    }

                    var tStack = stacks.First(item => item.StackName == tStackDetails[0].StackName);
                    
                    var startTime = detailGroup.Min(item => item.FeedBack.CreatedTime);
                    var finishedTime = detailGroup.Max(item => item.FeedBack.CreatedTime);
                    if (tStack.Status < Convert.ToInt32(StackStatus.Cutting))
                    {
                        tStack.Status = Convert.ToInt32(StackStatus.Cutting);
                        tStack.UpdatedTime = DateTime.Now;
                        tStack.StartTime = startTime;
                        updatedStacks.Add(tStack);
                    }

                    var tList = details.FindAll(item => item.PatternId == patternId && !item.IsOffPart);

                    if (!tList.Exists(item => !item.IsFinished))
                    {
                        tStackDetails.ForEach(item =>
                        {
                            item.Status = Convert.ToInt32(BookStatus.Cut);
                            item.UpdatedTime = DateTime.Now;
                            
                        });

                        var ttStackDetails = stackDetails.FindAll(item => item.StackName == tStack.StackName);
                        if (!ttStackDetails.Exists(item => item.Status < Convert.ToInt32(BookStatus.Cut)))
                        {
                            tStack.Status = Convert.ToInt32(StackStatus.Cut);
                            tStack.UpdatedTime = DateTime.Now;
                            tStack.FinishedTime = finishedTime;
                            updatedStacks.Add(tStack);
                        }


                        updatedStackDetails.AddRange(tStackDetails);
                        pattern.Status = Convert.ToInt32(PatternStatus.Cut);
                        pattern.FinishedTime = finishedTime;
                        pattern.UpdatedTime = DateTime.Now;
                        updatedPatterns.Add(pattern);
                    }
                    else
                    {
                        if (pattern.Status != Convert.ToInt32(PatternStatus.Cutting))
                        {
                            tStackDetails.ForEach(item =>
                            {
                                item.Status = Convert.ToInt32(BookStatus.Cutting);
                                item.UpdatedTime = DateTime.Now;

                            });
                            updatedStackDetails.AddRange(tStackDetails);

                            pattern.Status = Convert.ToInt32(PatternStatus.Cutting);
                            pattern.UpdatedTime = DateTime.Now;
                            pattern.StartTime = startTime;
                            updatedPatterns.Add(pattern);
                        }
                    }
                }
            }

            if (updatedPatternDetails.Count > 0)
            {
                Distribute.SavePatternDetails(updatedPatternDetails);
            }

            if (updatedPatterns.Count > 0)
            {
                Distribute.SavePatterns(updatedPatterns);
            }

            if (updatedStackDetails.Count > 0)
            {
                //Contract.BulkUpdatedStackDetails(updatedStackDetails);
                StackManager.SaveStackDetails(updatedStackDetails);
            }

            if (updatedStacks.Count > 0)
            {
                //Contract.BulkUpdatedStacks(updatedStacks);
                StackManager.SaveStacks(updatedStacks);
            }
        }
    }
}
