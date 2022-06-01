using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum LogType
    {
        [Description("普通日志")]
        GENERAL,
        [Description("异常日志")]
        ABNORMAL
    }
}
