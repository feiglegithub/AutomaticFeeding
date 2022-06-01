using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    /// <summary>
    /// 垛集合基础类
    /// </summary>
    public class StackInfoCollectionBase<T>
    where T:IStack
    {
        public List<T> StackInfos { get; set; }

        /// <summary>
        /// 垛的数量
        /// </summary>
        public int StackCount => StackInfos.Count;

        /// <summary>
        /// 最大耗时
        /// </summary>
        public TimeSpan MaxTimeSpan => StackInfos.Max(item => item.TotalTime);

        /// <summary>
        /// 最小耗时
        /// </summary>
        public TimeSpan MinTimeSpan => StackInfos.Min(item => item.TotalTime);

        /// <summary>
        /// 直径拣选复杂度
        /// </summary>
        public int DirectSortingCount => StackInfos.Sum(item => item.DirectSortingCount);

        /// <summary>
        /// 反向拣选复杂度
        /// </summary>
        public int ReverseSortingCount => StackInfos.Sum(item => item.ReverseSortingCount);

        /// <summary>
        /// 最小拣选复杂度
        /// </summary>
        public int MinSortingCount => StackInfos.Sum(item => item.MinSortingCount);

        /// <summary>
        /// 花色数
        /// </summary>
        public int ColorCount => ColorList.Count;

        /// <summary>
        /// 花色
        /// </summary>
        public string Colors => string.Join(",", ColorList);

        /// <summary>
        /// 花色列表
        /// </summary>
        public List<string> ColorList
        {
            get
            {
                List<string> colorList = new List<string>();
                StackInfos.ForEach(item => colorList.AddRange(item.ColorList));
                colorList = colorList.Distinct().ToList();
                return colorList;
            }
        }
        /// <summary>
        /// 垛的最大耗时差
        /// </summary>
        public TimeSpan StackMaxTimeDifference =>
            TimeSpan.FromSeconds(StackInfos.Max(item => item.TotalSeconds) - StackInfos.Min(item => item.TotalSeconds));

        /// <summary>
        /// 垛的平均饱和度
        /// </summary>
        public double StackSaturabilityAgv => StackInfos.Average(item => item.StackSaturability);
    }
}
