using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCSiemensDAAutomation;

namespace WCS
{
    public class OPCGroupEntity
    {
        public OPCGroup OpcGroupObj { get; set; }

        public List<OPCItem> OpcItemList { get; set; }
    }
}