// ************************************************************************************
//  解决方案：NJIS.FPZWS.Cutting.Client 
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：MqttHander.cs
//  创建时间：2018-05-03 14:12
//  作    者：
//  说    明：
//  修改时间：2018-05-03 14:13
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class MqttHander : IMessageHandler
    {
        public MqttHander(Action<MqttMessageBase> hander)
        {
            Hander = hander;
        }

        public Action<MqttMessageBase> Hander { get; }
        public string Topic { get; set; }
        public string CommandCode { get; set; }

        public virtual void Excute(MqttMessageBase input)
        {
            if (Hander != null)
            {
                try
                {
                    Hander(input);
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                }
            }
        }
    }
}