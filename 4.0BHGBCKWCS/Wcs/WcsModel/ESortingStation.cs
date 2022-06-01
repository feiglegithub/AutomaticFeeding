using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WcsModel
{
    /// <summary>
    /// 站台
    /// </summary>
    public enum ESortingStation
    {
        SortingStation2001 = 2001,
        SortingStation2002 = 2002,
        SortingStation2003 = 2003,
        SortingStation2004 = 2004,
        SortingStation2005 = 2005,
    }

    /// <summary>
    /// 板材出入库站台
    /// </summary>
    public enum EInOutStation
    {
        /// <summary>
        /// 板材入库站台
        /// </summary>
        InStockStationGt102 = 105,
        /// <summary>
        /// 板材出库站台
        /// </summary>
        OutStockStationGt116 = 100,

        /// <summary>
        /// 出库到Rgv站台
        /// </summary>
        OutStockStationGt208 = 1001,
    }

    /// <summary>
    /// 开料锯空垫板站台
    /// </summary>
    public enum ECuttingEmptyStation
    {
        /// <summary>
        /// 电子锯退空垫板站台Gt307
        /// </summary>
        CuttingEmptyStationGt307 = 3011,

        /// <summary>
        /// 电子锯退空垫板站台Gt317
        /// </summary>
        CuttingEmptyStationGt317 = 3012
    }

    /// <summary>
    /// 开料站台
    /// </summary>
    public enum ECuttingStation
    {
        /// <summary>
        /// 开料站台 3001
        /// </summary>
        CuttingStation3001 = 3001,
        /// <summary>
        /// 开料站台 3002
        /// </summary>
        CuttingStation3002 = 3002,
        /// <summary>
        /// 开料站台 3003
        /// </summary>
        CuttingStation3003 = 3003,
        /// <summary>
        /// 开料站台 3004
        /// </summary>
        CuttingStation3004 = 3004,
        /// <summary>
        /// 开料站台 3005
        /// </summary>
        CuttingStation3005 = 3005,
        /// <summary>
        /// 开料站台 3006
        /// </summary>
        CuttingStation3006 = 3006,
        /// <summary>
        /// 开料站台 3007
        /// </summary>
        CuttingStation3007 = 3007,
    }
}
