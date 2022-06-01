using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.RepositoryPlus;
using NJIS.FPZWS.Wcf.Service;

namespace NJIS.FPZWS.LineControl.Cutting.ServicePlus
{
    public partial class LineControlCuttingServicePlus : WcfServer<LineControlCuttingServicePlus>, ILineControlCuttingContractPlus
    {
        /// <summary>
        /// 根据批次号获取pattern
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public List<PartFeedBack> GetPattern(string batchName)
        {
            var partFeedBackRepository = new PartFeedBackRepository();
            string sql = "SELECT PATTERN FROM [NJIS.FPZWS.LineControl.CuttingPlus].[dbo].[PartFeedBack] " +
                $"WHERE BatchName='{batchName}' GROUP BY PATTERN";
            return partFeedBackRepository.FindAllBySQL(sql).ToList();
        }

        public void SyncPartFeedBack(string deviceName)
        {
            var rep = new CuttingPartScanLogRepository();
            string store = "GetPartFeedBack" + deviceName.Replace("0-240-07-", "");
            rep.ExecuteProcedure(store, null);
            
        }

        /// <summary>
        /// 获取用户表数据，只有一条数据
        /// </summary>
        /// <returns></returns>
        public List<users> GetUsers()
        {
            UsersRepository usersRepository = new UsersRepository();
            return usersRepository.FindAll().ToList();
        }

        public List<ChainBuffer> GetCuttingChainBuffers()
        {
            ChainBufferRepository repository = new ChainBufferRepository();
            return repository.QueryList($"SELECT * FROM [dbo].[ChainBuffer]", null).ToList();
        }
        public List<CuttingCheckRule> GetCuttingCheckRulesByEnable(bool isEnable)
        {
            var rep = new CuttingCheckRuleRepository();
            return rep.FindAll(item => item.IsEnable == isEnable).ToList();
        }

        public List<WMS_Task> GetWMSTaskByReqId(long reqId)
        {
            var rep = new WMSTaskRepository();
            return rep.FindAll(item => item.ReqId == reqId).ToList();
        }

        public List<WMS_Task> GetWMSTaskByPilerNo(int pilerNo)
        {
            var rep = new WMSTaskRepository();
            return rep.FindAll(item => item.PilerNo == pilerNo).ToList();
        }

        public List<WMS_Task> GetWMSTaskByReqId(int reqId)
        {
            var rep = new WMSTaskRepository();
            return rep.FindAll(item => item.ReqId == reqId).ToList();
        }

        public List<CuttingCheckPart> GetCuttingCheckPartsByBatchName(string batchName)
        {
            var rep = new CuttingCheckPartRepository();
            return rep.FindAll(item => item.BatchName == batchName).ToList();
        }
        public List<DeviceInfos> GetCuttingDeviceInfos()
        {
            DeviceInfosRepository repository = new DeviceInfosRepository();
            string sql = $"EXEC GetCuttingDeviceInfos";
            return repository.QueryList(sql, null).ToList();
        }

        public List<PartFeedBack> GetPartFeedBackByBatchName(string batchName)
        {
            PartFeedBackRepository repository = new PartFeedBackRepository();
            string sql = $"EXEC CuttingFeedBackPattern '{batchName}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<DeviceInfos> GetEnableCuttingDeviceInfos()
        {
            DeviceInfosRepository repository = new DeviceInfosRepository();
            string sql = $"SELECT * FROM DeviceInfos devInfo WHERE DeviceType = 'CuttingMachine' and State = 1";
            return repository.QueryList(sql, null).ToList();
        }

        public List<Pattern> GetPatternsByBatchName(string batchName)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        public List<Pattern> GetPatternsByDevice(string deviceName, PatternStatus status)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.DeviceName == deviceName && item.Status == Convert.ToInt32(status)).ToList();
        }

