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
    /// <summary>
    /// 请求要料
    /// </summary>
    public class RequestMaterialCommand : CommandBase<List<V_RequestMaterial>, string>
    {
        private IVRequestMaterialContract contract = VRequestMaterialService.GetInstance();
        private IWms wms = WmsServiceHelper.GetInstance();
        private DateTime FailedTime { get; set; } = DateTime.Now.AddMinutes(-10);
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());

        public RequestMaterialCommand(string baseArg="要料请求") : base(baseArg)
        {
            this.Validating += RequestMaterialCommand_Validating;
        }

        private void RequestMaterialCommand_Validating(object arg1, Args.CancelEventArg<List<V_RequestMaterial>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<V_RequestMaterial> LoadRequest(string baseArg)
        {
            List < V_RequestMaterial > list = new List<V_RequestMaterial>();
            try
            {
                list = contract.GetRequestMaterials();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
            //获取要料清单列表
            return list;
        }

        protected override void ExecuteContent()
        {
            try
            {
                if (FailedTime.AddSeconds(WcsSettings.Current.RequestWaiting) > DateTime.Now)
                {
                    return;
                }
                //向wms要料
                var requestMaterial = RequestData[0];
                var boardCount = requestMaterial.NeedBookCount.Value;// > 40 ? 40 : requestMaterial.NeedBookCount.Value;
                LogContract.InsertWcsLog(
                    $"拣选要料请求，请组Id：{requestMaterial.GroupId}，花色：{requestMaterial.ProductCode}，需求数量:{boardCount}");

                var ret = wms.ApplySortingMaterial(boardCount, 2003, requestMaterial.ProductCode);
                if (ret.Status == 200)
                {
                    var updatedResult = contract.InsertGroupRequestId(requestMaterial.GroupId, ret.ReqId);
                    if (!updatedResult)
                    {
                        Thread.Sleep(100);
                        updatedResult = contract.InsertGroupRequestId(requestMaterial.GroupId, ret.ReqId);
                    }
                    LogContract.InsertWcsLog(
                        $"拣选要料请求成功，请组Id：{requestMaterial.GroupId}，花色：{requestMaterial.ProductCode}，请求Id,{ret.ReqId}");
                }
                else
                {
                    FailedTime = DateTime.Now;
                    LogContract.InsertWcsErrorLog(
                        $"拣选要料请求失败，将在{WcsSettings.Current.RequestWaiting}s后再试:{ret.Message}，请组Id：{requestMaterial.GroupId}，花色：{requestMaterial.ProductCode}，请求Id,{ret.ReqId}");
                }

            }
            catch (Exception e)
            {
                //LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
        }
    }
}
