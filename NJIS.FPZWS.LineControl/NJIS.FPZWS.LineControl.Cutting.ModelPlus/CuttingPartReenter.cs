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
    
    public partial class CuttingPartReenter
    {
        public long LineId { get; set; }
        public string BatchName { get; set; }
        public string PartId { get; set; }
        public int ReenterType { get; set; }
        public Nullable<System.Guid> TaskDistributeId { get; set; }
        public string ProductionLine { get; set; }
        public bool OperationResult { get; set; }
        public string Remark { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }
    }
}
