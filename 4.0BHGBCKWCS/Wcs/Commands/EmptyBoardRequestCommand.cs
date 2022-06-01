using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using WcsConfig;
using WcsModel;
using WcsService;
using WCS.Helpers;
using WCS.Interfaces;

namespace WCS.Commands
{
    public class EmptyBoardRequestCommand:CommandBase<List<EmptyBoardRequest>,ESortingStation>
    {
        private DateTime FailedTime { get; set; }=DateTime.Now.AddMinutes(-10);
        private IEmptyBoardRequestContract _emptyBoardContract = null;

        private IEmptyBoardRequestContract EmptyBoardContract =>
            _emptyBoardContract ?? (_emptyBoardContract =  EmptyBoardRequestService.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        private IWms _wmsService = null;
        private IWms Wms => _wmsService ?? (_wmsService = WmsServiceHelper.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        public EmptyBoardRequestCommand(ESortingStation baseArg= ESortingStation.SortingStation2005) : base(baseArg)
        {
            this.Validating += EmptyBoardRequestCommand_Validating;
        }

        private void EmptyBoardRequestCommand_Validating(object arg1, Args.CancelEventArg<List<EmptyBoardRequest>> arg2)
        {
            arg2.Cancel = RequestData.Count == 0;
        }

        protected override List<EmptyBoardRequest> LoadRequest(ESortingStation baseArg)
        {
            List <EmptyBoardRequest> emptyBoardRequests = new List<EmptyBoardRequest>();
            try
            {
                emptyBoardRequests = EmptyBoardContract.GetEmptyBoardRequests();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }

            return emptyBoardRequests;
        }

        protected override void ExecuteContent()
        {
            try
            {
                if (FailedTime.AddSeconds(WcsSettings.Current.RequestWaiting) > DateTime.Now)
                {
                    return;
                }
                var request = RequestData[0];
                if (request.IsNeed)
                {
                    var ret = Wms.ApplyEmptyBoard(request.BookCount, request.ToStation);
                    if (ret.Status == 200)
                    {
                        request.ReqId = ret.ReqId;
                        request.IsRequest = true;
                        if (!EmptyBoardContract.UpdatedEmptyBoardRequest(request))
                        {
                            Thread.Sleep(100);
                            EmptyBoardContract.UpdatedEmptyBoardRequest(request);
                        }

                        LogContract.InsertWcsLog($"申请补空垫板成功");
                    }
                    else
                    {
                        FailedTime = DateTime.Now;
                        LogContract.InsertWcsErrorLog(GetType() + $"申请补空垫板失败,将在{WcsSettings.Current.RequestWaiting}s后再试：{ret.Message}");
                    }
                }
                else
                {
                    var ret = Wms.ApplyEmptyBoardInStock(request.BookCount, request.FromStation);

                    if (ret.Status == 200)
                    {
                        request.ReqId = ret.ReqId;
                        request.IsRequest = true;
                        if (!EmptyBoardContract.UpdatedEmptyBoardRequest(request))
                        {
                            Thread.Sleep(100);
                            EmptyBoardContract.UpdatedEmptyBoardRequest(request);
                        }

                        LogContract.InsertWcsLog($"申请空垫板回库成功");
                    }
                    else
                    {
                        FailedTime = DateTime.Now;
                        LogContract.InsertWcsErrorLog(GetType() + $"申请空垫板回库失败：{ret.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType()+ e.Message);
            }
        }
    }
}
