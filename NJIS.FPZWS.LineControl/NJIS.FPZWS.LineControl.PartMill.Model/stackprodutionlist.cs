using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class stackprodutionlist
    {
        public int id { get; set; }
        public string stack_name { get; set; }
        public int status { get; set; }
        public string place_no { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
