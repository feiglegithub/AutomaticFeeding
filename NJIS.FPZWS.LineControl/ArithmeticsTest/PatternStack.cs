using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmeticsTest
{
    public class PatternStack
    {
        public Stack Stack { get; set; } = new Stack(){CreatedTime = DateTime.Now,UpdatedTime = DateTime.Now};
        public string StackName => Stack.StackName;
        public List<StackDetail> StackDetails { get; set; } = new List<StackDetail>();
        public List<Pattern> Patterns { get; set; } = new List<Pattern>();
        /// <summary>
        /// 移动至上个垛的锯切图
        /// </summary>
        public List<Pattern> MoveLastStackPatterns { get; set; } = new List<Pattern>();
        /// <summary>
        /// 从下一个垛填补上来的锯切图
        /// </summary>
        public List<Pattern> NextStackPatterns { get; set; }= new List<Pattern>();
        public int TotallyTime => Patterns.Sum(item => item.TotallyTime);
        public int BookCount => Patterns.Sum(item => item.BookCount);
        public string BatchName => Stack.FirstBatchName;
        public List<string> Colors => Patterns.GroupBy(item => item.Color).ToList().ConvertAll(item => item.Key)
            .OrderBy(item => item).ToList();
        public int PartCount => Patterns.Sum(item => item.PartCount);
        public int OffPartCount => Patterns.Sum(item => item.OffPartCount);
        /// <summary>
        /// 越小越快
        /// </summary>
        public double Speed => TotallyTime * 1.0 / PartCount;
        public string DisplaySpeed => Speed.ToString("0.00");

        public string FirstColor => Colors.First();
        public int FirstColorPartCount => Patterns.FindAll(item => item.Color == FirstColor).Sum(item => item.PartCount);
        public int FirstBookCount => Patterns.FindAll(item => item.Color == FirstColor).Sum(item => item.BookCount);
        public int FirstTotallyTime => Patterns.FindAll(item => item.Color == FirstColor).Sum(item => item.TotallyTime);
        public double FirstSpeed => FirstTotallyTime * 1.0 / FirstColorPartCount;
        public string DisplayFirstSpeed => FirstSpeed.ToString("0.00");

        public string SecondColor => Colors.Count > 1 ? Colors[1] : "";
        public int SecondColorPartCount => Patterns.FindAll(item => item.Color == SecondColor).Sum(item => item.PartCount);
        public int SecondBookCount => Patterns.FindAll(item => item.Color == SecondColor).Sum(item => item.BookCount);
        public int SecondTotallyTime => Patterns.FindAll(item => item.Color == SecondColor).Sum(item => item.TotallyTime);
        public double SecondSpeed => SecondTotallyTime * 1.0 / SecondColorPartCount;
        public string DisplaySecondSpeed => SecondSpeed.ToString("0.00");

        public string ThirdColor => Colors.Count >= 3 ? Colors[2] : "";
        public int ThirdColorPartCount => Patterns.FindAll(item => item.Color == ThirdColor).Sum(item => item.PartCount);
        public int ThirdBookCount => Patterns.FindAll(item => item.Color == ThirdColor).Sum(item => item.BookCount);
        public int ThirdTotallyTime => Patterns.FindAll(item => item.Color == ThirdColor).Sum(item => item.TotallyTime);
        public double ThirdSpeed => ThirdTotallyTime * 1.0 / ThirdColorPartCount;
        public string DisplayThirdSpeed => ThirdSpeed.ToString("0.00");

        public int BookPileCount => Patterns.FindAll(item => item.BookCount > 1).Sum(item => item.BookCount);
        public double BookRatio => BookPileCount * 1.0 / BookCount;
        public string DisplayBookRatio => BookRatio.ToString("0.00");

    }
}
