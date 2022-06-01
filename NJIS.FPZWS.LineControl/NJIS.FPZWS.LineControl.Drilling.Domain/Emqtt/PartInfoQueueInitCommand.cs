//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PartInfoQueueInitCommand.cs
//   创建时间：2018-11-29 16:25
//   作    者：
//   说    明：
//   修改时间：2018-11-29 16:25
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Linq;
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
    public class PartInfoQueueInitCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(PartInfoQueueInitCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsInitQueueRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsInitQueueReq;

        public override void Execute(MqttMessageBase input)
        {
            if (string.IsNullOrEmpty(input.Data.ToString())) return;
            Guid clientId;
            if (!Guid.TryParse(input.Data.ToString(), out clientId))
            {
                _log.Info($"Data type error, guid-{input.Data}.");
                return;
            }

            try
            {
                var datas = _service.FindPartInfoQueues(DrillingSetting.Current.InitQueueNumber);
                var pqs = datas.Select(m => new PartInfoQueueArgs
                {
                    PcsMessage = m.PcsMessage,
                    BatchName = m.BatchName,
                    DrillingRouting = m.DrillingRouting,
                    Length = (int)m.FinishLength,
                    Width = (int)m.FinishWidth,
                    OrderNumber = m.OrderNumber,
                    Place = m.Place,
                    NextPlace = m.NextPlace,
                    PartId = m.PartId,
                    CreatedTime = m.CreatedTime,
                    Status = m.Status
                });
                MqttManager.Current.Publish($"{SendTopic}/{clientId}", pqs.ToList());
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs($"初始化队列数据=>{e.Message}"));
            }
        }
    }
}