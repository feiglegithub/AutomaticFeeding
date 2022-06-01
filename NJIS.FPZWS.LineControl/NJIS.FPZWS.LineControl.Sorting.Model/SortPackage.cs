//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：SortPackage.cs
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
    [Table("SortPackage")]
    public class SortPackage
    {
        [Key] [Identity] public long LineId { get; set; }

        public string OrderNumber { get; set; }
        public string BatchNumber { get; set; }
        public string Number { get; set; }
        public int Index { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Ratio { get; set; }
        public int Layer { get; set; }
        public int PartCount { get; set; }
        public string PackingRule { get; set; }
        public string MixRule { get; set; }
        public int Status { get; set; }
        public string Version { get; set; }
        public int ImportToMesStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }

        public Guid TaskId { get; set; }
        public string PaperType { get; set; }
        public double PaperWidth { get; set; }
        public double PaperLength { get; set; }
        public int Hem { get; set; }
        public int FootProtection { get; set; }
        public Guid TaskDistributeId { get; set; }
        public string TagNumber { get; set; }
        public string ProductionLine { get; set; }
        public int TypeId { get; set; }
    }
}