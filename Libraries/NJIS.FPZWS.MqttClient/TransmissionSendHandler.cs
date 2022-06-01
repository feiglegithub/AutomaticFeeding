// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：TransmissionSendHandler.cs
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

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class TransmissionSendHandler<T> : ITransmissionSendHandler where T : class
    {
        public string TransmissionId { get; set; }
        public string CommandCode { get; set; }
        public string Topic { get; set; }

        public string RequestId { get; set; }

        public List<object> DataList { get; set; }

        public DateTime ReTryTime { get; set; }

        public DateTime OverTime { get; set; }

        public SerializeMode SerializeMode { get; set; }

        public void Excute(MqttMessageBase input)
        {
            var t = input.DeserializeData<T>();
            Finish?.Invoke(this, t);
        }

        public event EventHandler<T> Finish;
    }
}