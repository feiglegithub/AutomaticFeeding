using Dapper;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Repository;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public partial class LineControlCuttingService
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
            insertSqlBuilder.AppendFormat("INSERT INTO DeviceInfos ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}) " +
                                          "VALUES (@{0},@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9})",
                nameof(deviceInfo.DepartmentId), nameof(deviceInfo.DeviceName), nameof(deviceInfo.ProcessName), 
                nameof(deviceInfo.ProductionLine), nameof(deviceInfo.Remark), nameof(deviceInfo.State), nameof(deviceInfo.Msg),
                nameof(deviceInfo.DeviceType), nameof(deviceInfo.DeviceDescription),nameof(deviceInfo.PlaceNo));
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

        public List<AllTask> GetAllTasks(DateTime planDate)
        {
            AllTaskRepository repository = new AllTaskRepository();
            string sql = $"EXEC GetAllTask @PlanDate='{planDate.Date:yyyy-MM-dd}'";
            return repository.QueryList(sql, null).ToList();
        }

        public List<AllTask> GetAllTasks_Test()
        {
            AllTaskRepository repository = new AllTaskRepository("Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;");
            string sql = $"SELECT * FROM AllTask";
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
            return repository.FindAll(item => item.PlanDate == planDate.Date).ToList();
        }

        public bool BulkInsertCuttingPatterns(List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingPatterns == null || cuttingPatterns.Count == 0) return true;
            CuttingPatternRepository repository = new CuttingPatternRepository();
            CuttingPattern cp = null;
            string insertSql = string.Format("INSERT INTO [dbo].[{0}] WITH(TABLOCK) ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}]) VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9})",
                nameof(CuttingPattern), nameof(cp.ItemName), nameof(cp.BatchName), nameof(cp.TaskDistributeId), nameof(cp.PlanDate), nameof(cp.BookCount), nameof(cp.CutMaxCount), nameof(cp.Cycles), nameof(cp.PartCount), nameof(cp.PatternName));
            return repository.ExecuteSql(insertSql, cuttingPatterns);
        }

        public bool BulkUpdateCuttingPatternsPartCount(List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingPatterns == null || cuttingPatterns.Count == 0) return true;
            CuttingPatternRepository repository = new CuttingPatternRepository();
            CuttingPattern cp = null;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                nameof(CuttingPattern), nameof(cp.PartCount), nameof(cp.NewPatternName), nameof(cp.LineId));
            return repository.ExecuteSql(updateSql, cuttingPatterns);
        }

        public bool CommitTaskError(string taskName)
        {
            var rep = new CuttingTaskDetailRepository();
            CuttingTaskDetail ctd;
            string store = "CommitErrorTask";
            return rep.ExecuteProcedure(store, new {ItemName = taskName});
            //var a = new { IsNg = true, PartFinishedStatus = true };
            //string sql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1} ,[{2}]=@{2}"
            //    , nameof(CuttingTaskDetail), nameof(ctd.IsNg), nameof(ctd.PartFinishedStatus));
            //return rep.ExecuteSql(sql, a);
        }

        public bool BulkDeleteSpiltMDBResults(List<SpiltMDBResult> spiltMdbResults)
        {
            if (spiltMdbResults == null || spiltMdbResults.Count == 0) return true;
            var repository = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string deleteSql = string.Format("DELETE [dbo].[{0}] WHERE [{1}]=@{1}", nameof(SpiltMDBResult),
                nameof(smr.LineId));
            return repository.ExecuteSql(deleteSql, spiltMdbResults);
        }

        public bool UpdateSpiltMDBResult(SpiltMDBResult spiltMdbResult)
        {
            if (spiltMdbResult == null) return true;
            var rep = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format(
                "UPDATE [dbo].{0} SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5} WHERE [{6}]=@{6} AND [{7}]=@{7}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus), nameof(smr.Msg),
                nameof(smr.EstimatedTime), nameof(smr.MDBFullName), nameof(smr.StackListId), nameof(smr.PlanDate), nameof(smr.ItemName));
            return rep.ExecuteSql(updateSql, spiltMdbResult);
        }

        public bool UpdateSpiltMDBFullPath(SpiltMDBResult spiltMdbResult)
        {
            if (spiltMdbResult == null) return true;
            var rep = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format(
                "UPDATE [dbo].{0} SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3} WHERE [{4}]=@{4} AND [{5}]=@{5}",
                nameof(SpiltMDBResult), nameof(smr.MDBFullName),nameof(smr.MdbStatus),nameof(smr.StackListId), nameof(smr.PlanDate), nameof(smr.ItemName));
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
                nameof(SpiltMDBResult), nameof(smr.MdbStatus), nameof(smr.LineId));
            return repository.ExecuteSql(updateSql, spiltMdbResults);
        }

        public bool BulkUpdateTaskAndMdbStatus(List<SpiltMDBResult> spiltMdbResults)
        {
            if (spiltMdbResults == null || spiltMdbResults.Count == 0) return true;
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3} WHERE [{4}]=@{4}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus), nameof(smr.MdbStatus), nameof(smr.DeviceName), nameof(smr.LineId));
            return repository.ExecuteSql(updateSql, spiltMdbResults);
        }

        public bool BulkSaveCuttingCheckRules(List<CuttingCheckRule> insertCheckRules, List<CuttingCheckRule> deleteCheckRules, List<CuttingCheckRule> updateCheckRules)
        {
            if (ListIsNullOrNoNumber(insertCheckRules) && ListIsNullOrNoNumber(deleteCheckRules) &&
                ListIsNullOrNoNumber(updateCheckRules)) return true;
            return false;
        }

        //private DataTable CreatedCheckRuleDataTable()
        //{
        //    CuttingCheckRule ccr;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(nameof(ccr.LineId), typeof(long));
        //    dt.Columns.Add(nameof(ccr.Args),)
        //}

        private bool ListIsNullOrNoNumber<T>(IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public List<CuttingStackList> GetCuttingStackLists(DateTime planTime)
        {
            DateTime date = planTime.Date;
            CuttingStackList csl;
            string sql = "";

            sql = $@"SELECT * FROM [dbo].[{nameof(CuttingStackList)}] cut WHERE cut.[{nameof(csl.PlanDate)}]='{date}' ";//AND cut.[{nameof(csl.ItemName)}] IS NOT NULL";
 
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

        private List<WMSCuttingStackList> ConvertToWmsCuttingStackLists(List<CuttingStackList> cuttingStackLists)
        {
            List<WMSCuttingStackList> wmsCuttingStackLists = new List<WMSCuttingStackList>();
            foreach (var cuttingStackList in cuttingStackLists)
            {
                WMSCuttingStackList wms = new WMSCuttingStackList()
                {
                    BatchName = cuttingStackList.BatchName,
                    Information = cuttingStackList.Information,
                    OptimizationRun = cuttingStackList.OptimizationRun,
                    PatternName = cuttingStackList.PatternName,
                    WorkshopCode = cuttingStackList.WorkshopCode,
                    StackIndex = cuttingStackList.StackIndex,
                    StackName = cuttingStackList.ItemName,
                    RawColor = cuttingStackList.RawColor,
                    RawLength = cuttingStackList.RawLength,
                    RawMaterialID = cuttingStackList.RawMaterialID,
                    RawMaterialName = cuttingStackList.RawMaterialName,
                    RawOrientation = (int)cuttingStackList.RawOrientation,
                    RawThickness = cuttingStackList.RawThickness,
                    RawWidth = cuttingStackList.RawWidth,
                    PlanDate = cuttingStackList.PlanDate.Date,
                    ProductionLine = cuttingStackList.ProductionLine,
                    TaskDistributeId = cuttingStackList.TaskDistributeId,
                    TaskId = cuttingStackList.TaskId,
                    StackType = cuttingStackList.StackType,
                    SupplierInfo = cuttingStackList.SupplierInfo,
                    ImportToMesStatus = (int)cuttingStackList.ImportToMesStatus,
                    BatchId = cuttingStackList.BatchId,
                    CreatedTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now,
                };
                wmsCuttingStackLists.Add(wms);

            }

            return wmsCuttingStackLists;
        }

        public bool BulkUpdatedStackInfos(List<CuttingStackList> cuttingStackLists, List<SpiltMDBResult> spiltMdbResults, List<CuttingPattern> cuttingPatterns)
        {
            if (cuttingStackLists == null || cuttingStackLists.Count == 0 || spiltMdbResults == null || spiltMdbResults.Count == 0 || cuttingPatterns == null || cuttingPatterns.Count == 0) return true;
            List<WMSCuttingStackList> wmsCuttingStackLists = ConvertToWmsCuttingStackLists(cuttingStackLists);

            wmsCuttingStackLists.ForEach(wmsCuttingStackList =>
            {
                var result = spiltMdbResults.FirstOrDefault(item => item.ItemName == wmsCuttingStackList.StackName);
                wmsCuttingStackList.BatchIndex = result == null ? 0 : result.BatchIndex;
            });


            List<List<WMSCuttingStackList> > lst = new List<List<WMSCuttingStackList>>();
            foreach (var group in wmsCuttingStackLists.GroupBy(item=>item.StackName))
            {
                var tmpList = group.ToList();
                int stackPartCount = tmpList.Count+1;
                tmpList.ForEach(item=>item.StackIndex=stackPartCount-item.StackIndex);
                tmpList.Sort((x, y) => x.StackIndex.CompareTo(y.StackIndex));
                lst.Add(tmpList);
            }
            var stackListRepository = new CuttingStackListRepository();
            var mdbRepository = new SpiltMDBResultRepository();
            var cuttingPatternRepository = new CuttingPatternRepository();
            var wmsCuttingStackListRepository = new WMSCuttingStackListRepository();

            var stackDbConnection = stackListRepository.Connection;
            var mdbDbConnection = mdbRepository.Connection;
            var patternDbConnection = cuttingPatternRepository.Connection;
            var wmsDbConnection = wmsCuttingStackListRepository.Connection;

            if (stackDbConnection.State == ConnectionState.Closed) stackDbConnection.Open();
            if (mdbDbConnection.State == ConnectionState.Closed) mdbDbConnection.Open();
            if (patternDbConnection.State == ConnectionState.Closed) patternDbConnection.Open();
            if (wmsDbConnection.State == ConnectionState.Closed) wmsDbConnection.Open();

            var stackDbTransaction = stackDbConnection.BeginTransaction();
            var mdbDbTransaction = mdbDbConnection.BeginTransaction();
            var patternDbTransaction = patternDbConnection.BeginTransaction();
            var wmsDbTransaction = wmsDbConnection.BeginTransaction();

            CuttingPattern cp = null;
            CuttingStackList csl;
            SpiltMDBResult smr;
            WMSCuttingStackList wms;
            string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                nameof(CuttingStackList), nameof(csl.ItemName), nameof(csl.StackIndex), nameof(csl.LineID));
            var batchNames = spiltMdbResults.GroupBy(item => item.BatchName).ToList().ConvertAll(item => item.Key);
            string deleteMdbSql =
                $"DELETE FROM [dbo].[{nameof(SpiltMDBResult)}] WHERE [{nameof(smr.BatchName)}] in ('{string.Join("','", batchNames)}')";
            string patternInsertSql = string.Format("INSERT INTO [dbo].[{0}] WITH(TABLOCK) ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}],[{10}],[{11}]) VALUES (@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11})",
                nameof(CuttingPattern), nameof(cp.ItemName), nameof(cp.BatchName), nameof(cp.TaskDistributeId), nameof(cp.PlanDate), nameof(cp.BookCount), nameof(cp.CutMaxCount), nameof(cp.Cycles), nameof(cp.PartCount), nameof(cp.PatternName), nameof(cp.CycleTime), nameof(cp.TotalTime));
            string wmsInsertSql = string.Format(
                "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}],[{10}]" +
                ",[{11}],[{12}],[{13}],[{14}],[{15}],[{16}],[{17}],[{18}],[{19}],[{20}],[{21}],[{22}],[{23}]) " +
                "VALUES(@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11}" +
                ",@{12},@{13},@{14},@{15},@{16},@{17},@{18},@{19},@{20},@{21},@{22},@{23})", nameof(WMSCuttingStackList),
                nameof(wms.BatchName), nameof(wms.Information), nameof(wms.OptimizationRun), nameof(wms.PatternName)
                , nameof(wms.WorkshopCode), nameof(wms.StackIndex), nameof(wms.StackName)
                , nameof(wms.RawColor), nameof(wms.RawLength), nameof(wms.RawMaterialID)
                , nameof(wms.RawMaterialName), nameof(wms.RawOrientation), nameof(wms.RawThickness)
                , nameof(wms.RawWidth), nameof(wms.PlanDate), nameof(wms.ProductionLine)
                , nameof(wms.TaskDistributeId), nameof(wms.TaskId), nameof(wms.StackType)
                , nameof(wms.SupplierInfo), nameof(wms.ImportToMesStatus), nameof(wms.BatchId),nameof(wms.BatchIndex));
            string mdbInsertSql = string.Format(
                "INSERT INTO [dbo].{0} ({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) VALUES(@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10},@{11})",
                nameof(SpiltMDBResult), nameof(smr.BatchName), nameof(smr.DeviceName), nameof(smr.FinishedStatus),
                nameof(smr.ItemName), nameof(smr.Msg), nameof(smr.PlanDate), nameof(smr.TaskDistributeId),
                nameof(smr.TaskId), nameof(smr.BatchIndex), nameof(smr.ItemIndex), nameof(smr.ActualPlanDate));
            try
            {
                mdbDbConnection.Execute(deleteMdbSql, null, mdbDbTransaction, 7200, CommandType.Text);

                mdbDbTransaction.Commit();

                mdbDbTransaction = mdbDbConnection.BeginTransaction();
                stackDbConnection.Execute(updateSql, cuttingStackLists, stackDbTransaction, 7200, CommandType.Text);
                
                mdbDbConnection.Execute(mdbInsertSql, spiltMdbResults, mdbDbTransaction, 7200, CommandType.Text);
                
                patternDbConnection.Execute(patternInsertSql, cuttingPatterns, patternDbTransaction, 7200, CommandType.Text);
                foreach (var item in lst)
                {
                    //数据量大，分段插入
                    int insertCount = 40;
                    for (int i = 0; i < item.Count; i += insertCount)
                    {
                        List<WMSCuttingStackList> data = null;
                        if (i + insertCount > item.Count)
                        {
                            data = item.GetRange(i, item.Count - i);
                        }
                        else
                        {
                            data = item.GetRange(i, insertCount);
                        }

                        wmsDbConnection.Execute(wmsInsertSql, data, wmsDbTransaction, 7200, CommandType.Text);
                    }


                }
                //lst.ForEach(item=> wmsCuttingStackListRepository.BulkInsert(item, wmsDbTransaction));
                stackDbTransaction.Commit();
                mdbDbTransaction.Commit();
                patternDbTransaction.Commit();
                wmsDbTransaction.Commit();
                //mdbDbConnection.Execute("EXEC [dbo].[PushTaskToDeviceName]");
                return true;

            }
            catch (Exception e)
            {
                stackDbTransaction.Rollback();
                mdbDbTransaction.Rollback();
                patternDbTransaction.Rollback();
                wmsDbTransaction.Rollback();
                return false;
            }
            finally
            {
                stackDbConnection.Close();
                mdbDbConnection.Close();
                patternDbConnection.Close();
                wmsDbConnection.Close();
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
            string selectSql = $"SELECT * FROM DeviceInfos WHERE DeviceInfos.ProcessName='{processName}'";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            return repository.QueryList(selectSql, null).ToList();
        }

        public List<DeviceInfos> GetDeviceInfosByPlaceNo(string placeNo)
        {
            string selectSql = $"SELECT * FROM DeviceInfos WHERE DeviceInfos.PlaceNo='{placeNo}'";
            DeviceInfosRepository repository = new DeviceInfosRepository();
            return repository.QueryList(selectSql, null).ToList();
        }

        public List<DeviceInfos> GetEnableCuttingDeviceInfos()
        {
            DeviceInfosRepository repository = new DeviceInfosRepository();
            string sql = $"SELECT * FROM DeviceInfos devInfo WHERE DeviceType = 'CuttingMachine' and State = 1";
            return repository.QueryList(sql, null).ToList();
        }

        public List<SpiltMDBResult> GetMdbUnCreatedTasks()
        {
            SpiltMDBResultRepository repository = new SpiltMDBResultRepository();
            string sql = "EXEC GetMdbUnCreatedTasks";
            return repository.QueryList(sql, null).ToList();
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
            ds.Tables[10].TableName = "HEADER";
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
            if (spiltMdbResults == null || spiltMdbResults.Count == 0) return true;
            var rep = new SpiltMDBResultRepository();
            SpiltMDBResult smr;
            string updateSql = string.Format(
                "UPDATE [dbo].{0} SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5},[{6}]=@{6} WHERE [{9}]=@{9} AND [{7}]=@{7} AND [{8}]=@{8}",
                nameof(SpiltMDBResult), nameof(smr.FinishedStatus), nameof(smr.Msg),
                nameof(smr.EstimatedTime), nameof(smr.StartLoadingTime), nameof(smr.StackListId),nameof(smr.DeviceName), nameof(smr.PlanDate), nameof(smr.ItemName), nameof(smr.BatchName));
            return rep.ExecuteSql(updateSql, spiltMdbResults);
        }

        public List<SpiltMDBResult> GetDeviceTopUnDownLoad(int topNum, string deviceName)
        {
            if (topNum <= 0) return new List<SpiltMDBResult>();
            SpiltMDBResult smr;

            string selectSql = string.Format("SELECT * FROM [dbo].[{0}] WHERE [{1}]='{2}' AND [{3}] " +
                                             "IN (SELECT TOP {4} [{3}] FROM [dbo].[{0}] WHERE [{1}]='{2}'" +
                                             "AND [{5}]<{6} AND [{5}]>={8} GROUP BY [{3}],[{7}],[{9}] ORDER BY [{7}],[{9}])",
                nameof(SpiltMDBResult)
                , nameof(smr.DeviceName), deviceName, nameof(smr.BatchName), topNum, nameof(smr.FinishedStatus)
                , Convert.ToInt32(FinishedStatus.Cut), nameof(smr.ActualPlanDate), Convert.ToInt32(FinishedStatus.NeedToSaw), nameof(smr.BatchIndex));
            //string selectSql = string.Format("SELECT * FROM [dbo].[{0}] WHERE [{1}]='{2}' AND [{3}] " +
            //                                 "IN (SELECT TOP {4} [{3}] FROM [dbo].[{0}] WHERE [{1}]='{2}'" +
            //                                 "AND [{5}]<{6} AND [{8}]>={9} GROUP BY [{3}],[{7}] ORDER BY [{7}])",
            //    nameof(SpiltMDBResult)
            //    , nameof(smr.DeviceName), deviceName, nameof(smr.BatchName), topNum, nameof(smr.FinishedStatus)
            //    , Convert.ToInt32(FinishedStatus.Cut), nameof(smr.ActualPlanDate), nameof(smr.FinishedStatus), Convert.ToInt32(FinishedStatus.NeedToSaw));
            //DateTime date = topNum.Date;
            //string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[PlanDate]='{date}' AND (spiltMdb.[DeviceId]={DeviceId} OR spiltMdb.[DeviceId] IS NULL)";

            //string sql = $@"SELECT * FROM [SpiltMDBResult] spiltMdb WHERE spiltMdb.[{nameof(smr.PlanDate)}]='{date}' AND (spiltMdb.[{nameof(smr.DeviceName)}]='{deviceName}')";
            var repository = new SpiltMDBResultRepository();
            var list = repository.QueryList(selectSql, null).ToList();

            return list;
        }

        public bool BulkInsertCuttingPartScanLog(List<CuttingPartScanLog> cuttingPartScanLogs)
        {
            if (cuttingPartScanLogs == null || cuttingPartScanLogs.Count == 0) return true;
            var rep = new CuttingPartScanLogRepository();
            CuttingPartScanLog cps;
            DataTable dt = new DataTable();
            dt.Columns.Add(nameof(cps.PartId), typeof(string));
            dt.Columns.Add(nameof(cps.BatchName), typeof(string));
            dt.Columns.Add(nameof(cps.InteractionPoints), typeof(string));
            dt.Columns.Add(nameof(cps.PartType), typeof(string));
            dt.Columns.Add(nameof(cps.Result), typeof(string));
            dt.Columns.Add(nameof(cps.Thickness), typeof(float));
            dt.Columns.Add(nameof(cps.Length), typeof(float));
            dt.Columns.Add(nameof(cps.Width), typeof(float));
            
            foreach (var cuttingPartScan in cuttingPartScanLogs)
            {
                dt.Rows.Add(cuttingPartScan.PartId, cuttingPartScan.BatchName,cuttingPartScan.InteractionPoints,cuttingPartScan.PartType,cuttingPartScan.Result,cuttingPartScan.Thickness,cuttingPartScan.Length,cuttingPartScan.Width);
            }
            string store = "InsertScanPartWithLength";

            try
            {
                rep.Connection.ExecuteScalar(store, new {CuttingScanPart = dt}, null, null, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception e)
            {
                throw;
                //return false;
            }
        }

        public List<PartInfo> GetPartInfosByPartId(string partId)
        {
            PartInfoRepository rep = new PartInfoRepository();
            return rep.FindAll(item => item.PartId == partId).ToList();
            
        }

        public void SyncCuttingFeedBackData(string deviceName)
        {
            var rep = new CuttingPartScanLogRepository();
            string store = "GetCuttingFeedBack" + deviceName.Replace("0-240-07-", "");
            rep.ExecuteSql($"EXEC [dbo].[{store}]", null);
        }



        public List<CuttingFeedBack> GetCuttingFeedBacksByPartId(string partId)
        {
            CuttingFeedBackRepositiory rep = new CuttingFeedBackRepositiory();
            return rep.FindAll(item => item.PartId == partId).ToList();
        }

        public List<CuttingFeedBack> GetCuttingFeedBacksByBatchName(string batchName)
        {
            CuttingFeedBackRepositiory rep = new CuttingFeedBackRepositiory();
            return rep.FindAll(item => item.BatchName == batchName).ToList();
        }
    }
}
