//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：ChainBufferCommand.cs
//   创建时间：2018-11-23 14:33
//   作    者：
//   说    明：
//   修改时间：2018-11-23 14:33
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Linq;
using System.Text;
using NJIS.FPZWS.LineControl.Drilling.Core;
using NJIS.FPZWS.LineControl.Drilling.Domain.Cache;
using NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Commands
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
            //    MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep, new PcsLogicAlarmArgs($"{this.CommandCode}=>未配置链式缓存"));
            //    return false;
            //}
            return true;
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            ushort onePartByteLength = 20;
            if (Input.Buffer == null || Input.Buffer.Length == 0)
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep,
                    new PcsLogicAlarmArgs($"{CommandCode}=>获取链式缓存数据失败"));
                return Output;
            }

            onePartByteLength = (byte)Input.Buffer[0];
            if (DrillingContext.Instance.ChainBuffers == null)
            {
                _log.Info("链式缓存为空！");
                return Output;
            }

            int headlength = 2;// 字符串头长度
            var cb = DrillingContext.Instance.ChainBuffers.FirstOrDefault(m => m.Code == Input.Code);
            if (cb != null)
            {
                cb.Parts.Clear();
                var i = 0;
                while (i < Input.Buffer.Length)
                {
                    var str = Encoding.ASCII.GetString(Input.Buffer, i + headlength, onePartByteLength-2).Trim('\0');
                    i = i + onePartByteLength+ headlength;
                    if (string.IsNullOrEmpty(str)) continue;

                    var partInfo = PartInfoCache.GetPartInfo(str);
                    if (partInfo != null)
                    {
                        cb.Parts.Add(partInfo);
                    }
                    else
                    {
                        cb.Parts.Add(new PartInfo { PartId = str });
                    }
                }

                var data = new ChainBufferArgs
                {
                    Code = cb.Code,
                    Remark = cb.Remark,
                    Size = cb.Size,
                    Status = cb.Status,
                    Parts = (from cbPart in cb.Parts
                             select new PartInfoArgs
                             {
                                 PartId = cbPart.PartId,
                                 BatchName = cbPart.BatchName,
                                 DrillingRoute = cbPart.DrillingRoute,
                                 OrderNumber = cbPart.OrderNumber,
                                 Thickness = cbPart.Thickness,
                                 Length = cbPart.Length,
                                 Width = cbPart.Width
                             }).ToList()
                };
                // 发送给客户端
                MqttManager.Current.Publish(EmqttSetting.Current.PcsChainBufferRep, data);
            }
            else
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep,
                    new PcsLogicAlarmArgs($"{CommandCode}=>找不到链式缓存{Input.Code}"));
            }

            Output.TriggerOut = Input.TriggerIn;
            return Output;
        }
    }
}