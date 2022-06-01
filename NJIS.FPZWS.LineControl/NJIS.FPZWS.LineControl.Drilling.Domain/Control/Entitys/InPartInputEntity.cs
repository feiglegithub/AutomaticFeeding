//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：InPartInputEntity.cs
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
    public class InPartInputEntity : EntityBase
    {
        public string PartId { get; set; }

        public int Position { get; set; }
    }
}