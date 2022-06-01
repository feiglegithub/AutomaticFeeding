// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：MqttManager.cs
//  创建时间：2017-10-19 17:44
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:45
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.MqttClient.Setting;
using System.Diagnostics;

#endregion

namespace NJIS.FPZWS.MqttClient
{
    public class MqttManager : Singleton<MqttManager>
    {
        public MqttManager()
        {
            MqQueues = new ConcurrentQueue<MqttMsg>();
            MqttClient = new MqttClient(TaskCenterSetting.Current.IP, TaskCenterSetting.Current.Port, TaskCenterSetting.Current.ClientId)
            {
                SerializeMode = SerializeMode.Json
            };
            if (TaskCenterSetting.Current.EnableHeartbeat > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        MqttClient.SendClientPublish(TaskCenterSetting.Current.HeartbeatTopic,
                            TaskCenterSetting.Current.ClientId, false);
                        MqttClient.ReceiveClientPublish(TaskCenterSetting.Current.HeartbeatTopic,
                            TaskCenterSetting.Current.ClientId, false);
                        Thread.Sleep(TaskCenterSetting.Current.HeartbeatInterval);
                    }
                });
            }

            LogHelper.Info("================================");
            LogHelper.Info($"ServerIp={MqttClient.ServerIp}");
            LogHelper.Info($"ServerPort={MqttClient.ServerPort}");
            LogHelper.Info($"ReceiveClientId={MqttClient.ReceiveClientId}");
            LogHelper.Info($"SendClientId={MqttClient.SendClientId}");
            LogHelper.Info($"SerializeMode={MqttClient.SerializeMode}");
            LogHelper.Info("================================");

            Task.Factory.StartNew(() =>
            {
                LogHelper.Info("start emqtt sender");
                while (true)
                {
                    MqttMsg msg;
                    while (MqQueues.TryDequeue(out msg))
                    {
                        MqttClient.Publish(msg);
                    }

                    Thread.Sleep(TaskCenterSetting.Current.QueueSendInterval);
                }
            });
        }

        private MqttClient MqttClient { get; }

        public bool IsConnected
        {
            get { return MqttClient.IsConnected; }
        }

        public void Disconnect()
        {
            MqttClient.Disconnect();
        }

        public void Connect()
        {
            try
            {
                MqttClient.Connect();
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
            }
        }


        private ConcurrentQueue<MqttMsg> MqQueues { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="datas"></param>
        /// <param name="retain"></param>
        public void Publish(string topic, object datas, bool retain = false)
        {
            var msg = new MqttMsg(topic, datas, retain);

            if (TaskCenterSetting.Current.Enable)
            {
                MqQueues.Enqueue(msg);
            }
            else
            {
                if (!IsConnected)
                {
                    Connect();
                }
                MqttClient.Publish(msg);
            }
        }

        /// <summary>
        ///     订阅
        /// </summary>
        public void Subscribe(string topic, IMessageHandler handler)
        {
            MqttClient.Subscribe(topic, handler);
        }
        /// <summary>
        ///     取消订阅
        /// </summary>
        public void UnSubscribe(string topic, IMessageHandler handler = null)
        {
            MqttClient.UnSubscribe(topic, handler);
        }

    }
}