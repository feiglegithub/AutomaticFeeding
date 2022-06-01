using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Manager
{
    [IniFile]
    public class CuttingSerialPortSetting:SettingBase<CuttingSerialPortSetting>
    {
        /// <summary>
        /// 是否自动
        /// </summary>
        [Property("IsAuto")]
        public bool IsAuto { get; set; }

        /// <summary>
        /// 串口名
        /// </summary>
        [Property("PortName")]
        public string PortName { get; set; } = "COM1";

        /// <summary>
        /// 数据位
        /// </summary>
        [Property("DataBits")]
        public int DataBits { get; set; } = 8;

        /// <summary>
        /// 停止位
        /// </summary>
        [Property("StopBits")]
        public string StopBits { get; set; } = "None";

        /// <summary>
        /// 奇偶校验
        /// </summary>
        [Property("Parity")]
        public string Parity { get; set; } = "None";

        /// <summary>
        /// 波特率
        /// </summary>
        [Property("BaudRate")]
        public int BaudRate { get; set; } = 115200;
        
        /// <summary>
        /// Plc Ip地址
        /// </summary>
        [Property("PlcIp")]
        public string PlcIp { get; set; } = "10.30.40.10";

        /// <summary>
        /// 请求Trigger值地址
        /// </summary>
        [Property("TriggerInDbAddr")]
        public string TriggerInDbAddr { get; set; } = "DB450.646";

        /// <summary>
        /// 回应Trigger值地址
        /// </summary>
        [Property("TriggerOutDbAddr")]
        public string TriggerOutDbAddr { get; set; } = "DB450.676";

        /// <summary>
        /// 回应板件号地址
        /// </summary>
        [Property("PartIdDbAddr")]
        public string PartIdDbAddr { get; set; } = "DB450.650";

        /// <summary>
        /// 回应批次地址
        /// </summary>
        [Property("BatchNameDbAddr")]
        public string BatchNameDbAddr { get; set; } = "DB450.684";

        /// <summary>
        /// 回应结果地址
        /// </summary>
        [Property("ResultDbAddr")]
        public string ResultDbAddr { get; set; } = "DB450.680";
    }
}
