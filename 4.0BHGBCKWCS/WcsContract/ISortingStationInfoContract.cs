using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface ISortingStationInfoContract
    {
        List<SortingStationInfo> GetSortingStationInfos();
        List<SortingStationInfo> GetSortingStationInfos(int stationNo);
        bool UpdatedSortingStationInfo(SortingStationInfo sortingStationInfo);
    }
}
