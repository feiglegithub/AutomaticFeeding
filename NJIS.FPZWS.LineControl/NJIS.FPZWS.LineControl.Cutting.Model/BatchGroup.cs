using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class BatchGroup
    {
        public long LineId { get; set; }
        public DateTime PlanDate { get; set; }
        public string FrontBatchName { get; set; }
        public string BatchName { get; set; }
        public string NextBatchName { get; set; }
        public bool IsConnect { get; set; }
        public int GroupId { get; set; }
        public int BatchIndex { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? StartLoadingTime { get; set; }
        public int StackListId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
