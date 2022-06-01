using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs
{
    public class PlcAlarmArgs: AlarmArgsBase
    {
        public PlcAlarmArgs(string paramName, string msg)
        {
            Category = "PLC";
            ParamName = paramName;
            Value = msg;
        }
    }
}
