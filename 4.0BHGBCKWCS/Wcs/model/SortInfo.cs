using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    /// <summary>
    /// 拣选明细
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        public long TaskId { get; set; }
        /// <summary>
        /// 花色
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 板材所在垛的位置
        /// </summary>
        public int StackIndex { get; set; }
        /// <summary>
        /// 拣选状态
        /// </summary>
        public int SortStatus { get; set; }
        /// <summary>
        /// 拣选时间
        /// </summary>
        public DateTime SortTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishedTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
    }
}
