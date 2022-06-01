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
using WCS.Helpers;
using WCS.Interfaces;
using static System.Threading.Thread;

namespace WCS.Commands
{
    /// <summary>
    /// 拣选完成回库请求
    /// </summary>
    public class SortingInStockRequestCommand : CommandBase<List<SortingInStockRequest>, string>
    {
        private ISortingInStockRequestContract _contract = null;

        private ISortingInStockRequestContract Contract =>
            _contract ?? (_contract =  SortingInStockRequestService.GetInstance());


        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());
        
        private IWms _wmsService = null;
        private IWms Wms => _wmsService ?? (_wmsService = WmsServiceHelper.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;
        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        public SortingInStockRequestCommand(string baseArg= "拣选完成入库请求") : base(baseArg)
        {
            this.Validating += SortingInStockRequestCommand_Validating;
        }

        private void SortingInStockRequestCommand_Validating(object arg1, Args.CancelEventArg<List<SortingInStockRequest>> arg2)
        {
            arg2.Cancel = RequestData.Count == 0;
        }

        protected override List<SortingInStockRequest> LoadRequest(string baseArg)
        {
            List<SortingInStockRequest> requests = new List<SortingInStockRequest>();
            try
            {
                requests = Contract.GetSortingInStockRequests();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }

            return requests;
        }

        protected override void ExecuteContent()
        {
            try
            {
                
                var request = RequestData[0];
                LogContract.InsertWcsLog($"请求拣选完成回库,拣选任务号：{request.SortingTaskId}");
                var ret = Wms.ApplySortingInStock(request.SortingTaskId, request.FromStation);
                if (ret.Status == 200)
                {
                    LogContract.InsertWcsLog($"请求拣选完成回库成功,拣选任务号：{request.SortingTaskId}，请求号{ret.ReqId}");
                    request.IsRequest = true;
                    request.ReqId = ret.ReqId;
                    var result =Contract.UpdatedRequestStatus(request);
                    var wmsTasks = WmsTaskContract.GetWmsTasksByTaskId(request.SortingTaskId);
                    if(wmsTasks.Count>0)
                    {
                        var wmsTask = wmsTasks[0];
                        if (wmsTask.TaskStatus < 98)
                        {
                            wmsTask.TaskStatus = 98;
                            wmsTask.FinishTime = DateTime.Now;
                            WmsTaskContract.UpdateWmsTask(wmsTask);
                        }
                    }
                    if (!result)
                    {
                        result = Contract.UpdatedRequestStatus(request);
                        Sleep(20);
                    }

                }
                else
                {
                    LogContract.InsertWcsErrorLog(GetType() + ret.Message);
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
