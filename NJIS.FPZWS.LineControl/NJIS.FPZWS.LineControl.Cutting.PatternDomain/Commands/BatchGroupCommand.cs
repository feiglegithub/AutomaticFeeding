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
    /// 更新批次组的状态
    /// </summary>
    public class BatchGroupCommand:CommandBase<List<BatchGroup>,string>
    {
        private ILineControlCuttingContractPlus _contract = null;
        private ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());


        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        public BatchGroupCommand(string baseArg="Updated BatchGroup Command") : base(baseArg)
        {
            this.Validating += BatchGroupCommand_Validating;
        }

        private void BatchGroupCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<BatchGroup>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<BatchGroup> LoadRequest(string baseArg)
        {
            var batchGroups = Contract.GetUnFinishedBatchGroups();
            if (batchGroups.Count == 0) return batchGroups;
            var dateAvgStatus = batchGroups.GroupBy(item => item.PlanDate, bg => bg,
                (planDate, bgs) => new {PlanDate = planDate, AvgStatus = bgs.Average(item => item.Status)});

            var curPlanDate = dateAvgStatus.OrderByDescending(item => item.AvgStatus).First().PlanDate;

            return batchGroups.FindAll(item => item.PlanDate == curPlanDate);
        }

        private void SendMsg(string objectStr, string type, string msg)
        {
            //BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() { Object = objectStr, Msg = msg, Command = GetType().Name, Type = type });
        }

        protected override void ExecuteContent()
        {
            Distribute.RemoveFinished(RequestData.First().PlanDate);
            StackManager.RemoveFinished(RequestData.First().PlanDate);
            var batchGroups = RequestData;
            var updatedBatchGroups = new List<BatchGroup>();
            var planDate = batchGroups[0].PlanDate;
            var stacks = StackManager.GetStacksByPlanDate(planDate);

            var stackGroups = stacks.GroupBy(item => item.StackListId);
            foreach (var stackGroup in stackGroups)
            {
                var stackListId = stackGroup.Key;
                var tList = stackGroup.ToList();
                var avgStatus = tList.Average(item => item.Status);
                var tBatchGroups = batchGroups.FindAll(item => item.StackListId == stackListId);
                if(tBatchGroups.Count==0) continue;
                var batchGroup = tBatchGroups.First();

                if (avgStatus < Convert.ToInt32(StackStatus.Stocked))
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.Stocking))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.Stocking);
                        SendMsg("批次组", "状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为备料中");
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }

                if (Math.Abs(avgStatus - Convert.ToInt32(StackStatus.Stocked)) < 0.001)
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.Stocked))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.Stocked);
                        SendMsg("批次组","状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为备料完成");
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }

                if (Math.Abs(avgStatus - Convert.ToInt32(StackStatus.Cut)) < 0.001)
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.Cut))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.Cut);
                        SendMsg("批次组","状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为开料完成");
                        batchGroup.IsFinished = true;
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }

                if (tList.Exists(item => item.Status >= Convert.ToInt32(StackStatus.Cutting)))
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.Cutting))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.Cutting);
                        SendMsg("批次组","状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为开料中");
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }

                if (tList.Exists(item => item.Status == Convert.ToInt32(StackStatus.LoadingMaterial)))
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.LoadingMaterial))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.LoadingMaterial);
                        SendMsg("批次组","状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为上料中");
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }

                if (tList.Exists(item => item.Status == Convert.ToInt32(StackStatus.WaitMaterial)))
                {
                    if (batchGroup.Status != Convert.ToInt32(BatchGroupStatus.WaitMaterial))
                    {
                        batchGroup.Status = Convert.ToInt32(BatchGroupStatus.WaitMaterial);
                        SendMsg("批次组","状态更新",
                            $"批次组:{batchGroup.GroupId}-{nameof(batchGroup.StackListId)}={batchGroup.StackListId},状态为请求上料");
                        batchGroup.UpdatedTime = DateTime.Now;
                        updatedBatchGroups.Add(batchGroup);
                    }
                    continue;
                }


            }

            if (updatedBatchGroups.Count > 0)
            {
                Contract.BulkUpdatedBatchGroups(updatedBatchGroups);
            }
        }
    }


    
}
