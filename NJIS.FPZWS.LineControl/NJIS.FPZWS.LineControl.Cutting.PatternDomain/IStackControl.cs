using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    /// <summary>
    /// 垛控制接口
    /// </summary>
    public interface IStackControl
    {
        /// <summary>
        /// 垛明细
        /// </summary>
        List<StackDetail> StackDetails { get;  }
        /// <summary>
        /// 垛信息
        /// </summary>
        Stack Stack { get;  }
        /// <summary>
        /// 是否需要分配锯切图
        /// </summary>
        /// <returns></returns>
        bool NeedDistributePattern { get; }

        /// <summary>
        /// 当前第一个花色
        /// </summary>
        string CurrentFirstColor { get; }

        /// <summary>
        /// 当前预分配的第一个板材的批次
        /// </summary>
        string CurrentFirstBookBatchName { get; }

        /// <summary>
        /// 当前第一个花色未使用的数量
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        int CurrentFirstColorUnUseCount(string batchName);

        /// <summary>
        /// 第一个批次
        /// </summary>
        string FirstBatchName { get; }

        /// <summary>
        /// 第二批次
        /// </summary>
        string SecondBatchName { get; }

        //BookType GetCurrentBookType(string curBatchName,int bookCount);

        int CurrentColorBookCount { get; }

        int GetMaxBookCountByBookType(string frontBatchName, string batchName,string nextBatchName, BookType bookType);

        bool BookLinkedPattern(Pattern pattern,out List<StackDetail> stackDetails);

        /// <summary>
        /// 是否需要上下一垛
        /// </summary>
        bool NeedLoadNextStack { get; }

    }

     
}
