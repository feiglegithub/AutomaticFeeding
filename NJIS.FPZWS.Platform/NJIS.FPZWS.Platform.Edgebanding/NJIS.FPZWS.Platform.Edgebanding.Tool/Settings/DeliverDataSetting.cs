using NJIS.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.Platform.Edgebanding.Tool.Settings
{
    [IniFile]
    public class DeliverDataSetting : SettingBase<DeliverDataSetting>
    {
        [Property("general")]
        public string Line { get; set; }

        [Property("cache")]
        public string AdressIP { get; set; }

        [Property("cache")]
        public string AdressIP2 { get; set; }

        [Property("cache")]
        public string SqlConnectIP { get; set; }
        [Property("cache")]
        public string SqlConnectIP2 { get; set; }

    }
}
