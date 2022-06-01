using NJIS.FPZWS.LineControl.PLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Entitys
{
    public class ChainBufferInputEntity : EntityBase
    {
        /// <summary>
        ///     链式缓存编号
        /// </summary>
        public string Code { get; set; }

        public byte[] Buffer { get; set; }
    }
}
