using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Emqtt
{

    public interface IEmqttCommand
    {
        string SendTopic { get; set; }
        string ReceiveTopic { get; set; }
        string Name { get; set; }
        void Execute(MqttMessageBase input);
    }
}
