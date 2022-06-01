//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：LoggingAdapterElement.cs
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
    public class LoggingAdapterElement : ConfigurationElement
    {
        private const string EnabledKey = "enabled";
        private const string NameKey = "name";
        private const string TypeKey = "type";


        /// <summary>
        ///     获取或设置 是否启用
        /// </summary>
        [ConfigurationProperty(EnabledKey, IsRequired = true)]
        public virtual bool Enabled
        {
            get { return (bool)this[EnabledKey]; }
            set { this[EnabledKey] = value; }
        }

        /// <summary>
        ///     获取或设置 名称
        /// </summary>
        [ConfigurationProperty(NameKey, IsRequired = true)]
        public virtual string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        /// <summary>
        ///     获取或设置 类型
        /// </summary>
        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public virtual string Type
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }
    }
}