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
    /// 拣选上料完成
    /// </summary>
    public class SortingLoadingFinishedCommand : CommandBase<bool, ESortingStation>
    {
        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private ISortingStationInfoContract _sortingStationInfoContract = null;

        private ISortingStationInfoContract SortingStationInfoContract =>
            _sortingStationInfoContract ?? (_sortingStationInfoContract =  SortingStationInfoService.GetInstance());

        private IGroupLinkTaskContract _groupLinkTaskContract = null;

        private IGroupLinkTaskContract GroupGroupLinkTaskContract =>
            _groupLinkTaskContract ?? (_groupLinkTaskContract = GroupLinkTaskService.GetInstance());

        public SortingLoadingFinishedCommand(ESortingStation baseArg) : base(baseArg)
        {
            this.Validating += SortingLoadingFinishedCommand_Validating;
        }

        private void SortingLoadingFinishedCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !RequestData;
        }

        protected override bool LoadRequest(ESortingStation baseArg)
        {
            bool flag = false;
            try
            {
                //LogContract.InsertWcsLog($"开始读取{baseArg}状态");
                flag = Line.IsDone(baseArg);
                //LogContract.InsertWcsLog($"完成读取{baseArg}状态");
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog($"读取拣选工位{BaseArg}上料完成状态失败:" + e.Message);
            }

            return flag;
        }

        protected override void ExecuteContent()
        {
            try
            {
                //LogContract.InsertWcsLog($"收到站台【{BaseArg}】上料完成信号");
                var stackNo = Line.ReadStackNo(BaseArg);
                LogContract.InsertWcsLog($"收到站台【{BaseArg}】上料完成信号,垛号{stackNo}");
                if (stackNo <= 0)
                {
                    string msg = "垛号丢失";
                    LogContract.InsertWcsErrorLog(msg);
                    Line.ClearPlcRequest(BaseArg);
                    return;
                }
                var wmsTasks = WmsTaskContract.GetWmsTasksByStackNo(stackNo, 2);
                //LogContract.InsertWcsLog($"查找任务");
                if (wmsTasks.Count == 0)
                {
                    string msg = $"找不到垛号：{stackNo} 的要料出库任务";
                    LogContract.InsertWcsErrorLog(msg);
                    Line.ClearPlcRequest(BaseArg);
                    return;
                }

                var stationInfos = SortingStationInfoContract.GetSortingStationInfos(Convert.ToInt32(BaseArg));
                //LogContract.InsertWcsLog($"拣选工位信息");
                if (stationInfos.Count == 0)
                {
                    string msg = $"找不到站台【{BaseArg}】信息";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                var task = wmsTasks[0];
                var writeSuccess = Line.WriteBoardCountToStation(BaseArg, task.Amount.Value + (task.HasUpProtect.Value ? 2 : 1));
                if (!writeSuccess)
                {
                    LogContract.InsertWcsErrorLog($"写入Plc垛{task.PilerNo.Value}板材数量失败");
                    return;
                }
                //LogContract.InsertWcsLog($"写入数量");
                writeSuccess &= Line.WriteProductCodeToStation(BaseArg, task.ProductCode);
                //LogContract.InsertWcsLog($"写入花色");
                if (!writeSuccess)
                {
                    LogContract.InsertWcsErrorLog($"写入Plc垛{task.PilerNo.Value}花色失败");
                    return;
                }

                var stationInfo = stationInfos[0];
                if (BaseArg == ESortingStation.SortingStation2005)
                {
                    stationInfo.BookCount = task.Amount.Value;
                }
                else
                {
                    stationInfo.BookCount = task.Amount.Value + 1;//包含底板数量
                }
                
                if (BaseArg == ESortingStation.SortingStation2001 || BaseArg == ESortingStation.SortingStation2002 || BaseArg == ESortingStation.SortingStation2004)
                {
                    GroupLinkTask linkTask = null;
                    var linkTasks = GroupGroupLinkTaskContract.GetGroupLinkTasksByPilerNo(task.PilerNo.Value);
                    if (linkTasks.Count > 0)
                    {
                        linkTask = linkTasks[0];
                        linkTask.Status = 19;//拣选上料完成
                        GroupGroupLinkTaskContract.UpdatedGroupLinkTask(linkTask);
                    }
                }

                stationInfo.HasUpBoard = task.HasUpProtect.Value;
                stationInfo.Color = task.ProductCode;
                stationInfo.PilerNo = task.PilerNo.Value;
                stationInfo.IsOuting = false;
                task.FinishTime = DateTime.Now;
                task.TaskStatus = 98;
                var ret = WmsTaskContract.UpdateWmsTask(task);
                if (!ret)
                {
                    Thread.Sleep(20);
                    ret = WmsTaskContract.UpdateWmsTask(task);
                }


                var updatedResult = SortingStationInfoContract.UpdatedSortingStationInfo(stationInfo);
                if (!updatedResult)
                {
                    Thread.Sleep(500);
                    updatedResult = SortingStationInfoContract.UpdatedSortingStationInfo(stationInfo);
                }
                writeSuccess = Line.ClearPlcRequest(BaseArg);
                if (!writeSuccess)
                {
                    Thread.Sleep(500);
                    writeSuccess = Line.ClearPlcRequest(BaseArg);
                }

                LogContract.InsertWcsLog($"垛：{task.PilerNo.Value}上料完成，目标站台:【{Convert.ToInt32(BaseArg)}】");
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }

        }
    }
}
