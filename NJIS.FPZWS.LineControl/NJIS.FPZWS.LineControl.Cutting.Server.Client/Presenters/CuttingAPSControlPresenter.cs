using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers.Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class CuttingAPSControlPresenter:PresenterBase
    {
        public const string GetAllTask = nameof(GetAllTask);
        /// <summary>
        /// 
        /// 计算切割次数，接收数据类型<see cref="Tuple{T1,T2,T3}"/>
        /// ,Item1为堆垛最大板件数，Item2为设备数量，Item3为单次切割最大板件数
        /// </summary>
        public const string CalculateCutTimes = nameof(CalculateCutTimes);

        public const string GetCuttingDeviceInfos = nameof(GetCuttingDeviceInfos);

        public const string SaveSatck = nameof(SaveSatck);

        private List<DeviceTaskInfo> _calculateResults = null;
        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf? WcfClient.GetProxy<ILineControlCuttingContract>():new LineControlCuttingService());
        private List<AllTask> _datas = null;
        /// <summary>
        /// 堆垛容量
        /// </summary>
        private int CutMaxCount = 0;
        /// <summary>
        /// 单次最大板件数
        /// </summary>
        private int MaxCutCount = 0;
        private DateTime CurrPlanDate = DateTime.Today;

        public CuttingAPSControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<DateTime>(GetAllTask, ExecuteGetAllTask);
            Register<string>(SaveSatck, ExecuteSaveSatck);

            Register<string>(GetCuttingDeviceInfos, ExecuteGetCuttingDeviceInfos);
            Register<Tuple<int, List<DeviceInfos>, int,List<AllTask>>>(CalculateCutTimes, ExecuteCalculateCutTimes);
        }

        private void ExecuteSaveSatck(object sender ,string str = "")
        {
            if (_datas == null)
            {
                Send(CuttingAPSControl.SaveResult, false);
                SendTipsMessage("没有需要保存的数据！", sender);
                return;
            }

            if (_calculateResults==null)
            {
                Send(CuttingAPSControl.SaveResult, false);
                SendTipsMessage( "没有计算结果，无法保存！", sender);
                return;
            }
            var tmpData = _datas = LineControlCuttingContract.GetAllTasks(CurrPlanDate);
            //比较数据是否发生变更
            if (_datas.Count != tmpData.Count)
            {
                Send(CuttingAPSControl.SaveResult, false);
                SendTipsMessage("数据发生变更，请重新查询后再操作！", sender);
                return;
            }

            if (_datas.FindAll(item => !tmpData.Exists(item1 => item.BatchName == item1.BatchName)).Count > 0)
            {
                Send(CuttingAPSControl.SaveResult, false);
                SendTipsMessage("数据发生变更，请重新查询后再操作！", sender);
                return;
            }

            var stackLists = LineControlCuttingContract.GetCuttingStackLists(CurrPlanDate);
            stackLists.RemoveAll(item => !_calculateResults.Exists(item1 => item1.BatchName == item.BatchName));
            var tmpList = new List<CuttingStackList>().Concat(stackLists).ToList();
            foreach (var calculateResult in _calculateResults)
            {
                var batchName = calculateResult.BatchName;
                //foreach (var task in calculateResult.Tasks)
                foreach (var task in calculateResult.NewTasks)
                {
                    for (int i = 0; i < task.BookNum; i++)
                    {
                        var index = tmpList.FindIndex(item =>
                            item.BatchName == batchName && item.PatternName.Trim() == task.PTN_INDEX.ToString());
                        if (index < 0) continue;
                        var stackItem = tmpList[index];
                        stackItem.ItemName = task.ItemName;
                        tmpList.RemoveAt(index);
                    }
                }
            }
            List<SpiltMDBResult> smrs = new List<SpiltMDBResult>();
            foreach (var deviceTaskInfo in _calculateResults)
            {
                var batchName = deviceTaskInfo.BatchName;
                var deviceName = deviceTaskInfo.DeviceName;
                //foreach (var task in deviceTaskInfo.Tasks.GroupBy(item=>item.ItemName))
                foreach (var task in deviceTaskInfo.NewTasks.GroupBy(item=>item.ItemName))
                {
                    var itemName = task.Key;
                    var csl = stackLists.Find(item => item.BatchName == batchName && item.ItemName == itemName);
                    var smr = new SpiltMDBResult();
                    smr.FinishedStatus = Convert.ToInt32(FinishedStatus.UnStock);
                    smr.MdbStatus = Convert.ToInt32(FinishedStatus.MdbUnCreated);
                    smr.BatchName = batchName;
                    smr.DeviceName = deviceName;
                    smr.TaskDistributeId = (Guid) csl.TaskDistributeId;
                    smr.TaskId = csl.TaskId;
                    smr.ActualPlanDate = smr.PlanDate = (DateTime)csl.PlanDate;
                    smr.StartLoadingTime = smr.PlanDate.AddHours(6);
                    smr.ItemName = itemName;
                    smrs.Add(smr);
                }
            }
            //分配任务推送默认顺序
            var groups = smrs.GroupBy(item => item.BatchName).ToList();
            groups.Sort((x,y)=> string.Compare(x.Key, y.Key, StringComparison.Ordinal));

           var spilts= LineControlCuttingContract.GetSpiltMDBResults(CurrPlanDate.Date);
            int batchIndex = spilts.Count==0?0:spilts.Max(item=>item.BatchIndex);
            foreach (var group in groups)
            {
                batchIndex++;
                foreach (var tmpGroup in group.ToList().GroupBy(item => item.DeviceName))
                {
                    int itemIndex = 0;
                    var lst = tmpGroup.ToList();
                    lst.Sort((x,y)=> string.Compare(x.ItemName, y.ItemName, StringComparison.Ordinal));
                    foreach (var spiltMdbResult in lst)
                    {
                        spiltMdbResult.BatchIndex = batchIndex;
                        spiltMdbResult.ItemIndex = ++itemIndex;
                    }
                }
            }

            //分配板件顺序
            foreach (var group in stackLists.GroupBy(item=>item.ItemName))
            {
                var lst = group.ToList();
                lst.Sort((x,y)=>string.Compare(x.PatternName, y.PatternName, StringComparison.Ordinal));
                lst.Sort((x, y) => string.Compare(x.RawMaterialID, y.RawMaterialID, StringComparison.Ordinal));

                int stackIndex = lst.Count+1;
                foreach (var item in lst)
                {
                    item.StackIndex = stackIndex--;
                }
            }

            List< CuttingPattern > cps = new List<CuttingPattern>();
            //锯切图详细信息
            foreach (var batchGroup in _calculateResults.GroupBy(item=>item.BatchName))
            {
                foreach (var group in batchGroup)
                {
                    //foreach (var itemNameGroup in group.Tasks.GroupBy(item => item.ItemName))
                    foreach (var itemNameGroup in group.NewTasks.GroupBy(item => item.ItemName))
                    {
                        var lst = itemNameGroup.ToList();
                        foreach (var task in lst)
                        {
                            var cp = new CuttingPattern
                            {
                                TaskDistributeId = task.TaskDistributeId,
                                BatchName = task.BatchName,
                                ItemName = task.ItemName,
                                PatternName = task.PTN_INDEX,
                                Cycles = task.CutTimes,
                                CutMaxCount = MaxCutCount,
                                BookCount = task.BookNum,
                                PlanDate = CurrPlanDate,
                                CycleTime = task.CYCLE_TIME,
                                TotalTime = task.TotalTime
                            };
                            cps.Add(cp);
                        }
                       
                    }
                }
                
            }
            var saveResult = LineControlCuttingContract.BulkUpdatedStackInfos(stackLists, smrs, cps);
            Send(CuttingAPSControl.SaveResult,saveResult);
            SendTipsMessage(saveResult?"保存成功!":"保存失败!",sender);
            //WorkStationContract.BulkUpdatedCuttingStackLists(stackLists);
        }

        private void ExecuteGetCuttingDeviceInfos(string str = "")
        {
            List<DeviceInfos> deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
            Send(CuttingAPSControl.BindingCuttingDeviceInfos,deviceInfos);
        }

        /// <summary>
        /// 计算垛明细
        /// </summary>
        /// <param name="tuple">Item1为堆垛最大板件数，Item2为设备数量，Item3为单次切割最大板件数</param>
        private void ExecuteCalculateCutTimes(Tuple<int, List<DeviceInfos>, int,List<AllTask>> tuple)
        {
            var dis = tuple.Item2;
            List<DeviceInfos> deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
            // 获取最新的效率比
            dis = deviceInfos.FindAll(item => dis.Exists(item1 => item1.DeviceName == item.DeviceName));

            int maxBook = CutMaxCount = tuple.Item1;
            
            //int deviceCount = dis.Count;
            int maxCutCount = MaxCutCount = tuple.Item3;
            List<AllTask> addAllTasks = new List<AllTask>();
            foreach (var data in _datas)
            {
                if(!tuple.Item4.Exists(item=>item.BatchName == data.BatchName)) continue;
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
                    PartCount = data.PartCount
                };
                int count1 = tmpTask.BookNum / maxCutCount;
                int count2 = (tmpTask.BookNum % maxCutCount > 0 ? 1 : 0);
                tmpTask.CutTimes = count1 + count2;
                data.CutTimes = count1 + count2;
                data.TotalTime = data.CutTimes*data.CYCLE_TIME;
                tmpTask.TotalTime = tmpTask.CYCLE_TIME;
                while (tmpTask.CutTimes>1)
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
                    addAllTasks.Add(task);
                    tmpTask.CutTimes -= 1;
                    tmpTask.BookNum -= task.BookNum;
                    tmpTask.PartCount -= task.PartCount;
                }
                addAllTasks.Add(tmpTask);
            }
            
            //Send(CuttingAPSControl.BindingDatas, GroupByBatch(_datas));

            Method(dis,maxBook, addAllTasks);
        }

        private List<AllTask> GroupByBatch(List<AllTask> datas)
        {
            List<AllTask> allTasks = new List<AllTask>();
            foreach (var group in datas.GroupBy(item=>item.BatchName))
            {
                AllTask allTask = new AllTask()
                {
                    BatchName = group.Key,
                    BookNum = group.Sum(item=>item.BookNum),
                    TotalTime = group.Sum(item=>item.TotalTime),
                    PartCount = group.Sum(item => item.PartCount),
                    OffPartCount = group.Sum(item => item.OffPartCount),
                    MdbCount = group.Sum(item=>item.MdbCount),
                    IsError = group.First().IsError,
                    Msg = group.First().Msg

                };
                allTasks.Add(allTask);
            }

            return allTasks;
        }

        private void ExecuteGetAllTask(object sender,DateTime planTime)
        {
            try
            {
                _datas = LineControlCuttingContract.GetAllTasks(planTime);
                CurrPlanDate = planTime.Date;
                Send(CuttingAPSControl.BindingDatas, GroupByBatch(_datas));
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败:"+e.Message,sender);
            }
            
        }

        /// <summary>
        /// 均分到设备
        /// </summary>
        /// <param name="deviceInfoses"></param>
        /// <param name="maxBook"></param>
        /// <param name="allTasks"></param>
        private void Method(List<DeviceInfos> deviceInfoses,int maxBook, List<AllTask> allTasks)
        {
            
            var groups = allTasks.GroupBy(item => item.BatchName);
            Dictionary<string, TimeSpan> dic = new Dictionary<string, TimeSpan>();
            //List<DeviceTaskInfo> batchTasks = new List<DeviceTaskInfo>();
            _calculateResults = new List<DeviceTaskInfo>();
            DateTime tmp = DateTime.Now;
            foreach (var group in groups)
            {
                if (CuttingServerSettings.Current.IsRawMaterialID)
                {
                    //按花色拆分
                    var tmpList = group.ToList();
                    SendTipsMessage("按花色拆分功能未实现");
                    var materialGroups = tmpList.GroupBy(item => new {item.RawMaterialID,item.BatchName},
                        (item,tasks)=>new
                    {
                        item.BatchName,
                        item.RawMaterialID,
                        SumBookNum=tasks.Sum(task=>task.BookNum)
                    });

                    var tGroups = materialGroups.GroupBy(item => item.RawMaterialID, (key, items) =>
                        new
                        {
                            RawMaterialID = key,
                            SumBookNum = items.Sum(item => item.SumBookNum)
                        });
                    tGroups.Join(materialGroups, tGroup => tGroup.RawMaterialID,
                        materialGroup => materialGroup.RawMaterialID, (first, second) => new
                        {
                            first.RawMaterialID,
                            TotalBookNum = first.SumBookNum,
                            second.BatchName,
                            second.SumBookNum,
                        }).OrderByDescending(item => item.TotalBookNum).ThenByDescending(item => item.SumBookNum);
                }
                else
                {
                    //按时间均分
                    dic.Add(group.Key, tmp.AddSeconds(group.Sum(item => item.TotalTime)) - tmp);
                    List<DeviceTaskInfo> lst = CreatedDeviceTaskInfos(deviceInfoses, maxBook, group.ToList());
                    _calculateResults.AddRange(lst);
                }
                
                //batchTasks.AddRange(lst);
            }

            
            Send(CuttingAPSControl.BindingBatchDatas, _calculateResults);
        }

        private void SpiltTask(List<AllTask> allTasks, List<DeviceInfos> dis)
        {
            var totalTime = allTasks.Sum(item => item.TotalTime);
            var totalRatio = dis.Sum(item => item.Ratio)*1.0;
            List<PermutationResult<PermutationResult<AllTask>>> permutations = PermutationAlgorithm.NoIntersectionPermutation(allTasks, dis.Count);
            foreach (var permutation in permutations)
            {
                //按开料时间匹配
                List<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>> cuttingTimeMatchs = PermutationMatchAlgorithm.PermutationMatch(
                    permutation.PermutationCollection, dis,
                    first => first.PermutationCollection.Sum(item1 => item1.TotalTime),
                    second => (second.Ratio / totalRatio) * totalTime,
                    (permutationSumTime, expectedValue) => permutationSumTime - expectedValue);

                //按叠板率匹配
                List<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>> stackRateMatchs = PermutationMatchAlgorithm.PermutationMatch(permutation.PermutationCollection, dis,
                    first => (1.0 * first.PermutationCollection.Sum(item1 => item1.BookNum)) / first.PermutationCollection.Count,
                    second => second.Ratio / totalRatio,
                    (stackRate, expectedValue) => stackRate - expectedValue);

                List<Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>> cuttingTimeMatchTuples = new List<Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>>();

                
                foreach (var match in cuttingTimeMatchs)
                {
                    //方案方差值
                    var cuttingTimeVariance = VarianceAlgorithm.GetVarianceResults(match.PermutationCollection, p => p.OffsetQuota);
                    cuttingTimeMatchTuples.Add(new Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>(match, cuttingTimeVariance)); 
                }

                List<Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>> stackRateMatchTuples = new List<Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>>();

                foreach (PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>> match in stackRateMatchs)
                {
                    //方案方差值
                    var stackRateVariance = VarianceAlgorithm.GetVarianceResults(match.PermutationCollection, p => p.OffsetQuota);
                    stackRateMatchTuples.Add(new Tuple<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>, double>(match, stackRateVariance));
                }

                var results = stackRateMatchTuples.Join(cuttingTimeMatchTuples, first => first.Item1, second => second.Item1,
                    (first, second) =>
                        new
                        {
                            Match = first.Item1,
                            StackRateVariance = first.Item2,
                            CuttingTimeVariance = second.Item2
                        }, new FuncEqualityComparer<PermutationResult<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>>(
                        (x, y) => !x.PermutationCollection.Except(y.PermutationCollection,new FuncEqualityComparer<MatchingDegree<PermutationResult<AllTask>, DeviceInfos, double>>(
                            (x1, y1) => !x1.First.PermutationCollection.Except(y1.First.PermutationCollection).Any())).Any()));



            }
            

        }

        private List<Tuple<DeviceInfos, List<AllTask>>> SpiltTask(List<DeviceInfos> dis, List<AllTask> allTasks)
        {
            var totalTime = allTasks.Sum(item => item.TotalTime);
            var totalRatio = dis.Sum(item => item.Ratio);
            List<Tuple<DeviceInfos, List<AllTask>>> list = new List<Tuple<DeviceInfos, List<AllTask>>> ();
            dis = dis.OrderBy(item => item.Ratio).ToList();
            foreach (var di in dis)
            {
                var curRatioTime = totalTime * (di.Ratio / totalRatio);
                
                List<AllTask> lis = dis.Last().Equals(di) ? allTasks.ToList() : GetRange(curRatioTime, allTasks);
                allTasks.RemoveAll(item => lis.Contains(item));
                list.Add(new Tuple<DeviceInfos, List<AllTask>>(di,lis));
            }

            return list;
        }

        /// <summary>
        /// 获取指定满足指定范围的集合
        /// </summary>
        /// <param name="ratioTime"></param>
        /// <param name="allTasks"></param>
        /// <returns></returns>
        private List<AllTask> GetRange(float ratioTime,List<AllTask> allTasks)
        {
            List<AllTask> list = new List<AllTask>();

            var tmpAllTask = allTasks.OrderByDescending(item => item.CutTimes)
                .ThenByDescending(item => item.RawMaterialID).ToList();
            var curTime = 0;
            for (int i = 0; i < tmpAllTask.Count; i++)
            {
                var allTask = tmpAllTask[i];
                if(curTime+ allTask.TotalTime < ratioTime)
                {
                    curTime += allTask.TotalTime;
                    list.Add(allTask);
                }
            }

            if (list.Count == 0)
            {
                list.Add(tmpAllTask.Last());
            }

            return list;
        }

        /// <summary>
        /// 拆分成垛
        /// </summary>
        /// <param name="dis"></param>
        /// <param name="maxBook"></param>
        /// <param name="allTasks"></param>
        /// <returns></returns>
        private List<DeviceTaskInfo> CreatedDeviceTaskInfos(List<DeviceInfos> dis,int maxBook, List<AllTask> allTasks)
        {
            SpiltTask( allTasks, dis);
            var list = SpiltTask(dis, allTasks);

            //allTasks = allTasks.OrderByDescending(item => item.CutTimes).ToList();
            //allTasks.Sort((x,y)=>x.CutTimes.CompareTo(y.CutTimes));
            //allTasks.Reverse();

            List<DeviceTaskInfo> taskInfos = new List<DeviceTaskInfo>();
            foreach (var tuple in list)
            {
                var deviceTaskInfo = new DeviceTaskInfo(maxBook);
                deviceTaskInfo.DeviceName = tuple.Item1.DeviceName;
                foreach (var allTask in tuple.Item2)
                {
                    deviceTaskInfo.AddTask(allTask);
                }
                taskInfos.Add(deviceTaskInfo);
            }


            //for (int i = 0; i < dis.Count; i++)
            //{
            //    taskInfos.Add(new DeviceTaskInfo(maxBook)); 
            //}

            //foreach (var task in allTasks)
            //{
            //    taskInfos.Sort((x,y)=>x.TotalTime.CompareTo(y.TotalTime));
            //    taskInfos[0].AddTask(task);
            //}

            CreatedStackName(taskInfos);

            return taskInfos;
        }
        /// <summary>
        /// 生成垛号
        /// </summary>
        /// <param name="taskInfos"></param>
        private void CreatedStackName(List<DeviceTaskInfo> taskInfos)
        {
            var groups = taskInfos.GroupBy(item => item.BatchName);
            int curStackNum = 0;
            foreach (var group in groups)
            {
                string batchNamePart = group.Key.Remove(0, 6);
                var tmpList = group.ToList();
                foreach (var taskInfo in tmpList)
                {
                    List<string> stackNames = new List<string>();

                    for (int i = 0; i < taskInfo.StackCount; i++)
                    {
                        stackNames.Add($"SP{(++curStackNum):000}{batchNamePart}");
                    }

                    taskInfo.SatckNames = stackNames;
                }

                curStackNum = 0;
            }

        }

        protected internal class StackInfo
        {
            public int BookCount { get; set; }
            public string StatckName { get; set; }
            public int CutTimes { get; set; }
            public int TotalTime { get; set; }
            public int MaxCount { get; set; }
            public bool CanAdd(AllTask allTask)
            {
                return BookCount + allTask.BookNum <= MaxCount;
            }

            public void Add(AllTask allTask)
            {
                if (!CanAdd(allTask))
                {
                    throw new Exception("板件数量超出堆垛最大容量");
                }

            }
        }

    }

    public class DeviceTaskInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxBook">堆垛最大板件数</param>
        public DeviceTaskInfo(int maxBook)
        {
            //Tasks = new List<AllTask>();
            MaxCount = maxBook;
        }

        public string DeviceName { get; set; }


        public TimeSpan Time
        {
            get
            {
                DateTime tmp = DateTime.Now;
                return tmp.AddSeconds(TotalTime) - tmp;
            }
        }

        /// <summary>
        /// 堆垛最大板件数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 堆垛数量
        /// </summary>
        public int StackCount => stacks.Count;

        /// <summary>
        /// 总板件数
        /// </summary>
        public int TotalBook
        {
            get { return NewTasks.Sum(item => item.BookNum); }
            //get { return Tasks.Sum(item => item.BookNum); }
        } 
        public string BatchName { get; private set; }

        /// <summary>
        /// 花色总数
        /// </summary>
        //public int MaterialCount { get;  }

        public int TotalTime => NewTasks.Sum(item => item.CYCLE_TIME*item.CutTimes);
        public int CutTimes => NewTasks.Sum(item => item.CutTimes);

        public List<string> SatckNames
        {
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    stacks[i].Tasks.ForEach(allTask=>allTask.ItemName=value[i]);
                }
            }
        }

        public List<AllTask> NewTasks
        {
            get
            {
                List<AllTask> tmp = new List<AllTask>();
                foreach (var stack in stacks)
                {
                    tmp = tmp.Concat(stack.Tasks).ToList();
                }

                return tmp;
            }
        }
        //private int CurrIndex = 0;
        private List<Stack> stacks = new List<Stack>();

        //public bool CanAddTask(AllTask task)
        //{

        //}
        
        public void AddTask(AllTask task)
        {
            BatchName = task.BatchName;
            var stack = stacks.Find(item => (item.BookCount + task.BookNum <= MaxCount) &&(item.Tasks.Exists(item1=>item1.RawMaterialID==task.RawMaterialID || item.ColorCount<3)) );
            if (stack!=null)
            {
                
                stack.Tasks.Add(task);
                stack.BookCount += task.BookNum;
            }
            else
            {
                var newStack = new Stack();
                stacks.Add(newStack);
                newStack.Tasks.Add(task);
                newStack.BookCount += task.BookNum;
            }
        }

        protected internal class Stack
        {
            public Stack()
            {
                Tasks = new List<AllTask>();
            }
            public int BookCount { get; set; }
            public List<AllTask> Tasks { get; }
            /// <summary>
            /// 花色数
            /// </summary>
            public int ColorCount
            {
                get => Tasks.GroupBy(item => item.RawMaterialID).Count();
            }
        }

    }
}
