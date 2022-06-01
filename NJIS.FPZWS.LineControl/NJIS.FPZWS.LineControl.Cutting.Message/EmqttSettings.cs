using NJIS.Ini;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [IniFile]
    public class EmqttSettings: SettingBase<EmqttSettings>
    {
        public const string CuttingTopic = "/sfy/rx/pcs/cutting/1";

        [Property("topic")]
        public string CuttingReq { get; set; } = $"{CuttingTopic}/req/#";

        [Property("topic")]
        public string PcsTaskReq { get; set; } = "/sfy/rx/pcs/cutting/1/req/Task";
        /// <summary>
        /// 通知客户端准备任务（Mdb转换Saw）
        /// </summary>
        [Property("topic")]
        public string PcsTaskRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/Task";

        /// <summary>
        /// 通知客户端下载新任务Mdb
        /// </summary>
        [Property("topic")]
        public string PcsDownMdbRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/DownMdb";

        /// <summary>
        /// 向服务端请求下载Mdb
        /// </summary>
        [Property("topic")]
        public string PcsDownMdbReq { get; set; } = "/sfy/rx/pcs/cutting/1/req/DownMdb";

        [Property("topic")]
        public string PcsMsg { get; set; } = "/sfy/rx/pcs/cutting/1/msg";

        [Property("topic")]
        public string PcsAlarmRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/alarm";
        [Property("topic")]
        public string PcsAlarmReq { get; set; } = "/sfy/rx/pcs/cutting/1/req/alarm";

        [Property("topic")]
        public string PcsChanageStatusChainBufferRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/ChangeStatusChainBuffer";

        [Property("topic")]
        public string PcsChanageStatusChainBufferReq { get; set; } = "/sfy/rx/pcs/cutting/1/req/ChangeStatusChainBuffer";

        [Property("topic")]
        public string PcsChainBufferRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/ChainBufferInfo";

        [Property("topic")]
        public string PcsInitQueueRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/InitQueue";

        public string PcsCommandStart { get; set; } = "/sfy/rx/pcs/cutting/1/command/s";
        public string PcsCommandEnd { get; set; } = "/sfy/rx/pcs/cutting/1/command/e";

        public string PcsPartInfoPositionRep { get; set; } = "/sfy/rx/pcs/cutting/1/rep/PartInfoPositionCommand";
        public string PcsPartInfoPositionReq { get; set; } = "/sfy/rx/pcs/cutting/1/req/PartInfoPositionCommand";
    }
}
