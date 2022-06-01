using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers.Arithmetics
{
    /// <summary>
    /// 方案筛选算法
    /// </summary>
    public class PermutationFilterAlgorithm
    {
        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Filter<TSource>(List<PermutationResult<TSource>> tSources, Predicate<PermutationResult<TSource>> match)
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
        public static List<PermutationResult<PermutationResult<TSource>>> Filter<TSource>(List<PermutationResult<PermutationResult<TSource>>> tSources, Predicate<PermutationResult<PermutationResult<TSource>>> match)
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
        public static List<PermutationResult<PermutationResult<TSource>>> Filter<TSource,TSelect>(List<PermutationResult<PermutationResult<TSource>>> tSources,Func<PermutationResult<PermutationResult<TSource>>, TSelect> selector ,Func<TSelect,bool> filter)
        {
            return tSources.FindAll(item => filter(selector(item)));
        }

    }
}
