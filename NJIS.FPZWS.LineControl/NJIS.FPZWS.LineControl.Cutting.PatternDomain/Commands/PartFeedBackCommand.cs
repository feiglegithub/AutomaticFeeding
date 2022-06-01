using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    /// <summary>
    /// 更新板件反馈的状态
    /// </summary>
    public class PartFeedBackCommand:CommandBase<List<PartFeedBack>,string>
    {
        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        private ILineControlCuttingContractPlus _contract = null;

        private ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());

        public PartFeedBackCommand(string baseArg="FeedBackCommand") : base(baseArg)
        {
            this.Validating += PartFeedBackCommand_Validating;
        }

        private void PartFeedBackCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<PartFeedBack>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<PartFeedBack> LoadRequest(string baseArg)
        {
            return Contract.GetPartFeedBacksByStatus(PartFeedBackStatus.UnFinished);
        }

        private void SendMsg(string objectStr, string type, string msg)
        {
            BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() { Object = objectStr, Msg = msg, Command = this.GetType().Name, Type = type });
        }

        private void SendMsg(List<ExecuteMsg> executeMsgs)
        {
            //BroadcastMessage.Send(nameof(ExecuteMsg), executeMsgs);
        }

        protected override void ExecuteContent()
        {
            var groups = RequestData.GroupBy(item => item.BatchName);
            List<PatternDetail> patternDetails = new List<PatternDetail>();
            List<Pattern> patterns = new List<Pattern>();
            foreach (var group in groups)
            {
                var batchName = group.Key;
                var list = group.ToList();
                var tPatternDetails = Distribute.GetPatternDetailsByBatchName(batchName);
                //余料不做检测
                tPatternDetails = tPatternDetails.FindAll(item => !item.IsOffPart);

                var matchPatternDetails = tPatternDetails.Join(list, pd => new {pd.PartId}, l => new {l.PartId}, (pd, feedBack) => pd).ToList();

                matchPatternDetails.ForEach(item =>
                {
                    item.FinishedPartCount += 1;
                    item.IsFinished = item.FinishedPartCount == item.PartCount;
                    item.UpdatedTime =DateTime.Now;
                });
                patternDetails.AddRange(matchPatternDetails);
                var patternIds = matchPatternDetails.GroupBy(item => item.PatternId).ToList().ConvertAll(item=>item.Key);
                var tPatterns = Distribute.GetPatternsByBatchName(batchName);
                foreach (var patternId in patternIds)
                {
                    var tDetails = tPatternDetails.FindAll(item => item.PatternId == patternId);
                    var pattern = tPatterns.FirstOrDefault(item => item.IsEnable && item.PatternId == patternId);
                    var isUnFinished = tDetails.Exists(item => !item.IsFinished);
                    if (pattern != null)
                    {
                        var stackDetails = Contract.GetStackDetailsByPlanDate(pattern.PlanDate);
                        //var batchGroups = Contract.GetBatchGroupsByPlanDate(pattern.PlanDate);
                        var stacks = Contract.GetStacksByPlanDate(pattern.PlanDate);
                        
                        var tStackDetails = stackDetails.FindAll(item =>
                            item.ActualBatchName == pattern.BatchName && item.PatternId == pattern.PatternId);
                        var stackNames = tStackDetails.GroupBy(item => item.StackName).ToList().ConvertAll(item => item.Key);
                        var tStacks = stacks.FindAll(item => stackNames.Contains(item.StackName));
                        List<Stack> updatedStacks = new List<Stack>();
                        var ttStackDetails = stackDetails.FindAll(item => stackNames.Contains(item.StackName));
                        if (!isUnFinished)
                        {
                            tStackDetails.ForEach(item =>
                            {
                                item.Status = Convert.ToInt32(BookStatus.Cut);
                                item.UpdatedTime = DateTime.Now;
                            });

                            if (ttStackDetails.Average(item => item.Status) >= Convert.ToInt32(BookStatus.Cut))
                            {
                                tStacks.ForEach(item =>
                                {
                                    item.Status = Convert.ToInt32(StackStatus.Cut);
                                    item.UpdatedTime = DateTime.Now;
                                    item.FinishedTime = DateTime.Now;
                                });
                                var batchGroups = Contract.GetBatchGroupsByPlanDate(pattern.PlanDate);
                                var updatedGroups = new List<BatchGroup>();
                                foreach (var tGroup in tStacks.GroupBy(item => item.StackListId))
                                {
                                    var tStackListId = tGroup.Key;
                                    var avgStatus = stacks.FindAll(item => item.StackListId == tStackListId)
                                        .Average(item => item.Status);
                                    if (avgStatus >= Convert.ToInt32(StackStatus.Cut))
                                    {
                                        var tBatchGroups = batchGroups.FindAll(item => item.StackListId == tStackListId);
                                        tBatchGroups.ForEach(item =>
                                        {
                                            item.UpdatedTime = DateTime.Now;
                                            item.IsFinished = true;
                                        });
                                        updatedGroups.AddRange(tBatchGroups);
                                    }
                                }

                                if (updatedGroups.Count > 0)
                                {
                                    Contract.BulkUpdatedBatchGroups(updatedGroups);
                                }
                                updatedStacks.AddRange(tStacks);
                            }
                            pattern.Status = Convert.ToInt32(PatternStatus.Cut);
                            pattern.UpdatedTime = DateTime.Now;
                            pattern.FinishedTime = DateTime.Now;
                            patterns.Add(pattern);
                        }
                        else if (pattern.Status < Convert.ToInt32(PatternStatus.Cutting))
                        {
                            tStackDetails = tStackDetails.FindAll(item =>
                                item.Status == Convert.ToInt32(BookStatus.DistributedPattern));
                            tStacks = tStacks.FindAll(item => item.Status == Convert.ToInt32(StackStatus.LoadedMaterial));
                            tStacks.ForEach(item =>
                            {
                                item.Status = Convert.ToInt32(StackStatus.Cutting);
                                item.UpdatedTime = DateTime.Now;
                                item.StartTime = DateTime.Now;
                            });
                            updatedStacks.AddRange(tStacks);
                            tStackDetails.ForEach(item=>
                            {
                                item.UpdatedTime = DateTime.Now;
                                item.Status = Convert.ToInt32(BookStatus.Cutting);
                            });
                            pattern.Status = Convert.ToInt32(PatternStatus.Cutting);
                            pattern.UpdatedTime = DateTime.Now;
                            pattern.StartTime = DateTime.Now;
                            patterns.Add(pattern);
                        }

                        if (tStackDetails.Count > 0)
                        {
                            Contract.BulkUpdatedStackDetails(tStackDetails);
                        }

                        if (updatedStacks.Count > 0)
                        {
                            Contract.BulkUpdatedStacks(updatedStacks);
                        }
                    }
                }
            }

            RequestData.ForEach(item=>
            {
                item.UpdatedTime = DateTime.Now;
                item.Status = Convert.ToInt32(PartFeedBackStatus.Finished);
            });
            Contract.BulkUpdatePartFeedBacks(RequestData);
            if (patterns.Count > 0)
            {
                Distribute.SavePatterns(patterns);
                var descriptions = PartFeedBackStatus.Finished.GetAllFinishStatusDescription();

                var msgs = patterns.ConvertAll(p => new ExecuteMsg()
                {
                    Command = GetType().Name, Object = "锯切图", Type = "状态更新",
                    Msg = $"锯切图状态更新为：{descriptions.First(t => t.Item1 == p.Status).Item2}"
                });
                SendMsg(msgs);
            }

            if (patternDetails.Count > 0)
            {
                Distribute.SavePatternDetails(patternDetails);
                //SendMsg("板件", "加工完成", $"");
            }
        }
    }
}
