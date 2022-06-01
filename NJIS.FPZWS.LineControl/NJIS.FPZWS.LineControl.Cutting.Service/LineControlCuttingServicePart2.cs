using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public partial class LineControlCuttingService
    {
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

        public bool BulkInsertPatterns(List<Pattern> patterns)
        {
            var req = new PatternRepository();
            Pattern p;
            string insertSql = GetInsertSql<Pattern>(nameof(p.LineId));
            return req.ExecuteSql(insertSql, patterns);
        }


 
        public bool BulkUpdatePatterns(List<Pattern> patterns)
        {
            var req = new PatternRepository();
            Pattern p;
            string insertSql = GetUpdateSql<Pattern>(new string[]{nameof(p.LineId)});
            return req.ExecuteSql(insertSql, patterns);
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
            var req = new BatchGroupRepository();
            return req.FindAll(item=>!item.IsFinished).ToList();
        }

        public bool BulkInsertBatchGroups(List<BatchGroup> batchGroups)
        {
            if (batchGroups == null || batchGroups.Count == 0) return true;
            var req = new BatchGroupRepository();
            BatchGroup batchGroup;
            string sql = GetInsertSql<BatchGroup>(nameof(batchGroup.LineId));
            return req.ExecuteSql(sql, batchGroups);

        }

        public bool BulkUpdatedBatchGroups(List<BatchGroup> batchGroups)
        {
            if (batchGroups == null || batchGroups.Count == 0) return true;
            var req = new BatchGroupRepository();
            BatchGroup batchGroup;
            string sql = GetUpdateSql<BatchGroup>(new[]{ nameof(batchGroup.LineId) });
            return req.ExecuteSql(sql, batchGroups);

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
            var req = new WMSStacktFeedBackRepository();
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

        public List<PartFeedBack> GetPartFeedBacksTop1000(DateTime minCreatedTime)
        {
            var req = new PartFeedBackRepository();
            PartFeedBack p;
            string sql =
                $"SELECT TOP 1000 *  FROM {nameof(PartFeedBackRepository)} WHERE [{nameof(p.CreatedTime)}]='{minCreatedTime}' ORDER BY  [{nameof(p.CreatedTime)}] DESC";
            return req.Connection.Query<PartFeedBack>(sql, null).ToList();
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

        public bool BulkInsertWmsCuttingStackList(List<WMSCuttingStackList> wmsCuttingStackLists)
        {
            if (wmsCuttingStackLists == null || wmsCuttingStackLists.Count == 0) return true;
            var req = new WMSCuttingStackListRepository();
            WMSCuttingStackList cuttingStack;
            string sql = GetInsertSqlBase(nameof(WMSCuttingStackList), new []
            {
                nameof(cuttingStack.PlanDate),
                nameof(cuttingStack.BatchName),
                nameof(cuttingStack.CreatedTime),
                nameof(cuttingStack.LastUpdatedTime),
                nameof(cuttingStack.StackName),
                nameof(cuttingStack.RawMaterialID),
                nameof(cuttingStack.StackIndex),
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

        public bool BulkUpdatedMdbParses(List<MdbParse> mdbParses)
        {
            var rep = new MdbParseRepository();
            MdbParse mdb;
            string updatedSql = GetUpdateSql<MdbParse>(new string[]{nameof(mdb.LineId)});
            return rep.ExecuteSql(updatedSql, mdbParses);
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
            return insertSql + ";";

        }

    }
}
