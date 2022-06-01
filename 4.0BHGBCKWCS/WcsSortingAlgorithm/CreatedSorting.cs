using System.Collections.Generic;
using System.Linq;
using WcsModel;

namespace WcsSortingAlgorithm
{
    public class CreatedSorting
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortingTaskStatuses">拣选明细</param>
        /// <param name="groupLinkTasks">组申请要料信息</param>
        /// <returns></returns>
        public static List<Solution> CreatedSolutions(List<SortingTaskStatus> sortingTaskStatuses,List<GroupLinkTask> groupLinkTasks)
        {
            var list = sortingTaskStatuses.GroupBy(item => new { item.SortingTaskId, item.ProductCode }, (key, source) =>
                new
                {
                    key.SortingTaskId,
                    key.ProductCode,
                    //ProductCodeCount = source.Count(),
                    SumBookCount = source.Sum(item => item.NeedBookCount),
                    ColorIndex=source.Min(item=>item.ColorIndex),
                }
            ).ToList();
            var ll = list.GroupBy(item => new {item.SortingTaskId}, (key, source) => new
            {
                key.SortingTaskId,
                ProductCodeCount = source.Count(),
            }).ToList();
            var oneProductCodell = ll.FindAll(item => item.ProductCodeCount == 1);
            //获取单花色的垛
            var oneProductCode = list.FindAll(item => oneProductCodell.Exists(item1=>item1.SortingTaskId==item.SortingTaskId) );
            //获取混花色的垛
            var mixProductCode = list.FindAll(item => !oneProductCodell.Exists(item1 => item1.SortingTaskId == item.SortingTaskId));
            //联合当前所有可以拣选的要料垛信息
            //var pilerInfos = sortingStationInfos.ConvertAll(item => new
            //{
            //    TaskId = (long) 0,item.StationNo,ProductCode = item.Color
            //    ,item.BookCount,item.PilerNo
            //}).Concat(groupLinkTasks.ConvertAll(item => new
            //{
            //    item.TaskId,StationNo = 0,item.ProductCode,
            //    item.BookCount,item.PilerNo
            //})).ToList();

            var pilerInfos = groupLinkTasks.ConvertAll(item => new
            {
                item.TaskId,
                StationNo = 0,
                item.ProductCode,
                item.BookCount,
                item.PilerNo
            });

            List<Solution> solutionTuples = new List<Solution>();
            List<TmpSolution> tCanReverseSortingTasks =  new List<TmpSolution>();
            if (oneProductCode.Count > 0)
            {
                //筛选支持反拣选的任务
                var canReverseSortingTasks = from one in oneProductCode
                    from info in pilerInfos
                    where one.ProductCode == info.ProductCode && one.SumBookCount <= info.BookCount
                                                              && one.SumBookCount > info.BookCount - one.SumBookCount
                    select new TmpSolution
                    {
                        ProductCode = one.ProductCode,
                        SortingTaskId = one.SortingTaskId,
                        SortingBookCount = one.SumBookCount,
                        PilerNo = info.PilerNo,
                        PilerBookCount = info.BookCount,
                        TaskId = info.TaskId,
                        OperationCount = info.BookCount - one.SumBookCount,
                    };

                tCanReverseSortingTasks = canReverseSortingTasks.ToList();
            }
            
            if (tCanReverseSortingTasks.Count > 0)
            {
                
                //生成反拣选方案
                List <PermutationResult<TmpSolution>> solutions = PermutationAlgorithm.Permutation(tCanReverseSortingTasks, (first, second) =>
                    first.SortingTaskId != second.SortingTaskId && first.PilerNo != second.PilerNo);
                //最大支持反拣选的板垛数
                var maxReverseSortingTaskCount = solutions.Max(item => item.PermutationCollection.Count);
                //最大支持反拣选板垛数的所有方案
                var maxReverseSolutions =
                    solutions.FindAll(item => item.PermutationCollection.Count == maxReverseSortingTaskCount);
                //对正拣选的方案进行分组计算，每个方案的每个花色需要正向拣选的复杂度
                
                foreach (var solution in maxReverseSolutions)
                {
                    //获取当前方案中剩余正向拣选的垛信息
                    var reverseSolution = solution.PermutationCollection;
                    var forwardSortingTasks= oneProductCode.FindAll(item =>
                        !reverseSolution.Exists(item1 => item.SortingTaskId == item1.SortingTaskId));

                    //计算正向拣选各花色的拣选复杂度
                    var forwardNeedBook = forwardSortingTasks.GroupBy(item => item.ProductCode, (productCode, source) => new
                    {
                        ProductCode = productCode,
                        NeedBookCount = source.Sum(item => item.SumBookCount)
                    });

                    var reverseOperationCount = reverseSolution.GroupBy(item => item.ProductCode, (productCode, source) => new
                    {
                        ProductCode = productCode,
                        OperationCount = source.Sum(item => item.OperationCount)
                    });

                    //计算当前方案总复杂度  全连接
                    //左连接
                    var leftJoin = reverseOperationCount.GroupJoin(forwardNeedBook, leftItem => leftItem.ProductCode,
                        rightItem => rightItem.ProductCode,
                        (leftItem, rightList) => new {leftItem, rightList}).SelectMany(pp => pp.rightList.DefaultIfEmpty(), (item, rightItem) => new
                    {
                        ForwardProductCode = rightItem == null ? "" : rightItem.ProductCode,
                        ReverseProductCode = item.leftItem.ProductCode,
                        NeedBookCount = rightItem?.NeedBookCount ?? 0,
                        item.leftItem.OperationCount
                    }); 
                    //右连接
                    var rightJoin = forwardNeedBook.GroupJoin(reverseOperationCount, rightItem => rightItem.ProductCode,
                        leftItem => leftItem.ProductCode,
                        (rightItem, leftList) => new {rightItem, leftList}).SelectMany(pp => pp.leftList.DefaultIfEmpty(), (item, leftItem) => new
                    {
                        ForwardProductCode = item.rightItem.ProductCode,
                        ReverseProductCode = leftItem==null?"":leftItem.ProductCode,
                        NeedBookCount = item.rightItem.NeedBookCount,
                        OperationCount=leftItem?.OperationCount??0
                    });
                    //全连接
                    var fullJoin = leftJoin.Union(rightJoin);
                    int complexity = 0;
                    Dictionary<string,int> recycleCount = new Dictionary<string, int>();
                    Dictionary<string, int> overflowCount = new Dictionary<string, int>();
                    Dictionary<string, int> needCount = new Dictionary<string, int>();
                    Dictionary<string, int> supplementPiler = new Dictionary<string, int>();
                    foreach (var full in fullJoin)
                    {
                        if (full.ForwardProductCode == null)//正拣选中没有此反拣选花色
                        {
                            if (overflowCount.ContainsKey(full.ForwardProductCode))
                            {
                                overflowCount[full.ForwardProductCode] += full.OperationCount;
                            }
                            else
                            {
                                overflowCount.Add(full.ForwardProductCode, full.OperationCount);
                            }
                            
                            complexity += full.OperationCount;
                        }
                        else if (full.ReverseProductCode==null)//反拣选中没有此正拣选花色
                        {
                            if (needCount.ContainsKey(full.ReverseProductCode))
                            {
                                needCount[full.ReverseProductCode] += full.NeedBookCount;
                            }
                            else
                            {
                                needCount.Add(full.ReverseProductCode,full.NeedBookCount);
                            }
                            
                            complexity += full.NeedBookCount;
                        }
                        else
                        {
                            if (!recycleCount.ContainsKey(full.ForwardProductCode))
                            {
                                recycleCount.Add(full.ForwardProductCode,0);
                            }
                            recycleCount[full.ForwardProductCode] += full.OperationCount >= full.NeedBookCount
                                ? full.NeedBookCount
                                : full.OperationCount;
                            if (!needCount.ContainsKey(full.ForwardProductCode))
                            {
                                needCount.Add(full.ForwardProductCode,0);
                            }
                            needCount[full.ForwardProductCode] += full.OperationCount >= full.NeedBookCount
                                ? 0
                                : full.NeedBookCount - full.OperationCount;
                            complexity += full.OperationCount >= full.NeedBookCount
                                ? full.OperationCount
                                : full.NeedBookCount;
                        }
                    }

                    foreach (var keyValuePair in recycleCount.ToList().FindAll(item => item.Value == 0))
                    {
                        recycleCount.Remove(keyValuePair.Key);
                    }

                    foreach (var keyValuePair in overflowCount.ToList().FindAll(item => item.Value == 0))
                    {
                        overflowCount.Remove(keyValuePair.Key);
                    }

                    foreach (var keyValuePair in needCount.ToList().FindAll(item => item.Value == 0))
                    {
                        needCount.Remove(keyValuePair.Key);
                    }
                    Dictionary<string, int> supplementPilerSurplusBookCount = new Dictionary<string, int>();
                    if (needCount.Count > 0)
                    {
                        //去除反向拣选后剩余的要料垛
                        var surplusPilers = pilerInfos.FindAll(item => !solution.PermutationCollection.Exists(reverse => reverse.PilerNo == item.PilerNo));
                        int surplusBookCount = 0;
                        //给每个单花色进行构建溢出
                        foreach (var keyValue in needCount)
                        {
                            var curProductCode = keyValue.Key;
                            var curNeedCount = keyValue.Value;
                            //构建填充组合
                            var permutations = PermutationAlgorithm.Permutation(surplusPilers.FindAll(item => item.ProductCode == curProductCode));
                            permutations = permutations.FindAll(item =>
                                item.PermutationCollection.Sum(item1 => item1.BookCount) >= curNeedCount);
                            var choosePermutation = permutations
                                .OrderBy(item => item.PermutationCollection.Sum(item1 => item1.BookCount)).ToList()[0];
                            foreach (var value in choosePermutation.PermutationCollection)
                            {
                                supplementPiler.Add(curProductCode,value.PilerNo);
                            }

                            surplusBookCount = choosePermutation.PermutationCollection.Sum(item => item.BookCount) -curNeedCount;
                            if (surplusBookCount > 0)
                            {
                                supplementPilerSurplusBookCount.Add(curProductCode,surplusBookCount);
                            }
                        }
                    }
                    solutionTuples.Add(new Solution {
                        ReverseSolutions = solution.PermutationCollection,
                        TotallyComplexity = complexity+needCount.Sum(item=>item.Value),
                        ProductCodeRecycleComplexity = recycleCount,
                        ProductCodeOverflowComplexity = overflowCount,
                        ProductCodeNeedComplexity = needCount,
                        SupplementPiler = supplementPiler,
                        SupplementPilerSurplusBookCount = supplementPilerSurplusBookCount
                    });
                }

            }
            else
            {
                var tSolution = new Solution
                {
                    ReverseSolutions = new List<TmpSolution>(),
                    ForwardMixProductCodeTasks = sortingTaskStatuses.FindAll(item=>!oneProductCode.Exists(i=>i.SortingTaskId== item.SortingTaskId)),
                    ForwardOneProductCodeTasks = sortingTaskStatuses.FindAll(item => oneProductCode.Exists(i => i.SortingTaskId == item.SortingTaskId)),
                    TotallyComplexity = 0,
                    ProductCodeRecycleComplexity = new Dictionary<string, int>(),
                    ProductCodeOverflowComplexity = new Dictionary<string, int>(),
                    ProductCodeNeedComplexity = new Dictionary<string, int>(),
                    SupplementPiler = new Dictionary<string, int>(),
                    SupplementPilerSurplusBookCount = new Dictionary<string, int>()
                };
                solutionTuples.Add(tSolution);
            }


            foreach (var solution in solutionTuples)
            {
                var forwardOneProduct = oneProductCode.FindAll(item =>
                    !solution.ReverseSolutions.Exists(item1 => item1.SortingTaskId == item.SortingTaskId));
                solution.ForwardOneProductCodeTasks = sortingTaskStatuses.FindAll(item => forwardOneProduct.Exists(item1 => item1.SortingTaskId == item.SortingTaskId));

                solution.ForwardMixProductCodeTasks = sortingTaskStatuses.FindAll(item =>
                    mixProductCode.Exists(item1 => item1.SortingTaskId == item.SortingTaskId));
            }

            return solutionTuples;
        }
        /// <summary>
        /// 要料垛使用情况
        /// </summary>
        internal class PilerNoUseInfo
        {
            public int PilerNo { get; set; } = 0;
            public string ProductCode { get; set; } = "";
            public int CanUsedBookCount { get; set; } = 0;
            public long TaskId { get; set; } = (long) 0;
            public bool IsReverseSortingPilerNo { get; set; } = false;
            public long ParallelSortingTaskId { get; set; } = (long) 0;
        }

