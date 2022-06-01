//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：EntityBase.cs
//   创建时间：2018-11-20 14:37
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:37
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.PLC
{
    [Serializable]
    public class EntityBase
    {
        /// <summary>
        ///     信号触发
        /// </summary>
        public bool Trigger { get; set; }

        public int TriggerIn { get; set; }

        public int TriggerOut { get; set; }
    }


    /// <summary>
    ///     空实体
    /// </summary>
    public class EmptyEntityBase : EntityBase
    {
    }
}