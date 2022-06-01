using System;
using System.Threading;
using Contract;
using WcsModel;
using WcsService;
using WCS.Commands.Args;
using WCS.Communications;


namespace WCS.Commands
{
    /// <summary>
    /// 板材入库命令
    /// </summary>
    public class BoardInStockCommand : CommandBase<bool, EInOutStation>
    {
        private IWmsTaskContract _wmsTaskContract = null;
        private IWmsTaskContract IwmsTaskContract => _wmsTaskContract ?? (_wmsTaskContract = WmsService.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        private InOutCommunication _inOutCommunication = null;

        private InOutCommunication InOutCommunication =>
            _inOutCommunication ?? (_inOutCommunication = InOutCommunication.GetInstance());

        public BoardInStockCommand():base(EInOutStation.InStockStationGt102)
        {
            Validating += BoardInStockCommand_Validating;
        }

        private void BoardInStockCommand_Validating(object arg1, CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        private bool isLog = false;
        protected override void ExecuteContent()
        {
            try
            {
                var tasks = IwmsTaskContract.GetBoardInStockTask();
                if (tasks.Count > 0)
                {
                    var task = tasks[0];
                    int ddj = (Convert.ToInt32(task.ToPosition.Split('.')[0]) + 1) / 2;
                    task.TaskStatus = 20;
                    var ret = IwmsTaskContract.UpdateWmsTask(task);
                    if (ret)
                    {
                        var writeSuccess = InOutCommunication.WriteInStockTaskToLine(task.PilerNo.Value, 105 - ddj);
                        if (!writeSuccess)
                        {
                            Thread.Sleep(20);
                            writeSuccess = InOutCommunication.WriteInStockTaskToLine(task.PilerNo.Value, 105 - ddj);
                        }

                        writeSuccess = InOutCommunication.ClearRequest(BaseArg);
                        if (writeSuccess)
                        {
                            Thread.Sleep(20);
                            writeSuccess = InOutCommunication.ClearRequest(BaseArg);
                        }
                        LogContract.InsertWcsLog($"板材入库开始：垛号：{task.PilerNo}");
                    }

                    isLog = false;
                }
                else
                {
                    if (!isLog)
                    {
                        LogContract.InsertWcsErrorLog($"板材入库失败：找不到板材入库任务");
                        isLog = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }

        protected override bool LoadRequest(EInOutStation baseArg)
        {
            try
            {
                return InOutCommunication.HasInStockRequest;
            }
            catch (Exception e)
            {
                return false;
            }

            
        }
    }
}
