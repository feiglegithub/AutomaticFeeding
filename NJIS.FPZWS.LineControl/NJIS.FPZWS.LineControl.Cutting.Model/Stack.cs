using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class Stack
    {
        public long LineId { get; set; }
        public DateTime PlanDate { get; set; }
        public string StackName { get; set; }
        public string MainColor { get; set; }
        public int Status { get; set; }
        public string PlanDeviceName { get; set; }
        public string ActualDeviceName { get; set; }
        public string FirstBatchName { get; set; }
        public int FirstBatchBookCount { get; set; }
        public string SecondBatchName { get; set; }
        public int SecondBatchBookCount { get; set; }
        public int BookCount { get; set; }
        public int StackListId { get; set; }
        public string LastStackName { get; set; }
        public string NextStackName { get; set; }
        public string ConnectColor { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishedTime { get; set; }
    }
}
