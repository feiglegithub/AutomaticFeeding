// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：MqttMessageBase.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

#endregion

namespace NJIS.FPZWS.Libraries.MqttClient
{
    [Serializable]
    public class MqttMessageBase
    {
        public string Topic { get; set; }
        public string RequestId { get; set; }

        public string ClientId { get; set; }

        /// <summary>
        ///     消息指定的接收操作
        /// </summary>
        public string CommandCode { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        ///     序列化模式
        /// </summary>
        public SerializeMode SerializeMode { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        public object Data { get; set; }

        public int Index { get; set; }
        public int TotalCount { get; set; }

        /// <summary>
        ///     将实体类序列化成二进制
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public byte[] Serialize(SerializeMode mode)
        {
            if (mode == SerializeMode.Json)
            {
                var json = JsonConvert.SerializeObject(this);

                return Encoding.UTF8.GetBytes(json);
            }

            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Position = 0;
            var data = new byte[ms.Length];
            ms.Read(data, 0, data.Length);
            ms.Close();
            return data;
        }

        public T DeserializeData<T>() where T : class
        {
            if (SerializeMode == SerializeMode.Json)
            {
                var obj = JsonConvert.DeserializeObject<T>(Data.ToString());
                return obj;
            }

            return Data as T;
        }

        public static MqttMessageBase DeserializeData(byte[] data, SerializeMode mode)
        {
            if (mode == SerializeMode.Json)
            {
                var input = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<MqttMessageBase>(input);
            }

            var ms = new MemoryStream(data);
            var bf = new BinaryFormatter();
            ms.Position = 0;
            return bf.Deserialize(ms) as MqttMessageBase;
        }
    }


    public enum MessageType
    {
        //普通
        Normal,

        //对主动请求作出的回调
        Response,

        //主动请求
        Request,

        //传输发送方
        TransmissionSend,

        //传输接收方
        TransmissionGet
    }

    public enum SerializeMode
    {
        //二进制
        Binary,

        //Json
        Json
    }
}