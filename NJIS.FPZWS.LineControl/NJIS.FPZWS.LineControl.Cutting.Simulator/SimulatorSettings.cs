using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    [IniFile]
    public class SimulatorSettings:SettingBase<SimulatorSettings>
    {
        [Property("IsWcf")]
        public bool IsWcf { get; set; }
        [Property("PartCount")]
        public int PartCount { get; set; } = 0;

        [Property("BatchWriteLength")]
        public ushort BatchWriteLength { get; set; } = 30;

        [Property("UpiWriteLength")]
        public ushort UpiWriteLength { get; set; } = 20;
    }
}
