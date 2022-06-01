using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Core.CuttingTasksManager
{
    public interface ICuttingTaskManager
    {
        /// <summary>
        /// 获取当前任务
        /// </summary>
        /// <param name="deviceName">设备名</param>
        /// <param name="planDate">生产计划时间</param>
        /// <returns></returns>
        SpiltMDBResult GetCurCuttingTask(string deviceName,DateTime planDate);

        /// <summary>
        /// 获取就绪任务
        /// </summary>
        /// <param name="deviceName">设备名</param>
        /// <param name="planDate">生产计划时间</param>
        /// <returns></returns>
        SpiltMDBResult GetNextCuttingTask(string deviceName,DateTime planDate);

        /// <summary>
        /// 获取当前任务的任务信息
        /// </summary>
        /// <param name="deviceName">设备名</param>
        /// <param name="planDate">生产计划时间</param>
        /// <returns></returns>
        CuttingTaskInfos GetCurCuttingTaskInfos(string deviceName, DateTime planDate);

        /// <summary>
        /// 获取就绪任务的任务信息
        /// </summary>
        /// <param name="deviceName">设备名</param>
        /// <param name="planDate">生产计划时间</param>
        /// <returns></returns>
        CuttingTaskInfos GetNextCuttingTaskInfos(string deviceName, DateTime planDate);

        /// <summary>
        /// 检查当前任务是否完成
        /// </summary>
        /// <param name="itemName">堆垛号</param>
        /// <param name="planDate">生产计划时间</param>
        /// <param name="deviceName">设备名</param>
        /// <returns></returns>
        bool CheckTaskIsFinish(string itemName, DateTime planDate, string deviceName="");

    }

}
