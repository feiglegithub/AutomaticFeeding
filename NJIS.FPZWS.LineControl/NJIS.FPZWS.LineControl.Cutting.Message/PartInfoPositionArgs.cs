using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class PartInfoPositionArgs:MqttMessageArgsBase
    {
        public PartInfoPositionArgs() : base() { }

        public string PartId { get; set; }
        public int Position { get; set; }
        public DateTime Time { get; set; }
    }
}
