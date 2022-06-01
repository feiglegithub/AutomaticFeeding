using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class task
    {
        public long id { get; set; }
        public int task_id { get; set; }
        public string file_path { get; set; }
        public string device_name { get; set; }
        public int task_status { get; set; }
        public bool is_enable { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
