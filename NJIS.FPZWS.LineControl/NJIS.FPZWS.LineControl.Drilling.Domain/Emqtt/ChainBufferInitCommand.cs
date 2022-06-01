//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：ChainBufferInitCommand.cs
//   创建时间：2018-11-30 10:28
//   作    者：
//   说    明：
//   修改时间：2018-11-30 10:28
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Core;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     缓存架初始化命令
    /// </summary>
    public class ChainBufferInitCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(ChainBufferInitCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsInitChainBufferRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsInitChainBufferReq;

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
                var datas = new List<ChainBufferArgs>();
                foreach (var instanceChainBuffer in DrillingContext.Instance.ChainBuffers)
                {
                    datas.Add(new ChainBufferArgs
                    {
                        Code = instanceChainBuffer.Code,
                        Remark = instanceChainBuffer.Remark,
                        Size = instanceChainBuffer.Size,
                        Status = instanceChainBuffer.Size,
                        Parts = (from cbPart in instanceChainBuffer.Parts
                            select new PartInfoArgs
                            {
                                PartId = cbPart.PartId,
                                BatchName = cbPart.BatchName,
                                DrillingRoute = cbPart.DrillingRoute,
                                OrderNumber = cbPart.OrderNumber,
                                Thickness = cbPart.Thickness,
                                Width = cbPart.Width
                            }).ToList()
                    });
                }

                MqttManager.Current.Publish($"{SendTopic}/{clientId}", datas);
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs($"初始化队列数据=>{e.Message}"));
            }
        }
    }
}