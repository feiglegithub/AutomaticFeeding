using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client
{
    [IniFile]
    public class CuttingServerSettings : SettingBase<CuttingServerSettings>
    {
        [Property("IsWcf")]
        public bool IsWcf { get; set; }

        /// <summary>
        /// 按花色拆分
        /// </summary>
        [Property("IsRawMaterialID")]
        public bool IsRawMaterialID { get; set; } = false;
        /// <summary>
        /// 是否显示旧的锯切图号
        /// </summary>
        [Property("DisplayOldPTN_INDEX")]
        public bool DisplayOldPTN_INDEX { get; set; } = false;

        /// <summary>
        /// 开料锯监控刷新时间
        /// </summary>
        [Property("RefreshTime")]
        public int RefreshTime { get; set; } = 3000;

        /// <summary>
        /// 当前监控的开料锯
        /// </summary>
        [Property("CurrDeviceName")]
        public string CurrDeviceName { get; set; }

        /// <summary>
        /// 是否自动刷新
        /// </summary>
        [Property("IsAutoRefresh")]
        public bool IsAutoRefresh { get; set; } = true;
    }
}
