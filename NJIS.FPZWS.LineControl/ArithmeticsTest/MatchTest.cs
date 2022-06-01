using Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticsTest
{
    public class MatchTest
    {
        public static void SpiltTask(List<AllTask> allTasks, List<DeviceInfos> dis)
        {
            var totalTime = allTasks.Sum(item => item.TotalTime);
            var totalRatio = dis.Sum(item => item.Ratio) * 1.0;
            List<ICombinationResult<ICombinationResult<AllTask>>> permutations = CombinationHelper.NoIntersectionCombination(allTasks, dis.Count,1);
            foreach (var permutation in permutations)
            {
                //按开料时间匹配
                List<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>> cuttingTimeMatchs = CombinationMatchHelper.CombinationMatch(
                    permutation.CombinationCollection, dis,
                    first => first.CombinationCollection.Sum(item1 => item1.TotalTime),
                    second => (second.Ratio / totalRatio) * totalTime,
                    (permutationSumTime, expectedValue) => permutationSumTime - expectedValue);

                //按叠板率匹配
                List<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>> stackRateMatchs = CombinationMatchHelper.CombinationMatch(permutation.CombinationCollection, dis,
                    first => (1.0 * first.CombinationCollection.Sum(item1 => item1.BookNum)) / first.CombinationCollection.Count,
                    second => second.Ratio / totalRatio,
                    (stackRate, expectedValue) => stackRate - expectedValue);

                List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>> cuttingTimeMatchTuples = new List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>>();


                foreach (var match in cuttingTimeMatchs)
                {
                    //方案方差值
                    var cuttingTimeVariance = Variance.GetVarianceResults(match.CombinationCollection, p => p.OffsetQuota);
                    cuttingTimeMatchTuples.Add(new Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>(match, cuttingTimeVariance));
                }

                List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>> stackRateMatchTuples = new List<Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>>();

                foreach (var match in stackRateMatchs)
                {
                    //方案方差值
                    var stackRateVariance = Variance.GetVarianceResults(match.CombinationCollection, p => p.OffsetQuota);
                    stackRateMatchTuples.Add(new Tuple<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>, double>(match, stackRateVariance));
                }

                var results = stackRateMatchTuples.Join(cuttingTimeMatchTuples, first => first.Item1, second => second.Item1,
                    (first, second) =>
                        new
                        {
                            Match = first.Item1,
                            StackRateVariance = first.Item2,
                            CuttingTimeVariance = second.Item2
                        }, new FuncEqualityComparer<ICombinationResult<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>>(
                        (x, y) => !x.CombinationCollection.Except(y.CombinationCollection, new FuncEqualityComparer<MatchingDegree<ICombinationResult<AllTask>, DeviceInfos, double>>(
                            (x1, y1) => !x1.First.CombinationCollection.Except(y1.First.CombinationCollection).Any())).Any()));



            }


        }

    }
}
