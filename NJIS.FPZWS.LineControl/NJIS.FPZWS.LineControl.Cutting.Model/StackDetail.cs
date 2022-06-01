using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 垛明细
    /// </summary>
    public class StackDetail
    {
        public long LineId { get; set; }
        public string StackName { get; set; }
        public string Color { get; set; }
        public int StackIndex { get; set; }
        public int Status { get; set; }
        public int PatternId { get; set; }
        public string PlanBatchName { get; set; }
        public string ActualBatchName { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
