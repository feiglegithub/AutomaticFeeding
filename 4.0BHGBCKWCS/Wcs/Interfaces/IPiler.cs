using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;
using WCS.model;

namespace WCS.Interfaces
{
    /// <summary>
    /// 堆垛车交互接口
    /// </summary>
    public interface IPiler
    {
        /// <summary>
        /// 堆垛车编号
        /// </summary>
        EPiler Piler { get;  }

        /// <summary>
        /// 堆垛机是否空闲
        /// </summary>
        /// <returns></returns>
        bool IsFree { get; }

        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// 读取堆垛车的垛号
        /// </summary>
        /// <returns></returns>
        string PilerStackName { get; }

        /// <summary>
        /// 给堆垛车写入库任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        bool WritePilerInStockTask(WMS_Task taskInfo);

        /// <summary>
        /// 给堆垛车写出库任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        bool WritePilerOutStockTask(WMS_Task taskInfo);
        /// <summary>
        /// 清除任务
        /// </summary>
        bool ClearTaskFinished();

        /// <summary>
        /// 当前堆垛车所在的列
        /// </summary>
        int CurrentColumn { get; }
    }
}
