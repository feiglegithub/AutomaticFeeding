using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class LED_Content
    {
        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：
        /// </summary>
        public int LID { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：200	 默认值： 	 允许空：√
        /// </summary>
        public string LContent { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值：((1)) 	 允许空：√
        /// </summary>
        public int LStatus { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int LPort { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int LType { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：23	 默认值：(getdate()) 	 允许空：√
        /// </summary>
        public DateTime LCreateDate { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime LSendDate { get; set; }

        public string LMsg { get; set; }
        public string LIP { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public short Port { get; set; }
        //LED屏幕ID
        public int DeviceId { get; set; }
    }
}
