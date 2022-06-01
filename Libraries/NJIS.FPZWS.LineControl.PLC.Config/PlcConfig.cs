//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：PlcConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.PLC.Config.ConfigFile;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    /// <summary>
    ///     WinCc 配置
    /// </summary>
    public class PlcConfig : IConfig
    {
        private const string ConfigPath = "plc";

        public PlcConfig()
        {
            var appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath);
            if (!File.Exists(appPath))
            {
                File.Create(appPath);
            }

            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath + ".config");
            if (!File.Exists(configPath)) return;

            var cfg = ConfigurationManager.OpenExeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                appPath));

            Connectors = new List<ConnectorConfig>();
            Propertys = new List<PropertyConfig>();
            Plc = (PlcConfigSection) cfg.GetSection(ConfigPath);

            Logging = new LoggingConfig(Plc.Logging);

            foreach (ConnectorElement conn in Plc.Connectors)
            {
                var connector = new ConnectorConfig
                {
                    Commands = new List<CommandConfig>(),
                    ReaderInterval = conn.ReaderInterval,
                    Name = conn.Name,
                    IsDebug = conn.IsDebug,
                    IsCommandMultiThreading = conn.IsCommandMultiThreading,
                    IsSimulator = conn.IsSimulator,
                    Type = conn.Type,
                    SimulatorType = conn.SimulatorType,
                    PlcType = conn.PlcType,
                    Address = conn.Address,
                    Port = conn.Port,
                    TimeOut = conn.TimeOut,
                    ReceiveTimeOut = conn.ReceiveTimeOut
                };

                foreach (CommandElement item in conn.Commands)
                {
                    var command = new CommandConfig
                    {
                        Name = item.Name,
                        Enabled = item.Enabled,
                        Type = item.Type,
                        CommandType = item.CommandType,
                        IsClearData = item.IsClearData,
                        IsSync = item.IsSync,
                        CommandExecutInterval = item.CommandExecutInterval
                    };

                    var propertys = new List<PropertyConfig>();
                    if (item.Entity.Input != null)
                    {
                        command.InputConfig.Entity = Type.GetType(item.Entity.Input.Entity);
                        foreach (PropertyElement pp in item.Entity.Input.Propertys)
                        {
                            var property = new PropertyConfig
                            {
                                Name = pp.Name,
                                Map = pp.Map,
                                IsMap = pp.IsMap,
                                Length = pp.Length,
                                ValueType = pp.ValueType + ""
                            };
                            propertys.Add(property);
                        }

                        Propertys.AddRange(propertys);
                        command.InputConfig.PropertyConfigs.AddRange(propertys);

                        propertys.Clear();
                        command.OutputConfig.Entity = Type.GetType(item.Entity.Output.Entity);
                        foreach (PropertyElement pp in item.Entity.Output.Propertys)
                        {
                            var property = new PropertyConfig
                            {
                                Name = pp.Name,
                                Map = pp.Map,
                                IsMap = pp.IsMap,
                                Length = pp.Length,
                                ValueType = pp.ValueType + "",
                                WriteIndex = pp.WriteIndex,
                                IsCheck = pp.IsCheck
                            };
                            propertys.Add(property);
                        }

                        Propertys.AddRange(propertys);
                        command.OutputConfig.PropertyConfigs.AddRange(propertys);
                    }

                    connector.Commands.Add(command);
                }

                Connectors.Add(connector);
            }
        }

        public PlcConfigSection Plc { get; }

        #region Singleton

        public static PlcConfig Current => Nested.Instance;

        private sealed class Nested
        {
            internal static readonly PlcConfig Instance = new PlcConfig();

            static Nested()
            {
            }
        }

        /// <summary>
        ///     日志配置
        /// </summary>
        public LoggingConfig Logging { get; set; }

        public List<PropertyConfig> Propertys { get; }

        public List<ConnectorConfig> Connectors { get; }

        #endregion
    }
}