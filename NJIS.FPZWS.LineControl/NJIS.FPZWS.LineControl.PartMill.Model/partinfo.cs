using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class partinfo
    {
        public int id { get; set; }
        public string part_id { get; set; }
        public int task_id { get; set; }
        public string batch_name { get; set; }
        public string order_number { get; set; }
        public float length { get; set; }
        public float width { get; set; }
        public float thickness { get; set; }
        public string color { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
