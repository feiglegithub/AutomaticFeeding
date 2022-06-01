using System;
using System.Collections.Generic;

namespace Arithmetics
{
    /// <summary>
    /// 方案筛选算法
    /// </summary>
    public class CombinationFilter
    {
        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<CombinationResult<TSource>> Filter<TSource>(List<CombinationResult<TSource>> tSources, Predicate<CombinationResult<TSource>> match)
        {
            return tSources.FindAll(match);
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<CombinationResult<CombinationResult<TSource>>> Filter<TSource>(List<CombinationResult<CombinationResult<TSource>>> tSources, Predicate<CombinationResult<CombinationResult<TSource>>> match)
        {
            return tSources.FindAll(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="selector"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<CombinationResult<CombinationResult<TSource>>> Filter<TSource,TSelect>(List<CombinationResult<CombinationResult<TSource>>> tSources,Func<CombinationResult<CombinationResult<TSource>>, TSelect> selector ,Func<TSelect,bool> filter)
        {
            return tSources.FindAll(item => filter(selector(item)));
        }

    }
}
