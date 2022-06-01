using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PartMill.Model;

namespace NJIS.FPZWS.LineControl.PartMill.Client
{
    public class ReadLableInfo
    {
        public read_labelfile_config Config { get; set; }
        public object ConvertData { get; set; }
        public string[] ReadStrings { get; set; }
    }
}
