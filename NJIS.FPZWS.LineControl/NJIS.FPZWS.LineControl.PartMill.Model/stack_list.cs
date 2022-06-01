using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class stack_list
    {
        public long id { get; set; }
        public int stack_name { get; set; }
        public int stack_index { get; set; }
        public string project { get; set; }
        public string jobname { get; set; }
        public string batch_name { get; set; }
        public string color { get; set; }
        public double width { get; set; }
        public double length { get; set; }
        public double thinkness { get; set; }
        public int part_count { get; set; }
        public int book_type { get; set; } = 1;
        public int book_status { get; set; } = 0;
        public DateTime plan_date { get; set; }=DateTime.Today;
        public int wms_status { get; set; } = 0;
        public int finished_status { get; set; } = 0;
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }

    }
}
