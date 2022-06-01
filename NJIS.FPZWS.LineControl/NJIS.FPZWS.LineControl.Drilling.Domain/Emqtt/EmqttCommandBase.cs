//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：EmqttCommandBase.cs
//   创建时间：2018-11-28 16:09
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:09
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    public abstract class EmqttCommandBase : IEmqttCommand
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(EmqttCommandBase).Name);
        public virtual string Name
        {
            get => GetType().Name;
            set { }
        }

        public virtual string SendTopic { get; set; }

        public virtual string ReceiveTopic { get; set; }

        public abstract void Execute(MqttMessageBase input);

        /// <summary>
        ///     发送消息给客户端
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SendMsg(string msg)
        {
            MqttManager.Current.Publish(EmqttSetting.Current.PcsMsg, new MsgArgs(msg));
            _log.Info(msg);
        }


        /// <summary>
        ///     发送报警消息给客户端
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SendAlarmMsg(PcsErrorAlarmArgs entity)
        {
            MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep, entity);
            _log.Error(entity.ToString());
        }
    }
}