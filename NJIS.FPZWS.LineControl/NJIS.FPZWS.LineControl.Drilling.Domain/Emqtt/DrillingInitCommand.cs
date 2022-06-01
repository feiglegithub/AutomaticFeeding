//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：DrillingInitCommand.cs
//   创建时间：2018-11-28 16:09
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:09
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     缓存架初始化命令
    /// </summary>
    public class DrillingInitCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(DrillingInitCommand).Name);

        public override string SendTopic => EmqttSetting.Current.PcsInitRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsInitReq;

        public override void Execute(MqttMessageBase input)
        {
            if (string.IsNullOrEmpty(input.Data.ToString())) return;
            Guid clientId;
            if (!Guid.TryParse(input.Data.ToString(), out clientId))
            {
                _log.Info($"Data type error, guid-{input.Data}.");
                return;
            }

            MqttManager.Current.Publish($"{SendTopic}/{clientId}", null);
        }
    }
}