﻿//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：LoggingAdapterConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    public class LoggingAdapterConfig
    {
        /// <summary>
        ///     是否启用
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        ///     获取或设置名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     获取或设置类型
        /// </summary>
        public virtual string Type { get; set; }
    }
}