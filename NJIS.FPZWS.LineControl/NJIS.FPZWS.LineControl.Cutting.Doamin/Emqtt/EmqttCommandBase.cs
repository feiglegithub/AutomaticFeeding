using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.MqttClient.Setting;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Emqtt
{
    public abstract class EmqttCommandBase : IEmqttCommand
    {
        public virtual string SendTopic { get; set; }
        public virtual string ReceiveTopic { get; set; }
        public virtual string Name { get; set; }

        public abstract void Execute(MqttMessageBase input);

        public virtual void SendMsg(string msg)
        {
            MqttManager.Current.Publish(EmqttSettings.Current.PcsMsg,new MsgArgs(msg));
        }
    }
}
