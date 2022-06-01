using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs
{
    public class PcsErrorAlarmArgs:AlarmArgsBase
    {
        private const string _Category = "PCS";
        private const string _ParamName = "Error";
        public PcsErrorAlarmArgs(string msg)
        {
            ParamName = _ParamName;
            Value = msg;
            Category = _Category;
        }
    }
}
