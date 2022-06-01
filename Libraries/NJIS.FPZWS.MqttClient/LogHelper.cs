using System;
using System.IO;

namespace NJIS.FPZWS.MqttClient
{

    /// <summary>
    /// 日志文件存放文件夹分类枚举
    /// </summary>
    internal enum LogType
    {
        /// <summary>
        /// 其他全部信息
        /// </summary>
        Overall = 0,
        /// <summary>
        /// 普通信息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,
    }

    internal class LogHelper
    {
        /// <summary>
        /// 日志存放路径
        /// </summary>
        public static string LogPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\emqtt\log";
            }
        }

        /// <summary>
        /// 信息类型
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// 普通信息
            /// </summary>
            Info,
            /// <summary>
            /// 错误
            /// </summary>
            Error
        }
        /// <summary>
        /// 普通信息写入日志
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            if (string.IsNullOrEmpty(message) && Setting.TaskCenterSetting.Current.LogLevel <= LogType.Overall.GetHashCode())
                return;;
            WriteLog(LogType.Overall, "", message);
        }


        /// <summary>
        /// 普通信息写入日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public static void Info(string message, LogType logType = LogType.Info)
        {
            if (string.IsNullOrEmpty(message))
                return;
            WriteLog(logType, "", message);
        }
        /// <summary>
        /// 自定义错误信息写入
        /// </summary>
        /// <param name="message">自定义消息</param>
        /// <param name="logType">存储目录类型</param>
        public static void Error(string message, LogType logType = LogType.Error)
        {
            if (string.IsNullOrEmpty(message))
                return;
            WriteLog(logType, "Error ", message);
        }
        /// <summary>
        /// 程序异常信息写入
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="logType">存储目录类型</param>
        public static void Error(Exception e, LogType logType = LogType.Error)
        {
            if (e == null)
                return;
            WriteLog(logType, "Error ", e.Message);
        }
        /// <summary>
        /// 写日志的最终执行动作
        /// </summary>
        /// <param name="logType">文件路径</param>
        /// <param name="prefix">前缀</param>
        /// <param name="message">内容</param>
        public static void WriteLog(LogType logType, string prefix, string message)
        {
            var rpath = Path.Combine(LogPath, logType.ToString());
            var fileName = string.Format("{0}{1}.log", prefix, DateTime.Now.ToString("yyyyMMdd"));

            if (!Directory.Exists(rpath))
                Directory.CreateDirectory(rpath);

            using (var fs = new FileStream(Path.Combine(rpath , fileName), FileMode.Append, FileAccess.Write,
                FileShare.Write, 1024, FileOptions.Asynchronous))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("HH:mm:ss") + " " + message + "\r\n");
                var writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                    (asyncResult) =>
                    {
                        var fStream = (FileStream)asyncResult.AsyncState;
                        fStream.EndWrite(asyncResult);
                    },

                    fs);
                fs.Close();
            }
        }
    }
}
