using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class PartInfoQueueArgs: MqttMessageArgsBase
    {
        public PartInfoQueueArgs() : base(){}
        public string PartId { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 下一个位置
        /// </summary>
        //public string NextPlace { get; set; }
        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public int Position { get; set; }
        public int Plc { get; set; }
        public int Pcs { get; set; }
        public ReenterType PartType { get; set; }
        public string PcsMessage { get; set; }
    }
}
