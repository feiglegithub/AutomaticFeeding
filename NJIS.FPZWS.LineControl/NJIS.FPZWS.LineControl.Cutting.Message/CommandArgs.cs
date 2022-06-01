using System;


namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class CommandArgs:MqttMessageArgsBase
    {
        public CommandArgs() : base() { }
        public string CommandCode { get; set; }
        public object Input { get; set; }
        public object Output { get; set; }
        public CommandType Type { get; set; }
    }

    public enum CommandType
    {
        /// <summary>
        /// 开始
        /// </summary>
        S=10,
        /// <summary>
        /// 结束
        /// </summary>
        E=11
    }
}

