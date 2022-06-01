using NJIS.FPZWS.LineControl.PLC.Message;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control
{
    public class PlcMqttMessager : IMessager
    {
        public void Publish<T>(string topic, T msg)
        {
        }
    }
}
