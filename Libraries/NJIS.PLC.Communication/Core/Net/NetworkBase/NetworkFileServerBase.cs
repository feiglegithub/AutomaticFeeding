//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：NetworkFileServerBase.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.IO;
using System.Net.Sockets;
using NJIS.PLC.Communication.BasicFramework;
using NJIS.PLC.Communication.Core.Types;

namespace NJIS.PLC.Communication.Core.Net.NetworkBase
{
    /// <summary>
    ///     文件服务器类的基类，为直接映射文件模式和间接映射文件模式提供基础的方法支持
    /// </summary>
    public class NetworkFileServerBase : NetworkServerBase
    {
        #region Override Method

        /// <summary>
        ///     服务器启动时的操作
        /// </summary>
        protected override void StartInitialization()
        {
            if (string.IsNullOrEmpty(FilesDirectoryPath))
            {
                throw new ArgumentNullException("FilesDirectoryPath", "No saved path is specified");
            }

            CheckFolderAndCreate();
            base.StartInitialization();
        }

        #endregion

        #region Protect Method

        /// <summary>
        ///     检查文件夹是否存在，不存在就创建
        /// </summary>
        protected virtual void CheckFolderAndCreate()
        {
            if (!Directory.Exists(FilesDirectoryPath))
            {
                Directory.CreateDirectory(FilesDirectoryPath);
            }
        }

        #endregion

        #region Object Override

        /// <summary>
        ///     获取本对象的字符串标识形式
        /// </summary>
        /// <returns>对象信息</returns>
        public override string ToString()
        {
            return "NetworkFileServerBase";
        }

        #endregion

        #region Receie File Head

