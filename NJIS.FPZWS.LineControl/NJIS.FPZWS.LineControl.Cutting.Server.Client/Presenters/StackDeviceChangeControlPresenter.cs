using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using Telerik.Windows.Diagrams.Core;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class StackDeviceChangeControlPresenter:PresenterBase
    {

        public const string GetData = nameof(GetData);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        public const string GetDevices = nameof(GetDevices);
        /// <summary>
        /// 保存
        /// </summary>
        public const string Save = nameof(Save);
        public const string BindingData = nameof(BindingData);

        public const string Delete = nameof(Delete);

        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());

        private List<DeviceInfos> deviceInfos = null;

        private DateTime CurrPlanDate = DateTime.Today;

        public StackDeviceChangeControlPresenter()
        {
            Register<string>(GetDevices, ExecuteGetDevices);
            Register<DateTime>(GetData, ExecuteGetData);
            Register<List<SpiltMDBResult>>(Save, ExecuteSave);
            Register<List<SpiltMDBResult>>(Delete, ExecuteDelete);
        }

        private void ExecuteGetDevices(object sender, string str = null)
        {
            try
            {
                deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
                var list = deviceInfos.Clone().ToList();
                list.RemoveAll(item => item.State.Value == 0);
                list.Sort((x, y) => string.Compare(x.DeviceDescription, y.DeviceDescription, StringComparison.Ordinal));
                Send(BindingData, sender,list);
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
            }
        }

        private void ExecuteGetData(object sender, DateTime planTime)
        {
            CurrPlanDate = planTime;
            try
            {
                var data = LineControlCuttingContract.GetSpiltMDBResults(planTime.Date);
                data.RemoveAll(item => item.FinishedStatus >= Convert.ToInt32(FinishedStatus.NeedToSaw));
                data = data.OrderBy(item => item.ActualPlanDate).ThenBy(item => item.BatchIndex)
                    .ThenBy(item => item.ItemIndex).ToList();
                Send(BindingData,sender,data);
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message,sender);
            }
        }

        private void ExecuteSave(object sender, List<SpiltMDBResult> stackInfos)
        {
            try
            {
                var data = LineControlCuttingContract.GetSpiltMDBResults(CurrPlanDate);
                //数据对比
                data.RemoveAll(item => item.FinishedStatus < Convert.ToInt32(FinishedStatus.NeedToSaw));
                if (data.Exists(item => stackInfos.Exists(stack => stack.ItemName == item.ItemName)))
                {
                    SendTipsMessage("数据有发生变更，请刷新后再操作", sender);
                    Send(BindingData,sender,false);
                    return;
                }
                var ret = LineControlCuttingContract.BulkUpdateSpiltMDBResults(stackInfos);
                Send(BindingData, sender, ret);
                SendTipsMessage(ret?"保存成功":"保存失败", sender);
                if (ret)
                {
                    ExecuteGetData(sender, CurrPlanDate);
                }

            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
                Send(BindingData, sender, false);
            }
        }


        private void ExecuteDelete(object sender, List<SpiltMDBResult> stackInfos)
        {
            try
            {
                var ret = LineControlCuttingContract.BulkDeleteSpiltMDBResults(stackInfos);
                Send(BindingData, sender, ret);
                SendTipsMessage(ret ? "删除成功" : "删除失败", sender);
                if (ret)
                {
                    ExecuteGetData(sender, CurrPlanDate);
                }
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
                Send(BindingData, sender, false);
            }
            

        }
    }
}
