//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    using System;
    using System.Collections.Generic;
    
    public partial class BatchGroupPlus
    {
        public int LineId { get; set; }
        public System.DateTime PlanDate { get; set; }
        public string BatchName { get; set; }
        public int BatchIndex { get; set; }
        public Nullable<System.DateTime> StartLoadingTime { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }
    }
}
