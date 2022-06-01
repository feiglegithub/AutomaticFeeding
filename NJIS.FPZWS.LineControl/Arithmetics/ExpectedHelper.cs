using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    public class ExpectedHelper
    {
        /// <summary>
        /// 计算元素的期望值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TNorm"></typeparam>
        /// <typeparam name="TExpected"></typeparam>
        /// <param name="tSources">源数据</param>
        /// <param name="tNorms">期望指标</param>
        /// <param name="selectNormFunc">筛选元素对应的指标</param>
        /// <param name="expectedFunc">计算元素的期望值</param>
        /// <returns></returns>
        public static ExpectedResult<TSource, TNorm, TExpected> ExpectedBase<TSource, TNorm, TExpected>(IEnumerable<TSource> tSources,IEnumerable<TNorm> tNorms,Func<TSource,IEnumerable<TNorm>,TNorm> selectNormFunc,Func<TSource,IEnumerable<TSource>,TNorm,IEnumerable<TNorm>,TExpected> expectedFunc)
        {
            ExpectedResult<TSource, TNorm, TExpected> expectedResult = new ExpectedResult<TSource, TNorm, TExpected>();

            foreach (var source in tSources)
            {
                var norm = selectNormFunc(source, tNorms);
                expectedResult.CombinationCollection.Add(new ExpectedResultBase<TSource, TNorm, TExpected>(){Expected = expectedFunc(source, tSources, norm,tNorms),Source = source,Norm = norm });
            }

            return expectedResult;
        }

        /// <summary>
        /// 计算元素的期望值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TNorm"></typeparam>
        /// <typeparam name="TExpected"></typeparam>
        /// <param name="tSources">源数据</param>
        /// <param name="tNorms">期望指标</param>
        /// <param name="selectNormFunc">筛选元素对应的指标</param>
        /// <param name="expectedFunc">计算元素的期望值</param>
        /// <returns></returns>
        public static ExpectedResult<TSource, TNorm, TExpected> ExpectedBase<TSource, TNorm, TExpected>(IEnumerable<TSource> tSources, IEnumerable<TNorm> tNorms, Func<TSource, IEnumerable<TNorm>, TNorm> selectNormFunc, Func<TSource, TNorm, TExpected> expectedFunc)
        {
            ExpectedResult<TSource, TNorm, TExpected> expectedResult = new ExpectedResult<TSource, TNorm, TExpected>();

            foreach (var source in tSources)
            {
                var norm = selectNormFunc(source, tNorms);
                expectedResult.CombinationCollection.Add(new ExpectedResultBase<TSource, TNorm, TExpected>() { Expected = expectedFunc(source, norm), Source = source, Norm = norm });
            }

            return expectedResult;
        }
    }

    public class ExpectedResultBase<TSource, TNorm, TExpected>
    {
        public TSource Source { get; set; }
        public TNorm Norm { get; set; }
        public TExpected Expected { get; set; }
    }

    public class ExpectedResult<TSource, TNorm, TExpected> : ICombinationResult<ExpectedResultBase<TSource, TNorm, TExpected>>
    {
        public List<ExpectedResultBase<TSource, TNorm, TExpected>> CombinationCollection
        { get ; set; } = new List<ExpectedResultBase<TSource, TNorm, TExpected>>();
    }
}
