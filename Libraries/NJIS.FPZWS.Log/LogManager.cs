// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.Log
//  文 件 名：LogManager.cs
//  创建时间：2017-07-28 14:33
//  作    者：
//  说    明：
//  修改时间：2017-09-04 8:45
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.Log.Implement.Log4Net;

#endregion

namespace NJIS.FPZWS.Log
{
    /// <summary>
    ///     日志管理器
    /// </summary>
    public static class LogManager
    {
        private static readonly ConcurrentDictionary<string, ILogger> Loggers;
        private static readonly object LockObj = new object();

        /// <summary>
        ///     初始化一个<see cref="LogManager" />实例
        /// </summary>
        static LogManager()
        {
            Loggers = new ConcurrentDictionary<string, ILogger>();
            Adapters = new List<ILoggerAdapter>();
        }

        /// <summary>
        ///     获取 日志适配器集合
        /// </summary>
        internal static ICollection<ILoggerAdapter> Adapters { get; }

        /// <summary>
        ///     添加日志适配器
        /// </summary>
        public static void AddLoggerAdapter(ILoggerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.Any(m => m == adapter))
                {
                    return;
                }
                Adapters.Add(adapter);
                Loggers.Clear();
            }
        }

        /// <summary>
        ///     移除日志适配器
        /// </summary>
        public static void RemoveLoggerAdapter(ILoggerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.All(m => m != adapter))
                {
                    return;
                }
                Adapters.Remove(adapter);
                Loggers.Clear();
            }
        }

        /// <summary>
        ///     设置日志记录入口参数
        /// </summary>
        /// <param name="enabled">是否允许记录日志，如为 false，将完全禁止日志记录</param>
        /// <param name="entryLevel">日志级别的入口控制，级别决定是否执行相应级别的日志记录功能</param>
        public static void SetEntryInfo(bool enabled, LogLevel entryLevel)
        {
            InternalLogger.EntryEnabled = enabled;
            InternalLogger.EntryLogLevel = entryLevel;
        }

        /// <summary>
        ///     获取日志记录者实例
        /// </summary>
        public static ILogger GetLogger(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            lock (LockObj)
            {
                ILogger logger;
                if (Loggers.TryGetValue(name, out logger))
                {
                    return logger;
                }
                logger = new InternalLogger(name);
                Loggers[name] = logger;
                return logger;
            }
        }

        /// <summary>
        ///     获取指定类型的日志记录实例
        /// </summary>
        public static ILogger GetLogger(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return GetLogger(type.FullName);
        }

        /// <summary>
        ///     获取指定类型的日志记录实例
        /// </summary>
        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }
    }
}