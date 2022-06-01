using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum TriggerType
    {
        //public static string PLC = "PLC";
        //public static string LINE_CONTROL = "LineControl";
        [Description("PLC")]
        PLC = 0,
        [Description("线控")]
        LineControl = 1
    }
}
