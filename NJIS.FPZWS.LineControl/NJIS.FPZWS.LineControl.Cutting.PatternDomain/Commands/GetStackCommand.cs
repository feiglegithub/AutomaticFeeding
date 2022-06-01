using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    public class GetStackCommand:CommandBase<List<BatchGroup>,string>
    {
        private ILineControlCuttingContractPlus _contract = null;

        private ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());

        private IStackManage _stackManage = null;
        private IStackManage StackManager => _stackManage ?? (_stackManage = StackManage.GetInstance());

        public GetStackCommand(string deviceName) : base(deviceName)
        {
            this.Validating += GetStackCommand_Validating;
        }

        private void GetStackCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<BatchGroup>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<BatchGroup> LoadRequest(string deviceName)
        {
            var batchGroups = Contract.GetUnFinishedBatchGroups();
            if (batchGroups.Count == 0) return batchGroups;
            var groups = batchGroups.GroupBy(item => item.PlanDate, batchGroup => batchGroup,
                (planDate, bgs) => new
                {
                    PlanDate = planDate, BatchGroups = bgs,
                    FinishedStatus = bgs.Average(item => item.Status)
                }).ToList();
            groups = groups.OrderByDescending(item => item.FinishedStatus).ThenBy(item=>item.PlanDate).ToList();

            return groups[0].BatchGroups.ToList().FindAll(item =>
                item.StartLoadingTime != null && DateTime.Now > item.StartLoadingTime);
        }
        private void SendMsg(string objectStr, string type, string msg)
        {
            //BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() { Object = objectStr, Msg = msg, Command = GetType().Name, Type = type });
        }
        protected override void ExecuteContent()
        {
            var deviceName = BaseArg;
            var deviceInfos = Contract.GetCuttingDeviceInfos();
            var deviceInfo = deviceInfos.FirstOrDefault(item => item.DeviceName == deviceName);
            //需要校验设备是否禁用
            if (deviceInfo?.State == null || deviceInfo.State==0) return;
            //当天计划日期，当前时间允许上料的
            var canLoadings = RequestData;
            var stacks = StackManager.GetStacksByPlanDate(canLoadings[0].PlanDate);
            //var stacks = Contract.GetStacksByPlanDate(canLoadings[0].PlanDate);
            //有正在上料，则不请求上料
            if(stacks.Exists(item=>item.ActualDeviceName==deviceName && item.Status >= Convert.ToInt32(StackStatus.WaitMaterial) && item.Status <= Convert.ToInt32(StackStatus.LoadedMaterial))) return;

            var curStack = stacks.FirstOrDefault(item =>
                item.ActualDeviceName == deviceName && item.Status == Convert.ToInt32(StackStatus.Cutting));
            if (curStack != null)
            {
                //var details = Contract.GetStackDetailsByStackName(curStack.StackName);
                var details = StackManager.GetStackDetailsByStackName(curStack.StackName);
                IStackControl iStackControl = new StackControl(curStack,details);
                if(!iStackControl.NeedLoadNextStack) return;

                if (!string.IsNullOrWhiteSpace(curStack.NextStackName) && curStack.PlanDeviceName==deviceName)
                {
                    var nextStack = StackManager.GetStackByStackName(curStack.NextStackName);
                    RequestLoad(nextStack, deviceName, deviceInfo);
                    return;
                }
            }


            //var unStockedBatchNames = stacks.FindAll(item => item.Status < Convert.ToInt32(StackStatus.Stocked))
            //    .ConvertAll(item => item.FirstBatchName).Distinct();

            var unStackGroups = canLoadings.FindAll(item => item.Status == Convert.ToInt32(BatchGroupStatus.Stocking)).ConvertAll(item=>item.GroupId).Distinct().ToList();

            var finishedGroups = canLoadings.GroupBy(item => item.GroupId,
                (groupId, gbs) => new
                {
                    GroupId = groupId,
                    IsFinished = gbs.Count(item => item.Status < Convert.ToInt32(BatchGroupStatus.Cut)) == 0
                }).ToList().FindAll(item => item.IsFinished).ConvertAll(item => item.GroupId).Distinct().ToList();
            var removeGroups = unStackGroups.Concat(finishedGroups).ToList();
            //未备料完的组
            //var unStocks = canLoadings.FindAll(item => unStockedBatchNames.Contains(item.BatchName)).ConvertAll(item=>item.GroupId).Distinct();
            //canLoadings.RemoveAll(item => unStocks.Contains(item.GroupId));

            canLoadings.RemoveAll(item => removeGroups.Contains(item.GroupId));

            if (canLoadings.Count==0) return;

            //获取当前组
            var batchGroupStatus = canLoadings.GroupBy(item => new { item.GroupId }, bg => bg,
                (key, bgs) =>
                    new
                    {
                        GroupId = key.GroupId,
                        //StackListId = key.StackListId,
                        MaxStatus = bgs.Max(item => item.Status),
                        AvgStatus = bgs.Average(item => item.Status),
                        StackListCount = bgs.Count()
                    })
                .OrderByDescending(item => item.MaxStatus).ThenBy(item => item.GroupId)
                .ThenByDescending(item => item.StackListCount).ToList();

            if (!batchGroupStatus.Any()) return;


            int curGroupId = batchGroupStatus.First().GroupId;

            var curBatchGroups = canLoadings.FindAll(item => item.GroupId == curGroupId).OrderBy(item => item.BatchIndex).ToList();
            var stackListIds = curBatchGroups.ConvertAll(item => item.StackListId).Distinct().ToList();

            if (!stacks.Exists(item =>
                item.Status == Convert.ToInt32(StackStatus.Stocked) && stackListIds.Contains(item.StackListId)))
            {
                if (batchGroupStatus.Count == 1) return;//无其他组（无垛可上）

                curGroupId = batchGroupStatus[1].GroupId;

                curBatchGroups = canLoadings.FindAll(item => item.GroupId == curGroupId).OrderBy(item => item.BatchIndex).ToList();
            }
            

            Stack stack = null;
            
            foreach (var batchGroup in curBatchGroups)
            {
                var tStacks = stacks.FindAll(item => item.StackListId == batchGroup.StackListId && item.Status == Convert.ToInt32(StackStatus.Stocked));
                if(tStacks.Count==0) continue;
                stack = tStacks.FirstOrDefault(item => item.PlanDeviceName == deviceName) ?? tStacks.OrderByDescending(item => item.FirstBatchBookCount).First();
                break;
            }
            if(stack==null) return;
            if(stack.PlanDeviceName!=deviceName) return;
            
            RequestLoad(stack, deviceName, deviceInfo);



            #region Old Code

            //var cuttingStack = stacks.FirstOrDefault(item =>
            //    item.ActualDeviceName == deviceName && item.Status == Convert.ToInt32(StackStatus.Cutting));
            //if (cuttingStack == null)
            //{

            //}
            //var groups = canLoadings.GroupBy(item => item.BatchName, bg => bg,
            //    (batchName, bgs) => new
            //        {BatchName = batchName, AvgStatus = bgs.Average(item => item.IsFinished ? 1 : 0)});
            ////最多只能支持少量混批的情况下适用
            //groups = groups.OrderBy(item => item.AvgStatus);
            //var canLoading = canLoadings.First(item=>item.BatchName == groups.First().BatchName);

            //// 缓慢生产的设备的垛
            //var slowStacks = stacks.FindAll(item => item.SecondBatchName == canLoading.BatchName);
            //if (slowStacks.Count > 0)
            //{
            //    stack = slowStacks.FirstOrDefault(item => item.PlanDeviceName == deviceName && item.Status == Convert.ToInt32(StackStatus.Stocked)) ?? slowStacks.OrderByDescending(item => item.FirstBatchBookCount).First();
            //}
            //else
            //{
            //    var curBatchStacks = stacks.FindAll(item => item.FirstBatchName == canLoading.BatchName);
            //    //当前批次
            //    if (curBatchStacks.Count == 0)
            //    {
            //        var nextBatchName = canLoading.NextBatchName;
            //        if (string.IsNullOrWhiteSpace(nextBatchName))
            //        {
            //            var nextGroup = canLoadings.FindAll(item => item.GroupId > canLoading.GroupId).OrderBy(item => item.GroupId).ThenBy(item => item.BatchIndex)
            //                .FirstOrDefault();
            //            if (nextGroup == null) return;
            //            nextBatchName = nextGroup.BatchName;
            //        }

            //        var matchStacks = stacks.FindAll(item => item.FirstBatchName == nextBatchName);

            //        if (matchStacks.Count == 0) return;

            //        stack = matchStacks.FirstOrDefault(item => item.PlanDeviceName == deviceName && item.Status == Convert.ToInt32(StackStatus.Stocked)) ?? matchStacks.FindAll(item=> item.Status == Convert.ToInt32(StackStatus.Stocked)).OrderByDescending(item => item.FirstBatchBookCount).First();
            //    }
            //    else
            //    {
            //        stack = curBatchStacks.FirstOrDefault(item => item.PlanDeviceName == deviceName && item.Status==Convert.ToInt32(StackStatus.Stocked)) ?? curBatchStacks.FindAll(item => item.Status == Convert.ToInt32(StackStatus.Stocked)).OrderByDescending(item => item.FirstBatchBookCount).FirstOrDefault();
            //        if(stack==null) return;
            //    }

            //}

            //stack.ActualDeviceName = deviceName;
            ////发起上料请求
            //stack.Status = Convert.ToInt32(StackStatus.WaitMaterial);
            //stack.UpdatedTime = DateTime.Now;

            //CuttingStackProductionList cutting = new CuttingStackProductionList {LastUpdatedTime = DateTime.Now,PlaceNo = deviceInfo.PlaceNo,ProductionLine = "",Status = Convert.ToInt32(RequestLoadingStatus.RequestLoad),StackName = stack.StackName};

            //var result = Contract.BulkUpdatedStacks(new List<Stack> { stack });
            //if (result)
            //{
            //    Contract.BulkInsertCuttingStackProductionList(new List<CuttingStackProductionList>() {cutting});
            //}

            #endregion
        }

        private void RequestLoad(Stack stack,string deviceName,DeviceInfos deviceInfo)
        {
            stack.ActualDeviceName = deviceName;
            //发起上料请求
            stack.Status = Convert.ToInt32(StackStatus.WaitMaterial);
            stack.UpdatedTime = DateTime.Now;

            CuttingStackProductionList cutting = new CuttingStackProductionList { LastUpdatedTime = DateTime.Now, PlaceNo = deviceInfo.PlaceNo, ProductionLine = "", Status = Convert.ToInt32(RequestLoadingStatus.RequestLoad), StackName = stack.StackName };

            //var result = Contract.BulkUpdatedStacks(new List<Stack> { stack });
            var result = StackManager.SaveStacks(new List<Stack> { stack });
            SendMsg("垛", "更新状态", $"垛号：{cutting.StackName},状态更新为：请求上料");
            if (result)
            {
                Contract.BulkInsertCuttingStackProductionList(new List<CuttingStackProductionList>() { cutting });
                SendMsg("Wms", "上料请求", $"设备：{deviceName}发起上料请求，垛号：{cutting.StackName}");
            }
        }
    }
}
