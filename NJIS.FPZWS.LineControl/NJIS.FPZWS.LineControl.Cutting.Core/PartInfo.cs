using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    /// <summary>
    ///     板件
    /// </summary>
    public class PartInfo
    {
        public string PartId { get; set; }
        public string BatchName { get; set; }
        //public string OrderNumber { get; set; }
        public string PcsMessage { get; set; }

    }
}
