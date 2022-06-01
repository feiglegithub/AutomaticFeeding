//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：InPartOutputEntity.cs
//   创建时间：2018-11-22 8:51
//   作    者：
//   说    明：
//   修改时间：2018-11-22 8:51
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys
{
    public class InPartOutputEntity : EntityBase
    {
        /// <summary>
        ///     10:正常
        ///     20：找不到板件
        ///     100：线控故障
        /// </summary>
        public int Res { get; set; }

        public string PartId { get; set; }

        public string BatchName { get; set; }

        public int Thickness { get; set; }

        public int Rotation { get; set; }

        public int Width { get; set; }

        /// <summary>
        ///     打孔类型
        ///     10：不打孔
        ///     20：单面孔
        ///     30：双面孔
        /// </summary>
        public int DrillingRoute { get; set; }

        public int Length { get; set; }

        /// <summary>
        ///     板件位置
        /// </summary>
        public int Place { get; set; }

        /// <summary>
        ///     是否Ng
        ///     10：正常
        ///     20：抽检板件（NG）
        /// </summary>
        public int IsNg { get; set; }
    }
}