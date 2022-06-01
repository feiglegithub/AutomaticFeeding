using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Contract
{
    public partial interface ILineControlCuttingContract
    {
        /// <summary>
        /// 获取生产计划时间上的开料数据
        /// </summary>
        /// <param name="planTime">生产计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingStackList> GetCuttingStackLists(DateTime planTime);

        /// <summary>
        /// 更新分配的堆垛号
        /// </summary>
        /// <param name="cuttingStackLists"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdatedCuttingStackLists(List<CuttingStackList> cuttingStackLists);

        /// <summary>
        /// 更新堆垛信息
        /// </summary>
        /// <param name="cuttingStackLists">分配的堆垛</param>
        /// <param name="spiltMdbResults">mdb信息</param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdatedStackInfos(List<CuttingStackList> cuttingStackLists, List<SpiltMDBResult> spiltMdbResults, List<CuttingPattern> cuttingPatterns);

        /// <summary>
        /// 获取任务的板件列表
        /// </summary>
        /// <param name="taskSpiltMdbResult"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetTaskPartsInfo(SpiltMDBResult taskSpiltMdbResult);

        /// <summary>
        /// 获取MDB拆分结果
        /// </summary>
        /// <param name="planTime">生产计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetSpiltMDBResults(DateTime planTime);

        /// <summary>
        /// 获取未生成的任务（只获取计划时间最前的一天）
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetMdbUnCreatedTasks();

        /// <summary>
        /// 获取未分配设备的任务（拆分结果）
        /// </summary>
        /// <param name="planTime">生产计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetSpiltMDBResultsNoDevice(DateTime planTime);

        /// <summary>
        /// 获取可推送任务
        /// </summary>
        /// <param name="planTime">生产计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCanPushSpiltMDBResults(DateTime planTime);

        /// <summary>
        /// 获取指定设备的MDB
        /// </summary>
        /// <param name="planTime">生产计划时间</param>
        /// <param name="deviceName">设备编号</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetDeviceMDBResults(DateTime planTime, string deviceName);
        /// <summary>
        /// 获取可以请求上料的任务
        /// </summary>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCanLoadMaterialMdbResults(DateTime planTime);

        /// <summary>
        /// 获取已收到上料请求的任务
        /// </summary>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCanLoadingMaterialMdbResults(DateTime planTime);
        /// <summary>
        /// 获取正在上料的任务
        /// </summary>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCanLoadedMaterialMdbResults(DateTime planTime);
        /// <summary>
        /// 获取所有开料锯的当前任务
        /// </summary>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCuttingsCurrTasks(DateTime planTime);

        /// <summary>
        /// 获取当前设备的就绪任务
        /// </summary>
        /// <param name="planDate">计划时间</param>
        /// <param name="deviceName">设备名</param>
        /// <returns></returns>
        [OperationContract]
        SpiltMDBResult GetCurrDeviceCuttingNextTasks(DateTime planDate, string deviceName);

        /// <summary>
        /// 获取开料锯的所有就绪任务
        /// </summary>
        /// <param name="planDate">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCurrCuttingNextTasks(DateTime planDate);

        /// <summary>
        /// 获取正在转换saw文件的任务
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetConvertingTasks();

        /// <summary>
        /// 获取待转换saw文件的任务
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetNeedToConvertTasks();
        /// <summary>
        /// 获取当前开料锯的当前任务
        /// </summary>
        /// <param name="deviceName">当前设备名</param>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetCurrCuttingTasks(string deviceName, DateTime planTime);

        /// <summary>
        /// 插入拆分结果
        /// </summary>
        /// <param name="spiltMdbResults"></param>
        /// <returns></returns>
        [OperationContract]
        int BulkInsertSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults);

        /// <summary>
        /// 插入拆分结果
        /// </summary>
        /// <param name="spiltMdbResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertSpiltMDBResult(SpiltMDBResult spiltMdbResult);

        [OperationContract]
        bool BulkUpdateSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults);

        bool BulkDeleteSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults);

        [OperationContract]
        bool UpdateSpiltMDBResult(SpiltMDBResult spiltMdbResult);

        /// <summary>
        /// 更新mdb路径
        /// </summary>
        /// <param name="spiltMdbResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateSpiltMDBFullPath(SpiltMDBResult spiltMdbResult);

        /// <summary>
        /// MDB获取后，更新状态
        /// </summary>
        /// <param name="spiltMDBResult"></param>
        /// <returns></returns>
        //[OperationContract]
        //bool UpdateSpiltMDBResult(SpiltMDBResult spiltMDBResult);

        /// <summary>
        /// MDB获取后，更新任务状态
        /// </summary>
        /// <param name="spiltMDBResults"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateFinishedStatus(List<SpiltMDBResult> spiltMDBResults);

        /// <summary>
        /// 更新mdb的状态
        /// </summary>
        /// <param name="spiltMdbResults"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateMdbStatus(List<SpiltMDBResult> spiltMdbResults);

        /// <summary>
        /// 更新任务状态以及mdb状态
        /// </summary>
        /// <param name="spiltMdbResults"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateTaskAndMdbStatus(List<SpiltMDBResult> spiltMdbResults);

        /// <summary>
        /// 获取堆垛上板件的开料信息（MDB）
        /// </summary>
        /// <param name="itemName">堆垛号</param>
        /// <returns></returns>
        [OperationContract]
        DataSet GetMDBDatas(string itemName);

        /// <summary>
        /// 获取所有开料锯信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DeviceInfos> GetCuttingDeviceInfos();

        /// <summary>
        /// 获取指定工段的设备信息
        /// </summary>
        /// <param name="processName">工段名称</param>
        /// <returns></returns>
        [OperationContract]
        List<DeviceInfos> GetDeviceInfosByProcessName(string processName);

        /// <summary>
        /// 插入设备信息
        /// </summary>
        /// <param name="deviceInfos">设备信息</param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertDeviceInfos(List<DeviceInfos> deviceInfos);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="deviceInfos">设备信息</param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateDeviceInfos(List<DeviceInfos> deviceInfos);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <param name="planDate">计划时间</param>
        /// <returns></returns>
        [OperationContract]
        List<AllTask> GetAllTasks(DateTime planDate);

        [OperationContract]
        List<AllTask> GetAllTasks_Test();

        /// <summary>
        /// 获取堆垛开料图的信息
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="itemName"></param>
        /// <param name="planDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingPattern> GetCuttingPatterns(string batchName, string itemName, DateTime planDate);

        /// <summary>
        /// 获取堆垛开料图的信息
        /// </summary>
        /// <param name="planDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingPattern> GetCuttingPatternsByPlanDate(DateTime planDate);

        /// <summary>
        /// 插入堆垛开料图信息
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertCuttingPatterns(List<CuttingPattern> cuttingPatterns);

        /// <summary>
        /// 更新开料图总共开出的板件数
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateCuttingPatternsPartCount(List<CuttingPattern> cuttingPatterns);

        /// <summary>
        /// 提交任务异常
        /// </summary>
        /// <param name="taskName">任务名</param>
        /// <returns></returns>
        [OperationContract]
        bool CommitTaskError(string taskName);
        /// <summary>
        /// 获取指定设备前N个批次的任务
        /// </summary>
        /// <param name="topNum">前N个批次</param>
        /// <param name="deviceName">设备编号</param>
        /// <returns></returns>
        [OperationContract]
        List<SpiltMDBResult> GetDeviceTopUnDownLoad(int topNum, string deviceName);

        /// <summary>
        /// 插入扫码板件
        /// </summary>
        /// <param name="cuttingPartScanLogs"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertCuttingPartScanLog(List<CuttingPartScanLog> cuttingPartScanLogs);

        /// <summary>
        /// 获取板件信息
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PartInfo> GetPartInfosByPartId(string partId);
        [OperationContract]
        void SyncCuttingFeedBackData(string deviceName);
        [OperationContract]
        List<CuttingFeedBack> GetCuttingFeedBacksByPartId(string partId);
        [OperationContract]
        List<CuttingFeedBack> GetCuttingFeedBacksByBatchName(string batchName);


    }
}
