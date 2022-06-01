using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class devicehearbeat
    {
        public int id { get; set; }
        public string device_name { get; set; }
        public int device_status { get; set; }
        public DateTime updated_time { get; set; }
    }
}
