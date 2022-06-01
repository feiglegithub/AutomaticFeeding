// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：ResponseAsyncHandler.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Libraries.MqttClient
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

        public void Completed(object result, SerializeMode mode = SerializeMode.Binary)
        {
            var msg = new MqttMessageBase();
            var topic = string.Format("$client/{0}", ClientId);
            msg.RequestId = RequestId;
            msg.MessageType = MessageType.Response;
            msg.Data = result;
            Client.Response(topic, msg, mode);
        }

        /// <summary>
        ///     接收到消息
        /// </summary>
        public event EventHandler Receive;
    }
}