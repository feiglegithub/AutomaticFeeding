using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Interfaces;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices
{
    internal class DeviceComparer : IEqualityComparer<DeviceInfos>
    {
        public bool Equals(DeviceInfos x, DeviceInfos y)
        {
            return x.DeviceDescription == y.DeviceDescription && x.DeviceName == y.DeviceName && x.State == y.State;
        }

        public int GetHashCode(DeviceInfos obj)
        {
            return obj.GetType().GetHashCode();
        }
    }


    public class CurTaskListenService : ServiceBase<CurTaskListenService>
    {

        /// <summary>
        /// 当前任务详细数据 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingData = nameof(BroadcastBindingData);
        /// <summary>
        /// 监听消息 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingListenMsg = nameof(BroadcastBindingListenMsg);

        /// <summary>
        /// 当前设备的信息 -- 广播消息标签 
        /// </summary>
        public const string BroadcastBindingDeviceInfos = nameof(BroadcastBindingDeviceInfos);

        /// <summary>
        /// 当前设备的当前任务数据 -- 广播消息标签 
        /// </summary>
        public const string BroadcastBindingCurTask = nameof(BroadcastBindingCurTask);

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());

        private CurTaskListenService() { }

        private Thread thread = null;
        private Thread Th => thread ?? (thread = new Thread(GetCurTaskDetail) { IsBackground = true });

        public void Start()
        {
            if (!IsStart)
            {
                Th.Start();
                base.Start();
            }
        }

        public void Stop()
        {
            if (IsStart)
            {
                Th.Abort();
                base.Stop();
            }
        }

        private DeviceInfos CurDeviceInfos = null;

        private List<DeviceInfos> CurDeviceInfoses = null;

        private bool DeviceInfoHasChange(DeviceInfos deviceInfos,DeviceInfos other)
        {
            if (other == null || deviceInfos == null) return true;
            return other.DeviceName != deviceInfos.DeviceName || other.State != deviceInfos.State ||
                   other.DeviceDescription != deviceInfos.DeviceDescription;
        }

        private bool DeviceInfoHasChange(DeviceInfos deviceInfos)
        {
            return DeviceInfoHasChange(CurDeviceInfos, deviceInfos);
        }

        private bool DeviceInfosHasChange(List<DeviceInfos> deviceInfoses, List<DeviceInfos> others)
        {
            if (deviceInfoses == null || others == null) return true;
            if (deviceInfoses.Count != others.Count) return true;
            return !deviceInfoses.SequenceEqual(others, new DeviceComparer());
        }
        public bool CanStart { get; set; } = false;
        private void GetCurTaskDetail()
        {
            while (true)
            {
                if (CanStart)
                {
                    try
                    {
                        var divs = LineControlCuttingContract.GetCuttingDeviceInfos();
                        if (DeviceInfosHasChange(CurDeviceInfoses, divs))
                        {
                            CurDeviceInfoses = divs;
                            BroadcastMessage.Send(BroadcastBindingDeviceInfos, divs);
                        }

                        if (!string.IsNullOrWhiteSpace(CuttingServerSettings.Current.CurrDeviceName))
                        {
                            var div = divs.FirstOrDefault(item => item.DeviceName == CuttingServerSettings.Current.CurrDeviceName);
                            if (DeviceInfoHasChange(div))
                            {
                                CurDeviceInfos = div;
                                BroadcastMessage.Send(BroadcastBindingDeviceInfos, div);
                            }
                            //获取当前任务
                            var tasks = LineControlCuttingContract?.GetCurrCuttingTasks(
                                CuttingServerSettings.Current.CurrDeviceName, DateTime.Today);
                            if (tasks == null)
                            {
                                Thread.Sleep(3000);
                                continue;
                            }
                            if (tasks.Count > 0)
                            {
                                var task = tasks[0];
                                BroadcastMessage.Send(BroadcastBindingCurTask, task);
                                List<CuttingPattern> cuttingPatterns = LineControlCuttingContract.GetCuttingPatterns(task.BatchName, task.ItemName, task.PlanDate);
                                DataSet datas = LineControlCuttingContract.GetMDBDatas(task.ItemName);
                                List<CuttingTaskDetail> cuttingTaskDetails =
                                    LineControlCuttingContract.GetDeviceCuttingTaskDetail(CuttingServerSettings.Current.CurrDeviceName,
                                        task.ItemName, task.PlanDate);
                                //Regex.Match("", "[0-9]");

                                var ofcParts = cuttingTaskDetails.FindAll(item => item.IsOffPart);
                                var parts = cuttingTaskDetails.FindAll(item => !item.IsOffPart);
                                cuttingPatterns.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                                ofcParts.Sort((x, y) => x.NewPTN_INDEX.CompareTo(y.NewPTN_INDEX));
                                parts.Sort((x, y) => x.NewPTN_INDEX.CompareTo(y.NewPTN_INDEX));

                                BroadcastMessage.Send(BroadcastBindingData, new Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>>(cuttingPatterns, datas, parts, ofcParts));


                            }
                            else
                            {
                                //Send(CurTaskDetailControl.BindingCurTask,"暂时没有当前数据");
                                //SendTipsMessage("暂时没有当前数据");
                                BroadcastMessage.Send(BroadcastBindingCurTask, default(SpiltMDBResult));
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        BroadcastMessage.Send(BroadcastBindingListenMsg, e.Message);
                        Thread.Sleep(CuttingServerSettings.Current.RefreshTime);
                    }
                }
                
                Thread.Sleep(CuttingServerSettings.Current.RefreshTime);

            }
        }

    }
}
