using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IEmptyBoardRequestContract
    {
        /// <summary>
        /// 获取2005空垫板入库请求
        /// </summary>
        /// <returns></returns>
        List<EmptyBoardRequest> GetEmptyBoardInStockRequests();

        /// <summary>
        /// 获取空垫板出库请求（到2005）
        /// </summary>
        /// <returns></returns>
        List<EmptyBoardRequest> GetEmptyBoardOutStockRequests();

        List<EmptyBoardRequest> GetEmptyBoardRequests();

        bool UpdatedEmptyBoardRequest(EmptyBoardRequest emptyBoardRequest);
    }
}
