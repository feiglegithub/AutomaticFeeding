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
using WCS.Interfaces;

namespace WCS.Commands
{
    /// <summary>
    /// 堆垛车开始任务命令
    /// </summary>
    public class PilerBeginCommand:CommandBase<bool, EPiler>
    {
        private IPiler _piler = null;

        private IPiler Piler =>
            _piler ?? (_piler = new PilerCommunication(BaseArg));

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        private InStockStationCommunication _inStockStationCommunication = null;

        private InStockStationCommunication InStockStationCommunication =>
            _inStockStationCommunication ?? (_inStockStationCommunication = InStockStationCommunication.GetInstance());

        private OutStockStationCommunication _outStockStationCommunication = null;

        private OutStockStationCommunication OutStockStationCommunication =>
            _outStockStationCommunication ??
            (_outStockStationCommunication = OutStockStationCommunication.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private IGroupLinkTaskContract _groupLinkTaskContract = null;

        private IGroupLinkTaskContract GroupGroupLinkTaskContract =>
            _groupLinkTaskContract ?? (_groupLinkTaskContract = GroupLinkTaskService.GetInstance());

        public PilerBeginCommand( EPiler ePiler) : base(ePiler)
        {
            Validating += PilerBeginCommand_Validating;
        }

        private void PilerBeginCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(EPiler baseArg)
        {
            return Piler.IsFree;
        }

        protected override void ExecuteContent()
        {
            try
            {
                var isInStock = InStockStationCommunication.IsRequestInStock(BaseArg);
                if (isInStock)
                {
                    var curColumn = Piler.CurrentColumn;
                    if (OutStockStationCommunication.OutStationIsFree(Piler.Piler))
                    {
                        LogContract.InsertWcsLog($"收到堆垛车【{Piler.Piler}】入库站台入库请求");
                        var stackNo = InStockStationCommunication.StackNo(Piler.Piler);

                        var inTasks = WmsTaskContract.GetWmsTasksByStackNo(stackNo, 1);
                        if (inTasks.Count > 0)
                        {
                            var task = inTasks[0];
                            var ret = Piler.WritePilerInStockTask(task);
                            if (ret)
                            {
                                task.TaskStatus = 21;
                                task.StartTime = DateTime.Now;
                                if (!WmsTaskContract.UpdateWmsTask(task))
                                {
                                    Thread.Sleep(20);
                                    WmsTaskContract.UpdateWmsTask(task);
                                }

                                LogContract.InsertWcsLog($"堆垛车【{Piler.Piler}】入库任务开始：垛号：{task.PilerNo}，目标位：{task.ToPosition}");
                            }
                            //LogContract.InsertWcsLog($"堆垛车入库任务开始：垛号：{task.PilerNo}");
                            return;
                        }
                        else
                        {
                            LogContract.InsertWcsErrorLog($"找不到堆垛车【{Piler.Piler}】任务，垛号：{stackNo}");
                            return;
                        }
                        
                    }
                }

                if(!OutStockStationCommunication.OutStationIsFree(Piler.Piler)) return;
                //查询是否有出库任务
                var outTasks = WmsTaskContract.GetWmsTasksByDdjNo((int) Piler.Piler, 2);
                if (outTasks.Count > 0)
                {
                    LogContract.InsertWcsLog($"收到堆垛车【{Piler.Piler}】空闲请求");
                    var task = outTasks[0];
                    var ret = Piler.WritePilerOutStockTask(task);
                    if (ret)
                    {
                        task.TaskStatus = 30;
                        task.StartTime = DateTime.Now;
                        if (task.ToPosition=="2001" || task.ToPosition == "2002" || task.ToPosition == "2004")
                        {
                            var linkTasks = GroupGroupLinkTaskContract.GetGroupLinkTasksByPilerNo(task.PilerNo.Value);
                            if (linkTasks.Count > 0)
                            {
                                var linkTask = linkTasks[0];
                                linkTask.Status = 1;//堆垛机开始执行拣选上料
                                GroupGroupLinkTaskContract.UpdatedGroupLinkTask(linkTask);
                            }
                        }
                        if (!WmsTaskContract.UpdateWmsTask(task))
                        {
                            Thread.Sleep(200);
                            WmsTaskContract.UpdateWmsTask(task);
                        }
                        LogContract.InsertWcsLog($"堆垛车【{Piler.Piler}】任务开始成功：垛号：{task.PilerNo}");
                    }
                    else
                    {
                        LogContract.InsertWcsErrorLog($"堆垛车【{Piler.Piler}】任务写入失败:垛号：{task.PilerNo}");
                    }
                }

                
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
