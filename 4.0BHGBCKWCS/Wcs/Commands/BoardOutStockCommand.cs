using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using Telerik.WinControls;
using WcsModel;
using WcsService;
using WCS.Communications;

namespace WCS.Commands
{
    /// <summary>
    /// 板材出库 出库到Rgv站台，出库口
    /// </summary>
    public class BoardOutStockCommand: CommandBase<bool, EInOutStation>
    {
        private IWmsTaskContract _wmsTaskContract = null;
        private IWmsTaskContract IwmsTaskContract => _wmsTaskContract ?? (_wmsTaskContract = WmsService.GetInstance());

        private InOutCommunication _inOutCommunication = null;
        private InOutCommunication InOutCommunication =>
            _inOutCommunication ?? (_inOutCommunication = InOutCommunication.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        private IRgvContract _rgvContract = null;
        private IRgvContract RgvContract => _rgvContract ?? (_rgvContract = RgvService.GetInstance());

        private DateTime ErrorTime { get; set; }=DateTime.Now;

        public BoardOutStockCommand(EInOutStation baseArg) : base(baseArg)
        {
            Validating += BoardOutStockCommand_Validating;
        }

        private void BoardOutStockCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(EInOutStation baseArg)
        {
            if (ErrorTime.AddSeconds(10) > DateTime.Now)
                return false;
            if(baseArg == EInOutStation.OutStockStationGt116)
                return InOutCommunication.OutStockIsFinished;
            if (baseArg == EInOutStation.OutStockStationGt208)
            {
                return InOutCommunication.IsReachedRgv;
            }

            return false;
        }


        protected override void ExecuteContent()
        {
            try
            {
                var stackNo = InOutCommunication.ReadStationStackNo(BaseArg);
                if (stackNo == 0)
                {
                    LogContract.InsertWcsErrorLog($"收到板材出库完成信号，垛号：{stackNo}");
                    ErrorTime=DateTime.Now;
                }
                else
                {
                    LogContract.InsertWcsLog($"收到板材出库完成信号，垛号：{stackNo}");
                }
                
                var tasks = IwmsTaskContract.GetWmsTasksByStackNo(stackNo, 2);
                if (tasks.Count > 0)
                {
                    var task = tasks[0];
                    if (BaseArg == EInOutStation.OutStockStationGt208 && task.ToPosition!="1001")
                    {
                        task.TaskStatus = 31;
                        List<RGV_Task> rgvTasks = new List<RGV_Task>
                        {
                            new RGV_Task
                            {
                                TaskType = 1,
                                FromPosition = 3014,
                                ToPosition = int.Parse(task.ToPosition),
                                PilerNo = task.PilerNo,
                                Status = 1
                            }
                        };
                        while (!(RgvContract.BulkInsertRgvTasks(rgvTasks) > 0))
                        {
                            Thread.Sleep(20);
                        }
                        LogContract.InsertWcsLog($"到达Rgv站台，创建Rgv任务成功");
                    }
                    else
                    {
                        task.TaskStatus = 98;
                    }

                    if (!IwmsTaskContract.UpdateWmsTask(task))
                    {
                        Thread.Sleep(100);
                        IwmsTaskContract.UpdateWmsTask(task);
                    }

                    var writeSuccess = InOutCommunication.ClearRequest(BaseArg);
                    if (!writeSuccess)
                    {
                        Thread.Sleep(20);
                        writeSuccess = InOutCommunication.ClearRequest(BaseArg);
                    }
                    LogContract.InsertWcsErrorLog($"板材出库完成成功：垛号：{stackNo}");
                }
                else
                {
                    LogContract.InsertWcsErrorLog($"板材出库完成失败：找不到出库任务，垛号：{stackNo}");
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(this.GetType().ToString() + e.Message);
            }
            
        }
    }
}
