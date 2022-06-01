//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：SortSpotCheckTask.cs
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
    [Table("SortSpotCheckTask")]
    public class SortSpotCheckTask
    {
        [Key] [Identity] public long LineId { get; set; }

        public Guid TaskDistributeId { get; set; }
        public int Type { get; set; }
        public int CheckNum { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TimingTime { get; set; }
        public int Status { get; set; }
        public int ProductionLine { get; set; }

        public string Section { get; set; }
        public string SpotType { get; set; }
        public string Upi { get; set; }
        public string Machine { get; set; }
        public int Number { get; set; }
        public int Count { get; set; }
        public string SpotMan { get; set; }
    }
}