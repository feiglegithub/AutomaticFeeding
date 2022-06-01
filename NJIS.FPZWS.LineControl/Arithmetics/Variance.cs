using System;
using System.Collections.Generic;
using System.Linq;

namespace Arithmetics
{
    /// <summary>
    /// 方差算法
    /// </summary>
    public class Variance
    {
        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TVarianceQuota"></typeparam>
        /// <typeparam name="TConvert"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="varianceFunc">方差值</param>
        /// <returns></returns>
        public static TVarianceQuota GetVarianceResults<TSource,TConvert,TVarianceQuota>(IEnumerable<TSource> tSources, Converter<TSource, TConvert> converter,Func<IEnumerable<TConvert>, TVarianceQuota> varianceFunc)
        {
            return varianceFunc(tSources.ToList().ConvertAll(converter));
        }

        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static double GetVarianceResults<TSource>(IEnumerable<TSource> tSources, Converter<TSource, double> converter)
        {
            return GetVarianceResults(tSources, converter,VarianceBase);
        }

        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static double GetVarianceResults<TSource>(IEnumerable<TSource> tSources, Converter<TSource, float> converter)
        {
            return GetVarianceResults(tSources, converter, VarianceBase);
        }

        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <param name="tSources"></param>
        /// <returns></returns>
        public static double GetVarianceResults(IEnumerable<double> tSources)
        {
            return VarianceBase(tSources);
        }

        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <param name="tSources"></param>
        /// <returns></returns>
        public static double GetVarianceResults(IEnumerable<float> tSources)
        {
            return VarianceBase(tSources);
        }

        /// <summary>
        /// 获取方差值
        /// </summary>
        /// <param name="tSources"></param>
        /// <returns></returns>
        public static double GetVarianceResults(IEnumerable<int> tSources)
        {
            return VarianceBase(tSources);
        }

        public static double VarianceBase(IEnumerable<double> values)
        {
            var count = values.Count();
            var avg = values.Average();
            double varianceSum = 0;
            foreach (var value in values)
            {
                varianceSum += Math.Pow(value - avg, 2);
            }

            return varianceSum / count;
        }
        /// <summary>
        /// 计算方差
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double VarianceBase(IEnumerable<float> values)
        {
            var count = values.Count();
            var avg = values.Average();
            double varianceSum = 0;
            foreach (var value in values)
            {
                varianceSum += Math.Pow(value - avg, 2);
            }

            return varianceSum / count;
        }

        /// <summary>
        /// 计算方差
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double VarianceBase(IEnumerable<int> values)
        {
            var count = values.Count();
            var avg = values.Average();
            double varianceSum = 0;
            foreach (var value in values)
            {
                varianceSum += Math.Pow(value - avg, 2);
            }

            return varianceSum / count;
        }
    }
}
