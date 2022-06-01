using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class RGVTask
    {
        public int RTaskId { get; set; }
        public int TaskId { get; set; }
        public int TaskType { get; set; }
        public int FromPosition { get; set; }
        public int ToPosition { get; set; }
        public int PilerNo { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