        /// <summary>
        ///     接收本次操作的信息头数据
        /// </summary>
        /// <param name="socket">网络套接字</param>
        /// <param name="command">命令</param>
        /// <param name="fileName">文件名</param>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <returns>是否成功的结果对象</returns>
        protected OperateResult ReceiveInformationHead(
            Socket socket,
            out int command,
            out string fileName,
            out string factory,
            out string group,
            out string id
        )
        {
            // 先接收文件名
            var fileNameResult = ReceiveStringContentFromSocket(socket);
            if (!fileNameResult.IsSuccess)
            {
                command = 0;
                fileName = null;
                factory = null;
                group = null;
                id = null;
                return fileNameResult;
            }

            command = fileNameResult.Content1;
            fileName = fileNameResult.Content2;

            // 接收Factory
            var factoryResult = ReceiveStringContentFromSocket(socket);
            if (!factoryResult.IsSuccess)
            {
                factory = null;
                group = null;
                id = null;
                return factoryResult;
            }

            factory = factoryResult.Content2;


            // 接收Group
            var groupResult = ReceiveStringContentFromSocket(socket);
            if (!groupResult.IsSuccess)
            {
                group = null;
                id = null;
                return groupResult;
            }

            group = groupResult.Content2;

            // 最后接收id
            var idResult = ReceiveStringContentFromSocket(socket);
            if (!idResult.IsSuccess)
            {
                id = null;
                return idResult;
            }

            id = idResult.Content2;

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        ///     获取一个随机的文件名，由GUID码和随机数字组成
        /// </summary>
        /// <returns>文件名</returns>
        protected string CreateRandomFileName()
        {
            return SoftBasic.GetUniqueStringByGuidAndRandom();
        }

        /// <summary>
        ///     返回服务器的绝对路径
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <returns>是否成功的结果对象</returns>
        protected string ReturnAbsoluteFilePath(string factory, string group, string id)
        {
            var result = m_FilesDirectoryPath;
            if (!string.IsNullOrEmpty(factory)) result += "\\" + factory;
            if (!string.IsNullOrEmpty(group)) result += "\\" + group;
            if (!string.IsNullOrEmpty(id)) result += "\\" + id;
            return result;
        }


        /// <summary>
        ///     返回服务器的绝对路径
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <param name="fileName">文件名</param>
        /// <returns>是否成功的结果对象</returns>
        protected string ReturnAbsoluteFileName(string factory, string group, string id, string fileName)
        {
            return ReturnAbsoluteFilePath(factory, group, id) + "\\" + fileName;
        }


        /// <summary>
        ///     返回相对路径的名称
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <param name="fileName">文件名</param>
        /// <returns>是否成功的结果对象</returns>
        protected string ReturnRelativeFileName(string factory, string group, string id, string fileName)
        {
            var result = "";
            if (!string.IsNullOrEmpty(factory)) result += factory + "\\";
            if (!string.IsNullOrEmpty(group)) result += group + "\\";
            if (!string.IsNullOrEmpty(id)) result += id + "\\";
            return result + fileName;
        }

        #endregion

        #region Clear File Mark

        //private Timer timer;

        //private void ClearDict(object obj)
        //{
        //    hybirdLock.Enter();

        //    List<string> waitRemove = new List<string>();
        //    foreach(var m in m_dictionary_files_marks)
        //    {
        //        if(m.Value.CanClear())
        //        {
        //            waitRemove.Add(m.Key);
        //        }
        //    }

        //    foreach(var m in waitRemove)
        //    {
        //        m_dictionary_files_marks.Remove(m);
        //    }

        //    waitRemove.Clear();
        //    waitRemove = null;

        //    hybirdLock.Leave();
        //}

        #endregion

        #region 临时文件复制块

        /// <summary>
        ///     移动一个文件到新的文件去
        /// </summary>
        /// <param name="fileNameOld">旧的文件名称</param>
        /// <param name="fileNameNew">新的文件名称</param>
        /// <returns>是否成功</returns>
        protected bool MoveFileToNewFile(string fileNameOld, string fileNameNew)
        {
            try
            {
                var info = new FileInfo(fileNameNew);
                if (!Directory.Exists(info.DirectoryName))
                {
                    Directory.CreateDirectory(info.DirectoryName);
                }

                if (File.Exists(fileNameNew))
                {
                    File.Delete(fileNameNew);
                }

                File.Move(fileNameOld, fileNameNew);
                return true;
            }
            catch (Exception ex)
            {
                LogNet?.WriteException(ToString(), "Move a file to new file failed: ", ex);
                return false;
            }
        }


        /// <summary>
        ///     删除文件并回发确认信息，如果结果异常，则结束通讯
        /// </summary>
        /// <param name="socket">网络套接字</param>
        /// <param name="fullname">完整路径的文件名称</param>
        /// <returns>是否成功的结果对象</returns>
        protected OperateResult DeleteFileAndCheck(
            Socket socket,
            string fullname
        )
        {
            // 删除文件，如果失败，重复三次
            var customer = 0;
            var times = 0;
            while (times < 3)
            {
                times++;
                if (DeleteFileByName(fullname))
                {
                    customer = 1;
                    break;
                }

                System.Threading.Thread.Sleep(500);
            }

            // 回发消息
            return SendStringAndCheckReceive(socket, customer, StringResources.Language.SuccessText);
        }

        #endregion

        #region File Upload Event

        // 文件的上传事件

        #endregion

        #region Public Members

        /// <summary>
        ///     文件所存储的路径
        /// </summary>
        public string FilesDirectoryPath
        {
            get => m_FilesDirectoryPath;
            set => m_FilesDirectoryPath = PreprocessFolderName(value);
        }


        /// <summary>
        ///     获取文件夹的所有文件列表
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <returns>文件列表</returns>
        public virtual string[] GetDirectoryFiles(string factory, string group, string id)
        {
            if (string.IsNullOrEmpty(FilesDirectoryPath)) return new string[0];

            var absolutePath = ReturnAbsoluteFilePath(factory, group, id);

            // 如果文件夹不存在
            if (!Directory.Exists(absolutePath)) return new string[0];
            // 返回文件列表
            return Directory.GetFiles(absolutePath);
        }

        /// <summary>
        ///     获取文件夹的所有文件夹列表
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <returns>文件夹列表</returns>
        public string[] GetDirectories(string factory, string group, string id)
        {
            if (string.IsNullOrEmpty(FilesDirectoryPath)) return new string[0];

            var absolutePath = ReturnAbsoluteFilePath(factory, group, id);

            // 如果文件夹不存在
            if (!Directory.Exists(absolutePath)) return new string[0];
            // 返回文件列表
            return Directory.GetDirectories(absolutePath);
        }

        #endregion

        #region Private Members

        private string m_FilesDirectoryPath; // 文件的存储路径
        private Random m_random = new Random(); // 随机生成的文件名

        #endregion
    }
}