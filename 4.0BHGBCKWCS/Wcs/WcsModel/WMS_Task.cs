//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcsModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class WMS_Task
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
