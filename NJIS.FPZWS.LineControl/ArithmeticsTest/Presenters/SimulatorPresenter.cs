using ArithmeticsTest.Helpers;
using ArithmeticsTest.Services;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace ArithmeticsTest.Presenters
{
    public class SimulatorPresenter:PresenterBase
    {
        public const string BindingData = nameof(BindingData);

        public const string GetData = nameof(GetData);
        public const string BeginDistribute = nameof(BeginDistribute);
        public const string StopDistribute = nameof(StopDistribute);

        public const string Begin = nameof(Begin);

        public const string Stop = nameof(Stop);

        private const string PARTS_UDI = nameof(PARTS_UDI);

        private const string QTY_PARTS = nameof(QTY_PARTS);

        private const string PART_INDEX = nameof(PART_INDEX);

        private const string CUTS = nameof(CUTS);

        

        private List<string> Devices { get; set; } = new List<string>();
        private string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");

        private readonly object DictionaryLock = new object();

        private Dictionary<string, TmpClass> Dictionary { get; }= new Dictionary<string, TmpClass>();


        private List<TmpControl> TmpControls { get; set; } = new List<TmpControl>();

        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract Contract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = WcfClient.GetProxy<ILineControlCuttingContract>());

        private static SimulatorPresenter _simulatorPresenter = null;
        public static SimulatorPresenter GetInstance()
        {
            if (_simulatorPresenter == null)
            {
                lock (Stop)
                {
                    if (_simulatorPresenter == null)
                    {
                        _simulatorPresenter = new SimulatorPresenter();
                    }
                }
            }

            return _simulatorPresenter;
        }

        private LocalService local = LocalService.GetInstance();

        private Thread thread = null;
        private bool IsRun { get; set; } = false;

        private SimulatorPresenter()
        {
            Register();
            Init();
            
        }

        private void Remove(string deviceName)
        {
            lock (DictionaryLock)
            {
                Dictionary.Remove(deviceName);
            }
        }


        private void Init()
        {
            Devices.Add("1#");
            Devices.Add("2#");
            Devices.Add("3#");
            Devices.Add("4#");
            Devices.Add("5#");
            Devices.ForEach(device=>
            {
                TmpControls.Add(new TmpControl() {DeviceName = device, IsStart = false});
                Dictionary.Add(device,null);
            });

        }

        private List<PartFeedBack> InsertPartFeedBacks { get; set; } = new List<PartFeedBack>();
        private object _partFeedBackLock = new object();
        private void AddPartFeedBack(PartFeedBack partFeedBack)
        {
            lock (_partFeedBackLock)
            {
                InsertPartFeedBacks.Add(partFeedBack);
            }
        }

        private void Clear()
        {
            lock (_partFeedBackLock)
            {
                InsertPartFeedBacks.Clear();
            }
        }

        private void AutoInsert()
        {
            while (true)
            {
                if (InsertPartFeedBacks.Count > 0)
                {
                    lock (_partFeedBackLock)
                    {
                        Contract.BulkInsertPartFeedBacks(InsertPartFeedBacks);
                        InsertPartFeedBacks.Clear();
                    }
                }
                Thread.Sleep(200);
            }
        }

        //private void Run()
        //{
        //    while (true)
        //    {
        //        foreach (var tmpControl in TmpControls)
        //        {
        //            if (!tmpControl.IsStart)
        //            {
        //                Thread.Sleep(20);
        //                continue;
        //            }

        //            var deviceName = tmpControl.DeviceName;

        //            if (!Dictionary.ContainsKey(deviceName))
        //            {
        //                var patterns = Contract.GetPatternsByDevice(deviceName, PatternStatus.Loaded);
        //                if (patterns.Count > 0)
        //                {
        //                    var pattern = patterns[0];
        //                    var file = Path.Combine(_mdbDownLoadPath, pattern.PlanDate.ToString("yyyy-MM-dd"),
        //                        pattern.BatchName, pattern.MdbName + ".mdb");
        //                    var accessDb = new AccessDb(file);
        //                    var dataSet = accessDb.GetDatas();
        //                    accessDb.Dispose();
        //                    Dictionary.Add(deviceName, new TmpClass {DeviceName = deviceName,CurrentIndex = 0, DataSet = dataSet, BatchName = pattern.BatchName, Color = pattern.Color, TotalTime = pattern.TotallyTime });
        //                }
        //                else
        //                {
        //                    Thread.Sleep(20);
        //                    continue;
        //                }
        //            }

        //            var tmpClass = Dictionary[deviceName];
        //            var cuts = tmpClass.DataSet.Tables[CUTS];
        //            var dataRow = cuts.Rows[tmpClass.CurrentIndex];
        //            var partIndexStr = dataRow[PART_INDEX].ToString();
        //            var partCount = Convert.ToInt32(dataRow[QTY_PARTS]);
        //            if (partCount > 0)
        //            {
        //                if (!partIndexStr.Contains("X"))
        //                {
        //                    var partIndex = Convert.ToInt32(partIndexStr);
        //                    var parts = tmpClass.DataSet.Tables[PARTS_UDI];
        //                    string partId = "";
        //                    foreach (DataRow dr in parts.Rows)
        //                    {
        //                        if (Convert.ToInt32(dr[PART_INDEX]) == partIndex)
        //                        {
        //                            partId = dr["INFO30"].ToString();
        //                            break;
        //                        }
        //                    }

        //                    Contract.BulkInsertPartFeedBacks(new List<PartFeedBack>()
        //                    {
        //                        new PartFeedBack()
        //                        {
        //                            BatchName = tmpClass.BatchName, Color = tmpClass.Color, CreatedTime = DateTime.Now,
        //                            UpdatedTime = DateTime.Now, DeviceName = deviceName,
        //                            Status = Convert.ToInt32(PartFeedBackStatus.UnFinished), DevicePPCD_ID = 0,
        //                            Length = 0, PartId = partId, Thickness = 0, Width = 0
        //                        }
        //                    });
        //                }
        //            }

        //            var tt = (tmpClass.TotalTime * 1.0 / tmpClass.DataSet.Tables[CUTS].Rows.Count);
        //            Thread.Sleep(Convert.ToInt32(5 * tt));
        //            tmpClass.CurrentIndex += 1;
        //            this.Send(BindingData, tmpClass);
        //            if (tmpClass.CurrentIndex == tmpClass.DataSet.Tables[CUTS].Rows.Count)
        //            {
        //                Remove(deviceName);
        //            }
        //        }

        //        Thread.Sleep(20);

        //    }
        //}


        int times = 50;
        private void Register()
        {
            this.Register<int>(BeginDistribute,(sender,speed)=>
            {
                times = speed;
                local.BeginSwap(times);
                this.Send(BindingData,true);
            });
            this.Register<string>(StopDistribute, (sender, str) =>
            {
                local.StopSwap();
                this.Send(BindingData, true);
            });

            this.Register<string>(Begin, (sender, deviceName) =>
            {
                ExecuteBase(this,sender,deviceName, (tSender, tDeviceName) =>
                {
                    var t = TmpControls.FirstOrDefault(item => item.DeviceName == deviceName);
                    if (t == null || t.IsStart) return;
                    t.IsStart = true;
                    if (!IsRun)
                    {
                        IsRun = true;
                        //thread.Start();
                    }

                    if (thread == null)
                    {
                        thread = new Thread(AutoInsert) { IsBackground = true };
                        thread.Start();
                    }

                    while (true)
                    {
                        var tmpControl = TmpControls.FirstOrDefault(item => item.DeviceName == deviceName);
                        if (tmpControl == null || !tmpControl.IsStart) return;
                        if (Dictionary[deviceName]==null)
                        {
                            var patterns = Contract.GetPatternsByDevice(deviceName, PatternStatus.Loaded);
                            if (patterns.Count > 0)
                            {
                                var pattern = patterns[0];
                                var file = Path.Combine(_mdbDownLoadPath, pattern.PlanDate.ToString("yyyy-MM-dd"),
                                    pattern.BatchName, pattern.MdbName + ".mdb");
                                var accessDb = new AccessDb(file);
                                var dataSet = accessDb.GetDatas();
                                accessDb.Dispose();
                                lock (DictionaryLock)
                                {
                                    Dictionary[deviceName] = new TmpClass
                                    {
                                        DeviceName = deviceName,
                                        CurrentIndex = 0,
                                        DataSet = dataSet,
                                        BatchName = pattern.BatchName,
                                        Color = pattern.Color,
                                        TotalTime = pattern.TotallyTime
                                    };
                                    
                                }
                            }
                            else
                            {
                                Thread.Sleep(20);
                                continue;
                            }
                        }

                        var tmpClass = Dictionary[deviceName];
                        var partTable = tmpClass.DataSet.Tables[PARTS_UDI];

                        var cuts = tmpClass.DataSet.Tables[CUTS];
                        var dataRow = cuts.Rows[tmpClass.CurrentIndex];
                        var partIndexStr = dataRow[PART_INDEX].ToString();
                        var partCount = Convert.ToInt32(dataRow[QTY_PARTS]);
                        var tt = (tmpClass.TotalTime * 1000.0 / partTable.Rows.Count);
                        var rr = Convert.ToInt32(tt / times);

                        var dr = partTable.Rows[tmpClass.CurrentIndex];
                        var partId = dr["INFO30"].ToString();
                        var feedBack = new PartFeedBack()
                        {
                            BatchName = tmpClass.BatchName,
                            Color = tmpClass.Color,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now,
                            DeviceName = deviceName,
                            Status = Convert.ToInt32(PartFeedBackStatus.UnFinished),
                            DevicePPCD_ID = 0,
                            Length = 0,
                            PartId = partId,
                            Thickness = 0,
                            Width = 0
                        };
                        AddPartFeedBack(feedBack);
                        Thread.Sleep(rr);

                        tmpClass.CurrentIndex += 1;
                        this.Send(BindingData, tmpClass);
                        if (tmpClass.CurrentIndex == tmpClass.DataSet.Tables[PARTS_UDI].Rows.Count)
                        {
                            //Remove(deviceName);
                            Dictionary[deviceName] = null;
                            var bs = BatchStops.FirstOrDefault(item =>
                                item.BatchName == tmpClass.BatchName && item.DeviceName == tmpClass.DeviceName);
                            if (bs != null)
                            {
                                if (bs.StopMin > bs.CurrentStopCount)
                                {
                                    bs.CurrentStopCount += 1;
                                    Thread.Sleep(1 * 60 * 1000 / times);
                                }
                            }
                        }
                    }

                });
                
            });
            this.Register<List<BatchStop>>(Begin, (sender, stops) =>
            {
                var updated = BatchStops.Join(stops, bs => new {bs.BatchName, bs.DeviceName},
                    stop => new {stop.BatchName, stop.DeviceName},
                    (bs, stop) => new {BatchStop = bs, Stop = stop}).ToList();
                updated.ForEach(item=>item.BatchStop.StopMin = item.Stop.StopMin);
            });

            this.Register<string>(Stop, (sender, deviceName) =>
            {
                var t = TmpControls.FirstOrDefault(item => item.DeviceName == deviceName);
                if (t == null || !t.IsStart) return;
                t.IsStart = false;
            });

            this.Register<DateTime>(GetData, (sender, planDate) =>
            {
                var patterns = Contract.GetPatternsByPlanDate(planDate); 
                 BatchStops = patterns.GroupBy(item => new {item.BatchName, item.ActualDeviceName}, (key, ps) => new BatchStop
                {
                    BatchName = key.BatchName,
                    DeviceName = key.ActualDeviceName,
                    PatternCount = ps.Count(),
                    StopMin = 0,
                    CurrentStopCount=0
                }).ToList();

                Send(BindingData, sender, BatchStops);
            });
        }

        private List<BatchStop> BatchStops { get; set; } = new List<BatchStop>();

        internal class BatchStop
        {
            public string BatchName { get; set; }
            public string DeviceName { get; set; }
            public int PatternCount { get; set; }
            public int CurrentStopCount { get; set; }
            public int StopMin { get; set; }
        }

        internal class TmpClass
        {
            public DataSet DataSet { get; set; }
            public int CurrentIndex { get; set; }
            public string BatchName { get; set; }
            public string Color { get; set; }
            public int TotalTime { get; set; }
            public string DeviceName { get; set; }
        }

        internal class TmpControl
        {
            public string DeviceName { get; set; }
            public bool IsStart { get; set; } = false;
            public int StopMin { get; set; }

        }
    }
}
