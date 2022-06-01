using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    public interface IStack
    {
        /// <summary>
        /// 板材总数
        /// </summary>
        int BookCount { get; }

        /// <summary>
        /// 直接拣选复杂度
        /// </summary>
        int DirectSortingCount { get; }

        /// <summary>
        /// 反向拣选复杂度
        /// </summary>
        int ReverseSortingCount { get; }
        
        /// <summary>
        /// 最小拣选复杂度
        /// </summary>
        int MinSortingCount { get; }
        
        /// <summary>
        /// 总耗时
        /// </summary>
        TimeSpan TotalTime { get; }

        /// <summary>
        /// 总秒数
        /// </summary>
        int TotalSeconds { get; }

        /// <summary>
        /// 花色数
        /// </summary>
        int ColorCount { get; }

        /// <summary>
        /// 垛的饱和度
        /// </summary>
        double StackSaturability { get; }

        /// <summary>
        /// 花色列表
        /// </summary>
        List<string> ColorList { get; }

    }
}
