using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public enum CheckWay
    {
        /// <summary>
        /// 定时抽检
        /// </summary>
        [Description("定时抽检")]
        Timing=1,
        /// <summary>
        /// 定量抽检
        /// </summary>
        [Description("定量抽检")]
        Quantify=2,
        /// <summary>
        /// 前N抽检
        /// </summary>
        [Description("抽检前N")]
        Top=3,
        
    }
}
