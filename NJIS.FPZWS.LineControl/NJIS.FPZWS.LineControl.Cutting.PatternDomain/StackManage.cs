using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    /// <summary>
    /// 垛管理
    /// </summary>
    public class StackManage:IStackManage
    {
        private static readonly object CreatedLock = new object();
        private static StackManage _stackManage = null;
        private ILineControlCuttingContractPlus _contract = null;
        private ILineControlCuttingContractPlus Contract => _contract??(_contract=
            WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private DateTime LastUpdatedTime { get; set; } = DateTime.Now;
        private StackManage()
        {
            var startTime = DateTime.Now;
            CommandMsg startCommandMsg = new CommandMsg() { StartTime = startTime, FinishedTime = startTime, CommandName = "开始加载垛数据" };
            SendMsg(startCommandMsg);
            //AutoUpdated(DateTime.Today.AddDays(-1));

            var stacks = Contract.GetUnfinishedStacks();
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "加载垛数据完成" });
            var updates = UpdatedStacks(stacks, false);
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新垛数据完成" });
            foreach (var group in stacks.GroupBy(item => item.StackName))
            {
                var details = Contract.GetStackDetailsByStackName(group.Key);
                UpdatedStackDetails(details, false);
            }
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新垛明细完成" });
            LastUpdatedTime = DateTime.Now;

            var finishedTime = DateTime.Now;
            CommandMsg commandMsg = new CommandMsg() { StartTime = startTime, FinishedTime = finishedTime, CommandName = "加载垛数据数据" };
            SendMsg(commandMsg);
            Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (DateTime.Now > LastUpdatedTime.AddSeconds(30))
                        {
                            AutoUpdated(DateTime.Now.AddMinutes(-20));
                        }
                        Thread.Sleep(20);
                    }
                })
                { IsBackground = true };
            thread.Start();
        }

        private void SendMsg(CommandMsg commandMsg)
        {
            //BroadcastMessage.Send(nameof(CommandMsg), commandMsg);
        }

        public static StackManage GetInstance()
        {
            if (_stackManage == null)
            {
                lock (CreatedLock)
                {
                    if (_stackManage == null)
                    {
                        _stackManage = new StackManage();
                    }
                }
            }

            return _stackManage;
        }
        private readonly object _objLock = new object();
        private readonly List<Stack> _stacks = new List<Stack>();
        private readonly List<StackDetail> _stackDetails = new List<StackDetail>();


        private void AutoUpdated(DateTime minUpdatedTime)
        {
            var stacks = Contract.GetStacksByUpdatedTime(minUpdatedTime);
            var updates = UpdatedStacks(stacks,false);
            foreach (var group in updates.GroupBy(item => item.StackName))
            {
                var details = Contract.GetStackDetailsByStackName(group.Key);
                UpdatedStackDetails(details,false);
            }
            LastUpdatedTime = DateTime.Now;
        }

        private List<Stack> UpdatedStacks(List<Stack> stacks,bool needSave=true)
        {
            lock (_objLock)
            {
                var updated = _stacks.Join(stacks, s1 => s1.LineId, s2 => s2.LineId,
                    (s1, s2) => s1.UpdatedTime > s2.UpdatedTime ? s1 : s2).ToList();

                var newItems = stacks.FindAll(item => !_stacks.Exists(i => i.LineId == item.LineId));

                _stacks.RemoveAll(item => updated.Exists(i => i.LineId == item.LineId));
                _stacks.AddRange(updated);
                _stacks.AddRange(newItems);
                if (needSave)
                {
                    Contract.BulkUpdatedStacks(updated);
                }

                return updated;
            }
        }

        private bool UpdatedStackDetails(List<StackDetail> stackDetails, bool needSave = true)
        {
            lock (_objLock)
            {
                var updated = _stackDetails.Join(stackDetails, s1 => s1.LineId, s2 => s2.LineId,
                    (s1, s2) => s1.UpdatedTime > s2.UpdatedTime ? s1 : s2).ToList();

                var newItems = stackDetails.FindAll(item => !_stackDetails.Exists(i => i.LineId == item.LineId));

                _stackDetails.RemoveAll(item => updated.Exists(i => i.LineId == item.LineId));
                _stackDetails.AddRange(updated);
                _stackDetails.AddRange(newItems);
                if (needSave)
                {
                    return Contract.BulkUpdatedStackDetails(updated);
                }

                return true;
            }
        }

        public List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate)
        {
            lock (_objLock)
            {
                return _stackDetails.FindAll(item => item.PlanDate == planDate);
            }
        }

        public List<StackDetail> GetStackDetailsByStatus(BookStatus bookStatus)
        {
            lock (_objLock)
            {
                return _stackDetails.FindAll(item => item.Status==bookStatus.GetHashCode());
            }
        }


        public List<StackDetail> GetStackDetailsByStackName(string stackName)
        {
            lock (_objLock)
            {
                return _stackDetails.FindAll(item => item.StackName == stackName);
            }
        }

        public bool SaveStacks(List<Stack> stacks)
        {
            return UpdatedStacks(stacks).Count>0;
            
        }

        public bool SaveStackDetails(List<StackDetail> stackDetails)
        {
            return UpdatedStackDetails(stackDetails);
            
        }

        public void RemoveFinished(DateTime planDate)
        {
            lock (_objLock)
            {
                if (_stacks.Exists(item => item.PlanDate < planDate.Date))
                {
                    _stacks.RemoveAll(item => item.PlanDate < planDate.Date);
                }

                if (_stackDetails.Exists(item => item.PlanDate < planDate.Date))
                {
                    _stackDetails.RemoveAll(item => item.PlanDate < planDate.Date);
                }
            }
        }

        public List<Stack> GetStacksByDeviceName(string deviceName)
        {
            lock (_objLock)
            {
                return _stacks.FindAll(item => item.ActualDeviceName == deviceName);
            }
        }

        public Stack GetStackByStackName(string stackName)
        {
            lock (_objLock)
            {
                return _stacks.FirstOrDefault(item => item.StackName == stackName);
            }
        }

        public List<Stack> GetStacksByStatus(StackStatus stackStatus)
        {
            lock (_objLock)
            {
                return _stacks.FindAll(item => item.Status == Convert.ToInt32(stackStatus));
            }
        }

        public List<Stack> GetStacksByPlanDate(DateTime planDate)
        {
            lock (_objLock)
            {
                return _stacks.FindAll(item => item.PlanDate == planDate);
            }
        }
    }
}
