using Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmeticsTest
{
    public partial class ArithmeticsTestForm : Telerik.WinControls.UI.RadForm
    {
        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract= WcfClient.GetProxy<ILineControlCuttingContract>());

        public ArithmeticsTestForm()
        {
            InitializeComponent();
            ViewInit();
        }
        
        private void ViewInit()
        {
            DeviceInfos dis;
            gvbDevice.AddColumns(new List<NJIS.FPZWS.UI.Common.Controls.ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="设备号", FieldName=nameof(dis.DeviceName), DataType=typeof(string), ReadOnly=false },
                //new ColumnInfo(EColumnType.ComboBox){ HeaderText="设备类型", FieldName=nameof(dis.DeviceType)
                //    , DataType=typeof(string), ReadOnly=false,DataSource = new Dictionary<string,string>(){{ "CuttingMachine", "CuttingMachine" } }
                //    ,DisplayMember = "value",ValueMember = "key"},
                //new ColumnInfo(){ HeaderText="部门", FieldName=nameof(dis.DepartmentId), DataType=typeof(int), ReadOnly=false },
                //new ColumnInfo(){ HeaderText="产线", FieldName=nameof(dis.ProductionLine), DataType=typeof(string), ReadOnly=false },
                //new ColumnInfo(){ HeaderText="位置", FieldName=nameof(dis.PlaceNo), DataType=typeof(string), ReadOnly=false },
                new ColumnInfo(){ HeaderText="时间比", FieldName=nameof(dis.Ratio), DataType=typeof(float), ReadOnly=false },
                new ColumnInfo(){ HeaderText="叠板率比", FieldName=nameof(dis.OverlappingRate), DataType=typeof(float), ReadOnly=false },
                new ColumnInfo(){ HeaderText="工件数量比", FieldName=nameof(dis.PartNumRate), DataType=typeof(float), ReadOnly=false },
                new ColumnInfo(){ HeaderText="工件速率比", FieldName=nameof(dis.PartSpeedRate), DataType=typeof(float), ReadOnly=false },
                new ColumnInfo(){ HeaderText="余料数量比", FieldName=nameof(dis.OffPartNumRate), DataType=typeof(float), ReadOnly=false },

                //new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(dis.State)
                //    , DataType=typeof(int), ReadOnly=false,DataSource = new Dictionary<int,string>(){{0,"禁用"},{1,"启用"}}
                //    ,DisplayMember = "value",ValueMember = "key"},
                //new ColumnInfo(){ HeaderText="工段", FieldName=nameof(dis.ProcessName), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(){ HeaderText="设备描述", FieldName=nameof(dis.DeviceDescription), DataType=typeof(string), ReadOnly=false },
                //new ColumnInfo(){ HeaderText="备注", FieldName=nameof(dis.Remark), DataType=typeof(string), ReadOnly=false },
            });

            AllTask allTask;
            gvbTask.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText = "批次",FieldName = nameof(allTask.BatchName),DataType = typeof(string)},
                new ColumnInfo(){ HeaderText = "板材数",FieldName =nameof(allTask.BookNum), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "需求数",FieldName =nameof(allTask.MdbCount), DataType = typeof(int)},
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(allTask.IsError)
                    , DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{false,"正常"},{true,"异常"}},DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){ HeaderText = "板件数",FieldName = nameof(allTask.PartCount),DataType = typeof(int)},

                new ColumnInfo(){ HeaderText = "信息",FieldName =nameof(allTask.Msg), DataType = typeof(string)},
            });
            gvbTask.ShowCheckBox = true;


            DeviceInfoses = new List<DeviceInfos>()
            {
                new DeviceInfos(){DeviceName = "1#",Ratio = 1,LineId = 1,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "2#",Ratio = 1,LineId = 2,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "3#",Ratio = 1,LineId = 3,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "4#",Ratio = 1,LineId = 4,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "5#",Ratio = 1,LineId = 5,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
            };
            gvbDevice.DataSource = DeviceInfoses;
        }

        private List<DeviceInfos> DeviceInfoses { get; set; }

        private List<AllTask> GroupByBatch(List<AllTask> datas)
        {
            List<AllTask> allTasks = new List<AllTask>();
            var groups = datas.GroupBy(item => item.BatchName);
            foreach (var group in groups)
            {
                AllTask allTask = new AllTask()
                {
                    BatchName = group.Key,
                    BookNum = group.Sum(item => item.BookNum),
                    TotalTime = group.Sum(item => item.TotalTime),
                    PartCount = group.Sum(item => item.PartCount),
                    OffPartCount = group.Sum(item => item.OffPartCount),
                    MdbCount = group.Sum(item => item.MdbCount),
                    IsError = group.First().IsError,
                    Msg = group.First().Msg

                };
                allTasks.Add(allTask);
            }

            var a = allTasks.FindAll(item => item.OffPartCount > 0);
            return allTasks;
        }

        private List<AllTask> ConvertTasks(List<AllTask> allTasks)
        {
            int maxCutCount = 4;
            var a = allTasks.FindAll(item => item.OffPartCount > 0);
            var t = allTasks.OrderBy(item => item.CYCLE_TIME).ToList();
            List<AllTask> addAllTasks = new List<AllTask>();
            foreach (var data in allTasks)
            {
                AllTask tmpTask = new AllTask()
                {
                    BatchName = data.BatchName,
                    BookNum = data.BookNum,
                    CYCLE_TIME = data.CYCLE_TIME,
                    CutTimes = data.CutTimes,
                    TaskDistributeId = data.TaskDistributeId,
                    TotalTime = data.CYCLE_TIME,
                    PTN_INDEX = data.PTN_INDEX,
                    RawMaterialID = data.RawMaterialID,
                    PartCount = data.PartCount,
                    OffPartCount = data.OffPartCount
                    
                };
                int count1 = tmpTask.BookNum / maxCutCount;
                int count2 = (tmpTask.BookNum % maxCutCount > 0 ? 1 : 0);
                tmpTask.CutTimes = count1 + count2;
                data.CutTimes = count1 + count2;
                data.TotalTime = data.CutTimes * data.CYCLE_TIME;
                tmpTask.TotalTime = tmpTask.CYCLE_TIME;
                while (tmpTask.CutTimes > 1)
                {
                    AllTask task = new AllTask();
                    
                    task.BatchName = tmpTask.BatchName;
                    task.BookNum = maxCutCount;
                    task.CYCLE_TIME = tmpTask.CYCLE_TIME;
                    task.CutTimes = 1;
                    task.TaskDistributeId = tmpTask.TaskDistributeId;
                    task.TotalTime = tmpTask.CYCLE_TIME;
                    task.PTN_INDEX = tmpTask.PTN_INDEX;
                    task.RawMaterialID = tmpTask.RawMaterialID;
                    task.PartCount = (tmpTask.PartCount / tmpTask.BookNum) * maxCutCount;
                    task.OffPartCount = (tmpTask.OffPartCount / tmpTask.BookNum) * maxCutCount;
                    addAllTasks.Add(task);
                    tmpTask.CutTimes -= 1;
                    tmpTask.BookNum -= task.BookNum;
                    tmpTask.PartCount -= task.PartCount;
                }
                addAllTasks.Add(tmpTask);
            }

            return addAllTasks;
        }

        internal class TmpDeviceInfo:DeviceInfos
        {
            public double ExpectTotalTime { get; set; }
        }

        private Tuple<List<Solution>,List<SolutionDetail>> SpiltTask(List<AllTask> allTasks, List<DeviceInfos> dis,int topCount=5)
        {
            var totalTime = allTasks.Sum(item => item.TotalTime);
            var totalRatio = dis.Sum(item => item.Ratio) * 1.0;

            allTasks = allTasks.OrderBy(item => item.CYCLE_TIME).ToList();

            var tt = allTasks.ConvertAll(item => item.TotalTime);
            var avg = tt.Average();

            var tGroups = allTasks.GroupBy(item => new {item.RawMaterialID, item.TotalTime});

            var c = tGroups.Count();
            
            foreach (var group in tGroups)
            {
                var ptnIndex = group.Min(item => item.PTN_INDEX);
                foreach (var item in group)
                {
                    item.PTN_INDEX = ptnIndex;
                }
            }

            var variance = Variance.VarianceBase(allTasks.ConvertAll(item => item.TotalTime));

            var findTasks = allTasks.FindAll(task => Math.Pow(avg - task.TotalTime, 2) <= variance).OrderBy(task=>task.TotalTime);

            var groups = findTasks.GroupBy(task => task.RawMaterialID);
            dis.RemoveAll(item => item.LineId >= 6);
            var startTime = DateTime.Now;

            var ab = new AverageBisector<AllTask>(allTask => allTask.TotalTime,
                items =>
                {
                    var tList = items.ToList();
                    var avg1 = tList.Average(task => task.TotalTime);
                    return tList.FindAll(t => t.TotalTime > avg1).Count >= dis.Count ||
                           tList.FindAll(t => t.TotalTime <= avg1).Count >= dis.Count;
                });

            var tDis= dis.ConvertAll(t => new TmpDeviceInfo { DeviceName = t.DeviceName, ExpectTotalTime = (t.Ratio / totalRatio) * totalTime, Ratio = t.Ratio, OffPartNumRate = t.OffPartNumRate, OverlappingRate = t.OverlappingRate, PartNumRate = t.PartNumRate, PartSpeedRate = t.PartSpeedRate,LineId = t.LineId});

            var permutations = CombinationHelper.NoIntersectionCombination(allTasks, dis.Count, tDis,
                ab,
                first => first.CombinationCollection.Sum(item1 => item1.TotalTime),
                (second,firsts) => second.ExpectTotalTime - firsts.Sum(),
                (permutationSumTime, expectedValue) => permutationSumTime - expectedValue,
                item=>item,
                item =>
                {
                    //return true;
                    //if (item.CombinationCollection.Exists(t => t.CombinationCollection.Count == 0)) return true;
                    if (item.CombinationCollection.Min(t => t.CombinationCollection.Count) < 2) return true;
                    var maxTime = item.CombinationCollection.Max(t => t.CombinationCollection.Average(tt1 => tt1.TotalTime));
                    var minTime = item.CombinationCollection.Min(t => t.CombinationCollection.Average(tt1 => tt1.TotalTime));
                    return maxTime < minTime * 1.5;
                }, (first, second) =>
                {

                    var tGroup1 = first.CombinationCollection.ConvertAll(t1 => t1.First.CombinationCollection.GroupBy(item => item.RawMaterialID, allTask => allTask, (key, tasks) => new ColorTask { Color = key, TotalBookCount = tasks.Sum(item1 => item1.BookNum) }).ToList());

                    var tGroup2 = second.CombinationCollection.ConvertAll(t1 => t1.First.CombinationCollection.GroupBy(item => item.RawMaterialID, allTask => allTask, (key, tasks) => new ColorTask { Color = key, TotalBookCount = tasks.Sum(item1 => item1.BookNum) }).ToList());

                    IEnumerable<List<ColorTask>> excepts1 = tGroup2.Except(tGroup1, new FuncEqualityComparer<List<ColorTask>>((task1, task2) => !task1.Except(task2).Any()));
                    if (excepts1.Any())
                    {
                        return false;
                    }

                    //for (int i = 0; i < first.CombinationCollection.Count; i++)
                    //{
                        
                    //    var t1 = first.CombinationCollection[i].First.CombinationCollection;
                    //    var t2 = second.CombinationCollection[i].First.CombinationCollection;

                    //    var tGroup11 = t1.GroupBy(item => new { item.PTN_INDEX, item.BookNum }, allTask => allTask, (key, tasks) => new PictureTask { BookNum = key.BookNum, PtnIndex = key.PTN_INDEX, TotalBookCount = tasks.Sum(item1 => item1.BookNum) }).ToList();
                    //    var tGroup12 = t2.GroupBy(item => new { item.PTN_INDEX, item.BookNum }, allTask => allTask, (key, tasks) => new PictureTask { BookNum = key.BookNum, PtnIndex = key.PTN_INDEX, TotalBookCount = tasks.Sum(item1 => item1.BookNum) }).ToList();

                    //    var excepts12 = tGroup12.Except(tGroup11);
                    //    if (excepts12.Any()) return false;

                    //}

                    

                    return true;
                },topCount,
                1,38);

            //List<ICombinationResult<ICombinationResult<AllTask>>> permutations = CombinationHelper.NoIntersectionCombination(allTasks, dis.Count,10,15);
            var endTime = DateTime.Now;
            var times = endTime - startTime;
            textBox1.Text = times.ToString();
            List<List<List<int>>> vList = new List<List<List<int>>>();

            var bookCounts = permutations.ConvertAll(item =>
                item.CombinationCollection.ConvertAll(item1 => item1.First.CombinationCollection.Sum(i => i.BookNum)));

            var timeCounts = permutations.ConvertAll(item =>
                item.CombinationCollection.ConvertAll(item1 => item1.First.CombinationCollection.Sum(i => i.TotalTime)));

            var aa = allTasks.GroupBy(item => item.RawMaterialID);


            var tts = permutations.ConvertAll(item =>
            {
                var maxTime = item.CombinationCollection.Max(t => t.First.CombinationCollection.Sum(t1 => t1.TotalTime));
                var maxOffPartNum = item.CombinationCollection.Max(t => t.First.CombinationCollection.Sum(t1 => t1.OffPartCount));
                var maxPartNum = item.CombinationCollection.Max(t => t.First.CombinationCollection.Sum(t1 => t1.PartCount));
                var maxPartSpeed = item.CombinationCollection.Max(t => t.First.CombinationCollection.Sum(t1 => t1.PartCount+t1.OffPartCount)/(1.0* t.First.CombinationCollection.Sum(t1=>t1.TotalTime)));
                var maxOverlappingRate = item.CombinationCollection.Max(t =>
                    t.First.CombinationCollection.FindAll(t1 => t1.BookNum > 1).Sum(t1 => t1.BookNum) /
                    (1.0 * t.First.CombinationCollection.Sum(t1 => t1.BookNum)));

                var maxOffPartNumExpectedRate = item.CombinationCollection.Max(t => t.Second.OffPartNumRate);
                var maxTimeExpectedRate = item.CombinationCollection.Max(t => t.Second.Ratio);
                var maxOverlappingExpectedRate = item.CombinationCollection.Max(t => t.Second.OverlappingRate);
                var maxPartNumExpectedRate = item.CombinationCollection.Max(t => t.Second.PartNumRate);
                var maxPartSpeedExpectedRate = item.CombinationCollection.Max(t => t.Second.PartSpeedRate);

                return new
                {
                    TimeDegree = item.Degree,
                    OtherOffsize = item.CombinationCollection.ConvertAll(item1 =>
                    {
                        #region 

                        var time = item1.First.CombinationCollection.Sum(a => a.TotalTime);
                        var totalTimeRatio = item1.First.CombinationCollection.Sum(a => a.TotalTime) / (1.0 * maxTime);
                         
                        var offPartNum = item1.First.CombinationCollection.Sum(a => a.OffPartCount);
                         
                        var partNum = item1.First.CombinationCollection.Sum(a => a.PartCount);
                         
                        var partSpeed = (1.0 * item1.First.CombinationCollection.Sum(a => a.TotalTime))/ item1.First.CombinationCollection.Sum(a => a.PartCount + a.OffPartCount);

                        var overlappingRate = item1.First.CombinationCollection.FindAll(t => t.BookNum > 1)
                                              .Sum(t => t.BookNum) /
                                          (1.0 * item1.First.CombinationCollection.Sum(t => t.BookNum));
                        #endregion

                        var device = item1.Second;
                        return new SolutionDetail
                        {
                            SolutionId= permutations.FindIndex(t=>t.Equals(item)),
                            DeviceName = device.DeviceName,
                            ExpectTotalTime = device.ExpectTotalTime,
                            TotalTime = time,
                            TotalTimeRatio = totalTimeRatio/maxTime,
                            TotalTimeExpect = device.Ratio / maxTimeExpectedRate,
                            Ratio=device.Ratio,
                            OffPartNum = offPartNum,
                            OffPartNumRatio= offPartNum/maxOffPartNum,
                            OffPartNumExpect = device.OffPartNumRate / maxOffPartNumExpectedRate,
                            OffPartNumRate=device.OffPartNumRate,
                            PartNum = partNum,
                            PartNumRatio= partNum/maxPartNum,
                            PartNumExpect = device.PartNumRate / maxPartNumExpectedRate,
                            PartNumRate = device.PartNumRate,
                            PartSpeed = partSpeed,
                            PartSpeedRatio= partSpeed/maxPartSpeed,
                            PartSpeedExpect = device.PartSpeedRate / maxPartSpeedExpectedRate,
                            PartSpeedRate=device.PartSpeedRate,
                            Overlapping = overlappingRate,
                            OverlappingRatio = overlappingRate/ maxOverlappingRate,
                            OverlappingExpect = device.OverlappingRate / maxOverlappingExpectedRate,
                            OverlappingRate = device.OverlappingRate,
                            AllTasks = item1.First.CombinationCollection,
                        };
                    })

                };
            });
            
            List<Solution> solutions = new List<Solution>();
            List<SolutionDetail> solutionDetails = new List<SolutionDetail>();
            for (int i = 0; i < tts.Count; i++)
            {
                var ts = tts[i];
                solutionDetails.AddRange(ts.OtherOffsize);
                Solution solution = new Solution
                {
                    SolutionId = i,
                    TimeDegree = Variance.VarianceBase(ts.OtherOffsize.ConvertAll(item => item.TotalTime - item.TotalTimeExpect)),
                    OffPartNumRateDegree = Variance.VarianceBase(ts.OtherOffsize.ConvertAll(item=>item.OffPartNumRatio-item.OffPartNumExpect)),
                    PartNumRateDegree = Variance.VarianceBase(ts.OtherOffsize.ConvertAll(item => item.PartNumRatio-item.PartNumExpect)),
                    PartSpeedRateDegree = Variance.VarianceBase(ts.OtherOffsize.ConvertAll(item => item.PartSpeedRatio-item.PartSpeedExpect)),
                    OverlappingRateDegree = Variance.VarianceBase(ts.OtherOffsize.ConvertAll(item => item.OverlappingRatio-item.OverlappingExpect)),
                };
                solutions.Add(solution);
            }

#region test

            //foreach (var permutation in permutations)
            //{
            //    List<List<int>> vv = permutation.CombinationCollection.ConvertAll(item =>
            //        item.First.CombinationCollection.ConvertAll(item1 => item1.GetHashCode()));
            //    vList.Add(vv);
            //    //按开料时间匹配
            //    //List<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>> cuttingTimeMatchs = CombinationMatchHelper.CombinationMatch(
            //    //    permutation.CombinationCollection, dis,
            //    //    first => first.CombinationCollection.Sum(item1 => item1.TotalTime),
            //    //    second => (second.Ratio / totalRatio) * totalTime,
            //    //    (permutationSumTime, expectedValue) => permutationSumTime - expectedValue);

            //    ////按叠板率匹配
            //    //List<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>> stackRateMatchs = CombinationMatchHelper.CombinationMatch(permutation.CombinationCollection, dis,
            //    //    first => (1.0 * first.CombinationCollection.Sum(item1 => item1.BookNum)) / first.CombinationCollection.Count,
            //    //    second => second.Ratio / totalRatio,
            //    //    (stackRate, expectedValue) => stackRate - expectedValue);

            //    //List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>> cuttingTimeMatchTuples = new List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>>();


            //    //foreach (var match in cuttingTimeMatchs)
            //    //{
            //    //    //方案方差值
            //    //    var cuttingTimeVariance = Variance.GetVarianceResults(match.CombinationCollection, p => p.OffsetQuota);
            //    //    cuttingTimeMatchTuples.Add(new Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>(match, cuttingTimeVariance));
            //    //}

            //    //List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>> stackRateMatchTuples = new List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>>();

            //    //foreach (ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>> match in stackRateMatchs)
            //    //{
            //    //    //方案方差值
            //    //    var stackRateVariance = Variance.GetVarianceResults(match.CombinationCollection, p => p.OffsetQuota);
            //    //    stackRateMatchTuples.Add(new Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>(match, stackRateVariance));
            //    //}

            //    //var results = stackRateMatchTuples.Join(cuttingTimeMatchTuples, first => first.Item1, second => second.Item1,
            //    //    (first, second) =>
            //    //        new
            //    //        {
            //    //            Match = first.Item1,
            //    //            StackRateVariance = first.Item2,
            //    //            CuttingTimeVariance = second.Item2
            //    //        }, new FuncEqualityComparer<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>>(
            //    //        (x, y) => !x.CombinationCollection.Except(y.CombinationCollection, new FuncEqualityComparer<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>(
            //    //            (x1, y1) => !x1.First.CombinationCollection.Except(y1.First.CombinationCollection).Any())).Any()));



            //}
            //List<string> lsList = new List<string>();
            //for (int i = 0; i < vList.Count; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        List<int> list11 = vList[i][j];
            //        var s11 = string.Join(",", list11);
            //        lsList.Add(s11);
            //    }
            //}

            //var s = string.Join("\n", lsList);
#endregion
            return new Tuple<List<Solution>, List<SolutionDetail>>(solutions,solutionDetails);
            
        }

        

        internal class PictureTask
        {
            public short PtnIndex { get; set; }
            public int BookNum { get; set; }
            public int TotalBookCount { get; set; }

            public override int GetHashCode()
            {
                return typeof(PictureTask).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = (PictureTask) obj;
                return BookNum == other.BookNum && PtnIndex == other.PtnIndex && TotalBookCount == other.TotalBookCount;
            }
        }

        internal class ColorTask
        {
            public string Color { get; set; }
            public int TotalBookCount { get; set; }

            public override int GetHashCode()
            {
                return typeof(ColorTask).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = (ColorTask)obj;
                return Color == other.Color && TotalBookCount == other.TotalBookCount;
            }
        }

        private List<DeviceInfos> GetDeviceInfos()
        {
            return gvbDevice.DataSource as List<DeviceInfos>;
        }

        private List<Solution> Solutions { get; set; }
        private List<SolutionDetail> SolutionDetails { get; set; }

        private void btnCreated_Click(object sender, EventArgs e)
        {
            btnSolutions.Enabled = false;
            var topCount = int.Parse(this.topCount.Text);
            var tasks = gvbTask.GetSelectItemsData<AllTask>();
            var devices = GetDeviceInfos();

            var convertTasks = ConvertTasks(_data.FindAll(item=> tasks.Exists(item1=>item1.BatchName==item.BatchName)));

            var tuple = SpiltTask(convertTasks, devices, topCount);
            Solutions = tuple.Item1;
            SolutionDetails = tuple.Item2;
            btnSolutions.Enabled = true;
        }
        private List<AllTask> _data = new List<AllTask>();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //var a1 = new int[] {1, 2, 3, 4, 5};
            //var a2 = new int[] {1, 1, 1, 1, 1, 1, 1};
            //a1.CopyTo(a2,2);
            _data = LineControlCuttingContract.GetAllTasks_Test();
            
            gvbTask.DataSource = GroupByBatch(_data);
        }

        private void btnSolutions_Click(object sender, EventArgs e)
        {
            var form = new RadForm1();
            form.BindingData(Solutions,SolutionDetails);
            form.Show();
        }

        private void btnMdb_Click(object sender, EventArgs e)
        {
            //MDBStarter mdbStarter = new MDBStarter();
            //mdbStarter.CallBackNew();
            //mdbStarter.Test();
        }
    }
}
