//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：RoBotGroup.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJIS.Model.Attributes;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    [Table("RoBotGroup")]
    public class RoBotGroup
    {
        [Key] [Identity] public long LineId { get; set; }

        public int ProductionLine { get; set; }

        public string BatchCode { get; set; }
        public string OutBatchCode { get; set; }
        public string Code { get; set; }
        public int? LastInPartPosition { get; set; }
        public int? Status { get; set; }
        public string ConvergeRobotCode { get; set; }
        public string ConvergePartId { get; set; }
        public int? ConvergeStatus { get; set; }

        public string InArithmetic { get; set; }
    }
}