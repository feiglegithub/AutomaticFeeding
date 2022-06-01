//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：LogNetFileSize.cs
//   创建时间：2018-11-08 16:23
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:23
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.IO;
using NJIS.PLC.Communication.LogNet.Core;

namespace NJIS.PLC.Communication.LogNet.Logs
{
    /// <summary>
    ///     根据文件的大小来存储日志信息
    /// </summary>
    /// <remarks>
    ///     此日志的实例是根据文件的大小储存，例如设置了2M，每隔2M，系统将生成一个新的日志文件。
    /// </remarks>
    public class LogNetFileSize : LogNetBase, ILogNet
    {
        private readonly int m_fileMaxSize = 2 * 1024 * 1024; //2M

        private readonly string m_filePath = string.Empty;
        private int m_CurrentFileSize;


        /// <summary>
        ///     当前正在存储的文件名称
        /// </summary>
        private string m_fileName = string.Empty;

        #region Constructor

        /// <summary>
        ///     实例化一个根据文件大小生成新文件的
        /// </summary>
        /// <param name="filePath">日志文件的保存路径</param>
        /// <param name="fileMaxSize">每个日志文件的最大大小，默认2M</param>
        public LogNetFileSize(string filePath, int fileMaxSize = 2 * 1024 * 1024)
        {
            m_filePath = filePath;
            m_fileMaxSize = fileMaxSize;


            LogSaveMode = LogNetManagment.LogSaveModeByFileSize;

            m_filePath = CheckPathEndWithSprit(m_filePath);
        }

        #endregion


        /// <summary>
        ///     返回所有的日志文件
        /// </summary>
        /// <returns>所有的日志文件信息</returns>
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
        /// <returns>字符串数据</returns>
        protected override string GetFileSaveName()
        {
            //路径没有设置则返回空
            if (string.IsNullOrEmpty(m_filePath)) return string.Empty;

            if (string.IsNullOrEmpty(m_fileName))
            {
                //加载文件名称
                m_fileName = GetLastAccessFileName();
            }

            if (File.Exists(m_fileName))
            {
                var fileInfo = new FileInfo(m_fileName);

                if (fileInfo.Length > m_fileMaxSize)
                {
                    //新生成文件
                    m_fileName = GetDefaultFileName();
                }
            }

            return m_fileName;
        }


        /// <summary>
        ///     获取之前保存的日志文件
        /// </summary>
        /// <returns></returns>
        private string GetLastAccessFileName()
        {
            foreach (var m in GetExistLogFileNames())
            {
                var fileInfo = new FileInfo(m);
                if (fileInfo.Length < m_fileMaxSize)
                {
                    m_CurrentFileSize = (int) fileInfo.Length;
                    return m;
                }
            }

            //返回一个新的默认当前时间的日志名称
            return GetDefaultFileName();
        }


        /// <summary>
        ///     获取一个新的默认的文件名称
        /// </summary>
        /// <returns></returns>
        private string GetDefaultFileName()
        {
            //返回一个新的默认当前时间的日志名称
            return m_filePath + LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
        }
    }
}