using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class Wcs_DdjInfo
    {
        ///<summary>
        /// 属性说明：
        /// 长度：3	 默认值： 	 允许空：√
        /// </summary>
        public int ScNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public bool AlarmStatus { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public bool AutoStatus { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public bool IsFree { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：3	 默认值： 	 允许空：√
        /// </summary>
        public int CColumn { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：3	 默认值： 	 允许空：√
        /// </summary>
        public int CLayer { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int TaskId { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string NPalletID { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public bool TaskStatus { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：80	 默认值： 	 允许空：√
        /// </summary>
        public string ErrorMsg { get; set; }

        //堆垛机一楼出库站台设备编号
        public int StationOut1No { get; set; }
        //堆垛机二楼出库站台设备编号
        public int StationOut2No { get; set; }
        //堆垛机二楼入库站台设备编号
        public int StationInNo { get; set; }

        public Wcs_DdjInfo() { }

        public Wcs_DdjInfo(int _scno, int _stationout1no, int _stationout2no, int _stationinno)
        {
            this.ScNo = _scno;
            this.StationOut1No = _stationout1no;
            this.StationOut2No = _stationout2no;
            this.StationInNo = _stationinno;
        }        
    }
}
