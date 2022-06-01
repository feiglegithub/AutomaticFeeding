using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.PLC.Message;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control
{
    public class PlcMqttMessagerStarter: IModularStarter
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(PlcMqttMessagerStarter));

        public void Start()
        {
            _logger.Info("启动Emqtt 消息消息发送器");
            MessageManager.AddMessagerAdapter(new EmqttMessagerAdapter());
        }

        public void Stop()
        {
            _logger.Info("停止Emqtt消息发送器");
        }

        public StarterLevel Level { get; }
    }
}
