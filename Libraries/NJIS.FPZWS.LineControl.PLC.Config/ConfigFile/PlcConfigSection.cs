//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：PlcConfigSection.cs
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
    public class PlcConfigSection : ConfigurationSection
    {
        private const string LoggingKey = "logging";
        private const string MessgerKey = "messger";
        private const string ConnectorsKey = "connectors";

        [ConfigurationProperty(ConnectorsKey, IsRequired = true)]
        public virtual ConnectorCollection Connectors
        {
            get { return (ConnectorCollection)this[ConnectorsKey]; }
            set { this[ConnectorsKey] = value; }
        }

        [ConfigurationProperty(LoggingKey, IsRequired = false)]
        public virtual LoggingElement Logging
        {
            get { return (LoggingElement)this[LoggingKey]; }
            set { this[LoggingKey] = value; }
        }
    }
}