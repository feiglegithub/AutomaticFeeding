using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IWmsTaskContract
    {
        /// <summary>
        /// 获取板材入库任务
        /// </summary>
        /// <returns></returns>
        List<WMS_Task> GetBoardInStockTask();

        List<WMS_Task> GetTaskByFromStationNo(int fromStationNo, int taskType);

        /// <summary>
        /// 获取指定垛未完成的任务
        /// </summary>
        /// <param name="stackNo">垛号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns></returns>
        List<WMS_Task> GetWmsTasksByStackNo(int stackNo, int taskType = 0);

        /// <summary>
        /// 更新wms任务
        /// </summary>
        /// <param name="wmsTasks"></param>
        /// <returns></returns>
        bool BulkUpdateWmsTasks(List<WMS_Task> wmsTasks);

        /// <summary>
        /// 更新wms任务
        /// </summary>
        /// <param name="wmsTask"></param>
        /// <returns></returns>
        bool UpdateWmsTask(WMS_Task wmsTask);

        /// <summary>
        /// 通过请求号获取任务
        /// </summary>
        /// <param name="reqId"></param>
        /// <returns></returns>
        List<WMS_Task> GetWmsTasksByReqId(long reqId);

        /// <summary>
        /// 获取堆垛车的未完成任务
        /// </summary>
        /// <param name="ddjNo"></param>
        /// <param name="taskType"></param>
        /// <returns></returns>
        List<WMS_Task> GetWmsTasksByDdjNo(int ddjNo, int taskType = 0);

        /// <summary>
        /// 获取wms任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        List<WMS_Task> GetWmsTasksByTaskId(long taskId);

        /// <summary>
        /// 获取Wms下发的Rgv任务
        /// </summary>
        /// <returns></returns>
        List<WMS_Task> GetRgvWmsTasks();

        /// <summary>
        /// 根据垛号获取Wms下发的Rgv任务
        /// </summary>
        /// <param name="stackNo">垛号</param>
        /// <returns></returns>
        List<WMS_Task> GetRgvWmsTasksByStackNo(int stackNo);

        /// <summary>
        /// 获取补空托盘的任务
        /// </summary>
        /// <returns></returns>
        List<WMS_Task> GetEmptyPadTasks();



    }
}
