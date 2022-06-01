using System;
using System.Collections.Generic;
using System.Linq;

namespace NJIS.FPZWS.LineControl.Manager.Helpers.Arithmetics
{
    /// <summary>
    /// 排列组合算法
    /// </summary>
    public sealed class PermutationAlgorithm
    {
        private PermutationAlgorithm() { }
        private class tmpClass<TSource>
        {
            public bool IsMatch { get; set; } = false;
            public TSource Source { get; set; }
        }

        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources">原始序列</param>
        /// <param name="comparer">元素比较器</param>
        /// <param name="minElementCount">组合最小元素个数</param>
        /// <param name="maxElementCount">组合最大元素个数</param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Permutation<TSource>(IEnumerable<TSource> tSources, IEqualityComparer<TSource> comparer = null,int minElementCount=1,int maxElementCount=0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;
            List<PermutationResult<TSource>> results = new List<PermutationResult<TSource>>();
            if (minElementCount >= tSources.Count())
            {
                results.Add(new PermutationResult<TSource>(){PermutationCollection = tSources.ToList()});
                return results;
            }
            var convertSource = tSources.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            for (int i = 0; i <= convertSource.Count-minElementCount; i++)
            {
                tmpClass<TSource> source = convertSource[i];
                var tmpList = new List<TSource>();
                var result = new PermutationResult<TSource>();
                tmpList.Add(source.Source);

                while (true)
                {
                    if (tmpList.Count == maxElementCount)
                    {
                        break;
                    }
                    var matchItem = convertSource.FirstOrDefault(item =>
                        item.IsMatch == false && !tmpList.Contains(item.Source, comparer));
                    if (matchItem == null)
                    {
                        break;
                    }
                    tmpList.Add(matchItem.Source);
                    
                }
                source.IsMatch = true;
                result.PermutationCollection = tmpList;
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="tSources">原始序列</param>
        /// <param name="permutationSelector">组合特征</param>
        /// <param name="permutationFilter">组合特征过滤器</param>
        /// <param name="comparer">元素比较器</param>
        /// <param name="minElementCount">组合最小元素个数</param>
        /// <param name="maxElementCount">组合最大元素个数</param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Permutation<TSource,TSelector>(IEnumerable<TSource> tSources,Func<PermutationResult<TSource>, TSelector> permutationSelector,Func<TSelector,bool> permutationFilter, IEqualityComparer<TSource> comparer = null, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;
            List<PermutationResult<TSource>> results = new List<PermutationResult<TSource>>();
            if (minElementCount >= tSources.Count())
            {
                results.Add(new PermutationResult<TSource>() { PermutationCollection = tSources.ToList() });
                return results;
            }
            var convertSource = tSources.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            for (int i = 0; i <= convertSource.Count - minElementCount; i++)
            {
                tmpClass<TSource> source = convertSource[i];
                var result = new PermutationResult<TSource>();
                result.PermutationCollection = new List<TSource>();
                result.PermutationCollection.Add(source.Source);

                while (true)
                {
                    if (result.PermutationCollection.Count == maxElementCount)
                    {
                        break;
                    }
                    var matchItem = convertSource.FirstOrDefault(item =>
                        item.IsMatch == false && !result.PermutationCollection.Contains(item.Source, comparer));
                    if (matchItem == null)
                    {
                        break;
                    }
                    result.PermutationCollection.Add(matchItem.Source);
                    
                }
                source.IsMatch = true;

                results.Add(result);
            }
            results.RemoveAll(item => permutationFilter(permutationSelector(item)));
            return results;
        }


        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">原始序列</param>
        /// <param name="funcComparer">元素比较器</param>
        /// <param name="minElementCount">组合最小元素个数</param>
        /// <param name="maxElementCount">组合最大元素个数</param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Permutation<TSource>(IEnumerable<TSource> list, Func<TSource, TSource, bool> funcComparer, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? list.Count() : maxElementCount;
            List<PermutationResult<TSource>> results = new List<PermutationResult<TSource>>();
            if (minElementCount >= list.Count())
            {
                results.Add(new PermutationResult<TSource>() { PermutationCollection = list.ToList() });
                return results;
            }
            var convertSource = list.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            for (int i = 0; i <= convertSource.Count-minElementCount; i++)
            {
                tmpClass<TSource> source = convertSource[i];
                var tmpList = new List<TSource>();
                var result = new PermutationResult<TSource>();
                tmpList.Add(source.Source);

                while (true)
                {
                    if (tmpList.Count == maxElementCount)
                    {
                        break;
                    }
                    tmpClass<TSource> matchItem = null;
                    if (funcComparer != null)
                    {
                        matchItem = convertSource.FirstOrDefault(item =>
                        {
                            bool contains = false;
                            foreach (var tmpSource in tmpList)
                            {
                                contains = !funcComparer(item.Source, tmpSource);
                                if (contains) break;
                            }
                            return item.IsMatch == false && !contains;
                        });
                    }
                    else
                    {
                        matchItem = convertSource.FirstOrDefault(item =>
                            item.IsMatch == false && !tmpList.Contains(item.Source));
                    }

                    if (matchItem == null)
                    {
                        source.IsMatch = true;
                        break;
                    }
                    tmpList.Add(matchItem.Source);
                }
                result.PermutationCollection = tmpList;
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="permutationCount"></param>
        /// <param name="comparer"></param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <returns></returns>
        public static List<PermutationResult<PermutationResult<TSource>>> NoIntersectionPermutation<TSource>(IEnumerable<TSource> tSources,int permutationCount, IEqualityComparer<TSource> comparer = null, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;

            List<PermutationResult<TSource>> permutations = Permutation(tSources, comparer, minElementCount, maxElementCount);

            List<PermutationResult<PermutationResult<TSource>>> ps = Permutation(permutations, (first, second) =>
            {
                var intersect = first.PermutationCollection.Intersect(second.PermutationCollection, comparer);
                return intersect.Count() == 0;
            }, permutationCount);

            ps.RemoveAll(permutation =>
            {
                IEnumerable<TSource> tEnumerable = new List<TSource>();
                foreach (var p in permutation.PermutationCollection)
                    tEnumerable = tEnumerable.Union(p.PermutationCollection, comparer);
                return tEnumerable.Except(tSources, comparer).Any();
            });

            return ps;
        }


        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="permutationCount"></param>
        /// <param name="funcComparer"></param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <returns></returns>
        public static List<PermutationResult<PermutationResult<TSource>>> NoIntersectionPermutation<TSource>(IEnumerable<TSource> tSources, int permutationCount, Func<TSource, TSource, bool> funcComparer, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;

            List<PermutationResult<TSource>> permutations = Permutation(tSources, funcComparer, minElementCount, maxElementCount);

            List<PermutationResult<PermutationResult<TSource>>> ps = Permutation(permutations, (first, second) =>
            {
                var intersect = first.PermutationCollection.Intersect(second.PermutationCollection,new FuncEqualityComparer<TSource>(funcComparer));
                return intersect.Count() == 0;
            }, permutationCount);

            return ps;
        }

    }

    public class FuncEqualityComparer<TSource> : IEqualityComparer<TSource>
    {
        private Func<TSource, TSource, bool> func = null;
        public FuncEqualityComparer(Func<TSource, TSource, bool> func)
        {
            this.func = func;
        }
        public bool Equals(TSource x, TSource y)
        {
            return func(x, y);
        }

        public int GetHashCode(TSource obj)
        {
            return typeof(TSource).GetHashCode();
        }
    }

    public class PermutationResult<T>: IPermutationResult<T>
    {
        public List<T> PermutationCollection { get; set; }
    }
}
