// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：TransmissionGetHandler.cs
//  创建时间：2017-10-19 8:19
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class TransmissionGetHandler<T> : ITransmissionGetHandler where T : class
    {
        public delegate void EventHandler(object obj, List<T> msg);

        public string Topic { get; set; }
        public string CommandCode { get; set; }

        public string ClientId { get; set; }

        public string RequestId { get; set; }

        public MqttClient Client { get; set; }

        public Dictionary<string, List<object>> DataList { get; set; }

        public void Excute(MqttMessageBase input, MqttClient client)
        {
            if (DataList == null)
                DataList = new Dictionary<string, List<object>>();
            if (!DataList.ContainsKey(input.RequestId))
                DataList.Add(input.RequestId, new List<object>());

            var t = input.DeserializeData<T>();
            DataList[input.RequestId].Add(t);
            if (DataList[input.RequestId].Count == input.TotalCount)
            {
                var list = new List<T>();
                foreach (var o in DataList[input.RequestId])
                {
                    list.Add(o as T);
                }
                var backup = new TransmissionGetHandler<T>
                {
                    Client = client,
                    ClientId = input.ClientId,
                    RequestId = input.RequestId
                };
                Receive?.Invoke(backup, list);
                DataList.Remove(input.RequestId);
            }
        }

        public void Completed(object result)
        {
            var msg = new MqttMessageBase();
            var topic = string.Format("$client/{0}", ClientId);
            msg.RequestId = RequestId;
            msg.MessageType = MessageType.TransmissionGet;
            msg.Data = result;
            Client.Response(topic, msg);
        }

        /// <summary>
        ///     接收到消息
        /// </summary>
        public event EventHandler Receive;
    }
}