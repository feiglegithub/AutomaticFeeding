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
    
    public partial class RGV_Task
    {
        public int RTaskId { get; set; }
        public Nullable<long> TaskId { get; set; }
        public Nullable<int> TaskType { get; set; }
        public Nullable<int> FromPosition { get; set; }
        public Nullable<int> ToPosition { get; set; }
        public Nullable<int> PilerNo { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> FinishTime { get; set; }
    }
}
