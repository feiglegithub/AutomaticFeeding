using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WcsModel;
using WCS.model;
using WCS.OPC;

namespace WCS.Communications
{
    /// <summary>
    /// 出库站台交互
    /// </summary>
    public class OutStockStationCommunication:Singleton<OutStockStationCommunication>
    {
        private OutStockStationCommunication() { }

        /// <summary>
        /// 堆垛机对应的出库站台是否有物体
        /// </summary>
        /// <param name="ePiler"></param>
        /// <returns></returns>
        public bool OutStationIsFree(EPiler ePiler)
        {
            return !OpcHsc.OutStationHasBoard(Convert.ToInt32(ePiler));
        }

        /// <summary>
        /// 写出库任务给线体
        /// </summary>
        /// <param name="ePiler">堆垛车</param>
        /// <param name="stackNo">垛号</param>
        /// <param name="target"></param>
        public bool WriteTask(EPiler ePiler,int stackNo,int target)
        {
            return OpcHsc.WriteSCStationOutTask(Convert.ToInt32(ePiler), stackNo, target);
        }
    }
}
