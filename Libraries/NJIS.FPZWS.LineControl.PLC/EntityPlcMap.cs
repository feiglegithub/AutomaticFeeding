//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：EntityPlcMap.cs
//   创建时间：2018-11-20 14:39
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:39
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Reflection;

#endregion

namespace NJIS.FPZWS.LineControl.PLC
{
    /// <summary>
    ///     实体属性->PLC 变量映射关系
    /// </summary>
    public class EntityPlcMap
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string PlcVariable { get; set; }

        public PlcValType ValueType { get; set; }

        public string Desc { get; set; }

        public int Length { get; set; }

        /// <summary>
        ///     是否映射
        /// </summary>
        public bool IsMap { get; set; } = true;

        public bool IsCheck { get; set; } = false;

        public int WriteIndex { get; set; } = -1;

        public PlcVariableDirection Direction { get; set; }
    }
}