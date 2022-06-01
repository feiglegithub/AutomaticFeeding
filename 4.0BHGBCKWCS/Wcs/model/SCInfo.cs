using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class SCInfo
    {
        public int IsActivation { get; set; }
        public int IsAuto { get; set; }
        public int IsFree { get; set; }
        public int State { get; set; }
        public long PilerNo { get; set; }
        public int TaskType { get; set; }
        public int TaskStatus { get; set; }
        public string CurrentPos { get; set; }
        public string FromPos { get; set; }
        public string ToPos { get; set; }
        public int OutStationState { get; set; }
        public int InStationState { get; set; }
        public string ErrorMsg { get; set; }
    }
}
