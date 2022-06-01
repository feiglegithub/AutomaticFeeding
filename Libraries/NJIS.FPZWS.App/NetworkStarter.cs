//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.Cleaner
//   文 件 名：NetworkStarter.cs
//   创建时间：2019-05-11 19:48
//   作    者：
//   说    明：
//   修改时间：2019-05-11 19:48
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Net.NetworkInformation;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.App
{
    public class NetworkStarter : IModularStarter
    {
        private static ILogger _logger = LogManager.GetLogger(nameof(NetworkStarter));

        public void Start()
        {
            NetworkChange.NetworkAddressChanged += AddressChangedCallback;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            _logger.Info(string.Format($"NetworkAvailabilityChanged[{e.IsAvailable}]"));
        }

        public void Stop()
        {
        }

        public StarterLevel Level { get; }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            _logger.Info("-------------------------------------");
            foreach (var n in adapters)
            {
                var info =
                    $"Name:[{n.Name}],OperationalStatus[{n.OperationalStatus}],Description[{n.Description}],OperationalStatus[{n.OperationalStatus}],Speed[{n.Speed}],SupportsMulticast[{n.SupportsMulticast}]";
                _logger.Info(info);
            }
            _logger.Info("-------------------------------------");
        }
    }
}