using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.CuttingDevice
{
    public enum PLCResultEnum
    {
        [Description("正常")]
        NORMAL = 10,
        [Description("异常")]
        ABNORMAL = 20
    }
}
