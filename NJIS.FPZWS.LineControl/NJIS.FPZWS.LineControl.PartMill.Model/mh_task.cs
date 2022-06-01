using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class mh_task
    {
        public long id { get; set; }
        public short command_value { get; set; }
        public string commad_db_address { get; set; }
        public short task_type { get; set; }
        public int status { get; set; }
        public int machine_no { get; set; }
        public bool is_enable { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? finished_time { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }

    }
}
