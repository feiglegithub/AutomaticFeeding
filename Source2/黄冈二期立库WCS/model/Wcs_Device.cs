using System;

namespace WCS.model
{
    public class Wcs_Device
    {
        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DId { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DCupNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：5	 默认值： 	 允许空：√
        /// </summary>
        public string DType { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DWorkId { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string DPalletId { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DTarget { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DErrorNo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int DTotalCount { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime DUpTime { get; set; }

        public int DItemNo { get; set; }
    }
}
