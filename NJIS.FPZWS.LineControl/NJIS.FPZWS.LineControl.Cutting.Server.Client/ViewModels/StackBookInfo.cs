using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    /// <summary>
    /// 垛板材的信息
    /// </summary>
    public class StackBookInfo
    {
        public short PTN_INDEX { get; set; }

        public int StackIndex { get; set; }

        public string RawMaterialId { get; set; }

        public int CycleTime { get; set; }

        
    }
}
