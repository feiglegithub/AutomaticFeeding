//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：LoadingSortPackage.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class LoadingSortPackage
    {
        public string BatchNumber { get; set; }

        public string OrderNumber { get; set; }
        public string TagNumber { get; set; }
        public int Number { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int PartWidth { get; set; }
        public int PartLength { get; set; }
        public int PartHeight { get; set; }
        public string PartId { get; set; }
        public int Layer { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int R { get; set; }
        public int TypeId { get; set; }
    }
}