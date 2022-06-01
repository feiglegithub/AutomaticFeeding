using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum CuttingStackListBatchType
    {
        [Description("自动线")]
        AUTO = 1,
        [Description("柔性线")]
        FLEXIBLE = 2,
        [Description("套铣线")]
        MILLING = 3
    }
}
