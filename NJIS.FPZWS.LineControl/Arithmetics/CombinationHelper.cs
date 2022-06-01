using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Arithmetics
{
    /// <summary>
    /// 组合算法
    /// </summary>
    public sealed class CombinationHelper
    {
        private CombinationHelper() { }
        private class tmpClass<TSource>
        {
            public bool IsMatch { get; set; } = false;
            public TSource Source { get; set; }
        }

        //public static List<int[]> Combination(int n, int[] ints)
        //{

        //    List<int[]> collection = new List<int[]>();
        //    // C(n,m)=C(n,m-1)+C(n-1,m-1)
        //    int m = ints.Length;
        //    if (n == 1)
        //    {
        //        foreach (var item in ints)
        //        {
        //            collection.Add(new int[] { item });
        //        }

        //        return collection;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < m; i++)
        //        {
        //            List<int[]> collectionItems = new List<int[]>();
        //            var curItem = ints[i];

        //            var tmpInts = ints.Skip(i + 1).ToArray();
        //            var combinationValues = Combination(n - 1, tmpInts);
        //            foreach (var item in combinationValues)
        //            {
        //                int[] collectionItem = new int[] { curItem };
        //                collectionItem = collectionItem.Concat(item).ToArray();
        //                collectionItems.Add(collectionItem);
        //            }

        //            collection = collection.Concat(collectionItems).ToList();
        //        }

        //    }

        //    return collection;
        //}

        /// <summary>
        /// 构建组合[ C(n,m) ]
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="n">组合的元素</param>
        /// <param name="tSources"></param>
        /// <returns></returns>
        //public static LinkedList<ICombinationResult<TSource>> Combination<TSource>(int n, IEnumerable<TSource> tSources)
        public static List<ICombinationResult<TSource>> Combination<TSource>(int n, IEnumerable<TSource> tSources)
        {

            //LinkedList<ICombinationResult<TSource>> collection = new LinkedList<ICombinationResult<TSource>>();
            List<ICombinationResult<TSource>> collection = new List<ICombinationResult<TSource>>();
            // C(n,m)=C(n,m-1)+C(n-1,m-1)
            int m = tSources.Count();
            //if(m==0) 
            //    throw new Exception("集合元素总数不能为0");
            if (n == 1)
            {
                foreach (var item in tSources)
                {
                    var t = new CombinationResult<TSource>();
                    t.CombinationCollection.Add(item);
                    //t.CombinationCollection.AddLast(item);
                    collection.Add(t);
                    //collection.AddLast(t);
                }
                
                return collection;
            }
            else
            {
                var g = tSources.GetEnumerator();
                int i = 0;
                while (g.MoveNext())
                {
                    //LinkedList<ICombinationResult<TSource>> collectionItems = new LinkedList<ICombinationResult<TSource>>();
                    List<ICombinationResult<TSource>> collectionItems = new List<ICombinationResult<TSource>>();
                    var curItem = g.Current;

                    var tmpInts = tSources.Skip(i + 1).ToArray();
                    var combinationValues = Combination(n - 1, tmpInts);
                    foreach (var item in combinationValues)
                    {
                        ICombinationResult<TSource> collectionItem = new CombinationResult<TSource>();
                        collectionItem.CombinationCollection.Add(curItem);
                        
                        collectionItem.CombinationCollection.AddRange(item.CombinationCollection);
                        collectionItems.Add(collectionItem);
                        
                    }

                    collection = collection.Concat(collectionItems).ToList();
                    i++;
                }
            }

            
            return collection;
        }


        //public static List<ICombinationResult<TFirstSource>> Combination<TFirstSource, TSecondSource>(IEnumerable<TFirstSource> tFirstSources, IEnumerable<TSecondSource> tSecondSources)
        //{

        //}



        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources">原始序列</param>
        /// <param name="comparer">元素比较器</param>
        /// <param name="minElementCount">组合最小元素个数</param>
        /// <param name="maxElementCount">组合最大元素个数</param>
        /// <returns></returns>
        public static List<ICombinationResult<TSource>> Combination<TSource>(IEnumerable<TSource> tSources, IEqualityComparer<TSource> comparer = null,int minElementCount=1,int maxElementCount=0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;
            List<ICombinationResult<TSource>> results = new List<ICombinationResult<TSource>>();
            HashSet<TSource> hashSet = new HashSet<TSource>(tSources,comparer);
            //double totall = 1;
            //for (int i = minElementCount; i <= tSources.Count(); i++)
            //{
            //    totall *= i;
            //}
            for (int i = minElementCount; i <= maxElementCount; i++)
            {
                var t = Combination(i, hashSet);
                results.AddRange(t);
            }

            return results;

            #region Old Code


            //if (minElementCount >= tSources.Count())
            //{
            //    results.Add(new CombinationResult<TSource>() { CombinationCollection = tSources.ToList() });
            //    return results;
            //}
            //var convertSource = tSources.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            //for (int i = 0; i <= convertSource.Count - minElementCount; i++)
            //{
            //    tmpClass<TSource> source = convertSource[i];
            //    var tmpList = new List<TSource>();
            //    var result = new CombinationResult<TSource>();
            //    tmpList.Add(source.Source);

            //    while (true)
            //    {
            //        if (tmpList.Count == maxElementCount)
            //        {
            //            break;
            //        }
            //        var matchItem = convertSource.FirstOrDefault(item =>
            //            item.IsMatch == false && !tmpList.Contains(item.Source, comparer));
            //        if (matchItem == null)
            //        {
            //            break;
            //        }
            //        tmpList.Add(matchItem.Source);

            //    }
            //    source.IsMatch = true;
            //    result.CombinationCollection = tmpList;
            //    results.Add(result);
            //}
            //return results;

            #endregion

        }

        /// <summary>
        /// 构建组合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="tSources">原始序列</param>
        /// <param name="combinationSelector">组合特征</param>
        /// <param name="combinationFilter">组合特征过滤器</param>
        /// <param name="comparer">元素比较器</param>
        /// <param name="minElementCount">组合最小元素个数</param>
        /// <param name="maxElementCount">组合最大元素个数</param>
        /// <returns></returns>
        public static List<ICombinationResult<TSource>> Combination<TSource,TSelector>(IEnumerable<TSource> tSources,Func<CombinationResult<TSource>, TSelector> combinationSelector,Func<TSelector,bool> combinationFilter, IEqualityComparer<TSource> comparer = null, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;
            List<ICombinationResult<TSource>> results = new List<ICombinationResult<TSource>>();
            HashSet<TSource> hashSet = new HashSet<TSource>(tSources, comparer);
            for (int i = minElementCount; i < maxElementCount; i++)
            {
                results.AddRange(Combination(i, hashSet));
            }

            return results;


            #region Old Code

            //List<CombinationResult<TSource>> results = new List<CombinationResult<TSource>>();
            //if (minElementCount >= tSources.Count())
            //{
            //    results.Add(new CombinationResult<TSource>() { CombinationCollection = tSources.ToList() });
            //    results.RemoveAll(item => combinationFilter(combinationSelector(item)));
            //    return results;
            //}
            //var convertSource = tSources.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            //for (int i = 0; i <= convertSource.Count - minElementCount; i++)
            //{
            //    tmpClass<TSource> source = convertSource[i];
            //    var result = new CombinationResult<TSource>();
            //    result.CombinationCollection = new List<TSource>();
            //    result.CombinationCollection.Add(source.Source);

            //    while (true)
            //    {
            //        if (result.CombinationCollection.Count == maxElementCount)
            //        {
            //            break;
            //        }
            //        var matchItem = convertSource.FirstOrDefault(item =>
            //            item.IsMatch == false && !result.CombinationCollection.Contains(item.Source, comparer));
            //        if (matchItem == null)
            //        {
            //            break;
            //        }
            //        result.CombinationCollection.Add(matchItem.Source);

            //    }
            //    source.IsMatch = true;

            //    results.Add(result);
            //}
            //results.RemoveAll(item => combinationFilter(combinationSelector(item)));
            //return results;

            #endregion

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
        public static List<ICombinationResult<TSource>> Combination<TSource>(IEnumerable<TSource> list, Func<TSource, TSource, bool> funcComparer, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? list.Count() : maxElementCount;
            List<ICombinationResult<TSource>> results = new List<ICombinationResult<TSource>>();
            if (minElementCount >= list.Count())
            {
                results.Add(new CombinationResult<TSource>() { CombinationCollection = list.ToList() });
                return results;
            }

            var comparer = new FuncEqualityComparer<TSource>(funcComparer);

            results = Combination(list, comparer, minElementCount, maxElementCount);
            return results;

            #region Old Code

            //var convertSource = list.ToList().ConvertAll(item => new tmpClass<TSource> { Source = item, IsMatch = false });
            //for (int i = 0; i <= convertSource.Count-minElementCount; i++)
            //{
            //    tmpClass<TSource> source = convertSource[i];
            //    var tmpList = new List<TSource>();
            //    var result = new CombinationResult<TSource>();
            //    tmpList.Add(source.Source);

            //    while (true)
            //    {
            //        if (tmpList.Count == maxElementCount)
            //        {
            //            break;
            //        }
            //        tmpClass<TSource> matchItem = null;
            //        if (funcComparer != null)
            //        {
            //            matchItem = convertSource.FirstOrDefault(item =>
            //            {
            //                bool contains = false;
            //                foreach (var tmpSource in tmpList)
            //                {
            //                    contains = !funcComparer(item.Source, tmpSource);
            //                    if (contains) break;
            //                }
            //                return (item.IsMatch == false) && !contains;
            //            });
            //        }
            //        else
            //        {
            //            matchItem = convertSource.FirstOrDefault(item =>
            //                item.IsMatch == false && !tmpList.Contains(item.Source));
            //        }

            //        if (matchItem == null)
            //        {
            //            source.IsMatch = true;
            //            break;
            //        }
            //        tmpList.Add(matchItem.Source);
            //    }
            //    result.CombinationCollection = tmpList;
            //    results.Add(result);
            //}

            //return results;
            #endregion
        }

        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="combinationCount">组合（方案）数量</param>
        /// <param name="comparer"></param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <returns></returns>
        public static List<ICombinationResult<ICombinationResult<TSource>>> NoIntersectionCombination<TSource>(IEnumerable<TSource> tSources,int combinationCount, IEqualityComparer<TSource> comparer = null, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() - (combinationCount * minElementCount) + minElementCount : maxElementCount;
            maxElementCount = combinationCount<=tSources.Count()?maxElementCount:0;
            List<ICombinationResult<TSource>> combinations = Combination(tSources, comparer, minElementCount, maxElementCount);
            //var ta = ArrangementHelper.Arrangement(tSources.ToArray());
            var tt = Combination(tSources.Count()>combinationCount?combinationCount: tSources.Count(), combinations);

            tt = tt.FindAll(item =>
                item.CombinationCollection.SelectMany(pp=>pp.CombinationCollection).Distinct().Count()== tSources.Count()
                &&item.CombinationCollection.Sum(item1 => item1.CombinationCollection.Count) == tSources.Count()
            );

            tt.RemoveAll(combination =>
            {
                IEnumerable<TSource> tEnumerable = new List<TSource>();
                foreach (var p in combination.CombinationCollection)
                    tEnumerable = tEnumerable.Union(p.CombinationCollection, comparer);
                return tEnumerable.Except(tSources, comparer).Any();
            });

            //List<ICombinationResult<ICombinationResult<TSource>>> ps = Combination(combinations, (first, second) =>
            //{
            //    var intersect = first.CombinationCollection.Intersect(second.CombinationCollection);
            //    return intersect.Count() == 0;
            //}, combinationCount, combinationCount);

            //ps.RemoveAll(combination =>
            //{
            //    IEnumerable<TSource> tEnumerable = new List<TSource>();
            //    foreach (var p in combination.CombinationCollection)
            //        tEnumerable = tEnumerable.Union(p.CombinationCollection, comparer);
            //    return tEnumerable.Except(tSources, comparer).Any();
            //});

            return tt;
        }

        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="combinationCount">组合（方案）数量</param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <returns></returns>
        public static List<ICombinationResult<ICombinationResult<TSource>>> NoIntersectionCombination<TSource>(IEnumerable<TSource> tSources, int combinationCount, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;

            List<ICombinationResult<TSource>> combinations = Combination(tSources,(IEqualityComparer<TSource>) null, minElementCount, maxElementCount);

            List<ICombinationResult<ICombinationResult<TSource>>> ps = Combination(combinations, (first, second) =>
            {
                var intersect = first.CombinationCollection.Intersect(second.CombinationCollection);
                return intersect.Count() == 0;
            }, combinationCount, combinationCount);

            ps.RemoveAll(combination =>
            {
                IEnumerable<TSource> tEnumerable = new List<TSource>();
                foreach (var p in combination.CombinationCollection)
                    tEnumerable = tEnumerable.Union(p.CombinationCollection);
                return tEnumerable.Except(tSources).Any();
            });

            return ps;
        }

        /// <summary>
        /// 两个数量一致的序列进行一一匹配
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tFirsts"></param>
        /// <param name="tSeconds"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static List<ICombinationResult<TOut>>
            CollectionMatchCombination<TFirst, TSecond, TOut>(IEnumerable<TFirst> tFirsts, IEnumerable<TSecond> tSeconds,
                Func<TFirst, TSecond, TOut> convertFunc)
        {
            if(tFirsts.Count()!=tSeconds.Count()) throw new Exception("两个序列数量不一致，无法组合");
            var combinationCount = tFirsts.Count();

            var seconds = ArrangementHelper.StaticArrangement(tSeconds.ToArray());
            var firsts = tFirsts.ToArray();
            List<ICombinationResult<TOut>> convertList = new List<ICombinationResult<TOut>>();

            foreach (var second in seconds)
            {
                CombinationResult<TOut> c = new CombinationResult<TOut>();
                for (int i = 0; i < combinationCount; i++)
                {
                    c.CombinationCollection.Add(convertFunc(firsts[i],second[i]));
                }
                convertList.Add(c);
            }

            return convertList;

            //var fullJoin = tFirsts.GroupJoin(tSeconds, first => true, second => true,
            //    (first, secondArgs) => new {First = first, SecondArgs = secondArgs}).SelectMany(
            //    pp => pp.SecondArgs.DefaultIfEmpty(), (first, second) => new {First = first.First, Second = second});

            //var t = Combination(combinationCount, fullJoin);

            //var result = t.FindAll(combination =>
            //{
            //    var list = combination.CombinationCollection;
            //    var firstCount = list.ConvertAll(item => item.First).Distinct().Count();
            //    var secondCount = list.ConvertAll(item => item.Second).Distinct().Count();

            //    return list.Count == firstCount && list.Count == secondCount;
            //});
            //List<ICombinationResult<TOut>> convertList = new List<ICombinationResult<TOut>>();
            //foreach (var combinationResult in result)
            //{
            //    CombinationResult<TOut> c = new CombinationResult<TOut>();
            //    combinationResult.CombinationCollection.ForEach(item=>c.CombinationCollection.Add(convertFunc(item.First,item.Second)));
            //    convertList.Add(c);
            //}

            //return convertList;
        }


        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMatch"></typeparam>
        /// <typeparam name="TSecondSelect"></typeparam>
        /// <typeparam name="TFirstSelect"></typeparam>
        /// <typeparam name="TOffsetQuota"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="combinationCount">组合（方案）数量</param>
        /// <param name="tMatches"></param>
        /// <param name="iCollectionBisector">二等分器</param>
        /// <param name="offsetFunc"></param>
        /// <param name="converter"></param>
        /// <param name="filter"></param>
        /// <param name="resultTopCount"></param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <param name="tFirstFunc"></param>
        /// <param name="tSecondFunc"></param>
        /// <returns></returns>
        public static List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> NoIntersectionCombination<TSource, TMatch, TOffsetQuota, TFirstSelect, TSecondSelect>(IEnumerable<TSource> tSources, 
            int combinationCount,
            IEnumerable<TMatch> tMatches,
            ICollectionBisector<TSource> iCollectionBisector,
            Func<ICombinationResult<TSource>, TFirstSelect> tFirstFunc,
            Func<TMatch,List<TFirstSelect>, TSecondSelect> tSecondFunc,
            Func<TFirstSelect, TSecondSelect, TOffsetQuota> offsetFunc,
            Converter<TOffsetQuota, double> converter,
            Predicate<ICombinationResult<ICombinationResult<TSource>>> filter,
            Func<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>, MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>,bool> comparerFunc,
            int resultTopCount = 10,
            int minElementCount = 1, 
            int maxElementCount = 0,
            int nodeCount=0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;
            nodeCount += 1;
            var bisectorResult= iCollectionBisector.Bisector(tSources);
            List<ICombinationResult<ICombinationResult<TSource>>> combinations = null;

            List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> matchList = new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();

            if (bisectorResult.IsBisect)
            {
                var firstList = bisectorResult.FirstEnumerable;
                var secondList = bisectorResult.SecondEnumerable;
                List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> firstCombinations = new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();

                List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> secondCombinations = new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();

                

                #region MyRegion


                //firstCombinations = NoIntersectionCombination(firstList, combinationCount, tMatches, iCollectionBisector, tFirstFunc, tSecondFunc, offsetFunc, converter,  filter, comparerFunc,resultTopCount,
                //    1, maxElementCount);


                
                //secondCombinations = NoIntersectionCombination(secondList, combinationCount, tMatches,
                //    iCollectionBisector, tFirstFunc, tSecondFunc, offsetFunc, converter, filter, comparerFunc, resultTopCount, 1, maxElementCount);


                #endregion

                #region 异步

                var firstTask = Task.Run(() =>
                {
                    try
                    {
                        firstCombinations = NoIntersectionCombination(firstList, combinationCount, tMatches, iCollectionBisector, tFirstFunc, tSecondFunc, offsetFunc, converter, filter, comparerFunc, resultTopCount,
                            1, maxElementCount, nodeCount);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                });

                var secondTask = Task.Run(() =>
                {
                    try
                    {
                        secondCombinations = NoIntersectionCombination(secondList, combinationCount, tMatches,
                            iCollectionBisector, tFirstFunc, tSecondFunc, offsetFunc, converter, filter, comparerFunc, resultTopCount, 1, maxElementCount, nodeCount);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                });
                try
                {
                    firstTask.Wait();
                    secondTask.Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


                #endregion



                List<ICombinationResult<ICombinationResult<TSource>>> results = new List<ICombinationResult<ICombinationResult<TSource>>>();
                if (firstCombinations.Count == 0 || secondCombinations.Count == 0)
                {
                    return new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();
                }

                

                //全连接
                var fullJoin = firstCombinations.GroupJoin(
                    secondCombinations, 
                    first => true, 
                    second => true,
                    (first, secondArgs) => new {First = first, SecondList = secondArgs})
                    .SelectMany(pp=>pp.SecondList,(tFirst,tSecond)=>new
                {
                    First = tFirst.First,Second=tSecond
                    });

                

                var totallyCount = tSources.Count();
                foreach (var item in fullJoin)
                {
                    var tFirstList = item.First;
                    var tSecondList = item.Second;

                    List<ICombinationResult<ICombinationResult<TSource>>> t1;
                    var firsts = tFirstList.CombinationCollection.ConvertAll(item1 => new CombinationResult<TSource>()
                        {CombinationCollection = item1.First.CombinationCollection});
                    var seconds = tSecondList.CombinationCollection.ConvertAll(item1 => new CombinationResult<TSource>()
                        { CombinationCollection = item1.First.CombinationCollection });
                    t1 = CollectionMatchCombination(firsts, seconds, (first, second) =>
                    {
                        ICombinationResult<TSource> c = new CombinationResult<TSource>
                        {
                            CombinationCollection =
                                first.CombinationCollection.Concat(second.CombinationCollection).ToList()
                        };
                        return c;
                    });
                    var t2 =t1.FindAll(filter);
                    //t2.RemoveAll(item1 =>
                    //    item1.CombinationCollection.Exists(items => items.CombinationCollection.Count == 0 || items.CombinationCollection.Count>maxElementCount ));

                    foreach (var combinationResult in t2)
                    {
                        double variance = 0;
                        
                        MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>,double> matches = CombinationMatchHelper.CombinationMinMatch(combinationResult.CombinationCollection, tMatches, tFirstFunc, tSecondFunc, offsetFunc, converter/*, out variance*/);
                        matchList.Add(matches);
                    }
                    results.AddRange(t2);
                }

                if (nodeCount == 1)
                {
                    
                }

                var matchGroup = matchList.GroupBy(item => item.Degree,
                    new FuncEqualityComparer<double>((v1, v2) => Math.Abs(v1 - v2) < 0.1));
                var vv = matchGroup.Count();
                List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> distinctMatchList = new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();
                foreach (var match in matchGroup)
                {
                    distinctMatchList.AddRange(match.Distinct(new FuncEqualityComparer<MatchDegreeResult<
                            MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc))
                        );
                }
                if (nodeCount <=2)
                {
                    var t = distinctMatchList.GroupBy(item1 => item1, new FuncEqualityComparer<MatchDegreeResult<
                        MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc));

                    distinctMatchList = t.ToList().ConvertAll(item => item.OrderBy(item1 => item1.Degree).ToArray()[0]);

                    //distinctMatchList = distinctMatchList.Distinct(new FuncEqualityComparer<MatchDegreeResult<
                    //    MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc)).ToList();
                }
                

                //matchList =matchList.Distinct(
                //    new FuncEqualityComparer<MatchDegreeResult<
                //        MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc)).ToList();

                if (distinctMatchList.Count == 0) return new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();

                distinctMatchList = distinctMatchList.OrderBy(item =>item.Degree).ToList();
                var top10 = distinctMatchList.GetRange(0, distinctMatchList.Count > resultTopCount ? resultTopCount : distinctMatchList.Count);

                //var l = new List<ICombinationResult<ICombinationResult<TSource>>>();
                //foreach (var tuple in top10)
                //{
                //    ICombinationResult<ICombinationResult<TSource>> v = new CombinationResult<ICombinationResult<TSource>>();
                //    foreach (var matchingDegree in tuple.CombinationCollection)
                //    {
                //        v.CombinationCollection.Add(matchingDegree.First);
                //    }
                //    l.Add(v);
                //}
                return top10;
            }
            else
            {
                var results = NoIntersectionCombination(tSources, combinationCount,
                    new FuncEqualityComparer<TSource>((x, y) => x.Equals(y)));
                foreach (var result in results)
                {
                    while (result.CombinationCollection.Count<combinationCount)
                    {
                        result.CombinationCollection.Add(new CombinationResult<TSource>());
                    }
                }

                var t2 = results.FindAll(filter);
                //t2.RemoveAll(item1 =>
                //    item1.CombinationCollection.Exists(items => items.CombinationCollection.Count == 0 || items.CombinationCollection.Count > maxElementCount));
                //List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>>, double>> matchList = new List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>>, double>>();

                if (t2.Count == 0) return new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(); 
                foreach (var combinationResult in t2)
                {
                    double variance = 0;
                    MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double> matches = CombinationMatchHelper.CombinationMinMatch(combinationResult.CombinationCollection, tMatches, tFirstFunc, tSecondFunc, offsetFunc, converter/*, out variance*/);
                    matchList.Add(matches);
                }

                var matchGroup = matchList.GroupBy(item => item.Degree,
                    new FuncEqualityComparer<double>((v1, v2) => Math.Abs(v1 - v2) < 0.001));
                List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>> distinctMatchList = new List<MatchDegreeResult<MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>();
                foreach (var match in matchGroup)
                {
                    distinctMatchList.AddRange(match.Distinct(new FuncEqualityComparer<MatchDegreeResult<
                            MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc))
                    );
                }

                if (distinctMatchList.Count == 0) return distinctMatchList;
                //matchList = matchList.Distinct(
                //    new FuncEqualityComparer<MatchDegreeResult<
                //        MatchingDegree<ICombinationResult<TSource>, TMatch, TOffsetQuota>, double>>(comparerFunc)).ToList();

                distinctMatchList = distinctMatchList.OrderBy(item => item.Degree).ToList();
                var top10 = distinctMatchList.GetRange(0, distinctMatchList.Count > resultTopCount ? resultTopCount : distinctMatchList.Count);

                //var l = new List<ICombinationResult<ICombinationResult<TSource>>>();
                //foreach (var tuple in top10)
                //{
                //    ICombinationResult<ICombinationResult<TSource>> v = new CombinationResult<ICombinationResult<TSource>>();
                //    foreach (var matchingDegree in tuple.Item1.CombinationCollection)
                //    {
                //        v.CombinationCollection.Add(matchingDegree.First);
                //    }
                //    l.Add(v);
                //}
                return top10;
            }

            //var filterCombinations = combinations.Where(item =>
            //    item.CombinationCollection.Count >= minElementCount &&
            //    item.CombinationCollection.Count <= maxElementCount);

            //List<ICombinationResult<TSource>> combinations = Combination(tSources, (IEqualityComparer<TSource>)null, minElementCount, maxElementCount);

            //List<ICombinationResult<ICombinationResult<TSource>>> ps = Combination(combinations, (first, second) =>
            //{
            //    var intersect = first.CombinationCollection.Intersect(second.CombinationCollection);
            //    return intersect.Count() == 0;
            //}, combinationCount, combinationCount);

            //ps.RemoveAll(combination =>
            //{
            //    IEnumerable<TSource> tEnumerable = new List<TSource>();
            //    foreach (var p in combination.CombinationCollection)
            //        tEnumerable = tEnumerable.Union(p.CombinationCollection);
            //    return tEnumerable.Except(tSources).Any();
            //});

            //return ;
        }


        /// <summary>
        /// 创建没有交集的方案
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tSources"></param>
        /// <param name="combinationCount"></param>
        /// <param name="funcComparer"></param>
        /// <param name="minElementCount"></param>
        /// <param name="maxElementCount"></param>
        /// <returns></returns>
        public static List<ICombinationResult<ICombinationResult<TSource>>> NoIntersectionCombination<TSource>(IEnumerable<TSource> tSources, int combinationCount, Func<TSource, TSource, bool> funcComparer, int minElementCount = 1, int maxElementCount = 0)
        {
            maxElementCount = maxElementCount == 0 ? tSources.Count() : maxElementCount;

            List<ICombinationResult<TSource>> combinations = Combination(tSources, funcComparer, minElementCount, maxElementCount);

            List<ICombinationResult<ICombinationResult<TSource>>> ps = Combination(combinations, (first, second) =>
            {
                var intersect = first.CombinationCollection.Intersect(second.CombinationCollection,new FuncEqualityComparer<TSource>(funcComparer));
                return intersect.Count() == 0;
            }, combinationCount, combinationCount);

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

    public class CombinationResult<T>: ICombinationResult<T>
    {
        public List<T> CombinationCollection { get; set; }=new List<T>();
    }
}
