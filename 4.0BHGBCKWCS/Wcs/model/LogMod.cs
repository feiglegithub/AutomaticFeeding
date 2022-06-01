using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class LogMod
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public long PilerNo { get; set; }
        public string Msg { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Times { get; set; }
    }
}
