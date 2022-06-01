using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public class CuttingChainBuffer
    {
        public long LineId { get; set; }
        public string Code { get; set; }
        public Nullable<int> Status { get; set; }
        public int Size { get; set; }
        public string Remark { get; set; }
    }
}
