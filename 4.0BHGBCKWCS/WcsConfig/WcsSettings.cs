using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NJIS.Ini;

namespace WcsConfig
{
    [IniFile]
    public class WcsSettings:SettingBase<WcsSettings>
    {
        [Property("DbConnectString")]
        public string DbConnectString { get; set; }
        [Property("RequestWaiting")]
        public int RequestWaiting { get; set; } = 60;
    }
}
