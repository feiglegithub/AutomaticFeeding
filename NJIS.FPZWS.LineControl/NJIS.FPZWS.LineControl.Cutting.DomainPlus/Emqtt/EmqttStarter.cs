using System;
using System.Threading.Tasks;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Emqtt
{
    /// <summary>
    /// Emqtt模块启动器
    /// </summary>
    public class EmqttStarter:ModularStarterBase
    {
        private readonly ILogger _log = LogManager.GetLogger<EmqttStarter>();
        public override StarterLevel Level => StarterLevel.High;
        public override void Start()
        {
            MqttManager.Current.Subscribe(EmqttSettings.Current.CuttingReq, new MqttHandler(input =>
             {
                 _log.Info($"收到emqtt{input.Topic}请求");
                 var commands = EmqttInitializer.Commands.FindAll(command => command.ReceiveTopic == input.Topic);
                 foreach (var command in commands)
                 {
                     Task.Factory.StartNew(() =>
                     {
                         try
                         {
                             _log.Info($"开始执行emqtt[{input.Topic}]=>{command.GetType().FullName}");
                             command.Execute(input);
                             _log.Info($"执行完成emqtt[{input.Topic}]=>{command.GetType().FullName}");
                         }
                         catch (Exception e)
                         {
                             _log.Info($"执行emqtt[{ input.Topic}]=>{ command.GetType().FullName}异常：{e.Message}");
                         }
                     });

                 }
             }));
        }

        public override void Stop()
        {
            MqttManager.Current.UnSubscribe(EmqttSettings.Current.CuttingReq);
            MqttManager.Current.Disconnect();
            base.Stop();
        }
    }

    public class MqttHandler : IMessageHandler
    {

        public MqttHandler(Action<MqttMessageBase> handler)
        {
            Handler = handler;
        }
        public Action<MqttMessageBase> Handler { get; } 
        public string Topic { get; set; }
        public string CommandCode { get; set; }
        public void Excute(MqttMessageBase input)
        {
            if (Handler != null)
            {
                Handler(input);
            }
        }
    }
}
