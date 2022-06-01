using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Models
{
    public class LineModel
    {
        public string LineName { get; set; }
        public bool HasBoard { get; set; }
        public int PilerNo { get; set; }
        public short Target { get; set; }
        public bool IsFinished { get; set; }
        public short Amount { get; set; }
        public bool NeedRun { get; set; }
        public short BackupShort { get; set; }
        public string BackupString { get; set; }
        public short Position { get; set; }
    }
}
