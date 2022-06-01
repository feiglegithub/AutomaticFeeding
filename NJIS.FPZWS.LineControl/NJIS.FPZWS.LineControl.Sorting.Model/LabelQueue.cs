//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：LabelQueue.cs
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
    [Table("LabelQueue")]
    public class LabelQueue
    {
        [Key] [Identity] public long LineId { get; set; }

        public string PackNumber { get; set; }
        public int PackingIndex { get; set; }
        public int Status { get; set; }
        public int ProductionLine { get; set; }

        public string BatchNumber { get; set; }
        public string OrderNumber { get; set; }
        public string PanelName { get; set; }
        public string ColorCode { get; set; }
        public string VehicleNum { get; set; }
        public string LotNumber { get; set; }
        public string ScheduleDeliveryDate { get; set; }
        public string Sequences { get; set; }
        public string Attribute7 { get; set; }
        public string PackCount { get; set; }
        public int PackIndex { get; set; }
        public string OderInFarmePackCountStr { get; set; }
        public int OderPackCount { get; set; }
        public int OderInFarmePackCount { get; set; }
        public int PartCount { get; set; }

        [CreatedAt] public DateTime CreatedTime { get; set; }

        [UpdatedAt] public DateTime UpdatedTime { get; set; }
    }
}