        public List<Pattern> GetCuttingPatternsByDevice(string deviceName)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.DeviceName == deviceName && item.Status <= Convert.ToInt32(PatternStatus.Cutting) && item.Status >= Convert.ToInt32(PatternStatus.Converted)).ToList();
        }

        public List<Pattern> GetNextPatternsByDevice(string deviceName)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.DeviceName == deviceName && item.Status <= Convert.ToInt32(PatternStatus.ConvertingSaw) && item.Status >= Convert.ToInt32(PatternStatus.Loaded)).ToList();
        }

        public List<Pattern> GetPatternsByPlanDate(DateTime planDate)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.PlanDate == planDate && item.IsEnable).ToList();
        }

        public List<Pattern> GetPatternsByUpdatedTime(DateTime minUpdatedTime)
        {
            var rep = new PatternRepository();
            return rep.FindAll(item => item.UpdatedTime >= minUpdatedTime).ToList();
        }

        public List<Pattern> GetUpdatedPatterns(DateTime minPlanDate)
        {
            var req = new PatternRepository();
            return req.FindAll(item => item.PlanDate >= minPlanDate && item.IsEnable).ToList();
        }

        public List<BatchNamePilerNoBind> GetBatchNamePilerNoBindByPilerNo(int pilerNo)
        {
            var req = new BatchNamePilerNoBindRepository();
            string sql = $"SELECT * FROM BatchNamePilerNoBind WHERE PilerNo = {pilerNo} ORDER BY CreateTime DESC";
            //return req.FindAll(item => item.PilerNo == pilerNo).ToList();
            return req.FindAllBySQL(sql).ToList<BatchNamePilerNoBind>();
        }

        public List<BatchNamePilerNoBind> GetBatchNamePilerNoBindByStackName(string stackName)
        {
            var req = new BatchNamePilerNoBindRepository();
            return req.FindAll(item => item.StackName == stackName).ToList();
        }

        public List<BatchNamePilerNoBind> GetBatchNamePilerNoBindByBatchName(string batchName)
        {
            var req = new BatchNamePilerNoBindRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        public bool InsertBatchNamePilerNoBind(BatchNamePilerNoBind batchNamePilerNoBind)
        {
            var req = new BatchNamePilerNoBindRepository();
            string insertSql = GetInsertSql<BatchNamePilerNoBind>(nameof(batchNamePilerNoBind.Id),nameof(batchNamePilerNoBind.CreateTime));
            return req.ExecuteSql(insertSql, batchNamePilerNoBind);
        }

        public bool BulkInsertPatterns(List<Pattern> patterns)
        {
            var req = new PatternRepository();
            Pattern p;
            string insertSql = GetInsertSql<Pattern>(nameof(p.LineId));
            return req.ExecuteSql(insertSql, patterns);
        }

        public bool BulkInsertBatchProductionDetails(List<BatchProductionDetails> listBatchProductionDetails)
        {
            var req = new BatchProductionDetailsRepository();
            BatchProductionDetails batchProductionDetails;
            string insertSql = GetInsertSql<BatchProductionDetails>(nameof(batchProductionDetails.Id));
            return req.ExecuteSql(insertSql, listBatchProductionDetails);
        }

        public bool BulkUpdatePatterns(List<Pattern> patterns)
        {
            var req = new PatternRepository();
            Pattern p;
            string updateSql = GetUpdateSql<Pattern>(new string[]{nameof(p.LineId)});
            return req.ExecuteSql(updateSql, patterns);
        }

        public bool BulkUpdateNewPatterns(List<Pattern> patterns)
        {
            if (patterns == null || patterns.Count == 0) return true;
            var req = new PatternRepository();
            var maxLineId = patterns.Max(item => item.LineId);
            var minLineId = patterns.Min(item => item.LineId);
            var dbs = req.FindAll(item => item.LineId >= minLineId && item.LineId <= maxLineId).ToList();

            var updates = dbs.Join(patterns, db => db.LineId, patternDetail => patternDetail.LineId,
                (db, pd) => new { Db = db, Pd = pd }).Where(item => item.Db.UpdatedTime < item.Pd.UpdatedTime).ToList().ConvertAll(item => item.Pd);
            if (updates.Count == 0) return true;
            
            Pattern p;
            string updateSql = GetUpdateSql<Pattern>(new string[] { nameof(p.LineId) });
            updateSql += $" AND [{nameof(p.UpdatedTime)}]<@{nameof(p.UpdatedTime)}";
            return req.ExecuteSql(updateSql, updates);
        }

        public bool BulkUpdatePatternStatus(List<Pattern> patterns)
        {
            var req = new PatternRepository();
            Pattern p;
            string updateSql = string.Format("UPDATE {0} SET [{1}]=@{1},[{2}]=GETDATE() WHERE [{3}]=@{3}",
                nameof(Pattern), nameof(p.Status), nameof(p.UpdatedTime), nameof(p.LineId));
            return req.ExecuteSql(updateSql, patterns);
        }

        public bool BulkDeletePatterns(List<Pattern> patterns)
        {
            if (patterns == null || patterns.Count == 0) return true;
            var req = new PatternRepository();
            Pattern p;
            string deleteSql = string.Format("DELETE [dbo].[{0}] WHERE [{1}]=@{1}", nameof(Pattern),
                nameof(p.LineId));
            return req.ExecuteSql(deleteSql, patterns);
        }

        public List<PatternDetail> GetPatternDetailsByBatchName(string batchName)
        {
            var req = new PatternDetailRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        public List<CuttingStackList> GetCuttingStackListGroupByBatchNameByPlanDate(DateTime planDate,
            CuttingStackListBatchType cuttingStackListBatchType)
        {
            string sql = $"SELECT BatchName,BatchProductIndex,BatchType FROM  CuttingStackList WHERE" +
                $" DateDiff(DAY,PlanDate,'{planDate.ToShortDateString()}') = 0 GROUP BY BatchName,BatchProductIndex," +
                $"BatchType HAVING BatchType = '{cuttingStackListBatchType.GetFinishStatusDescription().Item2}'";

            var req = new CuttingStackListRepository();
            return req.FindAllBySQL(sql).ToList();
        }

        public List<CuttingStackList> GetCuttingStackListByBatchName(string batchName)
        {
            var req = new CuttingStackListRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        /// <summary>
        /// 获取未要料的垛生产顺序最小的数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public List<CuttingStackList> GetUnRequestCuttingStackListByBatchNameAndMinStackProductIndex(String batchName,
            CuttingStackListBatchType cuttingStackListBatchType)
        {
            string stackName = "";

            List<BatchNamePilerNoBind> listBatchNamePilerNoBind =  GetBatchNamePilerNoBindByBatchName(batchName);
            if (listBatchNamePilerNoBind.Count > 0)
            {
                for (int i = 0; i < listBatchNamePilerNoBind.Count; i++)
                {
                    stackName += $"'{listBatchNamePilerNoBind[i].StackName}'";
                    if (i < listBatchNamePilerNoBind.Count - 1)
                    {
                        stackName += ",";
                    }
                }
            }else
            {
                stackName = "''";
            }

            string sql = $"SELECT * FROM  CuttingStackList WHERE BatchName = '{batchName}' AND StackName NOT IN({stackName}) " +
                $"AND BatchType = '{cuttingStackListBatchType.GetFinishStatusDescription().Item2}' AND StackProductIndex = " +
                $"(SELECT MIN(StackProductIndex) FROM  CuttingStackList WHERE BatchName = '{batchName}' AND StackName NOT IN" +
                $"({stackName}) AND BatchType = '{cuttingStackListBatchType.GetFinishStatusDescription().Item2}')";

            var req = new CuttingStackListRepository();
            return req.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 获取StackProductIndex最小的CuttingStackList，但不包含stackNames的数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public List<CuttingStackList> GetMinStackProductIndexCuttingStackList(String batchName,string stackNames,
            CuttingStackListBatchType cuttingStackListBatchType)
        {
            string sql = $"SELECT * FROM  CuttingStackList WHERE BatchName = '{batchName}' AND StackName NOT IN({stackNames}) " +
                $"AND BatchType = '{cuttingStackListBatchType.GetFinishStatusDescription().Item2}' AND StackProductIndex = " +
                $"(SELECT MIN(StackProductIndex) FROM  CuttingStackList WHERE BatchName = '{batchName}' AND StackName NOT IN" +
                $"({stackNames}) AND BatchType = '{cuttingStackListBatchType.GetFinishStatusDescription().Item2}')";

            var req = new CuttingStackListRepository();
            return req.FindAllBySQL(sql).ToList();
        }

        public List<PatternDetail> GetPatternDetailsByPattern(string batchName, int patternId)
        {
            var req = new PatternDetailRepository();
            return req.FindAll(item => item.BatchName == batchName && item.PatternId==patternId).ToList();
        }

        public List<PatternDetail> GetPatternDetailsByPartId(string partId)
        {
            var req = new PatternDetailRepository();
            return req.FindAll(item => item.PartId == partId).ToList();
        }

        public bool BulkInsertPatternDetails(List<PatternDetail> patternDetails)
        {
            var req = new PatternDetailRepository();
            PatternDetail p;
            string insertSql = GetInsertSql<PatternDetail>(nameof(p.LineId));
            return req.ExecuteSql(insertSql, patternDetails);
        }

        public bool BulkUpdatePatternDetails(List<PatternDetail> patternDetails)
        {
            var req = new PatternDetailRepository();
            PatternDetail p;
            string insertSql = GetUpdateSql<PatternDetail>(new string[]{nameof(p.LineId)});
            return req.ExecuteSql(insertSql, patternDetails);
        }

        public bool BulkUpdateNewPatternDetails(List<PatternDetail> patternDetails)
        {
            if (patternDetails == null || patternDetails.Count == 0) return true;
            var req = new PatternDetailRepository();
            var maxLineId = patternDetails.Max(item => item.LineId);
            var minLineId = patternDetails.Min(item => item.LineId);
            var dbs = req.FindAll(item => item.LineId >= minLineId && item.LineId <= maxLineId).ToList();

            var updates = dbs.Join(patternDetails, db => db.LineId, patternDetail => patternDetail.LineId,
                (db, pd) => new {Db = db, Pd = pd}).Where(item => item.Db.UpdatedTime < item.Pd.UpdatedTime).ToList().ConvertAll(item=>item.Pd);
            if (updates.Count == 0) return true;

            PatternDetail p;
            string updateSql = GetUpdateSql<PatternDetail>(new string[] { nameof(p.LineId) });
            updateSql += $" AND [{nameof(p.UpdatedTime)}]<@{nameof(p.UpdatedTime)}";
            return req.ExecuteSql(updateSql, updates);
        }

        public bool BulkDeletePatternDetails(List<PatternDetail> patternDetails)
        {
            var req = new PatternDetailRepository();
            PatternDetail p;
            string deleteSql = string.Format("DELETE [dbo].[{0}] WHERE [{1}]=@{1}", nameof(PatternDetail),
                nameof(p.LineId));
            return req.ExecuteSql(deleteSql, patternDetails);
        }

        public DataSet GetMdbDataByBatchId(string batchId)
        {
            var repository = new PatternDetailRepository();
            var command = repository.Connection.CreateCommand();
            command.CommandText = "GetBatchMdb";
            command.CommandType = CommandType.StoredProcedure;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@BatchId";
            parameter.Value = batchId;
            command.Parameters.Add(parameter);
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command);
            DataSet ds = new DataSet();
            ds.DataSetName = "MDBInfos";
            adapter.Fill(ds);
            ds.Tables[0].TableName = "PATTERNS";
            ds.Tables[1].TableName = "JOBS";
            ds.Tables[2].TableName = "NOTES";
            ds.Tables[3].TableName = "BOARDS";
            ds.Tables[4].TableName = "MATERIALS";
            ds.Tables[5].TableName = "CUTS";
            ds.Tables[6].TableName = "PARTS_UDI";
            ds.Tables[7].TableName = "PARTS_DST";
            ds.Tables[8].TableName = "PARTS_REQ";
            ds.Tables[9].TableName = "OFFCUTS";
            ds.Tables[10].TableName = "HEADER";
            return ds;
        }

        public List<Stack> GetStacksByFirstBatchName(string firstBatchName)
        {
            var req = new StackRepository();
            return req.FindAll(item => item.FirstBatchName == firstBatchName).ToList();
        }

        public List<Stack> GetStacksBySecondBatchName(string secondBatchName)
        {
            var req = new StackRepository();
            return req.FindAll(item => item.SecondBatchName == secondBatchName).ToList();
        }

        public List<Stack> GetStacksByPlanDate(DateTime planDate)
        {
            var req = new StackRepository();
            return req.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<Stack> GetStacksByUpdatedTime(DateTime minUpdatedTime)
        {
            var rep = new StackRepository();
            return rep.FindAll(item => item.UpdatedTime >= minUpdatedTime).ToList();
        }

        public List<Stack> GetStacksByStatus(StackStatus status)
        {
            var req = new StackRepository();
            return req.FindAll(item => item.Status == Convert.ToInt32(status)).ToList();
        }

        public List<Stack> GetStacksByStackName(string stackName)
        {
            var req = new StackRepository();
            return req.FindAll(item => item.StackName == stackName).ToList();
        }

        public bool BulkInsertStacks(List<Stack> stacks)
        {
            if (stacks == null || stacks.Count == 0) return true;
            var req = new StackRepository();
            Stack stack;
            string sql = GetInsertSql<Stack>(nameof(stack.LineId));
            return req.ExecuteSql(sql, stacks);
        }

        public bool BulkDeleteStacks(List<Stack> stacks)
        {
            if(stacks == null || stacks.Count == 0) return true;
            Stack stack;
            string deleteSql = string.Format("DELETE [dbo].[{0}] WHERE [{1}]=@{1}", nameof(Stack),
                nameof(stack.LineId));
            var req = new StackRepository();
            return req.ExecuteSql(deleteSql, stacks);
        }

        public bool BulkUpdatedStacks(List<Stack> stacks)
        {
            if (stacks == null || stacks.Count == 0) return true;
            Stack stack;
            string sql = GetUpdateSql<Stack>(new string[] { nameof(stack.LineId) });
            var req = new StackRepository();
            return req.ExecuteSql(sql, stacks);
        }

        public List<Stack> GetStacksByDevice(string deviceName, StackStatus stackStatus)
        {
            var req = new StackRepository();
            return req.FindAll(item =>
                    item.ActualDeviceName == deviceName && 
                    item.Status == Convert.ToInt32(stackStatus))
                .ToList();
        }

        public List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate)
        {
            var req = new StackDetailRepository();
            return req.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<StackDetail> GetStackDetailsByStackName(string stackName)
        {
            var req = new StackDetailRepository();
            return req.FindAll(item => item.StackName == stackName).ToList();
        }

        public bool BulkInsertStackDetails(List<StackDetail> stackDetails)
        {
            if (stackDetails == null || stackDetails.Count == 0) return true;
            StackDetail stackDetail;
            string sql = GetInsertSql<StackDetail>(nameof(stackDetail.LineId));
            var req = new StackDetailRepository();
            return req.ExecuteSql(sql, stackDetails);
        }

        public bool BulkDeleteStackDetails(List<StackDetail> stackDetails)
        {
            if (stackDetails == null || stackDetails.Count == 0) return true;
            StackDetail stackDetail;
            string deleteSql = string.Format("DELETE [dbo].[{0}] WHERE [{1}]=@{1}", nameof(StackDetail),
                nameof(stackDetail.LineId));
            var req = new StackDetailRepository();
            return req.ExecuteSql(deleteSql, stackDetails);
        }

        /// <summary>
        /// 根据Id删除BatchNamePilerNoBind
        /// </summary>
        /// <param name="batchNamePilerNoBind"></param>
        /// <returns></returns>
        public bool DeleteBatchNamePilerNoBind(BatchNamePilerNoBind batchNamePilerNoBind)
        {
            string deleteSql = $"DELETE BatchNamePilerNoBind WHERE Id = {batchNamePilerNoBind.Id}";
            var req = new BatchNamePilerNoBindRepository();
            return req.ExecuteSql(deleteSql,null);
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

        public List<PartFeedBack> GetPartFeedBacksByPartId(string partId)
        {
            var rep = new PartFeedBackRepository();
            return rep.FindAll(item => item.PartId == partId).ToList();
        }

        public bool BulkInsertCuttingPartScanLog(List<CuttingPartScanLog> cuttingPartScanLogs)
        {
            if (cuttingPartScanLogs == null || cuttingPartScanLogs.Count == 0) return true;
            var rep = new CuttingPartScanLogRepository();
            CuttingPartScanLog cps;
            string sql =
                GetInsertSql<CuttingPartScanLog>(nameof(cps.LineId), nameof(cps.CreatedTime), nameof(cps.UpdatedTime));

            return rep.ExecuteSql(sql, cuttingPartScanLogs);
        }


        public bool BulkUpdatedStackDetails(List<StackDetail> stackDetails)
        {
            if (stackDetails == null || stackDetails.Count == 0) return true;
            StackDetail stackDetail;
            string sql = GetUpdateSql<StackDetail>(new string[] { nameof(stackDetail.LineId) });
            var req = new StackDetailRepository();
            return req.ExecuteSql(sql, stackDetails);
        }

        public List<BatchGroup> GetBatchGroupsByPlanDate(DateTime planDate)
        {
            var req = new BatchGroupRepository();
            return req.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<BatchGroup> GetAllBatchGroups()
        {
            var req = new BatchGroupRepository();
            return req.FindAll().ToList();
        }

        
        public List<BatchGroup> GetUnFinishedBatchGroups()
        {
            //var req = new BatchGroupRepository();
            //return req.FindAll(item=>!item.IsFinished).ToList();
            return null;
        }

        public bool BulkInsertBatchGroups(List<BatchGroup> batchGroups)
        {
            if (batchGroups == null || batchGroups.Count == 0) return true;
            var req = new BatchGroupRepository();
            BatchGroup batchGroup;
            string sql = GetInsertSql<BatchGroup>(nameof(batchGroup.LineId));
            return req.ExecuteSql(sql, batchGroups);

        }

        public bool BulkInsertBatchGroupPlus(List<BatchGroupPlus> listBatchGroupPlus)
        {
            if (listBatchGroupPlus == null || listBatchGroupPlus.Count == 0) return true;
            var req = new BatchGroupPlusRepository();
            BatchGroupPlus batchGroupPlus;
            string sql = GetInsertSql<BatchGroupPlus>(nameof(batchGroupPlus.LineId), nameof(batchGroupPlus.StartLoadingTime));
            return req.ExecuteSql(sql, listBatchGroupPlus);

        }

        public bool BulkUpdatedBatchGroups(List<BatchGroup> batchGroups)
        {
            if (batchGroups == null || batchGroups.Count == 0) return true;
            var req = new BatchGroupRepository();
            BatchGroup batchGroup;
            string sql = GetUpdateSql<BatchGroup>(new[]{ nameof(batchGroup.LineId) });
            return req.ExecuteSql(sql, batchGroups);

        }

        public bool BulkUpdatedBatchGroupsLoadTime(List<BatchGroup> batchGroups)
        {
            if (batchGroups == null || batchGroups.Count == 0) return true;
            var req = new BatchGroupRepository();
            BatchGroup batchGroup;
            string sql = GetUpdatePropertiesSql<BatchGroup>(new[] { nameof(batchGroup.LineId) },nameof(batchGroup.StartLoadingTime));
            return req.ExecuteSql(sql, batchGroups);
        }

        public bool BulkUpdatedCuttingSawFileRelationPlus(List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus)
        {
            if (listCuttingSawFileRelationPlus == null || listCuttingSawFileRelationPlus.Count == 0) return true;
            var req = new CuttingSawFileRelationPlusRepository();
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus;
            string sql = GetUpdatePropertiesSql<CuttingSawFileRelationPlus>(new[] { nameof(cuttingSawFileRelationPlus.SawFileName) },
                new[] { nameof(cuttingSawFileRelationPlus.Status),nameof(cuttingSawFileRelationPlus.DeviceName),nameof(cuttingSawFileRelationPlus.UpdatedTime) });
            return req.ExecuteSql(sql, listCuttingSawFileRelationPlus);
        }

        public bool BulkInsertCuttingStackProductionList(List<CuttingStackProductionList> cuttingStackProductionLists)
        {
            if (cuttingStackProductionLists == null || cuttingStackProductionLists.Count == 0) return true;
            CuttingStackProductionList cutting;
            string sql = GetInsertSql<CuttingStackProductionList>(nameof(cutting.LineId));
            var req = new CuttingStackProductionListRepository();
            return req.ExecuteSql(sql, cuttingStackProductionLists);
        }

        public List<CuttingStackProductionList> GetCuttingStackProductionLists()
        {
            var req = new CuttingStackProductionListRepository();
            return req.FindAll().ToList();
        }

        public List<CuttingStackProductionList> GetCuttingStackProductionListsByStackName(string stackName)
        {
            var req = new CuttingStackProductionListRepository();
            return req.FindAll(item=>item.StackName==stackName).ToList();
        }

        public List<CuttingStackProductionList> GetCuttingStackProductionListsByStatus(RequestLoadingStatus status)
        {
            var req = new CuttingStackProductionListRepository();
            return req.FindAll(item=>item.Status == Convert.ToInt32(status)).ToList();
        }

        public List<WMSStacktFeedBack> GetWmsStacktFeedBacksByStackName(string stackName)
        {
            var req = new WmsStacktFeedBackRepository();
            return req.FindAll(item => item.StackName == stackName).ToList();
        }

        public List<PartFeedBack> GetPartFeedBacksByStatus(PartFeedBackStatus status)
        {
            var req = new PartFeedBackRepository();
            return req.FindAll(item => item.Status == Convert.ToInt32(status)).ToList();
        }

        public List<PartFeedBack> GetPartFeedBacksByBatchName(string batchName)
        {
            var req = new PartFeedBackRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        public List<BatchProductionDetails> GetBatchProductionDetailsByBatchName(string batchName)
        {
            var req = new BatchProductionDetailsRepository();
            return req.FindAll(item => item.BatchName == batchName).ToList();
        }

        public List<PartFeedBack> GetPartFeedBacksTop1000(DateTime minCreatedTime)
        {
            var req = new PartFeedBackRepository();
            PartFeedBack p;
            string sql =
                $"SELECT TOP 1000 *  FROM {nameof(PartFeedBackRepository)} WHERE [{nameof(p.CreatedTime)}]='{minCreatedTime}' ORDER BY  [{nameof(p.CreatedTime)}] DESC";
            return req.Connection.Query<PartFeedBack>(sql, null).ToList();
        }

        public List<PLCLog> GetPLCLogByDate(DateTime dateTime)
        {
            var plcLogRepository = new PLCLogRepository();
            string sql = $"SELECT Id,Station,TriggerType,substring(Detail,0,200) AS Detail,CreatedTime,LogType FROM PLCLog WHERE " +
                $"DATEDIFF(DAY,CreatedTime,'{dateTime}')=0 ORDER BY CreatedTime DESC";
            return plcLogRepository.FindAllBySQL(sql).ToList();
        }

        public List<BatchGroupPlus> GetBatchGroupPlusByBatchName(string BatchName)
        {
            BatchGroupPlusRepository batchGroupPlusRepository = new BatchGroupPlusRepository();
            string sql = $"SELECT * FROM BatchGroupPlus WHERE BatchName = '{BatchName}'";
            return batchGroupPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<BatchGroupPlus> GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus batchGroupPlusStatus)
        {
            BatchGroupPlusRepository batchGroupPlusRepository = new BatchGroupPlusRepository();
            string sql = $"SELECT * FROM BatchGroupPlus WHERE Status = {(int)batchGroupPlusStatus} AND BatchIndex = " +
                $"(SELECT MIN(BatchIndex) FROM BatchGroupPlus WHERE Status = {(int)batchGroupPlusStatus})";
            return batchGroupPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<BatchGroupPlus> GetBatchGroupPlus(DateTime planDate,string column)
        {
            BatchGroupPlusRepository batchGroupPlusRepository = new BatchGroupPlusRepository();
            string sql = $"SELECT {column} FROM BatchGroupPlus WHERE PlanDate = '{planDate}'";
            return batchGroupPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<BatchGroupPlus> GetBatchGroupPlusByStatus(BatchGroupPlusStatus batchGroupPlusStatus)
        {
            BatchGroupPlusRepository batchGroupPlusRepository = new BatchGroupPlusRepository();
            string sql = $"SELECT * FROM BatchGroupPlus WHERE Status = {(int)batchGroupPlusStatus} ORDER BY BatchIndex";
            return batchGroupPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByBatchNameAndStatus(string BatchName,int status)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE BatchName = '{BatchName}' AND Status = {status}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByStackNameAndStatus(string satchName, CuttingSawFileRelationPlusStatus status)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE StackName = '{satchName}' AND Status = {(int)status}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据planDate，sawType获取指定column的CuttingSawFileRelationPlus数据
        /// </summary>
        /// <param name="planDate">计划日期</param>
        /// <param name="column">字段名</param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlus(DateTime planDate,
            string column, SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT {column} FROM CuttingSawFileRelationPlus WHERE PlanDate = '{planDate}'" +
                $" AND SawType = {(int)sawType}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlus(DateTime planDate,
            string groupByColumn)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT {groupByColumn} FROM CuttingSawFileRelationPlus WHERE PlanDate = '{planDate}' GROUP BY {groupByColumn}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据satchName，status，sawType获取CuttingSawFileRelationPlus数据
        /// </summary>
        /// <param name="satchName"></param>
        /// <param name="status"></param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlus(string satchName, 
            CuttingSawFileRelationPlusStatus status,SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE StackName = '{satchName}' AND Status = " +
                $"{(int)status} AND SawType = {(int)sawType}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据batchName，sawType,获取CuttingSawFileRelationPlus数据
        /// </summary>
        /// <param name="satchName"></param>
        /// <param name="sawType"></param>
        /// <param name="cuttingSawFileRelationPlusStatus"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlus(string batchName, SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE BatchName = '{batchName}'" +
                $" AND SawType = {(int)sawType}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据batchName，sawType,获取不包含CuttingSawFileRelationPlusStatus的CuttingSawFileRelationPlus数据
        /// </summary>
        /// <param name="satchName"></param>
        /// <param name="sawType"></param>
        /// <param name="cuttingSawFileRelationPlusStatus"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlus(string batchName, SawType sawType,
            CuttingSawFileRelationPlusStatus cuttingSawFileRelationPlusStatus)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE BatchName = '{batchName}'" +
                $" AND SawType = {(int)sawType} AND Status <> {(int)cuttingSawFileRelationPlusStatus}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByDate(DateTime dateTime)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE DATEDIFF(DAY,PlanDate,'{dateTime}')=0";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByCreateTime(DateTime dateTime)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE DATEDIFF(DAY,CreatedTime,'{dateTime}')=0";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByCreateTime(DateTime dateTime,string column)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT {column} FROM CuttingSawFileRelationPlus WHERE DATEDIFF(DAY,CreatedTime,'{dateTime}')=0";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByDeviceNameAndCreatedTime(string deviceName,DateTime dateTime)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE DATEDIFF(DAY,CreatedTime,'{dateTime}')=0 AND DeviceName = '{deviceName}'";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusBySawFileName(string name)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE " +
                $"SawFileName = '{name}'";
            return cuttingSawFileRelationPlusRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据stackName，sawType获取数据
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="sawType"></param>
        /// <param name="cuttingSawFileRelationPlusStatus"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByStackName(string stackName,SawType sawType)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE " +
                $"StackName = '{stackName}' AND SawType = {(int)sawType}";
            return cuttingSawFileRelationPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByBatchName(string batchName)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE BatchName = '{batchName}'";
            return cuttingSawFileRelationPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByBatchNameAndSawType(string batchName,
            SawType sawType)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE BatchName = '{batchName}' AND SawType = " +
                $"{sawType.GetFinishStatusDescription().Item1}";
            return cuttingSawFileRelationPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelationPlus> GetCuttingSawFileRelationPlusByDeviceNameAndStatus(string DeviceName,CuttingSawFileRelationPlusStatus cuttingSawFileRelationPlusStatus)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelationPlus WHERE DeviceName = '{DeviceName}' And Status = {(int)cuttingSawFileRelationPlusStatus}";
            return cuttingSawFileRelationPlusRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByStackName(string stackName,SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE StackName = '{stackName}' AND SawType = " +
                $"{(int)sawType}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationBySawFileName(string name)
        {
            CuttingSawFileRelationRepository repository = new CuttingSawFileRelationRepository();

            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE SawFileName = '{name}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByBatchName(string BatchName)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{BatchName}' AND SawType = 0";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据批次号、sawType获取CuttingSawFileRelation，但不包含stackName的数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="stackName"></param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelation(string batchName,
            string stackNames,SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = " +
                $"{(int)sawType} AND StackName NOT IN({stackNames})";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据计划日期，sawType获取CuttingSawFileRelation，但不包含stackNames的数据
        /// </summary>
        /// <param name="planDate">计划日期</param>
        /// <param name="stackNames">垛号</param>
        /// <param name="column">要查询的字段</param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelation(DateTime planDate,string stackNames,
            string column, SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT {column} FROM CuttingSawFileRelation WHERE PlanDate = '{planDate}' AND SawType = " +
                $"{(int)sawType} AND StackName NOT IN({stackNames})";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据planDate，sawType获取不包含stackNames，batchNames的CuttingSawFileRelation数据
        /// </summary>
        /// <param name="planDate">计划日期</param>
        /// <param name="stackNames">不包含的垛</param>
        /// <param name="batchNames">不包含的批次</param>
        /// <param name="column">要查询的字段</param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelation(DateTime planDate, string stackNames,string batchNames,
            string column, SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT {column} FROM CuttingSawFileRelation WHERE PlanDate = '{planDate}' AND SawType = " +
                $"{(int)sawType} AND StackName NOT IN({stackNames}) AND BatchName NOT IN({batchNames})";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据批次号、sawType获取CuttingSawFileRelation
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="sawType"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelation(string batchName, SawType sawType)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = " +
                $"{sawType.GetFinishStatusDescription().Item1}";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelation> GetMinStackIndexCuttingSawFileRelationByStackName(string stackName)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = 
                GetCuttingSawFileRelationPlusByStackName(stackName,SawType.TYPE1);
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }

            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = "";
            if (String.IsNullOrEmpty(sawFileName))
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE " +
                $"StackName = '{stackName}' AND SawType = 0 AND StackIndex = (select MIN(StackIndex) from CuttingSawFileRelation WHERE StackName = '{stackName}' AND SawType = 0)";
            else
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE " +
                $"StackName = '{stackName}' AND SawType = 0 AND StackIndex = (select MIN(StackIndex) from " +
                $"CuttingSawFileRelation where SawFileName not in({sawFileName}) AND StackName = '{stackName}' AND SawType = 0)";

            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据批次号获取一条CuttingSawFileRelationPlus表中不存在的SawIndex最小的一条数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByBatchNameAndMinStackIndex(string batchName)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = 
                GetCuttingSawFileRelationPlusByBatchName(batchName);
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }

            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();

            string sql = "";
            if (String.IsNullOrEmpty(sawFileName))
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0 AND StackIndex = " +
                    $"(SELECT MIN(StackIndex) From CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0)";
            else
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0 AND StackIndex = " +
                    $"(SELECT MIN(StackIndex) FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawFileName not in({sawFileName}) AND " +
                    $"SawType = 0)";

            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 根据batchName，sawType，但不包含sawFileNames,stackNames的CuttingSawFileRelation数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="sawFileNames"></param>
        /// <param name="stackNames"></param>
        /// <param name="sawType"></param>
        /// <param name="column">需要查询的列</param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetCuttingSawFileRelation(string batchName,string sawFileNames,
            string stackNames,SawType sawType,string column)
        {
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            
            string sql = $"SELECT {column} FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND " +
                $"SawFileName NOT IN({sawFileNames}) AND StackName NOT IN({stackNames}) AND SawType = {(int)sawType}";

            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByStackNameAndMinStackIndex(string stackName)
        {
            //已下发给1-5号锯的锯切图
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = 
                GetCuttingSawFileRelationPlusByStackName(stackName,SawType.TYPE1);
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }

            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();

            if (String.IsNullOrEmpty(sawFileName))
                sawFileName += "''";
                //string sql = "";
                //if (String.IsNullOrEmpty(sawFileName))
                //    sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0 AND StackIndex = " +
                //        $"(SELECT MIN(StackIndex) From CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0)";
                //else
                string sql = $"SELECT * FROM CuttingSawFileRelation WHERE StackName = '{stackName}' AND SawType = 0 AND " +
                $"StackIndex = (SELECT MIN(StackIndex) FROM CuttingSawFileRelation WHERE StackName = '{stackName}' AND " +
                $"SawFileName NOT IN({sawFileName}) AND SawType = 0)";

            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public List<CuttingSawFileRelation> GetMinStackIndexCuttingSawFileRelationByBatchName(string batchName,
            string sawFileNames,string stackNames)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus =
                GetCuttingSawFileRelationPlusByBatchName(batchName);
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += listCuttingSawFileRelationPlus[i].SawFileName;
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }

            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();
            string sql = $"SELECT * FROM CuttingSawFileRelation WHERE " +
                $"BatchName = '{batchName}' AND SawType = 0 AND StackIndex = (select MIN(StackIndex) from CuttingSawFileRelation where SawFileName" +
                $" not in({sawFileName}) AND BatchName = '{batchName}' AND SawType = 0)";
            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public List<CuttingSawFileRelation> GetCuttingSawFileRelationByBatchNameAndBoardCount(string batchName,int boardCount)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = 
                GetCuttingSawFileRelationPlusByBatchName(batchName);
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }

            var cuttingSawFileRelationRepository = new CuttingSawFileRelationRepository();

            string sql = "";
            if (String.IsNullOrEmpty(sawFileName))
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0 AND BoardCount = {boardCount}";
            else
                sql = $"SELECT * FROM CuttingSawFileRelation WHERE BatchName = '{batchName}' AND SawType = 0 AND BoardCount = {boardCount} AND SawFileName not in({sawFileName})";

            return cuttingSawFileRelationRepository.FindAllBySQL(sql).ToList();
        }

        public bool BulkInsertPLCLog(List<PLCLog> listPLCLog)
        {
            if (listPLCLog == null || listPLCLog.Count == 0) return true;
            PLCLog plcLog;
            var plcLogRepository = new PLCLogRepository();
            string sql = GetInsertSql<PLCLog>(nameof(plcLog.Id),nameof(plcLog.CreatedTime));
            return plcLogRepository.ExecuteSql(sql, listPLCLog);
        }

        public bool InsertPLCLog(PLCLog plcLog)
        {
            var plcLogRepository = new PLCLogRepository();
            string sql = GetInsertSql<PLCLog>(nameof(plcLog.Id), nameof(plcLog.CreatedTime));
            return plcLogRepository.ExecuteSql(sql, plcLog);
        }

        public bool InsertCuttingSawFileRelationPlus(CuttingSawFileRelationPlus cuttingSawFileRelationPlus)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = GetInsertSql<CuttingSawFileRelationPlus>("Id");
            return cuttingSawFileRelationPlusRepository.ExecuteSql(sql, cuttingSawFileRelationPlus);
        }

        public bool BulkInsertCuttingSawFileRelationPlus(List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus)
        {
            if (listCuttingSawFileRelationPlus == null || listCuttingSawFileRelationPlus.Count == 0) return true;
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus;
            var cuttingSawFileRelationRepository = new CuttingSawFileRelationPlusRepository();
            string sql = GetInsertSql<CuttingSawFileRelationPlus>(nameof(CuttingSawFileRelationPlus.Id));
            return cuttingSawFileRelationRepository.ExecuteSql(sql, listCuttingSawFileRelationPlus);
        }

        public bool BulkInsertPartFeedBacks(List<PartFeedBack> partFeedBacks)
        {
            if (partFeedBacks == null || partFeedBacks.Count == 0) return true;
            var req = new PartFeedBackRepository();
            PartFeedBack partFeedBack;
            string sql = GetInsertSql<PartFeedBack>(nameof(partFeedBack.LineId));
            return req.ExecuteSql(sql, partFeedBacks);
        }

        public bool BulkUpdatePartFeedBacks(List<PartFeedBack> partFeedBacks)
        {

            if (partFeedBacks == null || partFeedBacks.Count == 0) return true;
            var req = new PartFeedBackRepository();
            PartFeedBack partFeedBack;
            string sql = GetUpdateSql<PartFeedBack>(new []{ nameof(partFeedBack.LineId) });
            return req.ExecuteSql(sql, partFeedBacks);
        }

        public bool BulkUpdateBatchGroupPlus(List<BatchGroupPlus> listBatchGroupPlus)
        {

            if (listBatchGroupPlus == null || listBatchGroupPlus.Count == 0) return true;
            var req = new BatchGroupPlusRepository();
            BatchGroupPlus BatchGroupPlus;
            string sql = GetUpdateSql<BatchGroupPlus>(new[] { nameof(BatchGroupPlus.LineId) });
            return req.ExecuteSql(sql, listBatchGroupPlus);
        }

        public bool BulkUpdateBatchGroupPlusStatusByBatchName(List<BatchGroupPlus> listBatchGroupPlus)
        {

            if (listBatchGroupPlus == null || listBatchGroupPlus.Count == 0) return true;
            var req = new BatchGroupPlusRepository();
            BatchGroupPlus BatchGroupPlus;
            string sql = GetUpdateSql<BatchGroupPlus>(new[] { nameof(BatchGroupPlus.BatchName) },new[] { nameof(BatchGroupPlus.BatchIndex),
                nameof(BatchGroupPlus.BatchName),nameof(BatchGroupPlus.CreatedTime),nameof(BatchGroupPlus.LineId),nameof(BatchGroupPlus.PlanDate),
                nameof(BatchGroupPlus.StartLoadingTime)});
            return req.ExecuteSql(sql, listBatchGroupPlus);
        }

        /// <summary>
        /// 根据Id修改BatchNamePilerNoBind
        /// </summary>
        /// <param name="listBatchNamePilerNoBind"></param>
        /// <returns></returns>
        public bool BulkUpdateBatchNamePilerNoBindByPilerNo(List<BatchNamePilerNoBind> listBatchNamePilerNoBind)
        {

            if (listBatchNamePilerNoBind == null || listBatchNamePilerNoBind.Count == 0) return true;
            var req = new BatchNamePilerNoBindRepository();
            BatchNamePilerNoBind batchNamePilerNoBind;
            string sql = GetUpdateSql<BatchNamePilerNoBind>(new[] { nameof(BatchNamePilerNoBind.Id) });
            return req.ExecuteSql(sql, listBatchNamePilerNoBind);
        }

        public bool BulkUpdateBatchProductionDetails(List<BatchProductionDetails> listBatchProductionDetails)
        {

            if (listBatchProductionDetails == null || listBatchProductionDetails.Count == 0) return true;
            var req = new BatchProductionDetailsRepository();
            BatchProductionDetails batchProductionDetails;
            string sql = GetUpdateSql<BatchProductionDetails>(new[] { nameof(BatchProductionDetails.Id) });
            return req.ExecuteSql(sql, listBatchProductionDetails);
        }

        public bool UpdateBatchProductionDetailsStatusByBatchName(BatchProductionDetails batchProductionDetails)
        {
            var req = new BatchProductionDetailsRepository();
            string sql = GetUpdateSql<BatchProductionDetails>(new[] { nameof(BatchProductionDetails.BatchName) },
                new[] {nameof(BatchProductionDetails.BatchName), nameof(BatchProductionDetails.DifferenceNumber),
                    nameof(BatchProductionDetails.Id),nameof(BatchProductionDetails.ProductCode),nameof(BatchProductionDetails.Total) });
            return req.ExecuteSql(sql, batchProductionDetails);
        }

        public bool BulkInsertWmsCuttingStackList(List<WMSCuttingStackList> wmsCuttingStackLists)
        {
            if (wmsCuttingStackLists == null || wmsCuttingStackLists.Count == 0) return true;
            var req = new WmsCuttingStackListRepository();
            WMSCuttingStackList cuttingStack;
            string sql = GetInsertSqlBase(nameof(WMSCuttingStackList), new []
            {
                nameof(cuttingStack.WorkshopCode),
                nameof(cuttingStack.PlanDate),
                nameof(cuttingStack.BatchName),
                nameof(cuttingStack.CreatedTime),
                nameof(cuttingStack.LastUpdatedTime),
                nameof(cuttingStack.StackName),
                nameof(cuttingStack.RawMaterialID),
                nameof(cuttingStack.BatchIndex),
                nameof(cuttingStack.TaskId),
            });
            return req.ExecuteSql(sql, wmsCuttingStackLists);
        }

        public List<MdbParse> GetMdbParses()
        {
            var rep = new MdbParseRepository();
            return rep.FindAll().ToList();
        }

        public bool BulkInsertMdbParses(List<MdbParse> mdbParses)
        {
            var rep = new MdbParseRepository();
            MdbParse mdb;
            string insertSql = GetInsertSql<MdbParse>(nameof(mdb.LineId));
            return rep.ExecuteSql(insertSql, mdbParses);
        }

        public bool UpdateCuttingSawFileRelationPlus(CuttingSawFileRelationPlus cuttingSawFileRelationPlus)
        {
            var cuttingSawFileRelationPlusRepository = new CuttingSawFileRelationPlusRepository();
            string sql = GetUpdateSql<CuttingSawFileRelationPlus>(new string[] { nameof(cuttingSawFileRelationPlus.Id) });
            return cuttingSawFileRelationPlusRepository.ExecuteSql(sql, cuttingSawFileRelationPlus);
        }

        public bool BulkUpdatedMdbParses(List<MdbParse> mdbParses)
        {
            var rep = new MdbParseRepository();
            MdbParse mdb;
            string updatedSql = GetUpdateSql<MdbParse>(new string[]{nameof(mdb.LineId)});
            return rep.ExecuteSql(updatedSql, mdbParses);
        }

        public bool BulkUpdateDeviceInfos(List<DeviceInfos> deviceInfos)
        {
            if (deviceInfos.Count == 0)
            {
                return true;
            }
            //var deviceInfo = deviceInfos[0];
            string ExeProcedureSql = "DECLARE @return_value int EXEC @return_value=[dbo].[UpdateDeviceInfo] @DeviceName=@DeviceName,@DepartmentId=@DepartmentID,@ProductionLine=@ProductionLine,@State=@State," +
                                     "@MSg=@Msg,@Remark=@Remark,@ProcessName=@ProcessName,@PlaceNo=@PlaceNo,@LineId=@LineId,@DeviceType=@DeviceType,@DeviceDescription=@DeviceDescription,@Ratio=@Ratio SELECT @return_value";

            //string ExeProcedureSql = "DECLARE @return_value int EXEC @return_value=[dbo].[UpdateDeviceInfo]";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            var conn = repository.Connection;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            var tran = conn.BeginTransaction();
            try
            {
                conn.Execute(ExeProcedureSql, deviceInfos, tran, 7200, CommandType.Text);
                //deviceInfos.ForEach(item => conn.Execute(ExeProcedureSql, item, tran, 7200, CommandType.Text));
                tran.Commit();

                return true;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<PatternFeedBack> GetPatternFeedBacksByMdbName(string mdbName)
        {
            var rep = new PatternFeedBackRepository();
            return rep.FindAll(item => item.MdbName == mdbName).ToList();
        }

        public List<PatternDetailLog> GetPatternDetailLogsByBatchName(string batchName)
        {
            var rep = new PatternDetailLogRepository();
            return rep.FindAll(item => item.BatchName == batchName).ToList();
        }

        public List<BatchGroupLog> GetBatchGroupLogsByPlanDate(DateTime planDate)
        {
            var rep = new BatchGroupLogRepository();
            return rep.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<StackLog> GetStackLogsByPlanDate(DateTime planDate)
        {
            var rep = new StackLogRepository();
            return rep.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<StackDetailLog> GetStackDetailLogsByPlanDate(DateTime planDate)
        {
            var rep = new StackDetailLogRepository();
            return rep.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<PatternLog> GetPatternLogsByPlanDate(DateTime planDate)
        {
            var rep = new PatternLogRepository();
            return rep.FindAll(item => item.PlanDate == planDate).ToList();
        }

        public List<Stack> GetUnfinishedStacks()
        {
            var rep = new StackRepository();
            string store = "GetUnFinishedStacks";
            return rep.Connection.Query<Stack>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
        }

        public List<Pattern> GetUnfinishedPatterns()
        {
            var rep = new PatternRepository();
            string store = "GetUnfinishedPatterns";
            return rep.Connection.Query<Pattern>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
        }

        public string CommitErrorTask(string mdbName)
        {
            return "此功能暂未实现，后续将会更新";
        }

        public List<Mdb_Parts_Udi> GetMdbPartsUdisByBatchName(string batchName)
        {
            var rep = new Mdb_Parts_UdiRepository();
            return rep.FindAll(item => item.INFO8 == batchName).ToList();
        }

        public bool BulkInsertDeviceInfos(List<DeviceInfos> deviceInfos)
        {
            var deviceInfo = deviceInfos[0];
            string ExeProcedureSql = "DECLARE @return_value int	EXEC @return_value=[NJIS.FPZWS.LineControl.Cutting].[dbo].[InsertDevicesFromWorkStation]";
            //@DeviceName =@DeviceName,@DepartmentId=@DepartmentID,@ProductionLine=@ProductionLine,@State=@State," +
            //"@MSg=@Msg,@Remark=@Remark,@ProcessName=@ProcessName,@DeviceType=@DeviceType,@DeviceDscription=@DeviceDscription SELECT @return_value";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            IDbConnection conn = repository.Connection;

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            StringBuilder insertSqlBuilder = new StringBuilder();
            insertSqlBuilder.AppendFormat("INSERT INTO DeviceInfos ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}) " +
                                          "VALUES (@{0},@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9})",
                nameof(deviceInfo.DepartmentId), nameof(deviceInfo.DeviceName), nameof(deviceInfo.ProcessName),
                nameof(deviceInfo.ProductionLine), nameof(deviceInfo.Remark), nameof(deviceInfo.State), nameof(deviceInfo.Msg),
                nameof(deviceInfo.DeviceType), nameof(deviceInfo.DeviceDescription), nameof(deviceInfo.PlaceNo));
            string insertSql = insertSqlBuilder.ToString();
            var tran = conn.BeginTransaction();
            try
            {
                //deviceInfos.ForEach(item => conn.Execute(ExeProcedureSql, item, tran, 7200, CommandType.Text));
                conn.Execute(insertSql, deviceInfos, tran, 7200, CommandType.Text);
                conn.Execute(ExeProcedureSql, null, tran, 7200, CommandType.Text);
                tran.Commit();
                return true;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<DeviceInfos> GetDeviceInfosByProcessName(string processName)
        {
            string selectSql = $@"SELECT * FROM DeviceInfos WHERE DeviceInfos.ProcessName='{processName}'";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            return repository.QueryList(selectSql, null).ToList();
        }

        /// <summary>
        /// 获取插入sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="noContainProperties">不需要插入的属性（字段）名</param>
        /// <returns></returns>
        private string GetInsertSql<T>(params string[] noContainProperties)
        {
            var type = typeof(T);

            var propertyInfos = type.GetProperties().ToList();
            if (noContainProperties != null && noContainProperties.Length > 0)
            {
                propertyInfos.RemoveAll(item => noContainProperties.Contains(item.Name));
            }
            var fields = propertyInfos.ConvertAll(item => item.Name);
            return GetInsertSqlBase(type.Name, fields);
            //string tag = @"@";
            
            //var fields = propertyInfos.ConvertAll(item => item.Name);
            //var values = propertyInfos.ConvertAll(item => tag + item.Name);
            //var fieldsPart = string.Join(",", fields);
            //var valuesPart = string.Join(",", values);
            //string insertSql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", type.Name, fieldsPart, valuesPart);
            //return insertSql + ";";
        }

        /// <summary>
        /// 获取插入sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="insertProperties">不需要插入的属性（字段）名</param>
        /// <returns></returns>
        private string GetInsertSqlBase(string tableName, IEnumerable<string> insertProperties)
        {
            var propertyNames = insertProperties.ToList();
            string tag = @"@";
            var fields = propertyNames.ConvertAll(item=>"["+item+"]");
            var values = propertyNames.ConvertAll(item => tag + item);
            var fieldsPart = string.Join(",", fields);
            var valuesPart = string.Join(",", values);
            string insertSql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, fieldsPart, valuesPart);
            return insertSql;
        }


        /// <summary>
        /// 获取更新的sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byProperties">条件属性（字段）名</param>
        /// <param name="unUpdateProperties">更新的属性（字段）名</param>
        /// <returns></returns>
        private string GetUpdateSql<T>(string[] byProperties, params string[] unUpdateProperties)
        {
            var type = typeof(T);

            var propertyInfos = type.GetProperties().ToList();
            if (unUpdateProperties != null && unUpdateProperties.Length > 0)
            {
                propertyInfos.RemoveAll(item => unUpdateProperties.Contains(item.Name));
            }

            propertyInfos.RemoveAll(item => byProperties.Contains(item.Name));

            string tag = @"@";
             
            var fields = byProperties.ToList().ConvertAll(s => $"[{s}]={tag}{s}");
            var updateParts = propertyInfos.ConvertAll(item => $"[{item.Name}]={tag}{item.Name}");
            var fieldsPart = string.Join(" AND ", fields);
            var valuesPart = string.Join(",", updateParts);
            string insertSql = string.Format("UPDATE {0}  SET {1} WHERE {2}", type.Name, valuesPart, fieldsPart);
            return insertSql ;

        }


        /// <summary>
        /// 获取更新指定字段的sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byProperties">条件属性（字段）名</param>
        /// <param name="updateProperties">更新的属性（字段）名</param>
        /// <returns></returns>
        private string GetUpdatePropertiesSql<T>(string[] byProperties, params string[] updateProperties)
        {
            var type = typeof(T);

            var propertyInfos = type.GetProperties().ToList();
            if (updateProperties != null && updateProperties.Length > 0)
            {
                propertyInfos = propertyInfos.FindAll(item => updateProperties.Contains(item.Name));
            }

            propertyInfos.RemoveAll(item => byProperties.Contains(item.Name));

            string tag = @"@";

            var fields = byProperties.ToList().ConvertAll(s => $"[{s}]={tag}{s}");
            var updateParts = propertyInfos.ConvertAll(item => $"[{item.Name}]={tag}{item.Name}");
            var fieldsPart = string.Join(" AND ", fields);
            var valuesPart = string.Join(",", updateParts);
            string insertSql = string.Format("UPDATE {0}  SET {1} WHERE {2}", type.Name, valuesPart, fieldsPart);
            return insertSql ;

        }

    }
}
