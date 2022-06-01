using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class line_task
    {
        public int id { get; set; }
        public int piler_no { get; set; }
        public int task_type { get; set; }
        public short start_position { get; set; }
        public short end_position { get; set; }
        public int task_status { get; set; }
        public bool is_enable { get; set; }
        public bool is_accept { get; set; }
        public DateTime start_time { get; set; }
        public DateTime Finished_time { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }

    }
}
