using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public class DefaultCuttingBuilder:CuttingBuilder
    {
        public override List<ChainBufferInfo> CreateChainBuffers()
        {
            return new List<ChainBufferInfo>();
        }

        public override InParter CreateInParter()
        {
            return new InParter();
        }

        public override ISpotter CreateSploter()
        {
            return  new SlotterBase();
        }

        public override List<ICollector> CreateCollectors()
        {
            return new List<ICollector>();
        }
    }
}
