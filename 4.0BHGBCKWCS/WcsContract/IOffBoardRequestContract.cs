using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IOffBoardRequestContract
    {
        List<OffBoardRequest> GetOffBoardRequests();
        bool UpdatedOffBoardRequest(OffBoardRequest offBoardRequest);
    }
}
