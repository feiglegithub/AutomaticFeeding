﻿// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：TaskCenterSetting.cs
//  创建时间：2017-10-19 8:17
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region namespace



#endregion

using NJIS.Ini;

namespace NJIS.FPZWS.MqttClient.Setting
{
    [IniFile]
    public class TaskCenterSetting : SettingBase<TaskCenterSetting>
    {
        protected TaskCenterSetting()
        {
        }


        [Property("MqttServer")]
        public string IP { get; set; } = "10.18.17.111";

        [Property("MqttServer")]
        public int Port { get; set; } = 1883;

        [Property("MqttServer")]
        public string User { get; set; }

        [Property("MqttServer")]
        public string Password { get; set; }

        [Property("MqttServer")]
        public string ClientId { get; set; } = "DefaultClient";


        [Property("MqttServer")]
        public bool ZipCompress { get; set; } = true;

        [Property("MqttServer")]
        public bool MessagePack { get; set; } = true;

        [Property("Heartbeat")]
        public int EnableHeartbeat { get; set; } = 1;

        [Property("Heartbeat")]
        public string HeartbeatTopic { get; set; } = "/sfy/heartbeat";

        [Property("Heartbeat")]
        public int HeartbeatInterval { get; set; } = 5000;

        [Property("MqttServer")]
        public int LogLevel { get; set; } = 0;


        [Property("MqttTask")]
        public bool Enable { get; set; } = true;

        [Property("MqttTask")]
        public int QueueSendInterval { get; set; } = 1;

    }
}