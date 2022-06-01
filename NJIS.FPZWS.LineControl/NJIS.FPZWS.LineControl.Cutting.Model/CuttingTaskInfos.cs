using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 开料任务的相关信息
    /// </summary>
    public class CuttingTaskInfos
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchName { get; set; }
        /// <summary>
        /// 堆垛号
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 小任务号
        /// </summary>
        public Guid TaskDistributeId { get; set; }
        /// <summary>
        /// 大任务号
        /// </summary>
        public Guid TaskId { get; set; }
        /// <summary>
        /// 任务所属设备名
        /// </summary>
        public string DeviceName { get; set; }
    }
}
