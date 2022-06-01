using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using WcsService;
using WCS.Communications;
using WCS.Helpers;
using WCS.Interfaces;

namespace WCS.Commands
{
    /// <summary>
    /// 空垫板要料请求
    /// </summary>
    public class EmptySubplateCommand : CommandBase<int, string>
    {
        private EmptySubplateStationCommunication _communication = null;

        private EmptySubplateStationCommunication EmptySubplateStationCommunication =>
            _communication ?? (_communication = EmptySubplateStationCommunication.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());

        private IWmsTaskContract _iWmsTaskContract = null;

        private IWmsTaskContract WmsTaskContract =>
            _iWmsTaskContract ?? (_iWmsTaskContract = WmsService.GetInstance());

        private IWms _wmsService = null;
        private IWms Wms => _wmsService ?? (_wmsService = WmsServiceHelper.GetInstance());

        protected override void ExecuteContent()
        {
            try
            {
                var tasks = WmsTaskContract.GetEmptyPadTasks();
                if (tasks.Count == 0)
                {
                
                    ////请求补空垫板
                    //var result = Wms.ApplyEmptyBoard(5);
                    //if (result.Status == 200)//成功
                    //{
                    //    LogContract.InsertWcsLog($"请求补空垫成功");
                    //}
                    //else
                    //{
                    //    LogContract.InsertWcsErrorLog($"请求补空垫失败：{result.Message}");
                    //}
                }
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }

        protected override int LoadRequest(string baseArg)
        {
            return EmptySubplateStationCommunication.EmptySubplateCount;
        }

        public EmptySubplateCommand() : base("空垫板")
        {
            this.Validating += EmptySubplateCommand_Validating;
        }

        private void EmptySubplateCommand_Validating(object arg1, Args.CancelEventArg<int> arg2)
        {
            arg2.Cancel = arg2.RequestData > 0;
        }
    }
}
