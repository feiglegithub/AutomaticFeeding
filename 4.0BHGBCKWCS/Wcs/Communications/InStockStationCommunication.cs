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
    /// 入库站台交互
    /// </summary>
    public class InStockStationCommunication:Singleton<InStockStationCommunication>
    {
        private InStockStationCommunication() { }

        /// <summary>
        /// 堆垛机对应的入库站台是否有入库请求
        /// </summary>
        /// <param name="ePiler">堆垛机</param>
        /// <returns></returns>
        public bool IsRequestInStock(EPiler ePiler)
        {
            return OpcHsc.IsStationInRequest(Convert.ToInt32(ePiler));
        }

        /// <summary>
        /// 读取堆垛车任务的垛号
        /// </summary>
        /// <param name="ePiler"></param>
        /// <returns></returns>
        public int StackNo(EPiler ePiler)
        {
            return OpcHsc.ReadStationPiler(Convert.ToInt32(ePiler));
        }
    }
}
