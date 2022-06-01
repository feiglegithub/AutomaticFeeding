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
    /// 拣选站台回库任务（余料回库，空垫板回库，拣选回库）
    /// </summary>
    public class SortingBeginInStockCommand: CommandBase<List<WMS_Task>,ESortingStation>
    {

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private LineCommunication _communication = null;
        private LineCommunication Communication => _communication ?? (_communication = LineCommunication.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());

        private ISortingStationInfoContract _sortingStationInfoContract = null;

        private ISortingStationInfoContract SortingStationInfoContract =>
            _sortingStationInfoContract ?? (_sortingStationInfoContract =  SortingStationInfoService.GetInstance());

        public SortingBeginInStockCommand(ESortingStation baseArg) : base(baseArg)
        {
            this.Validating += SortingInStockCommand_Validating;
        }

        private void SortingInStockCommand_Validating(object arg1, Args.CancelEventArg<List<WMS_Task>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<WMS_Task> LoadRequest(ESortingStation baseArg)
        {
            List<WMS_Task> wmsTasks = new List<WMS_Task>();
            try
            {
                wmsTasks=WmsTaskContract.GetTaskByFromStationNo(Convert.ToInt32(baseArg), 1);
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }

            return wmsTasks;
        }

        protected override void ExecuteContent()
        {
            try
            {
                LogContract.InsertWcsLog($"开始执行拣选入库命令");
                var wmsTask = RequestData[0];
                SortingStationInfoContract.GetSortingStationInfos();
                var fromStationInfos = SortingStationInfoContract.GetSortingStationInfos(Convert.ToInt32(BaseArg));
                if (fromStationInfos.Count == 0)
                {
                    string msg = $"找不到起始站台：【{Convert.ToInt32(BaseArg)}】的信息";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }

                var fromStationInfo = fromStationInfos[0];
                var writeSuccess = Communication.WriteInStockTask(Convert.ToInt32(BaseArg),wmsTask.PilerNo.Value,wmsTask.DdjNo.Value);
                if (!writeSuccess)
                {
                    string msg = $"写入Plc入库任务失败:任务号：{wmsTask.TaskId}";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }
                fromStationInfo.IsOuting = true;
                var ret = SortingStationInfoContract.UpdatedSortingStationInfo(fromStationInfo);
                if (!ret)
                {
                    Thread.Sleep(20);
                    ret = SortingStationInfoContract.UpdatedSortingStationInfo(fromStationInfo);
                   
                }

                LogContract.InsertWcsErrorLog($"更新站台【{fromStationInfo.StationNo}】为正在出垛状态{(ret ? "成功" : "失败")}");
                wmsTask.TaskStatus = 20;
                wmsTask.StartTime = DateTime.Now;
                ret = WmsTaskContract.UpdateWmsTask(wmsTask);
                if (!ret)
                {
                    Thread.Sleep(20);
                    ret = WmsTaskContract.UpdateWmsTask(wmsTask);
                }

                LogContract.InsertWcsLog($"拣选入库开始，任务号：{wmsTask.TaskId}，起始站台:【{Convert.ToInt32(BaseArg)}】");
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
