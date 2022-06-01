using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Contract
{
    public partial interface ILineControlCuttingContract
    {
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
        bool BulkDeletePatterns(List<Pattern> patterns);

        /// <summary>
        /// 获取锯切图（工件）明细
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        [OperationContract]
        List<PatternDetail> GetPatternDetailsByBatchName(string batchName);

        [OperationContract]
        bool BulkInsertPatternDetails(List<PatternDetail> patternDetails);

        [OperationContract]
        bool BulkUpdatePatternDetails(List<PatternDetail> patternDetails);

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



    }
}
