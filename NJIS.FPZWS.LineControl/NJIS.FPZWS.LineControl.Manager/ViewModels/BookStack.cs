using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Manager.ViewModels
{
    internal class BookStack
    {
        public Stack Stack { get; set; }

        public List<StackDetail> StackDetails { get; set; } = new List<StackDetail>();
    }
}
