using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Drilling.Message
{
    [Serializable]
    public class PcsNgArgs
    {
        public string PartId { get; set; }

        public string Msg { get; set; }

        public Nullable<int> Status { get; set; }

        public System.DateTime CreatedTime { get; set; }

        public System.DateTime UpdatedTime { get; set; }
    }
}
