using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Arithmetics
{
    /// <summary>
    /// 建垛算法
    /// </summary>
    public class CreatedStackArithmetic
    {
        /// <summary>
        /// 转换为板材信息
        /// </summary>
        /// <param name="cuttingPatterns">锯切图</param>
        /// <param name="cuttingTaskDetails">任务详细</param>
        /// <returns></returns>
        public List<PatternInfo> ConvertPatternInfos(List<CuttingPattern> cuttingPatterns, List<CuttingTaskDetail> cuttingTaskDetails)
        {
            var colorCount = from cuttingPattern in cuttingPatterns
                join cuttingTaskDetail in (from taskDetail in cuttingTaskDetails
                        group taskDetail by new {taskDetail.BatchName,taskDetail.Color, taskDetail.OldPTN_INDEX}
                        into g
                        select new {g.Key.Color, g.Key.OldPTN_INDEX,g.Key.BatchName})
                    on new{ PatternName=Convert.ToInt32(cuttingPattern.PatternName) , cuttingPattern.BatchName} equals new { PatternName=cuttingTaskDetail.OldPTN_INDEX , cuttingTaskDetail.BatchName}
                select new
                {
                    cuttingPattern.PatternName,
                    cuttingTaskDetail.Color,
                    cuttingPattern.BookCount,
                    cuttingPattern.PartCount,
                    cuttingPattern.CycleTime,
                    cuttingPattern.BatchName
                    
                };
                     //group cuttingPattern by new {cuttingTaskDetail.Color, cuttingPattern.BookCount}
                     //into tt
                     //select new {tt.Key.Color, ColorCount = tt.Sum(item => item.BookCount)};
                     //var books = colorCount.OrderByDescending(item => item.ColorCount);

            List <PatternInfo> patternInfos = new List<PatternInfo>();
            foreach (var color in colorCount)
            {
                patternInfos.Add(new PatternInfo()
                {
                    Pattern = color.PatternName,
                    Color = color.Color,
                    BookCount = color.BookCount,
                    PartCount = color.PartCount,
                    CycleTime = color.CycleTime
                });
            }

            return patternInfos;
        }


        public List<BatchIndexInfo> CreatedBatchIndex(List<PatternInfo> patternInfos)
        {
            //var t = from pattern in patternInfos
            //    group pattern by new { pattern.Color }
            //    into g
            //    select new { g.Key.Color, BookCount = g.Sum(item => item.BookCount) };
            //t = t.OrderByDescending(item => item.BookCount);
            var t = patternInfos.GroupBy(item => item.Color,
                (color, iGroup) => new {Color = color, BookCount = iGroup.Sum(item => item.BookCount)}).OrderByDescending(item=>item.BookCount);
            var t2 = patternInfos.GroupBy(item => new {item.BatchName, item.Color},
                (batchAndColor, iGroup) => new
                    {batchAndColor.BatchName, batchAndColor.Color, BookCount = iGroup.Sum(item => item.BookCount)});
            //var t2 = from pattern in patternInfos
            //    group pattern by new { pattern.BatchName,pattern.Color }
            //    into g
            //    select new { g.Key.BatchName,g.Key.Color, BookCount = g.Sum(item => item.BookCount) };
            //var t4 = from tt in t2
            //    group tt by new { tt.BatchName }
            //    into g
            //    select new { g.Key.BatchName, MaxCount = g.Max(item => item.BookCount) };
            var t4 = t2.GroupBy(item => item.BatchName,
                (batchName, iGroup) => new {BatchName = batchName, MaxCount = iGroup.Max(item => item.BookCount)});
            var t3=t2.Join(t4, a => new {a.BatchName, MaxCount = a.BookCount}, b => new {b.BatchName, b.MaxCount}, (a, b) =>new {a.BatchName, a.Color, a.BookCount});
            //var t3 = from ttt in t2
            //    join tttt in
            //        (from tt in t2
            //            group tt by new { tt.BatchName }
            //            into g
            //            select new { g.Key.BatchName, MaxCount = g.Max(item => item.BookCount) })
            //        on new {ttt.BatchName, MaxCount = ttt.BookCount} equals new {tttt.BatchName, tttt.MaxCount}

            List<BatchIndexInfo> batchList = new List<BatchIndexInfo>();
            string firstColor = t.ToList()[0].Color;
            var matchedBatchList = t3.Where(item => item.Color == firstColor).OrderByDescending(item => item.BookCount)
                .Select(item => new BatchIndexInfo(){BatchName = item.BatchName,MainColor = firstColor});
            batchList.AddRange(matchedBatchList);
            var tmpList = matchedBatchList.ToList();
            patternInfos.RemoveAll(item => tmpList.Exists(item1=>item1.BatchName==item.BatchName));
            if (patternInfos.Count > 0)
            {
                batchList.AddRange(CreatedBatchIndex(patternInfos));
            }

            return batchList;
        }

        //public List<StackInfo> ConvertStackInfos(List<PatternInfo> patternInfos, int deviceCount = 5)
        //{
            


        //}
    }

    public class BatchIndexInfo
    {
        public string BatchName { get; set; }
        public string MainColor { get; set; }
        public int Index { get; set; }
    }

    public class StackInfo
    {
        public List<BookInfo> BookInfos { get; set; }

        public int ColorCount => BookInfos.GroupBy(item => item.Color).Count();

        public bool IsFinished => !BookInfos.Exists(item => !item.Status);

        public int BookCount => BookInfos.Count;

        public int FinishedCount => BookInfos.FindAll(item => item.Status).Count;

        public int UnFinishedCount => BookCount - FinishedCount;
    }

    public class BookInfo
    {
        /// <summary>
        /// 花色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 所在垛的顺序
        /// </summary>
        public int Index { get; set; } = 0;
        public bool Status { get; set; } = false;
    }

    public class PatternInfo
    {
        /// <summary>
        /// 锯切图
        /// </summary>
        public int Pattern { get; set; }
        /// <summary>
        /// 花色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 周期时间
        /// </summary>
        public int CycleTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = false;
        /// <summary>
        /// 板材数
        /// </summary>
        public int BookCount { get; set; }
        /// <summary>
        /// 工件数
        /// </summary>
        public int PartCount { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string BatchName { get; set; }

    }

}
