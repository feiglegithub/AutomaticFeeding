using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository;
using NJIS.FPZWS.Wcf.Service;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public partial class LineControlCuttingService : WcfServer<LineControlCuttingService>, ILineControlCuttingContract
    {
        
        public List<NotCuttingData> GetNotCuttingData(string batchName,string sawFileName)
        {
            var notCuttingDataRepository = new NotCuttingDataRepository();
            string sql = $"SELECT a.PlanDate,a.BatchName,a.StackName,a.SawFileName,a.StackIndex,a.BoardCount,b.Status FROM " +
                $"[NJIS.FPZWS.LineControl.Cutting].[dbo].[CuttingSawFileRelation] AS a LEFT JOIN " +
                $"[NJIS.FPZWS.LineControl.CuttingPlus].[dbo].[CuttingSawFileRelationPlus] AS b ON " +
                $"a.SawFileName = b.SawFileName WHERE a.BatchName= '{batchName}' AND a.SawType= 0 AND " +
                $"a.SawFileName NOT IN({sawFileName})";
            return notCuttingDataRepository.FindAllBySQL(sql).ToList();
        }

        public List<CutPartInfoCollector> GetCutPartInfoCollectors(string partId)
        {
            CutPartInfoCollectorRepository repository= new CutPartInfoCollectorRepository();

            string sql = $@"EXEC GetCutPartInfoCollector @PartId='{partId}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByPlanDate(DateTime dateTime)
        {
            CuttingSawFileRelationRepository repository = new CuttingSawFileRelationRepository();

            string sql = $@"EXEC CuttingSawFileQueue '{dateTime}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingStackList> GetCuttingStackListByPlanDate(DateTime dateTime)
        {
            CuttingStackListRepository repository = new CuttingStackListRepository();

            string sql = $@"EXEC CuttingStackListTotal '{dateTime}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingTaskLog> GetCuttingTaskLogs(DateTime planDate)
        {
            CuttingTaskLogRepository repository = new CuttingTaskLogRepository();
            string sql = $"EXEC GetCuttingTaskLogs @PlanDate='{planDate.Date:yyyy-MM-dd}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingTaskDetail> GetTaskDetails(string deviceName, DateTime planDate)
        {
            CuttingTaskDetailRepository repository = new CuttingTaskDetailRepository();
            string sql = $"EXEC GeTaskDetails @PlanDate='{planDate.Date:yyyy-MM-dd}',@DeviceName='{deviceName}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingTaskDetail> GetBatchTaskDetailsByPartId(string partId)
        {
            CuttingTaskDetailRepository repository = new CuttingTaskDetailRepository();
            string sql = $"EXEC GetBatchTaskDetailByPartId @PartId='{partId}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingTaskDetail> GetDeviceCuttingTaskDetail(string deviceName, string itemName, DateTime planDate)
        {
            CuttingTaskDetailRepository repository = new CuttingTaskDetailRepository();
            string sql = $"EXEC [dbo].[GetDeviceCuttingTaskDetail] @PlanDate='{planDate.Date:yyyy-MM-dd}',@DeviceName='{deviceName}',@ItemName='{itemName}'";
            return repository.QueryList(sql, null).ToList();
        }

        public int BulkInsertTaskDetails(List<CuttingTaskDetail> cuttingTaskDetails)
        {
            if (cuttingTaskDetails == null || cuttingTaskDetails.Count == 0) return 0;
            CuttingTaskDetailRepository repository = new CuttingTaskDetailRepository();
            CuttingTaskDetail ctd;
            var conn = repository.Connection;
            string insertSql = string.Format(
                "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}],[{10}],[{11}],[{12}],[{13}],[{14}],[{15}],[{16}],[{17}],[{18}]) " +
                "VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11},@{12},@{13},@{14},@{15},@{16},@{17},@{18})",
                nameof(CuttingTaskDetail), nameof(ctd.Length), nameof(ctd.Width), nameof(ctd.BatchName),
                nameof(ctd.CreatedTime), nameof(ctd.DeviceName), nameof(ctd.ItemName), nameof(ctd.JOB_INDEX),
                nameof(ctd.PART_INDEX), nameof(ctd.PartFinishedStatus), nameof(ctd.PartId), nameof(ctd.PlanDate),
                nameof(ctd.TaskDistributeId), nameof(ctd.TaskEnable), nameof(ctd.UpdatedTime),nameof(ctd.OldPTN_INDEX),nameof(ctd.NewPTN_INDEX),nameof(ctd.IsOffPart),nameof(ctd.Color));
            //string sql =
            //    $"INSERT INTO [dbo].[CuttingTaskDetail] (TaskDistributeId,BatchName,DeviceName,ItemName,JOB_INDEX,PART_INDEX,PlanDate,PartId,PartFinishedStatus,TaskEnable,CreatedTime,UpdatedTime) VALUES (@TaskDistributeId,@BatchName,@DeviceName,@ItemName,@JOB_INDEX,@PART_INDEX,@PlanDate,@PartId,@PartFinishedStatus,@TaskEnable,@CreatedTime,@UpdatedTime)";
            string deleteSql =
                string.Format(
                    $"DELETE dbo.{nameof(CuttingTaskDetail)} WHERE [{nameof(ctd.PartId)}] IN ('{string.Join("','", cuttingTaskDetails.ConvertAll(item => item.PartId))}') ");
            conn.Execute(deleteSql);
            return conn.Execute(insertSql, cuttingTaskDetails);
        }

        public List<CutPartInfoCollector> BulkInsertCutPartInfoCollectors(List<CutPartInfoCollector> cutPartInfoCollectors)
        {
            throw new NotImplementedException();
        }

        public int BulkInsertCuttingManualLabelPrinters(List<CuttingManualLabelPrinter> cuttingManualLabelPrinters)
        {
            if (cuttingManualLabelPrinters == null || cuttingManualLabelPrinters.Count == 0) return 0;
            CuttingManualLabelPrinterRepository repository = new CuttingManualLabelPrinterRepository();
            CuttingManualLabelPrinter cmlp;
            var conn = repository.Connection;
            string sql =
                $"INSERT INTO [dbo].[{nameof(CuttingManualLabelPrinter)}] WITH(TABLOCK) ([{nameof(cmlp.WorkpieceID)}],[{nameof(cmlp.Status)}]) VALUES (@{nameof(cmlp.WorkpieceID)},@{nameof(cmlp.Status)})";
            //return conn.Execute(sql, cuttingManualLabelPrinters);
            if (conn.State == ConnectionState.Closed) conn.Open();
            var tran = conn.BeginTransaction();

            try
            {
                int ret = conn.Execute(sql, cuttingManualLabelPrinters, tran, 7200, CommandType.Text);
                tran.Commit();
                return ret;
            }
            catch (Exception e)
            {
                tran.Rollback();
                //Console.WriteLine(e);
                throw;
            }
            finally
            {
                conn.Close();
            }

            //return ;
            //return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingStackProductionList> GetUnLoadLists()
        {
            CuttingStackProductionListRepository repository = new CuttingStackProductionListRepository();
            string sql = $"SELECT * FROM [dbo].[CuttingStackProductionList] cspl WHERE cspl.[Status]=10";
            var ret = repository.QueryList(sql,null).ToList();
            return ret;
        }

        public List<CuttingStackProductionList> GetLoadingLists()
        {
            CuttingStackProductionListRepository repository = new CuttingStackProductionListRepository();
            string sql = $"SELECT * FROM [dbo].[CuttingStackProductionList] cspl WHERE cspl.[Status]=20";
            var ret = repository.QueryList(sql, null).ToList();
            return ret;
        }

        public bool BulkUpdateCuttingStackProductionList(List<Tuple<string, LoadMaterialStatus>> dataList)
        {
            var data = dataList.ConvertAll(item => new Tuple<string, int>(item.Item1, Convert.ToInt32(item.Item2)));
            CuttingStackProductionListRepository repository = new CuttingStackProductionListRepository();
            var conn = repository.Connection;
            string sql = $"UPDATE  [dbo].[CuttingStackProductionList] SET [Status]=@Item2 WHERE [StackName]=@Item1";
            return conn.Execute(sql, data) >0;
            //var ret = repository.QueryList(sql, null).ToList();
            //return ret;
        }

        public List<ChainBuffer> GetCuttingChainBuffers()
        {
            ChainBufferRepository repository = new ChainBufferRepository();
            return repository.QueryList($"SELECT * FROM [dbo].[ChainBuffer]", null).ToList();
        }

        public CuttingPartReenter GetCuttingPartReenters(string partId)
        {
            CuttingPartReenterRepository rep = new CuttingPartReenterRepository();
            return rep.Find(item => item.PartId == partId);
        }

        public bool BulkInsertCuttingPartReenters(List<CuttingPartReenter> cuttingPartReenters)
        {
            if (cuttingPartReenters == null || cuttingPartReenters.Count == 0) return true;
            CuttingPartReenterRepository rep = new CuttingPartReenterRepository();
            CuttingPartReenter cpr;
            string insertSql = string.Format(
                "INSERT INTO [dbo].[{0}] WITH(TABLOCK) ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}]) VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8})",
                nameof(CuttingPartReenter), nameof(cpr.PartId), nameof(cpr.BatchName), nameof(cpr.Length),
                nameof(cpr.ProductionLine), nameof(cpr.Remark), nameof(cpr.ReenterType), nameof(cpr.TaskDistributeId),
                nameof(cpr.Width));
            return rep.ExecuteSql(insertSql, cuttingPartReenters);
        }

        public bool BulkUpdateCuttingPartReenters(List<CuttingPartReenter> cuttingPartReenters)
        {
            if (cuttingPartReenters == null || cuttingPartReenters.Count == 0) return true;
            CuttingPartReenterRepository rep = new CuttingPartReenterRepository();
            CuttingPartReenter cpr;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                nameof(CuttingPartReenter), nameof(cpr.OperationResult),nameof(cpr.Remark), nameof(cpr.LineId));
            return rep.ExecuteSql(updateSql, cuttingPartReenters);
        }

        public List<WMSCuttingStackList> GetWmsCuttingStackLists(DateTime planDate, bool isStock)
        {
            WMSCuttingStackListRepository repository = new WMSCuttingStackListRepository();
            WMSCuttingStackList wms;

            if (isStock)
            {
                string selectSql = string.Format("SELECT * FROM [dbo].[{0}] WHERE [{1}]='{2}' AND [{3}]={4} AND [{5}] NOT IN (SELECT [{5}] FROM [dbo].[{6}] GROUP BY [{5}])",
                    nameof(WMSCuttingStackList), nameof(wms.PlanDate), planDate.Date.ToString("yyyy-MM-dd"),
                    nameof(wms.WMSStatus), 1,nameof(wms.StackName),nameof(WMSStacktFeedBack));
                return repository.QueryList(selectSql, null).ToList();
            }

            return repository.FindAll(item => item.PlanDate == planDate.Date && item.WMSStatus == 0).ToList();
            //return repository.QueryList(selectSql, null).ToList();
        }

        public bool UpdateWMSCuttingStackLists(List<WMSCuttingStackList> wmsCuttingStackLists)
        {
            if (wmsCuttingStackLists == null || wmsCuttingStackLists.Count == 0) return true;
            WMSCuttingStackListRepository repository = new WMSCuttingStackListRepository();
            WMSCuttingStackList wms;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1} WHERE [{2}]=@{2}",
                nameof(WMSCuttingStackList), nameof(wms.WMSStatus), nameof(wms.StackName));
            return repository.ExecuteSql(updateSql, wmsCuttingStackLists);
        }

        public int BulkInsertWMSStacktFeedBack(List<WMSStacktFeedBack> wmsStacktFeedBacks)
        {
            if (wmsStacktFeedBacks == null || wmsStacktFeedBacks.Count == 0) return 0;
            WMSStacktFeedBackRepository repository = new WMSStacktFeedBackRepository();
            WMSStacktFeedBack feedBack;
            string insertSql = string.Format("INSERT INTO [dbo].[{0}] ([{1}],[{2}]) VALUES(@{1},@{2})",
                nameof(WMSStacktFeedBack), nameof(feedBack.IsSuccess), nameof(feedBack.StackName));
            return repository.Connection.Execute(insertSql, wmsStacktFeedBacks);
        }

        public List<CuttingCheckPart> GetCuttingCheckPartsByBatchName(string batchName)
        {
            var rep = new CuttingCheckPartRepository();
            return rep.FindAll(item => item.BatchName == batchName).ToList();
        }

        public int BulkInsertCuttingCheckParts(List<CuttingCheckPart> checkParts)
        {

            if (checkParts == null || checkParts.Count == 0) return 0;
            var rep = new CuttingCheckPartRepository();
            CuttingCheckPart ccp;
            string insertSql = string.Format(
                "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}]) VALUES(@{1},@{2},@{3},@{4},@{5},@{6})"
                , nameof(CuttingCheckPart), nameof(ccp.BatchName), nameof(ccp.ItemName), nameof(ccp.PartId),
                nameof(ccp.TaskDistributeId), nameof(ccp.IsEnable), nameof(ccp.CheckStatus));
            return rep.Connection.Execute(insertSql, checkParts);
        }

        public bool BulkDeleteCuttingCheckParts(List<CuttingCheckPart> checkParts)
        {
            if (checkParts == null || checkParts.Count == 0) return true;
            CuttingCheckPart ccp;
            var rep = new CuttingCheckPartRepository();
            string deleteSql = string.Format("DELETE FROM [dbo].[{0}] WHERE [{1}]=@{1}", nameof(CuttingCheckPart),
                nameof(ccp.LineId));
            return rep.Connection.Execute(deleteSql, checkParts) == checkParts.Count;
        }

        public bool BulkUpdatedCuttingCheckParts(List<CuttingCheckPart> checkParts)
        {
            if (checkParts == null || checkParts.Count == 0) return true;
            CuttingCheckPart ccp;
            var rep = new CuttingCheckPartRepository();
            string updateSql = string.Format(
                "UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5},[{6}]=@{6} WHERE [{7}]=@{7}"
                , nameof(CuttingCheckPart), nameof(ccp.BatchName), nameof(ccp.ItemName), nameof(ccp.PartId),
                nameof(ccp.TaskDistributeId), nameof(ccp.IsEnable), nameof(ccp.CheckStatus), nameof(ccp.LineId));
            return rep.ExecuteSql(updateSql, checkParts);
        }

        public List<CuttingCheckRule> GetCuttingCheckRules()
        {
            var rep = new CuttingCheckRuleRepository();
            return rep.FindAll().ToList();
        }

        public List<CuttingCheckRule> GetCuttingCheckRulesByEnable(bool isEnable)
        {
            var rep = new CuttingCheckRuleRepository();
            return rep.FindAll(item=>item.IsEnable==isEnable).ToList();
        }

        public int BulkInsertCuttingCheckRules(List<CuttingCheckRule> checkRules)
        {
            if (checkRules == null || checkRules.Count == 0) return 0;
            var rep = new CuttingCheckRuleRepository();
            CuttingCheckRule ccr;
            string insertSql = string.Format(
                "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}]) VALUES(@{1},@{2},@{3},@{4},@{5},@{6})"
                , nameof(CuttingCheckRule), nameof(ccr.IsEnable), nameof(ccr.Args), nameof(ccr.CheckObject),
                nameof(ccr.CheckOperatorArgs), nameof(ccr.CheckWay), nameof(ccr.Message
                ));
            return rep.Connection.Execute(insertSql, checkRules);
        }

        public bool BulkDeleteCuttingCheckRules(List<CuttingCheckRule> checkRules)
        {
            if (checkRules == null || checkRules.Count == 0) return true;
            var rep = new CuttingCheckRuleRepository();
            CuttingCheckRule ccr;
            string deleteSql = string.Format("DELETE FROM [dbo].[{0}] WHERE [{1}]=@{1}", nameof(CuttingCheckRule),
                nameof(ccr.LineId));
            return rep.ExecuteSql(deleteSql, checkRules);
        }

        public bool BulkUpdatedCuttingCheckRules(List<CuttingCheckRule> checkRules)
        {
            if (checkRules == null || checkRules.Count == 0) return true;
            var rep = new CuttingCheckRuleRepository();
            CuttingCheckRule ccr;
            string updateSql = string.Format(
                "UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5},[{6}]=@{6} WHERE [{7}]=@{7}"
                , nameof(CuttingCheckRule), nameof(ccr.IsEnable), nameof(ccr.Args), nameof(ccr.CheckObject),
                nameof(ccr.CheckOperatorArgs), nameof(ccr.CheckWay), nameof(ccr.Message), nameof(ccr.LineId));
            return rep.ExecuteSql(updateSql, checkRules);
        }

        //public bool BulkInsertWMSCuttingStackList(List<WMSCuttingStackList> wmsCuttingStackLists)
        //{

        //}
    }
}
