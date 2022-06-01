using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Domain.Model;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.MDB
{
    public class MDBStarter : IModularStarter
    {
        private readonly string MDBPath = Path.Combine(Directory.GetCurrentDirectory(), "MDBs");
        private ILogger _log = LogManager.GetLogger(typeof(MDBStarter).Name);
        private bool flag = false;

        //private readonly IWorkStationContract _lineControlCuttingContract =/* ServiceLocator.Current.GetInstance<IWorkStationContract>(); //*/new WorkStationService();
        private readonly ILineControlCuttingContract _lineControlCuttingContract = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();//new LineControlCuttingService();
        Thread thread = null;
        private Thread syncDataThread = null;
        public void Start()
        {
            //Thread.Sleep(10000);
            LogManager.AddLoggerAdapter(new Log.Implement.Log4Net.Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(MDBStarter).Name);
            _log.Info("创建日志成功");
            thread = new Thread(CallBackNew);
            thread.IsBackground = true;
            thread.Start();

            //syncDataThread = new Thread(SyncData);
            //syncDataThread.IsBackground = true;
            //syncDataThread.Start();
            _log.Info("服务启动成功");
        }

        public void Stop()
        {
            thread.Abort();
            //syncDataThread.Abort();
            //timer.Dispose();
        }

        public StarterLevel Level { get; }

        private void SyncData()
        {
            while (true)
            {
                SyncDeviceData("8014");
                SyncDeviceData("8015");
                SyncDeviceData("8016");
                SyncDeviceData("8017");
                SyncDeviceData("8018");
                Thread.Sleep(1000);
            }
        }

        private void SyncDeviceData(string deviceName)
        {
            try
            {
                _lineControlCuttingContract.SyncCuttingFeedBackData(deviceName);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }
            
        }


        public void CallBackNew()
        {
            while (true)
            {
                try
                {
                    var mdbParses = _lineControlCuttingContract.GetMdbParses();
                    mdbParses = mdbParses.FindAll(item => item.Status < Convert.ToInt32(MdbParseStatus.Parsed));
                    foreach (var mdbParse in mdbParses)
                    {
                        DataSet dataSet = _lineControlCuttingContract.GetMdbDataByBatchId(mdbParse.BatchId);
                        DateTime planDate = mdbParse.PlanDate;//DateTime.Parse("2020-02-20").Date;
                                                              //var patternAllInfos1 = MdbOperator.GetPatternAllInfos(dataSet);
                        mdbParse.Status = Convert.ToInt32(MdbParseStatus.Parsing);
                        mdbParse.UpdatedTime = DateTime.Now;
                        _lineControlCuttingContract.BulkUpdatedMdbParses(new List<MdbParse>() { mdbParse });
                        var tuples = MdbOperator.GetPatternsDatas(dataSet, true);
                        short ptnIndex = 0;
                        List<Pattern> patterns = new List<Pattern>();
                        List<PatternDetail> patternDetails = new List<PatternDetail>();
                        foreach (var tuple in tuples)
                        {
                            //对每个锯切图叠板拆解
                            var ts = MdbOperator.MdbFormatConverter(tuple.Item2);
                            foreach (var t in ts)
                            {
                                ptnIndex += 1;
                                //提取拆解的每个锯切图的板件信息
                                var patternAllInfos = MdbOperator.GetPatternAllInfos(t.Item2);
                                int oldPtnIndex = t.Item1;
                                var success = MdbOperator.UpdatePtnIndex(t.Item2, 1);
                                var patternAllInfo = patternAllInfos[0];
                                var batchName = patternAllInfo.PatternInfo.BatchName;
                                var mdbName = batchName + ptnIndex.ToString("000");
                                MdbRebuild.Rebuild(t.Item2);
                                MdbOperator.UpdatedTitle(t.Item2, mdbName);
                                MdbOperator.UpdateDatas(t.Item2, out string tmpFileName);
                                var newFileName = Path.Combine(MDBPath, planDate.ToString("yyyy-MM-dd"), batchName, batchName + ptnIndex.ToString("000") + ".mdb");
                                if (!Directory.Exists(Path.GetDirectoryName(newFileName)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
                                }
                                if (File.Exists(newFileName)) File.Delete(newFileName);
                                File.Move(tmpFileName, newFileName);
                                var color = patternAllInfo.PatternInfo.MaterialCode;
                                Pattern patternList = new Pattern()
                                {
                                    BatchName = batchName,
                                    BookCount = patternAllInfo.PatternInfo.TotalBookCount,
                                    Color = color,
                                    FileFullPath = newFileName,
                                    IsEnable = true,
                                    IsNodePattern = false,
                                    MdbName = mdbName,
                                    Status = 0,
                                    CreatedTime = DateTime.Now,
                                    UpdatedTime = DateTime.Now,
                                    PatternId = ptnIndex,
                                    OldPatternId = oldPtnIndex,
                                    TotallyTime = patternAllInfo.PatternInfo.TotalTime,
                                    PartCount = patternAllInfo.PatternInfo.NormalPartCount,
                                    OffPartCount = patternAllInfo.PatternInfo.OffPartCount,
                                    PlanDate = planDate,
                                };
                                patterns.Add(patternList);
                                foreach (var workpieceInfo in patternAllInfo.WorkpieceInfos)
                                {
                                    PatternDetail patternDetail = new PatternDetail()
                                    {
                                        CreatedTime = DateTime.Now,
                                        UpdatedTime = DateTime.Now,
                                        BatchName = batchName,
                                        Color = color,
                                        IsFinished = false,
                                        IsOffPart = workpieceInfo.IsOffPart,
                                        Length = Convert.ToSingle(workpieceInfo.Length),
                                        Width = Convert.ToSingle(workpieceInfo.Width),
                                        Thickness = Convert.ToSingle(workpieceInfo.Thickness),
                                        PatternId = ptnIndex,
                                        OldPatternId = oldPtnIndex,
                                        PartCount = workpieceInfo.PartCount,
                                        FinishedPartCount = 0,
                                        PartId = workpieceInfo.PartId
                                    };
                                    patternDetails.Add(patternDetail);
                                }
                            }
                        }

                        _lineControlCuttingContract.BulkInsertPatterns(patterns);
                        _lineControlCuttingContract.BulkInsertPatternDetails(patternDetails);
                        mdbParse.Status = Convert.ToInt32(MdbParseStatus.Parsed);
                        mdbParse.UpdatedTime = DateTime.Now;
                        _lineControlCuttingContract.BulkUpdatedMdbParses(new List<MdbParse>() { mdbParse });
                    }
                    Thread.Sleep(200);
                }
                catch (Exception e)
                {
                    _log.Error("解析mdb异常",e);
                }
                
            }
            
        }

        public void Test()
        {
            var patterns =_lineControlCuttingContract.GetPatternsByBatchName("HB_RX200108A1-5(1)");
            var patternDetails = _lineControlCuttingContract.GetPatternDetailsByBatchName("HB_RX200108A1-5(1)");
            var pattern = patterns.First(item => item.PatternId == 14);
            var tmpPatterns = patternDetails.FindAll(item => item.PatternId == 14);
            DisintegratedMdb(pattern, tmpPatterns, patterns.Max(item => item.PatternId), 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern">锯切图</param>
        /// <param name="patternDetails">锯切图明细</param>
        /// <param name="curMaxPatternId">当前锯切图的最大Id</param>
        /// <param name="maxBook">最大叠板数</param>
        public bool DisintegratedMdb(Pattern pattern,List<PatternDetail> patternDetails, int curMaxPatternId,int maxBook)
        {
            AccessDB db = new AccessDB(pattern.FileFullPath);
            List<Pattern> patterns = new List<Pattern>();
            List<PatternDetail> newOffPartDetails = new List<PatternDetail>();
            List<PatternDetail> oldOffPartDetails = patternDetails.FindAll(item=>item.IsOffPart);
            try
            {
                var ds = db.GetDatas();
                db.Dispose();

                var tuples = MdbOperator.MdbFormatConverter(ds, maxBook);
                List<Tuple< PatternAllInfo ,DataSet>> ts = new List<Tuple<PatternAllInfo,DataSet>>();
                foreach (var tuple in tuples)
                {
                    List<PatternAllInfo> allInfos = MdbOperator.GetPatternAllInfos(tuple.Item2);
                    var patternInfo = allInfos[0];
                    var success = MdbOperator.UpdatePtnIndex(tuple.Item2, 1);
                    MdbRebuild.Rebuild(tuple.Item2);
                    
                    ts.Add(new Tuple<PatternAllInfo, DataSet>(patternInfo,tuple.Item2));
                }

                //var ts = SpiltMdb(ds, maxBook);
                foreach (var t in ts)
                {
                    //var tmpFilePath = t.Item2;
                    
                    var patternAllInfo = t.Item1;
                    var ptnIndex = ++curMaxPatternId;
                    var batchName = patternAllInfo.PatternInfo.BatchName;
                    var mdbName = batchName + ptnIndex.ToString("000");
                    MdbOperator.UpdatedTitle(t.Item2, mdbName);
                    MdbOperator.UpdateDatas(t.Item2, out string tmpFileName);

                    var newFileName = Path.Combine(MDBPath, pattern.PlanDate.ToString("yyyy-MM-dd"), batchName, batchName + ptnIndex.ToString("000") + ".mdb");
                    File.Move(tmpFileName, newFileName);
                    var color = patternAllInfo.PatternInfo.MaterialCode;
                    Pattern tmpPattern = new Pattern()
                    {
                        BatchName = batchName,
                        BookCount = patternAllInfo.PatternInfo.TotalBookCount,
                        Color = color,
                        FileFullPath = newFileName,
                        IsEnable = true,
                        IsNodePattern = true,
                        MdbName = mdbName,
                        Status = 0,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        PatternId = ptnIndex,
                        OldPatternId = pattern.PatternId,
                        TotallyTime = patternAllInfo.PatternInfo.TotalTime,
                        PartCount = patternAllInfo.PatternInfo.NormalPartCount,
                        OffPartCount = patternAllInfo.PatternInfo.OffPartCount,
                        PlanDate = pattern.PlanDate,
                        PlanStackName = pattern.PlanStackName
                    };
                    patterns.Add(tmpPattern);
                    //存在余料板

                    var offParts = t.Item1.WorkpieceInfos.FindAll(item => item.IsOffPart);
                    foreach (var workpieceInfo in offParts)
                    {
                        PatternDetail patternDetail = new PatternDetail()
                        {
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now,
                            BatchName = batchName,
                            Color = color,
                            IsFinished = false,
                            IsOffPart = workpieceInfo.IsOffPart,
                            Length = Convert.ToSingle(workpieceInfo.Length),
                            Width = Convert.ToSingle(workpieceInfo.Width),
                            Thickness = Convert.ToSingle(workpieceInfo.Thickness),
                            PatternId = ptnIndex,
                            OldPatternId = pattern.OldPatternId,
                            PartCount = workpieceInfo.PartCount,
                            FinishedPartCount = 0,
                            PartId = workpieceInfo.PartId
                        };
                        newOffPartDetails.Add(patternDetail);
                    }

                    
                    //更新板件对应的锯切图Id
                    var tmpList = patternDetails.FindAll(item =>
                        item.IsOffPart == false && t.Item1.WorkpieceInfos.Exists(i => i.PartId == item.PartId));
                    tmpList.ForEach(item=>item.PatternId=curMaxPatternId);
                }

                pattern.IsEnable = false;
                _lineControlCuttingContract.BulkDeletePatternDetails(oldOffPartDetails);
                _lineControlCuttingContract.BulkInsertPatternDetails(newOffPartDetails);
                _lineControlCuttingContract.BulkUpdatePatternDetails(patternDetails);
                _lineControlCuttingContract.BulkUpdatePatterns(new List<Pattern>() {pattern});
                _lineControlCuttingContract.BulkInsertPatterns(patterns);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private List<Tuple<PatternAllInfo,string>> SpiltMdb(DataSet ds,int maxBook)
        {
            var tuples = MdbOperator.MdbFormatConverter(ds, maxBook);
            List<Tuple< PatternAllInfo ,string>> ts = new List<Tuple<PatternAllInfo, string>>();
            foreach (var tuple in tuples)
            {
                List<PatternAllInfo> allInfos = MdbOperator.GetPatternAllInfos(tuple.Item2);
                var patternInfo = allInfos[0];
                var success = MdbOperator.UpdatePtnIndex(tuple.Item2, 1);
                //MdbOperator.UpdatedTitle(t.Item2, batchName + ptnIndex.ToString("000"));
                MdbRebuild.Rebuild(tuple.Item2);
                MdbOperator.UpdateDatas(tuple.Item2, out string tmpFileName);

                ts.Add(new Tuple<PatternAllInfo, string>(patternInfo,tmpFileName));
            }

            return ts;
        }

        private void CallBack(object planDate)
        {
            while (true)
            {
                try
                {
                    List<SpiltMDBResult> spiltMDBResults = _lineControlCuttingContract.GetMdbUnCreatedTasks();
                    if (spiltMDBResults.Count == 0)
                    {
                        Thread.Sleep(10000);
                        continue;
                    }

                    DateTime date = spiltMDBResults[0].PlanDate;
                    List<CuttingStackList> cuttingStackList = _lineControlCuttingContract.GetCuttingStackLists(date);
                    if (spiltMDBResults.Count > 0 && cuttingStackList.Count > 0)
                    {
                        //获取当前日期上一次生成的MDB文件的数据在 CuttingStackList 表中的最大行号
                        var SpildMDBMaxId = spiltMDBResults.Max(item => item.StackListId);
                        //获取当前日期 CuttingStackList 表中的最大行号
                        var StackListTableMaxId = cuttingStackList.Max(item => item.LineID);
                        //_log.Info($"SpildMDBMaxId ={SpildMDBMaxId}    StackListTableMaxId = {StackListTableMaxId}");
                        if (SpildMDBMaxId < StackListTableMaxId) //有批次更新
                        {
                            //获取更新数据
                            cuttingStackList = GetUpdateInfos(cuttingStackList, spiltMDBResults);
                            _log.Debug("有批次更新");
                            flag = false;
                        }
                        else
                        {
                            cuttingStackList.Clear();
                        }
                    }
                    else if (spiltMDBResults.Count > 0 && cuttingStackList.Count == 0)
                    {
                        //CuttingStackList表（PlanDate的数据已经被外部意外修改删除）
                        _log.Debug("CuttingStackList表（PlanDate的数据已经被外部意外修改删除）");
                    }
                    else if (spiltMDBResults.Count == 0 && cuttingStackList.Count == 0)
                    {
                        //CuttingStatckList表：该天暂无计划生产数据
                        _log.Debug("CuttingStatckList表：该天暂无计划生产数据");
                    }

                    if (cuttingStackList.Count == 0)
                    {
                        if (!flag)
                        {
                            _log.Debug("无最新数据");
                            flag = true;
                        }
                    }
                    else
                    {
                        SpiltMDBInfos(cuttingStackList, spiltMDBResults);
                        //var cuttingDeviceInfos = _workStationContract.GetCuttingDeviceInfos();
                        //var spiltMdbResults = _workStationContract.GetSpiltMDBResultsNoDevice(date);
                        //CuttingTaskArithmetic.AssigningTask(spiltMdbResults, cuttingDeviceInfos);
                        //_workStationContract.BulkUpdateFinishedStatus(spiltMdbResults);
                    }

                    #region 分配任务
                    //var cuttingDeviceInfos = _workStationContract.GetCuttingDeviceInfos();
                    //var spiltMdbResults = _workStationContract.GetSpiltMDBResultsNoDevice(date);
                    //if (spiltMdbResults.Count > 0)
                    //{
                    //    CuttingTaskArithmetic.AssigningTask(spiltMdbResults, cuttingDeviceInfos);
                    //    _workStationContract.BulkUpdateFinishedStatus(spiltMdbResults);
                    //}
                    #endregion

                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                    _log.Error(e.StackTrace);
                }

                Thread.Sleep(10000);
                
            }
        }

        private List<CuttingStackList> GetUpdateInfos(List<CuttingStackList> cuttingStackLists,List<SpiltMDBResult> spiltMDBResults)
        {
            //更新的最小颗粒度为批次
            var resultGroups = spiltMDBResults.GroupBy(item => item.TaskDistributeId);
            var stackListGroups = cuttingStackLists.GroupBy(item => item.TaskDistributeId);
            var resultDic = new Dictionary<Guid, long>();
            var stackListDic = new Dictionary<Guid?, long>();
            foreach (var group in resultGroups)
            {
                var maxStackListId = group.Max(item => item.StackListId);

                resultDic.Add(group.Key, maxStackListId);
            }

            foreach (var group in stackListGroups)
            {
                if (group.Key == null)
                {
                    continue;
                }
                var maxLineID = group.Max(item => item.LineID);
                stackListDic.Add(group.Key, maxLineID);
            }

            foreach (var item in resultDic)
            {
                if (item.Value == stackListDic[item.Key])
                {
                    cuttingStackLists.RemoveAll(stackListItem => stackListItem.TaskDistributeId == item.Key);
                }
            }

            return cuttingStackLists;
        }

        /// <summary>
        ///     获取MDB的所有数据进行拆分重组
        /// </summary>
        /// <param name="cuttingStackList"></param>
        /// <param name="spiltMDBResults"></param>
        private void SpiltMDBInfos(List<CuttingStackList> cuttingStackList, List<SpiltMDBResult> spiltMDBResults)
        {
            cuttingStackList.RemoveAll(item => !spiltMDBResults.Exists(smr => smr.ItemName == item.ItemName));
            var groups = cuttingStackList.GroupBy(item => item.ItemName).ToList();
            var datas = new List<Tuple<string, DataSet, SpiltMDBResult>>();
            foreach (var group in groups)
            {
                var ItemName = group.Key;
                DataSet ds = null;
                ds = _lineControlCuttingContract.GetMDBDatas(ItemName);
                
                var stackItem = group.ToList()[0];
                SpiltMDBResult result = spiltMDBResults.Find(item => item.ItemName == stackItem.ItemName);
                foreach (DataRow row in ds.Tables["JOBS"].Rows)
                {
                    row["NAME"] = result.ItemName;
                }
                //SpiltMDBResult result = new SpiltMDBResult();
                //result.BatchName = stackItem.BatchName;
                //result.ItemName = stackItem.ItemName;
                //result.PlanDate = (DateTime)stackItem.PlanDate;
                result.StackListId = group.Max(item => item.LineID); //stackItem.LineID;
                //result.TaskDistributeId = (Guid)stackItem.TaskDistributeId;
                //result.TaskId = stackItem.TaskId;
                var mdbName = Path.Combine(MDBPath, result.PlanDate.ToString("yyyy-MM-dd"),result.BatchName, ItemName + ".mdb");
                result.MDBFullName = mdbName;
                //result.FinishedStatus = Convert.ToInt32(FinishedStatus.MdbUnloaded);
                datas.Add(new Tuple<string,  DataSet, SpiltMDBResult>(mdbName,  ds, result));
            }
            var splitMDB = new SplitMDB();
            datas.Sort((x, y) => x.Item3.StackListId.CompareTo(y.Item3.StackListId));
            
            foreach (var tuple in datas)
            {
                var result = tuple.Item3;
                var cuttingPatterns =
                    _lineControlCuttingContract.GetCuttingPatterns(result.BatchName, result.ItemName, result.PlanDate);
                foreach (var cuttingPattern in cuttingPatterns)
                {
                    cuttingPattern.PartCount = tuple.Item2.Tables["CUTS"]
                        .Select(string.Format("PTN_INDEX={0} ", cuttingPattern.PatternName))
                        .Sum(row => Convert.ToInt32(row["QTY_PARTS"]));
                }
                var tuplePTN_INDEX = SpiltArithmetic.Rebuild(tuple.Item2, cuttingStackList.FindAll(item=>item.ItemName==result.ItemName&&item.BatchName==result.BatchName));//重构MDB数据
                
                result.EstimatedTime = tuplePTN_INDEX.Item1;
                //设置mdb文件状态为生成中
                result.MdbStatus = Convert.ToInt32(FinishedStatus.MdbCreating);
                _lineControlCuttingContract.BulkUpdateMdbStatus(new List<SpiltMDBResult>() {result});
                splitMDB.UpdateDatas(tuple.Item1,tuple.Item2);
                
                //_workStationContract.InsertSpiltMDBResult(result);
                _lineControlCuttingContract.UpdateSpiltMDBFullPath(result);
                _log.Debug(result.BatchName + "->" + result.ItemName + "MDB生成成功");

                _log.Debug(result.BatchName + "->" + result.ItemName + "开始生成板件明细");
                var cuttingTaskDetails = CreatedCuttingTaskDetails(tuple.Item2, result,tuplePTN_INDEX.Item2);
                _log.Debug(result.BatchName + "->" + result.ItemName + "板件明细生成完毕");
                _lineControlCuttingContract.BulkInsertTaskDetails(cuttingTaskDetails);
                foreach (var keyValue in tuplePTN_INDEX.Item2)
                {
                    cuttingPatterns.Find(item => item.PatternName == keyValue.Value).NewPatternName =Convert.ToInt16(keyValue.Key);
                }
                _log.Debug(result.BatchName + "->" + result.ItemName + "开始更新新旧mdb的锯切图关联");
                _lineControlCuttingContract.BulkUpdateCuttingPatternsPartCount(cuttingPatterns);
                _log.Debug(result.BatchName + "->" + result.ItemName + "更新新旧mdb的锯切图关联成功");
                
                result.MdbStatus = Convert.ToInt32(FinishedStatus.MdbCreated);
                _log.Debug(result.BatchName + "->" + result.ItemName + "开始更新mdb为生成完成状态");
                _lineControlCuttingContract.BulkUpdateMdbStatus(new List<SpiltMDBResult>() { result });
                _log.Debug(result.BatchName + "->" + result.ItemName + "更新mdb为生成完成状态成功");
                result.MdbStatus = Convert.ToInt32(FinishedStatus.MdbUnloaded);
                _log.Debug(result.BatchName + "->" + result.ItemName + "开始更新mdb为未下载状态");
                _lineControlCuttingContract.BulkUpdateMdbStatus(new List<SpiltMDBResult>() { result });
                _log.Debug(result.BatchName + "->" + result.ItemName + "更新mdb为未下载状态成功");
                //if (string.IsNullOrWhiteSpace(result.DeviceName))
                //{
                //    //result.FinishedStatus = Convert.ToInt32(FinishedStatus.Undistrbuted);
                //    _workStationContract.BulkUpdateTaskAndMdbStatus(new List<SpiltMDBResult>() { result });
                //}
                //else
                //{
                //    _workStationContract.BulkUpdateMdbStatus(new List<SpiltMDBResult>() { result });
                //}

            }
            
        }

        private List<CuttingTaskDetail> CreatedCuttingTaskDetails(DataSet ds, SpiltMDBResult result,Dictionary<int,int> dic)
        {
            var cutsTable = ds.Tables["CUTS"];
            Dictionary<int, int> partIndex_PtnIndex = new Dictionary<int, int>();
            //Dictionary<int, int> offPartIndex_PtnIndex = new Dictionary<int, int>();
            foreach (DataRow row in cutsTable.Rows)
            {
                int QTY_PARTS = Convert.ToInt32(row["QTY_PARTS"].ToString().Trim());
                if (QTY_PARTS < 1) continue;

                string partIndex = row["PART_INDEX"].ToString().Trim();
                int PTN_INDEX = Convert.ToInt32(row["PTN_INDEX"].ToString().Trim());
                if (partIndex.Contains("X"))
                {
                    var offPartIndex = Convert.ToInt32(partIndex.Replace("X", ""));
                    //if (!offPartIndex_PtnIndex.ContainsKey(offPartIndex))
                    //{
                    //    offPartIndex_PtnIndex.Add(offPartIndex, PTN_INDEX);
                    //}
                }
                else
                {
                    partIndex_PtnIndex.Add(Convert.ToInt32(partIndex), PTN_INDEX);
                }

            }

            var data = from pRow in ds.Tables["PATTERNS"].AsEnumerable()
                join bRow in ds.Tables["BOARDS"].AsEnumerable()
                    on new {JOB_INDEX = pRow["JOB_INDEX"].ToString(), BRD_INDEX = pRow["BRD_INDEX"].ToString()}
                    equals new {JOB_INDEX = bRow["JOB_INDEX"].ToString(), BRD_INDEX = bRow["BRD_INDEX"].ToString()}
                join mRow in ds.Tables["MATERIALS"].AsEnumerable() 
                    on new {JOB_INDEX = bRow["JOB_INDEX"].ToString(), MAT_INDEX = bRow["MAT_INDEX"].ToString()} 
                    equals new {JOB_INDEX = mRow["JOB_INDEX"].ToString(), MAT_INDEX = mRow["MAT_INDEX"].ToString()}
                select new
                {
                    PTN_INDEX = int.Parse(pRow["PTN_INDEX"].ToString()),
                    MAT_INDEX = int.Parse(mRow["MAT_INDEX"].ToString()),
                    BRD_INDEX = int.Parse(pRow["BRD_INDEX"].ToString()),
                    JOB_INDEX = int.Parse(pRow["JOB_INDEX"].ToString()),
                    CODE = mRow["CODE"].ToString().Trim()
                };

            var group = from da in data
                group da by new {da.PTN_INDEX, da.CODE}
                into g
                select new {g.Key.PTN_INDEX, g.Key.CODE};


            var udiDetails = CreatedCuttingTaskDetails(ds.Tables["PARTS_UDI"], partIndex_PtnIndex, result, dic);

            var dt = ds.Tables["PARTS_REQ"];
            var dtCollection = dt.AsEnumerable();

            var ret = from row in dtCollection
                select new
                {
                    PartId = row["CODE"].ToString().Trim(),
                    Length = Convert.ToDouble(row["LENGTH"]),
                    Width = Convert.ToDouble(row["WIDTH"])
                };
            var tmpList = ret.ToList();
            foreach (var detail in udiDetails)
            {
                var partId = detail.PartId;
                var r = tmpList.FirstOrDefault(item => item.PartId == partId);
                if (r != null)
                {
                    detail.Length = r.Length;
                    detail.Width = r.Width;
                    tmpList.Remove(r);
                }
            }

            var list = udiDetails;
            //var list = udiDetails.Concat(CreatedCuttingTaskDetails(ds.Tables["OFFCUTS"], offPartIndex_PtnIndex, result, dic)).ToList();

            foreach (var da in group)
            {
                list.FindAll(item => da.PTN_INDEX == item.NewPTN_INDEX).ForEach(item=>item.Color = da.CODE);
            }

            return list;
        }

        private List<CuttingTaskDetail> CreatedCuttingTaskDetails(DataTable udiDataTable, Dictionary<int,int> partIndex_PtnIndex,SpiltMDBResult result, Dictionary<int, int> dic)
        {
            
            List<CuttingTaskDetail> cuttingTaskDetails = new List<CuttingTaskDetail>();
            
            if (udiDataTable.TableName == "PARTS_UDI")
            {
               
                foreach (DataRow dr in udiDataTable.Rows)
                {
                    int partIndex = Convert.ToInt32(dr["PART_INDEX"].ToString().Trim());
                    int newPTN_INDEX = partIndex_PtnIndex[partIndex];
                    int oldPTN_INDEX = dic[newPTN_INDEX];
                    cuttingTaskDetails.Add(CreatedCuttingTaskDetail_UDI(dr, result, oldPTN_INDEX, newPTN_INDEX));
                }
            }
            else if (udiDataTable.TableName == "OFFCUTS")
            {
                foreach (DataRow dr in udiDataTable.Rows)
                {
                    int partIndex = Convert.ToInt32(dr["OFC_INDEX"].ToString().Trim());
                    int newPTN_INDEX = partIndex_PtnIndex[partIndex];
                    int oldPTN_INDEX = dic[newPTN_INDEX];
                    cuttingTaskDetails.Add(CreatedCuttingTaskDetail_OFF(dr, result,oldPTN_INDEX,newPTN_INDEX));
                }
            }
            

            return cuttingTaskDetails;
        }

        private CuttingTaskDetail CreatedCuttingTaskDetail_OFF(DataRow dataRow, SpiltMDBResult result, int oldPTN_INDEX, int newPTN_INDEX)
        {
            string offPartId = dataRow["CODE"].ToString();
            offPartId = offPartId.Substring(offPartId.IndexOf('/')+1);
            CuttingTaskDetail cuttingTaskDetail = new CuttingTaskDetail()
            {
                BatchName = result.BatchName,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                DeviceName = result.DeviceName,
                ItemName = result.ItemName,
                PartFinishedStatus = false,
                PlanDate = result.PlanDate,
                TaskEnable = true,
                TaskDistributeId = result.TaskDistributeId,
                PART_INDEX = Convert.ToInt32(dataRow["OFC_INDEX"].ToString())+20000,
                JOB_INDEX = Convert.ToInt32(dataRow["JOB_INDEX"].ToString()),
                PartId = "XHB40"+ offPartId,
                Length = Convert.ToDouble(dataRow["LENGTH"].ToString().Trim()),
                Width = Convert.ToDouble(dataRow["WIDTH"].ToString().Trim()),
                OldPTN_INDEX = oldPTN_INDEX,
                NewPTN_INDEX = newPTN_INDEX,
                IsOffPart=true
            };

            return cuttingTaskDetail;

        }

        private CuttingTaskDetail CreatedCuttingTaskDetail_UDI(DataRow dataRow, SpiltMDBResult result,int oldPTN_INDEX,int newPTN_INDEX)
        {
            CuttingTaskDetail cuttingTaskDetail = new CuttingTaskDetail()
            {
                BatchName = result.BatchName,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                DeviceName = result.DeviceName,
                ItemName = result.ItemName,
                PartFinishedStatus = false,
                PlanDate = result.PlanDate,
                TaskEnable = true,
                TaskDistributeId = result.TaskDistributeId,
                PART_INDEX = Convert.ToInt32(dataRow["PART_INDEX"].ToString()),
                JOB_INDEX = Convert.ToInt32(dataRow["JOB_INDEX"].ToString()),
                PartId = dataRow["INFO30"].ToString(),
                Length = Convert.ToDouble(dataRow["INFO23"].ToString().Trim()),
                Width = Convert.ToDouble(dataRow["INFO24"].ToString().Trim()),
                OldPTN_INDEX = oldPTN_INDEX,
                NewPTN_INDEX = newPTN_INDEX,
                IsOffPart=false
                
            };

            return cuttingTaskDetail;

        }
    }
}
