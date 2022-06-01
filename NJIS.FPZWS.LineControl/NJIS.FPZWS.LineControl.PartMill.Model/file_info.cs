using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class file_info
    {
        public long id { get; set; }
        public string batch_name { get; set; }
        public string color { get; set; }
        public double width { get; set; }
        public double length { get; set; }
        public double thinkness { get; set; }
        public int estimated_time { get; set; } = 0;
        public string label_file_fullname { get; set; }
        public string label_name { get; set; }
        public string nc_file_fullname { get; set; }
        public string nc_name { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }

    }
}
