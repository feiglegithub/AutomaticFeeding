//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：Types.cs
//   创建时间：2018-11-08 16:23
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:23
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Threading;

namespace NJIS.PLC.Communication.LogNet.Core
{
    #region Log EventArgs

    /// <summary>
    ///     带有日志消息的事件
    /// </summary>
    public class NjisEventArgs : EventArgs
    {
        /// <summary>
        ///     消息信息
        /// </summary>
        public NjisMessageItem NjisMessage { get; set; }
    }

    #endregion

    #region Log Output Format

    /// <summary>
    ///     日志文件输出模式
    /// </summary>
    public enum GenerateMode
    {
        /// <summary>
        ///     按每个小时生成日志文件
        /// </summary>
        ByEveryHour = 1,

        /// <summary>
        ///     按每天生成日志文件
        /// </summary>
        ByEveryDay = 2,

        /// <summary>
        ///     按每个周生成日志文件
        /// </summary>
        ByEveryWeek = 3,

        /// <summary>
        ///     按每个月生成日志文件
        /// </summary>
        ByEveryMonth = 4,

        /// <summary>
        ///     按每季度生成日志文件
        /// </summary>
        ByEverySeason = 5,

        /// <summary>
        ///     按每年生成日志文件
        /// </summary>
        ByEveryYear = 6
    }

    #endregion

    #region Message Degree

    /// <summary>
    ///     记录消息的等级
    /// </summary>
    public enum NjisMessageDegree
    {
        /// <summary>
        ///     一条消息都不记录
        /// </summary>
        None = 1,

        /// <summary>
        ///     记录致命等级及以上日志的消息
        /// </summary>
        FATAL = 2,

        /// <summary>
        ///     记录异常等级及以上日志的消息
        /// </summary>
        ERROR = 3,

        /// <summary>
        ///     记录警告等级及以上日志的消息
        /// </summary>
        WARN = 4,

        /// <summary>
        ///     记录信息等级及以上日志的消息
        /// </summary>
        INFO = 5,

        /// <summary>
        ///     记录调试等级及以上日志的信息
        /// </summary>
        DEBUG = 6
    }

    #endregion

    #region LogMessage

    /// <summary>
    ///     单个日志的记录信息
    /// </summary>
    public class NjisMessageItem
    {
        private static long IdNumber;


        /// <summary>
        ///     默认的无参构造器
        /// </summary>
        public NjisMessageItem()
        {
            Id = Interlocked.Increment(ref IdNumber);
        }

        /// <summary>
        ///     单个记录信息的标识ID，程序重新运行时清空
        /// </summary>
        public long Id { get; }

        /// <summary>
        ///     消息的等级
        /// </summary>
        public NjisMessageDegree Degree { get; set; } = NjisMessageDegree.DEBUG;

        /// <summary>
        ///     线程ID
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        ///     消息文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     消息发生的事件
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     消息的关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        ///     返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串信息</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(KeyWord))
            {
                return
                    $"[{Degree}] {Time.ToString("yyyy-MM-dd HH:mm:ss.fff")} Thread [{ThreadId.ToString("D2")}] {Text}";
            }

            return
                $"[{Degree}] {Time.ToString("yyyy-MM-dd HH:mm:ss.fff")} Thread [{ThreadId.ToString("D2")}] {KeyWord} : {Text}";
        }

        /// <summary>
        ///     返回表示当前对象的字符串，剔除了关键字
        /// </summary>
        /// <returns>字符串信息</returns>
        public string ToStringWithoutKeyword()
        {
            return $"[{Degree}] {Time.ToString("yyyy-MM-dd HH:mm:ss.fff")} Thread [{ThreadId.ToString("D2")}] {Text}";
        }
    }

    #endregion
}