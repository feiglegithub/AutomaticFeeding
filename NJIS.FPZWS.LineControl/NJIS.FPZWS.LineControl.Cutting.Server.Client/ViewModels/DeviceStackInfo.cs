using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    /// <summary>
    /// 设备的垛信息
    /// </summary>
    public class DeviceStackInfo:StackInfoCollectionBase<StackInfo>
    {
        public string DeviceName { get; set; }

        public DeviceStackInfo(string deviceName)
        {
            DeviceName = deviceName;
        }

        
    }
}
