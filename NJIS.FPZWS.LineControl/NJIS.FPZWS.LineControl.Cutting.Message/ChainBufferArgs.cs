using NJIS.FPZWS.LineControl.Cutting.Core;
using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    [Serializable]
    public class ChainBufferArgs:MqttMessageArgsBase
    {
        public ChainBufferArgs() : base() { }

        public CuttingChainBuffer CuttingChainBuffer { get; set; }
        public List<PartInfoArgs> PartInfoArgses { get; set; }
    }
}
