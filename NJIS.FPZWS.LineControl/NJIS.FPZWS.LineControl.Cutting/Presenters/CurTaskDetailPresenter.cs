using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Presenters
{
    public class CurTaskDetailPresenter : PresenterBase
    {
        private Thread _thread = null;

        public const string Begin = nameof(Begin);
        public const string SetDeviceStatus = nameof(SetDeviceStatus);
        public const string SetTaskError = nameof(SetTaskError);
        private static CurTaskDetailPresenter _presenter = null;
        private static object lockObj = new object();
        public static CurTaskDetailPresenter GetInstance()
        {
            if (_presenter == null)
            {
                lock (lockObj)
                {
                    if (_presenter == null)
                        _presenter = new CurTaskDetailPresenter();
                }
            }
            return _presenter;
        }
        private CurTaskDetailPresenter()
        {
            Register<string>(Begin, Execute);
            Register<bool>(SetDeviceStatus, ExecuteSetDeviceStatus);
            Register<string>(SetTaskError, ExecuteSetTaskError);
        }

        private void Execute(string str)
        {
            if (_thread != null) return;
            _thread = new Thread(GetCurTaskDetail);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());

        //private IWorkStationContract _workStationContract = null;

        //private IWorkStationContract LineControlCuttingContract =>
        //    _workStationContract ?? (_workStationContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<IWorkStationContract>(): new WorkStationService());

        private void ExecuteSetTaskError(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName)) return;
            LineControlCuttingContract.CommitTaskError(taskName);
        }

        private void GetCurTaskDetail()
        {
            while (true)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(CuttingSetting.Current.CurrDeviceName))
                    {
                        var divs = LineControlCuttingContract.GetCuttingDeviceInfos();
                        var div = divs.FirstOrDefault(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
                        Send(CurTaskDetailControl.BindingDeviceInfos, div);
                        //获取当前任务
                        var tasks = LineControlCuttingContract?.GetCurrCuttingTasks(
                            CuttingSetting.Current.CurrDeviceName, DateTime.Today);
                        if (tasks == null)
                        {
                            Thread.Sleep(3000);
                            continue;
                        }
                        if (tasks.Count > 0)
                        {
                            var task = tasks[0];
                            Send(CurTaskDetailControl.BindingCurTask, task);
                            List<CuttingPattern> cuttingPatterns = LineControlCuttingContract.GetCuttingPatterns(task.BatchName, task.ItemName, task.PlanDate);
                            DataSet datas = LineControlCuttingContract.GetMDBDatas(task.ItemName);
                            List<CuttingTaskDetail> cuttingTaskDetails =
                                LineControlCuttingContract.GetDeviceCuttingTaskDetail(CuttingSetting.Current.CurrDeviceName,
                                    task.ItemName, task.PlanDate);
                            //Regex.Match("", "[0-9]");

                            var ofcParts = cuttingTaskDetails.FindAll(item => item.IsOffPart);
                            var parts = cuttingTaskDetails.FindAll(item => !item.IsOffPart);
                            cuttingPatterns.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                            ofcParts.Sort((x, y) => x.NewPTN_INDEX.CompareTo(y.NewPTN_INDEX));
                            parts.Sort((x, y) => x.NewPTN_INDEX.CompareTo(y.NewPTN_INDEX));

                            Send(CurTaskDetailControl.BindingDatas, new Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>>(cuttingPatterns, datas, parts, ofcParts));


                        }
                        else
                        {
                            //Send(CurTaskDetailControl.BindingCurTask,"暂时没有当前数据");
                            //SendTipsMessage("暂时没有当前数据");
                            Send(CurTaskDetailControl.BindingDatas, default(SpiltMDBResult));
                        }
                    }

                }
                catch (Exception e)
                {
                    this.SendTipsMessage(e.Message);
                    Thread.Sleep(CuttingSetting.Current.RefreshTime);
                }
                Thread.Sleep(CuttingSetting.Current.RefreshTime);

            }
        }

        private void ExecuteSetDeviceStatus(bool enable)
        {
            try
            {
                var deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
                var div = deviceInfos.FirstOrDefault(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
                if (div == null)
                {
                    SendTipsMessage($"无法找到设备{CuttingSetting.Current.CurrDeviceName}");

                }

                div.State = enable ? 1 : 0;
                LineControlCuttingContract.BulkUpdateDeviceInfos(new List<DeviceInfos>() { div });
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message);
            }
        }

        private void GetFinishedPattern(string itemName, DateTime planDate)
        {

        }

        private void GetCuttingPattern(string itemName, DateTime planDate)
        {

        }
    }

}
