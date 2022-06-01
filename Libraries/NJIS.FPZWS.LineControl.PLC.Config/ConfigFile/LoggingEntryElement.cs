//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：LoggingEntryElement.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Configuration;
using NJIS.FPZWS.Log;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config.ConfigFile
{
    public class LoggingEntryElement : ConfigurationElement
    {
        private const string LevelKey = "level";
        private const string EnabledKey = "enabled";

        /// <summary>
        ///     获取或设置 记录日志级别
        /// </summary>
        [ConfigurationProperty(LevelKey, IsRequired = true, DefaultValue = LogLevel.All)]
        public virtual LogLevel Level
        {
            get { return (LogLevel)this[LevelKey]; }
            set { this[LevelKey] = value; }
        }

        /// <summary>
        ///     获取或设置 是否启用改入口设置
        /// </summary>
        [ConfigurationProperty(EnabledKey, DefaultValue = true)]
        public virtual bool Enabled
        {
            get {return (bool)this[EnabledKey]; }
            set { this[EnabledKey] = value; }
        }
    }
}