//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：PlcCommandExecutorBase.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PLC.Config;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.PLC
{
    /// <summary>
    ///     PLC 命令执行者
    /// </summary>
    public class PlcCommandExecutorBase : IPlcCommandExecutor
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(PlcCommandExecutorBase).FullName);

        public PlcCommandExecutorBase()
        {
            Connectors = new List<IPlcConnector>();
            Config = new PlcConfig();
        }

        public List<IPlcConnector> Connectors { get; }

        public PlcConfig Config { get; set; }

        /// <summary>
        ///     初始化
        ///     创建连接器
        /// </summary>
        /// <returns></returns>
        public virtual bool Init()
        {
            foreach (var configConnector in Config.Connectors)
            {
                var ct = Type.GetType(configConnector.Type);
                if (ct == null) continue;

                var connector = Activator.CreateInstance(ct) as IPlcConnector;
                if (connector == null) continue;
                 connector.Name = configConnector.Name;
                connector.CommandExecutInterval = configConnector.ReaderInterval;
                if (!connector.Init(configConnector.PlcType, configConnector.Address, configConnector.Port,
                    configConnector.TimeOut, configConnector.ReceiveTimeOut)) continue;

                foreach (var configConnectorCommand in configConnector.Commands)
                {
                    var cmt = Type.GetType(configConnectorCommand.Type);
                    if (cmt == null) continue;

                    var command = Activator.CreateInstance(cmt) as CommandBase;
                    if (command == null) continue;
                    command.CommandCode = configConnectorCommand.Name;
                    command.IsClearData = configConnectorCommand.IsClearData;
                    command.IsSync = configConnectorCommand.IsSync;
                    command.CommandExecutInterval = configConnectorCommand.CommandExecutInterval;

                    //var input = Activator.CreateInstance(configConnectorCommand.InputConfig.Entity);
                    //var output = Activator.CreateInstance(configConnectorCommand.OutputConfig.Entity);

                    foreach (var icpc in configConnectorCommand.InputConfig.PropertyConfigs)
                    {
                        PlcValType vt;
                        if (!Enum.TryParse(icpc.ValueType, out vt)) continue;
                        var pi = command.GetInput().GetType().GetProperty(icpc.Name);
                        if (pi == null) continue;
                        command.EntityPlcMaps.Add(new EntityPlcMap
                        {
                            PlcVariable = icpc.Map,
                            ValueType = vt,
                            Desc = icpc.Desc,
                            IsMap = icpc.IsMap,
                            PropertyInfo = pi,
                            Length = icpc.Length,
                            IsCheck = icpc.IsCheck,
                            WriteIndex = icpc.WriteIndex,
                            Direction = PlcVariableDirection.Input
                        });
                    }
                    command.EntityPlcMaps.Sort((x,y)=>x.WriteIndex.CompareTo(x.WriteIndex));

                    foreach (var icpc in configConnectorCommand.OutputConfig.PropertyConfigs)
                    {
                        PlcValType vt;
                        if (!Enum.TryParse(icpc.ValueType, out vt)) continue;
                        var pi = command.GetOutput().GetType().GetProperty(icpc.Name);
                        if (pi == null) continue;
                        command.EntityPlcMaps.Add(new EntityPlcMap
                        {
                            PlcVariable = icpc.Map,
                            ValueType = vt,
                            Desc = icpc.Desc,
                            IsMap = icpc.IsMap,
                            Length = icpc.Length,
                            PropertyInfo = pi,
                            IsCheck = icpc.IsCheck,
                            WriteIndex = icpc.WriteIndex,
                            Direction = PlcVariableDirection.Output
                        });
                    }

                    connector.Commands.Add(command);
                }

                Connectors.Add(connector);
            }

            return true;
        }

        public virtual bool Start()
        {
            foreach (var connector in Connectors)
            {
                connector.Connect();

                CreatePLcExecutor(connector);
            }

            return true;
        }

        public bool _isStop = false;

        public virtual bool Stop()
        {
            foreach (var plcConnector in Connectors)
            {
                plcConnector.DisConnect();
            }

            _isStop = true;
            return _isStop;
        }

        protected virtual void CreatePLcExecutor(IPlcConnector connector)
        {
            //构建连接器
            Task.Factory.StartNew(() =>
            {
                if (connector.Commands == null || connector.Commands.Count == 0)
                {
                    _logger.Error("PLC 连接信息为空！");
                    return;
                }
                var commands = connector.Commands.FindAll(m => m.IsSync);
                long readCount = 0;
                while (!_isStop)
                {
                    try
                    {
                        _logger.Debug($"开始扫描{readCount++}=============================================");
                        if (connector.GetConnectState() != PlcConnectState.Connected)
                        {
                            connector.Connect();
                        }

                        var cmds = new List<CommandBase>();
                        foreach (var connectorCommand in commands)
                        {
                            if (connectorCommand.CheckInput(connector))
                            {
                                cmds.Add(connectorCommand);
                            }
                        }
                        _logger.Debug($"结束扫描{readCount}=============================================");

                        foreach (var connectorCommand in cmds)
                        {
                            _logger.Debug($"开始执行命令 command{connectorCommand.CommandCode}================");
                            if (!connectorCommand.LoadInput(connector)) continue;
                            connectorCommand.ExecuteCommand(connector);
                            if (connectorCommand.IsClearData)
                            {
                                // 要清除所有数据，暂时没必要
                            }

                            connectorCommand.LoadOutput(connector);
                            _logger.Debug($"结束执行命令 command{connectorCommand.CommandCode}================");
                        }

                    }
                    catch (Exception e)
                    {
                        _logger.Error(e);
                    }

                    Thread.Sleep(connector.CommandExecutInterval);
                }
            }, TaskCreationOptions.LongRunning);

            var syncCommands = connector.Commands.FindAll(m => !m.IsSync);
            foreach (var syncCommand in syncCommands)
            {
                Task.Factory.StartNew(() =>
                {
                    if (connector.Commands == null || connector.Commands.Count == 0)
                    {
                        _logger.Error("PLC 连接信息为空！");
                        return;
                    }
                    if (connector.GetConnectState() != PlcConnectState.Connected)
                    {
                        connector.Connect();
                    }
                    long readCount = 0;
                    while (!_isStop)
                    {
                        try
                        {
                            _logger.Debug($"开始扫描sync{readCount++}=============================================");
                            if (syncCommand.CheckInput(connector))
                            {
                                if (!syncCommand.LoadInput(connector)) return;
                                syncCommand.ExecuteCommand(connector);
                                if (syncCommand.IsClearData)
                                {
                                    // 要清除所有数据，暂时没必要
                                }

                                syncCommand.LoadOutput(connector);
                            }
                            _logger.Debug($"结束扫描sync{readCount}=============================================");
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e);
                        }

                        Thread.Sleep(syncCommand.CommandExecutInterval);
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }
    }
}