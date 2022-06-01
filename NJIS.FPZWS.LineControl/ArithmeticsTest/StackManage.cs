using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.UI.Common.Message;

namespace ArithmeticsTest
{
    /// <summary>
    /// 垛管理
    /// </summary>
    public class StackManage:IStackManage
    {
        private static readonly object CreatedLock = new object();
        private static StackManage _stackManage = null;

        private readonly ILineControlCuttingContract _lineControlCuttingContract =
            WcfClient.GetProxy<ILineControlCuttingContract>();
        private DateTime LastUpdatedTime { get; set; } = DateTime.Now;
        private StackManage()
        {
            var startTime = DateTime.Now;
            CommandMsg startCommandMsg = new CommandMsg() { StartTime = startTime, FinishedTime = startTime, CommandName = "开始加载垛数据" };
            BroadcastMessage.Send(nameof(CommandMsg), startCommandMsg);
            //AutoUpdated(DateTime.Today.AddDays(-1));

            var stacks = _lineControlCuttingContract.GetStacksByUpdatedTime(DateTime.Today.AddDays(-1));
            BroadcastMessage.Send(nameof(CommandMsg), new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "加载垛数据完成" });
            var updates = UpdatedStacks(stacks, false);
            BroadcastMessage.Send(nameof(CommandMsg), new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新垛数据完成" });
            foreach (var group in updates.GroupBy(item => item.StackName))
            {
                var details = _lineControlCuttingContract.GetStackDetailsByStackName(group.Key);
                UpdatedStackDetails(details, false);
            }
            BroadcastMessage.Send(nameof(CommandMsg), new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新垛明细完成" });
            LastUpdatedTime = DateTime.Now;

            var finishedTime = DateTime.Now;
            CommandMsg commandMsg = new CommandMsg() { StartTime = startTime, FinishedTime = finishedTime, CommandName = "加载垛数据数据" };
            BroadcastMessage.Send(nameof(CommandMsg), commandMsg);
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
        private readonly object ObjLock = new object();
        private readonly List<Stack> Stacks = new List<Stack>();
        private readonly List<StackDetail> StackDetails = new List<StackDetail>();


        private void AutoUpdated(DateTime minUpdatedTime)
        {
            var stacks = _lineControlCuttingContract.GetStacksByUpdatedTime(minUpdatedTime);
            var updates = UpdatedStacks(stacks,false);
            foreach (var group in updates.GroupBy(item => item.StackName))
            {
                var details = _lineControlCuttingContract.GetStackDetailsByStackName(group.Key);
                UpdatedStackDetails(details,false);
            }
            LastUpdatedTime = DateTime.Now;
        }

        private List<Stack> UpdatedStacks(List<Stack> stacks,bool needSave=true)
        {
            lock (ObjLock)
            {
                var updated = Stacks.Join(stacks, s1 => s1.LineId, s2 => s2.LineId,
                    (s1, s2) => s1.UpdatedTime > s2.UpdatedTime ? s1 : s2).ToList();

                var newItems = stacks.FindAll(item => !Stacks.Exists(i => i.LineId == item.LineId));

                Stacks.RemoveAll(item => updated.Exists(i => i.LineId == item.LineId));
                Stacks.AddRange(updated);
                Stacks.AddRange(newItems);
                if (needSave)
                {
                    _lineControlCuttingContract.BulkUpdatedStacks(updated);
                }

                return updated;
            }
        }

        private bool UpdatedStackDetails(List<StackDetail> stackDetails, bool needSave = true)
        {
            lock (ObjLock)
            {
                var updated = StackDetails.Join(stackDetails, s1 => s1.LineId, s2 => s2.LineId,
                    (s1, s2) => s1.UpdatedTime > s2.UpdatedTime ? s1 : s2).ToList();

                var newItems = stackDetails.FindAll(item => !StackDetails.Exists(i => i.LineId == item.LineId));

                StackDetails.RemoveAll(item => updated.Exists(i => i.LineId == item.LineId));
                StackDetails.AddRange(updated);
                StackDetails.AddRange(newItems);
                if (needSave)
                {
                    return _lineControlCuttingContract.BulkUpdatedStackDetails(updated);
                }

                return true;
            }
        }

        public List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate)
        {
            lock (ObjLock)
            {
                return StackDetails.FindAll(item => item.PlanDate == planDate);
            }
        }

        public List<StackDetail> GetStackDetailsByStackName(string stackName)
        {
            lock (ObjLock)
            {
                return StackDetails.FindAll(item => item.StackName == stackName);
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

        public List<Stack> GetStacksByDeviceName(string deviceName)
        {
            lock (ObjLock)
            {
                return Stacks.FindAll(item => item.PlanDeviceName == deviceName);
            }
        }

        public Stack GetStackByStackName(string stackName)
        {
            lock (ObjLock)
            {
                return Stacks.FirstOrDefault(item => item.StackName == stackName);
            }
        }

        public List<Stack> GetStacksByStatus(StackStatus stackStatus)
        {
            lock (ObjLock)
            {
                return Stacks.FindAll(item => item.Status == Convert.ToInt32(stackStatus));
            }
        }

        public List<Stack> GetStacksByPlanDate(DateTime planDate)
        {
            lock (ObjLock)
            {
                return Stacks.FindAll(item => item.PlanDate == planDate);
            }
        }
    }
}
