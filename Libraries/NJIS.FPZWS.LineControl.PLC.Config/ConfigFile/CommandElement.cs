//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：CommandElement.cs
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
    public class CommandElement : ConfigurationElement
    {
        private const string NameKey = "name";
        private const string TypeKey = "type";
        private const string CommandTypeKey = "commandType";
        private const string EntityKey = "entity";
        private const string EnabledKey = "enabled";
        private const string IsClearDataKey = "isClearData";
        private const string IsSyncKey = "isSync";
        private const string CommandExecutIntervalKey = "commandExecutInterval";


        [ConfigurationProperty(EnabledKey, IsRequired = false, DefaultValue = true)]
        public virtual bool Enabled
        {
            get { return (bool)this[EnabledKey]; }
            set { this[EnabledKey] = value; }
        }

        [ConfigurationProperty(NameKey, IsRequired = true)]
        public virtual string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(CommandTypeKey, IsRequired = false, DefaultValue = 0)]
        public virtual int CommandType
        {
            get { return (int)this[CommandTypeKey]; }
            set { this[CommandTypeKey] = value; }
        }


        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public virtual string Type
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }

        [ConfigurationProperty(EntityKey, IsRequired = true)]
        public virtual EntityElement Entity
        {
            get { return (EntityElement)this[EntityKey]; }
            set { this[EntityKey] = value; }
        }

        [ConfigurationProperty(IsClearDataKey, IsRequired = false, DefaultValue = true)]
        public virtual bool IsClearData
        {
            get { return (bool)this[IsClearDataKey]; }
            set { this[IsClearDataKey] = value; }
        }

        [ConfigurationProperty(IsSyncKey, IsRequired = false, DefaultValue = true)]
        public virtual bool IsSync
        {
            get { return (bool)this[IsSyncKey]; }
            set { this[IsSyncKey] = value; }
        }

        [ConfigurationProperty(CommandExecutIntervalKey, IsRequired = false, DefaultValue = 100)]
        public virtual int CommandExecutInterval
        {
            get { return (int)this[CommandExecutIntervalKey]; }
            set { this[CommandExecutIntervalKey] = value; }
        }
    }
}