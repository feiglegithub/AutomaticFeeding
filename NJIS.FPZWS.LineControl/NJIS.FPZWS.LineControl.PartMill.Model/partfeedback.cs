using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class partfeedback
    {
        public int id { get; set; }
        public int task_id { get; set; }
        public string part_id { get; set; }
        public bool is_success { get; set; }
        public DateTime begin_time { get; set; }
        public DateTime finished_time { get; set; }
        public string msg { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
