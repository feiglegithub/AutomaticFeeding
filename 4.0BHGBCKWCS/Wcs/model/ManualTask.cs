using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public partial class ManualTask
    {
        public long TaskId { get; set; }
        public Nullable<long> ReqId { get; set; }
        public Nullable<int> PilerNo { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> Amount { get; set; }
        public int TaskType { get; set; }
        public Nullable<int> Priority { get; set; }
        public string FromPosition { get; set; }
        public string ToPosition { get; set; }
        public Nullable<int> DdjNo { get; set; }
        public Nullable<bool> HasUpProtect { get; set; }
        public int TaskStatus { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> DdjTime { get; set; }
        public Nullable<System.DateTime> FinishTime { get; set; }
        public string ErrorMsg { get; set; }
    }
}
