using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Cutting.UI.LocalServices
{
    public class MqttMessage
    {
        //public static void SubscribeMqttMessage()
        //{
            
        //    AddSubscibeMqttMessage<List<AssigningTaskArgs>>(EmqttSettings.Current.PcsDownMdbRep,
        //        EmqttSettings.Current.PcsDownMdbRep);
        //    AddSubscibeMqttMessage<List<PushTaskArgs>>(EmqttSettings.Current.PcsTaskRep,
        //        EmqttSettings.Current.PcsTaskRep);
        //}

        //public static void UnSubscribeMqttMessage()
        //{
        //    RemoveSubscibeMqttMessage<List<PushTaskArgs>>(EmqttSettings.Current.PcsTaskRep,
        //        EmqttSettings.Current.PcsTaskRep);
        //    RemoveSubscibeMqttMessage<List<AssigningTaskArgs>>(EmqttSettings.Current.PcsDownMdbRep,
        //        EmqttSettings.Current.PcsDownMdbRep);
        //    MqttManager.Current.Disconnect();
        //}



        //private static void AddSubscibeMqttMessage<T>(string emqttMessage, string broadcastMessage) where T : class
        //{

        //    MqttManager.Current.Subscribe(emqttMessage, new MessageHandler<T>((sender, messageArgs) =>
        //    BroadcastMessage.Send(broadcastMessage, messageArgs)));
        //}
        //private static void RemoveSubscibeMqttMessage<T>(string emqttMessage, string broadcastMessage) where T : class
        //{
        //    MqttManager.Current.UnSubscribe(emqttMessage, new MessageHandler<T>((sender, messageArgs) =>
        //    BroadcastMessage.Send(broadcastMessage, messageArgs)));
        //}

    }
}
