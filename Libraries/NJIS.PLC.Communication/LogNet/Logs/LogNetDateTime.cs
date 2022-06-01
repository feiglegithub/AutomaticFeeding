//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：LogNetDateTime.cs
//   创建时间：2018-11-08 16:23
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:23
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Globalization;
using System.IO;
using NJIS.PLC.Communication.LogNet.Core;

namespace NJIS.PLC.Communication.LogNet.Logs
{
    /// <summary>
    ///     一个日志组件，可以根据时间来区分不同的文件存储
    /// </summary>
    /// <remarks>
    ///     此日志实例将根据日期时间来进行分类，支持的时间分类如下：
    ///     <list type="number">
    ///         <item>小时</item>
    ///         <item>天</item>
    ///         <item>周</item>
    ///         <item>月份</item>
    ///         <item>季度</item>
    ///         <item>年份</item>
    ///     </list>
    /// </remarks>
    public class LogNetDateTime : LogNetBase, ILogNet
    {
        /// <summary>
        ///     文件的路径
        /// </summary>
        private readonly string m_filePath = string.Empty;

        /// <summary>
        ///     文件的存储模式，默认按照年份来存储
        /// </summary>
        private readonly GenerateMode m_generateMode = GenerateMode.ByEveryYear;

        /// <summary>
        ///     当前正在存储的文件名称
        /// </summary>
        private string m_fileName = string.Empty;

        #region 构造方法

        /// <summary>
        ///     实例化一个根据时间存储的日志组件
        /// </summary>
        /// <param name="filePath">文件存储的路径</param>
        /// <param name="generateMode">存储文件的间隔</param>
        public LogNetDateTime(string filePath, GenerateMode generateMode = GenerateMode.ByEveryYear)
        {
            m_filePath = filePath;
            m_generateMode = generateMode;

            LogSaveMode = LogNetManagment.LogSaveModeByDateTime;

            m_filePath = CheckPathEndWithSprit(m_filePath);
        }

        #endregion


        /// <summary>
        ///     获取所有的文件夹中的日志文件
        /// </summary>
        /// <returns>所有的文件路径集合</returns>
        public string[] GetExistLogFileNames()
        {
            if (!string.IsNullOrEmpty(m_filePath))
            {
                return Directory.GetFiles(m_filePath, LogNetManagment.LogFileHeadString + "*.txt");
            }

            return new string[] { };
        }


        /// <summary>
        ///     获取需要保存的日志文件
        /// </summary>
        /// <returns>完整的文件路径，含文件名</returns>
        protected override string GetFileSaveName()
        {
            if (string.IsNullOrEmpty(m_filePath)) return string.Empty;

            switch (m_generateMode)
            {
                case GenerateMode.ByEveryHour:
                {
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMdd_HH") +
                           ".txt";
                }

                case GenerateMode.ByEveryDay:
                {
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }

                case GenerateMode.ByEveryWeek:
                {
                    var gc = new GregorianCalendar();
                    var weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.Year + "_W" + weekOfYear +
                           ".txt";
                }

                case GenerateMode.ByEveryMonth:
                {
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyy_MM") + ".txt";
                }

                case GenerateMode.ByEverySeason:
                {
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.Year + "_Q" +
                           (DateTime.Now.Month / 3 + 1) + ".txt";
                }

                case GenerateMode.ByEveryYear:
                {
                    return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.Year + ".txt";
                }

                default: return string.Empty;
            }
        }
    }
}