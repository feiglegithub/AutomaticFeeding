using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace ArithmeticsTest
{
    /// <summary>
    /// 锯切图分配接口
    /// </summary>
    public interface IPatternDistribute
    {
        //Pattern DistributePattern(List<Pattern> patterns, IStackControl iStackControl,string curBatchName);

        /// <summary>
        /// 分配锯切图 
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="stackName"></param>
        /// <param name="maxBookCount"></param>
        /// <param name="color"></param>
        /// <param name="bookType"></param>
        /// <returns>null 表示没有符合的</returns>
        Pattern DistributePattern( string batchName,string stackName, int maxBookCount,string color, BookType bookType);

        /// <summary>
        /// 是否有指定花色的锯切图
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        bool HasPattern(string batchName, string color);


        /// <summary>
        /// 拆解锯切图
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="maxBookCount"></param>
        /// <param name="color"></param>
        void RequestDisintegratedPattern(string batchName, int maxBookCount, string color);

        List<Pattern> GetPatternsByBatchName(string batchName);

        List<Pattern> GetPatternsByPlanDate(DateTime planDate);

        List<PatternDetail> GetPatternDetailsByBatchName(string batchName);

        /// <summary>
        /// 正在加工的锯切图
        /// </summary>
        List<Pattern> CuttingPatterns { get; }

        /// <summary>
        /// 完成的批次
        /// </summary>
        List<string> FinishedBatchs { get; }
        
        /// <summary>
        /// 最后一个完成的批次
        /// </summary>
        string LastFinishedBatchName { get; }

        /// <summary>
        /// 当前批次
        /// </summary>
        string CurrentBatchName { get; }

        bool SavePatterns(List<Pattern> patterns);

        bool SavePatternDetails(List<PatternDetail> patternDetails);
        /// <summary>
        /// 交换锯切图
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="times"></param>
        void SwapPattern(string batchName, int times = 50);

    }

    public enum BookType
    {

        /// <summary>
        /// 包含前一个批次的板材
        /// </summary>
        ContainFrontBatch,
        /// <summary>
        /// 下一个批次的板材
        /// </summary>
        NextBatch,
        /// <summary>
        /// 包含下一个批次的板材
        /// </summary>
        ContainNextBatch,
        /// <summary>
        /// 当前批次的板材
        /// </summary>
        CurrentBatch,
        /// <summary>
        /// 默认类型
        /// </summary>
        Default,
        /// <summary>
        /// 异常（数量不足）
        /// </summary>
        Error,

    }
}
