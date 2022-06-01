﻿// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：MessageHandler.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Libraries.MqttClient
{
    public class MessageHandler<T> : IMessageHandler where T : class
    {
        [Obsolete("MqttClient.Subscribe时指定")] public string Topic { get; set; }

        public string CommandCode { get; set; }

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