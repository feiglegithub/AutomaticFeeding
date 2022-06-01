using System;
using System.Collections.Generic;
using System.Linq;

namespace Arithmetics
{
    /// <summary>
    /// 组合匹配方案
    /// </summary>
    public class CombinationMatchHelper
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
        public static List<ICombinationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>>> CombinationMatch
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
            List<ICombinationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>>> permutation = CombinationHelper.Combination(matchingDegrees, (x, y) =>
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
        public static ICombinationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>> CombinationMinMatch
            <TFirst, TSecond, TFirstSelect, TSecondSelect, TOffsetQuota>
            (IEnumerable<TFirst> tFirsts, IEnumerable<TSecond> tsSeconds,
                Func<TFirst, TFirstSelect> tFirstFunc,
                Func<TSecond, TSecondSelect> tSecondFunc,
                Func<TFirstSelect, TSecondSelect, TOffsetQuota> offsetFunc,
                IEqualityComparer<TFirst> firstComparer = null,
                IEqualityComparer<TSecond> secondComparer = null)
        {
            List<MatchingDegree<TFirst, TSecond, TOffsetQuota>> matchingDegrees = new List<MatchingDegree<TFirst, TSecond, TOffsetQuota>>();

            foreach (var first in tFirsts)
            {
                foreach (var second in tsSeconds)
                {
                    matchingDegrees.Add(new MatchingDegree<TFirst, TSecond, TOffsetQuota>() { First = first, Second = second, OffsetQuota = offsetFunc(tFirstFunc(first), tSecondFunc(second)) });
                }
            }
            //构建匹配方案
            List<ICombinationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>>> permutation = CombinationHelper.Combination(matchingDegrees, (x, y) =>
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
            var min = permutation.Min(item => item.CombinationCollection.Min(item1 => item1.OffsetQuota));
            return permutation.First(item=>item.CombinationCollection.Exists(item1=>item1.OffsetQuota.Equals(min)));
        }

        ///// <summary>
        ///// 计算偏差值
        ///// </summary>
        ///// <typeparam name="TFirst"></typeparam>
        ///// <typeparam name="TSecond"></typeparam>
        ///// <typeparam name="TFirstSelect"></typeparam>
        ///// <typeparam name="TSecondSelect"></typeparam>
        ///// <typeparam name="TOffsetQuota"></typeparam>
        ///// <param name="tFirsts"></param>
        ///// <param name="tsSeconds"></param>
        ///// <param name="tFirstFunc"></param>
        ///// <param name="tSecondFunc"></param>
        ///// <param name="offsetFunc">计算偏差指标</param>
        ///// <param name="converter"></param>
        ///// <param name="variance1"></param>
        ///// <param name="secondComparer"></param>
        //public static ICombinationResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>> CombinationMinMatch
        //    <TFirst, TSecond, TFirstSelect, TSecondSelect, TOffsetQuota>
        //    (IEnumerable<TFirst> tFirsts, IEnumerable<TSecond> tsSeconds,
        //        Func<TFirst, TFirstSelect> tFirstFunc,
        //        Func<TSecond,List<TFirstSelect>, TSecondSelect> tSecondFunc,
        //        Func<TFirstSelect, TSecondSelect, TOffsetQuota> offsetFunc,
        //        Converter<TOffsetQuota, double> converter,out double variance
        //        )
        //{
        //    List<MatchingDegree<TFirst, TSecond, TOffsetQuota>> matchingDegrees = new List<MatchingDegree<TFirst, TSecond, TOffsetQuota>>();
        //    List<TFirstSelect> tFirstSelects = new List<TFirstSelect>();
        //    foreach (var first in tFirsts)
        //    {
        //        tFirstSelects.Add(tFirstFunc(first));
        //    }

        //    var ttt= CombinationHelper.NoIntersectionCombination(tFirsts,tsSeconds,(tFirst,tSecond)=> new MatchingDegree<TFirst, TSecond, TOffsetQuota>() { First = tFirst, Second = tSecond, OffsetQuota = offsetFunc(tFirstFunc(tFirst), tSecondFunc(tSecond, tFirstSelects))});

        //    var tttt = ttt.ConvertAll(combinationResult =>
        //    {
        //        var offsetQuotas = combinationResult.CombinationCollection.ConvertAll(item => item.OffsetQuota);
        //        var variance1 = Variance.GetVarianceResults(offsetQuotas, converter);
        //        return new {combinationResult, Variance= variance1 };
        //    });

        //    tttt=tttt.OrderBy(item => item.Variance).ToList();
        //    variance = tttt[0].Variance;
        //    return tttt[0].combinationResult;
        //}


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
        /// <param name="converter"></param>
        public static MatchDegreeResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>,double> CombinationMinMatch
            <TFirst, TSecond, TFirstSelect, TSecondSelect, TOffsetQuota>
            (IEnumerable<TFirst> tFirsts, IEnumerable<TSecond> tsSeconds,
                Func<TFirst, TFirstSelect> tFirstFunc,
                Func<TSecond, List<TFirstSelect>, TSecondSelect> tSecondFunc,
                Func<TFirstSelect, TSecondSelect, TOffsetQuota> offsetFunc,
                Converter<TOffsetQuota, double> converter//, out double variance
                )
        {
            
            List<TFirstSelect> tFirstSelects = new List<TFirstSelect>();
            foreach (var first in tFirsts)
            {
                tFirstSelects.Add(tFirstFunc(first));
            }

            var ttt = CombinationHelper.CollectionMatchCombination(tFirsts, tsSeconds, (tFirst, tSecond) => new MatchingDegree<TFirst, TSecond, TOffsetQuota>() { First = tFirst, Second = tSecond, OffsetQuota = offsetFunc(tFirstFunc(tFirst), tSecondFunc(tSecond, tFirstSelects)) });

            var tttt = ttt.ConvertAll(combinationResult =>
            {
                var offsetQuotas = combinationResult.CombinationCollection.ConvertAll(item => item.OffsetQuota);
                var variance1 = Variance.GetVarianceResults(offsetQuotas, converter);
                return new { combinationResult, Variance = variance1 };
            });

            tttt = tttt.OrderBy(item => item.Variance).ToList();
            var variance = tttt[0].Variance;
            return new MatchDegreeResult<MatchingDegree<TFirst, TSecond, TOffsetQuota>, double>(){CombinationCollection = tttt[0].combinationResult.CombinationCollection,Degree = tttt[0].Variance, };
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

    public class MatchDegreeResult<T, TDegree> : ICombinationResult<T>
    {
        /// <summary>
        /// 离散程度
        /// </summary>
        public TDegree Degree { get; set; }

        public List<T> CombinationCollection { get; set; }
    }


}
