using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcsSortingAlgorithm
{
    /// <summary>
    /// 排列组合算法
    /// </summary>
    public class PermutationAlgorithm
    {
        private class tmpClass<TSource>
         {
             public bool IsMatch { get; set; } = false;
             public TSource Source { get; set; }
        }

        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">原始序列</param>
        /// <param name="comparer">元素比较器</param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Permutation<TSource>(IEnumerable<TSource> list, IEqualityComparer<TSource> comparer =null)
        {
            List<PermutationResult<TSource>> results = new List<PermutationResult<TSource>>();
            var convertSource = list.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch= false});
            foreach (var source in convertSource)
            {
                var tmpList = new List<TSource>();
                var result = new PermutationResult<TSource>();
                tmpList.Add(source.Source);
                
                while (true)
                {
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
        /// <param name="list">原始序列</param>
        /// <param name="funcComparer">元素比较器</param>
        /// <returns></returns>
        public static List<PermutationResult<TSource>> Permutation<TSource>(IEnumerable<TSource> list, Func<TSource,TSource,bool> funcComparer)
        {
            List<PermutationResult<TSource>> results = new List<PermutationResult<TSource>>();
            var convertSource = list.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            foreach (var source in convertSource)
            {
                var tmpList = new List<TSource>();
                var result = new PermutationResult<TSource>();
                tmpList.Add(source.Source);

                while (true)
                {
                    tmpClass<TSource> matchItem = null;
                    if (funcComparer != null)
                    {
                        matchItem = convertSource.FirstOrDefault(item =>
                        {
                            bool contains = false;
                            foreach (var tmpSource in tmpList)
                            {
                                contains = !funcComparer(item.Source, tmpSource);
                                if(contains) break;
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

    }

    public class PermutationResult<T>
    {
        public List<T> PermutationCollection { get; set; }
    }
}
