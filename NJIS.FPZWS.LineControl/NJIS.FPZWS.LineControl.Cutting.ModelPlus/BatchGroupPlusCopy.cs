using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public class BatchGroupPlusCopy
    {
        public long LineId { get; set; }
        public System.DateTime PlanDate { get; set; }
        public string BatchName { get; set; }
        public int BatchIndex { get; set; }
        public System.DateTime? StartLoadingTime { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
