using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum BatchGroupPlusStatus
    {
        [Description("未生产")]
        NotProduce = 1,
        [Description("待生产")]
        WaitingForProduction = 2,
        [Description("生产中")]
        InProduction = 3,
        [Description("生产完成")]
        ProductionIsCompleted = 4
    }
}
