using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Presenters
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

        private ILineControlCuttingContractPlus _lineControlCuttingContract = null;

        private ILineControlCuttingContractPlus Contract =>
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());

        private void ExecuteSetTaskError(string mdbName)
        {
            if (string.IsNullOrWhiteSpace(mdbName))
            {
                SendTipsMessage("当前任务为空，无法提交！");
            }
            var msg = Contract.CommitErrorTask(mdbName);
            SendTipsMessage(msg);
            //Contract.CommitTaskError(taskName);
        }

        private void GetCurTaskDetail()
        {
            while (true)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(CuttingSetting.Current.CurrDeviceName))
                    {
                        var deviceName = CuttingSetting.Current.CurrDeviceName;
                        var divs = Contract.GetCuttingDeviceInfos();
                        var div = divs.FirstOrDefault(item => item.DeviceName == deviceName);
                        Send(CurTaskDetailControl.BindingDeviceInfos, div);
                        var cutStacks = Contract.GetStacksByDevice(deviceName, StackStatus.Cutting);
                        var cutStack = cutStacks.FirstOrDefault();
                        if (cutStack == null)
                        {
                            cutStacks = Contract.GetStacksByDevice(deviceName, StackStatus.LoadedMaterial);
                            cutStack = cutStacks.FirstOrDefault();
                        }
                        List<StackDetail> stackDetails = new List<StackDetail>();
                        if (cutStack != null)
                        {
                            stackDetails = Contract.GetStackDetailsByStackName(cutStack.StackName);
                        }

                        var cuttingTasks = Contract.GetCuttingPatternsByDevice(deviceName).OrderByDescending(item=>item.UpdatedTime).ToList();
                        
                        var cuttingTask = cuttingTasks.FirstOrDefault();

                        List<PatternDetail> patternDetails = new List<PatternDetail>();
                        if (cuttingTask != null)
                        {
                            patternDetails = Contract
                                .GetPatternDetailsByPattern(cuttingTask.BatchName, cuttingTask.PatternId);
                        }
                        var nextTasks = Contract.GetNextPatternsByDevice(deviceName).OrderByDescending(item => item.UpdatedTime).ToList();

                        var nextTask = nextTasks.FirstOrDefault();
                        Send(CurTaskDetailControl.BindingDatas,
                            new Tuple<Pattern, List<PatternDetail>, Pattern, List<StackDetail>>(cuttingTask, patternDetails, nextTask, stackDetails));
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
                var deviceInfos = Contract.GetCuttingDeviceInfos();
                var div = deviceInfos.FirstOrDefault(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
                if (div == null)
                {
                    SendTipsMessage($"无法找到设备{CuttingSetting.Current.CurrDeviceName}");

                }

                div.State = enable ? 1 : 0;
                Contract.BulkUpdateDeviceInfos(new List<DeviceInfos>() { div });
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message);
            }
        }
    }

}
