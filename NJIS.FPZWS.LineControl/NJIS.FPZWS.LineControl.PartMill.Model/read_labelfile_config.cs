using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class read_labelfile_config
    {
        public long id { get; set; }
        public string type_name { get; set; }
        public string field_name { get; set; }
        public int row_index { get; set; }
        public int column_index { get; set; }
        public bool is_enable { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }

    }
}
