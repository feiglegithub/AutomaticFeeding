//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：InPartOutputEntity.cs
//   创建时间：2018-12-13 16:06
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Entitys
{
    public class InPartOutputEntity : EntityBase
    {
        public string PartId { get; set; }
        public string BatchName { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }
        public int Res { get; set; }
    }
}