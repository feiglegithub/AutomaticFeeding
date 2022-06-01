using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels
{
    /// <summary>
    /// 垛信息
    /// </summary>
    public class StackInfo: IStack
    {
        private int MaxBookCount = 40;

        public StackInfo(string deviceName,string batchName,int maxBookCount=40)
        {
            MaxBookCount = maxBookCount;
            DeviceName = deviceName;
            BatchName = batchName;
        }

        /// <summary>
        /// 板材信息列表
        /// </summary>
        public List<StackBookInfo> StackBookInfos { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// 板材总数
        /// </summary>
        public int BookCount => StackBookInfos.Count;

        /// <summary>
        /// 直接拣选复杂度
        /// </summary>
        public int DirectSortingCount => BookCount;

        /// <summary>
        /// 反向拣选复杂度
        /// </summary>
        public int ReverseSortingCount
        {
            get
            {
                int sortingCount = 0;
                foreach (var group in StackBookInfos.GroupBy(item=>item.RawMaterialId))
                {
                    var list = group.ToList();
                    sortingCount += MaxBookCount - list.Count;
                }

                return sortingCount;
            }
        }


        /// <summary>
        /// 最小拣选复杂度
        /// </summary>
        public int MinSortingCount
        {
            get
            {
                int sortingCount = 0;
                foreach (var group in StackBookInfos.GroupBy(item => item.RawMaterialId))
                {
                    var list = group.ToList();
                    var reverseSortingCount = MaxBookCount - list.Count;
                    var directSortingCount = list.Count;
                    sortingCount += Math.Min(reverseSortingCount, directSortingCount);
                }
                return sortingCount;
            }
        }

        /// <summary>
        /// 总耗时
        /// </summary>
        public TimeSpan TotalTime => TimeSpan.FromSeconds(TotalSeconds);

        /// <summary>
        /// 总秒数
        /// </summary>
        public int TotalSeconds => StackBookInfos.Sum(item => item.CycleTime);

        /// <summary>
        /// 花色数
        /// </summary>
        public int ColorCount => ColorList.Count;

        /// <summary>
        /// 垛的饱和度
        /// </summary>
        public double StackSaturability => BookCount / (MaxBookCount * 1.0);

        /// <summary>
        /// 花色列表
        /// </summary>
        public List<string> ColorList =>
            (from stackBookInfo in StackBookInfos
            group stackBookInfo by stackBookInfo.RawMaterialId
            into t
            select t.Key).ToList();
            //StackBookInfos.GroupBy(item => item.RawMaterialId).ToList().ConvertAll(item => item.Key);

        

    }
}
