using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using WcsModel;
using WcsService;
using WCS.Communications;

namespace WCS.Commands
{
    /// <summary>
    /// 拣选命令
    /// </summary>
    public class MhBeginCommand:CommandBase<bool,string>
    {
        private MachineHandCommunication _handCommunication = null;

        private MachineHandCommunication Hand =>
            _handCommunication ?? (_handCommunication = MachineHandCommunication.GetInstance());
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());
        private IMachineHandTaskLogContract _contract = null;
        private IMachineHandTaskLogContract Contract => _contract ?? (_contract =  MachineHandTaskLogService.GetInstance());

        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        private ISortingStationInfoContract _sortingStationInfoContract = null;

        private ISortingStationInfoContract SortingStationInfoContract =>
            _sortingStationInfoContract ?? (_sortingStationInfoContract =  SortingStationInfoService.GetInstance());



        public MhBeginCommand(string baseArg="拣选开始") : base(baseArg)
        {
            this.Validating += SortingBeginCommand_Validating;
        }

        private void SortingBeginCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(string baseArg)
        {
            return Hand.IsFree;
        }

        protected override void ExecuteContent()
        {
            try
            {
                //获取拣选任务
                var tasks = Contract.GetMachineHandTaskLogs();
                if (tasks.Count == 0) return;
                var task = tasks[0];
                if (task.Status == 1)
                {
                    return;
                }
                var fromStation = (ESortingStation)task.FromStation;
                var toStation = (ESortingStation)task.ToStation;

                var fromStationInfos = SortingStationInfoContract.GetSortingStationInfos(task.FromStation);
                if (fromStationInfos.Count == 0)
                {
                    string msg = $"找不到起始站台：{task.FromStation}的信息";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                var fromStationInfo = fromStationInfos[0];
                var toStationInfos = SortingStationInfoContract.GetSortingStationInfos(task.ToStation);
                if (toStationInfos.Count == 0)
                {
                    string msg = $"找不到目标站台：{task.ToStation}的信息";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                var toStationInfo = toStationInfos[0];

                var fromStationBoardCount = Line.ReadStationBoardCount(fromStation);
                var toStationBoardCount = Line.ReadStationBoardCount(toStation);


                if (fromStationBoardCount != fromStationInfo.BookCount + (fromStationInfo.HasUpBoard ? 1 : 0))
                {
                    string msg = $"起始站台【{task.FromStation}】板材数：{fromStationInfo.BookCount + (fromStationInfo.HasUpBoard ? 1 : 0)}，Plc板材数：{fromStationBoardCount} 数量不一致";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                if (toStationBoardCount != toStationInfo.BookCount + (toStationInfo.HasUpBoard ? 1 : 0))
                {
                    string msg = $"起始站台【{task.ToStation}】板材数：{toStationInfo.BookCount + (toStationInfo.HasUpBoard ? 1 : 0)}，Plc板材数：{toStationBoardCount} 数量不一致";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                if (Line.StackIsOutingFinished(fromStation))
                {
                    string msg = $"站台：{task.FromStation}正在出垛，不能取板";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                if (Line.StackIsOutingFinished(toStation))
                {
                    string msg = $"站台：{task.ToStation}正在出垛，不能放板";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                var canBegin = Contract.MachineHandCanBegin(task.Id);
                if (canBegin.Result)
                {
                    var result = Contract.BeginMachineHandTask(task.Id);
                    if (!result.Result)
                    {
                        LogContract.InsertWcsErrorLog(
                            $"执行拣选任务开始失败：{result.Msg};任务号{task.Id},起始站台：{task.FromStation}，目标站台：{task.ToStation}");
                        return;
                    }

                    var ret = Hand.WriteTask(task.FromStation, task.ToStation);
                    //if (!ret)
                    //{
                    //    Thread.Sleep(20);
                    //    ret = Hand.WriteTask(task.FromStation, task.ToStation);
                    //    if (!ret)
                    //    {
                    //        task.Status = 0;//回滚状态
                    //        Contract.UpdatedMachineHand(task);
                    //    }
                    //}

                    
                }
                else
                {
                    LogContract.InsertWcsErrorLog(
                        $"不允许执行拣选任务：{canBegin.Msg};任务号{task.Id},起始站台：{task.FromStation}，目标站台：{task.ToStation}");
                }

                LogContract.InsertWcsLog(
                    $"拣选任务开始：任务号：{task.Id}，{task.FromStation}->{task.ToStation}，花色：{task.ProductCode}");
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
