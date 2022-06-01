//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NJIS.FPZWS.LineControl.Drilling.Model
{
    using System;
    using NJIS.Model.Attributes;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    
    
    [Table("PcsPartPosition")]
    [Serializable]
    public partial class PcsPartPosition
    {
        [Key, Identity]
    [Required]
    	public long LineId { get; set; }
        	[StringLength(80)]
    	public string PartId { get; set; }
        	public Nullable<int> Position { get; set; }
        	[CreatedAt]
    	[IgnoreUpdate]	public Nullable<System.DateTime> CreatedTime { get; set; }
        	[UpdatedAt]
    	public Nullable<System.DateTime> UpdatedTime { get; set; }
        	public string Msg { get; set; }
    }
}
