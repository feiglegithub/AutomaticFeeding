// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Service
//  文 件 名：WcfParameterInspector.cs
//  创建时间：2017-12-27 16:49
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:32
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using NJIS.FPZWS.Wcf.MessageHeader;
using NJIS.FPZWS.Wcf.Monitor;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    internal class WcfParameterInspector : IOperationBehavior, IParameterInspector
    {

        /// <summary>
        ///     调用方法后 输出结果值
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="outputs"></param>
        /// <param name="returnValue"></param>
        /// <param name="correlationState"></param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            try
            {
                if (WcfAfterCallEvent != null)
                {
                    var context = OperationContext.Current;
                    var absolutePath = "";
                    if (context != null)
                    {
                        //获取客户端请求的路径
                        absolutePath = context.EndpointDispatcher.EndpointAddress.Uri.AbsolutePath;
                    }
                    WcfAfterCallEvent(operationName, outputs, returnValue, correlationState, absolutePath);

#if debug

                    #region 测试使用

                    _log.Debug("返回操作结束：" + absolutePath + "/" + operationName);
                    _log.Debug("*************返回操作编号：" + correlationState + "**************");
                    for (var i = 0; i < outputs.Length; i++)
                    {
                        var T = outputs[i].GetType();
                        _log.Debug("返回操作参数" + i + "  类型为：" + T);
                        _log.Debug("返回操作参数" + i + "  ToString为：" + outputs[i]);
                        _log.Debug("返回操作参数" + i + "  属性：");
                        var pIs = T.GetProperties();
                        foreach (var pi in pIs)
                        {
                            Console.Write(pi.Name + ":");
                            _log.Debug(pi.GetValue(outputs[i], null));
                        }
                    }

                    var treturn = returnValue.GetType();
                    _log.Debug("操作返回值" + "  类型为：" + treturn);
                    _log.Debug("操作返回值" + "  ToString为：" + treturn);
                    _log.Debug("操作返回值" + "  属性：");

                    if (treturn.ToString() != "System.String")
                    {
                        var pIreturns = treturn.GetProperties();
                        foreach (var PI in pIreturns)
                        {
                            Console.Write(PI.Name + ":");
                            _log.Debug(PI.GetValue(returnValue, null));
                        }
                    }

                    #endregion
#endif
                }
            }
            catch (Exception )
            {
                //_log.Error(ex);
            }
        }

        /// <summary>
        ///     调用方法前 输出参数值
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public object BeforeCall(string operationName, object[] inputs)
        {
            var guid = Guid.NewGuid().ToString();

            try
            {
                if (WcfBeforeCallEvent != null)
                {
                    var context = OperationContext.Current;
                    var absolutePath = "";
                    if (context != null)
                    {
                        //获取传递的自定义消息头
                        var headercontext = HeaderOperater.GetServiceWcfHeader(context);
                        var wcfappname = HeaderOperater.GetServiceWcfAppNameHeader(context);
                        wcfappname = wcfappname == null ? "" : wcfappname;
                        if (headercontext != null)
                            guid = headercontext.CorrelationState;

                        //获取客户端请求的路径
                        absolutePath = context.EndpointDispatcher.EndpointAddress.Uri.AbsolutePath;

                        //获取客户端ip和端口
                        var properties = context.IncomingMessageProperties;
                        var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                        var clientIp = endpoint.Address;
                        //int client_port = endpoint.Port;

                        if (!absolutePath.Contains("Eltc.Base/FrameWork/Helper/Wcf"))
                        {
                            var ht = new Hashtable
                            {
                                {"ip", clientIp + "_" + wcfappname},
                                {"url", absolutePath},
                                {"operatename", operationName}
                            };
                            //MonitorData.Instance.UpdateOperateNums(client_ip, AbsolutePath, operationName);
                            var th = new Thread(Run);
                            th.Start(ht);
                        }
                    }
                    WcfBeforeCallEvent(operationName, inputs, absolutePath, guid);

#if debug

                    #region

                    _log.Debug("返回操作开始：" + absolutePath + "/" + operationName);
                    _log.Debug("*************调用操作编号：" + guid + "**************");
                    for (var i = 0; i < inputs.Length; i++)
                    {
                        var T = inputs[i].GetType();
                        _log.Debug("操作参数" + i + "  类型为：" + T);
                        _log.Debug("操作参数" + i + "  ToString为：" + inputs[i]);
                        _log.Debug("操作参数" + i + "  属性：");
                        var pIs = T.GetProperties();
                        foreach (var pi in pIs)
                        {
                            Console.Write(pi.Name + ":");
                            _log.Debug(pi.GetValue(inputs[i], null));
                        }
                    }

                    #endregion

#endif
                }
            }
            catch (Exception )
            {
                //_log.Error(ex);
            }

            return guid;
        }

        internal event Action<string, object[], string, object> WcfBeforeCallEvent;
        internal event Action<string, object[], object, object, string> WcfAfterCallEvent;

        private void Run(object operateht)
        {
            var ht = (Hashtable) operateht;

            MonitorData.Instance.UpdateOperateNums((string) ht["ip"], (string) ht["url"], (string) ht["operatename"]);

            Thread.CurrentThread.Abort();
        }

        #region IOperationBehavior Members

        /// <summary>
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(OperationDescription operationDescription,
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="clientOperation"></param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription,
            DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}