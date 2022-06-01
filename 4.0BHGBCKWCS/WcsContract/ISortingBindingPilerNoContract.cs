using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface ISortingBindingPilerNoContract
    {
        List<SortingBindingPilerNo> GetBindingPilerNoes(long groupId);

        bool BulkInsertSortingBindingPilerNo(List<SortingBindingPilerNo> sortingBindingPilerNoes);
    }
}
