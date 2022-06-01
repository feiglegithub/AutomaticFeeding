using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class DeviceManageControlPresenter:PresenterBase
    {
        public const string AddDeviceInfo = nameof(AddDeviceInfo);

        public const string UpdateDeviceInfo = nameof(UpdateDeviceInfo);

        public const string GetDeviceInfos = nameof(GetDeviceInfos);

        public const string GetProcessNames = nameof(GetProcessNames);

        private string processName = "Cutting";

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract => 
            _lineControlCuttingContract ?? (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>():new LineControlCuttingService());

        public DeviceManageControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<string>(GetDeviceInfos, ExecuteGetDeviceInfos);
            Register<string>(GetProcessNames, ExecuteGetProcessNames);
            Register<List<DeviceInfos>>(AddDeviceInfo, ExecuteAddDeviceInfos);
            Register<List<DeviceInfos>>(UpdateDeviceInfo, ExecuteUpdateDeviceInfos);
        }

        private void ExecuteUpdateDeviceInfos(object sender,List<DeviceInfos> deviceInfos)
        {
            var recipient = sender;
            try
            {
                bool ret = LineControlCuttingContract.BulkUpdateDeviceInfos(deviceInfos);
                Send(DeviceManageControl.SaveDeviceInfos, recipient,ret);
            }
            catch(Exception e)
            {
                SendTipsMessage("修改数据失败:\n" + e.Message, recipient);
            }
        }

        private void ExecuteAddDeviceInfos(object sender,List<DeviceInfos> deviceInfos)
        {
            var recipient = sender;
            try
            {
                bool ret = LineControlCuttingContract.BulkInsertDeviceInfos(deviceInfos);
                Send(DeviceManageControl.SaveDeviceInfos,recipient, ret);
                
            }
            catch (Exception e)
            {
                SendTipsMessage("添加数据失败:\n" + e.Message,recipient);
            }
        }

        private void ExecuteGetDeviceInfos(object sender,string processName)
        {
            var recipient = sender;
            try
            {
                var deviceInfos = LineControlCuttingContract.GetDeviceInfosByProcessName(processName);
                Send(DeviceManageControl.ReceiveDatas,recipient, deviceInfos);
            }
            catch(Exception e)
            {
                SendTipsMessage("获取设备信息失败:\n" + e.Message,recipient);
            }
            
        }

        private void ExecuteGetProcessNames(object sender,string strDisableValue)
        {
            var recipient = sender;
            Send(DeviceManageControl.ReceiveProcessNames,recipient, new List<string>() { processName });
        }


    }
}
