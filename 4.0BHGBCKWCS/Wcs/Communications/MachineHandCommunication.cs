using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WCS.OPC;
using WcsModel;

namespace WCS.Communications
{
    /// <summary>
    /// 机械手交互类
    /// </summary>
    public class MachineHandCommunication:Singleton<MachineHandCommunication>
    {
        private MachineHandCommunication() { }

        /// <summary>
        /// 机械手是否空闲
        /// </summary>
        public bool IsFree => OpcHsc.RMCanDo(1);

        /// <summary>
        /// 给机械手写任务
        /// </summary>
        /// <param name="fromStation">起始站点</param>
        /// <param name="toStation">目标站点</param>
        /// <returns></returns>
        public bool WriteTask(int fromStation, int toStation)
        {
            return OpcHsc.WriteToMainpulator(fromStation, toStation);
        }
        /// <summary>
        /// 机械手完成
        /// </summary>
        /// <returns></returns>
        public bool IsFinished=>OpcHsc.RStop();


        /// <summary>
        /// 读取拣选工位的板材数
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public int ReadSortingStationBoardCount(ESortingStation eSortingStation)
        {
            return OpcHsc.ReadBoradsCount(Convert.ToInt32(eSortingStation));
        }

        public void ClearMhTask()
        {
            OpcHsc.ClearMhTask();
        }
        /// <summary>
        /// 读取拣选工位的板材数
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public int ReadSortingStationBoardCount(int stationNo)
        {
            return OpcHsc.ReadBoradsCount(stationNo);
        }

    }
}
