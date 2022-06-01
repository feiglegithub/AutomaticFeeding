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
    /// 堆垛车任务完成命令
    /// </summary>
    public class PilerFinishedCommand : CommandBase<bool, EPiler>
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

        public PilerFinishedCommand(EPiler baseArg) : base(baseArg)
        {
            Validating += PilerFinishedCommand_Validating;
        }

        private void PilerFinishedCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !arg2.RequestData;
        }

        protected override bool LoadRequest(EPiler baseArg)
        {
            return Piler.IsFinished;
        }

        protected override void ExecuteContent()
        {
            try
            {
                var stackName = Piler.PilerStackName;
                var stackNo = Convert.ToInt32(stackName);
                var tasks = WmsTaskContract.GetWmsTasksByStackNo(stackNo);
                //var tasks = WmsTaskContract.GetWmsTasksByPilerNo(Convert.ToInt32(Piler.Piler));
                LogContract.InsertWcsLog($"收到堆垛车【{Piler.Piler}】任务完成请求，垛号:{stackNo}");
                if (tasks.Count > 0)
                {
                    var task = tasks[0];
                    if (task.TaskType == 1)//入库任务
                    {
                        task.TaskStatus = 98;
                        task.FinishTime = DateTime.Now;
                        var ret = WmsTaskContract.UpdateWmsTask(task);
                        if (ret)
                        {
                            ret = Piler.ClearTaskFinished();
                            if(!ret)
                            {
                                Thread.Sleep(100);
                                ret = Piler.ClearTaskFinished();
                                
                            }
                        }

                        LogContract.InsertWcsLog($"堆垛车入库【{Piler.Piler}】任务完成成功:垛号：{stackNo}");
                    }

                    if (task.TaskType == 2)//出库任务
                    {
                        task.TaskStatus = 31;
                        task.DdjTime = DateTime.Now;
                        var ret =  WmsTaskContract.UpdateWmsTask(task);
                        if (ret)
                        {
                            GroupLinkTask linkTask = null;
                            if (task.ToPosition == "2001" || task.ToPosition == "2002" || task.ToPosition == "2004")
                            {
                                var linkTasks = GroupGroupLinkTaskContract.GetGroupLinkTasksByPilerNo(task.PilerNo.Value);
                                if (linkTasks.Count > 0)
                                {
                                    linkTask = linkTasks[0];
                                    linkTask.Status = 9;//堆垛机开始执行拣选上料完成
                                    GroupGroupLinkTaskContract.UpdatedGroupLinkTask(linkTask);
                                }
                            }

                            var target = int.Parse(task.ToPosition);
                            target = target >= 3001 ? Convert.ToInt32(EInOutStation.OutStockStationGt208) : target;
                            var writeResult = OutStockStationCommunication.WriteTask(Piler.Piler, stackNo, target);
                            if (!writeResult)
                            {
                                Thread.Sleep(100);
                                writeResult = OutStockStationCommunication.WriteTask(Piler.Piler, stackNo, target);
                                
                            }

                            if (linkTask != null)
                            {
                                linkTask.Status = 11;//堆垛机开始执行拣选上料完成
                                GroupGroupLinkTaskContract.UpdatedGroupLinkTask(linkTask);
                            }
                            ret = Piler.ClearTaskFinished();
                            if (!ret)
                            {
                                Thread.Sleep(100);
                                ret = Piler.ClearTaskFinished();
                                
                            }
                            LogContract.InsertWcsLog($"堆垛车【{Piler.Piler}】出库任务完成成功:垛号：{stackNo}");
                        }
                    }
                }
                else
                {
                    LogContract.InsertWcsErrorLog($"堆垛车【{Piler.Piler}】任务完成失败:找不到任务垛号:{stackNo}");
                    Piler.ClearTaskFinished();
                }

            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
