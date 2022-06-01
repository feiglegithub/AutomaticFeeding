using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public enum RequestLoadingStatus
    {
        /// <summary>
        /// 请求上料
        /// </summary>
        [Description("请求上料")]
        RequestLoad=10,
        /// <summary>
        /// 上料中
        /// </summary>
        [Description("上料中")]
        WmsLoading =20,
        /// <summary>
        /// 上料完成
        /// </summary>
        [Description("上料完成")]
        Loaded =30
    }
}
