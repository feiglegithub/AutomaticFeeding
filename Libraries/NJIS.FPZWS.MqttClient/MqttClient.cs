// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：MqttClient.cs
//  创建时间：2017-10-19 8:19
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class MqttClient
    {
        private static readonly object _mqttLock = new object();
        private readonly List<ITransmissionGetHandler> _getHandlerList = new List<ITransmissionGetHandler>();
        private readonly List<IMessageHandler> _handlerList = new List<IMessageHandler>();

        private readonly object _lockRequest = new object();

        //请求回调事件
        private readonly List<IRequestHandler> _requestHandlerList = new List<IRequestHandler>();

        private readonly List<IResponseAsyncHandler> _responseAsyncHandlerList = new List<IResponseAsyncHandler>();
        private readonly List<IResponseHandler> _responseHandlerList = new List<IResponseHandler>();
        private readonly List<ITransmissionSendHandler> _sendHandlerList = new List<ITransmissionSendHandler>();

        private static object _locker = new object();

        public MqttClient(string serverIp, int serverPort, string clientId)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
            ReceiveClientId = clientId + Guid.NewGuid();
            SendClientId = clientId + Guid.NewGuid();

            ReceiveClient = new uPLibrary.Networking.M2Mqtt.MqttClient(serverIp, ServerPort, false, null, null,
                MqttSslProtocols.TLSv1_0);

            ReceiveClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            SendClient = new uPLibrary.Networking.M2Mqtt.MqttClient(serverIp, ServerPort, false, null, null,
                MqttSslProtocols.TLSv1_0);
            SerializeMode = SerializeMode.Json;
            WaitTime = 10;
            OverTime = 1;
        }


        public void Disconnect()
        {
            lock (_mqttLock)
            {
                if (ReceiveClient.IsConnected)
                    ReceiveClient.Disconnect();
                if (SendClient.IsConnected)
                    SendClient.Disconnect();
            }
        }

        /// <summary>
        ///     订阅主题，并注册对主题的回调处理
        /// </summary>
        /// <param name="topic">订阅的查询</param>
        /// <param name="responseHandler">对查询的处理</param>
        public void SubscribeResponse(string topic, IResponseHandler responseHandler)
        {
            responseHandler.Topic = topic;
            ReceiveClient.Subscribe(new[] { topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            _responseHandlerList.Add(responseHandler);
        }

        public void SubscribeResponse(string topic, IResponseAsyncHandler responseHandler)
        {
            responseHandler.Topic = topic;
            ReceiveClient.Subscribe(new[] { topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            _responseAsyncHandlerList.Add(responseHandler);
        }

        public void Subscribe(string topic, IMessageHandler handler)
        {
            LogHelper.Info($"Subscribe=>{topic}");

            if (!ReceiveClient.IsConnected) Connect();
            if (ReceiveClient.IsConnected)
            {
                lock (_locker)
                {
                    _handlerList.Add(handler);
                }

                handler.Topic = topic;
                ReceiveClient.Subscribe(new[] {topic}, new[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="handler"></param>
        public void UnSubscribe(string topic, IMessageHandler handler = null)
        {
            LogHelper.Info($"UnSubscribe=>{topic}");
            lock (_locker)
            {
                var dms = new List<IMessageHandler>();
                foreach (var messageHandler in _handlerList)
                {
                    if (messageHandler.Topic == topic && messageHandler == handler)
                    {
                        dms.Add(messageHandler);
                    }
                }

                foreach (var messageHandler in dms)
                {
                    _handlerList.Remove(messageHandler);
                }
            }
            //只移除handler，不取消订阅主题
            //ReceiveClient.Unsubscribe(new[] { topic });
        }

        public void Publish(MqttMsg msg)
        {
            if (!SendClient.IsConnected) Connect();
            if (SendClient.IsConnected)
            {
                LogHelper.Info($"Publish=>{msg.Key}:{msg.Topic},Datas[{msg.Datas}],retain:{msg.Retain}");
                var m = new MqttMessageBase
                {
                    Topic = msg.Topic,
                    Data = msg.Datas
                };
                var data = m.Serialize(SerializeMode);
                SendClient.Publish(msg.Topic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, msg.Retain);
            }
        }

        /// <summary>
        ///     发送客户端发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        /// <param name="retain"></param>
        public void SendClientPublish(string topic, object msg, bool retain)
        {
            if (!SendClient.IsConnected) Connect();
            if (SendClient.IsConnected)
            {
                var m = new MqttMessageBase
                {
                    Topic = topic,
                    Data = msg
                };
                var data = m.Serialize(SerializeMode);
                SendClient.Publish(topic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, retain);
            }
        }

        /// <summary>
        ///     接收客户端发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        /// <param name="retain"></param>
        public void ReceiveClientPublish(string topic, object msg, bool retain)
        {
            if (!ReceiveClient.IsConnected) Connect();
            if (ReceiveClient.IsConnected)
            {
                var m = new MqttMessageBase
                {
                    Topic = topic,
                    Data = msg
                };
                var data = m.Serialize(SerializeMode);
                ReceiveClient.Publish(topic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, retain);
            }
        }

        internal void Response(string topic, MqttMessageBase msg)
        {
            if (!ReceiveClient.IsConnected) Connect();
            if (ReceiveClient.IsConnected)
            {
                var data = msg.Serialize(SerializeMode);
                SendClient.Publish(topic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        /// <summary>
        ///     主动查询
        /// </summary>
        /// <param name="sendTopic">发起查询的主题</param>
        /// <param name="msg">消息</param>
        /// <param name="handler">对这次查询的回调</param>
        public void Request(string sendTopic, object msg, IRequestHandler handler = null)
        {
            if (!SendClient.IsConnected) Connect();
            if (SendClient.IsConnected)
            {

                if (handler == null)
                    handler = new RequestHandler<string>();

                var m = new MqttMessageBase { Data = msg };
                handler.RequestId = Guid.NewGuid().ToString();
                m.MessageType = MessageType.Request;
                m.RequestId = handler.RequestId;
                m.ClientId = ReceiveClient.ClientId;
                var data = m.Serialize(SerializeMode);

                handler.Topic = sendTopic;
                handler.ReTryTime = DateTime.Now.AddMinutes(WaitTime);
                handler.OverTime = DateTime.Now.AddHours(OverTime);
                handler.Data = data;
                lock (_lockRequest)
                {
                    _requestHandlerList.Add(handler);
                }
                //第四个参数决定是否永久保持最后一条发布消息，为true时，订阅这主题立即就收到最后一条消息
                SendClient.Publish(sendTopic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void TransmissionSend<T>(string sendTopic, List<T> list, ITransmissionSendHandler handler)
        {
            if (!ReceiveClient.IsConnected) Connect();
            if (ReceiveClient.IsConnected)
            {
                if (list.Count == 0) return;
                handler.RequestId = Guid.NewGuid().ToString();
                handler.DataList = new List<object>();
                list.ForEach(o => handler.DataList.Add(o));
                handler.Topic = sendTopic;
                handler.ReTryTime = DateTime.Now.AddMinutes(WaitTime);
                handler.OverTime = DateTime.Now.AddHours(OverTime);

                foreach (object o in list)
                {
                    var m = new MqttMessageBase
                    {
                        MessageType = MessageType.TransmissionSend,
                        TotalCount = list.Count,
                        RequestId = handler.RequestId,
                        Data = o,
                        SerializeMode = SerializeMode,
                        ClientId = ReceiveClient.ClientId
                    };

                    var data = m.Serialize(SerializeMode);
                    SendClient.Publish(sendTopic, data, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                }

                lock (_lockRequest)
                {
                    _sendHandlerList.Add(handler);
                }
            }
        }

        public void TransmissionGet(string topic, ITransmissionGetHandler handler)
        {
            handler.Topic = topic;
            ReceiveClient.Subscribe(new[] { topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            _getHandlerList.Add(handler);
        }

        /// <summary>
        ///     判断订阅是否与主题匹配
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        private bool IsMatch(string subscribe, string topic)
        {
            if (subscribe.IndexOf('#') < 0)
            {
                if (subscribe == topic) return true;
            }
            else
            {
                var idx = subscribe.IndexOf('#');
                if (topic[topic.Length - 1] != '/')
                {
                    topic += "/";
                }
                if (topic.Length < idx) return false;

                if (subscribe.Substring(0, idx) == topic.Substring(0, idx))
                    return true;
            }
            return false;
        }


        /// <summary>
        ///     接收处理
        /// </summary>
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            MqttMessageBase msg;

            try
            {
                if (SerializeMode == SerializeMode.Json)
                {
                    msg = MqttMessageBase.DeserializeData(e.Message, SerializeMode.Json);
                    msg.SerializeMode = SerializeMode.Json;
                }
                else
                {
                    msg = MqttMessageBase.DeserializeData(e.Message, SerializeMode.Binary);
                    msg.SerializeMode = SerializeMode.Binary;
                }
            }
            catch (Exception ex)
            {
                return;
            }
            //按收到的消息类型，过滤处理方式
            if (msg.MessageType == MessageType.Normal)
            {
                List<IMessageHandler> handers = new List<IMessageHandler>();
                lock (_locker)
                {
                    //普通的
                    foreach (var item in _handlerList)
                    {
                        if (IsMatch(item.Topic, e.Topic) && item.CommandCode == msg.CommandCode)
                        {
                            handers.Add(item);
                        }
                    }
                }
                foreach (var hander in handers)
                {
                    hander.Excute(msg);
                }
            }
            else if (msg.MessageType == MessageType.Response)
            {
                //收到一个回调，在请求完成处理列表中，找出请求完成时的处理
                lock (_lockRequest)
                {
                    foreach (var item in _requestHandlerList)
                    {
                        if (item.RequestId == msg.RequestId)
                        {
                            item.Excute(msg);
                            _requestHandlerList.Remove(item);
                            break;
                        }
                    }
                }
            }
            else if (msg.MessageType == MessageType.Request)
            {
                //收到一个请求，在回调处理列表中找出这个请求的回调处理
                foreach (var item in _responseHandlerList)
                {
                    if (IsMatch(item.Topic, e.Topic) && item.CommandCode == msg.CommandCode)
                    {
                        var result = item.Excute(msg);
                        var topic = string.Format("$client/{0}", msg.ClientId);
                        result.RequestId = msg.RequestId;
                        result.MessageType = MessageType.Response;
                        Response(topic, result);
                        return;
                    }
                }

                //在异步回调中找出请求的回调处理
                foreach (var item in _responseAsyncHandlerList)
                {
                    if (IsMatch(item.Topic, e.Topic) && item.CommandCode == msg.CommandCode)
                    {
                        item.Excute(msg, this);
                        return;
                    }
                }
            }
            else if (msg.MessageType == MessageType.TransmissionSend)
            {
                //收到一个数据传输，在接收处理列表中找出对应的处理
                foreach (var item in _getHandlerList)
                {
                    if (IsMatch(item.Topic, e.Topic) && item.CommandCode == msg.CommandCode)
                    {
                        item.Excute(msg, this);
                        return;
                    }
                }
            }
            else if (msg.MessageType == MessageType.TransmissionGet)
            {
                //收到数据传输的接收响应
                foreach (var item in _sendHandlerList)
                {
                    if (item.RequestId == msg.RequestId)
                    {
                        item.Excute(msg);
                        _sendHandlerList.Remove(item);
                        return;
                    }
                }
            }
        }

        public bool Connect()
        {
            var isConnect = true;
            lock (_mqttLock)
            {
                if (!ReceiveClient.IsConnected)
                {
                    try
                    {
                        ReceiveClient.Connect(ReceiveClientId);

                        if (_handlerList.Count > 0)
                        {
                            foreach (var messageHandler in _handlerList)
                            {
                                ReceiveClient.Subscribe(new[] { messageHandler.Topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                        isConnect = false;
                    }
                }
                if (!SendClient.IsConnected)
                {
                    try
                    {
                        SendClient.Connect(SendClientId);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                        isConnect = false;
                    }
                }
            }

            return isConnect;
        }

        #region 属性

        /// <summary>
        ///     MQTT数据接收客户端实例
        /// </summary>
        public uPLibrary.Networking.M2Mqtt.MqttClient ReceiveClient { get; }

        /// <summary>
        ///     MQTT数据发送客户端实例
        /// </summary>
        public uPLibrary.Networking.M2Mqtt.MqttClient SendClient { get; }

        /// <summary>
        ///     MQTT 服务器端口
        /// </summary>
        public int ServerPort { get; }

        /// <summary>
        ///     MQTT 服务器IP
        /// </summary>
        public string ServerIp { get; }

        /// <summary>
        ///     主动请求，等待应答时间(单位分钟)
        /// </summary>
        public double WaitTime { get; set; }

        /// <summary>
        ///     是否连接
        /// </summary>
        public bool IsConnected
        {
            get { return ReceiveClient.IsConnected && SendClient.IsConnected; }
        }

        /// <summary>
        ///     数据序列化方式
        /// </summary>
        public SerializeMode SerializeMode { get; set; }


        /// <summary>
        ///     主动请求，超时时间(单位小时)
        /// </summary>
        public double OverTime { get; set; }

        /// <summary>
        ///     客户端ID（发送）
        /// </summary>
        public string ReceiveClientId { get; }

        /// <summary>
        ///     收客户端ID（接收）
        /// </summary>
        public string SendClientId { get; }

        #endregion
    }
}