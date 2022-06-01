using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class wmsstackfeedback
    {
        public long id { get; set; }
        public string stack_name { get; set; }
        public bool is_success { get; set; }
        public string fail_reason { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
