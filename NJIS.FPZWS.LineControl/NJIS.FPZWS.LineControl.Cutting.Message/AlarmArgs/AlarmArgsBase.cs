using System;

namespace NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs
{
    [Serializable]
    public class AlarmArgsBase:MqttMessageArgsBase
    {
        public AlarmArgsBase() : base() { }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 报警参数（PLC采集的参数，或线控命令）
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 报警值（采集参数,或描述）
        /// </summary>
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Category}=>{ParamName}:{Value}";
        }
    }

}
