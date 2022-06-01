//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Message
//   文 件 名：MqttMessageBase.cs
//   创建时间：2018-11-28 16:59
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:59
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.LineControl.Drilling.Message
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
