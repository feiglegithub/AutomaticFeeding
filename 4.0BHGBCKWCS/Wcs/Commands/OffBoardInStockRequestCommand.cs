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

namespace WCS.Commands
{
    /// <summary>
    /// 拣选余料回库请求命令
    /// </summary>
    public class OffBoardInStockRequestCommand : CommandBase<List<OffBoardRequest>, string>
    {
        private IOffBoardRequestContract _contract = null;
        private IOffBoardRequestContract Contract => _contract ?? (_contract = OffBoardRequestService.GetInstance());
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());
        private IWms _wmsService = null;
        private IWms Wms => _wmsService ?? (_wmsService = WmsServiceHelper.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;
        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        public OffBoardInStockRequestCommand(string baseArg="请求余料回库") : base(baseArg)
        {
            this.Validating += OffBoardInStockRequestCommand_Validating;
        }

        private void OffBoardInStockRequestCommand_Validating(object arg1, Args.CancelEventArg<List<OffBoardRequest>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<OffBoardRequest> LoadRequest(string baseArg)
        {
            List<OffBoardRequest> list = new List<OffBoardRequest>();
            try
            {
                list=Contract.GetOffBoardRequests();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }

            return list;
        }

        protected override void ExecuteContent()
        {
            try
            {
                var requestData = RequestData[0];
                if(requestData.FromStation==0) return;
                LogContract.InsertWcsLog($"拣选余料请求回库,站台：{requestData.FromStation}，垛号：{requestData.PilerNo}，花色：{requestData.ProductCode},站台：{requestData.FromStation}");
                var ret = Wms.ApplyBoardReenterStock(requestData.BookCount, requestData.FromStation, requestData.PilerNo);
                if (ret.Status == 200)
                {
                    var inStockTasks = WmsTaskContract.GetWmsTasksByReqId(ret.ReqId);
                    if (inStockTasks.Count == 0)
                    {
                        LogContract.InsertWcsErrorLog($"找不到请求Id：{ret.ReqId}的任务");
                        return;
                    }

                    LogContract.InsertWcsLog($"拣选余料请求回库成功，垛号：{requestData.PilerNo}，花色：{requestData.ProductCode},站台：{requestData.FromStation}");
                    requestData.ReqId = ret.ReqId;
                    requestData.IsRequest = true;
                    if (!Contract.UpdatedOffBoardRequest(requestData))
                    {
                        Contract.UpdatedOffBoardRequest(requestData);
                        Thread.Sleep(20);
                    }

                    var inStockTask = inStockTasks[0];
                    inStockTask.FromPosition = inStockTask.FromPosition == "0" ? "2003" : inStockTask.FromPosition;
                    var writeSuccess = Line.WriteInStockTask(Convert.ToInt32(inStockTask.FromPosition),
                        inStockTask.PilerNo.Value,
                        inStockTask.DdjNo.Value);
                    if (writeSuccess)
                    {
                        Thread.Sleep(50);
                        writeSuccess = Line.WriteInStockTask(Convert.ToInt32(inStockTask.FromPosition),
                            inStockTask.PilerNo.Value,
                            inStockTask.DdjNo.Value);
                    }
                }
                else
                {
                    LogContract.InsertWcsLog($"拣选余料请求回库失败：{ret.Message}，垛号：{requestData.PilerNo}，花色：{requestData.ProductCode},站台：{requestData.FromStation}");
                }

                LogContract.InsertWcsLog($"拣选余料请求回库成功");
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
