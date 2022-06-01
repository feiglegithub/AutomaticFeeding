using WCS.WebServiceDemo;

namespace WCS.Interfaces
{
    public interface IWms
    {
        /// <summary>
        /// 申请空垫板
        /// </summary>
        /// <param name="boardCount"></param>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        ResultMsg ApplyEmptyBoard(int boardCount,int stationNo);

        /// <summary>
        /// 拣选回库
        /// </summary>
        /// <param name="taskId">任务号</param>
        /// <param name="fromStation">起始位</param>
        /// <returns></returns>
        ResultMsg ApplySortingInStock(long taskId, int fromStation);

        /// <summary>
        /// 空垫板回库
        /// </summary>
        /// <param name="boardCount">数量</param>
        /// <param name="fromStation">起始位</param>
        /// <returns></returns>
        ResultMsg ApplyEmptyBoardInStock(int boardCount,int fromStation);

        /// <summary>
        /// 余料回库
        /// </summary>
        /// <param name="boardCount">数量</param>
        /// <param name="fromStation">起始位</param>
        /// <param name="stackNo">垛号</param>
        /// <returns></returns>
        ResultMsg ApplyBoardReenterStock(int boardCount, int fromStation,int stackNo);

        /// <summary>
        /// 拣选要料请求
        /// </summary>
        /// <param name="boardCount">板材数</param>
        /// <param name="toStation">目标位</param>
        /// <param name="productCode">花色</param>
        /// <returns></returns>
        ResultMsg ApplySortingMaterial(int boardCount, int toStation, string productCode);

    }
}