using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Mdb
{
    public class MdbStarter : IModularStarter
    {
        private readonly string _mdbPath = Path.Combine(Directory.GetCurrentDirectory(), "MDBs");
        private ILogger _log = LogManager.GetLogger(typeof(MdbStarter).Name);

        private readonly ILineControlCuttingContractPlus _lineControlCuttingContract = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();//new LineControlCuttingService();
        Thread _thread = null;
        //private Thread _syncDataThread = null;
        public void Start()
        {
            //Thread.Sleep(10000);
            LogManager.AddLoggerAdapter(new Log.Implement.Log4Net.Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(MdbStarter).Name);
            _log.Info("创建日志成功");
            _thread = new Thread(CallBack);
            _thread.IsBackground = true;
            _thread.Start();

            //syncDataThread = new Thread(SyncData);
            //syncDataThread.IsBackground = true;
            //syncDataThread.Start();
            _log.Info("Mdb服务启动成功");
        }

        public void Stop()
        {
            _thread.Abort();
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
                _lineControlCuttingContract.SyncPartFeedBack(deviceName);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }
            
        }


        public void CallBack()
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

                        var tDataSet = dataSet.Copy();
                        mdbParse.Status = Convert.ToInt32(MdbParseStatus.Parsing);
                        mdbParse.UpdatedTime = DateTime.Now;
                        _lineControlCuttingContract.BulkUpdatedMdbParses(new List<MdbParse>() { mdbParse });
                        var tuples = MdbOperator.GetPatternsDatas(dataSet, true);
                        short ptnIndex = 0;

                        var tBatchName = MdbOperator.GetTitle(tDataSet);
                        //MdbRebuild.Rebuild(tDataSet);
                        MdbOperator.UpdatedJobName(tDataSet, tBatchName);
                        MdbOperator.UpdateDatas(tDataSet, out string tmpMdbFileName);

                        
                        var newMdbFileName = Path.Combine(_mdbPath, planDate.ToString("yyyy-MM-dd"), string.IsNullOrWhiteSpace(tBatchName)? mdbParse.BatchId: tBatchName + ".mdb");
                        

                        if (!Directory.Exists(Path.GetDirectoryName(newMdbFileName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newMdbFileName));
                        }
                        if (File.Exists(newMdbFileName)) File.Delete(newMdbFileName);
                        File.Move(tmpMdbFileName, newMdbFileName);

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
                                var color = patternAllInfo.PatternInfo.MaterialCode;
                                var mdbName = batchName + ptnIndex.ToString("000");
                                MdbRebuild.Rebuild(t.Item2);
                                MdbOperator.UpdatedJobName(t.Item2, mdbName);
                                MdbOperator.UpdateDatas(t.Item2, out string tmpFileName);
                                var newFileName = Path.Combine(_mdbPath, planDate.ToString("yyyy-MM-dd"), batchName, color, batchName + ptnIndex.ToString("000") + ".mdb");

                                _log.Info("Mdb路径:"+newFileName);
                                if (!Directory.Exists(Path.GetDirectoryName(newFileName)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
                                }
                                if (File.Exists(newFileName)) File.Delete(newFileName);
                                File.Move(tmpFileName, newFileName);
                                //var color = patternAllInfo.PatternInfo.MaterialCode;
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

        //public void Test()
        //{
        //    var patterns =_lineControlCuttingContract.GetPatternsByBatchName("HB_RX200108A1-5(1)");
        //    var patternDetails = _lineControlCuttingContract.GetPatternDetailsByBatchName("HB_RX200108A1-5(1)");
        //    var pattern = patterns.First(item => item.PatternId == 14);
        //    var tmpPatterns = patternDetails.FindAll(item => item.PatternId == 14);
        //    DisintegratedMdb(pattern, tmpPatterns, patterns.Max(item => item.PatternId), 2);
        //}


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


    }
}
