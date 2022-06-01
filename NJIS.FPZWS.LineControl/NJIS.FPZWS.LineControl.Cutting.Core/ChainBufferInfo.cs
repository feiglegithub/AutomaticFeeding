using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    /// <summary>
    ///     链式缓存
    /// </summary>
    public class ChainBufferInfo
    {
        public ChainBufferInfo()
        {
            Parts = new List<ControlPartInfo>();
        }
        public CuttingChainBuffer CuttingChainBuffer { get; set; }
        public List<ControlPartInfo> Parts { get; set; }
    }
}
