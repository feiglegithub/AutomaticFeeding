using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs
{
    public class PcsLogicAlarmArgs: AlarmArgsBase
    {
        public PcsLogicAlarmArgs(string msg)
        {
            Category = "PCS";
            ParamName = "Logic";
            Value = msg;
        }
    }
}
