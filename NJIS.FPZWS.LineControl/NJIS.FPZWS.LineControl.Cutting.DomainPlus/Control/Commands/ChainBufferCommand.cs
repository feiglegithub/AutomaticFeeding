﻿using System;
using System.Linq;
using System.Text;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Entitys;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Commands
{
    /// <summary>
    ///     链式缓存数据采集
    ///     继承<see cref="CommandBase" /> 不需要处理命令开始和结束
    /// </summary>
    public class ChainBufferCommand : CommandBase<ChainBufferInputEntity, ChainBufferOutputEntity>
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(ChainBufferCommand).Name);

        public ChainBufferCommand() : base("ChainBuffer")
        {
        }

        public override bool CheckInput(IPlcConnector plc)
        {
            //if (string.IsNullOrEmpty(Input.Code))
            //{
            //    MqttManager.Current.Publish(EmqttSettings.Current.PcsAlarmRep, new PcsLogicAlarmArgs($"{this.CommandCode}=>未配置链式缓存"));
            //    return false;
            //}
            return true;
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            int onePartByteLength = CuttingSetting.Current.OnePartByteLength;
            if (Input.Buffer == null || Input.Buffer.Length == 0)
            {
                string msg = $"{CommandCode}=>获取链式缓存数据失败";
                MqttManager.Current.Publish(EmqttSettings.Current.PcsAlarmRep,
                    new PcsLogicAlarmArgs(msg));
                _log.Debug(msg);
                return Output;
            }

            if (CuttingContext.Instance.ChainBuffers == null)
            {
                _log.Debug("链式缓存为空！");
                return Output;
            }

            ChainBufferInfo cb = CuttingContext.Instance.ChainBuffers.FirstOrDefault(m => m.CuttingChainBuffer.Code == Input.Code);
            if (cb != null)
            {
                cb.Parts.Clear();
                var i = 0;
                while (i < Input.Buffer.Length)
                {
                    var str = Encoding.ASCII.GetString(Input.Buffer, i+2, onePartByteLength-2).Trim('\0');
                    i = i + onePartByteLength;
                    if (string.IsNullOrEmpty(str)) continue;
                    var partInfo = PartInfoCache.GetScanPartInfo(str);
                    
                    if (partInfo != null)
                    {
                        var cpi = new ControlPartInfo
                        {
                            BatchName = partInfo.BatchName,
                            DeviceName = partInfo.DeviceName,
                            Length = partInfo.Length,
                            Width = partInfo.Width,
                            PartId = partInfo.PartId
                        };
                        cb.Parts.Add(cpi);
                    }
                    else
                    {
                        cb.Parts.Add(new ControlPartInfo { PartId = str });
                    }
                }

                try
                {
                    var data = new ChainBufferArgs
                    {
                        CuttingChainBuffer = cb.CuttingChainBuffer,
                        PartInfoArgses = (from cbPart in cb.Parts
                            select new PartInfoArgs
                            {
                                PartId = cbPart.PartId,
                                BatchName = cbPart.BatchName,
                            }).ToList()
                    };
                    MqttManager.Current.Publish(EmqttSettings.Current.PcsChainBufferRep, data);

                }
                catch (Exception e)
                {
                    _log.Error($"板件号有误:{e.Message}");
                    MqttManager.Current.Publish(EmqttSettings.Current.PcsAlarmRep, new PcsLogicAlarmArgs($"板件号有误:{e.Message}"));
                }
               
                // 发送给客户端
               
            }
            else
            {
                MqttManager.Current.Publish(EmqttSettings.Current.PcsAlarmRep,
                    new PcsLogicAlarmArgs($"{CommandCode}=>找不到链式缓存{Input.Code}"));
            }

            Output.TriggerOut = Input.TriggerIn;
            return Output;
        }
    }
}
