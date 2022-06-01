using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Manager.Helpers.Arithmetics
{
    /// <summary>
    /// 组合匹配方案
    /// </summary>
    public class PermutationMatchAlgorithm
    {
        /// <summary>
        /// 计算偏差值
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TFirstSelect"></typeparam>
        /// <typeparam name="TSecondSelect"></typeparam>
        /// <typeparam name="TOffsetQuota"></typeparam>
        /// <param name="tFirsts"></param>
        /// <param name="tsSeconds"></param>
        /// <param name="tFirstFunc"></param>
        /// <param name="tSecondFunc"></param>
        /// <param name="offsetFunc">计算偏差指标</param>
        /// <param name="firstComparer"></param>
        /// <param name="secondComparer"></param>
        public static List<PermutationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>>> PermutationMatch
            <TFirst, TSecond,TFirstSelect,TSecondSelect, TOffsetQuota>
            (IEnumerable<TFirst> tFirsts, IEnumerable<TSecond> tsSeconds,
                Func<TFirst, TFirstSelect> tFirstFunc,
                Func<TSecond, TSecondSelect> tSecondFunc,
                Func<TFirstSelect,TSecondSelect, TOffsetQuota> offsetFunc, 
                IEqualityComparer<TFirst> firstComparer = null, 
                IEqualityComparer<TSecond> secondComparer = null)
        {
            List<MatchingDegree<TFirst, TSecond, TOffsetQuota>> matchingDegrees = new List<MatchingDegree<TFirst, TSecond, TOffsetQuota>>();

            foreach (var first in tFirsts)
            {
                foreach (var second in tsSeconds)
                {
                    matchingDegrees.Add(new MatchingDegree<TFirst, TSecond, TOffsetQuota>() { First = first,Second = second, OffsetQuota = offsetFunc(tFirstFunc(first), tSecondFunc(second)) });
                }
            }
            //构建匹配方案
            List<PermutationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>>> permutation = PermutationAlgorithm.Permutation(matchingDegrees, (x, y) =>
            {
                bool equals = false;
                if (firstComparer == null)
                {
                    equals |= x.First.Equals(y.First);
                }
                else
                {
                    equals |= firstComparer.Equals(x.First, y.First);
                }

                if (secondComparer == null)
                {
                    equals |= x.Second.Equals(y.Second);
                }
                else
                {
                    equals |= secondComparer.Equals(x.Second, y.Second);
                }

                return equals;
            });

            return permutation;
        }
    }

    /// <summary>
    /// 匹配程度（偏差）
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TOffsetQuota"></typeparam>
    public class MatchingDegree<TFirst, TSecond, TOffsetQuota>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
        /// <summary>
        /// 偏差指标
        /// </summary>
        public TOffsetQuota OffsetQuota { get; set; }
    }
}
