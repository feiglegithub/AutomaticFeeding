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
    
    public partial class SortingStationInfo
    {
        public int LineId { get; set; }
        public int StationNo { get; set; }
        public int StationType { get; set; }
        public string Color { get; set; }
        public int BookCount { get; set; }
        public string StationRemark { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public int PilerNo { get; set; }
        public bool HasBoard { get; set; }
        public bool HasUpBoard { get; set; }
        public bool IsOuting { get; set; }
    }
}
