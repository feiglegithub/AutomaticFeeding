using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Entitys
{
    public class PositionInputEntity:DbProcInputEntity
    {
        public int Position { get; set; }
        public string Data { get; set; }
    }
}
