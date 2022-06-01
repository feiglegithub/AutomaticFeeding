// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：ITransmissionGetHandler.cs
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
    public interface ITransmissionGetHandler
    {
        string Topic { get; set; }
        string CommandCode { get; set; }
        Dictionary<string, List<object>> DataList { get; set; }

        string RequestId { get; set; }

        MqttClient Client { get; set; }

        string ClientId { get; set; }

        void Excute(MqttMessageBase input, MqttClient client);

        void Completed(object result);
    }
}