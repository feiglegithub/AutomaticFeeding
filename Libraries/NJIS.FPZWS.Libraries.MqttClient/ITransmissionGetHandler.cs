// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：ITransmissionGetHandler.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.Libraries.MqttClient
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