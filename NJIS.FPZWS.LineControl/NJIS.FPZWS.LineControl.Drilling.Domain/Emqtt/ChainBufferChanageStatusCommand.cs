//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：ChainBufferChanageStatusCommand.cs
//   创建时间：2018-11-30 10:28
//   作    者：
//   说    明：
//   修改时间：2018-11-30 10:28
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     缓存架初始化命令
    /// </summary>
    public class ChainBufferChanageStatusCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(ChainBufferChanageStatusCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsChanageStatusChainBufferRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsChanageStatusChainBufferReq;

        public override void Execute(MqttMessageBase input)
        {
            try
            {
                MqttManager.Current.Publish($"{SendTopic}", null);
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs($"初始化队列数据=>{e.Message}"));
            }
        }
    }
}