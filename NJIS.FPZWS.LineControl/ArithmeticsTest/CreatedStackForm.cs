using Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.WinControls.UI;

namespace ArithmeticsTest
{
    public partial class CreatedStackForm : Telerik.WinControls.UI.RadForm
    {
        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = new LineControlCuttingService());
        public CreatedStackForm()
        {
            InitializeComponent();
            ViewInit();
            this.gvbStack.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
        }

        private void RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is Stack stack)
            {
                var stackDetails = DeviceStacks.FindAll(item => item.Stack.StackName==stack.StackName).SelectMany(item => item.StackDetails);
                gvbStackDetail.DataSource = stackDetails;
            }
        }

        private void ViewInit()
        {
            TmpBatchInfo t;
            gvbPatterns.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="批次", FieldName=nameof(t.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(t.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材数", FieldName=nameof(t.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件数", FieldName=nameof(t.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="余料数", FieldName=nameof(t.OffPartCount), DataType=typeof(int), ReadOnly=true },
            });

            Stack s;
            gvbStack.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="垛号", FieldName=nameof(s.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="一批", FieldName=nameof(s.FirstBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="一批数", FieldName=nameof(s.FirstBatchBookCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="二批", FieldName=nameof(s.SecondBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="二批数", FieldName=nameof(s.SecondBatchBookCount), DataType=typeof(string), ReadOnly=true },
                
                new ColumnInfo(){ HeaderText="主花色", FieldName=nameof(s.MainColor), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(){ HeaderText="板材数", FieldName=nameof(s.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="设备", FieldName=nameof(s.PlanDeviceName), DataType=typeof(int), ReadOnly=true },
                
            });
            StackDetail sd;
            gvbStackDetail.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="垛号", FieldName=nameof(sd.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="批次", FieldName=nameof(sd.PlanBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(sd.Color), DataType=typeof(string), ReadOnly=true },
            });
        }

        private List<TmpBatchInfo> BatchInfos { get; set; } = new List<TmpBatchInfo>();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var patterns = LineControlCuttingContract.GetPatternsByPlanDate(DateTime.Parse("2020-02-20"));

            var groups = patterns.GroupBy(item => new {item.BatchName, item.Color}, p => p, (key, tPatterns) =>
                new TmpBatchInfo
                {
                    PlanDate = tPatterns.ToList()[0].PlanDate,
                    BatchName=key.BatchName,
                    Color=key.Color, BookCount = tPatterns.Sum(i => i.BookCount),
                    PartCount = tPatterns.Sum(i => i.PartCount), OffPartCount = tPatterns.Sum(i => i.OffPartCount)
                });

            BatchInfos = groups.OrderBy(item=>item.BatchName).ThenBy(item=>item.Color).ToList();
            gvbPatterns.DataSource = BatchInfos;
        }

        private List<BookStack> DeviceStacks { get; set; }

        private void btnCreatedStack_Click(object sender, EventArgs e)
        {
            DeviceStacks = CreatedStacks(out List<BatchGroup> batchGroups);

            gvbStack.DataSource = DeviceStacks.ConvertAll(item => item.Stack);
            
        }


        
        private List<BookStack> CreatedStacks(out List<BatchGroup> batchGroups)
        {
            var dictionary = CreatedStacks(BatchInfos, 5);
            batchGroups = new List<BatchGroup>();
            var list = dictionary.ToList();
            List<BookStack> deviceStacks = new List<BookStack>();
            for (int i = 0; i < list.Count; i++)
            {
                var first = list[i];
                if (i == list.Count - 1)
                {
                    deviceStacks.AddRange(first.Value);
                    break;
                }
                
                var second = list[i + 1];
                var firstDeviceStacks = first.Value;
                if (firstDeviceStacks.Exists(item => string.IsNullOrWhiteSpace(item.Stack.PlanDeviceName)))
                {
                    for (int j = 0; j < firstDeviceStacks.Count; j++)
                    {
                        firstDeviceStacks[j].Stack.PlanDeviceName = j.ToString();
                    }
                }
                var secondDeviceStacks = second.Value;
                var isCombine = CombineBatchStack(ref firstDeviceStacks,ref secondDeviceStacks);

                deviceStacks.AddRange(firstDeviceStacks);
                if (isCombine)
                {
                    firstDeviceStacks.ForEach(item=>item.Stack.SecondBatchName = second.Key);
                    
                }
                else
                {
                    for (int j = 0; j < secondDeviceStacks.Count; j++)
                    {
                        secondDeviceStacks[j].Stack.PlanDeviceName = j.ToString();
                    }
                }
                BatchGroup batchGroup = new BatchGroup()
                {
                    IsConnect = isCombine,
                    BatchName = first.Key,
                    NextBatchName = isCombine?second.Key:string.Empty,
                    PlanDate = first.Value[0].Stack.PlanDate,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    GroupId = 0
                };
                batchGroups.Add(batchGroup);

            }

            do
            {
                int groupId = 1;
                for (int i = 0; i < batchGroups.Count;i++)
                {
                    var batch = batchGroups[i];
                    batch.GroupId = groupId;
                    batch.BatchIndex = i;
                    if (batch.IsConnect)
                    {
                        var next = batchGroups[i + 1];
                        if (next.BatchName == batch.NextBatchName)
                        {
                            next.FrontBatchName = batch.BatchName;
                        }
                    }
                    else
                    {
                        groupId++;
                    }
                }
            } while (false);

            for (int i = 0; i < deviceStacks.Count; i++)
            {
                var deviceStack = deviceStacks[i];
                string stackName = i.ToString("000");
                deviceStack.Stack.StackName = stackName;
                deviceStacks.ForEach(item =>
                    {
                        item.Stack.FirstBatchBookCount = item.StackDetails
                            .FindAll(item1 => item1.PlanBatchName == item.Stack.FirstBatchName).Count;
                        item.Stack.SecondBatchBookCount = item.StackDetails
                            .FindAll(item1 => item1.PlanBatchName == item.Stack.SecondBatchName).Count;
                        item.Stack.BookCount = item.StackDetails.Count;
                    });
                deviceStack.StackDetails.ForEach(item=>item.StackName=stackName);
            }

            return deviceStacks;


        }

        private Dictionary<string, List<BookStack>> CreatedStacks(List<TmpBatchInfo> tmpBatchInfos,int deviceCount=5)
        {
            List<TmpBatchInfo> batchInfos = new List<TmpBatchInfo>().Concat(tmpBatchInfos).ToList();

            var groups = batchInfos.GroupBy(item => item.Color, bi => bi,
                (key, bis) => new
                {
                    Color = key,
                    BookCount = bis.Sum(item => item.BookCount),
                    BatchCount = bis.GroupBy(item => item.BatchName).Count(),
                }).OrderByDescending(item => item.BookCount).ThenByDescending(item => item.BatchCount).ToList();

            var mainColor = batchInfos.GroupBy(item => item.BatchName, bi => bi, (batchName, bis) => new
            {
                BatchName = batchName,
                MainColor = bis.OrderByDescending(i => i.BookCount).ToList()[0].Color,
                MainColorBookCount= bis.OrderByDescending(i => i.BookCount).ToList()[0].BookCount,
                ColorCount= bis.OrderByDescending(i => i.BookCount).ToList()[0].ColorCount
            }).ToList();

            var curMainColor = groups[0].Color;
            var batchs = mainColor.FindAll(item => item.MainColor == curMainColor);

            Dictionary<string, List<BookStack>> dictionary = new Dictionary<string, List<BookStack>>();

            foreach (var batch in batchs.OrderBy(item=>item.ColorCount).ThenByDescending(item=>item.MainColorBookCount))
            {
                var matchBatchInfos =batchInfos.FindAll(item => item.BatchName == batch.BatchName);
                var totalBookCount = matchBatchInfos.Sum(item => item.BookCount);
                var avgBook = totalBookCount / deviceCount;
                List<BookStack> stacks = new List<BookStack>();
                foreach (var batchInfo in matchBatchInfos)
                {
                    stacks.AddRange(CreatedStack(batchInfo, avgBook));
                    batchInfos.Remove(batchInfo);
                }
                stacks.ForEach(item => item.Stack.MainColor = curMainColor);
                stacks = CombineStack(stacks, deviceCount);
                stacks.RemoveAll(item => item.StackDetails.Count == 0);
                dictionary.Add(batch.BatchName,stacks);
                

                #region Old Code

                //List<DeviceStack> stacks = new List<DeviceStack>();
                //if (batch.ColorCount == 1)
                //{
                //    var matchBatchInfo = matchBatchInfos[0];

                //    for (int i = 0; i < deviceCount; i++)
                //    {
                //        stacks.Add(new DeviceStack { Stack = new Stack { FirstBatchName = matchBatchInfo.BatchName, BookCount = 0, MainColor = batch.MainColor } });
                //    }
                //    for (int i = 0; i < matchBatchInfo.BookCount; i++)
                //    {
                //        var deviceStack = stacks.OrderBy(item => item.StackDetails.Count).ToArray()[0];
                //        deviceStack.StackDetails.Add(new StackDetail
                //        {
                //            Color = matchBatchInfo.Color,
                //            PlanDate = matchBatchInfo.PlanDate,
                //            PlanBatchName = matchBatchInfo.BatchName,
                //            Status = 0,
                //            CreatedTime = DateTime.Now,
                //            UpdatedTime = DateTime.Now,
                //        });
                //    }

                //}
                //else //if (batch.ColorCount<=3)
                //{
                //    foreach (var secondaryColor in matchBatchInfos.OrderByDescending(item => item.BookCount))
                //    {
                //        var curColorDeviceCount = secondaryColor.BookCount / avgBook;
                //        for (int i = 0; i < curColorDeviceCount; i++)
                //        {
                //            var deviceStack = new DeviceStack { Stack = new Stack { FirstBatchName = secondaryColor.BatchName, BookCount = 0, MainColor = batch.MainColor } };
                //            for (int j = 0; j < avgBook; j++)
                //            {
                //                deviceStack.StackDetails.Add(new StackDetail
                //                {
                //                    Color = secondaryColor.Color,
                //                    PlanDate = secondaryColor.PlanDate,
                //                    PlanBatchName = secondaryColor.BatchName,
                //                    Status = 0,
                //                    CreatedTime = DateTime.Now,
                //                    UpdatedTime = DateTime.Now,
                //                });
                //            }
                //            stacks.Add(deviceStack);
                //        }

                //        var surplusCount = secondaryColor.BookCount - curColorDeviceCount * avgBook;
                //        if (surplusCount > 0)
                //        {
                //            var tStacks = stacks.FindAll(item => item.StackDetails.Count < avgBook);
                //            tStacks = stacks.OrderBy(item => item.StackDetails.GroupBy(i => i.Color).Count())
                //                .ThenByDescending(item => item.StackDetails.Count).ToList();

                //            foreach (var stack in tStacks)
                //            {
                //                var needCount = avgBook - stack.StackDetails.Count;
                //                needCount = needCount > surplusCount ? surplusCount : needCount;
                //                surplusCount -= needCount;
                //                for (int j = 0; j < needCount; j++)
                //                {
                //                    stack.StackDetails.Add(new StackDetail
                //                    {
                //                        Color = secondaryColor.Color,
                //                        PlanDate = secondaryColor.PlanDate,
                //                        PlanBatchName = secondaryColor.BatchName,
                //                        Status = 0,
                //                        CreatedTime = DateTime.Now,
                //                        UpdatedTime = DateTime.Now,
                //                    });
                //                }
                //                if (surplusCount < 1) break;
                //            }


                //            if (surplusCount > 0)
                //            {
                //                if (stacks.Count == deviceCount) //溢出数量，填充其他垛
                //                {
                //                    tStacks = stacks.FindAll(item =>
                //                        item.StackDetails.Exists(i => i.Color == secondaryColor.Color));
                //                }
                //                else
                //                {
                //                    var deviceStack = new DeviceStack { Stack = new Stack { FirstBatchName = secondaryColor.BatchName, BookCount = 0, MainColor = batch.MainColor } };
                //                    for (int j = 0; j < surplusCount; j++)
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
                //                }


                //                stacks.Add(deviceStack);
                //            }
                //        }
                //    }
                //}
                //deviceStacks.AddRange(stacks);

                #endregion

            }

            if (batchInfos.Count > 0)
            {
                foreach (var keyValue in CreatedStacks(batchInfos, deviceCount))
                {
                    dictionary.Add(keyValue.Key,keyValue.Value);
                }
            }

            return dictionary;
            
        }

        /// <summary>
        /// 花色建垛
        /// </summary>
        /// <param name="tmpBatchInfo"></param>
        /// <param name="maxBookCount"></param>
        /// <returns></returns>
        private List<BookStack> CreatedStack(TmpBatchInfo tmpBatchInfo,int maxBookCount)
        {
            List<BookStack> stacks = new List<BookStack>();

            for (int i = 0; i < tmpBatchInfo.BookCount; i++)
            {
                BookStack deviceStack;
                if (i % maxBookCount == 0)
                {
                    var stack = new Stack
                    {
                        FirstBatchName = tmpBatchInfo.BatchName,
                        PlanDate = tmpBatchInfo.PlanDate,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now
                    };
                    deviceStack = new BookStack(){Stack = stack,StackDetails = new List<StackDetail>()};
                    stacks.Add(deviceStack);
                }
                else
                {
                    deviceStack = stacks[stacks.Count - 1];
                }
                deviceStack.StackDetails.Add(new StackDetail
                {
                    Color = tmpBatchInfo.Color,
                    PlanDate = tmpBatchInfo.PlanDate,
                    PlanBatchName = tmpBatchInfo.BatchName,
                    Status = 0,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                });
            }

            return stacks;
        }

        /// <summary>
        /// 同一个批次内的垛合并
        /// </summary>
        /// <param name="deviceStacks"></param>
        /// <param name="stackCount"></param>
        private List<BookStack> CombineStack(List<BookStack> deviceStacks,int stackCount=5)
        {
            if(deviceStacks.Count==stackCount) return deviceStacks;
            var avgCount = deviceStacks.Sum(item => item.StackDetails.Count)/stackCount;
            var mainColor = deviceStacks[0].Stack.MainColor;
            var needCombineStacks = deviceStacks.FindAll(item => item.StackDetails.Count < avgCount);
            var subordinateColors =
                needCombineStacks.FindAll(item => item.StackDetails.Exists(i => i.Color != mainColor));
            
            var needCombineStackCount = stackCount - deviceStacks.FindAll(item => item.StackDetails.Count >= avgCount).Count;

            if (subordinateColors.Count > needCombineStackCount)
            {
                var combinations = CombinationHelper.NoIntersectionCombination(subordinateColors, needCombineStackCount,
                    (IEqualityComparer<BookStack>) null);

                var tt = combinations.ConvertAll(item => item.CombinationCollection.ConvertAll(i =>
                {
                    var stack = i.CombinationCollection[0].Stack;
                    var deviceStack = new BookStack()
                    {
                        Stack = new Stack()
                        {
                            FirstBatchName = stack.FirstBatchName, PlanDate = stack.PlanDate,
                            CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now, MainColor = stack.MainColor,
                        }
                    };
                    i.CombinationCollection.ForEach(t =>
                    {
                        deviceStack.StackDetails.AddRange(t.StackDetails);
                        t.StackDetails.Clear();
                    });
                    return deviceStack;
                }));

                var mainColorStack = needCombineStacks.FirstOrDefault(item => item.StackDetails.Count < avgCount && item.StackDetails.Exists(i => i.Color == mainColor));
                List<BookStack> selectCombination;
                if (mainColorStack == null)
                {
                    tt = tt.OrderBy(items => items.FindAll(item=>item.StackDetails.Count>avgCount).Count).ToList();
                    selectCombination = tt[0];
                    while (true)
                    {
                        var overflowStacks = selectCombination.FindAll(item => item.StackDetails.Count > avgCount).OrderBy(item => item.StackDetails.Count);
                        var insufficientStacks = selectCombination.FindAll(item => item.StackDetails.Count < avgCount).OrderBy(item => item.StackDetails.Count);
                        if(insufficientStacks.Count()==0) break;
                        var insufficientStack = insufficientStacks.First();
                        var overflowStack = overflowStacks.First();
                        var needCount = avgCount - insufficientStack.StackDetails.Count;
                        var overCount = overflowStack.StackDetails.Count - avgCount;
                        var addCount = needCount > overCount ? overCount : needCount;

                        overflowStack.StackDetails = overflowStack.StackDetails.OrderBy(item => item.Color).ToList();
                        insufficientStack.StackDetails.AddRange(overflowStack.StackDetails.GetRange(0, addCount));
                        overflowStack.StackDetails.RemoveRange(0,addCount);
                    }

                }
                else
                {
                    tt.RemoveAll(items => items.Exists(item => item.StackDetails.Count > avgCount));
                    tt = tt.OrderBy(items => items.Max(item => item.StackDetails.GroupBy(i => i.Color).Count())).ToList();
                    selectCombination = tt[0];
                    foreach (var deviceStack in selectCombination)
                    {
                        var needCount = avgCount - deviceStack.StackDetails.Count;
                        deviceStack.StackDetails.AddRange(mainColorStack.StackDetails.GetRange(0,needCount));
                        mainColorStack.StackDetails.RemoveRange(0, needCount);
                    }
                }
                deviceStacks.RemoveAll(item => item.StackDetails.Count == 0);
                deviceStacks.AddRange(selectCombination);
            }
            else
            {
                var mainColorStack = needCombineStacks.FirstOrDefault(item=>item.StackDetails.Count<avgCount && item.StackDetails.Exists(i=>i.Color == mainColor));
                if (mainColorStack == null)
                {
                    var t = subordinateColors[0];
                    for (int i = 1; i < subordinateColors.Count; i++)
                    {
                        t.StackDetails.AddRange(subordinateColors[i].StackDetails);
                        subordinateColors[i].StackDetails.Clear();
                    }
                }
                else
                {
                    for (int i = 0; i < subordinateColors.Count; i++)
                    {
                        var t = subordinateColors[i];
                        var needCount = avgCount - t.StackDetails.Count;
                        t.StackDetails.AddRange(mainColorStack.StackDetails.GetRange(0,needCount));
                        mainColorStack.StackDetails.RemoveRange(0, needCount);
                    }

                    if (mainColorStack.StackDetails.Count > 0)
                    {
                        var mainFullStacks = deviceStacks.FindAll(item => item.StackDetails.Count >= avgCount && item.Stack.MainColor == mainColor);
                        for (int i = 0; i < mainColorStack.StackDetails.Count;)
                        {
                            var detail = mainColorStack.StackDetails[i];
                            mainFullStacks[i].StackDetails.Add(detail);
                            mainColorStack.StackDetails.Remove(detail);
                        }
                    }
                   
                }
            }

            return deviceStacks;
        }

        /// <summary>
        /// 两个批次的垛合并
        /// </summary>
        /// <param name="firstDeviceStacks"></param>
        /// <param name="secondDeviceStacks"></param>
        /// <param name="maxStackBookCount"></param>
        private bool CombineBatchStack(ref List<BookStack> firstDeviceStacks,ref List<BookStack> secondDeviceStacks,int maxStackBookCount=37)
        {
            //确认主花色是否一致
            var firstDeviceStack = firstDeviceStacks.FirstOrDefault();
            var secondDeviceStack = secondDeviceStacks.FirstOrDefault();
            if (firstDeviceStack?.Stack.MainColor == secondDeviceStack?.Stack.MainColor)
            {
                var firsts = firstDeviceStacks.FindAll(item =>item.StackDetails.Count< maxStackBookCount && item.StackDetails.Exists(i => i.Color == firstDeviceStack?.Stack.MainColor))
                    .OrderBy(item => item.StackDetails.GroupBy(i => i.Color).Count()).ThenBy(item=>item.StackDetails.Count);

                var seconds = secondDeviceStacks.FindAll(item => item.StackDetails.Exists(i => i.Color == secondDeviceStack?.Stack.MainColor))
                    .OrderByDescending(item => item.StackDetails.GroupBy(i => i.Color).Count()).ToList();

                foreach (var first in firsts)
                {
                    if(seconds.Count==0) break;
                    var needCount = maxStackBookCount - first.StackDetails.Count;
                    var tSecond = seconds.OrderByDescending(item => item.StackDetails.GroupBy(i => i.Color).Count())
                        .ThenByDescending(item =>
                            item.StackDetails.FindAll(i => i.Color == secondDeviceStack?.Stack.MainColor).Count -
                            needCount).First();
                    var mainColors =
                        tSecond.StackDetails.FindAll(item => item.Color == secondDeviceStack?.Stack.MainColor);
                    var addMainColors =
                        mainColors.GetRange(0, mainColors.Count > needCount ? needCount : mainColors.Count);
                    first.StackDetails.AddRange(addMainColors);
                    tSecond.StackDetails.RemoveRange(0, mainColors.Count > needCount ? needCount : mainColors.Count);
                    seconds.Remove(tSecond);
                    tSecond.Stack.PlanDeviceName = first.Stack.PlanDeviceName;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        //internal class DeviceStack
        //{
        //    public Stack Stack { get; set; }

        //    public List<StackDetail> StackDetails { get; set; }=new List<StackDetail>();
        //}
    }



   
    
}
