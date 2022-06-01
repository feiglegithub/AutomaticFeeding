using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class ResponseModel
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
        public string Address { get; set; }
        public short ReturnValue { get; set; }
    }
}
