//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：AlarmArgs.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    [Serializable]
    public class AlarmArgs : MqttMessageArgsBase
    {
        public AlarmArgs()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        ///     报警类别
        ///     PLC,PCS
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     报警参数名称
        ///     主要指采集的PLC参数，或线控命令
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        ///     报警值
        ///     采集参数，或描述
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 报警标识
        /// </summary>
        public string AlarmId { get; set; }

        public override string ToString()
        {
            return $"{Category}=>{ParamName}:{Value}";
        }

        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class  PlcAlarmArgs: AlarmArgs
    {
        public PlcAlarmArgs(string pn, string msg)
        {
            Category = "PLC";
            ParamName = pn;
            Value = msg;
        }
    }

    public class PcsAlarmArgs : AlarmArgs
    {
        public PcsAlarmArgs(string pn, string msg)
        {
            Category = "PCS";
            ParamName = pn;
            Value = msg;
        }
    }

    public class PcsLogicAlarmArgs : AlarmArgs
    {
        public PcsLogicAlarmArgs(string msg)
        {
            Category = "PCS";
            ParamName = "Logic";
            Value = msg;
        }
    }

    public class PcsErrorAlarmArgs : AlarmArgs
    {
        public PcsErrorAlarmArgs(string msg)
        {
            Category = "PCS";
            ParamName = "Error";
            Value = msg;
        }
    }
}