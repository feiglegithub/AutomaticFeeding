using Dapper;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Repository;
using NJIS.FPZWS.Wcf.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public class WorkStationService : WcfServer<WorkStationService>, IWorkStationContract
    {
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
            insertSqlBuilder.AppendFormat("INSERT INTO DeviceInfos ({0},{1},{2},{3},{4},{5},{6},{7},{8}) VALUES (@{0},@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8})", 
                nameof(deviceInfo.DepartmentId), nameof(deviceInfo.DeviceName), nameof(deviceInfo.ProcessName), nameof(deviceInfo.ProductionLine), nameof(deviceInfo.Remark), nameof(deviceInfo.State), nameof(deviceInfo.Msg), nameof(deviceInfo.DeviceType), nameof(deviceInfo.DeviceDescription));
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

        public List<SpiltMDBResult> GetCanLoadMaterialMdbResults(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"EXEC GetCanLoadMaterial @PlanDate='{date:yyyy-MM-dd}'";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public List<SpiltMDBResult> GetCanLoadingMaterialMdbResults(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"SELECT * FROM SpiltMDBResult smr WHERE smr.[PlanDate]='{date:yyyy-MM-dd}' AND smr.[FinishedStatus]={Convert.ToInt32(FinishedStatus.WaitMaterial)}";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public List<SpiltMDBResult> GetCanLoadedMaterialMdbResults(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"SELECT * FROM SpiltMDBResult smr WHERE smr.[PlanDate]='{date:yyyy-MM-dd}' AND smr.[FinishedStatus]={Convert.ToInt32(FinishedStatus.LoadingMaterial)}";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public List<SpiltMDBResult> GetCuttingsCurrTasks(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"EXEC GetCuttingsCurrTasks @PlanDate='{date:yyyy-MM-dd}'";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public SpiltMDBResult GetCurrDeviceCuttingNextTasks(DateTime planDate, string deviceName)
        {
            DateTime date = planDate.Date;
            var repository = new SpiltMDBResultRepository();
            var rets = repository.FindAll(item =>
                item.DeviceName == deviceName && item.PlanDate.Date == date &&
                item.FinishedStatus >= Convert.ToInt32(FinishedStatus.NeedToSaw) &&
                item.FinishedStatus <= Convert.ToInt32(FinishedStatus.LoadedMaterial)).ToList();
            return rets.Count > 0 ? rets[0] : null;
        }

        public List<SpiltMDBResult> GetCurrCuttingNextTasks(DateTime planDate)
        {
            DateTime date = planDate.Date;
            var repository = new SpiltMDBResultRepository();
            var rets = repository.FindAll(item =>
                item.PlanDate.Date == date &&
                item.FinishedStatus >= Convert.ToInt32(FinishedStatus.NeedToSaw) &&
                item.FinishedStatus <= Convert.ToInt32(FinishedStatus.LoadedMaterial)).ToList();
            return rets;
        }

        public List<SpiltMDBResult> GetConvertingTasks()
        {
            var repository = new SpiltMDBResultRepository();
            return repository.QueryProcedure("GetConvertingTasks", null).ToList();
        }

        public List<SpiltMDBResult> GetNeedToConvertTasks()
        {
            var repository = new SpiltMDBResultRepository();
            return repository.QueryProcedure("GetNeedToConvertTasks", null).ToList();
        }

        public List<SpiltMDBResult> GetCurrCuttingTasks(string deviceName, DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"EXEC GetCurrCuttingTasks @PlanDate='{date:yyyy-MM-dd}',@DeviceName='{deviceName}'";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public int BulkInsertSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults)
        {
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            return repository.BulkInsert(spiltMdbResults);
        }

        public bool BulkUpdateDeviceInfos(List<DeviceInfos> deviceInfos)
        {
            if(deviceInfos.Count==0)
            {
                return true;
            }
            //var deviceInfo = deviceInfos[0];
            string ExeProcedureSql = "DECLARE @return_value int EXEC @return_value=[dbo].[UpdateDeviceInfo] @DeviceName=@DeviceName,@DepartmentId=@DepartmentID,@ProductionLine=@ProductionLine,@State=@State," +
                "@MSg=@Msg,@Remark=@Remark,@ProcessName=@ProcessName,@LineId=@LineId,@DeviceType=@DeviceType,@DeviceDescription=@DeviceDescription SELECT @return_value";
            DeviceInfosRepository repository = new DeviceInfosRepository(); 
            var conn = repository.Connection;
            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            var tran = conn.BeginTransaction();
            try
            {
                deviceInfos.ForEach(item => conn.Execute(ExeProcedureSql, item, tran, 7200, CommandType.Text));
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

        public List<AllTask> GetAllTasks(DateTime planDate)
        {
            AllTaskRepository repository = new AllTaskRepository();
            string sql = $"EXEC GetAllTask @PlanDate='{planDate.Date:yyyy-MM-dd}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<CuttingPattern> GetCuttingPatterns(string batchName, string itemName, DateTime planDate)
        {
            CuttingPatternRepository repository = new CuttingPatternRepository();
            return repository.FindAll(item =>
                item.ItemName == itemName && item.BatchName == batchName && item.PlanDate == planDate.Date).ToList();
        }

        public List<CuttingPattern> GetCuttingPatternsByPlanDate(DateTime planDate)
        {
            CuttingPatternRepository repository = new CuttingPatternRepository();
            return repository.FindAll(item =>item.PlanDate == planDate.Date).ToList();
        }

        public bool BulkInsertCuttingPatterns(List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingPatterns == null || cuttingPatterns.Count == 0) return true;
            CuttingPatternRepository repository = new CuttingPatternRepository();
            CuttingPattern cp = null;
            string insertSql = string.Format("INSERT INTO [dbo].[{0}] WITH(TABLOCK) ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}]) VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9})",
                nameof(CuttingPattern),nameof(cp.ItemName), nameof(cp.BatchName), nameof(cp.TaskDistributeId), nameof(cp.PlanDate), nameof(cp.BookCount), nameof(cp.CutMaxCount), nameof(cp.Cycles), nameof(cp.PartCount), nameof(cp.PatternName));
            return repository.ExecuteSql(insertSql, cuttingPatterns);
        }

        public bool BulkUpdateCuttingPatternsPartCount(List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingPatterns == null || cuttingPatterns.Count == 0) return true;
            CuttingPatternRepository repository = new CuttingPatternRepository();
            CuttingPattern cp = null;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                nameof(CuttingPattern), nameof(cp.PartCount), nameof(cp.NewPatternName),nameof(cp.LineId));
            return repository.ExecuteSql(updateSql, cuttingPatterns);
        }

        public bool UpdateSpiltMDBResult(SpiltMDBResult spiltMdbResult)
        {
            if (spiltMdbResult == null) return true;
            var rep = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format(
                "UPDATE [dbo].{0} SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5} WHERE [{6}]=@{6} AND [{7}]=@{7}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus), nameof(smr.Msg),
                nameof(smr.EstimatedTime), nameof(smr.MDBFullName), nameof(smr.StackListId),nameof(smr.PlanDate),nameof(smr.ItemName));
            return rep.ExecuteSql(updateSql, spiltMdbResult);
        }

        public bool BulkUpdateFinishedStatus(List<SpiltMDBResult> spiltMdbResults)
        {
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            var con = repository.Connection;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            string UpdateSql =
                "EXEC UpdateSpiltMDBResult @LineId=@LineId,@DeviceName=@DeviceName,@FinishedStatus=@FinishedStatus";
            var tran = con.BeginTransaction();
            try
            {
                con.Execute(UpdateSql, spiltMdbResults, tran);
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
                con.Close();
            }
            //return repository.BulkUpdate(spiltMDBResults);
        }

        public bool BulkUpdateMdbStatus(List<SpiltMDBResult> spiltMdbResults)
        {
            if (spiltMdbResults == null || spiltMdbResults.Count == 0) return true;
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1} WHERE [{2}]=@{2}",
                nameof(SpiltMDBResult),  nameof(smr.MdbStatus), nameof(smr.LineId));
            return repository.ExecuteSql(updateSql, spiltMdbResults);
        }

        public bool BulkUpdateTaskAndMdbStatus(List<SpiltMDBResult> spiltMdbResults)
        {
            if (spiltMdbResults == null || spiltMdbResults.Count == 0) return true;
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3} WHERE [{4}]=@{4}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus), nameof(smr.MdbStatus),nameof(smr.DeviceName), nameof(smr.LineId));
            return repository.ExecuteSql(updateSql, spiltMdbResults);
        }

        public List<CuttingStackList> GetCuttingStackLists(DateTime planTime,bool itemNameIsNull)
        {
            DateTime date = planTime.Date;
            CuttingStackList csl;
            string sql = "";
            if (itemNameIsNull)
            {
                 sql = $@"SELECT * FROM [dbo].[{nameof(CuttingStackList)}] cut WHERE cut.[{nameof(csl.PlanDate)}]='{date}' AND cut.[{nameof(csl.ItemName)}] IS NULL";
            }
            else
            {
                sql = $@"SELECT * FROM [dbo].[{nameof(CuttingStackList)}] cut WHERE cut.[{nameof(csl.PlanDate)}]='{date}' AND cut.[{nameof(csl.ItemName)}] IS NOT NULL";
            }
            var repository = new CuttingStackListRepository();
            var list = repository.QueryList(sql, null).ToList();
            return list;
        }

        public bool BulkUpdatedCuttingStackLists(List<CuttingStackList> cuttingStackLists)
        {
            if (cuttingStackLists == null || cuttingStackLists.Count == 0) return true;
            var rep = new CuttingStackListRepository();
            CuttingStackList csl;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1} WHERE [{2}]=@{2}",
                nameof(CuttingStackList), nameof(csl.StackName), nameof(csl.LineID));
            return rep.ExecuteSql(updateSql, cuttingStackLists);
        }

        public bool BulkUpdatedStackInfos(List<CuttingStackList> cuttingStackLists, List<SpiltMDBResult> spiltMdbResults, List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingStackLists == null || cuttingStackLists.Count == 0 || spiltMdbResults==null||spiltMdbResults.Count==0 || cuttingPatterns==null || cuttingPatterns.Count==0) return true;
            var stackListRepository = new CuttingStackListRepository();
            var mdbRepository = new SpiltMDBResultRepository();
            var cuttingPatternRepository = new CuttingPatternRepository();
            var stackDbConnection = stackListRepository.Connection;
            var mdbDbConnection = mdbRepository.Connection;
            var patternDbConnection = cuttingPatternRepository.Connection;
            if(stackDbConnection.State == ConnectionState.Closed) stackDbConnection.Open();
            if(mdbDbConnection.State == ConnectionState.Closed) mdbDbConnection.Open();
            if(patternDbConnection.State == ConnectionState.Closed) patternDbConnection.Open();
            var stackDbTransaction = stackDbConnection.BeginTransaction();
            var mdbDbTransaction = mdbDbConnection.BeginTransaction();
            var patternDbTransaction = patternDbConnection.BeginTransaction();

            CuttingPattern cp = null;
            CuttingStackList csl;
            SpiltMDBResult smr;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                nameof(CuttingStackList), nameof(csl.ItemName),nameof(csl.StackIndex), nameof(csl.LineID));
            var batchNames = spiltMdbResults.GroupBy(item => item.BatchName).ToList().ConvertAll(item => item.Key);
            string deleteMdbSql =
                $"DELETE FROM [dbo].[{nameof(SpiltMDBResult)}] WHERE [{nameof(smr.BatchName)}] in ('{string.Join("','", batchNames)}')";
            string patternInsertSql = string.Format("INSERT INTO [dbo].[{0}] WITH(TABLOCK) ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}],[{10}],[{11}]) VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11})",
                nameof(CuttingPattern), nameof(cp.ItemName), nameof(cp.BatchName), nameof(cp.TaskDistributeId), nameof(cp.PlanDate), nameof(cp.BookCount), nameof(cp.CutMaxCount), nameof(cp.Cycles), nameof(cp.PartCount), nameof(cp.PatternName),nameof(cp.CycleTime),nameof(cp.TotalTime));
            string mdbInsertSql = string.Format(
                "INSERT INTO [dbo].{0} ({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) VALUES(@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11},@{12})",
                nameof(SpiltMDBResult), nameof(smr.BatchName), nameof(smr.DeviceName), nameof(smr.FinishedStatus),
                nameof(smr.ItemName), nameof(smr.Msg), nameof(smr.PlanDate), nameof(smr.TaskDistributeId),
                nameof(smr.TaskId),nameof(smr.BatchIndex),nameof(smr.ItemIndex),nameof(smr.ActualPlanDate),nameof(smr.StartLoadingTime));
            try
            {
                stackDbConnection.Execute(updateSql, cuttingStackLists, stackDbTransaction, 7200, CommandType.Text);
                mdbDbConnection.Execute(deleteMdbSql, null, mdbDbTransaction, 7200, CommandType.Text);
                mdbDbConnection.Execute(mdbInsertSql, spiltMdbResults, mdbDbTransaction, 7200, CommandType.Text);
                patternDbConnection.Execute(patternInsertSql, cuttingPatterns, patternDbTransaction, 7200, CommandType.Text);
                stackDbTransaction.Commit();
                mdbDbTransaction.Commit();
                patternDbTransaction.Commit();
                mdbDbConnection.Execute("EXEC [WorkStation].[dbo].[PushTaskToDeviceName]");
                return true;

            }
            catch (Exception e)
            {
                stackDbTransaction.Rollback();
                mdbDbTransaction.Rollback();
                patternDbTransaction.Rollback();
                return false;
            }
            finally
            {
                stackDbConnection.Close();
                mdbDbConnection.Close();
                patternDbConnection.Close();
            }
        }

        public DataTable GetTaskPartsInfo(SpiltMDBResult taskSpiltMdbResult)
        {
            var access = new AccessDb(taskSpiltMdbResult.MDBFullName);
            var dt = access.GetData("PARTS_UDI");
            access.Dispose();
            return dt;
        }

        public List<DeviceInfos> GetCuttingDeviceInfos()
        {
            DeviceInfosRepository repository = new DeviceInfosRepository();
            string sql = $"EXEC GetCuttingDeviceInfos";
            //string SelectSql = @"SELECT * FROM DeviceInfos";
            return repository.QueryList(sql, null).ToList();
        }

        public List<DeviceInfos> GetDeviceInfosByProcessName(string processName)
        {
            string selectSql = $@"SELECT * FROM DeviceInfos WHERE DeviceInfos.ProcessName='{processName}'";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            return repository.QueryList(selectSql, null).ToList();
        }

        public List<SpiltMDBResult> GetSpiltMDBResultsNoDevice(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[PlanDate]='{date}' AND spiltMdb.[DeviceName] IS NULL";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public List<SpiltMDBResult> GetCanPushSpiltMDBResults(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $"EXEC GetCanPushTask @PlanDate='{date:yyyy-MM-dd}'";
            //string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[PlanDate]='{date}' AND spiltMdb.[FinishedStatus]={Convert.ToInt32(FinishedStatus.MdbLoaded)}";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public List<SpiltMDBResult> GetDeviceMDBResults(DateTime planTime, string deviceName)
        {
            DateTime date = planTime.Date;
            //string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[PlanDate]='{date}' AND (spiltMdb.[DeviceId]={DeviceId} OR spiltMdb.[DeviceId] IS NULL)";
            SpiltMDBResult smr;
            string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[{nameof(smr.PlanDate)}]='{date}' AND (spiltMdb.[{nameof(smr.DeviceName)}]='{deviceName}')";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public DataSet GetMDBDatas(string itemName)
        {
            var repository = new CuttingStackListRepository();
            var Command = repository.Connection.CreateCommand();
            Command.CommandText = "GetMDBInfos";
            Command.CommandType = CommandType.StoredProcedure;
            var parameter = Command.CreateParameter();
            parameter.ParameterName = "@ItemName";
            parameter.Value = itemName;
            Command.Parameters.Add(parameter);
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)Command);
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
            return ds;
        }

        public List<SpiltMDBResult> GetSpiltMDBResults(DateTime planTime)
        {
            DateTime date = planTime.Date;
            string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[PlanDate]='{date}'";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(sql, null).ToList();

            return list;
        }

        public bool InsertSpiltMDBResult(SpiltMDBResult spiltMdbResult)
        {
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();

            string InsertSql = @"DECLARE @return_value int EXEC [dbo].[InsertSpiltMDBResult] @TaskDistributeId=@TaskDistributeId
      ,@TaskId=@TaskId
      ,@ItemName=@ItemName
      ,@PlanDate=@PlanDate
      ,@StackListId=@StackListId
      ,@BatchName=@BatchName
      ,@MDBFullName=@MDBFullName
      ,@Msg=@Msg
      ,@EstimatedTime=@EstimatedTime
      ,@ActuallyTime=@ActuallyTime
      ,@CreatedTime=@CreatedTime
      ,@UpdateTime=@UpdateTime
      ,@DeviceName=@DeviceName
      ,@FinishedStatus=@FinishedStatus SELECT @return_value";
            return repository.ExecuteSql(InsertSql, spiltMdbResult);
        }

        public bool BulkUpdateSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults)
        {
            if (spiltMdbResults == null || spiltMdbResults.Count==0) return true;
            var rep = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format(
                "UPDATE [dbo].{0} SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4} ,[{5}]=@{5} WHERE [{6}]=@{6}  AND [{7}]=@{7}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus),  nameof(smr.Msg),
                nameof(smr.EstimatedTime), nameof(smr.StartLoadingTime),nameof(smr.DeviceName), nameof(smr.ItemName),nameof(smr.LineId));
            return rep.ExecuteSql(updateSql, spiltMdbResults);
        }

        //public bool UpdateSpiltMDBResult(SpiltMDBResult spiltMDBResult)
        //{
        //    SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
        //    return repository.Update(spiltMDBResult);
        //}
    }
}
