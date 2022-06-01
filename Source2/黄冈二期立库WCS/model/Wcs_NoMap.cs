using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class Wcs_NoMap
    {
        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int WMSNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DeviceNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int BufferC { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int BufferM { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public string SType { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：1	 默认值： 	 允许空：√
        /// </summary>
        public bool State { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：100	 默认值： 	 允许空：√
        /// </summary>
        public string Remark { get; set; }

    }

}