        private static List<SortingDetail> CreatedSortingDetails(List<PilerNoUseInfo> pilerNoUseInfos,
            List<SortingTaskStatus> usortingTaskStatuses, List<GroupLinkTask> groupLinkTasks, List<TmpSolution> reverseSolutions)
        {
            var tUsortingTaskStatuses =
                usortingTaskStatuses == null ? new List<SortingTaskStatus>() : usortingTaskStatuses;
            var sortingTaskStatuses = new List<SortingTaskStatus>().Concat(tUsortingTaskStatuses).ToList();
            List<SortingDetail> sortingDetails = new List<SortingDetail>();
            if (sortingTaskStatuses.Count > 0)
            {
                var productCodeGroup = sortingTaskStatuses.GroupBy(item => item.ProductCode, (productCode, source) => new
                {
                    ProductCode = productCode,
                    SumBookCount = source.Sum(item => item.NeedBookCount)
                }).OrderBy(item => item.SumBookCount);

                foreach (var group in productCodeGroup)
                {
                    var curProductCode = group.ProductCode;

                    var curProductCodeTasks = sortingTaskStatuses
                        .FindAll(item => item.ProductCode == curProductCode)
                        .OrderByDescending(item => item.NeedBookCount);

                    foreach (var sortingTaskId in curProductCodeTasks)
                    {
                        var curSortingTaskId = sortingTaskId.SortingTaskId;
                        var tasks = sortingTaskStatuses.FindAll(item => item.SortingTaskId == curSortingTaskId).OrderBy(item => item.ColorIndex).ToList();
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            SortingTaskStatus task = tasks[i];
                            var needBookCount = task.NeedBookCount;
                            while (needBookCount > 0)
                            {
                                var v = pilerNoUseInfos.FirstOrDefault(item =>
                                    item.ProductCode == task.ProductCode && item.CanUsedBookCount > 0);
                                if (v == null)
                                {
                                    var tmpList = groupLinkTasks.FindAll(item =>
                                        !pilerNoUseInfos.Exists(item1 => item1.PilerNo == item.PilerNo));
                                    var groupLinkTask = tmpList.FirstOrDefault(item =>
                                        !reverseSolutions.Exists(item1 => item1.PilerNo == item.PilerNo) && item.ProductCode== task.ProductCode);
                                    //是否有反拣选垛
                                    bool isReverseSorting = false;
                                    long parallelSortingTaskId = 0;
                                    TmpSolution reverseSolution = null;
                                    int canUsedBookCount = 0;
                                    if (groupLinkTask == null)
                                    {
                                        groupLinkTask = tmpList[0];
                                        isReverseSorting = true;
                                        reverseSolution = reverseSolutions.First(item => item.PilerNo == groupLinkTask.PilerNo);
                                        parallelSortingTaskId = reverseSolution.SortingTaskId;
                                        canUsedBookCount = reverseSolution.OperationCount;
                                    }
                                    else
                                    {
                                        canUsedBookCount = groupLinkTask.BookCount;
                                    }

                                    if (canUsedBookCount >= needBookCount)
                                    {
                                        pilerNoUseInfos.Add(new PilerNoUseInfo
                                        {
                                            PilerNo = groupLinkTask.PilerNo,
                                            ProductCode = groupLinkTask.ProductCode,
                                            CanUsedBookCount = canUsedBookCount - needBookCount,
                                            TaskId = groupLinkTask.TaskId,
                                            IsReverseSortingPilerNo = isReverseSorting,
                                            ParallelSortingTaskId = parallelSortingTaskId
                                        });

                                        sortingDetails.Add(new SortingDetail()
                                        {
                                            IsReverseSorting = isReverseSorting,
                                            MainPilerNo = 0,
                                            ParallelPilerNo = 0,
                                            StartPilerNo = groupLinkTask.PilerNo,
                                            SortingTaskId = task.SortingTaskId,
                                            ProductCode = task.ProductCode,
                                            BookCount = canUsedBookCount,
                                            OperationCount = needBookCount,
                                            ParallelSortingTaskId = parallelSortingTaskId,
                                            MainPilerIsFinished = ((i + 1) == tasks.Count),
                                            ParallelPilerIsFinished = isReverseSorting && canUsedBookCount == needBookCount
                                        });
                                        if (i + 1 == tasks.Count)
                                        {
                                            sortingTaskStatuses.RemoveAll(item=>item.SortingTaskId== task.SortingTaskId);
                                        }
                                        needBookCount = 0;
                                    }
                                    else
                                    {
                                        pilerNoUseInfos.Add(new PilerNoUseInfo
                                        {
                                            PilerNo = groupLinkTask.PilerNo,
                                            ProductCode = groupLinkTask.ProductCode,
                                            CanUsedBookCount = 0,
                                            TaskId = groupLinkTask.TaskId,
                                            IsReverseSortingPilerNo = isReverseSorting,
                                            ParallelSortingTaskId = parallelSortingTaskId
                                        });

                                        sortingDetails.Add(new SortingDetail()
                                        {
                                            IsReverseSorting = isReverseSorting,
                                            MainPilerNo = 0,
                                            ParallelPilerNo = 0,
                                            StartPilerNo = groupLinkTask.PilerNo,
                                            SortingTaskId = task.SortingTaskId,
                                            ProductCode = task.ProductCode,
                                            BookCount = canUsedBookCount,
                                            OperationCount = canUsedBookCount,
                                            ParallelSortingTaskId = parallelSortingTaskId,
                                            MainPilerIsFinished = false,
                                            ParallelPilerIsFinished = isReverseSorting
                                        });

                                        needBookCount -= canUsedBookCount;
                                    }
                                }
                                else
                                {
                                    if (v.CanUsedBookCount >= needBookCount)
                                    {
                                        sortingDetails.Add(new SortingDetail()
                                        {
                                            IsReverseSorting = v.IsReverseSortingPilerNo,
                                            MainPilerNo = 0,
                                            ParallelPilerNo = 0,
                                            StartPilerNo = v.PilerNo,
                                            SortingTaskId = task.SortingTaskId,
                                            ProductCode = task.ProductCode,
                                            BookCount = v.CanUsedBookCount,
                                            OperationCount = needBookCount,
                                            ParallelSortingTaskId = v.ParallelSortingTaskId,
                                            MainPilerIsFinished = ((i + 1) == tasks.Count),
                                            ParallelPilerIsFinished = v.IsReverseSortingPilerNo && v.CanUsedBookCount == needBookCount
                                        });
                                        v.CanUsedBookCount = v.CanUsedBookCount - needBookCount;
                                        if (i + 1 == tasks.Count)
                                        {
                                            sortingTaskStatuses.RemoveAll(item => item.SortingTaskId == task.SortingTaskId);
                                        }
                                        
                                        needBookCount = 0;
                                        
                                    }
                                    else
                                    {
                                        sortingDetails.Add(new SortingDetail()
                                        {
                                            IsReverseSorting = v.IsReverseSortingPilerNo,
                                            MainPilerNo = 0,
                                            ParallelPilerNo = 0,
                                            StartPilerNo = v.PilerNo,
                                            SortingTaskId = task.SortingTaskId,
                                            ProductCode = task.ProductCode,
                                            OperationCount = v.CanUsedBookCount,
                                            BookCount = v.CanUsedBookCount,
                                            ParallelSortingTaskId = v.ParallelSortingTaskId,
                                            MainPilerIsFinished = false,
                                            ParallelPilerIsFinished = v.IsReverseSortingPilerNo
                                        });

                                        needBookCount -= v.CanUsedBookCount;
                                        v.CanUsedBookCount = 0;
                                    }
                                }
                            }

                        }

                    }



                }
            }

