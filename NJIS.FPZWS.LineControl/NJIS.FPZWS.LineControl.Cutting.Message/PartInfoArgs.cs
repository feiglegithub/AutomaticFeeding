using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class PartInfoArgs:MqttMessageArgsBase
    {
        public PartInfoArgs() : base() { }

        public string PartId { get; set; }
        public string BatchName { get; set; }
    }
}
