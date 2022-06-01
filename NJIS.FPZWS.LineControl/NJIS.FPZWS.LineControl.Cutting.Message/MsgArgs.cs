using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class MsgArgs:MqttMessageArgsBase
    {
        public MsgArgs() : base() { }

        public MsgArgs(string message) : this()
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
