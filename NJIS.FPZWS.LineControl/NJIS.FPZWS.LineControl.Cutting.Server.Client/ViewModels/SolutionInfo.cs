using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    /// <summary>
    /// 建垛方案信息
    /// </summary>
    public class SolutionInfo: StackInfoCollectionBase<StackInfo>
    {
        /// <summary>
        /// 设备数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string BatchName { get; set; }

        public SolutionInfo(string batchName, int deviceCount)
        {
            DeviceCount = deviceCount;
            BatchName = batchName;
        }

       

    }
}
