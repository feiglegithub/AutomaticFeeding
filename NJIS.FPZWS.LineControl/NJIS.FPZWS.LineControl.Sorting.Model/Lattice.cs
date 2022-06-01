//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：Lattice.cs
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
    [Table("Lattice")]
    public class Lattice
    {
        [Key] [Identity] public long LineId { get; set; }

        public int Code { get; set; }
        public double Width { get; set; }
        public int Layer { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public string PartId { get; set; }
        public int FrameCode { get; set; }

        public string RobotCode { get; set; }

        public int Priority { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public int IsOpen { get; set; }
    }
}