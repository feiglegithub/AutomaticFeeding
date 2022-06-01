//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PositionOutputEntity.cs
//   创建时间：2018-11-26 10:54
//   作    者：
//   说    明：
//   修改时间：2018-11-26 10:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys
{
    public class PositionOutputEntity : DbProcOutputEntity
    {
        /// <summary>
        ///     下一个位置
        /// </summary>
        public int NextPlace { get; set; }

        public string PcsMessage { get; set; }

        public string Data { get; set; }
        public int IsNg { get; set; }
        public string Status { get; set; }
        public int Res { get; set; }
    }
}