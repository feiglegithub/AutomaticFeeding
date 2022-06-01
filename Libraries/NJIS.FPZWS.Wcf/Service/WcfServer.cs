// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：WcfServer.cs
//  创建时间：2017-12-29 14:45
//  作    者：
//  说    明：
//  修改时间：2018-01-20 17:01
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.Wcf.MessageHeader;
using NJIS.FPZWS.Wcf.Monitor;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    public abstract class WcfServer<T> : WcfServer<T, DefaultServiceConfigBuilder>
        where T : IWcfServiceContract, new()
    {
    }

    public abstract class WcfServer : IWcfServer
    {
        public abstract void Start();

        public abstract void Stop();

        /// <summary>
        ///     WCF错误事件
        /// </summary>
        public event Action<object, EventArgs> WcfFaulted;

        protected void OnWcfFaultedEvent(object sender, EventArgs e)
        {
            if (WcfFaulted != null)
            {
                WcfFaulted.Invoke(sender, e);
            }
        }

        /// <summary>
        ///     WCF关闭事件
        /// </summary>
        public event Action<object, EventArgs> WcfClosed;

        protected void OnWcfClosedEvent(object sender, EventArgs e)
        {
            if (WcfClosed != null)
            {
                WcfClosed.Invoke(sender, e);
            }
        }

        /// <summary>
        ///     Wcf请求之前处理事件
        /// </summary>
        public event Action<string, object[], string, string> WcfBeforeCall;

        protected void OnWcfBeforeCallEvent(string operationName, object[] inputs, string absolutePath,
            string correlationState)
        {
            if (WcfBeforeCall != null)
            {
                WcfBeforeCall.Invoke(operationName, inputs, absolutePath, correlationState);
            }
        }

        /// <summary>
        ///     Wcf请求结束后处理事件
        /// </summary>
        public event Action<string, object, object, object, string> WcfAfterCall;

        protected void OnWcfAfterCallEvent(string operationName, object outputs, object returnValue,
            object correlationState, string absolutePath)
        {
            if (WcfAfterCall != null)
            {
                WcfAfterCall.Invoke(operationName, outputs, returnValue, correlationState, absolutePath);
            }
        }

        /// <summary>
        ///     WCF错误事件
        /// </summary>
        public event Action<object, object> WcfServerOpening;
        protected void OnWcfServerOpeningEvent(object sender,object e)
        {
            if (WcfServerOpening != null)
            {
                WcfServerOpening.Invoke(sender, e);
            }
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public abstract class WcfServer<T, TC> : WcfServer
        where T : IWcfServiceContract, new()
        where TC : IServiceBuilder, new()
    {
        private ServiceHost _service;

        /// <summary>
        ///     获取客户端信息，备用记录日志
        /// </summary>
        public WcfServer()
        {
            Conifg = new TC();

            #region

            var context = OperationContext.Current;
            if (context != null)
            {
                //获取客户端请求的路径
                var absolutePath = context.EndpointDispatcher.EndpointAddress.Uri.AbsolutePath;
                //获取客户端ip和端口
                var properties = context.IncomingMessageProperties;
                var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                var clientIp = endpoint.Address;
                //int client_port = endpoint.Port;

                //获取客户端请求的契约信息
                //string contract_name = context.EndpointDispatcher.ContractName;
                //获取客户端请求的路径
                //Uri request_uri = context.EndpointDispatcher.EndpointAddress.Uri;
                var sessionid = context.SessionId;
                var wcfappname = HeaderOperater.GetServiceWcfAppNameHeader(context);
                wcfappname = wcfappname == null ? "" : wcfappname;
                context.Channel.Closed += (sender, e) =>
                {
                    //Console.WriteLine(sessionid + "请求结束:" + client_ip + ":" + client_port + "->" + request_uri.AbsolutePath);
                    MonitorData.Instance.UpdateUrlConnNums(clientIp + "_" + wcfappname, absolutePath, false);
                };

                //Console.WriteLine(sessionid + "请求开始:" + client_ip + ":" + client_port + "->" + request_uri.AbsolutePath);
                Task.Factory.StartNew(() =>
                {
                    var ht = new Hashtable
                    {
                        {"ip", clientIp + "_" + wcfappname},
                        {"url", absolutePath},
                        {"isadd", true}
                    };
                    MonitorData.Instance.UpdateUrlConnNums((string) ht["ip"], (string) ht["url"],
                        (bool) ht["isadd"]);
                    Thread.CurrentThread.Abort();
                });
            }

            #endregion
        }

        public bool IsStop { get; private set; } = true;

        public TC Conifg { get; set; }

        /// <summary>
        ///     开始服务
        /// </summary>
        public override void Start()
        {
            IsStop = false;

            _service = Conifg.Build<T>();
            #region 增加拦截器处理

            var endpointscount = _service.Description.Endpoints.Count;
            var wcfpi = new WcfParameterInspector();
            wcfpi.WcfAfterCallEvent += (operationName, outputs, returnValue, correlationState, absolutePath) =>
            {
                OnWcfAfterCallEvent(operationName, outputs, returnValue, correlationState, absolutePath);
            };
            wcfpi.WcfBeforeCallEvent += (operationName, inputs, absolutePath, correlationState) =>
            {
                OnWcfBeforeCallEvent(operationName, inputs, absolutePath, correlationState.ToString());
            };
            for (var i = 0; i < endpointscount; i++)
            {
                if (_service.Description.Endpoints[i].Contract.Name != "IMetadataExchange")
                {
                    var operationscount = _service.Description.Endpoints[i].Contract.Operations.Count;
                    for (var j = 0; j < operationscount; j++)
                    {
                        _service.Description.Endpoints[i].Contract.Operations[j].Behaviors.Add(wcfpi);
                    }
                }
            }

            #endregion

            #region 注册事件

            //错误状态处理
            _service.Faulted += (sender, e) =>
            {
                OnWcfFaultedEvent(sender, e);
            };

            _service.Opening += (sender, e) =>
            {
                OnWcfServerOpeningEvent(this, Conifg);
            };
            _service.Closed += (sender, e) =>
            {
                OnWcfClosedEvent(sender, e);

                //如果意外关闭，再次打开监听
                if (IsStop)
                    return;
                Start();
            };

            #endregion

            _service.Open();
        }


        /// <summary>
        ///     停止服务
        /// </summary>
        public override void Stop()
        {
            IsStop = true;
            if (_service.State != CommunicationState.Closed)
            {
                _service.Close();
            }
            _service = null;
        }
    }
}