using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Model
{
    public class line_config
    {
        public int id { get; set; }
        public string line_name   { get; set; }
        public short position    { get; set; }
        public string has_board_address   { get; set; }
        public string piler_no_address    { get; set; }
        public string target_address  { get; set; }
        public string is_finished_address { get; set; }
        public string amount_address  { get; set; }
        public string run_signal_address  { get; set; }
        public string accept_task_address { get; set; }
        //public string backup_short_address    { get; set; }
        public string backup_string_address   { get; set; }
        public DateTime created_time    { get; set; }
        public DateTime updated_time    { get; set; }

    }
}
