using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface ISortingInStockRequestContract
    {
        List<SortingInStockRequest> GetSortingInStockRequests();

        bool UpdatedRequestStatus(SortingInStockRequest sortingInStockRequest);

    }
}
