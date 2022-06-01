//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：ConnectorElement.cs
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
    public class ConnectorElement : ConfigurationElement
    {
        private const string NameKey = "name";
        private const string ReaderIntervalKey = "readerInterval";
        private const string CommandKey = "commands";
        private const string IsDebugKey = "isDebug";
        private const string IsSimulatorKey = "isSimulator";
        private const string SimulatorTypeKey = "simulatorType";
        private const string IsCommandMultiThreadingKey = "isCommandMultiThreading";
        private const string PlcTypeKey = "plcType";
        private const string AddressKey = "address";
        private const string PortKey = "port";
        private const string TimeOutKey = "timeOut";
        private const string TypeKey = "type";
        private const string ReceiveTimeOutKey = "receiveTimeOut";

        [ConfigurationProperty(IsDebugKey, DefaultValue = false, IsRequired = false)]
        public virtual bool IsDebug
        {
            get { return(bool)this[IsDebugKey]; }
            set { this[IsDebugKey] = value; }
        }

        [ConfigurationProperty(IsCommandMultiThreadingKey, DefaultValue = false, IsRequired = false)]
        public virtual bool IsCommandMultiThreading
        {
            get { return (bool)this[IsCommandMultiThreadingKey]; }
            set { this[IsCommandMultiThreadingKey] = value; }
        }


        [ConfigurationProperty(IsSimulatorKey, DefaultValue = false, IsRequired = false)]
        public virtual bool IsSimulator
        {
            get { return (bool)this[IsSimulatorKey]; }
            set { this[IsSimulatorKey] = value; }
        }


        [ConfigurationProperty(SimulatorTypeKey, DefaultValue = "", IsRequired = false)]
        public virtual string SimulatorType
        {
            get { return (string)this[SimulatorTypeKey]; }
            set { this[SimulatorTypeKey] = value; }
        }


        [ConfigurationProperty(CommandKey, IsRequired = true)]
        public virtual CommandCollection Commands
        {
            get { return (CommandCollection)this[CommandKey]; }
            set { this[CommandKey] = value; }
        }

        [ConfigurationProperty(NameKey, IsRequired = true)]
        public virtual string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(ReaderIntervalKey, IsRequired = true)]
        public virtual int ReaderInterval
        {
            get { return (int)this[ReaderIntervalKey]; }
            set { this[ReaderIntervalKey] = value; }
        }

        [ConfigurationProperty(PlcTypeKey, IsRequired = true)]
        public virtual string PlcType
        {
            get { return (string)this[PlcTypeKey]; }
            set { this[PlcTypeKey] = value; }
        }

        [ConfigurationProperty(AddressKey, IsRequired = true)]
        public virtual string Address
        {
            get { return (string)this[AddressKey]; }
            set { this[AddressKey] = value; }
        }

        [ConfigurationProperty(PortKey, IsRequired = false, DefaultValue = 102)]
        public virtual int Port
        {
            get { return (int)this[PortKey]; }
            set { this[PortKey] = value; }
        }

        [ConfigurationProperty(TimeOutKey, IsRequired = false, DefaultValue = 5000)]
        public virtual int TimeOut
        {
            get { return (int)this[TimeOutKey]; }
            set { this[TimeOutKey] = value; }
        }

        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public virtual string Type
        {
            get { return(string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }

        [ConfigurationProperty(ReceiveTimeOutKey, IsRequired = false, DefaultValue = 5000)]
        public virtual int ReceiveTimeOut
        {
            get { return (int)this[ReceiveTimeOutKey]; }
            set { this[ReceiveTimeOutKey] = value; }
        }
    }
}