            return sortingDetails;
        }

        /// <summary>
        /// 创建拣选明细
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="groupLinkTasks"></param>
        /// <returns></returns>
        public static List<SortingDetail> CreatedSortingDetails(Solution solution,
             List<GroupLinkTask> groupLinkTasks)
        {
            List<SortingDetail> sortingDetails = new List<SortingDetail>();
            var reverseSolutions = solution.ReverseSolutions;
            //当前垛板材使用的情况
            var productCodeInfos = new List<PilerNoUseInfo>();
            //混花色任务
            var forwardMixTasks = solution.ForwardMixProductCodeTasks;
            var forwardOneTasks = solution.ForwardOneProductCodeTasks;
            // 构建混花色拣选的任务
            sortingDetails.AddRange(CreatedSortingDetails(productCodeInfos,forwardMixTasks, groupLinkTasks,solution.ReverseSolutions));

            // 构建单花色拣选的任务
            sortingDetails.AddRange(CreatedSortingDetails(productCodeInfos, forwardOneTasks, groupLinkTasks, solution.ReverseSolutions));

            // 构建反拣选的任务

            //获取未完成的反拣选任务垛
            var unFinishedReversePilers = productCodeInfos.FindAll(item =>
                item.CanUsedBookCount > 0 && reverseSolutions.Exists(item1 => item1.PilerNo == item.PilerNo));
            var unBeginReverseTasks = reverseSolutions.FindAll(item =>
                !sortingDetails.Exists(item1 => item.SortingTaskId == item1.ParallelSortingTaskId));
            foreach (var unFinished in unFinishedReversePilers)
            {
                var curProductCode = unFinished.ProductCode;
                sortingDetails.Add(new SortingDetail()
                {
                    IsReverseSorting = true,
                    MainPilerNo = 0,
                    ParallelPilerNo = 0,
                    StartPilerNo = unFinished.PilerNo,
                    SortingTaskId = 0,
                    BookCount = unFinished.CanUsedBookCount,
                    ProductCode = unFinished.ProductCode,
                    OperationCount = unFinished.CanUsedBookCount,
                    ParallelSortingTaskId = unFinished.ParallelSortingTaskId,
                    MainPilerIsFinished = false,
                    ParallelPilerIsFinished = true
                });
                unFinished.CanUsedBookCount = 0;
                foreach (var unBegin in unBeginReverseTasks.FindAll(item=>item.ProductCode==curProductCode))
                {
                    sortingDetails.Add(new SortingDetail()
                    {
                        IsReverseSorting = true,
                        MainPilerNo = 0,
                        ParallelPilerNo = 0,
                        StartPilerNo = unBegin.PilerNo,
                        SortingTaskId = 0,
                        BookCount = unBegin.OperationCount,
                        ProductCode = unBegin.ProductCode,
                        OperationCount = unBegin.OperationCount,
                        ParallelSortingTaskId = unBegin.SortingTaskId,
                        MainPilerIsFinished = false,
                        ParallelPilerIsFinished = true
                    });
                    productCodeInfos.Add(new PilerNoUseInfo()
                    {
                        CanUsedBookCount = 0,
                        IsReverseSortingPilerNo = true,
                        ParallelSortingTaskId = unBegin.SortingTaskId,
                        PilerNo = unBegin.PilerNo,
                        ProductCode = unBegin.ProductCode,
                        TaskId = 0
                    });
                }
            }

            unBeginReverseTasks = reverseSolutions.FindAll(item =>
                !sortingDetails.Exists(item1 => item.SortingTaskId == item1.ParallelSortingTaskId));
            foreach (var group in unBeginReverseTasks.GroupBy(item => item.ProductCode))
            {
                var curProductCode = group.Key;
                foreach (var unBegin in unBeginReverseTasks.FindAll(item => item.ProductCode == curProductCode))
                {
                    sortingDetails.Add(new SortingDetail()
                    {
                        IsReverseSorting = true,
                        MainPilerNo = 0,
                        ParallelPilerNo = 0,
                        StartPilerNo = unBegin.PilerNo,
                        SortingTaskId = 0,
                        BookCount = unBegin.OperationCount,
                        ProductCode = unBegin.ProductCode,
                        OperationCount = unBegin.OperationCount,
                        ParallelSortingTaskId = unBegin.SortingTaskId,
                        MainPilerIsFinished = false,
                        ParallelPilerIsFinished = true
                    });
                    productCodeInfos.Add(new PilerNoUseInfo()
                    {
                        CanUsedBookCount = 0,
                        IsReverseSortingPilerNo = true,
                        ParallelSortingTaskId = unBegin.SortingTaskId,
                        PilerNo = unBegin.PilerNo,
                        ProductCode = unBegin.ProductCode,
                        TaskId = 0
                    });
                }
            }

            return sortingDetails;



        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="solutions">方案</param>
        /// <param name="sortingStationInfos"></param>
        /// <param name="groupLinkTasks"></param>
        /// <returns></returns>
        public static List<SortingDetail> CreatedSortingDetails(List<Solution> solutions, List<SortingStationInfo> sortingStationInfos, List<GroupLinkTask> groupLinkTasks)
        {
            var solution = FilterSolution(solutions);
            List<SortingDetail> sortingDetails = new List<SortingDetail>();

            //联合当前所有可以拣选的要料垛信息
            var pilerInfos = sortingStationInfos.ConvertAll(item => new
            {
                TaskId = (long)0,item.StationNo,ProductCode = item.Color,
                item.BookCount,item.PilerNo
            }).Concat(groupLinkTasks.ConvertAll(item => new
            {
                item.TaskId,StationNo = 0,item.ProductCode,
                item.BookCount,item.PilerNo
            })).ToList();

            var surplusPilers = pilerInfos.FindAll(item=>!solution.ReverseSolutions.Exists(reverse=>reverse.PilerNo== item.PilerNo));
            if (solution.SupplementPiler.Count > 0)
            {
                surplusPilers = surplusPilers.FindAll(item => !solution.SupplementPiler.ContainsValue(item.PilerNo));
            }
            Dictionary<string,List<SortingDetail>> sortingDetailDictionary = new Dictionary<string, List<SortingDetail>>();
            //计算溢出数量
            Dictionary<string,int> supplementPilerSurplusBookCount = new Dictionary<string, int>();
            foreach (var keyValue in solution.ProductCodeOverflowComplexity)
            {
                supplementPilerSurplusBookCount.Add(keyValue.Key,keyValue.Value);
            }

            foreach (var keyValue in solution.SupplementPilerSurplusBookCount)
            {
                supplementPilerSurplusBookCount.Add(keyValue.Key, keyValue.Value);
            }

            foreach (var keyValue in supplementPilerSurplusBookCount)
            {
                var curProductCode = keyValue.Key;
                var reverses = solution.ReverseSolutions.FindAll(item => item.ProductCode == curProductCode);
                var forwards = solution.ForwardOneProductCodeTasks.FindAll(item => item.ProductCode == curProductCode);

                var pilers = pilerInfos.FindAll(item => item.ProductCode == curProductCode);

                pilers.RemoveAll(item =>
                    !reverses.Exists(item1 => item1.PilerNo == item.PilerNo) &&
                    !solution.SupplementPiler.ContainsValue(item.PilerNo));
                var enumerator = forwards.GetEnumerator();
                Dictionary<SortingTaskStatus, int> sDictionary = new Dictionary<SortingTaskStatus, int>();
                for (int i = 0; i < pilers.Count; i++)
                {
                    var piler = pilers[i];
                    //垛剩余板材数
                    var pilerBookCount = piler.BookCount;
                    while (true)
                    {
                        SortingTaskStatus sortingTaskStatus = null;
                        int needCount = 0;
                        if (sDictionary.Values.Any(item => item > 0))
                        {
                            var v = sDictionary.First(item => item.Value > 0);
                            sortingTaskStatus = v.Key;
                        }
                        else
                        {
                            if (enumerator.MoveNext())
                            {
                                sortingTaskStatus = enumerator.Current;
                                sDictionary.Add(sortingTaskStatus, sortingTaskStatus.NeedBookCount);
                            }
                        }

                        if (sortingTaskStatus != null)
                        {
                            needCount = sDictionary[sortingTaskStatus];
                        }
                        
                        var reverse = reverses.FirstOrDefault(item => item.PilerNo == piler.PilerNo);

                        if (reverse != null) //反向
                        {
                            long parallelSortingTaskId = 0;
                            if (sortingTaskStatus == null)//无正向拣选任务
                            {
                                parallelSortingTaskId = 0;
                                if (!sortingDetailDictionary.ContainsKey(curProductCode))
                                {
                                    sortingDetailDictionary.Add(curProductCode,new List<SortingDetail>());
                                }
                                sortingDetailDictionary[curProductCode].Add(new SortingDetail()
                                { IsReverseSorting = true, MainPilerNo = 0,
                                    ParallelPilerNo = 0, StartPilerNo = reverse.PilerNo,
                                    SortingTaskId = reverse.SortingTaskId,
                                    ProductCode = reverse.ProductCode,
                                    OperationCount = reverse.OperationCount,
                                    ParallelSortingTaskId = parallelSortingTaskId });
                                break;
                            }
                            else//有正向拣选
                            {
                                if (needCount >= reverse.OperationCount)
                                {
                                    parallelSortingTaskId = sortingTaskStatus.SortingTaskId;
                                    if (!sortingDetailDictionary.ContainsKey(curProductCode))
                                    {
                                        sortingDetailDictionary.Add(curProductCode, new List<SortingDetail>());
                                    }
                                    sortingDetailDictionary[curProductCode].Add(new SortingDetail()
                                    {
                                        IsReverseSorting = true,
                                        MainPilerNo = 0,
                                        ParallelPilerNo = 0,
                                        StartPilerNo = reverse.PilerNo,
                                        SortingTaskId = reverse.SortingTaskId,
                                        ProductCode = reverse.ProductCode,
                                        OperationCount = reverse.OperationCount,
                                        ParallelSortingTaskId = parallelSortingTaskId,
                                        ParallelPilerIsFinished = needCount==reverse.OperationCount,
                                        MainPilerIsFinished = true//主垛完成

                                    });
                                    sDictionary[sortingTaskStatus] = needCount - reverse.OperationCount;
                                    break;
                                }
                                else
                                {
                                    parallelSortingTaskId = sortingTaskStatus.SortingTaskId;
                                    if (!sortingDetailDictionary.ContainsKey(curProductCode))
                                    {
                                        sortingDetailDictionary.Add(curProductCode, new List<SortingDetail>());
                                    }
                                    sortingDetailDictionary[curProductCode].Add(new SortingDetail()
                                    {
                                        IsReverseSorting = true,
                                        MainPilerNo = 0,
                                        ParallelPilerNo = 0,
                                        StartPilerNo = reverse.PilerNo,
                                        SortingTaskId = reverse.SortingTaskId,
                                        ProductCode = reverse.ProductCode,
                                        OperationCount = needCount,
                                        ParallelSortingTaskId = parallelSortingTaskId,
                                        ParallelPilerIsFinished = true,//当前并行垛完成
                                        MainPilerIsFinished = false

                                    });
                                    reverse.OperationCount -= needCount;
                                    sDictionary[sortingTaskStatus] = 0;
                                }
                            }
                        }
                        else// 正向
                        {
                            if (sortingTaskStatus == null) //无正向拣选任务
                            {
                                break;
                            }

                            if (needCount >= pilerBookCount)
                            {
                                if (!sortingDetailDictionary.ContainsKey(curProductCode))
                                {
                                    sortingDetailDictionary.Add(curProductCode, new List<SortingDetail>());
                                }
                                sortingDetailDictionary[curProductCode].Add(new SortingDetail()
                                {
                                    IsReverseSorting = false,
                                    MainPilerNo = 0,
                                    ParallelPilerNo = 0,
                                    StartPilerNo = piler.PilerNo,
                                    SortingTaskId = sortingTaskStatus.SortingTaskId,
                                    ProductCode = sortingTaskStatus.ProductCode,
                                    OperationCount = pilerBookCount,
                                    ParallelSortingTaskId = 0,
                                    MainPilerIsFinished = needCount==pilerBookCount
                                });
                                sDictionary[sortingTaskStatus] -= pilerBookCount;
                                pilerBookCount = 0;
                                break;
                                
                            }
                            else
                            {
                                if (!sortingDetailDictionary.ContainsKey(curProductCode))
                                {
                                    sortingDetailDictionary.Add(curProductCode, new List<SortingDetail>());
                                }
                                sortingDetailDictionary[curProductCode].Add(new SortingDetail()
                                {
                                    IsReverseSorting = false,
                                    MainPilerNo = 0,
                                    ParallelPilerNo = 0,
                                    StartPilerNo = piler.PilerNo,
                                    SortingTaskId = sortingTaskStatus.SortingTaskId,
                                    ProductCode = sortingTaskStatus.ProductCode,
                                    OperationCount = needCount,
                                    ParallelSortingTaskId = 0,
                                    MainPilerIsFinished = true
                                });
                                pilerBookCount -= needCount;
                                sDictionary[sortingTaskStatus] = 0;
                            }
                        }
                    }
                }

            }

            foreach (var keyValue in sortingDetailDictionary)
            {
                sortingDetails.AddRange(keyValue.Value);
            }

            return sortingDetails;
        }

        public static Solution FilterSolution(List<Solution> solutions)
        {
            //筛选方案
            var chooseSolution = solutions.OrderBy(item => item.TotallyComplexity)
                .ThenByDescending(item => item.ReverseSolutions.Count)
                .ThenBy(item => item.ProductCodeOverflowComplexity.Sum(keyValue => keyValue.Value))
                .ThenByDescending(item => item.ProductCodeRecycleComplexity.Sum(keyValue => keyValue.Value))
                .ThenBy(item => item.ProductCodeNeedComplexity.Sum(keyValue => keyValue.Value)).ToArray()[0];
            return chooseSolution;
        }

        public static List<SortingBindingPilerNo> ConvertBindingPilerNoes(Solution solution,long groupId)
        {
            List<SortingBindingPilerNo> sortingBindingPilerNoes = new List<SortingBindingPilerNo>();
            foreach (var tmpSolution in solution.ReverseSolutions)
            {
                sortingBindingPilerNoes.Add(new SortingBindingPilerNo()
                {
                    GroupId = groupId,
                    PilerNo = tmpSolution.PilerNo,
                    ProductCode = tmpSolution.ProductCode,
                    SortingTaskId = tmpSolution.SortingTaskId
                });
            }

            return sortingBindingPilerNoes;
        }
    }

    public class Solution
    {
        /// <summary>
        /// 反拣选的方案信息
        /// </summary>
        public List<TmpSolution> ReverseSolutions { get; set; }=new List<TmpSolution>();

        /// <summary>
        /// 正向拣选的单花色任务
        /// </summary>
        public List<SortingTaskStatus> ForwardOneProductCodeTasks { get; set; }=new List<SortingTaskStatus>();

        /// <summary>
        /// 正向拣选的混花色任务
        /// </summary>
        public List<SortingTaskStatus> ForwardMixProductCodeTasks { get; set; } = new List<SortingTaskStatus>();

        /// <summary>
        /// 总复杂度
        /// </summary>
        public int TotallyComplexity { get; set; }
        /// <summary>
        /// 复用复杂度
        /// </summary>
        public Dictionary<string,int> ProductCodeRecycleComplexity { get; set; }
        /// <summary>
        /// 溢出复杂度
        /// </summary>
        public Dictionary<string, int> ProductCodeOverflowComplexity { get; set; }
        /// <summary>
        /// 不足复杂度
        /// </summary>
        public Dictionary<string, int> ProductCodeNeedComplexity { get; set; }
        /// <summary>
        /// 补充垛（正向拣选）
        /// </summary>
        public Dictionary<string,int> SupplementPiler { get; set; }
        /// <summary>
        /// 补充后 花色剩余数量
        /// </summary>
        public Dictionary<string,int> SupplementPilerSurplusBookCount { get; set; }
    }

    
}
