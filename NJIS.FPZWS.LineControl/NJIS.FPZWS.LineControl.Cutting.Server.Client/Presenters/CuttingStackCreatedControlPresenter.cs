using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class CuttingStackCreatedControlPresenter:PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string BindingData = nameof(BindingData);
        public const string GetCuttingDeviceInfos = nameof(GetCuttingDeviceInfos);
        public const string GenerateSolutions = nameof(GenerateSolutions);

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());

        private List<AllTask> _datas = null;
        private DateTime CurrPlanDate = DateTime.Today;

        public CuttingStackCreatedControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<DateTime>(GetData, ExecuteGetAllTask);
            //Register<string>(SaveSatck, ExecuteSaveSatck);

            Register<string>(GetCuttingDeviceInfos, ExecuteGetCuttingDeviceInfos);
            //Register<Tuple<int, List<DeviceInfos>, int, List<AllTask>>>(CalculateCutTimes, ExecuteCalculateCutTimes);
        }

        private void ExecuteGetAllTask(object sender, DateTime planTime)
        {
            try
            {
                _datas = LineControlCuttingContract.GetAllTasks(planTime);
                CurrPlanDate = planTime.Date;
                Send(BindingData, GroupByBatch(_datas));
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败:" + e.Message, sender);
            }

        }

        private void ExecuteGetCuttingDeviceInfos(string str = "")
        {
            List<DeviceInfos> deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
            Send(BindingData, deviceInfos);
        }

        private List<AllTask> GroupByBatch(List<AllTask> datas)
        {
            List<AllTask> allTasks = new List<AllTask>();
            foreach (var group in datas.GroupBy(item => item.BatchName))
            {
                AllTask allTask = new AllTask()
                {
                    BatchName = group.Key,
                    BookNum = group.Sum(item => item.BookNum),
                    TotalTime = group.Sum(item => item.TotalTime),
                    PartCount = group.Sum(item => item.PartCount),
                    OffPartCount = group.Sum(item => item.OffPartCount)
                };
                allTasks.Add(allTask);
            }

            return allTasks;
        }

        /// <summary>
        /// 获取批次的锯切图详细任务信息
        /// </summary>
        /// <param name="batchName">批次号</param>
        /// <param name="maxCutCount">板材最大堆叠数</param>
        /// <returns></returns>
        private List<AllTask> GetBatchDetailTasks(string batchName,int maxCutCount)
        {
            List<AllTask> addAllTasks = new List<AllTask>();

            foreach (var data in _datas.FindAll(item=>item.BatchName==batchName))
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
                    RawMaterialID = data.RawMaterialID
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
                    addAllTasks.Add(task);
                    tmpTask.CutTimes -= 1;
                    tmpTask.BookNum -= task.BookNum;
                }
                addAllTasks.Add(tmpTask);
            }

            return addAllTasks;
        }

        public  void ExecuteGetCuttingDeviceInfos(Tuple<int, List<DeviceInfos>, int, List<AllTask>> tuple)
        {
            var dis = tuple.Item2;
            int maxBook = tuple.Item1;

            //int deviceCount = dis.Count;
            int maxCutCount = tuple.Item3;
            foreach (var allTask in tuple.Item4)
            {
                var batchName = allTask.BatchName;
                List<AllTask> addAllTasks = GetBatchDetailTasks(batchName, maxCutCount);
                var batchBookSum = addAllTasks.Sum(item => item.BookNum); 
                var batchTimeSum = addAllTasks.Sum(item => item.TotalTime);
                var groups = addAllTasks.GroupBy(item => item.RawMaterialID);

                var colorCount = from task in addAllTasks
                    group task by new {RawMaterialID = task.RawMaterialID}
                    into t 
                    select new {Count = t.Sum(item=>item.BookNum), RawMaterialID = t.Key.RawMaterialID};

                colorCount = colorCount.OrderByDescending(item => item.Count);

                for (int i = 1; i <=dis.Count; i++)
                {
                    int deviceCount = i;
                    List<Tuple<short,string,int>>[] lists = new List<Tuple<short, string, int>>[i];
                    //每个设备分配的平均时间
                    var stackAvg = batchTimeSum / deviceCount;
                    foreach (var color in colorCount)
                    {
                        var rawMaterialId = color.RawMaterialID;
                        var group = groups.FirstOrDefault(item => item.Key == rawMaterialId);
                        var rawMaterialIdTasks = group.ToList().OrderByDescending(item => item.CutTimes).ThenBy(item=>item.PTN_INDEX);

                    }


                }
            }
            
        }

        
    }
}
