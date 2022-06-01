using NJIS.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    [IniFile]
    public class CuttingSetting:SettingBase<CuttingSetting>
    {
        [Property("core")]
        public string CuttingBuilder { get; set; } = "NJIS.FPZWS.LineControl.Cutting.Core.DefaultCuttingBuilder,NJIS.FPZWS.LineControl.Cutting.Core";

        [Property("OnePartByteLength")]
        public int OnePartByteLength { get; set; } = 22;
    }
}
