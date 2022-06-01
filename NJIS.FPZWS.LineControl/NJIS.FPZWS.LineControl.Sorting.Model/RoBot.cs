//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：RoBot.cs
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
    [Table("RoBot")]
    public class RoBot
    {
        [Key] [Identity] public long LineId { get; set; }

        public int IsOpen { get; set; }
        public int ConvergeIndex { get; set; }
        public string DeviceId { get; set; }
        public string Code { get; set; }
        public string CurrentLattice { get; set; }
        public int? Status { get; set; }
        public int? InHandlingTime { get; set; }
        public int? OutHandlingTime { get; set; }
        public DateTime? LastHandlingTime { get; set; }
        public DateTime? LastHandledTime { get; set; }
        public string RobotGroupCode { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }

        [UpdatedAt] public DateTime? UpdatedTime { get; set; }

        public int? InMaxCount { get; set; }
        public int? OutMaxCount { get; set; }
        public int? ConvergeMaxCount { get; set; }
        public int? FindPriority { get; set; }
    }
}