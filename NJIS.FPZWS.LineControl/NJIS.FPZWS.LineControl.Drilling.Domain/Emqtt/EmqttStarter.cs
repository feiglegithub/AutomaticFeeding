//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：EmqttStarter.cs
//   创建时间：2018-11-28 16:10
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Linq;
using System.Threading.Tasks;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    public class EmqttStarter : ModularStarterBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(EmqttStarter).Name);

        public override StarterLevel Level => StarterLevel.High;

        public override void Start()
        {
            MqttManager.Current.Subscribe($"{EmqttSetting.Current.DrillingReq}", new MqttHander(
                input =>
                {
                    _log.Info($"收到 emqtt[{input.Topic}] 请求");
                    var command = EmqttInitializer.Commands.FirstOrDefault(m => m.ReceiveTopic == input.Topic);

                    if (command != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _log.Info($"开始执行 emqtt[{input.Topic}]=>{command.GetType().FullName}");
                                command.Execute(input);
                                _log.Info($"完成执行 emqtt[{input.Topic}]=>{command.GetType().FullName}");
                            }
                            catch (Exception e)
                            {
                                _log.Error($"execute emqtt command fail [{command.Name}]：{e}");
                            }
                        });
                    }
                }));
        }

        public override void Stop()
        {
            MqttManager.Current.UnSubscribe($"{EmqttSetting.Current.DrillingReq}");
            MqttManager.Current.Disconnect();
            base.Stop();
        }
    }

    public class MqttHander : IMessageHandler
    {
        public MqttHander(Action<MqttMessageBase> hander)
        {
            Hander = hander;
        }

        public Action<MqttMessageBase> Hander { get; }
        public string Topic { get; set; }
        public string CommandCode { get; set; }

        public void Excute(MqttMessageBase input)
        {
            if (Hander != null)
            {
                Hander(input);
            }
        }
    }
}