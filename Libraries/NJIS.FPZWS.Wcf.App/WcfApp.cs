// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.App
//  文 件 名：WcfApp.cs
//  创建时间：2017-12-29 14:27
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;
using NJIS.FPZWS.Wcf.Service;

#endregion

namespace NJIS.FPZWS.Wcf.App
{
    public class WcfApp : AppBase<WcfApp>, IService
    {
        private readonly ILogger _log;

        public WcfApp()
        {
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(WcfApp).Name);
            Servers = new List<IWcfServer>();
        }

        public List<IWcfServer> Servers { get; set; }

        public override bool Start()
        {
            var s= base.Start();
            if (s)
            {

                _log.Info("start the service.");
                var finder = new DirectoryReflectionFinder();
                var types = finder.GetTypeFromAssignable<IWcfServer>();
                foreach (var type in types)
                {
                    _log.Info($"start open the service[{type}].");
                    try
                    {
                        var server = Activator.CreateInstance(type) as WcfServer;
                        if (server != null)
                        {
                            server.WcfAfterCall += Server_WcfAfterCall;
                            server.WcfBeforeCall += Server_WcfBeforeCall;
                            server.WcfClosed += Server_WcfClosed;
                            server.WcfFaulted += Server_WcfFaulted;
                            server.WcfServerOpening += Server_WcfServerOpening;
                            server.Start();
                            Servers.Add(server);
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Info(e.Message);
                        _log.Error(e);
                        s = false;
                        break;
                    }
                    _log.Info($"competed open the service[{type}].");
                }
            }

            return s;
        }

        private void Server_WcfServerOpening(object arg1, object arg2)
        {
            _log.Info($"WcfServerOpening：{arg1}.");
        }

        public string SessionId
        {
            get
            {
                if (OperationContext.Current == null)
                {
                    return "";
                }
                return OperationContext.Current.SessionId;
            }
        }

        private void Server_WcfFaulted(object arg1, EventArgs arg2)
        {
            _log.Info($"WcfFaulted.");
        }

        private void Server_WcfClosed(object arg1, EventArgs arg2)
        {
            _log.Info($"WcfClosed.");
        }

        private void Server_WcfBeforeCall(string arg1, object[] arg2, string arg3, string arg4)
        {
            _log.Info($"{SessionId}-{ClientIpAndPort()}:WcfBeforeCall operationName:{arg1}, inputs:{string.Join(",", arg2)}, absolutePath:{arg3}, correlationState:{arg4}.");
        }

        private void Server_WcfAfterCall(string arg1, object arg2, object arg3, object arg4, string arg5)
        {
            _log.Info($"{SessionId}-{ClientIpAndPort()}:WcfAfterCall  operationName:{arg1}, outputs:{arg2}, returnValue:{arg3}, " +
                      $"correlationState:{arg4}, absolutePath:{arg5}.");
        }

        public string ClientIpAndPort()
        {
            var context = OperationContext.Current;
            if (context == null) return "";
            var  properties = context.IncomingMessageProperties;

            var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (endpoint != null)
            {
                return endpoint.Address + ":" + endpoint.Port.ToString();
            }
            else
            {
                return "";
            }
        }

        public override bool Stop()
        {
            _log.Info("stop the service.");
            foreach (var wcfServer in Servers)
            {
                try
                {
                    wcfServer.Stop();
                }
                catch (Exception e)
                {
                    _log.Info(e.Message);
                    _log.Error(e);
                }
            }
            return base.Stop();
        }

        public StarterLevel Level { get; } = StarterLevel.Low;
    }
}