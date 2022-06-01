//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：LoggingElement.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Configuration;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config.ConfigFile
{
    public class LoggingElement : ConfigurationElement
    {
        private const string AdapterKey = "adapter";
        private const string EntryKey = "entry";

        /// <summary>
        ///     获取或设置 日志适配器
        /// </summary>
        [ConfigurationProperty(AdapterKey, IsRequired = true)]
        public virtual LoggingAdapterElement Adapter
        {
            get { return (LoggingAdapterElement)this[AdapterKey]; }
            set { this[AdapterKey] = value; }
        }

        /// <summary>
        ///     获取或设置 日志入口
        /// </summary>
        [ConfigurationProperty(EntryKey, IsRequired = true)]
        public virtual LoggingEntryElement Entry
        {
            get { return (LoggingEntryElement)this[EntryKey]; }
            set { this[EntryKey] = value; }
        }
    }
}