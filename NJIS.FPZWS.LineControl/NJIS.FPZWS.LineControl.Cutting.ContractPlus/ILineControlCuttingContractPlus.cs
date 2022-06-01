using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.Wcf.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace NJIS.FPZWS.LineControl.Cutting.ContractPlus
{
    [ServiceContract]
    public partial interface ILineControlCuttingContractPlus : IWcfServiceContract
    {

        /// <summary>
        /// 获取指定生效状态的抽检规则
        /// </summary>
        /// <param name="isEnable">是否生效</param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingCheckRule> GetCuttingCheckRulesByEnable(bool isEnable);

        /// <summary>
        /// 获取指定抽检板件列表
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingCheckPart> GetCuttingCheckPartsByBatchName(string batchName);

        /// <summary>
        /// 获取所有开料锯信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DeviceInfos> GetCuttingDeviceInfos();

        /// <summary>
        /// 获取锯切图列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Pattern> GetPatternsByBatchName(string batchName);

        /// <summary>
        /// 获取锯切图列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Pattern> GetPatternsByDevice(string deviceName,PatternStatus status);

        /// <summary>
        /// 获取正在开的锯切图
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Pattern> GetCuttingPatternsByDevice(string deviceName);

        /// <summary>
        /// 获取下一个锯切图
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Pattern> GetNextPatternsByDevice(string deviceName);

        /// <summary>
        /// 获取锯切图列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Pattern> GetPatternsByPlanDate(DateTime planDate);

        [OperationContract]
        List<Pattern> GetPatternsByUpdatedTime(DateTime minUpdatedTime);

        List<Pattern> GetUpdatedPatterns(DateTime minPlanDate);

        [OperationContract]
        bool BulkInsertPatterns(List<Pattern> patterns);

        [OperationContract]
        bool BulkUpdatePatterns(List<Pattern> patterns);

        [OperationContract]
        bool BulkUpdateNewPatterns(List<Pattern> patterns);

        [OperationContract]
        bool BulkUpdatePatternStatus(List<Pattern> patterns);

        [OperationContract]
        bool BulkDeletePatterns(List<Pattern> patterns);

        /// <summary>
        /// 获取锯切图（工件）明细
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        [OperationContract]
        List<PatternDetail> GetPatternDetailsByBatchName(string batchName);

        /// <summary>
        /// 获取锯切图（工件）明细
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="patternId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PatternDetail> GetPatternDetailsByPattern(string batchName,int patternId);

        /// <summary>
        /// 获取锯切图（工件）明细
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PatternDetail> GetPatternDetailsByPartId(string partId);

        [OperationContract]
        bool BulkInsertPatternDetails(List<PatternDetail> patternDetails);

        [OperationContract]
        bool BulkUpdatePatternDetails(List<PatternDetail> patternDetails);

        [OperationContract]
        bool BulkUpdateNewPatternDetails(List<PatternDetail> patternDetails);

        [OperationContract]
        bool BulkDeletePatternDetails(List<PatternDetail> patternDetails);

        /// <summary>
        /// 获取批次的mdb
        /// </summary>
        /// <param name="batchId">批次Id</param>
        /// <returns></returns>
        [OperationContract]
        DataSet GetMdbDataByBatchId(string batchId);

        [OperationContract]
        List<Stack> GetStacksByFirstBatchName(string firstBatchName);

        [OperationContract]
        List<Stack> GetStacksBySecondBatchName(string secondBatchName);

        [OperationContract]
        List<Stack> GetStacksByPlanDate(DateTime planDate);

        [OperationContract]
        List<Stack> GetStacksByUpdatedTime(DateTime minUpdatedTime);

        [OperationContract]
        List<Stack> GetStacksByStatus(StackStatus status);

        [OperationContract]
        List<Stack> GetStacksByStackName(string stackName);

        [OperationContract]
        bool BulkInsertStacks(List<Stack> stacks);

        [OperationContract]
        bool BulkDeleteStacks(List<Stack> stacks);

        [OperationContract]
        bool BulkUpdatedStacks(List<Stack> stacks);

        [OperationContract]
        List<Stack> GetStacksByDevice(string deviceName, StackStatus stackStatus);

        [OperationContract]
        List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate);

        [OperationContract]
        List<StackDetail> GetStackDetailsByStackName(string stackName);

        [OperationContract]
        bool BulkInsertStackDetails(List<StackDetail> stackDetails);

        [OperationContract]
        bool BulkDeleteStackDetails(List<StackDetail> stackDetails);

        [OperationContract]
        bool BulkUpdatedStackDetails(List<StackDetail> stackDetails);

        [OperationContract]
        List<BatchGroup> GetBatchGroupsByPlanDate(DateTime planDate);

        [OperationContract]
        List<BatchGroup> GetAllBatchGroups();

        [OperationContract]
        List<BatchGroup> GetUnFinishedBatchGroups();

        [OperationContract]
        bool BulkInsertBatchGroups(List<BatchGroup> batchGroups);

        [OperationContract]
        bool BulkUpdatedBatchGroups(List<BatchGroup> batchGroups);

        [OperationContract]
        bool BulkUpdatedBatchGroupsLoadTime(List<BatchGroup> batchGroups);

        [OperationContract]
        bool BulkInsertCuttingStackProductionList(List<CuttingStackProductionList> cuttingStackProductionLists);

        [OperationContract]
        List<CuttingStackProductionList> GetCuttingStackProductionLists();

        [OperationContract]
        List<CuttingStackProductionList> GetCuttingStackProductionListsByStackName(string stackName);

        [OperationContract]
        List<CuttingStackProductionList> GetCuttingStackProductionListsByStatus(RequestLoadingStatus status);

        [OperationContract]
        List<WMSStacktFeedBack> GetWmsStacktFeedBacksByStackName(string stackName);

        [OperationContract]
        List<PartFeedBack> GetPartFeedBacksByStatus(PartFeedBackStatus status);

        [OperationContract]
        List<PartFeedBack> GetPartFeedBacksByBatchName(string batchName);

        /// <summary>
        /// 插入扫码板件
        /// </summary>
        /// <param name="cuttingPartScanLogs"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertCuttingPartScanLog(List<CuttingPartScanLog> cuttingPartScanLogs);

        /// <summary>
        /// 插入板材信息
        /// </summary>
        /// <param name="cuttingPartReenters"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertCuttingPartReenters(List<CuttingPartReenter> cuttingPartReenters);

        [OperationContract]
        List<PartFeedBack> GetPartFeedBacksByPartId(string partId);

        [OperationContract]
        List<PartFeedBack> GetPartFeedBacksTop1000(DateTime minCreatedTime);

        [OperationContract]
        bool BulkInsertPartFeedBacks(List<PartFeedBack> partFeedBacks);

        [OperationContract]
        bool BulkUpdatePartFeedBacks(List<PartFeedBack> partFeedBacks);

        [OperationContract]
        bool BulkInsertWmsCuttingStackList(List<WMSCuttingStackList> wmsCuttingStackLists);

        [OperationContract]
        List<MdbParse> GetMdbParses();
        [OperationContract]
        bool BulkInsertMdbParses(List<MdbParse> mdbParses);
        [OperationContract]
        bool BulkUpdatedMdbParses(List<MdbParse> mdbParses);

        [OperationContract]
        void SyncPartFeedBack(string deviceName);

        /// <summary>
        /// 获取开料的缓存架
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ChainBuffer> GetCuttingChainBuffers();

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

        [OperationContract]
        List<PatternFeedBack> GetPatternFeedBacksByMdbName(string mdbName);

        [OperationContract]
        List<PatternDetailLog> GetPatternDetailLogsByBatchName(string batchName);

        [OperationContract]
        List<BatchGroupLog> GetBatchGroupLogsByPlanDate(DateTime planDate);

        [OperationContract]
        List<StackLog> GetStackLogsByPlanDate(DateTime planDate);

        [OperationContract]
        List<StackDetailLog> GetStackDetailLogsByPlanDate(DateTime planDate);

        [OperationContract]
        List<PatternLog> GetPatternLogsByPlanDate(DateTime planDate);

        [OperationContract]
        List<Stack> GetUnfinishedStacks();
        [OperationContract]
        List<Pattern> GetUnfinishedPatterns();

        [OperationContract]
        string CommitErrorTask(string mdbName);

        [OperationContract]
        List<Mdb_Parts_Udi> GetMdbPartsUdisByBatchName(string batchName);
    }
}
