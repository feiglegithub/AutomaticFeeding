using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.Common.Dependency;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Service;

namespace NJIS.FPZWS.LineControl.Cutting.Contract
{
    [ServiceContract]
    public partial interface ILineControlCuttingContract:IWcfServiceContract
    {
        [OperationContract]
        List<CutPartInfoCollector> GetCutPartInfoCollectors(string partId);
        [OperationContract]
        List<CuttingTaskLog> GetCuttingTaskLogs(DateTime planDate);
        [OperationContract]
        List<CuttingTaskDetail> GetTaskDetails(string deviceName, DateTime planDate);

        /// <summary>
        /// 获取批次的任务
        /// </summary>
        /// <param name="partId">板件编号</param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingTaskDetail> GetBatchTaskDetailsByPartId(string partId);

        [OperationContract]
        List<CuttingTaskDetail> GetDeviceCuttingTaskDetail(string deviceName,string itemName, DateTime planDate);
        [OperationContract]
        int BulkInsertTaskDetails(List<CuttingTaskDetail> cuttingTaskDetails);
        [OperationContract]
        List<CutPartInfoCollector> BulkInsertCutPartInfoCollectors(List<CutPartInfoCollector> cutPartInfoCollectors);
        [OperationContract]
        int BulkInsertCuttingManualLabelPrinters(List<CuttingManualLabelPrinter> cuttingManualLabelPrinters);
        /// <summary>
        /// 获取等待上料的堆垛列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CuttingStackProductionList> GetUnLoadLists();

        /// <summary>
        /// 获取正在上料的堆垛列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CuttingStackProductionList> GetLoadingLists();

        /// <summary>
        /// 更新上料的堆垛状态
        /// </summary>
        /// <param name="dataList">Item1为堆垛号，Item2为更新状态</param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateCuttingStackProductionList(List<Tuple<string,LoadMaterialStatus>> dataList);

        /// <summary>
        /// 获取开料的缓存架
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ChainBuffer> GetCuttingChainBuffers();

        /// <summary>
        /// 获取重入的板件信息
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [OperationContract]
        CuttingPartReenter GetCuttingPartReenters(string partId);

        /// <summary>
        /// 插入板材信息
        /// </summary>
        /// <param name="cuttingPartReenters"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkInsertCuttingPartReenters(List<CuttingPartReenter> cuttingPartReenters);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="cuttingPartReenters"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdateCuttingPartReenters(List<CuttingPartReenter> cuttingPartReenters);

        /// <summary>
        /// 获取备料数据
        /// </summary>
        /// <param name="planDate"></param>
        /// <param name="isStock"></param>
        /// <returns></returns>
        [OperationContract]
        List<WMSCuttingStackList> GetWmsCuttingStackLists(DateTime planDate, bool isStock);

        /// <summary>
        /// 更新wmsStatus
        /// </summary>
        /// <param name="wmsCuttingStackLists"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateWMSCuttingStackLists(List<WMSCuttingStackList> wmsCuttingStackLists);

        [OperationContract]
        int BulkInsertWMSStacktFeedBack(List<WMSStacktFeedBack> wmsStacktFeedBacks);

        /// <summary>
        /// 获取指定抽检板件列表
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingCheckPart> GetCuttingCheckPartsByBatchName(string batchName);

        /// <summary>
        /// 插入抽检板件
        /// </summary>
        /// <param name="checkParts"></param>
        /// <returns></returns>
        [OperationContract]
        int BulkInsertCuttingCheckParts(List<CuttingCheckPart> checkParts);

        /// <summary>
        /// 删除抽检板件
        /// </summary>
        /// <param name="checkParts"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkDeleteCuttingCheckParts(List<CuttingCheckPart> checkParts);

        /// <summary>
        /// 更新抽检板件
        /// </summary>
        /// <param name="checkParts"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdatedCuttingCheckParts(List<CuttingCheckPart> checkParts);

        /// <summary>
        /// 获取抽检规则
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CuttingCheckRule> GetCuttingCheckRules();

        /// <summary>
        /// 获取指定生效状态的抽检规则
        /// </summary>
        /// <param name="isEnable">是否生效</param>
        /// <returns></returns>
        [OperationContract]
        List<CuttingCheckRule> GetCuttingCheckRulesByEnable(bool isEnable);

        /// <summary>
        /// 插入抽检规则
        /// </summary>
        /// <param name="checkRules"></param>
        /// <returns></returns>
        [OperationContract]
        int BulkInsertCuttingCheckRules(List<CuttingCheckRule> checkRules);

        /// <summary>
        /// 删除抽检规则
        /// </summary>
        /// <param name="checkRules"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkDeleteCuttingCheckRules(List<CuttingCheckRule> checkRules);

        /// <summary>
        /// 更新抽检规则
        /// </summary>
        /// <param name="checkRules"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkUpdatedCuttingCheckRules(List<CuttingCheckRule> checkRules);

        /// <summary>
        /// 保存抽检规则
        /// </summary>
        /// <param name="insertCheckRules"></param>
        /// <param name="deleteCheckRules"></param>
        /// <param name="updateCheckRules"></param>
        /// <returns></returns>
        [OperationContract]
        bool BulkSaveCuttingCheckRules(List<CuttingCheckRule> insertCheckRules, List<CuttingCheckRule> deleteCheckRules,
            List<CuttingCheckRule> updateCheckRules);

        ///// <summary>
        ///// 插入备料信息
        ///// </summary>
        ///// <param name="wmsCuttingStackLists"></param>
        ///// <returns></returns>
        //[OperationContract]
        //bool BulkInsertWMSCuttingStackList(List<WMSCuttingStackList> wmsCuttingStackLists);


    }
}
