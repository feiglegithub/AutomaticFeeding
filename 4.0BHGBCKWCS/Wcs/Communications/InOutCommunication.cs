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
    /// 板材出入库交互
    /// </summary>
    public class InOutCommunication:Singleton<InOutCommunication>
    {
        private InOutCommunication() { }

        /// <summary>
        /// 是否有板材入库请求
        /// </summary>
        public bool HasInStockRequest => OpcHsc.ReadPLCRequest(Convert.ToInt32(EInOutStation.InStockStationGt102));

        


        /// <summary>
        /// 写入库任务给线体
        /// </summary>
        /// <param name="stackNo">垛号</param>
        /// <param name="target">目标地址</param>
        public bool WriteInStockTaskToLine(int stackNo,int target)
        {
            return OpcHsc.WriteDeviceData("GT102", stackNo, target);
        }

        /// <summary>
        /// 是否有空垫板入库请求
        /// </summary>
        /// <param name="eCuttingEmptyStation">空垫板站台号</param>
        /// <returns></returns>
        public bool HasEmptyInStockRequest(ECuttingEmptyStation eCuttingEmptyStation)
        {
            return OpcHsc.ReadPLCRequest(Convert.ToInt32(eCuttingEmptyStation));
        }

        /// <summary>
        /// 是否有板材入库请求
        /// </summary>
        /// <param name="eInOutStation">入库口站台号</param>
        /// <returns></returns>
        public bool HasEmptyInStockRequest(EInOutStation eInOutStation)
        {
            return OpcHsc.ReadPLCRequest(Convert.ToInt32(eInOutStation));
        }

        /// <summary>
        /// 读取出库站台的垛号
        /// </summary>
        /// <param name="eInOutStation"></param>
        /// <returns></returns>
        public int ReadStationStackNo(EInOutStation eInOutStation)
        {
            return OpcHsc.ReadPilerNoByStationNo(Convert.ToInt32(eInOutStation));
        }

        /// <summary>
        /// 清除入库请求与出库完成请求
        /// </summary>
        /// <param name="eInOutStation">站台</param>
        public bool ClearRequest(EInOutStation eInOutStation)
        {
            return OpcHsc.ClearPLCRequest(Convert.ToInt32(eInOutStation));
        }

        /// <summary>
        /// 板材出库完成
        /// </summary>
        public bool OutStockIsFinished => OpcHsc.ReadIsTaskDone(Convert.ToInt32(EInOutStation.OutStockStationGt116));

        /// <summary>
        /// 板材到达Rgv站台
        /// </summary>
        public bool IsReachedRgv=> OpcHsc.ReadIsTaskDone(Convert.ToInt32(EInOutStation.OutStockStationGt208));
    }
}
