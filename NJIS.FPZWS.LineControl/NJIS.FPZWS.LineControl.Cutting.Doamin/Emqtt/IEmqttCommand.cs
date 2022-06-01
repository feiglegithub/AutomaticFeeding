using NJIS.FPZWS.MqttClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Emqtt
{

    public interface IEmqttCommand
    {
        string SendTopic { get; set; }
        string ReceiveTopic { get; set; }
        string Name { get; set; }
        void Execute(MqttMessageBase input);
    }
}
