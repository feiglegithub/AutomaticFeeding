using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public abstract class CuttingBuilder
    {
        /// <summary>
        ///     创建链式缓存
        /// </summary>
        /// <returns></returns>
        public abstract List<ChainBufferInfo> CreateChainBuffers();

        /// <summary>
        ///     创建入板器
        /// </summary>
        /// <returns></returns>
        public abstract InParter CreateInParter();

        /// <summary>
        ///     创建抽检器
        /// </summary>
        /// <returns></returns>
        public abstract ISpotter CreateSploter();

        public abstract List<ICollector> CreateCollectors();
    }
}
