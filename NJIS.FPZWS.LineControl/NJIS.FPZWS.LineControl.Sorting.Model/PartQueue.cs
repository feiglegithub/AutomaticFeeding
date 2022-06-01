//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：PartQueue.cs
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
    [Table("PartQueue")]
    public class PartQueue
    {
        [Key] [Identity] public long LineId { get; set; }

        public string BatchNumber { get; set; }
        public string RobotGroup { get; set; }
        public string OrderNumber { get; set; }
        public string PartId { get; set; }
        public string PackId { get; set; }
        public string PackCode { get; set; }
        public int PackIndex { get; set; }
        public int PackLength { get; set; }
        public int PackWidth { get; set; }
        public int PackHeigth { get; set; }
        public int Layer { get; set; }
        public int PositionPX { get; set; }
        public int PositionPY { get; set; }
        public int PositionPZ { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }
        public int Status { get; set; }
        public int IsRotate { get; set; }
        public int IsChange { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }

        [UpdatedAt] public DateTime? UpdatedTime { get; set; }

        public int ProductionLine { get; set; }
    }
}