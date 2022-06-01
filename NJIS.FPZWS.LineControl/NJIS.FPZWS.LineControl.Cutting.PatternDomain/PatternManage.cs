using Arithmetics;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    /// <summary>
    /// 锯切图管理
    /// </summary>
    public class PatternManage: IPatternDistribute
    {
        private  readonly object ObjLock = new object();
        //private  readonly object DisintegratedLock = new object();
        private  List<Pattern> Patterns { get; set; }  = new List<Pattern>();
        private  List<PatternDetail> PatternDetails { get; set; } = new List<PatternDetail>();
        private ILineControlCuttingContractPlus _contract = null;
        private  ILineControlCuttingContractPlus Contract => _contract??(_contract=WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        //private  readonly List<DisintegratedTask> DisintegratedTasks = new List<DisintegratedTask>();

        private static readonly object CreatedLock = new object();
        private static PatternManage _patternManage = null;

        private DateTime LastUpdatedTime { get; set; }=DateTime.Now;

        private PatternManage()
        {
            var startTime = DateTime.Now;
            CommandMsg startCommandMsg = new CommandMsg() { StartTime = startTime, FinishedTime = startTime, CommandName = "开始加载锯切图数据" };
            SendMsg(startCommandMsg);
            //AutoUpdated(DateTime.Today.AddDays(-1));


            var patterns = Contract.GetUnfinishedPatterns();
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "开始更新锯切图数据" });
            var updates = UpdatedPatterns(patterns, false);
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新锯切图数据完成" });
            foreach (var group in patterns.GroupBy(item => item.BatchName))
            {
                var details = Contract.GetPatternDetailsByBatchName(group.Key);
                UpdatedPatternDetails(details, false);
            }
            SendMsg(new CommandMsg() { StartTime = DateTime.Now, FinishedTime = DateTime.Now, CommandName = "更新锯切图明细完成" });
            LastUpdatedTime = DateTime.Now;
            var finishedTime = DateTime.Now;
            CommandMsg commandMsg = new CommandMsg(){StartTime = startTime,FinishedTime = finishedTime, CommandName = "加载锯切图数据"};
            SendMsg(commandMsg);
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    //AutoDisintegrated();
                    //AutoUpdatedDisintegrated();

                    if (DateTime.Now > LastUpdatedTime.AddSeconds(30))
                    {
                        AutoUpdated(DateTime.Now.AddMinutes(-2));
                        AutoSave();
                    }
                    Thread.Sleep(20);
                }
            }){IsBackground = true};
            thread.Start();
        }

        private void SendMsg(CommandMsg commandMsg)
        {
            //BroadcastMessage.Send(nameof(CommandMsg), commandMsg);
        }

        public static PatternManage GetInstance()
        {
            if (_patternManage == null)
            {
                lock (CreatedLock)
                {
                    if (_patternManage == null)
                    {
                        _patternManage = new PatternManage();
                    }
                }
            }

            return _patternManage;
        }

        /// <summary>
        /// 是否正在拆解
        /// </summary>
        private  bool IsDisintegrating { get; set; } = false;

        private  void AutoDisintegrated()
        {
            //if (IsDisintegrating)
            //{
            //    var task = DisintegratedTasks.FirstOrDefault(item => !item.IsFinished);
            //    if(task==null) return;
            //    var success = Starter.DisintegratedMdb(task.Pattern, task.Details, task.CurrentMaxPatternId, task.MaxBookCount);
            //    if (success)
            //    {
            //        task.IsFinished = true;
            //    }

            //    IsDisintegrating = false;
            //}
        }

        private  void AutoUpdatedDisintegrated()
        {
            //var updatedTask = DisintegratedTasks.FirstOrDefault(item => item.IsFinished && !item.IsUpdated);
            //if (updatedTask != null)
            //{
            //    var details = Contract.GetPatternDetailsByBatchName(updatedTask.Pattern.BatchName);
            //    var patterns = Contract.GetPatternsByBatchName(updatedTask.Pattern.BatchName);

            //    UpdatedPatterns(patterns);
            //    UpdatedPatternDetails(details);
            //    updatedTask.IsUpdated = true;
            //}
        }

        private void AutoUpdated(DateTime minUpdatedTime)
        {
            var patterns = Contract.GetPatternsByUpdatedTime(minUpdatedTime);
            var updates =UpdatedPatterns(patterns,false);
            foreach (var group in updates.GroupBy(item => item.BatchName))
            {
                var details = Contract.GetPatternDetailsByBatchName(group.Key);
                UpdatedPatternDetails(details,false);
            }
            LastUpdatedTime = DateTime.Now;
        }

        private void AutoSave()
        {
            lock (ObjLock)
            {
                Contract.BulkUpdateNewPatterns(Patterns);
                Contract.BulkUpdateNewPatternDetails(PatternDetails);
            }
        }

        private  void UpdatedPattern(Pattern sourcePattern, Pattern destPattern)
        {

            destPattern.UpdatedTime = sourcePattern.UpdatedTime;
            destPattern.Status = sourcePattern.Status;
            destPattern.DeviceName = sourcePattern.DeviceName;
            destPattern.IsEnable = sourcePattern.IsEnable;
            destPattern.IsNodePattern = sourcePattern.IsNodePattern;
            destPattern.OldPatternId = sourcePattern.OldPatternId;
            destPattern.PatternId = sourcePattern.PatternId;
            destPattern.PlanDate = sourcePattern.PlanDate;
        }

        private bool UpdatedPatternDetails(List<PatternDetail> patternDetails, bool needSave = true)
        {
            lock (ObjLock)
            {
                var updates = PatternDetails.Join(patternDetails, oldPatternDetail => oldPatternDetail.LineId,
                    newPatternDetail => newPatternDetail.LineId, (p1, p2) => p1.UpdatedTime > p2.UpdatedTime ? p1 : p2).ToList();
                var newPatternDetails =
                    patternDetails.FindAll(item => !PatternDetails.Exists(i => i.LineId == item.LineId));

                PatternDetails.RemoveAll(item => updates.Exists(i => i.LineId == item.LineId));
                updates.AddRange(newPatternDetails);
                PatternDetails.AddRange(updates);
                if (needSave)
                {
                    return Contract.BulkUpdatePatternDetails(updates);
                }

                return true;
            }
        }

        public bool SavePatterns(List<Pattern> patterns)
        {
            return UpdatedPatterns(patterns).Count>0;
            //return true;
            //return _lineControlCuttingContract.BulkUpdatePatterns(patterns);
        }

        public bool SavePatternDetails(List<PatternDetail> patternDetails)
        {
            return UpdatedPatternDetails(patternDetails);
            //return _lineControlCuttingContract.BulkUpdatePatternDetails(patternDetails);

        }


        /// <summary>
        /// 更新锯切图数据
        /// </summary>
        /// <param name="patterns"></param>
        private List<Pattern> UpdatedPatterns(List<Pattern> patterns, bool needSave = true)
        {
            lock (ObjLock)
            {
                var updates = Patterns.Join(patterns, oldPattern => oldPattern.LineId,
                    newPattern => newPattern.LineId, (p1, p2) => p1.UpdatedTime > p2.UpdatedTime ? p1 : p2).ToList();
                var newPatterns =
                    patterns.FindAll(item => !Patterns.Exists(i => i.LineId == item.LineId));

                Patterns.RemoveAll(item => updates.Exists(i => i.LineId == item.LineId));
                updates.AddRange(newPatterns);
                Patterns.AddRange(updates);
                
                var batchStatus = Patterns.GroupBy(item => item.BatchName, p => p,
                    (batchName, tPatterns) => new
                    { BatchName = batchName, AvgStatus = tPatterns.Average(item => item.Status), LastUpdateTime = tPatterns.OrderByDescending(item => item.UpdatedTime).First().UpdatedTime }).ToList();
                var tBatchStatus = batchStatus.FindAll(item => item.AvgStatus >= Convert.ToInt32(PatternStatus.Cut));
                _finishedBatchs = tBatchStatus.ToList().ConvertAll(item => item.BatchName);
                _cuttingPatterns = Patterns.FindAll(item =>
                    item.Status >= Convert.ToInt32(PatternStatus.UndistributedButUnLoad) &&
                    item.Status <= Convert.ToInt32(PatternStatus.Cutting));
                if (needSave)
                {
                    Contract.BulkUpdatePatterns(updates);
                }

                return updates;
            }
        }

        public string CurrentBatchName
        {
            get
            {
                lock (ObjLock)
                {
                    var batchStatus = Patterns.GroupBy(item => item.BatchName, p => p,
                        (batchName, patterns) => new
                            { BatchName = batchName, AvgStatus = patterns.Average(item => item.Status) }).ToList();
                    var tBatchStatus = batchStatus.FindAll(item =>
                        item.AvgStatus < Convert.ToInt32(PatternStatus.Cut));
                    if (tBatchStatus.Any(item => item.AvgStatus > 0))
                    {
                        return tBatchStatus.OrderByDescending(item => item.AvgStatus).First().BatchName;
                    }

                    return string.Empty;
                }
            }
        }

        private Dictionary<string, List<PatternDetail>> PatternDetailDictionary { get; } = new Dictionary<string, List<PatternDetail>>();

        public List<Pattern> GetPatternsByPlanDate(DateTime planDate)
        {
            return Patterns.FindAll(item => item.PlanDate == planDate);
        }

        public List<PatternDetail> GetPatternDetailsByBatchName(string batchName)
        {

            return PatternDetails.FindAll(item => item.BatchName == batchName);

            //if (!PatternDetailDictionary.ContainsKey(batchName))
            //{
            //    var tPatternDetails = PatternDetails.FindAll(item => item.BatchName == batchName);
            //    PatternDetailDictionary.Add(batchName,tPatternDetails);
            //}

            //return PatternDetailDictionary[batchName];
        }

        public List<Pattern> GetPatternsByDeviceName(string deviceName)
        {
            return Patterns.FindAll(item => item.DeviceName == deviceName);
        }

        private List<Pattern> _cuttingPatterns = new List<Pattern>();

        public List<Pattern> CuttingPatterns
        {
            get
            {
                lock (ObjLock)
                {
                    return _cuttingPatterns;
                }
            }
        }

        private List<string> _finishedBatchs  = new List<string>();

        public List<string> FinishedBatchs
        {
            get
            {
                lock (ObjLock)
                {
                    return _finishedBatchs;
                }
                //var batchStatus = Patterns.GroupBy(item => item.BatchName, p => p,
                //    (batchName, patterns) => new
                //        { BatchName = batchName, AvgStatus = patterns.Average(item =>item.Status), LastUpdateTime = patterns.OrderByDescending(item => item.UpdatedTime).First().UpdatedTime }).ToList();
                //var tBatchStatus = batchStatus.FindAll(item => item.AvgStatus >= Convert.ToInt32(PatternStatus.Cut));
                //return tBatchStatus.ToList().ConvertAll(item => item.BatchName);
            }
        }

        public string LastFinishedBatchName
        {
            get
            {
                var batchStatus = Patterns.GroupBy(item => item.BatchName, p => p,
                    (batchName, patterns) => new
                        { BatchName = batchName, AvgStatus = patterns.Average(item => item.Status),LastUpdateTime=patterns.OrderByDescending(item=>item.UpdatedTime).First().UpdatedTime }).ToList();
                var tBatchStatus = batchStatus.FindAll(item => item.AvgStatus >= Convert.ToInt32(PatternStatus.Cut));
                if (tBatchStatus.Any())
                {
                    return tBatchStatus.OrderByDescending(item => item.LastUpdateTime).First().BatchName;
                }
                return string.Empty;
            }
        }


        internal class DisintegratedTask
        {
            public Pattern Pattern { get; set; }
            public List<PatternDetail> Details { get; set; }
            public int CurrentMaxPatternId { get; set; }
            public int MaxBookCount { get; set; }
            public bool IsFinished { get; set; } = false;
            public bool IsUpdated { get; set; } = false;
        }

        //public Pattern DistributePattern(string batchName, string stackName, int maxBookCount, string color)
        //{
        //    if (maxBookCount <= 0) return null;
        //    lock (ObjLock)
        //    {
                

        //        var matchPatterns =
        //            Patterns.FindAll(item => item.IsEnable && item.PlanStackName == stackName && item.Status == Convert.ToInt32(PatternStatus.Undistributed) && item.BatchName == batchName && item.Color == color && item.BookCount <= maxBookCount);
        //    }
        //}

        private int GetActuallyTime(List<Pattern> patterns,int times=50)
        {
            var tPatterns = patterns.OrderBy(item => item.StartTime).ToList();
            int totallyTime = 0;
            var pList = SpiltPatterns(tPatterns, times, 10 * 60);

            foreach (var ps in pList)
            {
                if (ps.Count > 1)
                {
                    var tList = ps.OrderBy(item => item.StartTime).ToList();
                    var startTime = tList.First().StartTime;
                    var endTime = tList.Last().FinishedTime;
                    var t = (endTime - startTime).Value.TotalMilliseconds * times * 1.0 / 1000;
                    totallyTime += Convert.ToInt32(t);
                }
                else
                {
                    totallyTime += ps.First().ActuallyTotalTime;
                }
                
            }

            return totallyTime;
        }

        private List<List<Pattern>> SpiltPatterns(List<Pattern> patterns,int times,int maxSeconds)
        {
            var ttPatterns = patterns.OrderBy(item => item.StartTime).ToList();
            List<List<Pattern>> pList = new List<List<Pattern>>();
            List<Pattern> tPatterns = new List<Pattern>(); 
            for (int i = 0; i < ttPatterns.Count-1; i++)
            {
                var current = ttPatterns[i];
                tPatterns.Add(current);
                if (!pList.Contains(tPatterns))
                {
                    pList.Add(tPatterns);
                }
                var next = ttPatterns[i + 1];
                var time = next.StartTime - current.StartTime.Value.AddSeconds(current.ActuallyTotalTime);
                if (time.HasValue)
                {
                    if (time.Value.TotalMilliseconds * times * 1.0 / 1000 > maxSeconds)
                    {
                        tPatterns = new List<Pattern>();
                    }
                    tPatterns.Add(next);
                }

                if (!pList.Contains(tPatterns))
                {
                    pList.Add(tPatterns);
                }
            }

            return pList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="times">时间放大倍数</param>
        public string SwapPattern(string batchName, int times = 50)
        {
            lock (ObjLock)
            {
                int topMin = 600;
                int totallyMm = Convert.ToInt32(topMin * 60 * 1000 * 1.0 / times);
                var minTime = DateTime.Now.AddMilliseconds(0 - totallyMm);
                var tPatterns = Patterns.FindAll(item =>item.FinishedTime>DateTime.Today &&
                    item.StartTime >= minTime && item.IsEnable && item.Status == Convert.ToInt32(PatternStatus.Cut));
                tPatterns.RemoveAll(item => item.ActuallyTotalTime == 0  || item.ActuallyTotalTime > item.TotallyTime*8 || item.ActuallyTotalTime < item.TotallyTime / 8.0);
                //tPatterns.RemoveAll(item =>
                //{
                //    if (!item.StartTime.HasValue) return true;
                //    var time = (item.FinishedTime - item.StartTime);
                //    if (time.HasValue)
                //    {
                //        var isRemove = time.Value.TotalMilliseconds * times > item.TotallyTime * 1000 * 5
                //                       || time.Value.TotalMilliseconds * times < item.TotallyTime * 1000 / 5;
                //        return isRemove;
                //    }
                //     else
                //     {
                //         return true;
                //     }
                //});
                if (tPatterns.Count < 15)
                {
                    return $"批次;{batchName}，当前完成且符合条件的锯切图数量为：{tPatterns.Count},不足数量";
                }
                var tt = tPatterns.GroupBy(item => item.ActualDeviceName, (deviceName, ps) => new
                {
                    DeviceName = deviceName,
                    ActuallyTime = GetActuallyTime(ps.ToList(), times),
                    PlanTotalTime = ps.Sum(it=>it.TotallyTime)

                });

                Dictionary<string, int> stackTimeDictionary = new Dictionary<string, int>();
                Dictionary<string, double> devTimeAvailability = new Dictionary<string, double>();
                foreach (var t in tt)
                {
                    stackTimeDictionary.Add(t.DeviceName,t.ActuallyTime);
                    devTimeAvailability.Add(t.DeviceName,t.ActuallyTime*1.0/t.PlanTotalTime);
                }

                var stacksTime = Patterns.FindAll(item => item.BatchName == batchName && item.IsEnable).GroupBy(item => item.ActualDeviceName, p => p,
                    (actualDeviceName, ps) =>
                    {
                        var planTotallyTime = ps.Sum(item => item.TotallyTime);
                        var actuallyTime = stackTimeDictionary.ContainsKey(actualDeviceName) ? stackTimeDictionary[actualDeviceName] : 0;
                        var finishedPatterns = ps.Where(item => item.Status == Convert.ToInt32(PatternStatus.Cut)).ToList();
                        var remainingPatterns =
                            ps.Where(item => item.Status == Convert.ToInt32(PatternStatus.Undistributed)).ToList();
                        var planRemainingTime = remainingPatterns.Sum(item => item.TotallyTime);
                        var planDistributeTime = finishedPatterns.Sum(item => item.TotallyTime);
                        return new
                        {
                            ActualDeviceName = actualDeviceName,
                            //计划总时间
                            PlanTotallyTime = planTotallyTime,
                            //计划剩余时间
                            PlanRemainingTime = planRemainingTime,
                            PlanRemainingPatterns = remainingPatterns,
                            //已完成的锯切图的计划分配时间
                            PlanDistributeTime = planDistributeTime,
                            FinishedPatternCount = finishedPatterns.Count,
                            ActuallyTime = actuallyTime,
                            //时间利用率
                            TimeAvailability = devTimeAvailability.ContainsKey(actualDeviceName)? devTimeAvailability[actualDeviceName]:0,
                            //TimeAvailability = actuallyTime*1.0/ planDistributeTime,
                            //预计剩余时间
                            EstimateTime = devTimeAvailability.ContainsKey(actualDeviceName) ? planRemainingTime * devTimeAvailability[actualDeviceName] : planRemainingTime 
                        };
                    }).ToList();

                if (!stacksTime.Exists(item => item.FinishedPatternCount > 0))
                {
                    return $"批次：{ batchName},没有未开始开料，不执行调配";
                }

                var unBeginStacksTime = stacksTime.FindAll(item => item.ActuallyTime==0);
                var beginStacksTime = stacksTime.FindAll(item => item.ActuallyTime > 0);
                if (unBeginStacksTime.Count > 0 && beginStacksTime.Count>0)
                {
                    var maxTimeAvailability = beginStacksTime.Max(item => item.TimeAvailability);
                    foreach (var stackTime in unBeginStacksTime)
                    {
                        var tmp = new
                        {
                            ActualDeviceName = stackTime.ActualDeviceName,
                            PlanTotallyTime = stackTime.PlanTotallyTime,
                            PlanRemainingTime = stackTime.PlanRemainingTime,
                            PlanRemainingPatterns = stackTime.PlanRemainingPatterns,
                            PlanDistributeTime = stackTime.PlanDistributeTime,
                            FinishedPatternCount = stackTime.FinishedPatternCount,
                            ActuallyTime = stackTime.ActuallyTime,
                            TimeAvailability = maxTimeAvailability,
                            EstimateTime = stackTime.PlanRemainingTime * maxTimeAvailability
                        };
                        stacksTime.Remove(stackTime);
                        stacksTime.Add(tmp);
                    }
                }

                stacksTime = stacksTime.OrderByDescending(item => item.EstimateTime).ToList();

                var maxStack = stacksTime.First();
                var minStack = stacksTime.Last();
                double pp = maxStack.EstimateTime - minStack.EstimateTime;

                var swapTriggerMinTime = PatternAppSetting.Current.SwapTriggerMinTime;
                if (pp >= swapTriggerMinTime)
                {
                    var tStacks = stacksTime
                        .FindAll(item => maxStack.EstimateTime - item.EstimateTime >= swapTriggerMinTime)
                        .OrderBy(item => item.EstimateTime).ToList();
                    foreach (var tStack in tStacks)
                    {
                        minStack = tStack;
                        if (minStack.FinishedPatternCount == 0 && maxStack.FinishedPatternCount <= 3)
                        {
                            continue;
                        }

                        var p = maxStack.EstimateTime - minStack.EstimateTime;

                        var matchs = maxStack.PlanRemainingPatterns.Join(minStack.PlanRemainingPatterns,
                        max => new { max.Color, max.BookCount }, min => new { min.Color, min.BookCount },
                        (max, min) => new PatternMatch { MaxPattern = max, MinPattern = min, PP = Math.Abs((max.TotallyTime-min.TotallyTime) * (maxStack.TimeAvailability + minStack.TimeAvailability) - p) });//.Where(item => item.PP > 0);
                        
                        //最小偏差值
                        var minP = (p - 600) * maxStack.TimeAvailability * minStack.TimeAvailability /
                                (maxStack.TimeAvailability + maxStack.TimeAvailability);

                        var match = matchs.Where(item => item.PP <= swapTriggerMinTime).OrderBy(item => item.PP).FirstOrDefault();
                        if (match != null)
                        {
                            SwapPattern(match.MaxPattern, match.MinPattern);
                            List<Pattern> updates = new List<Pattern>();
                            updates.Add(match.MaxPattern);
                            updates.Add(match.MinPattern);
                            Contract.BulkUpdatePatterns(updates);

                            maxStack.PlanRemainingPatterns.Remove(match.MaxPattern);
                            maxStack.PlanRemainingPatterns.Add(match.MinPattern);

                            minStack.PlanRemainingPatterns.Remove(match.MinPattern);
                            minStack.PlanRemainingPatterns.Add(match.MaxPattern);
                            return $"微调--批次：{batchName}--垛号：{match.MaxPattern.ActuallyStackName}与垛号：{match.MinPattern.ActuallyStackName}交换锯切图[{match.MaxPattern.MdbName}]<--->[{match.MinPattern.MdbName}]";
                        }

                        var tMatchs = maxStack.PlanRemainingPatterns.GroupJoin(minStack.PlanRemainingPatterns, max => max.Color,
                            min => min.Color, (max, minPatterns) =>
                            {
                                var bookCount = max.BookCount;
                                var combinations = CombinationHelper.Combination(2, minPatterns).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i => i.ActuallyStackName).Count() == 1);
                                if (bookCount >= 3)
                                {
                                    var list2 = CombinationHelper.Combination(3, minPatterns).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i => i.ActuallyStackName).Count() == 1);
                                    combinations.AddRange(list2);
                                }
                                if (bookCount == 4)
                                {
                                    var list3 = CombinationHelper.Combination(4, minPatterns.Where(item => item.BookCount == 1)).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i=>i.ActuallyStackName).Count()==1);
                                    combinations.AddRange(list3);
                                }

                                var tCombinations = combinations.ConvertAll(combination => new PatternsMatchBase
                                {
                                    Combination = combination,
                                    PatternCount = combination.CombinationCollection.Count,
                                    PP = Math.Abs((max.TotallyTime - combination.CombinationCollection.Sum(item => item.TotallyTime))*(maxStack.TimeAvailability + minStack.TimeAvailability) - p)
                                });//.FindAll(item => item.PP > 0);

                                return new { MaxPattern = max, MinPatterns = minPatterns, Combinations = tCombinations };
                            }).SelectMany(item => item.Combinations, (t, ts) => new PatternsMatch
                            {
                                MaxPattern=t.MaxPattern,
                                Combination=ts.Combination,
                                PP=ts.PP,
                                PatternCount=ts.PatternCount
                            }); 

                        var ttMatch = tMatchs.Where(item => item.PP <= swapTriggerMinTime).OrderBy(item => item.PP).ThenBy(item => item.PatternCount).FirstOrDefault();
                        if (ttMatch != null)
                        {
                            var list = ttMatch.Combination.CombinationCollection;
                            var maxPattern = ttMatch.MaxPattern;
                            SwapPattern(maxPattern, list);
                            List<Pattern> updates = new List<Pattern>();
                            updates.Add(maxPattern);
                            updates.AddRange(list);
                            Contract.BulkUpdatePatterns(updates);
                            maxStack.PlanRemainingPatterns.Remove(maxPattern);
                            maxStack.PlanRemainingPatterns.AddRange(list);

                            minStack.PlanRemainingPatterns.RemoveAll(list.Contains);
                            minStack.PlanRemainingPatterns.Add(maxPattern);
                            return $"微调--批次：{batchName}--垛号：{ttMatch.MaxPattern.ActuallyStackName}与垛号：{ttMatch.Combination.CombinationCollection.First().ActuallyStackName}交换锯切图[{ttMatch.MaxPattern.MdbName}]<--->[{string.Join(",", list.ConvertAll(item=>item.MdbName))}]"; 
                        }
                        
                    }

                    foreach (var tStack in tStacks)
                    {
                        minStack = tStack;
                        if (/*minStack.FinishedPatternCount == 0 &&*/ maxStack.FinishedPatternCount <= 3)
                        {
                            continue;
                        }

                        var tp = (maxStack.EstimateTime - minStack.EstimateTime) * (minStack.TimeAvailability / (maxStack.TimeAvailability + minStack.TimeAvailability));

                        var matchs = maxStack.PlanRemainingPatterns.Join(minStack.PlanRemainingPatterns,
                            max => new { max.Color, max.BookCount }, min => new { min.Color, min.BookCount },
                            (max, min) => new PatternMatch
                            { MaxPattern = max, MinPattern = min, PP = max.TotallyTime - min.TotallyTime })
                            .Where(item=>item.PP>0)
                            .OrderBy(item=>Math.Abs(item.PP- tp));

                        var tMatchs = maxStack.PlanRemainingPatterns.GroupJoin(minStack.PlanRemainingPatterns, max => max.Color,
                            min => min.Color, (max, minPatterns) =>
                            {
                                var bookCount = max.BookCount;
                                var combinations = CombinationHelper.Combination(2, minPatterns).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i => i.ActuallyStackName).Count() == 1);
                                if (bookCount >= 3)
                                {
                                    var list2 = CombinationHelper.Combination(3, minPatterns).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i => i.ActuallyStackName).Count() == 1);
                                    combinations.AddRange(list2);
                                }
                                if (bookCount == 4)
                                {
                                    var list3 = CombinationHelper.Combination(4, minPatterns.Where(item => item.BookCount == 1)).FindAll(item => item.CombinationCollection.Sum(i => i.BookCount) == bookCount && item.CombinationCollection.GroupBy(i => i.ActuallyStackName).Count() == 1);
                                    combinations.AddRange(list3);
                                }

                                var tCombinations = combinations.ConvertAll(combination => new
                                {
                                    Combination = combination,
                                    PatternCount = combination.CombinationCollection.Count,
                                    PP = max.TotallyTime - combination.CombinationCollection.Sum(item => item.TotallyTime)
                                }).Where(item=>item.PP>0);

                                return new { MaxPattern = max, MinPatterns = minPatterns, Combinations = tCombinations };
                            }).SelectMany(item => item.Combinations, (t, ts) => new PatternsMatch
                            {
                                MaxPattern = t.MaxPattern,
                                Combination=ts.Combination,
                                PP = ts.PP,
                                PatternCount = ts.PatternCount
                            }).OrderBy(item => Math.Abs(item.PP - tp));

                        var maxMatch = matchs.FirstOrDefault();
                        var tMaxMatch = tMatchs.FirstOrDefault();
                        bool selectMatchs = false;
                        if(maxMatch == null && tMaxMatch ==null) continue;
                        if (maxMatch != null && tMaxMatch != null)
                        {
                            selectMatchs = !(Math.Abs(maxMatch.PP - tp) < Math.Abs(tMaxMatch.PP - tp));
                        }
                        else
                        {
                            selectMatchs = maxMatch == null;
                        }

                        if (selectMatchs)
                        {
                            var ttMatch = tMaxMatch;
                            var list = ttMatch.Combination.CombinationCollection;
                            var maxPattern = ttMatch.MaxPattern;
                            SwapPattern(maxPattern, list);
                            List<Pattern> updates = new List<Pattern>();
                            updates.Add(maxPattern);
                            updates.AddRange(list);
                            Contract.BulkUpdatePatterns(updates);
                            maxStack.PlanRemainingPatterns.Remove(maxPattern);
                            maxStack.PlanRemainingPatterns.AddRange(list);

                            minStack.PlanRemainingPatterns.RemoveAll(list.Contains);
                            minStack.PlanRemainingPatterns.Add(maxPattern);
                            return $"微调--批次：{batchName}--垛号：{ttMatch.MaxPattern.ActuallyStackName}与垛号：{ttMatch.Combination.CombinationCollection.First().ActuallyStackName}交换锯切图[{ttMatch.MaxPattern.MdbName}]<--->[{string.Join(",", list.ConvertAll(item => item.MdbName))}]";
                        }
                        else
                        {
                            var match = maxMatch;
                            SwapPattern(match.MaxPattern, match.MinPattern);
                            List<Pattern> updates = new List<Pattern>();
                            updates.Add(match.MaxPattern);
                            updates.Add(match.MinPattern);
                            Contract.BulkUpdatePatterns(updates);

                            maxStack.PlanRemainingPatterns.Remove(match.MaxPattern);
                            maxStack.PlanRemainingPatterns.Add(match.MinPattern);

                            minStack.PlanRemainingPatterns.Remove(match.MinPattern);
                            minStack.PlanRemainingPatterns.Add(match.MaxPattern);
                            return $"微调--批次：{batchName}--垛号：{match.MaxPattern.ActuallyStackName}与垛号：{match.MinPattern.ActuallyStackName}交换锯切图[{match.MaxPattern.MdbName}]<--->[{match.MinPattern.MdbName}]";
                        }
                        
                    }

                    return $"当前预测最大偏差时间为：{pp}s,大于触发调配时间{swapTriggerMinTime}s,但无符合调配锯切图";
                }
                else
                {
                    return $"当前预测最大偏差时间为：{pp}s,小于触发调配时间{swapTriggerMinTime}s,不执行调配";
                }
            }
        }

        public void RemoveFinished(DateTime planDate)
        {
            lock (ObjLock)
            {
                if (Patterns.Exists(item => item.PlanDate < planDate.Date))
                {
                    var removePatterns = Patterns.FindAll(item => item.PlanDate < planDate.Date);
                    Patterns.RemoveAll(removePatterns.Contains);
                    var removeBatchNames = removePatterns.ConvertAll(item => item.BatchName).Distinct().ToList();
                    PatternDetails.RemoveAll(item => removeBatchNames.Contains(item.BatchName));
                }
            }
        }

        private void SwapPattern(Pattern maxPattern, Pattern minPattern)
        {
            var maxStackName = maxPattern.ActuallyStackName;
            var minStackName = minPattern.ActuallyStackName;
            var maxActualDeviceName = maxPattern.ActualDeviceName;
            var minActualDeviceName = minPattern.ActualDeviceName;
            maxPattern.ActuallyStackName = minStackName;
            minPattern.ActuallyStackName = maxStackName;
            maxPattern.ActualDeviceName = minActualDeviceName;
            minPattern.ActualDeviceName = maxActualDeviceName;
            maxPattern.UpdatedTime = DateTime.Now;
            minPattern.UpdatedTime = DateTime.Now;
        }

        private void SwapPattern(Pattern maxPattern, List<Pattern> minPatterns)
        {
            var list = minPatterns;
            var maxStackName = maxPattern.ActuallyStackName;
            var minStackName = list[0].ActuallyStackName;

            var maxActualDeviceName = maxPattern.ActualDeviceName;
            var minActualDeviceName = list.First().ActualDeviceName;

            maxPattern.ActuallyStackName = minStackName;
            maxPattern.ActualDeviceName = minActualDeviceName;
            maxPattern.UpdatedTime = DateTime.Now;
            list.ForEach(item =>
            {
                item.ActuallyStackName = maxStackName;
                item.UpdatedTime = DateTime.Now;
                item.ActualDeviceName = maxActualDeviceName;
            });
        }


        public Pattern DistributePattern(string batchName, string stackName, int maxBookCount, string color, BookType bookType)
        {
            if (maxBookCount <= 0) return null;
            lock (ObjLock)
            {
                var matchPatterns =
                    Patterns.FindAll(item => item.IsEnable && item.ActuallyStackName == stackName && item.Status ==Convert.ToInt32(PatternStatus.Undistributed) && item.BatchName == batchName && item.Color == color && item.BookCount <= maxBookCount);
                if (matchPatterns.Count == 0) return null;
                Pattern pattern = null;

                
                switch (bookType)
                {
                    case BookType.Default:
                        {
                            pattern = matchPatterns.OrderByDescending(item => item.BookCount)
                                .ThenByDescending(item => (1.0 * item.TotallyTime) / (item.PartCount + item.OffPartCount))
                                .ThenByDescending(item => item.PartCount + item.OffPartCount).First();
                            break;

                        }
                    case BookType.ContainFrontBatch:
                        {
                            pattern = matchPatterns.OrderByDescending(item => item.BookCount)
                                .ThenBy(item => item.TotallyTime)
                                .ThenBy(item => item.PartCount + item.OffPartCount).First();
                            break;
                        }
                    case BookType.ContainNextBatch:
                        {
                            pattern = matchPatterns.OrderBy(item => item.BookCount)
                                .ThenByDescending(item => (1.0 * item.TotallyTime) / (item.PartCount + item.OffPartCount))
                                .ThenByDescending(item => item.PartCount + item.OffPartCount).First();
                            break;
                        }
                    case BookType.CurrentBatch:
                        {
                            pattern = matchPatterns.OrderByDescending(item => item.BookCount)
                                .ThenByDescending(item => (1.0 * item.TotallyTime) / (item.PartCount + item.OffPartCount))
                                .ThenByDescending(item => item.PartCount + item.OffPartCount).First();
                            break;
                        }
                    case BookType.NextBatch:
                        {
                            pattern = matchPatterns.OrderBy(item => item.BookCount)
                                .ThenByDescending(item => item.TotallyTime)
                                .ThenByDescending(item => item.PartCount + item.OffPartCount).First();
                            break;
                        }

                }

                if (pattern != null)
                {
                    pattern.Status = Convert.ToInt32(PatternStatus.UndistributedButUnLoad);
                    pattern.UpdatedTime = DateTime.Now;
                    SendMsg("锯切图", "分配锯切图", $"批次{pattern.BatchName}:{pattern.PatternId}号锯切图更新为未下载状态");
                }

                return pattern;
            }
        }
        private void SendMsg(string objectStr, string type, string msg)
        {
            BroadcastMessage.Send(nameof(ExecuteMsg), new ExecuteMsg() { Object = objectStr, Msg = msg, Command = this.GetType().Name, Type = type });
        }
        public bool HasPattern(string batchName, string color)
        {
            var t = Patterns.FindAll(item => item.IsEnable && item.BatchName == batchName && item.Color == color);

            var tts = Patterns.FindAll(item =>
                item.IsEnable && item.BatchName == batchName &&
                item.Status == Convert.ToInt32(PatternStatus.Undistributed));

            return 
                Patterns.Exists(item => item.IsEnable && item.BatchName == batchName && item.Color == color && item.Status == Convert.ToInt32(PatternStatus.Undistributed));
        }

        public void RequestDisintegratedPattern(string batchName, int maxBookCount, string color)
        {
            //if (IsDisintegrating) return;

            //lock (DisintegratedLock)
            //{
            //    if (IsDisintegrating) return;

            //    lock (ObjLock)
            //    {
            //        if (Patterns.Exists(item => item.IsEnable && item.BatchName == batchName && item.Color == color && item.BookCount <= maxBookCount)) return;
            //        var matchPatterns = Patterns.FindAll(item => item.IsEnable &&
            //                                                     item.BatchName == batchName && item.Color == color && item.BookCount > maxBookCount)
            //            .OrderBy(item => item.BookCount);
            //        if (!matchPatterns.Any()) return;
            //        var matchPattern = matchPatterns.First();
            //        matchPattern.IsEnable = false;
            //        var matchPatternDetails = PatternDetails.FindAll(item =>
            //            item.BatchName == matchPattern.BatchName && item.PatternId == matchPattern.PatternId);
            //        DisintegratedTask task = new DisintegratedTask() { Pattern = matchPattern, Details = matchPatternDetails, CurrentMaxPatternId = Patterns.FindAll(item => item.BatchName == batchName).Max(item => item.PatternId), MaxBookCount = maxBookCount };
            //        DisintegratedTasks.Add(task);
            //        IsDisintegrating = true;


            //    }

            //}

        }

        //private Dictionary<string,List<Pattern>> PatternDictionary { get; } = new Dictionary<string, List<Pattern>>();
        //private readonly object _dictionaryLock = new object();
        public List<Pattern> GetPatternsByBatchName(string batchName)
        {
            //lock (_dictionaryLock)
            //{
            //    if (!PatternDictionary.ContainsKey(batchName))
            //    {
            //        var tPatterns = Patterns.FindAll(item => item.BatchName == batchName);
            //        PatternDictionary.Add(batchName, tPatterns);
            //    }

            //    return PatternDictionary[batchName];
            //}
            return Patterns.FindAll(item => item.BatchName == batchName);

        }
    }
}
