//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：PartInfo.cs
//   创建时间：2018-11-23 15:30
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    /// <summary>
    ///     板件
    /// </summary>
    public class PartInfo
    {
        public string PartId { get; set; }
        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }

        public string Place { get; set; }

        public string NextPlace { get; set; }
        public string PcsMessage { get; set; }

        /// <summary>
        ///     打孔类型
        /// </summary>
        public string DrillingRoute { get; set; }

        public bool Rotation { get; set; }

        public int Status { get; set; }
        public bool IsNg { get; set; }
    }
}