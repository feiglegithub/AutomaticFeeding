//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：SortingFeedBack.cs
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
    [Table("SortingFeedBack")]
    public class SortingFeedBack
    {
        [Key] [Identity] public long LineId { get; set; }

        public string PartID { get; set; }
        public string RobotCode { get; set; }
        public string RotbotGroupCode { get; set; }
        public string FrameCode { get; set; }
        public string LatticeCode { get; set; }
        public int? FeedBackStatus { get; set; }

        public string Layer { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }

        [UpdatedAt] public DateTime? UpdatedTime { get; set; }

        public int Status { get; set; }

        public int ProductionLine { get; set; }
        public string BatchName { get; set; }
    }
}