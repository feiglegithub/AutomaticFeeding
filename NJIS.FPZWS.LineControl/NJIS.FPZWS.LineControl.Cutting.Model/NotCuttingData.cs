using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class NotCuttingData
    {
        public DateTime PlanDate { get; set; }
        public string BatchName { get; set; }
        public string StackName { get; set; }
        public string SawFileName { get; set; }
        public int StackIndex { get; set; }
        public int BoardCount { get; set; }
        public string Status { get; set; }
    }
}
