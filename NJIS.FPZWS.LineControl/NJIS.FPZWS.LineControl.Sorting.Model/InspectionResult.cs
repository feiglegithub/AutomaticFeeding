//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：InspectionResult.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJIS.Model.Attributes;

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    [Table("InspectionResult")]
    public class InspectionResult
    {
        [Key] [Identity] public int Id { get; set; }

        public string PartId { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string Description { get; set; }
        public float Length1 { get; set; }
        public float Length2 { get; set; }
        public float Width1 { get; set; }
        public float Width2 { get; set; }
        public string DeviationAngleLongSide { get; set; }
        public string DeviationAngleShortSide { get; set; }
        public string DecorCheck { get; set; }
        public int ProductionRun { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string ProductionLine { get; set; }
    }
}