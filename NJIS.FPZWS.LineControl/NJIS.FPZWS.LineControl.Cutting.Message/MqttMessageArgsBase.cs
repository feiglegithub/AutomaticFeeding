using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class MqttMessageArgsBase
    {
        public MqttMessageArgsBase()
        {
            CreatedTime = DateTime.Now;
        }
        public  DateTime CreatedTime { get; set; }
    }
}
