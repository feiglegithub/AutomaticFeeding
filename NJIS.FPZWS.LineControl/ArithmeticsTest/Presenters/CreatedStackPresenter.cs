using Arithmetics;
using ArithmeticsTest.Helpers;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using BatchGroup = NJIS.FPZWS.LineControl.Cutting.Model.BatchGroup;

namespace ArithmeticsTest.Presenters
{
    public class CreatedStackPresenter: PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string BindingData = nameof(BindingData);
        public const string CreatedStack = nameof(CreatedStack);

        public const string Save = nameof(Save);

        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract Contract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = WcfClient.GetProxy<ILineControlCuttingContract>());

        private List<TmpBatchInfo> BatchInfos { get; set; } = new List<TmpBatchInfo>();

        private List<BookStack> DeviceStacks { get; set; } = new List<BookStack>();

        private List<BatchGroup> BatchGroups { get; set; } = new List<BatchGroup>();

        private List<Pattern> Patterns { get; set; } = new List<Pattern>();

        private double MinRatio { get; set; } = 0.33;
        private double BaseRatio { get; set; } = 0.38;

        public CreatedStackPresenter()
        {
            Register();
        }

        private void Register()
        {
            this.Register<DateTime>(GetData, ExecuteGetData);
            this.Register<List<DeviceInfos>>(CreatedStack,ExecuteCreatedStacks);
            this.Register<List<BatchStack>>(Save, ExecuteSave);
        }

        private void ExecuteSave(object sender, List<BatchStack> batchStacks)
        {
            if (BatchGroups.Count == 0 || batchStacks.Count == 0)
            {
                this.SendTipsMessage("没有建垛信息，无法保存！",sender);
                this.Send(BindingData,sender,false);
            }

            CreatedStackDetail(batchStacks);
            var batchGroups = Contract.GetBatchGroupsByPlanDate(batchStacks[0].PatternStacks[0].Stack.PlanDate);
            int maxStackListId = 0;
            if (batchGroups.Count > 0)
            {
                maxStackListId = batchGroups.Max(item => item.StackListId);
            }

            BatchGroups.ForEach(item =>
            {
                item.StackListId += maxStackListId;
                item.Status = Convert.ToInt32(BatchGroupStatus.Stocking);
            });

            batchStacks.ForEach(batchStack =>
            {
                batchStack.PatternStacks.ForEach(item => item.Stack.StackListId += maxStackListId);
                batchStack.PatternStacks.RemoveAll(item => item.StackDetails.Count == 0);
            });
            var tt = batchStacks.FindAll(item => item.PatternStacks.Exists(i => i.Stack.StackListId == 0));

            List<WMSCuttingStackList> stackLists = new List<WMSCuttingStackList>();
            batchStacks.ForEach(batchStack => batchStack.PatternStacks.ForEach( patternStack=> stackLists.AddRange(ConvertStackList(patternStack.StackDetails))));

            var patternStacks = batchStacks.SelectMany(batchStack => batchStack.PatternStacks);
            var stacks = patternStacks.Select(patternStack => patternStack.Stack).ToList();
            var ttt = stacks.FindAll(item => item.StackListId == 0 && item.BookCount!=0);
            var stackDetails = patternStacks.SelectMany(patternStack => patternStack.StackDetails).ToList();
            var patterns = patternStacks.SelectMany(patternStack => patternStack.Patterns).ToList();

            for (int i = 0; i < BatchGroups.Count; )
            {
                var batchGroup = BatchGroups[i];
                if (!stacks.Exists(item => item.StackListId == batchGroup.StackListId))
                {
                    BatchGroups.RemoveAt(i);
                    continue;
                }

                i++;
            }

            var tJoin = patterns.Join(stacks, p => p.PlanStackName, s => s.StackName, (p, s) => new {Pattern = p, Stack = s}).ToList();
            tJoin.ForEach(item=>
            {
                item.Pattern.ActuallyStackName = item.Pattern.PlanStackName;
                item.Pattern.ActualDeviceName = item.Pattern.PlanDeviceName = item.Stack.PlanDeviceName;
            });
            
            Contract.BulkInsertStacks(stacks);
            Contract.BulkInsertWmsCuttingStackList(stackLists);
            Contract.BulkInsertBatchGroups(BatchGroups);
            Contract.BulkInsertStackDetails(stackDetails);
            Contract.BulkUpdatePatterns(patterns);
            BatchGroups = new List<BatchGroup>();
            this.Send(BindingData,sender,new List<BatchStack>());
            this.Send(BindingData,sender, BatchGroups);
            this.Send(BindingData,sender,true);
            ExecuteGetData(sender, CurPlanDate);
        }

        private List<WMSCuttingStackList> ConvertStackList(List<StackDetail> stackDetails)
        {
            
            List<WMSCuttingStackList> stackLists = new List<WMSCuttingStackList>();
            var bookCount = stackDetails.Count;
            foreach (var stackDetail in stackDetails)
            {
                WMSCuttingStackList stackList = new WMSCuttingStackList()
                {
                    PlanDate = stackDetail.PlanDate,
                    BatchName = stackDetail.PlanBatchName,
                    CreatedTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now,
                    StackName = stackDetail.StackName,
                    RawMaterialID = stackDetail.Color,
                    StackIndex = bookCount + 1 - stackDetail.StackIndex,
                    WMSStatus = 0,
                    TaskId = new Guid()
                };
                stackLists.Add(stackList);
            }

            return stackLists;
        }

        private DateTime CurPlanDate { get; set; } = DateTime.Today;

        private void ExecuteGetData(object sender, DateTime planDate)
        {
            try
            {
                CurPlanDate = planDate;
                var patterns = Contract.GetPatternsByPlanDate(planDate);
                Patterns = patterns;
                var groups = patterns.GroupBy(item => new { item.BatchName, item.Color }, p => p, (key, tPatterns) =>
                    new TmpBatchInfo
                    {
                        PlanDate = tPatterns.ToList()[0].PlanDate,
                        BatchName = key.BatchName,
                        Color = key.Color,
                        TotallyTime = tPatterns.Sum(i => i.TotallyTime),
                        BookCount = tPatterns.Sum(i => i.BookCount),
                        PartCount = tPatterns.Sum(i => i.PartCount),
                        OffPartCount = tPatterns.Sum(i => i.OffPartCount)
                    });

                BatchInfos = groups.OrderBy(item => item.BatchName).ThenBy(item => item.Color).ToList();
                this.Send(BindingData, sender, BatchInfos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private void CreatedStackDetail(List<BatchStack> batchStacks)
        {
            foreach (var batchStack in batchStacks)
            {
                foreach (var patternStack in batchStack.PatternStacks)
                {
                    patternStack.Stack.PlanDate = patternStack.Patterns[0].PlanDate;

                    patternStack.Patterns.RemoveAll(patternStack.MoveLastStackPatterns.Contains);
                    patternStack.Patterns.ForEach(item=>item.PlanStackName = patternStack.StackName);
                    var connectColor = patternStack.Stack.ConnectColor;
                    
                    var otherColorPatterns = patternStack.Patterns.FindAll(item => item.Color != connectColor).OrderBy(item => item.Color);
                    foreach (var otherColorPattern in otherColorPatterns)
                    {
                        var stackDetails = CreatedStackDetails(otherColorPattern);
                        patternStack.StackDetails.AddRange(stackDetails);
                    }
                    var connectColorPatterns = patternStack.Patterns.FindAll(item => item.Color == connectColor);
                    foreach (var colorPattern in connectColorPatterns)
                    {
                        var stackDetails = CreatedStackDetails(colorPattern);
                        patternStack.StackDetails.AddRange(stackDetails);
                    }

                    foreach (var nextStackPattern in patternStack.NextStackPatterns)
                    {
                        nextStackPattern.PlanStackName = patternStack.StackName;
                        var stackDetails = CreatedStackDetails(nextStackPattern);
                        patternStack.StackDetails.AddRange(stackDetails);
                    }

                    patternStack.Patterns.AddRange(patternStack.NextStackPatterns);
                    for (int i = 0; i < patternStack.StackDetails.Count; i++)
                    {
                        var stackDetail = patternStack.StackDetails[i];
                        stackDetail.StackIndex = i + 1;
                    }
                    if(patternStack.StackDetails.Count==0)continue;
                    var firstBatchName = patternStack.StackDetails.First().PlanBatchName;
                    var secondBatchName = patternStack.NextStackPatterns.FirstOrDefault()?.BatchName;
                    var stack = patternStack.Stack;
                    stack.BookCount = patternStack.StackDetails.Count;
                    stack.MainColor = stack.ConnectColor;
                    stack.FirstBatchName = firstBatchName;
                    stack.FirstBatchBookCount =
                        patternStack.StackDetails.Count(item => item.PlanBatchName == firstBatchName);
                    stack.SecondBatchName = secondBatchName;
                    stack.SecondBatchBookCount =
                        patternStack.StackDetails.Count(item => item.PlanBatchName == secondBatchName);
                    stack.Status = Convert.ToInt32(StackStatus.Stocking);
                    stack.CreatedTime = DateTime.Now;
                    stack.UpdatedTime = DateTime.Now;
                }
            }
        }

        private List<StackDetail> CreatedStackDetails(Pattern pattern)
        {
            List<StackDetail> list  = new List<StackDetail>();
            for (int i = 0; i < pattern.BookCount; i++)
            {
                list.Add(new StackDetail()
                {
                    StackName = pattern.PlanStackName,
                    PlanBatchName = pattern.BatchName,
                    Color = pattern.Color,
                    Status = Convert.ToInt32(BookStatus.UndistributedPattern),
                    PlanDate = pattern.PlanDate,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,

                });
            }

            return list;
        }

        internal class TmpGroupKey
        {
            public string FirstMainColor { get; set; }
            public string SecondMainColor { get; set; }
            public string BatchName { get; set; }
            public int FirstMainColorTime { get; set; }
            public int SecondMainColorTime { get; set; }
            public override int GetHashCode()
            {
                return typeof(TmpGroupKey).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = (TmpGroupKey)obj;
                return (FirstMainColor == other.FirstMainColor && SecondMainColor == other.SecondMainColor) ||
                       (FirstMainColor == other.SecondMainColor && SecondMainColor == other.FirstMainColor);
            }
        }

        /// <summary>
        /// 构建批次组
        /// </summary>
        /// <returns></returns>
        private List<BatchGroup> CreatedBatchGroups()
        {
            List<BatchGroup> batchGroups = new List<BatchGroup>();

            var planDate = Patterns[0].PlanDate;
            var batchMainColors = Patterns.GroupBy(item => item.BatchName, p => p, (batchName, ps) =>
            {
                var totallyTime = ps.Sum(item => item.TotallyTime);

                var tps = ps
                    .GroupBy(item => item.Color, p => p,
                        (color, p) => new { Color = color, TotallyTime = p.Sum(i => i.TotallyTime) });
                var r = tps.Any(item => item.TotallyTime > totallyTime * BaseRatio) ? BaseRatio : MinRatio;

                var tt = tps
                    .Where(item => item.TotallyTime > totallyTime * r)
                    .OrderByDescending(item => item.TotallyTime).ToList();
                return new
                {
                    BatchName = batchName,
                    TotallyTime = totallyTime,
                    ColorGroups = ps.GroupBy(item => item.Color),
                    MainColors = tt.ConvertAll(item => item.Color),
                    FirstMainColor = tt[0].Color,
                    FirstMainColorTime = tt[0].TotallyTime,
                    SecondMainColor = tt.Count > 1 ? tt[1].Color : string.Empty,
                    SecondMainColorTime = tt.Count > 1 ? tt[1].TotallyTime : 0,
                };
            }).ToList();

            var groupList = batchMainColors.GroupBy(
                item => new TmpGroupKey {FirstMainColor = item.FirstMainColor, SecondMainColor = item.SecondMainColor},
                g => g, new FuncEqualityComparer<TmpGroupKey>((x, y) =>
                    (x.FirstMainColor == y.FirstMainColor && x.SecondMainColor == y.SecondMainColor) ||
                    (x.FirstMainColor == y.SecondMainColor && x.SecondMainColor == y.FirstMainColor)
                )).ToList();
            var twoMainColors = groupList.FindAll(item => !string.IsNullOrWhiteSpace(item.Key.SecondMainColor)).OrderByDescending(item=>item.Count()).ToList();
            var oneMainColors = groupList.FindAll(item => string.IsNullOrWhiteSpace(item.Key.SecondMainColor));

            var groupId = 0;
            var stackListId = 0;
            foreach (var twoMainColor in twoMainColors)
            {
                groupId++;
                var list = twoMainColor.OrderByDescending(item=>item.FirstMainColorTime+item.SecondMainColorTime).ToList();

                var firstMainColor = twoMainColor.Key.FirstMainColor;
                var secondMainColor = twoMainColor.Key.SecondMainColor;

                var firstMainColorTime = twoMainColor.Sum(item => item.ColorGroups.First(i => i.Key == firstMainColor).Sum(i => i.TotallyTime));
                var secondMainColorTime = twoMainColor.Sum(item => item.ColorGroups.First(i => i.Key == secondMainColor).Sum(i => i.TotallyTime));

                var mainColor = firstMainColorTime >= secondMainColorTime ? firstMainColor : secondMainColor;

                var tOneMainColors = oneMainColors.FindAll(item => item.Key.FirstMainColor == mainColor);

                tOneMainColors = tOneMainColors.OrderByDescending(item => item.Sum(i => i.FirstMainColorTime)).ToList();
                var isConnect = list.Count > 1 || tOneMainColors.Count > 0;
                for (int i = 0; i < list.Count; i++)
                {
                    var batchIndex = i;
                    var nextBatchName = i + 1 == list.Count? string.Empty : list[i + 1].BatchName;
                    var batchGroup = new BatchGroup()
                    {
                        GroupId = groupId, BatchIndex = batchIndex+1, IsFinished = false,
                        CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now,
                        StackListId = ++stackListId, Status = Convert.ToInt32(BatchGroupStatus.UnStock),
                        PlanDate = planDate, IsConnect = isConnect,
                        FrontBatchName = i == 0 ? string.Empty : list[i - 1].BatchName,
                        NextBatchName = nextBatchName,
                        BatchName = list[i].BatchName
                    };
                    batchGroups.Add(batchGroup);
                }

                for (int i = 0; i < tOneMainColors.Count; i++)
                {
                    var batchIndex = list.Count+i;

                    var ttOneMainColors = tOneMainColors[i].OrderByDescending(item=>item.FirstMainColorTime).ToList();
                    for (int j = 0; j < ttOneMainColors.Count; j++)
                    {
                        var batchName = ttOneMainColors[j].BatchName;
                        var frontBatchName = batchGroups.Last().BatchName;
                        batchGroups.Last().NextBatchName = batchName;
                        var batchGroup = new BatchGroup()
                        {
                            GroupId = groupId,
                            BatchIndex = batchIndex+j + 1,
                            IsFinished = false,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now,
                            StackListId = ++stackListId,
                            Status = Convert.ToInt32(BatchGroupStatus.UnStock),
                            PlanDate = planDate,
                            IsConnect = isConnect,
                            FrontBatchName = frontBatchName,
                            NextBatchName = string.Empty,
                            BatchName = batchName
                        };
                        batchGroups.Add(batchGroup);
                    }
                }
                oneMainColors.RemoveAll(tOneMainColors.Contains);
            }

            for (int i = 0; i < oneMainColors.Count; i++)
            {
                groupId++;
                var ttOneMainColors = oneMainColors[i].OrderByDescending(item => item.FirstMainColorTime).ToList();
                for (int j = 0; j < ttOneMainColors.Count; j++)
                {
                    var batchName = ttOneMainColors[j].BatchName;
                    var lastGroup = j == 0 ? null : batchGroups.Last();
                    if (lastGroup != null)
                    {
                        lastGroup.NextBatchName = batchName;
                    }
                    var frontBatchName = lastGroup?.BatchName;
                    
                    var batchGroup = new BatchGroup()
                    {
                        GroupId = groupId,
                        BatchIndex = j + 1,
                        IsFinished = false,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        StackListId = ++stackListId,
                        Status = Convert.ToInt32(BatchGroupStatus.UnStock),
                        PlanDate = planDate,
                        IsConnect = ttOneMainColors.Count>1,
                        FrontBatchName = frontBatchName,
                        NextBatchName = string.Empty,
                        BatchName = batchName
                    };
                    batchGroups.Add(batchGroup);
                }
            }
            return batchGroups;
        }

        private TmpGroupKey GetTmpGroupKey(BatchStack batchStack)
        {
            var tPatterns = Patterns.FindAll(item => item.BatchName == batchStack.BatchName);
            var totallyTime = tPatterns.Sum(item => item.TotallyTime);
            var tps = tPatterns
                .GroupBy(item => item.Color, p => p,
                    (color, p) => new { Color = color, TotallyTime = p.Sum(i => i.TotallyTime) });
            var r = tps.Any(item => item.TotallyTime > totallyTime * BaseRatio) ? BaseRatio : MinRatio;

            var tt = tps
                .Where(item => item.TotallyTime > totallyTime * r)
                .OrderByDescending(item => item.TotallyTime).ToList();
            return new TmpGroupKey
            {
                BatchName = batchStack.BatchName,
                FirstMainColor = tt[0].Color,
                FirstMainColorTime = tt[0].TotallyTime,
                SecondMainColor = tt.Count > 1 ? tt[1].Color : string.Empty,
                SecondMainColorTime = tt.Count > 1 ? tt[1].TotallyTime : 0,
            };
        }

        private void CombineBatchStack(BatchStack firstBatchStack, BatchStack secondBatchStack)
        {

            var firstKey = GetTmpGroupKey(firstBatchStack);
            var secondKey = GetTmpGroupKey(secondBatchStack);

            firstBatchStack.MainColors.Add(firstKey.FirstMainColor);
            if (string.IsNullOrWhiteSpace(firstKey.SecondMainColor))
            {
                firstBatchStack.MainColors.Add(firstKey.SecondMainColor);
            }

            secondBatchStack.MainColors.Add(secondKey.FirstMainColor);
            if (string.IsNullOrWhiteSpace(secondKey.SecondMainColor))
            {
                secondBatchStack.MainColors.Add(secondKey.SecondMainColor);
            }

            var mainColorIntersect = firstBatchStack.MainColors.Intersect(secondBatchStack.MainColors).ToArray();
            if(!mainColorIntersect.Any()) return;

            if (mainColorIntersect.Length == 2) //双主花色一致
            {
                var mainColor = firstKey.FirstMainColorTime >= firstKey.SecondMainColorTime
                    ? firstKey.FirstMainColor
                    : firstKey.SecondMainColor;
                var otherMainColor = mainColor == firstKey.FirstMainColor
                    ? firstKey.SecondMainColor
                    : firstKey.FirstMainColor;
                StackConnect(firstBatchStack.PatternStacks, secondBatchStack.PatternStacks, mainColor,
                    otherMainColor);
            }
            else // 单个主花色一致
            {
                var mainColor = mainColorIntersect.First();
                
                StackConnect(firstBatchStack.PatternStacks, secondBatchStack.PatternStacks, mainColor,string.Empty);
            }
        }

        /// <summary>
        /// 双花色批次垛的连接
        /// </summary>
        /// <param name="firstPatternStacks"></param>
        /// <param name="secondPatternStacks"></param>
        /// <param name="mainColor"></param>
        /// <param name="otherMainColor"></param>
        private void StackConnect(List<PatternStack> firstPatternStacks, List<PatternStack> secondPatternStacks,
            string mainColor, string otherMainColor)
        {
            //var otherMainColor = firstMainColor == mainColor ? secondMainColor : firstMainColor;

            var list = CombinationHelper.CollectionMatchCombination(firstPatternStacks, secondPatternStacks,
                (first, second) =>
                {
                    var connectColor = "";
                    var isOneColor = false;
                    var firstIsOneColor = first.Colors.Count == 1;
                    var secondIsOneColor = second.Colors.Count == 1;
                    
                    var firstColors = first.Colors;
                    var secondColors = second.Colors;
                    var intersect = firstColors.Intersect(secondColors).ToList();
                    var intersectCount = intersect.Count;

                    if (intersect.Contains(mainColor))
                    {
                        connectColor = mainColor;
                        isOneColor = firstIsOneColor && secondIsOneColor;
                    }
                    else if (intersect.Contains(otherMainColor))
                    {
                        connectColor = otherMainColor;
                        isOneColor = firstIsOneColor && secondIsOneColor;
                    }
                    else
                    {
                        connectColor = string.Empty;
                        isOneColor = false;
                    }
                    return new {First = first, Second = second,IsOneColor= isOneColor, ConnectColor = connectColor };
                });

            var selectCombine = list.OrderByDescending(item =>
                    item.CombinationCollection.Count(i => i.IsOneColor && !string.IsNullOrWhiteSpace(i.ConnectColor)))
                .ThenByDescending(item =>
                    item.CombinationCollection.Count(i => !i.IsOneColor && !string.IsNullOrWhiteSpace(i.ConnectColor))).First();
            foreach (var combine in selectCombine.CombinationCollection)
            {
                combine.First.Stack.ConnectColor = combine.ConnectColor;
                combine.Second.Stack.PlanDeviceName = combine.First.Stack.PlanDeviceName;
                combine.First.Stack.NextStackName = combine.Second.Stack.StackName;
                combine.Second.Stack.LastStackName = combine.First.Stack.StackName;
            }

            #region old code
            
            //var firstOneColorStacks = firstPatternStacks.FindAll(item => string.IsNullOrWhiteSpace(item.SecondColor));
            //var secondOneColorStacks = secondPatternStacks.FindAll(item => string.IsNullOrWhiteSpace(item.SecondColor));
            //var firstOtherColorStacks =
            //    firstPatternStacks.FindAll(item => !string.IsNullOrWhiteSpace(item.SecondColor));

            //var secondOtherColorStacks =
            //    secondPatternStacks.FindAll(item => !string.IsNullOrWhiteSpace(item.SecondColor));

            ////单花色的垛
            //var firstGroups = firstOneColorStacks.GroupBy(item => item.FirstColor).ToDictionary(p=>p.Key,p=>p.ToList());
            //var secondGroups = secondOneColorStacks.GroupBy(item => item.FirstColor).ToDictionary(p => p.Key, p => p.ToList());

            //foreach (var firstGroup in firstGroups)
            //{
            //    var color = firstGroup.Key;
            //    List<PatternStack> secondList = secondGroups.ContainsKey(color) ? secondGroups[color] : secondOtherColorStacks;
                
            //    var firstList = firstGroup.Value;
            //    for (int i = 0; i < firstList.Count; )
            //    {
            //        var firstStack = firstList[i];
            //        var secondStack = secondList.FirstOrDefault(item=>item.Colors.Contains(color));
            //        if(secondStack == null)
            //        {
            //            break;
            //        }
            //        firstStack.Stack.NextStackName = secondStack.Stack.StackName;
            //        secondStack.Stack.LastStackName = firstStack.Stack.StackName;
            //        firstList.Remove(firstStack);
            //        secondList.Remove(secondStack);
            //    }
            //}

            //foreach (var secondGroup in secondGroups)
            //{
            //    var color = secondGroup.Key;
            //    List<PatternStack> firstList = firstOtherColorStacks;

            //    var secondList = secondGroup.Value;
            //    for (int i = 0; i < secondList.Count;)
            //    {
            //        var secondStack = secondList[i];
            //        var firstStack = firstList.FirstOrDefault(item => item.Colors.Contains(color));
            //        if (firstStack == null)
            //        {
            //            break;
            //        }
            //        secondStack.Stack.LastStackName = firstStack.Stack.StackName;
            //        firstStack.Stack.NextStackName = secondStack.Stack.StackName;

            //        secondList.Remove(secondStack);
            //        firstList.Remove(firstStack);
            //    }
            //}

            //for (int i = 0; i < firstOtherColorStacks.Count; )
            //{
            //    var firstOtherColorStack = firstOtherColorStacks[i];
            //    if (firstOtherColorStack.Colors.Contains(mainColor))
            //    {
            //        var secondStack = secondOtherColorStacks.FirstOrDefault(item => item.Colors.Contains(mainColor));
            //        if (secondStack != null)
            //        {
            //            firstOtherColorStack.Stack.NextStackName = secondStack.Stack.StackName;
            //            secondStack.Stack.LastStackName = firstOtherColorStack.Stack.StackName;
            //            secondOtherColorStacks.Remove(secondStack);
            //            firstOtherColorStacks.Remove(firstOtherColorStack);
            //            continue;
            //        }
            //    }

            //    var nextColor = mainColor != firstMainColor ? firstMainColor : secondMainColor;
            //    if (firstOtherColorStack.Colors.Contains(nextColor))
            //    {
            //        var secondStack = secondOtherColorStacks.FirstOrDefault(item => item.Colors.Contains(mainColor));
            //        if (secondStack != null)
            //        {
            //            firstOtherColorStack.Stack.NextStackName = secondStack.Stack.StackName;
            //            secondStack.Stack.LastStackName = firstOtherColorStack.Stack.StackName;
            //            continue;
            //        }
            //    }

            //    i++;
            //}

            #endregion
        }


        private void ExecuteCreatedStacks(object sender, List<DeviceInfos> dis)
        {
            List<BatchStack> batchStacks = PatternStackHelper.CreatedPatternStacks(Patterns, dis.Count);

            foreach (var batchStack in batchStacks)
            {
                for (int i = 0; i < batchStack.PatternStacks.Count; i++)
                {
                    batchStack.PatternStacks[i].Stack.PlanDeviceName = dis[i].DeviceName;
                }
            }

            int stackCount = 0;
            foreach (var batchStack in batchStacks)
            {
                batchStack.PatternStacks.ForEach(patternStack=>
                {
                    var stackName = patternStack.BatchName + (++stackCount).ToString("000");
                    patternStack.Stack.StackName = stackName;
                });
            }

            var tBatchGroups = CreatedBatchGroups();
            BatchGroups = tBatchGroups;
            var tGroups = BatchGroups.GroupBy(item => item.GroupId);
            foreach (var tGroup in tGroups)
            {
                var list = tGroup.OrderBy(item => item.BatchIndex).ToList();
                if (list.Count == 1)
                {
                    var batchName = list.First().BatchName;
                    var curBatchStack = batchStacks.First(item => item.BatchName == batchName);
                    curBatchStack.PatternStacks.ForEach(item=>
                    {
                        item.Stack.StackListId = list[0].StackListId;
                        item.Patterns.ForEach(i => i.PlanStackName = item.Stack.StackName);
                    });

                    continue;
                }
 
                for (int i = 0; i < list.Count; i++)
                {
                    var batchGroup = list[i];
                    BatchGroup nextBatchGroup = null;
                    if (i < list.Count - 1)
                    {
                        nextBatchGroup = list[i + 1];
                    }

                    var curBatchStack = batchStacks.First(item => item.BatchName == batchGroup.BatchName);
                    BatchStack nextBatchStack = null;
                    if (nextBatchGroup != null)
                    {
                        nextBatchStack = batchStacks.First(item => item.BatchName == nextBatchGroup.BatchName);
                        CombineBatchStack(curBatchStack, nextBatchStack);
                    }
                    
                    curBatchStack.PatternStacks.ForEach(item => item.Stack.StackListId = batchGroup.StackListId);


                    if (nextBatchStack != null)
                    {
                        curBatchStack.PatternStacks.ForEach(item =>
                        {
                            var nextStackName = item.Stack.NextStackName;
                            var nextStack = nextBatchStack.PatternStacks.First(it => it.Stack.StackName == nextStackName);
                            var curStackBookCount = item.Patterns.Sum(pattern => pattern.BookCount) - item.MoveLastStackPatterns.Sum(pattern => pattern.BookCount);
                            var needMaxCount = 40 - curStackBookCount;

                            var connectPatterns = nextStack.Patterns.FindAll(pattern => pattern.Color == item.Stack.ConnectColor).OrderBy(it => it.BookCount).ThenBy(it => it.TotallyTime).ToList();

                            foreach (var pattern in connectPatterns)
                            {
                                var curBookCount = item.NextStackPatterns.Sum(it => it.BookCount);
                                if (curBookCount + pattern.BookCount <= needMaxCount)
                                {
                                    pattern.PlanStackName = item.Stack.StackName;
                                    item.NextStackPatterns.Add(pattern);
                                    nextStack.MoveLastStackPatterns.Add(pattern);

                                }
                                else
                                {
                                    break;
                                }
                            }

                        });

                    }


                }

            }

            ////构建垛的板材index
            //var bt = batchStacks.FindAll(item => item.PatternStacks.Exists(i => i.Stack.StackListId == 0));
            //List<PatternStack> bts = bt.SelectMany(item => item.PatternStacks).ToList();
            //bts = bts.FindAll(item => item.Stack.StackListId == 0 && item.StackDetails.Count!=0);
            //var tt =batchStacks.FindAll(item => item.PatternStacks.Exists(it => it.Stack.StackListId == 0));

            //var bgs= BatchGroups.FindAll(item => tt.Exists(i => i.BatchName == item.BatchName));

            Send(BindingData,sender, tBatchGroups);
            Send(BindingData,sender, batchStacks);
            //return;

            #region old code

            //List<BookStack> deviceStacks = CreatedStacks(dis, out List<BatchGroup> batchGroups);
            //foreach (var deviceStack in deviceStacks)
            //{
            //    deviceStack.Stack.PlanDeviceName = dis[Convert.ToInt32(deviceStack.Stack.PlanDeviceName)].DeviceName;
            //}

            //deviceStacks.RemoveAll(item => item.StackDetails.Count == 0);

            ////构建板件顺序
            //deviceStacks.ForEach(item => item.Stack.BookCount = 0);
            //foreach (var group in deviceStacks.GroupBy(item => item.Stack.FirstBatchName))
            //{
            //    var deviceStack = group.First().Stack;
            //    var list = group.ToList();
            //    var batch = group.Key;
            //    var mainColor = deviceStack.MainColor;
            //    var otherColors = BatchInfos.FindAll(item => item.BatchName == batch && item.Color != mainColor);
            //    foreach (var otherColor in otherColors)
            //    {
            //        foreach (var item in list)
            //        {
            //            item.StackDetails.ForEach(i =>
            //            {
            //                if (i.Color == otherColor.Color)
            //                {
            //                    item.Stack.BookCount += 1;
            //                    i.StackIndex = item.Stack.BookCount;
            //                }
            //            });
            //        }
            //    }

            //    foreach (var item in list)
            //    {
            //        item.StackDetails.ForEach(i =>
            //        {
            //            if (i.Color == mainColor)
            //            {
            //                item.Stack.BookCount += 1;
            //                i.StackIndex = item.Stack.BookCount;
            //            }
            //        });
            //    }


            //}

            //DeviceStacks = deviceStacks;
            //BatchGroups = batchGroups;
            //this.Send(BindingData, sender, deviceStacks);
            //this.Send(BindingData, sender, batchGroups);

            #endregion

        }

        #region old code


        //private List<WMSCuttingStackList> ConvertStackList(BookStack deviceStack)
        //{
        //    var stack = deviceStack.Stack;
        //    var stackDetails = deviceStack.StackDetails;
        //    List<WMSCuttingStackList> stackLists = new List<WMSCuttingStackList>();
        //    foreach (var stackDetail in stackDetails)
        //    {
        //        WMSCuttingStackList stackList = new WMSCuttingStackList()
        //        {
        //            PlanDate = stack.PlanDate,
        //            BatchName = stack.FirstBatchName,
        //            CreatedTime = DateTime.Now,
        //            LastUpdatedTime = DateTime.Now,
        //            StackName = stack.StackName,
        //            RawMaterialID = stackDetail.Color,
        //            StackIndex = stack.BookCount + 1 - stackDetail.StackIndex,
        //            WMSStatus = 0,
        //            TaskId = new Guid()
        //        };
        //        stackLists.Add(stackList);
        //    }

        //    return stackLists;
        //}

        ///// <summary>
        ///// 花色建垛
        ///// </summary>
        ///// <param name="tmpBatchInfo"></param>
        ///// <param name="maxBookCount"></param>
        ///// <returns></returns>
        //private List<BookStack> CreatedStacks(TmpBatchInfo tmpBatchInfo, int maxBookCount)
        //{
        //    List<BookStack> stacks = new List<BookStack>();

        //    for (int i = 0; i < tmpBatchInfo.BookCount; i++)
        //    {
        //        BookStack bookStack;
        //        if (i % maxBookCount == 0)
        //        {
        //            var stack = new Stack
        //            {
        //                FirstBatchName = tmpBatchInfo.BatchName,
        //                PlanDate = tmpBatchInfo.PlanDate,
        //                CreatedTime = DateTime.Now,
        //                UpdatedTime = DateTime.Now
        //            };
        //            bookStack = new BookStack() { Stack = stack, StackDetails = new List<StackDetail>() };
        //            stacks.Add(bookStack);
        //        }
        //        else
        //        {
        //            bookStack = stacks[stacks.Count - 1];
        //        }
        //        bookStack.StackDetails.Add(new StackDetail
        //        {
        //            Color = tmpBatchInfo.Color,
        //            PlanDate = tmpBatchInfo.PlanDate,
        //            PlanBatchName = tmpBatchInfo.BatchName,
        //            Status = 0,
        //            CreatedTime = DateTime.Now,
        //            UpdatedTime = DateTime.Now,
        //        });
        //    }

        //    return stacks;
        //}



        ///// <summary>
        ///// 同一个批次内的垛合并
        ///// </summary>
        ///// <param name="deviceStacks"></param>
        ///// <param name="stackCount"></param>
        //private List<BookStack> CombineStack(List<BookStack> deviceStacks, int stackCount = 5)
        //{
        //    if (deviceStacks.Count == stackCount) return deviceStacks;
        //    var avgCount = deviceStacks.Sum(item => item.StackDetails.Count) / stackCount;
        //    var mainColor = deviceStacks[0].Stack.MainColor;
        //    var needCombineStacks = deviceStacks.FindAll(item => item.StackDetails.Count < avgCount);
        //    var subordinateColors =
        //        needCombineStacks.FindAll(item => item.StackDetails.Exists(i => i.Color != mainColor));

        //    var needCombineStackCount = stackCount - deviceStacks.FindAll(item => item.StackDetails.Count >= avgCount).Count;

        //    if (subordinateColors.Count > needCombineStackCount)
        //    {
        //        var combinations = CombinationHelper.NoIntersectionCombination(subordinateColors, needCombineStackCount,
        //            (IEqualityComparer<BookStack>)null);

        //        var tt = combinations.ConvertAll(item => item.CombinationCollection.ConvertAll(i =>
        //        {
        //            var stack = i.CombinationCollection[0].Stack;
        //            var deviceStack = new BookStack()
        //            {
        //                Stack = new Stack()
        //                {
        //                    FirstBatchName = stack.FirstBatchName,
        //                    PlanDate = stack.PlanDate,
        //                    CreatedTime = DateTime.Now,
        //                    UpdatedTime = DateTime.Now,
        //                    MainColor = stack.MainColor,
        //                }
        //            };
        //            i.CombinationCollection.ForEach(t =>
        //            {
        //                deviceStack.StackDetails.AddRange(t.StackDetails);
        //                t.StackDetails.Clear();
        //            });
        //            return deviceStack;
        //        }));

        //        var mainColorStack = needCombineStacks.FirstOrDefault(item => item.StackDetails.Count < avgCount && item.StackDetails.Exists(i => i.Color == mainColor));
        //        List<BookStack> selectCombination;
        //        if (mainColorStack == null)
        //        {
        //            tt = tt.OrderBy(items => items.FindAll(item => item.StackDetails.Count > avgCount).Count).ToList();
        //            selectCombination = tt[0];
        //            while (true)
        //            {
        //                var overflowStacks = selectCombination.FindAll(item => item.StackDetails.Count > avgCount).OrderBy(item => item.StackDetails.Count);
        //                var insufficientStacks = selectCombination.FindAll(item => item.StackDetails.Count < avgCount).OrderBy(item => item.StackDetails.Count);
        //                if (insufficientStacks.Count() == 0) break;
        //                var insufficientStack = insufficientStacks.First();
        //                var overflowStack = overflowStacks.First();
        //                var needCount = avgCount - insufficientStack.StackDetails.Count;
        //                var overCount = overflowStack.StackDetails.Count - avgCount;
        //                var addCount = needCount > overCount ? overCount : needCount;

        //                overflowStack.StackDetails = overflowStack.StackDetails.OrderBy(item => item.Color).ToList();
        //                insufficientStack.StackDetails.AddRange(overflowStack.StackDetails.GetRange(0, addCount));
        //                overflowStack.StackDetails.RemoveRange(0, addCount);
        //            }

        //        }
        //        else
        //        {
        //            tt.RemoveAll(items => items.Exists(item => item.StackDetails.Count > avgCount));
        //            tt = tt.OrderBy(items => items.Max(item => item.StackDetails.GroupBy(i => i.Color).Count())).ToList();
        //            selectCombination = tt[0];
        //            foreach (var deviceStack in selectCombination)
        //            {
        //                var needCount = avgCount - deviceStack.StackDetails.Count;
        //                deviceStack.StackDetails.AddRange(mainColorStack.StackDetails.GetRange(0, needCount));
        //                mainColorStack.StackDetails.RemoveRange(0, needCount);
        //            }
        //        }
        //        deviceStacks.RemoveAll(item => item.StackDetails.Count == 0);
        //        deviceStacks.AddRange(selectCombination);
        //    }
        //    else
        //    {
        //        var mainColorStack = needCombineStacks.FirstOrDefault(item => item.StackDetails.Count < avgCount && item.StackDetails.Exists(i => i.Color == mainColor));
        //        if (mainColorStack == null)
        //        {
        //            var t = subordinateColors[0];
        //            for (int i = 1; i < subordinateColors.Count; i++)
        //            {
        //                t.StackDetails.AddRange(subordinateColors[i].StackDetails);
        //                subordinateColors[i].StackDetails.Clear();
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < subordinateColors.Count; i++)
        //            {
        //                var t = subordinateColors[i];
        //                var needCount = avgCount - t.StackDetails.Count;
        //                t.StackDetails.AddRange(mainColorStack.StackDetails.GetRange(0, needCount));
        //                mainColorStack.StackDetails.RemoveRange(0, needCount);
        //            }

        //            if (mainColorStack.StackDetails.Count > 0)
        //            {
        //                var mainFullStacks = deviceStacks.FindAll(item => item.StackDetails.Count >= avgCount && item.Stack.MainColor == mainColor);
        //                for (int i = 0; i < mainColorStack.StackDetails.Count;)
        //                {
        //                    var detail = mainColorStack.StackDetails[i];
        //                    mainFullStacks[i].StackDetails.Add(detail);
        //                    mainColorStack.StackDetails.Remove(detail);
        //                }
        //            }

        //        }
        //    }

        //    return deviceStacks;
        //}

        ///// <summary>
        ///// 两个批次的垛合并
        ///// </summary>
        ///// <param name="firstDeviceStacks"></param>
        ///// <param name="secondDeviceStacks"></param>
        ///// <param name="maxStackBookCount"></param>
        //private bool CombineBatchStack(ref List<BookStack> firstDeviceStacks, ref List<BookStack> secondDeviceStacks, int maxStackBookCount = 37)
        //{
        //    //确认主花色是否一致
        //    var firstDeviceStack = firstDeviceStacks.FirstOrDefault();
        //    var secondDeviceStack = secondDeviceStacks.FirstOrDefault();
        //    if (firstDeviceStack?.Stack.MainColor == secondDeviceStack?.Stack.MainColor)
        //    {
        //        var firsts = firstDeviceStacks.FindAll(item => item.StackDetails.Count < maxStackBookCount && item.StackDetails.Exists(i => i.Color == firstDeviceStack?.Stack.MainColor))
        //            .OrderBy(item => item.StackDetails.GroupBy(i => i.Color).Count()).ThenBy(item => item.StackDetails.Count);

        //        var seconds = secondDeviceStacks.FindAll(item => item.StackDetails.Exists(i => i.Color == secondDeviceStack?.Stack.MainColor))
        //            .OrderByDescending(item => item.StackDetails.GroupBy(i => i.Color).Count()).ToList();

        //        foreach (var first in firsts)
        //        {
        //            if (seconds.Count == 0) break;
        //            var needCount = maxStackBookCount - first.StackDetails.Count;
        //            var tSecond = seconds.OrderByDescending(item => item.StackDetails.GroupBy(i => i.Color).Count())
        //                .ThenByDescending(item =>
        //                    item.StackDetails.FindAll(i => i.Color == secondDeviceStack?.Stack.MainColor).Count -
        //                    needCount).First();
        //            var mainColors =
        //                tSecond.StackDetails.FindAll(item => item.Color == secondDeviceStack?.Stack.MainColor);
        //            var addMainColors =
        //                mainColors.GetRange(0, mainColors.Count > needCount ? needCount : mainColors.Count);
        //            first.StackDetails.AddRange(addMainColors);
        //            tSecond.StackDetails.RemoveAll(item => addMainColors.Contains(item));
        //            seconds.Remove(tSecond);
        //            tSecond.Stack.PlanDeviceName = first.Stack.PlanDeviceName;
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}




        //private void CreatedStacks(List<Pattern> patterns, int deviceCount = 5)
        //{
        //    var totallyTime = patterns.Sum(item => item.TotallyTime);
        //    var minTime = patterns.Min(item => item.TotallyTime);
        //    var colorsTime = patterns.GroupBy(item => item.Color, pattern => pattern,
        //        (color, ps) => new { Color = color, TotallyTime = ps.Sum(item => item.TotallyTime) }).ToList();
        //    var avgTime = totallyTime / deviceCount;

        //    var mainColors = colorsTime.FindAll(item => item.TotallyTime + minTime > avgTime);

        //}


        //private List<BookStack> CreatedStacks(List<DeviceInfos> dis, out List<BatchGroup> batchGroups)
        //{
        //    var dictionary = CreatedStacks(BatchInfos, dis.Count);
        //    batchGroups = new List<BatchGroup>();
        //    var list = dictionary.ToList();

        //    List<BookStack> deviceStacks = new List<BookStack>();
        //    int stackListId = 0;
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        var first = list[i];
        //        if (i == list.Count - 1)
        //        {
        //            deviceStacks.AddRange(first.Value);
        //            BatchGroup tBatchGroup = new BatchGroup()
        //            {
        //                IsConnect = false,
        //                BatchName = first.Key,
        //                NextBatchName = string.Empty,
        //                StackListId = ++stackListId,
        //                PlanDate = first.Value[0].Stack.PlanDate,
        //                CreatedTime = DateTime.Now,
        //                UpdatedTime = DateTime.Now,
        //                GroupId = 0
        //            };
        //            batchGroups.Add(tBatchGroup);
        //            first.Value.ForEach(item => item.Stack.StackListId = tBatchGroup.StackListId);
        //            break;
        //        }

        //        var second = list[i + 1];
        //        var firstDeviceStacks = first.Value;
        //        if (firstDeviceStacks.Exists(item => string.IsNullOrWhiteSpace(item.Stack.PlanDeviceName)))
        //        {
        //            for (int j = 0; j < firstDeviceStacks.Count; j++)
        //            {
        //                firstDeviceStacks[j].Stack.PlanDeviceName = j.ToString();
        //            }
        //        }
        //        var secondDeviceStacks = second.Value;
        //        var isCombine = CombineBatchStack(ref firstDeviceStacks, ref secondDeviceStacks);

        //        deviceStacks.AddRange(firstDeviceStacks);
        //        if (isCombine)
        //        {
        //            firstDeviceStacks.ForEach(item => item.Stack.SecondBatchName = second.Key);

        //        }
        //        else
        //        {
        //            for (int j = 0; j < secondDeviceStacks.Count; j++)
        //            {
        //                secondDeviceStacks[j].Stack.PlanDeviceName = j.ToString();
        //            }
        //        }
        //        BatchGroup batchGroup = new BatchGroup()
        //        {
        //            IsConnect = isCombine,
        //            BatchName = first.Key,
        //            NextBatchName = isCombine ? second.Key : string.Empty,
        //            PlanDate = first.Value[0].Stack.PlanDate,
        //            StackListId = ++stackListId,
        //            CreatedTime = DateTime.Now,
        //            UpdatedTime = DateTime.Now,
        //            GroupId = 0
        //        };
        //        batchGroups.Add(batchGroup);
        //        first.Value.ForEach(item => item.Stack.StackListId = batchGroup.StackListId);

        //    }

        //    do
        //    {
        //        int groupId = 1;
        //        for (int i = 0; i < batchGroups.Count; i++)
        //        {
        //            var batch = batchGroups[i];
        //            batch.GroupId = groupId;
        //            batch.BatchIndex = i;
        //            if (batch.IsConnect)
        //            {
        //                var next = batchGroups[i + 1];
        //                if (next.BatchName == batch.NextBatchName)
        //                {
        //                    next.FrontBatchName = batch.BatchName;
        //                }
        //            }
        //            else
        //            {
        //                groupId++;
        //            }
        //        }
        //    } while (false);

        //    for (int i = 0; i < deviceStacks.Count; i++)
        //    {
        //        var deviceStack = deviceStacks[i];
        //        string stackName = i.ToString("000");
        //        deviceStack.Stack.StackName = stackName;
        //        deviceStacks.ForEach(item =>
        //        {
        //            item.Stack.FirstBatchBookCount = item.StackDetails
        //                .FindAll(item1 => item1.PlanBatchName == item.Stack.FirstBatchName).Count;
        //            item.Stack.SecondBatchBookCount = item.StackDetails
        //                .FindAll(item1 => item1.PlanBatchName == item.Stack.SecondBatchName).Count;
        //            item.Stack.BookCount = item.StackDetails.Count;
        //        });
        //        deviceStack.StackDetails.ForEach(item => item.StackName = stackName);
        //    }

        //    return deviceStacks;


        //}


        //private Dictionary<string, List<BookStack>> CreatedStacks(List<TmpBatchInfo> tmpBatchInfos, int deviceCount = 5)
        //{
        //    List<TmpBatchInfo> batchInfos = new List<TmpBatchInfo>().Concat(tmpBatchInfos).ToList();

        //    var groups = batchInfos.GroupBy(item => item.Color, bi => bi,
        //        (key, bis) => new
        //        {
        //            Color = key,
        //            BookCount = bis.Sum(item => item.BookCount),
        //            BatchCount = bis.GroupBy(item => item.BatchName).Count(),
        //        }).OrderByDescending(item => item.BookCount).ThenByDescending(item => item.BatchCount).ToList();

        //    var mainColor = batchInfos.GroupBy(item => item.BatchName, bi => bi, (batchName, bis) => new
        //    {
        //        BatchName = batchName,
        //        MainColor = bis.OrderByDescending(i => i.BookCount).ToList()[0].Color,
        //        MainColorBookCount = bis.OrderByDescending(i => i.BookCount).ToList()[0].BookCount,
        //        ColorCount = bis.OrderByDescending(i => i.BookCount).ToList()[0].ColorCount
        //    }).ToList();

        //    var curMainColor = groups[0].Color;
        //    var batchs = mainColor.FindAll(item => item.MainColor == curMainColor);

        //    Dictionary<string, List<BookStack>> dictionary = new Dictionary<string, List<BookStack>>();

        //    foreach (var batch in batchs.OrderBy(item => item.ColorCount).ThenByDescending(item => item.MainColorBookCount))
        //    {
        //        var matchBatchInfos = batchInfos.FindAll(item => item.BatchName == batch.BatchName);
        //        var totalBookCount = matchBatchInfos.Sum(item => item.BookCount);
        //        var avgBook = totalBookCount / deviceCount;
        //        List<BookStack> stacks = new List<BookStack>();
        //        foreach (var batchInfo in matchBatchInfos)
        //        {
        //            stacks.AddRange(CreatedStacks(batchInfo, avgBook));
        //            batchInfos.Remove(batchInfo);
        //        }
        //        stacks.ForEach(item => item.Stack.MainColor = curMainColor);
        //        stacks = CombineStack(stacks, deviceCount);
        //        stacks.RemoveAll(item => item.StackDetails.Count == 0);
        //        dictionary.Add(batch.BatchName, stacks);


        //        #region Old Code

        //        List<DeviceStack> stacks = new List<DeviceStack>();
        //        if (batch.ColorCount == 1)
        //        {
        //            var matchBatchInfo = matchBatchInfos[0];

        //            for (int i = 0; i < deviceCount; i++)
        //            {
        //                stacks.Add(new DeviceStack { Stack = new Stack { FirstBatchName = matchBatchInfo.BatchName, BookCount = 0, MainColor = batch.MainColor } });
        //            }
        //            for (int i = 0; i < matchBatchInfo.BookCount; i++)
        //            {
        //                var deviceStack = stacks.OrderBy(item => item.StackDetails.Count).ToArray()[0];
        //                deviceStack.StackDetails.Add(new StackDetail
        //                {
        //                    Color = matchBatchInfo.Color,
        //                    PlanDate = matchBatchInfo.PlanDate,
        //                    PlanBatchName = matchBatchInfo.BatchName,
        //                    Status = 0,
        //                    CreatedTime = DateTime.Now,
        //                    UpdatedTime = DateTime.Now,
        //                });
        //            }

        //        }
        //        else //if (batch.ColorCount<=3)
        //        {
        //            foreach (var secondaryColor in matchBatchInfos.OrderByDescending(item => item.BookCount))
        //            {
        //                var curColorDeviceCount = secondaryColor.BookCount / avgBook;
        //                for (int i = 0; i < curColorDeviceCount; i++)
        //                {
        //                    var deviceStack = new DeviceStack { Stack = new Stack { FirstBatchName = secondaryColor.BatchName, BookCount = 0, MainColor = batch.MainColor } };
        //                    for (int j = 0; j < avgBook; j++)
        //                    {
        //                        deviceStack.StackDetails.Add(new StackDetail
        //                        {
        //                            Color = secondaryColor.Color,
        //                            PlanDate = secondaryColor.PlanDate,
        //                            PlanBatchName = secondaryColor.BatchName,
        //                            Status = 0,
        //                            CreatedTime = DateTime.Now,
        //                            UpdatedTime = DateTime.Now,
        //                        });
        //                    }
        //                    stacks.Add(deviceStack);
        //                }

        //                var surplusCount = secondaryColor.BookCount - curColorDeviceCount * avgBook;
        //                if (surplusCount > 0)
        //                {
        //                    var tStacks = stacks.FindAll(item => item.StackDetails.Count < avgBook);
        //                    tStacks = stacks.OrderBy(item => item.StackDetails.GroupBy(i => i.Color).Count())
        //                        .ThenByDescending(item => item.StackDetails.Count).ToList();

        //                    foreach (var stack in tStacks)
        //                    {
        //                        var needCount = avgBook - stack.StackDetails.Count;
        //                        needCount = needCount > surplusCount ? surplusCount : needCount;
        //                        surplusCount -= needCount;
        //                        for (int j = 0; j < needCount; j++)
        //                        {
        //                            stack.StackDetails.Add(new StackDetail
        //                            {
        //                                Color = secondaryColor.Color,
        //                                PlanDate = secondaryColor.PlanDate,
        //                                PlanBatchName = secondaryColor.BatchName,
        //                                Status = 0,
        //                                CreatedTime = DateTime.Now,
        //                                UpdatedTime = DateTime.Now,
        //                            });
        //                        }
        //                        if (surplusCount < 1) break;
        //                    }


        //                    if (surplusCount > 0)
        //                    {
        //                        if (stacks.Count == deviceCount) //溢出数量，填充其他垛
        //                        {
        //                            tStacks = stacks.FindAll(item =>
        //                                item.StackDetails.Exists(i => i.Color == secondaryColor.Color));
        //                        }
        //                        else
        //                        {
        //                            var deviceStack = new DeviceStack { Stack = new Stack { FirstBatchName = secondaryColor.BatchName, BookCount = 0, MainColor = batch.MainColor } };
        //                            for (int j = 0; j < surplusCount; j++)
        //                            {
        //                                deviceStack.StackDetails.Add(new StackDetail
        //                                {
        //                                    Color = secondaryColor.Color,
        //                                    PlanDate = secondaryColor.PlanDate,
        //                                    PlanBatchName = secondaryColor.BatchName,
        //                                    Status = 0,
        //                                    CreatedTime = DateTime.Now,
        //                                    UpdatedTime = DateTime.Now,
        //                                });
        //                            }
        //                        }


        //                        stacks.Add(deviceStack);
        //                    }
        //                }
        //            }
        //        }
        //        deviceStacks.AddRange(stacks);

        //        #endregion

        //    }

        //    if (batchInfos.Count > 0)
        //    {
        //        foreach (var keyValue in CreatedStacks(batchInfos, deviceCount))
        //        {
        //            dictionary.Add(keyValue.Key, keyValue.Value);
        //        }
        //    }

        //    return dictionary;

        //}


        #endregion
    }
}
