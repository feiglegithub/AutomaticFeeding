using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Manager.Views;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class DeviceManageControlPresenter:PresenterBase
    {
        public const string AddDeviceInfo = nameof(AddDeviceInfo);

        public const string UpdateDeviceInfo = nameof(UpdateDeviceInfo);

        public const string GetDeviceInfos = nameof(GetDeviceInfos);

        public const string GetProcessNames = nameof(GetProcessNames);

        private string processName = "Cutting";

        private ILineControlCuttingContractPlus _contract = null;

        private ILineControlCuttingContractPlus Contract => 
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());

        private LineControlCuttingService lineControlCuttingService = new LineControlCuttingService();

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
                bool ret = Contract.BulkUpdateDeviceInfos(deviceInfos);
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
                bool ret = Contract.BulkInsertDeviceInfos(deviceInfos);
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
                var deviceInfos = lineControlCuttingService.GetDeviceInfosByProcessName(processName);
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
