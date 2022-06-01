// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：ResponseAsyncHandler.cs
//  创建时间：2017-10-19 8:19
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.MqttClient
{
    public class ResponseAsyncHandler<T> : IResponseAsyncHandler where T : class
    {
        public delegate void EventHandler(object obj, T msg);

        private MqttClient Client { get; set; }

        /// <summary>
        ///     回调处理的主题
        /// </summary>
        public string Topic { get; set; }

        public string CommandCode { get; set; }

        /// <summary>
        ///     发起请求的客户端
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     本次请求的唯一标识ID
        /// </summary>
        public string RequestId { get; set; }

        public void Excute(MqttMessageBase input, MqttClient client)
        {
            var t = input.DeserializeData<T>();
            var responseBackup = new ResponseAsyncHandler<T>();
            responseBackup.Client = client;
            responseBackup.ClientId = input.ClientId;
            responseBackup.RequestId = input.RequestId;

            Receive?.Invoke(responseBackup, t);
        }

        public void Completed(object result)
        {
            var msg = new MqttMessageBase();
            var topic = string.Format("$client/{0}", ClientId);
            msg.RequestId = RequestId;
            msg.MessageType = MessageType.Response;
            msg.Data = result;
            Client.Response(topic, msg);
        }

        /// <summary>
        ///     接收到消息
        /// </summary>
        public event EventHandler Receive;
    }
}