using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WcsModel;
using WCS.OPC;

namespace WCS.Communications
{
    /// <summary>
    /// 电子锯空垫板站台交互类
    /// </summary>
    public class CuttingEmptyStationCommunication: Singleton<CuttingEmptyStationCommunication>
    {
        private CuttingEmptyStationCommunication() { }

        /// <summary>
        /// 是否有入库请求
        /// </summary>
        /// <param name="eCuttingEmptyStation">开料锯空垫板站台</param>
        /// <returns></returns>
        public bool IsInStockRequest(ECuttingEmptyStation eCuttingEmptyStation)
        {
            return OpcHsc.ReadPLCRequest(Convert.ToInt32(eCuttingEmptyStation));
        }

        /// <summary>
        /// 清除请求
        /// </summary>
        /// <param name="eCuttingEmptyStation">开料锯空垫板站台</param>
        public void ClearInStockRequest(ECuttingEmptyStation eCuttingEmptyStation)
        {
            OpcHsc.ClearPLCRequest(Convert.ToInt32(eCuttingEmptyStation));
        }
    }
}
