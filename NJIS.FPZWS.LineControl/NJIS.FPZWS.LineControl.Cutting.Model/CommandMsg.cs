using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class CommandMsg
    {
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime StartTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public TimeSpan TimeSpan => FinishedTime - StartTime;
        public string CommandName { get; set; }
        public string Msg { get; set; }

    }
}
