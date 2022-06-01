using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IWcsLogContract
    {
        int InsertWcsLog(string msg, int pilerNo = 0, string device = "");

        int InsertWcsErrorLog(string msg, int pilerNo = 0, string device = "");
    }
}
