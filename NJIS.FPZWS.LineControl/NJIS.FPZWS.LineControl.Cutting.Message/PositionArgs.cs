using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    public class PositionArgs:MqttMessageArgsBase
    {
        public int Position { get; set; }
        public string PartId { get; set; }
    }
}
