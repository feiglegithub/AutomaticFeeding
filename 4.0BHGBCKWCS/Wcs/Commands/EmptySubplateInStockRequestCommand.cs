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
    /// 电子锯空垫板请求入库
    /// </summary>
    public class EmptySubplateInStockRequestCommand : CommandBase<bool, ECuttingEmptyStation>
    {
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());
        private CuttingEmptyStationCommunication _cuttingEmptyStationCommunication = null;

        private CuttingEmptyStationCommunication CuttingCommunication =>
            _cuttingEmptyStationCommunication ?? (_cuttingEmptyStationCommunication=CuttingEmptyStationCommunication.GetInstance());

        private IWms _wmsService = null;
        private IWms Wms => _wmsService ?? (_wmsService = WmsServiceHelper.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private IRgvContract _rgvContract = null;
        private IRgvContract RgvContract => _rgvContract ?? (_rgvContract = RgvService.GetInstance());

        protected override void ExecuteContent()
        {
            LogContract.InsertWcsLog($"收到电子锯空垫板回库请求");
            var resultMsg = Wms.ApplyEmptyBoardInStock(42, Convert.ToInt32(BaseArg));
            if (resultMsg.Status == 200)
            {
                LogContract.InsertWcsLog($"收到电子锯空垫板回库请求成功，请求Id：{resultMsg.ReqId}");
                var tasks = WmsTaskContract.GetWmsTasksByReqId(resultMsg.ReqId);
                if (tasks.Count == 0)
                {
                    //找不到请求id对应的任务
                    LogContract.InsertWcsErrorLog(GetType() + $"找不到请求id:{resultMsg.ReqId}对应的任务");
                    return;
                }

                var task = tasks[0];

                List<RGV_Task> rgvTasks = new List<RGV_Task>
                {
                    new RGV_Task
                    {
                        TaskType = 2, FromPosition = Convert.ToInt32(BaseArg), ToPosition = 3013, PilerNo = task.PilerNo
                    }
                };

                if (!(RgvContract.BulkInsertRgvTasks(rgvTasks) > 0))
                {
                    Thread.Sleep(20);
                    RgvContract.BulkInsertRgvTasks(rgvTasks);
                }

                LogContract.InsertWcsLog($"创建电子锯空垫板回库Rgv任务成功");

            }
            else
            {
                LogContract.InsertWcsLog($"收到电子锯空垫板回库请求失败:{resultMsg.Message}");
            }
        }

        protected override bool LoadRequest(ECuttingEmptyStation baseArg)
        {
            return CuttingCommunication.IsInStockRequest(BaseArg);
        }
        /// <summary>
        /// 电子锯空垫板请求入库
        /// </summary>
        /// <param name="eCuttingEmptyStation">电子锯空垫板站台</param>
        public EmptySubplateInStockRequestCommand(ECuttingEmptyStation eCuttingEmptyStation) : base(eCuttingEmptyStation)
        {
            Validating += EmptySubplateInStockCommand_Validating;
        }

        private void EmptySubplateInStockCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !RequestData;
        }
    }
}
