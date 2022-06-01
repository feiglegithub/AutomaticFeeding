using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class MdbParse
    {
        public int LineId { get; set; }
        public string    BatchId { get; set; }
        public Guid TaskId { get; set; }
        public int Status { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
