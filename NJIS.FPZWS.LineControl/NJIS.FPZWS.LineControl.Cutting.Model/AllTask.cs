using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class AllTask
    {
        /// <summary>
        /// 工件数
        /// </summary>
        public int PartCount { get; set; }
        /// <summary>
        /// 余板数
        /// </summary>
        public int OffPartCount { get; set; }
        /// <summary>
        /// 小任务号
        /// </summary>
        public Guid TaskDistributeId { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchName { get; set; }
        /// <summary>
        /// 锯切图号
        /// </summary>
        public short PTN_INDEX { get; set; }
        /// <summary>
        /// 物料花色
        /// </summary>
        public string RawMaterialID { get; set; }
        /// <summary>
        /// 单次锯切时间
        /// </summary>
        public int CYCLE_TIME { get; set; }
        /// <summary>
        /// 需要锯切的板件数
        /// </summary>
        public int BookNum { get; set; }
        /// <summary>
        /// 局切图总时间
        /// </summary>
        public int TotalTime { get; set; }
        /// <summary>
        /// 切割周期数
        /// </summary>
        public int CutTimes { get; set; }
        public string ItemName { get; set; }
        public int MdbCount { get; set; }
        public bool IsError { get; set; }
        public string Msg { get; set; }
    }
}
