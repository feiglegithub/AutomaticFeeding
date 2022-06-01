// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：MessageHandler.cs
//  创建时间：2017-10-19 8:19
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class MessageHandler<T> : IMessageHandler where T : class
    {
        public MessageHandler()
        {

        }
        public MessageHandler(EventHandler<T> input)
        {
            Receive = input;
        }

        [Obsolete("MqttClient.Subscribe时指定")]
        public string Topic { get; set; }

        public string CommandCode { get; set; }

        public string Data { get; set; }

        /// <summary>
        ///     使用 internal 关键字保护此方法不会在别的类库中的派生类中看到，别的类库中只能 override 此方法的泛形版本
        /// </summary>
        /// <param name="input"></param>
        public void Excute(MqttMessageBase input)
        {
            var t = input.DeserializeData<T>();
            Receive?.Invoke(this, t);
        }

        /// <summary>
        ///     接收到消息
        /// </summary>
        public event EventHandler<T> Receive;
    }
}