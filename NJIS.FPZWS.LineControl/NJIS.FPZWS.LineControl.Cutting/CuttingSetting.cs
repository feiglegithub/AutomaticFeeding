using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Cutting.UI
{
    [IniFile]
    public class CuttingSetting:SettingBase<CuttingSetting>
    {
        [Property("DeviceName")]
        public string CurrDeviceName { get; set; }
        [Property("SawPath")]
        public string SawPath { get; set; }
        [Property("TmpMDBPath")]
        public string TmpMDBPath { get; set; }
        [Property("SearchPattern")]
        public string SearchPattern { get; set; }
        [Property("IsWcf")]
        public bool IsWcf { get; set; }
        [Property("DisplayOldPTN_INDEX")]
        public bool DisplayOldPTN_INDEX { get; set; }

        [Property("RefreshTime")]
        public int RefreshTime { get; set; } = 3000;
        [Property("StartPositionX")]
        public int StartPositionX { get; set; } = 300;
        [Property("StartPositionY")]
        public int StartPositionY { get; set; } = 0;
        [Property("DefaultMinModel")]
        public bool DefaultMinModel { get; set; } = true;
    }
}
