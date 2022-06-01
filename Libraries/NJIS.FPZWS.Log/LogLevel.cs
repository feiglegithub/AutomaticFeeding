// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Log
//  文 件 名：LogLevel.cs
//  创建时间：2018-08-11 12:38
//  作    者：
//  说    明：
//  修改时间：2018-08-09 10:02
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Log
{
    /// <summary>
    ///     表示日志输出级别的枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///     输出所有级别的日志
        /// </summary>
        All = 0,

        /// <summary>
        ///     表示跟踪的日志级别
        /// </summary>
        Trace = 1,

        /// <summary>
        ///     表示调试的日志级别
        /// </summary>
        Debug = 2,

        /// <summary>
        ///     表示消息的日志级别
        /// </summary>
        Info = 3,

        /// <summary>
        ///     表示警告的日志级别
        /// </summary>
        Warn = 4,

        /// <summary>
        ///     表示错误的日志级别
        /// </summary>
        Error = 5,

        /// <summary>
        ///     表示严重错误的日志级别
        /// </summary>
        Fatal = 6,

        /// <summary>
        ///     关闭所有日志，不输出日志
        /// </summary>
        Off = 7
    }
}