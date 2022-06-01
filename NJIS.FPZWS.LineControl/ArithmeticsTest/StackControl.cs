using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmeticsTest
{
    /// <summary>
    /// 垛明细
    /// </summary>
    public class StackControl:IStackControl
    {
        public List<StackDetail> StackDetails { get; protected set; }
        public Stack Stack { get;protected set; }
        public StackControl(Stack stack, List<StackDetail> stackDetails)
        {
            StackDetails = stackDetails;
            Stack = stack;
        }

        public bool NeedDistributePattern 
        {
            get
            {
                if (StackDetails.Count(item => item.Status == Convert.ToInt32(BookStatus.UndistributedPattern)) == 0)
                    return false;
                return !StackDetails.Exists(item => item.Status == Convert.ToInt32(BookStatus.DistributedPattern));
                //var tList = StackDetails.GroupBy(item => new {item.ActualBatchName, item.PatternId}, key => key, (key, details) =>
                //    new
                //    {
                //        key.ActualBatchName,
                //        key.PatternId,
                //        MaxStatus = details.Max(item => item.Status)
                //    }).ToList();
                //return tList.Count(item =>
                //    item.MaxStatus == Convert.ToInt32(BookStatus.Cutting) ||
                //    item.MaxStatus == Convert.ToInt32(BookStatus.DistributedPattern)) < 2;
            }
        }

        public string CurrentFirstColor
        {
            get
            {
                if (StackDetails.Count(item => item.Status == Convert.ToInt32(BookStatus.UndistributedPattern)) == 0)
                    return string.Empty;
                return StackDetails.FindAll(item=>item.Status == Convert.ToInt32(BookStatus.UndistributedPattern)).OrderBy(item => item.StackIndex).First().Color;
            }
        }

        public string CurrentFirstBookBatchName
        {
            get
            {
                 if (StackDetails.Count(item => item.Status == Convert.ToInt32(BookStatus.UndistributedPattern)) == 0)
                    return string.Empty;
                return StackDetails.FindAll(item=>item.Status == Convert.ToInt32(BookStatus.UndistributedPattern)).OrderBy(item => item.StackIndex).First().PlanBatchName;
            }
        }

        public int CurrentFirstColorUnUseCount(string batchName)
        {
            return StackDetails.Count(item =>
                item.PlanBatchName == batchName && item.Status == Convert.ToInt32(BookStatus.UndistributedPattern) &&
                item.Color == this.CurrentFirstColor);
        }

        public string FirstBatchName => Stack.FirstBatchName;

        public string SecondBatchName => Stack.SecondBatchName;


        public int CurrentColorBookCount
        {
            get
            {
                return StackDetails.Count(item =>
                    item.Status == Convert.ToInt32(BookStatus.UndistributedPattern) &&
                    item.Color == this.CurrentFirstColor);
            }
        }

        public int GetMaxBookCountByBookType(string frontBatchName, string batchName, string nextBatchName, BookType bookType)
        {
            int maxBookCount = 0;
            switch (bookType)
            {
                case BookType.ContainFrontBatch:
                {
                    if (CurrentFirstBookBatchName == frontBatchName)
                    {
                        var count = CurrentFirstColorUnUseCount(frontBatchName);
                        maxBookCount = count==0?0: count +
                                       CurrentFirstColorUnUseCount(batchName);
                    }
                    break;
                }
                case BookType.ContainNextBatch:
                {
                    if (CurrentFirstBookBatchName == batchName)
                    {
                        var count = CurrentFirstColorUnUseCount(nextBatchName);
                        maxBookCount = count == 0 ? 0 : count +
                                                        CurrentFirstColorUnUseCount(batchName);
                    }
                    break;
                }
                case BookType.CurrentBatch:
                {
                    if (CurrentFirstBookBatchName == batchName)
                    {
                        
                        maxBookCount = CurrentFirstColorUnUseCount(batchName);
                    }
                    break;
                }
                case BookType.Default:
                {
                    if (CurrentFirstBookBatchName == batchName)
                    {

                        maxBookCount = CurrentFirstColorUnUseCount(batchName);
                    }
                    break;
                }
                case BookType.NextBatch:
                {
                    if (CurrentFirstBookBatchName == nextBatchName)
                    {
                        maxBookCount = CurrentFirstColorUnUseCount(nextBatchName);
                    }
                    break;
                }
            }

            return maxBookCount;
        }

        public bool BookLinkedPattern(Pattern pattern,out List<StackDetail> stackDetails)
        {
            stackDetails = new List<StackDetail>();
            if (pattern == null) return false;
            var color = pattern.Color;
            if (CurrentFirstColor != color) return false;
            if (pattern.BookCount > CurrentColorBookCount) return false;//花色不足

            var matchDetails = StackDetails.FindAll(item =>
                item.Status == Convert.ToInt32(BookStatus.UndistributedPattern) &&
                item.Color == this.CurrentFirstColor).OrderBy(item => item.StackIndex).ToList().GetRange(0,pattern.BookCount);

            matchDetails.ForEach(item=>
            {
                item.Status = Convert.ToInt32(BookStatus.DistributedPattern);
                item.PatternId = pattern.PatternId;
                item.UpdatedTime = DateTime.Now;
                item.ActualBatchName = pattern.BatchName;
            });
            pattern.DeviceName = Stack.ActualDeviceName;

            stackDetails = matchDetails;
            return true;
        }

        public bool NeedLoadNextStack
        {
            get
            {
                var unBeginCount = StackDetails.Count(item=>item.Status == Convert.ToInt32(BookStatus.UndistributedPattern));
                return unBeginCount < 8;
            }
        }
    }
}
