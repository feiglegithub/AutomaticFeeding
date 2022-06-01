using System;
using System.Collections.Generic;
using System.Linq;
using Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.ViewModels;

namespace NJIS.FPZWS.LineControl.Manager.Helpers
{
    public class PatternStackHelper
    {

        public static List<BatchStack> CreatedPatternStacks(List<Pattern> patterns,
            int deviceCount = 5)
        {
            var batchPatterns = patterns.GroupBy(item => item.BatchName, pattern => pattern,
                (batchName, ps) => new BatchPattern() {BatchName = batchName, Patterns = ps.ToList()});
            List < BatchStack > batchStacks = new List<BatchStack>();
            foreach (var batchPattern in batchPatterns)
            {
                var batchStack = CreatedPatternStacks(batchPattern, deviceCount);
                SpeedAdjusted(batchStack);
                TotallyTimeAdjusted(batchStack);
                if (batchStack.PatternStacks.Exists(item => item.BookCount > 40))
                {

                }
                //ColorAdjusted(batchStack);
                batchStacks.Add(batchStack);
                
            }

            return batchStacks;
        }

        /// <summary>
        /// 速度调整
        /// </summary>
        /// <param name="batchStack"></param>
        /// <param name="maxSapCount"></param>
        private static void SpeedAdjusted(BatchStack batchStack,int maxSapCount=1000)
        {
            int swapCount = 0;
            while (swapCount< maxSapCount && batchStack.AvgSpeed * 0.05 < batchStack.SpeedStd)
            {
                swapCount += 1;
                var tPatternsStacks = batchStack.PatternStacks.OrderByDescending(item => item.Speed).ToList();
                var slowStacks = batchStack.PatternStacks.FindAll(item => item.Speed > batchStack.AvgSpeed).OrderByDescending(item=>item.Speed).ToList();

                var fastPatternStack = tPatternsStacks.Last();
                Pattern fastPattern = null;
                
                var fastColorSpeeds = fastPatternStack.Patterns.GroupBy(item => item.Color, p => p,
                    (color, ps) => new
                    {
                        Color = color,
                        Speed = ps.Sum(item => item.TotallyTime) * 1.0 / ps.Sum(item => item.PartCount)
                    }).OrderBy(item => item.Speed);
                
                fastPattern = fastPatternStack.Patterns.FindAll(item => item.Color == fastColorSpeeds.First().Color)
                    .OrderBy(item => item.TotallyTime * 1.0 / item.PartCount).First();

                var t = batchStack.AvgSpeed - fastPattern.TotallyTime * 1.0 / fastPattern.PartCount;

                foreach (var tSlowPatternStack in slowStacks)
                {
                    var slowColorSpeeds = tSlowPatternStack.Patterns.GroupBy(item => item.Color, p => p,
                        (color, ps) => new
                        {
                            Color = color,
                            Speed = ps.Sum(item => item.TotallyTime) * 1.0 / ps.Sum(item => item.PartCount)
                        }).OrderByDescending(item => item.Speed);

                    var tColor = slowColorSpeeds.First().Color;
                    var slowPattern = tSlowPatternStack.Patterns.FindAll(item =>
                            item.Color == tColor &&
                            item.TotallyTime * 1.0 / item.PartCount >= t + batchStack.AvgSpeed)
                        .OrderBy(item => item.TotallyTime * 1.0 / item.PartCount).FirstOrDefault();
                    if (slowPattern != null)
                    {
                        tSlowPatternStack.Patterns.Remove(slowPattern);
                        fastPatternStack.Patterns.Remove(fastPattern);

                        tSlowPatternStack.Patterns.Add(fastPattern);
                        fastPatternStack.Patterns.Add(slowPattern);
                        
                        break;
                    }
                    else
                    {
                        if (tSlowPatternStack.Equals(slowStacks.Last()))
                        {
                            return;
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// 总时间调整
        /// </summary>
        /// <param name="batchStack"></param>
        /// <param name="maxSapCount"></param>
        private static void TotallyTimeAdjusted(BatchStack batchStack, int maxSapCount = 1000)
        {
            int swapCount = 0;
            while (true)
            {
                if(swapCount> maxSapCount) break;
                if (batchStack.TimeStd <= 1.1 * batchStack.MinPatternTime || batchStack.TimeStd< batchStack.AvgTime *0.03)
                {
                    break;
                }

                swapCount += 1;
                var tPatternsStacks = batchStack.PatternStacks.OrderByDescending(item => item.TotallyTime).ToList();
                var maxTimePatternStack = tPatternsStacks.First();
                var minTimePatternStack = tPatternsStacks.Last();

                var matchPatterns = maxTimePatternStack.Patterns.FindAll(item =>
                    Math.Abs(item.TotallyTime * 1.0 / item.PartCount - maxTimePatternStack.Speed) <
                    batchStack.TimeStd * 1.1);

                if (matchPatterns.Count == 0)
                {
                    matchPatterns = maxTimePatternStack.Patterns.FindAll(item =>
                        Math.Abs(item.TotallyTime * 1.0 / item.PartCount - maxTimePatternStack.Speed) <
                        batchStack.TimeStd * 1.3);
                }

                if (matchPatterns.Count == 0)
                {
                    matchPatterns = maxTimePatternStack.Patterns.FindAll(item =>
                        Math.Abs(item.TotallyTime * 1.0 / item.PartCount - maxTimePatternStack.Speed) <
                        batchStack.TimeStd * 1.5);
                }
                if (matchPatterns.Count == 0) return;

                var maxPattern = matchPatterns.FindAll(item => minTimePatternStack.Colors.Contains(item.Color)).OrderByDescending(item=>item.TotallyTime).FirstOrDefault();

                if (maxPattern == null)
                {
                    maxPattern = matchPatterns.OrderByDescending(item => item.TotallyTime).First();
                }

                maxTimePatternStack.Patterns.Remove(maxPattern);
                minTimePatternStack.Patterns.Add(maxPattern);
            }
        }

        /// <summary>
        /// 花色调整
        /// </summary>
        /// <param name="batchStack"></param>
        private static void ColorAdjusted(BatchStack batchStack)
        {
            if (batchStack.BatchName == "HB_RX200108A1-3(1)")
            {

            }
            int minCount = 4;
            int patternMinCount = 2;
            var patternStacks = batchStack.PatternStacks.FindAll(item =>
            {
                var groups = item.Patterns.GroupBy(i => i.Color, p => p,
                    (color, ps) => new {Color = color, BookCount = ps.Sum(it => it.BookCount)}).ToList();
                return groups.Count == 2 && groups.Exists(t => t.BookCount <= minCount);
            });

            var otherStacks = batchStack.PatternStacks.FindAll(item=>!patternStacks.Contains(item));

            Dictionary<string,List<PatternStack>> combineStacks = new Dictionary<string, List<PatternStack>>();



            foreach (var patternStack in patternStacks)
            {
                var colorBookCounts = patternStack.Patterns.GroupBy(item => item.Color, p => p,
                    (tColor, ps) => new {Color = tColor, BookCount = ps.Sum(i => i.BookCount)});

                var colorBookCount = colorBookCounts.FirstOrDefault(item => item.BookCount <= minCount);

                if(colorBookCount==null) continue;

                var patterns = patternStack.Patterns.FindAll(item => item.Color == colorBookCount.Color);
                if(patterns.Count > patternMinCount) continue;

                var bookCount = batchStack.PatternStacks.Sum(item =>
                {
                    var tColor = colorBookCount.Color;
                    return (item.FirstColor == tColor ? item.FirstBookCount : 0) +
                           (item.SecondColor == tColor ? item.SecondBookCount : 0) +
                           (item.FirstColor == tColor ? item.ThirdBookCount : 0);
                });

                if (colorBookCount.BookCount == bookCount) continue;

                var tOtherStacks = otherStacks.FindAll(item => item.Colors.Contains(colorBookCount.Color)).OrderBy(item=>item.TotallyTime);

                if (tOtherStacks.Count() == 0)
                {
                    if (combineStacks.ContainsKey(colorBookCount.Color))
                    {
                        combineStacks[colorBookCount.Color].Add(patternStack);
                    }
                    else
                    {
                        combineStacks.Add(colorBookCount.Color,new List<PatternStack>(){ patternStack });
                    }
                    continue;
                }

                var enumerator = tOtherStacks.GetEnumerator();
                foreach (var pattern in patterns)
                {
                    if (!enumerator.MoveNext())
                    {
                        enumerator = tOtherStacks.GetEnumerator();
                        enumerator.MoveNext();
                    }
                    var otherStack = enumerator.Current;
                    var ttPatterns = otherStack.Patterns.FindAll(item => item.Color != pattern.Color);
                    var tPatterns = ttPatterns.ConvertAll(item =>
                        new
                        {
                            Pattern = item,
                            AbsTime = Math.Abs(item.TotallyTime - pattern.TotallyTime),
                            AbsSpeed = Math.Abs(item.TotallyTime * 1.0 / item.PartCount -
                                                pattern.TotallyTime * 1.0 / pattern.PartCount)
                        });

                    if (tPatterns.Exists(item => item.AbsTime <= batchStack.TimeStd))
                    {
                        tPatterns = tPatterns.FindAll(item => item.AbsTime <= batchStack.TimeStd)
                            .OrderBy(item => item.AbsSpeed).ToList();
                    }
                    else
                    {
                        tPatterns = tPatterns.FindAll(item => item.AbsTime <= batchStack.TimeStd)
                            .OrderBy(item => item.AbsTime).ThenBy(item=>item.AbsSpeed).ToList();
                    }

                    var tPattern = tPatterns.First().Pattern;

                    patternStack.Patterns.Add(tPattern);
                    patternStack.Patterns.Remove(pattern);

                    otherStack.Patterns.Add(pattern);
                    otherStack.Patterns.Remove(tPattern);

                    

                }

            }


            //foreach (var combineStack in combineStacks)
            //{
            //    var color = combineStack.Key;
            //    var stacks = combineStack.Value;

            //    var convertStacks = stacks.ConvertAll(item =>
            //    {
            //        var otherColor = item.Patterns.First(p => p.Color != color).Color;
            //        var combineColorPatterns = item.Patterns.FindAll(i => i.Color == color);
            //        var otherColorPatterns = item.Patterns.FindAll(i => i.Color == otherColor);
            //        return new
            //        {
            //            OtherColor = otherColor,
            //            OtherColorBookCount = item.Patterns.FindAll(i => i.Color == otherColor).Sum(i => i.BookCount),
            //            OtherColorPatterns = otherColorPatterns,
            //            CombineColorBookCount = combineColorPatterns.Sum(i => i.BookCount),
            //            CombineColorPatterns = combineColorPatterns,
            //            PatternStack = item
            //        };
            //    });

            //    foreach (var tConvertStack in convertStacks.GroupBy(item=>item.OtherColor))
            //    {
            //        var list = tConvertStack.ToList();
            //        if(list.Count==1) continue;

            //    }

            //}


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchPattern">批次的锯切图</param>
        /// <param name="deviceCount">设备数量</param>
        /// <returns></returns>
        private static BatchStack CreatedPatternStacks(BatchPattern batchPattern,
            int deviceCount = 5)
        {
            BatchStack batchStack = new BatchStack(){BatchName = batchPattern.BatchName};
            var totallyTime = batchPattern.Patterns.Sum(item => item.TotallyTime);
            var minTime = batchPattern.Patterns.Min(item => item.TotallyTime);
            var colorPatterns = batchPattern.Patterns.GroupBy(item => item.Color, pattern => pattern,
                (color, ps) => new ColorPattern { Color = color,BatchName = batchPattern.BatchName, Patterns = ps.ToList() }).ToList();
            var avgTime = totallyTime*1.0 / deviceCount;

            //batchStack.MainColors = colorPatterns.FindAll(item => item.Patterns.Sum(i => i.TotallyTime) >= avgTime)
            //    .ConvertAll(item => item.Color);

            List < TmpPatternStack > tmpPatternStacks = new List<TmpPatternStack>();
            foreach (var colorPattern in colorPatterns)
            {
                var tmpList = CreatedPatternStacks(colorPattern, avgTime);
                tmpPatternStacks.AddRange(tmpList);
            }

            CombineStack(tmpPatternStacks, deviceCount);

            batchStack.PatternStacks = tmpPatternStacks.ConvertAll(item => item.PatternStack);
            return batchStack;
        }

        /// <summary>
        /// 合并成指定的垛数
        /// </summary>
        /// <param name="patternStacks"></param>
        /// <param name="count"></param>
        private static void CombineStack(List<TmpPatternStack> patternStacks, int count = 5)
        {
            var totallyTime = patternStacks.Sum(item => item.PatternStack.TotallyTime);

            if (patternStacks.Count==count) return;

            var needCombines = patternStacks.FindAll(item => item.NeedCombine).OrderByDescending(item=>item.PatternStack.TotallyTime).ToList();
            
            var finishedCount = patternStacks.Count - needCombines.Count;

            if (count - finishedCount == 1)
            {
                var tAvgTime = totallyTime * 1.0 / count;
                var tmpPatternStack = needCombines.First();
                for (int i = 1; i < needCombines.Count; i++)
                {
                    var item = needCombines[i];
                    if (tmpPatternStack.PatternStack.TotallyTime >= tAvgTime) break;
                    do
                    {
                        var tPatterns = item.PatternStack.Patterns.OrderByDescending(it => it.TotallyTime).ToList();
                        if (tPatterns.Count == 1)
                        {
                            var pattern = tPatterns.First();

                            tmpPatternStack.PatternStack.Patterns.Add(pattern);
                            item.PatternStack.Patterns.Remove(pattern);

                        }
                        else
                        {
                            var maxPattern = tPatterns.First();
                            var minPattern = tPatterns.Last();
                            if (tmpPatternStack.PatternStack.TotallyTime + maxPattern.TotallyTime < tAvgTime)
                            {
                                tmpPatternStack.PatternStack.Patterns.Add(maxPattern);
                                item.PatternStack.Patterns.Remove(maxPattern);
                            }

                            tmpPatternStack.PatternStack.Patterns.Add(minPattern);
                            item.PatternStack.Patterns.Remove(minPattern);

                        }

                        if (tmpPatternStack.PatternStack.TotallyTime >= tAvgTime) break;


                    } while (item.PatternStack.Patterns.Count > 0);
                    if (item.PatternStack.Patterns.Count == 0)
                    {
                        patternStacks.Remove(item);
                    }
                }

                tmpPatternStack.NeedCombine = false;
            }


            needCombines = patternStacks.FindAll(item => item.NeedCombine).OrderByDescending(item => item.PatternStack.TotallyTime).ToList();

            finishedCount = patternStacks.Count - needCombines.Count;

            var avgTime = needCombines.Sum(item => item.PatternStack.Patterns.Sum(i => i.TotallyTime)) / 1.0 *
                          (count - finishedCount);

            if (count == finishedCount)
            {
                var stacks = patternStacks.FindAll(item => !item.NeedCombine);
                foreach (var combine in needCombines)
                {
                    var color = combine.PatternStack.FirstColor;
                    var tt = stacks.FindAll(item => item.PatternStack.Colors.Contains(color));
                    if (tt.Count == 0)
                    {
                        tt = stacks;
                    }
                    for (int i = 0; i < combine.PatternStack.Patterns.Count; )
                    {
                        var pattern = combine.PatternStack.Patterns[i];
                        var tmpPattern = tt.OrderBy(item => item.PatternStack.TotallyTime).First();
                        tmpPattern.PatternStack.Patterns.Add(pattern);
                        combine.PatternStack.Patterns.RemoveAt(i);
                    }

                    patternStacks.Remove(combine);
                }
            }



            var t = count - finishedCount;
            var baseStacks = needCombines.GetRange(0, t);
            var otherStacks = needCombines.GetRange(t, needCombines.Count - t);

            foreach (var stack in otherStacks)
            {
                var patternStack = stack.PatternStack;
                for (int i = 0; i < patternStack.Patterns.Count; )
                {
                    var pattern = patternStack.Patterns[i];
                    var baseStack = baseStacks.FirstOrDefault(item =>
                        item.PatternStack.TotallyTime + pattern.TotallyTime < avgTime);
                    if (baseStack == null)
                    {
                        i++;
                        continue;
                    }
                    baseStack.PatternStack.Patterns.Add(pattern);
                    patternStack.Patterns.RemoveAt(i);
                }
            }

            otherStacks.RemoveAll(item => item.PatternStack.Patterns.Count == 0);
            foreach (var stack in otherStacks)
            {
                var patternStack = stack.PatternStack;
                for (int i = 0; i < patternStack.Patterns.Count;)
                {
                    var pattern = patternStack.Patterns[i];
                    var tt = baseStacks.OrderBy(item => item.PatternStack.TotallyTime);
                    var baseStack = tt.FirstOrDefault(item =>
                                        item.PatternStack.Colors.Contains(pattern.Color)) ??
                                    tt.First();

                    baseStack.PatternStack.Patterns.Add(pattern);
                    patternStack.Patterns.RemoveAt(i);
                }
            }

            patternStacks.RemoveAll(item => item.PatternStack.Patterns.Count == 0);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorPattern">花色锯切图</param>
        /// <param name="avgTime">平均时间</param>
        /// <returns></returns>
        private static List<TmpPatternStack> CreatedPatternStacks(ColorPattern colorPattern, double avgTime)
        {
            List<TmpPatternStack> patternStacks = new List<TmpPatternStack>();
            var batchName = colorPattern.BatchName;
            
            int minTime = colorPattern.Patterns.Min(item => item.TotallyTime);
            do
            {
                TmpPatternStack tmpPatternStack = new TmpPatternStack();
                
                PatternStack patternStack = new PatternStack() { Stack = new Stack() { FirstBatchName = batchName, } };
                tmpPatternStack.PatternStack = patternStack;
                if (colorPattern.Patterns.Sum(item => item.TotallyTime) <= avgTime+ minTime)
                {
                    patternStack.Patterns.AddRange(colorPattern.Patterns);
                    tmpPatternStack.NeedCombine = true;


                }
                else
                {
                    var tList = colorPattern.Patterns.OrderByDescending(item => item.TotallyTime)
                        .ThenByDescending(item => item.PartCount).ThenByDescending(item => item.BookCount).ToList();
                    int i = 0;
                    var itemCount = tList.Count;
                    while (true)
                    {
                        var topItem = tList[i];
                        var lastItem = tList[itemCount - 1 - i];

                        if (patternStack.Patterns.Sum(item => item.TotallyTime)+topItem.TotallyTime < avgTime)
                        {
                            patternStack.Patterns.Add(topItem);
                            if (patternStack.Patterns.Sum(item => item.TotallyTime) + lastItem.TotallyTime < avgTime)
                            {
                                patternStack.Patterns.Add(lastItem);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else 
                        {
                            if (patternStack.Patterns.Sum(item => item.TotallyTime) + lastItem.TotallyTime < avgTime)
                            {
                                patternStack.Patterns.Add(lastItem);
                            }
                            break;
                        }
                        i++;
                    }
                }

                colorPattern.Patterns.RemoveAll(patternStack.Patterns.Contains);

                patternStacks.Add(tmpPatternStack);

            } while (colorPattern.Patterns.Count > 0);

            return patternStacks;
        }
    }

    public class BatchStack
    {
        public string BatchName { get; set; }
        public List<PatternStack> PatternStacks { get; set; }
        public List<string> MainColors { get; set; } = new List<string>();

        public int MaxTotallyTime => PatternStacks.Max(item => item.TotallyTime);
        public int MinTotallyTime => PatternStacks.Min(item => item.TotallyTime);
        public int WaitingTime => MaxTotallyTime - MinTotallyTime;
        public double AvgSpeed => PatternStacks.Average(item => item.Speed);
        public string DisplayAvgSpeed => AvgSpeed.ToString("0.00");
        public double SpeedVariance => Variance.VarianceBase(PatternStacks.ConvertAll(item => item.Speed));
        public double SpeedStd => Math.Pow(SpeedVariance, 0.5);
        public string DisplaySpeedStd => SpeedStd.ToString("0.00");
        public double SpeedDeflectionDegree => SpeedStd / AvgSpeed;
        public string DisplaySpeedDeflectionDegree => SpeedDeflectionDegree.ToString("P");

        public double AvgTime => PatternStacks.Average(item => item.TotallyTime);
        public string DisplayAvgTime => AvgTime.ToString("0.00");
        public double TimeVariance => Variance.VarianceBase(PatternStacks.ConvertAll(item => item.TotallyTime));
        public double TimeStd => Math.Pow(TimeVariance, 0.5);
        public double TimeDeflectionDegree => TimeStd / AvgTime;
        public string DisplayTimeDeflectionDegree => TimeDeflectionDegree.ToString("P");
        public string DisplayTimeStd => TimeStd.ToString("0.00");
        public double MinPatternTime => PatternStacks.Min(item => item.Patterns.Min(i => i.TotallyTime));

    }

    internal class TmpPatternStack
    {
        public bool NeedCombine { get; set; } = false;
        public PatternStack PatternStack { get; set; }
    }

    internal class BatchPattern
    {
        public string BatchName { get; set; }
        public List<Pattern> Patterns { get; set; }
    }

    internal class ColorPattern
    {
        public string Color { get; set; }
        public string BatchName { get; set; }
        public List<Pattern> Patterns { get; set; }
    }
}
