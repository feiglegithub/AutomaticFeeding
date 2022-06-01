//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：SortingPart.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJIS.Model.Attributes;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    [Table("SortingPart")]
    public class SortingPart
    {
        [Key] [Identity] public long LineId { get; set; }

        public string BatchName { get; set; }
        public string PackslipNumber { get; set; }
        public string OrderNumber { get; set; }
        public string PackID { get; set; }
        public int PackIndex { get; set; }
        public int? OrderIndex { get; set; }
        public string PartID { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Thickness { get; set; }
        public double Weight { get; set; }
        public double PositionPX { get; set; }
        public double PositionPY { get; set; }
        public double PositionPZ { get; set; }
        public int? Status { get; set; }
        public int? ImportToMesStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }

        [UpdatedAt] public DateTime? UpdatedTime { get; set; }

        public int? PanelType { get; set; }
        public DateTime ProductionDate { get; set; }

        public Guid TaskDistributeId { get; set; }

        public Guid TaskId { get; set; }
        public string PartType { get; set; }
        public int IsRotate { get; set; }
    }
}