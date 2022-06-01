using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticsTest
{
    public class Solution
    {
        public int SolutionId { get; set; }
        /// <summary>
        /// 时间匹配偏离程度
        /// </summary>
        public double TimeDegree { get; set; }
        /// <summary>
        /// 堆叠率偏离程度
        /// </summary>
        public double OverlappingRateDegree { get; set; }
        /// <summary>
        /// 工件数量偏离程度
        /// </summary>
        public double PartNumRateDegree { get; set; }
        /// <summary>
        /// 工件速率偏离程度
        /// </summary>
        public double PartSpeedRateDegree { get; set; }
        /// <summary>
        /// 余料数量偏离程度
        /// </summary>
        public double OffPartNumRateDegree { get; set; }

    }
}
