// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：IRequestHandler.cs
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
    public interface IRequestHandler
    {
        string Topic { get; set; }
        string RequestId { get; set; }

        DateTime ReTryTime { get; set; }

        DateTime OverTime { get; set; }

        byte[] Data { get; set; }

        IResponseAsyncHandler ResponseAsync { get; set; }

        void Excute(MqttMessageBase input);

        void Aborted();
    }
}