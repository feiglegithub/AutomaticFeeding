using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace ArithmeticsTest
{
    internal class BookStack
    {
        public Stack Stack { get; set; }

        public List<StackDetail> StackDetails { get; set; } = new List<StackDetail>();
    }
}
