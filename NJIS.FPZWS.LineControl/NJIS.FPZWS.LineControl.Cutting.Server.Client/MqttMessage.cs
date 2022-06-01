using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client
{
    public class MqttMessage
    {
        

        public static void SubscribeMqttMessage()
        {
            AddSubscibeMqttMessage<List<PartInfoPositionArgs>>(EmqttSettings.Current.PcsPartInfoPositionRep,
                EmqttSettings.Current.PcsPartInfoPositionRep);
            AddSubscibeMqttMessage<ChainBufferArgs>(EmqttSettings.Current.PcsChainBufferRep,
                EmqttSettings.Current.PcsChainBufferRep);

            AddSubscibeMqttMessage<PartInfoQueueArgs>(EmqttSettings.Current.PcsInitQueueRep,
                EmqttSettings.Current.PcsInitQueueRep);
            AddSubscibeMqttMessage<PcsLogicAlarmArgs>(EmqttSettings.Current.PcsAlarmRep,
                EmqttSettings.Current.PcsAlarmRep);

        }
        public static void UnSubscribeMqttMessage()
        {
            RemoveSubscibeMqttMessage<List<PartInfoPositionArgs>>(EmqttSettings.Current.PcsPartInfoPositionRep,
                EmqttSettings.Current.PcsPartInfoPositionRep);
            RemoveSubscibeMqttMessage<ChainBufferArgs>(EmqttSettings.Current.PcsChainBufferRep,
                EmqttSettings.Current.PcsChainBufferRep);
            RemoveSubscibeMqttMessage<PartInfoQueueArgs>(EmqttSettings.Current.PcsInitQueueRep,
                EmqttSettings.Current.PcsInitQueueRep);

            RemoveSubscibeMqttMessage<PcsLogicAlarmArgs>(EmqttSettings.Current.PcsAlarmRep,
                EmqttSettings.Current.PcsAlarmRep);
            MqttManager.Current.Disconnect();
        }

        private static void AddSubscibeMqttMessage<T>(string emqttMessage, string broadcastMessage) where T:class 
        {
            MqttManager.Current.Subscribe(emqttMessage, new MessageHandler<T>((sender, messageArgs) => 
                BroadcastMessage.Send(broadcastMessage,messageArgs)));
        }
        private static void RemoveSubscibeMqttMessage<T>(string emqttMessage, string broadcastMessage) where T:class 
        {
            MqttManager.Current.Subscribe(emqttMessage, new MessageHandler<T>((sender, messageArgs) => 
                BroadcastMessage.Send(broadcastMessage,messageArgs)));
        }

        
    }
}
