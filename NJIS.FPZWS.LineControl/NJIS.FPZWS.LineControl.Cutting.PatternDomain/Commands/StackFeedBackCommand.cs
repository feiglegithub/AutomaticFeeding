using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using Convert = System.Convert;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    /// <summary>
    /// 垛状态反馈更新命令
    /// </summary>
    public class StackFeedBackCommand:CommandBase<List<Stack>,string>
    {
        private ILineControlCuttingContractPlus _contract = null;

        private ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        public StackFeedBackCommand(string baseArg=nameof(StackFeedBackCommand)) : base(baseArg)
        {
            this.Validating += StackFeedBackCommand_Validating;
        }

        private void StackFeedBackCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<Stack>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<Stack> LoadRequest(string baseArg)
        {
            //var waitings = Contract.GetStacksByStatus(StackStatus.WaitMaterial);
            //var loadings = Contract.GetStacksByStatus(StackStatus.LoadingMaterial);
            //var stockings = Contract.GetStacksByStatus(StackStatus.Stocking);

            var waitings = StackManager.GetStacksByStatus(StackStatus.WaitMaterial);
            var loadings = StackManager.GetStacksByStatus(StackStatus.LoadingMaterial);
            var stockings = StackManager.GetStacksByStatus(StackStatus.Stocking);
            waitings.AddRange(loadings);
            waitings.AddRange(stockings);
            return waitings;

        }
        private void SendMsg(List<ExecuteMsg> executeMsgs)
        {
            //BroadcastMessage.Send(nameof(ExecuteMsg), executeMsgs);
        }
        protected override void ExecuteContent()
        {
            List<Stack> updatedStacks = new List<Stack>();
            var stacks = RequestData.FindAll(item=>item.Status!=Convert.ToInt32(StackStatus.Stocking));
            var stockings = RequestData.FindAll(item => item.Status == Convert.ToInt32(StackStatus.Stocking));
            foreach (var stack in stacks)
            {
                var cuttings = Contract.GetCuttingStackProductionListsByStackName(stack.StackName);
                foreach (var cutting in cuttings)
                {
                    if (cutting.Status == Convert.ToInt32(RequestLoadingStatus.WmsLoading))
                    {
                        if (stack.Status == Convert.ToInt32(StackStatus.WaitMaterial))
                        {
                            stack.Status = Convert.ToInt32(StackStatus.LoadingMaterial);
                            stack.UpdatedTime = DateTime.Now;
                            updatedStacks.Add(stack);
                        }
                        continue;
                    }

                    if (cutting.Status == Convert.ToInt32(RequestLoadingStatus.Loaded))
                    {
                        stack.Status = Convert.ToInt32(StackStatus.LoadedMaterial);
                        stack.UpdatedTime = DateTime.Now;
                        updatedStacks.Add(stack);
                    }
                }
            }

            var groups = stockings.GroupBy(item => item.FirstBatchName);

            //foreach (var stocking in stockings)
            //{
            //    var stackName = stocking.StackName;
            //    var feedBacks = Contract.GetWmsStacktFeedBacksByStackName(stackName);
            //    if (feedBacks.Exists(item => item.IsSuccess))
            //    {

            //        stocking.Status = Convert.ToInt32(StackStatus.Stocked);
            //        stocking.UpdatedTime = DateTime.Now;
            //        updatedStacks.Add(stocking);

            //    }
            //}

            foreach (var group in groups)
            {
                var list = group.ToList();
                var stackName = list[0].StackName;
                var feedBacks = Contract.GetWmsStacktFeedBacksByStackName(stackName);
                if (feedBacks.Exists(item => item.IsSuccess))
                {
                    list.ForEach(item =>
                    {
                        item.Status = Convert.ToInt32(StackStatus.Stocked);
                        item.UpdatedTime = DateTime.Now;
                        updatedStacks.Add(item);
                    });
                }

            }

            if (updatedStacks.Count==0) return;
            StackManager.SaveStacks(updatedStacks);
            var descriptions = StackStatus.Cut.GetAllFinishStatusDescription();
            var msgs =updatedStacks.ConvertAll(p => new ExecuteMsg()
            {
                Command = GetType().Name, Object = "垛", Type = "状态更新",
                Msg = $"垛状态更新为：{descriptions.First(item => item.Item1 == p.Status).Item2}"
            });

            SendMsg(msgs);
            //Contract.BulkUpdatedStacks(updatedStacks);
        }

        private void SendMsg(string objectStr, string type, string msg)
        {
            BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() { Object = objectStr,Msg = msg, Command = this.GetType().Name, Type = type });
        }
    }
}
