﻿// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：IResponseAsyncHandler.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Libraries.MqttClient
{
    public interface IResponseAsyncHandler : IReceiveBaseHandler
    {
        /// <summary>
        ///     回调处理的主题
        /// </summary>
        string Topic { get; set; }

        /// <summary>
        ///     发起请求的客户端
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        ///     本次请求的唯一标识ID
        /// </summary>
        string RequestId { get; set; }

        string CommandCode { get; set; }

        void Excute(MqttMessageBase input, MqttClient client);

        void Completed(object msg, SerializeMode mode);
    }
}