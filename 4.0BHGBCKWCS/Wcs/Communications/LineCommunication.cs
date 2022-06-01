using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.Common;
using WcsModel;
using WCS.model;
using WCS.OPC;

namespace WCS.Communications
{
    /// <summary>
    /// 线体交互
    /// </summary>
    public class LineCommunication:Singleton<LineCommunication>
    {
        private LineCommunication() { }

        /// <summary>
        /// 写入开料站台是否有上保护板
        /// </summary>
        /// <param name="eCuttingStation">开料站台</param>
        /// <param name="hasUpProtect">是否有上保护板</param>
        public bool WriteUpToCutStation(ECuttingStation eCuttingStation,bool hasUpProtect)
        {
            int stationNo = Convert.ToInt32(eCuttingStation);
            return OpcHsc.WriteUpToCutStation(stationNo, hasUpProtect ? 1 : 0);
        }

        /// <summary>
        /// 写花色到工位
        /// </summary>
        /// <param name="station"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool WriteProductCodeToStation(int station,string productCode)
        {
            return OpcHsc.WriteProductCodeToStaion(station, productCode);
        }

        /// <summary>
        /// 读取拣选工位的板材数
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public int ReadStationBoardCount(ESortingStation eSortingStation)
        {
            return OpcHsc.ReadBoradsCount(Convert.ToInt32(eSortingStation));
        }

        /// <summary>
        /// 写花色到工位
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool WriteProductCodeToStation(ESortingStation eSortingStation, string productCode)
        {
            return OpcHsc.WriteProductCodeToStaion(Convert.ToInt32(eSortingStation), productCode);
        }

        public bool ClearPlcRequest(int stationNo) => OpcHsc.ClearPLCRequest(stationNo);

        public bool ClearPlcRequest(ESortingStation eSortingStation) => OpcHsc.ClearPLCRequest(Convert.ToInt32(eSortingStation));

        public bool ClearPlcRequest(ECuttingStation eCuttingStation) => OpcHsc.ClearPLCRequest(Convert.ToInt32(eCuttingStation));

        /// <summary>
        /// 写入开料站台是否有上保护板
        /// </summary>
        /// <param name="stationNo">开料站台</param>
        /// <param name="hasUpProtect">是否有上保护板</param>
        public void WriteUpToCutStation(int stationNo, bool hasUpProtect)
        {
            OpcHsc.WriteUpToCutStation(stationNo, hasUpProtect ? 1 : 0);
        }

        /// <summary>
        /// 给线体写入库任务
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="stackNo"></param>
        /// <param name="ePiler"></param>
        public bool WriteInStockTask(int stationNo, int stackNo, EPiler ePiler)
        {
            return OpcHsc.WriteDeviceData(stationNo, stackNo, 105-Convert.ToInt32(ePiler));
        }

        /// <summary>
        /// 写入板材数量到站台
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <param name="boardCount">包含上下保护板</param>
        /// <returns></returns>
        public bool WriteBoardCountToStation(ESortingStation eSortingStation, int boardCount)
        {
            return WriteBoardCountToStation(Convert.ToInt32(eSortingStation), boardCount);
        }

        /// <summary>
        /// 写入板材数量到站台
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="boardCount">包含上下保护板</param>
        /// <returns></returns>
        public bool WriteBoardCountToStation(int stationNo, int boardCount)
        {
            return OpcHsc.WriteBoradsCountToStaion(stationNo, boardCount);
        }

        /// <summary>
        /// 给线体写入库任务
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="stackNo"></param>
        /// <param name="ddjNo">堆垛机</param>
        public bool WriteInStockTask(int stationNo, int stackNo, int ddjNo)
        {
            return OpcHsc.WriteDeviceData(stationNo, stackNo, 105 - ddjNo);
        }

        /// <summary>
        /// 拣选工位是否正在出垛
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public bool StackIsOutingFinished(ESortingStation eSortingStation)
        {
            return OpcHsc.ReadSortingStationOut(Convert.ToInt32(eSortingStation));
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public bool ClearStackOuting(ESortingStation eSortingStation)
        {
            return OpcHsc.ClearSortingStationOut(Convert.ToInt32(eSortingStation));
        }

        public void SetSortingStationOut(ESortingStation eSortingStation)
        {
            OpcHsc.SetSortingStationOut(Convert.ToInt32(eSortingStation));
        }

        /// <summary>
        /// 读取线体的数据
        /// </summary>
        /// <param name="dno"></param>
        /// <returns></returns>
        public DeviceData ReadDeviceDataByNo(string dno)
        {
            return OpcHsc.ReadDeviceDataByNo(dno);
        }

        /// <summary>
        /// 读取线体垛号
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public int ReadStackNo(int stationNo)
        {
            return OpcHsc.ReadPilerNoByStationNo(stationNo);
        }
        /// <summary>
        /// 读取出入库站台垛号
        /// </summary>
        /// <param name="eInOutStation"></param>
        /// <returns></returns>
        public int ReadStackNo(EInOutStation eInOutStation)
        {
            return OpcHsc.ReadPilerNoByStationNo(Convert.ToInt32(eInOutStation));
        }
        /// <summary>
        /// 读取开料站台垛号
        /// </summary>
        /// <param name="eCuttingStation"></param>
        /// <returns></returns>
        public int ReadStackNo(ECuttingStation eCuttingStation)
        {
            return OpcHsc.ReadPilerNoByStationNo(Convert.ToInt32(eCuttingStation));
        }
        /// <summary>
        /// 读取拣选站台垛号
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public int ReadStackNo(ESortingStation eSortingStation)
        {
            return OpcHsc.ReadPilerNoByStationNo(Convert.ToInt32(eSortingStation));
        }

        /// <summary>
        /// 写入线体的垛号
        /// </summary>
        /// <param name="stationNo">站台号</param>
        /// <param name="stackNo">垛号</param>
        /// <param name="target">目标地址</param>
        public bool WriteStackNo(int stationNo,int stackNo,int target)
        {
            return OpcHsc.WriteDeviceData(stationNo, stackNo, target);
        }

        /// <summary>
        /// 写入线体的垛号
        /// </summary>
        /// <param name="stationName">站台号</param>
        /// <param name="stackNo">垛号</param>
        /// <param name="target">目标地址</param>
        public bool WriteStackNo(string stationName, int stackNo, int target)
        {
            return OpcHsc.WriteDeviceData(stationName, stackNo, target);
        }
        /// <summary>
        /// 线体是否到达完成（出库口、Rgv站台口）
        /// </summary>
        /// <param name="eInOutStation"></param>
        /// <returns></returns>
        public bool IsDone(EInOutStation eInOutStation)
        {
            return OpcHsc.ReadIsTaskDone(Convert.ToInt32(eInOutStation));
        }

        /// <summary>
        /// 线体是否到达开料锯完成（开料站台）
        /// </summary>
        /// <param name="eCuttingStation"></param>
        /// <returns></returns>
        public bool IsDone(ECuttingStation eCuttingStation)
        {
            return OpcHsc.ReadIsTaskDone(Convert.ToInt32(eCuttingStation));
        }

        /// <summary>
        /// 线体是否到达拣选站台
        /// </summary>
        /// <param name="eSortingStation"></param>
        /// <returns></returns>
        public bool IsDone(ESortingStation eSortingStation)
        {
            return OpcHsc.ReadIsTaskDone(Convert.ToInt32(eSortingStation));
        }

    }
}
