using System.Collections.Generic;
using WcsModel;

namespace Contract
{
    public interface IRgvContract
    {
        /// <summary>
        /// 获取未开始的任务
        /// </summary>
        /// <returns></returns>
        List<RGV_Task> GetUnBeingRgvTasks();

        bool UpdateRgvTasks(List<RGV_Task> rgvTasks);

        bool UpdateRgvTask(RGV_Task rgvTask);

        int InsertRgvTask(RGV_Task rgvTask);

        int BulkInsertRgvTasks(List<RGV_Task> rgvTasks);

        /// <summary>
        /// 通过垛号获取未完成的堆垛车任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        List<RGV_Task> GetRgvTasksByTaskId(int taskId);
    }
}
