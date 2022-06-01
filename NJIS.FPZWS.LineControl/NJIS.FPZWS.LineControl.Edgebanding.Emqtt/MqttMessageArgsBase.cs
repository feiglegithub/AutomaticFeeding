//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：MqttMessageArgsBase.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    public class MqttMessageArgsBase
    {
        public MqttMessageArgsBase()
        {
            CreatedTime = DateTime.Now;
        }

        public DateTime CreatedTime { get; set; }
    }
}