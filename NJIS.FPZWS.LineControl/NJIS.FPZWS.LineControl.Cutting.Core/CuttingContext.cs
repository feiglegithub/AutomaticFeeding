using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public class CuttingContext
    {
        private static CuttingContext _instance = null;

        public static CuttingContext Instance
        {
            get => _instance ?? (_instance = new CuttingContext());
            set => _instance = value;
        }

        internal bool Build(CuttingBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            ChainBuffers = builder.CreateChainBuffers();
            Sploter = builder.CreateSploter();
            InParter = builder.CreateInParter();
            Collectors = builder.CreateCollectors();
            return true;
        }

        public ISpotter Sploter { get; protected set; }
        public InParter InParter { get; protected set; }
        public List<ICollector> Collectors { get; protected set; }
        public List<ChainBufferInfo> ChainBuffers { get; protected set; }
    }
}
