//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CuttingTaskLog
    {
        public long LineId { get; set; }
        public Nullable<System.Guid> TaskDistributeId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }
        public string BatchName { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> FinishedStatus { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string DeviceName { get; set; }
        public Nullable<bool> IsSuccess { get; set; }
        public string Msg { get; set; }
        public Nullable<System.DateTime> PlanDate { get; set; }
        public Nullable<bool> IsChangedDevice { get; set; }
    }
}
