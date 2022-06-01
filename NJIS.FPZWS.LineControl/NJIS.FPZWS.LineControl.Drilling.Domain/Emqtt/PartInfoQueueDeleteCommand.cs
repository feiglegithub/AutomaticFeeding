//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PartInfoQueueDeleteCommand.cs
//   创建时间：2018-11-29 15:41
//   作    者：
//   说    明：
//   修改时间：2018-11-29 15:41
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
    public class PartInfoQueueDeleteCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(PartInfoQueueDeleteCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsDeleteQueueRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsDeleteQueueReq;

        public override void Execute(MqttMessageBase input)
        {
            if (string.IsNullOrEmpty(input.Data.ToString())) return;
            var partId = input.Data.ToString();

            try
            {
                var msg = _service.DeletePartInfoQueues(partId);
                if (!string.IsNullOrEmpty(msg))
                {
                    SendAlarmMsg(new PcsErrorAlarmArgs(msg));
                }

                MqttManager.Current.Publish($"{SendTopic}", new PartInfoQueueArgs {PartId = input.Data.ToString()});
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs(e.ToString()));  
            }
        }
    }
}