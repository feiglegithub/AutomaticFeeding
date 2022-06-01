using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.UI.Common.Message;

namespace ArithmeticsTest.Commands
{
    public class GetPatternCommand:CommandBase<List<Stack>,string>
    {
        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContract>());


        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());


        public GetPatternCommand(string deviceName) : base(deviceName)
        {
            this.Validating += GetPatternCommand_Validating;
        }

        private void GetPatternCommand_Validating(object arg1, Args.CancelEventArg<List<Stack>> arg2)
        {
            arg2.Cancel = RequestData.Count == 0;
        }

        protected override List<Stack> LoadRequest(string baseArg)
        {
            var t = StackManager.GetStacksByDeviceName(baseArg);
            return t.FindAll(item =>
                item.Status == Convert.ToInt32(StackStatus.Cutting) ||
                item.Status == Convert.ToInt32(StackStatus.LoadedMaterial));
            //return Contract.GetStacksByDevice(baseArg, StackStatus.Cutting)
            //    .Concat(Contract.GetStacksByDevice(baseArg, StackStatus.LoadedMaterial)).ToList();
        }

        private List<Tuple<string,int>> ConvertBatchList(List<BatchGroup> batchGroups)
        {
            List<string> list = new List<string>();
            List<Tuple<string,int>> tuples = new List<Tuple<string, int>>();
            var index = 0;
            foreach (var batchGroup in batchGroups.OrderBy(item=>item.BatchIndex))
            {
                if(!list.Contains(batchGroup.FrontBatchName))
                    list.Add(batchGroup.FrontBatchName);
                if (!list.Contains(batchGroup.BatchName))
                    list.Add(batchGroup.BatchName);
                if (!list.Contains(batchGroup.NextBatchName))
                    list.Add(batchGroup.NextBatchName);
            }

            list.RemoveAll(string.IsNullOrWhiteSpace);

            foreach (var l in list)
            {
                tuples.Add(new Tuple<string, int>(l,++index));
            }

            return tuples;
        }

        private void SendMsg(string objectStr,string type, string msg)
        {
            BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() {Object = objectStr, Msg = msg, Command = this.GetType().Name, Type = type });
        }

        protected override void ExecuteContent()
        {
            var stack = RequestData.OrderByDescending(item => item.Status).First();
            var stackDetails = StackManager.GetStackDetailsByStackName(stack.StackName);
            IStackControl iStackControl = new StackControl(stack, stackDetails);
            if(!iStackControl.NeedDistributePattern) return;
            var batchGroups = Contract.GetBatchGroupsByPlanDate(stack.PlanDate);

            var tBatchGroup = batchGroups.FirstOrDefault(item =>
                item.BatchName == stack.FirstBatchName || 
                item.FrontBatchName == stack.FirstBatchName ||
                item.NextBatchName == stack.FirstBatchName);
            if(tBatchGroup==null) return;
            var ttBatchGroups = batchGroups.FindAll(item => item.GroupId == tBatchGroup.GroupId);
            var batchList = ConvertBatchList(ttBatchGroups);
            var tCurrentBatchName = Distribute.CurrentBatchName;
            if (string.IsNullOrWhiteSpace(tCurrentBatchName))
            {
                var join = batchList.Join(Distribute.FinishedBatchs, tuple => tuple.Item1, batchName => batchName,
                    (tuple, batchName) => tuple);
                if (!join.Any())
                {
                    tCurrentBatchName = stack.FirstBatchName;
                }
                else
                {
                    var lastTuple = join.OrderByDescending(item => item.Item2).First();
                    tCurrentBatchName = batchList.FindAll(item => item.Item2 > lastTuple.Item2).OrderBy(item => item.Item2).First().Item1;
                }
            }

            var curTuple = batchList.FirstOrDefault(item => item.Item1 == tCurrentBatchName);
            if(curTuple==null) return;

            var tFronItem = batchList.FindAll(item=>item.Item2<curTuple.Item2).OrderByDescending(item=>item.Item2).FirstOrDefault();
            var tNextItem = batchList.FindAll(item => item.Item2 > curTuple.Item2).OrderBy(item => item.Item2).FirstOrDefault();

            var frontBatchName = tFronItem == null ? string.Empty : tFronItem.Item1;
            var nextBatchName = tNextItem == null ? string.Empty : tNextItem.Item1;

            var curColor = iStackControl.CurrentFirstColor;

            var hasPattern = Distribute.HasPattern(tCurrentBatchName, curColor);

            if (hasPattern)
            {
                Pattern pattern = null;
                
                var curBookBatch = iStackControl.CurrentFirstBookBatchName;
                if (tCurrentBatchName == curBookBatch)
                {
                    var maxBookCount = iStackControl.GetMaxBookCountByBookType(frontBatchName, tCurrentBatchName,
                        nextBatchName, BookType.CurrentBatch);

                    pattern = Distribute.DistributePattern(tCurrentBatchName,stack.StackName, maxBookCount, curColor,
                        BookType.CurrentBatch);
                    //if (pattern == null)//无直接符合的锯切图
                    //{
                    //    var tMaxBookCount = iStackControl.GetMaxBookCountByBookType(frontBatchName, tCurrentBatchName,
                    //        nextBatchName, BookType.ContainNextBatch);
                    //    pattern = Distribute.DistributePattern(tCurrentBatchName, stack.StackName, tMaxBookCount, curColor,
                    //        BookType.ContainNextBatch);
                    //    if (pattern == null)
                    //    {
                    //        //拆解锯切图
                    //        Distribute.RequestDisintegratedPattern(tCurrentBatchName, maxBookCount,curColor);
                    //        return;
                    //    }
                    //}
                }
                else if(frontBatchName == curBookBatch)
                {
                    var maxBookCount = iStackControl.GetMaxBookCountByBookType(frontBatchName, tCurrentBatchName,
                        nextBatchName, BookType.ContainFrontBatch);

                    pattern = Distribute.DistributePattern(tCurrentBatchName, stack.StackName, maxBookCount, curColor,
                        BookType.ContainFrontBatch);
                    if (pattern == null)//无直接符合的锯切图
                    {
                        //拆解锯切图
                        Distribute.RequestDisintegratedPattern(tCurrentBatchName, maxBookCount, curColor);
                        return;
                    }
                }
                else if (nextBatchName == curBookBatch)
                {
                    var maxBookCount = iStackControl.GetMaxBookCountByBookType(frontBatchName, tCurrentBatchName,
                        nextBatchName, BookType.NextBatch);

                    pattern = Distribute.DistributePattern(tCurrentBatchName, stack.StackName, maxBookCount, curColor,
                        BookType.NextBatch);
                    if (pattern == null)//无直接符合的锯切图
                    {
                        //拆解锯切图
                        Distribute.RequestDisintegratedPattern(tCurrentBatchName, maxBookCount, curColor);
                        return;
                    }
                }

                if (pattern!=null)
                {
                    // 锯切图占用
                    var linkResult = iStackControl.BookLinkedPattern(pattern,out List<StackDetail> tDetails);
                    if (!linkResult)
                    {
                        pattern.DeviceName = string.Empty;
                        pattern.Status = Convert.ToInt32(PatternStatus.Undistributed);
                        SendMsg("锯切图", "状态更新", $"批次{pattern.BatchName}:{pattern.PatternId}号锯切图占用失败,状态还原为未分配状态");
                        pattern.UpdatedTime = DateTime.Now;
                    }
                    else
                    {
                        //Contract.BulkUpdatedStackDetails(tDetails);
                        StackManager.SaveStackDetails(tDetails);
                        if (string.IsNullOrWhiteSpace(pattern.DeviceName))
                        {
                            pattern.DeviceName = iStackControl.Stack.ActualDeviceName;
                        }
                        Distribute.SavePatterns(new List<Pattern>() {pattern});
                    }
                    Distribute.SavePatterns(new List<Pattern>() { pattern });
                }

            }
            else //无指定花色的锯切图（后续可新增少量混批功能）
            {
                return;
            }
            
        }
    }
}
