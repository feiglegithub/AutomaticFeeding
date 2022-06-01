using System.Collections.Generic;
using WcsModel;

namespace Contract
{
    public interface ISortingTaskStatusContract
    {
        /// <summary>
        /// 获取当前任务的拣选明细
        /// </summary>
        /// <returns></returns>
        List<SortingTaskStatus> GetCurTaskSortingTaskStatuses();
        /// <summary>
        /// 获取当前组的拣选明细
        /// </summary>
        /// <returns></returns>
        List<SortingTaskStatus> GetUnCreatedSolutionSortingTaskStatuses();
    }
}
