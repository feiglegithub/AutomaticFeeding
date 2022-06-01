//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//   项目名称：NJIS.FPZWS.MqttClient
//   文 件 名：MqttMsg.cs
//   创建时间：2019-04-23 8:58
//   作    者：
//   说    明：
//   修改时间：2019-04-23 8:58
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.MqttClient
{
    public class MqttMsg
    {
        public MqttMsg(string topic, object datas, bool retain = false)
        {
            Topic = topic;
            Datas = datas;
            Retain = retain;
            Key = Guid.NewGuid();
        }

        public Guid Key { get; }
        public string Topic { get; set; }
        public object Datas { get; set; }
        public bool Retain { get; set; }
    }
}