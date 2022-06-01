//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：PlanningTask.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class CalculationRule
    {
        private double Weight;
        public string code { get; set; }
        public double maxlength { get; set; }
        public double minlength { get; set; }
        public double maxwidth { get; set; }
        public double minwidth { get; set; }

        public double weight
        {
            get => Weight;
            set => Weight = Math.Round(value, 3);
        }

        public int maxlayer { get; set; }
        public double height { get; set; }
        public double area { get; set; }
        public double layerCnt { get; set; }
        public double layerMaxLength { get; set; }
        public double layerMaxWidth { get; set; }
        public double packageMaxLength { get; set; }
        public double packageMaxWidth { get; set; }
        public string remark { get; set; }
        public int InOnePackage { get; set; }
    }

    public class CalculationMix
    {
        private double Weight;
        public string code { get; set; }
        public string mcode { get; set; }
        public double maxlength { get; set; }
        public double minlength { get; set; }
        public double maxwidth { get; set; }
        public double minwidth { get; set; }

        public double weight
        {
            get => Weight;
            set => Weight = Math.Round(value, 3);
        }

        public double height { get; set; }
        public double area { get; set; }
        public int maxlayer { get; set; }
        public double layerCnt { get; set; }
        public double layerMaxLength { get; set; }
        public double layerMaxWidth { get; set; }
        public double packageMaxLength { get; set; }
        public double packageMaxWidth { get; set; }
        public double mixCnt { get; set; }
        public string remark { get; set; }
    }

    public class PackageTagNumber
    {
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
    }

    public class CalculationPackingParameter
    {
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }
}