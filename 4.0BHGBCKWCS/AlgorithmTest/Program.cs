using Contract;
using System;
using WcsService;
using WcsSortingAlgorithm;

namespace AlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IGroupLinkTaskContract groupLinkTaskContract =  GroupLinkTaskService.GetInstance();
            ISortingTaskStatusContract sortingTaskStatusContract = SortingTaskStatusService.GetInstance();

            var groupLinkTasks = groupLinkTaskContract.GetUnCreatedSolutionGroupLinkTask();
            var sortingTaskStatuses = sortingTaskStatusContract.GetUnCreatedSolutionSortingTaskStatuses();
            
            var obj = CreatedSorting.CreatedSolutions(sortingTaskStatuses, groupLinkTasks);

            var solution1 = CreatedSorting.FilterSolution(obj);

            var details = CreatedSorting.CreatedSortingDetails(solution1, groupLinkTasks);

            var groupId = sortingTaskStatuses[0].GroupId;
            details.ForEach(item=>item.GroupId= groupId);
            var pilers = CreatedSorting.ConvertBindingPilerNoes(solution1, groupId);
            ISortingBindingPilerNoContract sortingBindingPilerNoContract =  SortingBindingPilerNoService.GetInstance();
            sortingBindingPilerNoContract.BulkInsertSortingBindingPilerNo(pilers);
            ISortingRequestGroupContract sortingRequestGroupContract = SortingRequestGroupService.GetInstance();
            sortingRequestGroupContract.UpdatedGroupIsCreatedSolution(pilers.ConvertAll(item => item.GroupId));
            ISortingDetailContract sortingDetailContract = SortingDetailService.GetInstance();
            sortingDetailContract.BulkInsertSortingDetail(details);

            int i = 0;
            foreach (var o in obj)
            {
                i++;
                Console.WriteLine($"复杂度:{o.TotallyComplexity}");

                foreach (var needComplexity in o.ProductCodeNeedComplexity)
                {
                    Console.WriteLine($"花色:{needComplexity.Key}复用复杂度:{needComplexity.Value}");
                }

                foreach (var overflowComplexity in o.ProductCodeOverflowComplexity)
                {
                    Console.WriteLine($"花色:{overflowComplexity.Key}溢出复杂度:{overflowComplexity.Value}");
                }

                foreach (var recycleComplexity in o.ProductCodeRecycleComplexity)
                {
                    Console.WriteLine($"花色:{recycleComplexity.Key}不足复杂度:{recycleComplexity.Value}");
                }

                Console.WriteLine($"方案{i}:");
                foreach (var solution in o.ReverseSolutions)
                {
                    Console.WriteLine($"{solution.ProductCode}-->{solution.SortingTaskId}-->{solution.PilerNo}-->{solution.SortingBookCount}-->{solution.PilerBookCount}");
                }

            }

            //var details = CreatedSorting.CreatedSortingDetails(obj, sortingStationInfos, groupLinkTasks);

            Console.ReadKey();



        }
    }
}
