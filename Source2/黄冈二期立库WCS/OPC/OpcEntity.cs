using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCSiemensDAAutomation;

namespace WCS
{
    public class OPCGroupEntity
    {
        private OPCGroup opcGroup;

        public OPCGroup OpcGroupObj
        {
            get
            {
                return opcGroup;
            }
            set
            {
                opcGroup = value;
            }
        }

        private List<OPCItem> opcItem;

        public List<OPCItem> OpcItemList
        {
            get
            {
                return opcItem;
            }
            set
            {
                opcItem = value;
            }

        }
    }
}