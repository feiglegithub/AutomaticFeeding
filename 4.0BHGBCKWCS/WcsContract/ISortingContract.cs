using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface ISortingContract
    {
        /// <summary>
        /// 获取待拣选明细
        /// </summary>
        /// <returns></returns>
        //List<SortInfo> GetSortInfos();

        //List<PilerSortingStatus> GetPilerSortingStatuses();

        /// <summary>
        /// 获取拣选工位的信息
        /// </summary>
        /// <returns></returns>
        List<SortingStationInfo> GetSortingStationInfos();

        /// <summary>
        /// 获取未拣选完成的批次列表
        /// </summary>
        /// <returns></returns>
        List<BatchSortingList> GetUnFinishedBatchSortingLists();

        /// <summary>
        /// 拣选任务开始
        /// </summary>
        /// <param name="sortTaskId"></param>
        /// <param name="stackIndex"></param>
        /// <returns></returns>
        bool SortTaskBegin(long sortTaskId,int stackIndex);

        /// <summary>
        /// 拣选任务结束
        /// </summary>
        /// <param name="sortTaskId"></param>
        /// <param name="stackIndex"></param>
        /// <returns></returns>
        bool SortTaskFinished(long sortTaskId,int stackIndex);



    }
}
