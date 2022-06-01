//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：LogNetManagment.cs
//   创建时间：2018-11-08 16:23
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:23
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Text;
using NJIS.PLC.Communication.LogNet.Core;

namespace NJIS.PLC.Communication.LogNet
{
    /*************************************************************************************
     * 
     *    目标：
     *        1. 高性能的日志类
     *        2. 灵活的配置
     *        3. 日志分级
     *        4. 控制输出
     *        5. 方便筛选
     *        6. 方便的配置按小时，天，月，年记录
     * 
     * 
     * 
     *************************************************************************************/


    /// <summary>
    ///     日志类的管理器
    /// </summary>
    public class LogNetManagment
    {
        /// <summary>
        ///     存储文件的时候指示单文件存储
        /// </summary>
        internal const int LogSaveModeBySingleFile = 1;

        /// <summary>
        ///     存储文件的时候指示根据文件大小存储
        /// </summary>
        internal const int LogSaveModeByFileSize = 2;

        /// <summary>
        ///     存储文件的时候指示根据日志时间来存储
        /// </summary>
        internal const int LogSaveModeByDateTime = 3;


        /// <summary>
        ///     日志文件的头标志
        /// </summary>
        internal const string LogFileHeadString = "Logs_";


        /// <summary>
        ///     公开的一个静态变量，允许随意的设置
        /// </summary>
        public static ILogNet LogNet { get; set; }


        internal static string GetDegreeDescription(NjisMessageDegree degree)
        {
            switch (degree)
            {
                case NjisMessageDegree.DEBUG: return StringResources.Language.LogNetDebug;
                case NjisMessageDegree.INFO: return StringResources.Language.LogNetInfo;
                case NjisMessageDegree.WARN: return StringResources.Language.LogNetWarn;
                case NjisMessageDegree.ERROR: return StringResources.Language.LogNetError;
                case NjisMessageDegree.FATAL: return StringResources.Language.LogNetFatal;
                case NjisMessageDegree.None: return StringResources.Language.LogNetAbandon;
                default: return StringResources.Language.LogNetAbandon;
            }
        }


        /// <summary>
        ///     通过异常文本格式化成字符串用于保存或发送
        /// </summary>
        /// <param name="text">文本消息</param>
        /// <param name="ex">异常</param>
        /// <returns>异常最终信息</returns>
        public static string GetSaveStringFromException(string text, Exception ex)
        {
            var builder = new StringBuilder(text);

            if (ex != null)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    builder.Append(" : ");
                }

                try
                {
                    builder.Append(StringResources.Language.ExceptionMessage);
                    builder.Append(ex.Message);
                    builder.Append(Environment.NewLine);
                    builder.Append(StringResources.Language.ExceptionSourse);
                    builder.Append(ex.Source);
                    builder.Append(Environment.NewLine);
                    builder.Append(StringResources.Language.ExceptionStackTrace);
                    builder.Append(ex.StackTrace);
                    builder.Append(Environment.NewLine);
                    builder.Append(StringResources.Language.ExceptionType);
                    builder.Append(ex.GetType());
                    builder.Append(Environment.NewLine);
                    builder.Append(StringResources.Language.ExceptopnTargetSite);
                    builder.Append(ex.TargetSite);
                }
                catch
                {
                }

                builder.Append(Environment.NewLine);
                builder.Append(
                    "=================================================[    Exception    ]================================================");
            }

            return builder.ToString();
        }
    }
}