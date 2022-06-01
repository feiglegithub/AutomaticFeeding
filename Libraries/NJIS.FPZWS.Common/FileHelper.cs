// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：FileHelper.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2018-02-03 10:20
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.IO;
using System.Text;

#endregion

namespace NJIS.FPZWS.Common
{
    /// <summary>
    ///     文件帮助
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        ///     根据文件路径获取文件流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetFileByte(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //利用新传来的路径实例化一个FileStream对像
            var fileLength = Convert.ToInt32(fs.Length);
            //得到对像大小
            var fileByteArray = new byte[fileLength];
            //声明一个byte数组
            var br = new BinaryReader(fs);
            //声明一个读取二进流的BinaryReader对像
            for (var i = 0; i < fileLength; i++)
            {
                //循环数组
                br.Read(fileByteArray, 0, fileLength);
                //将数据读取出来放在数组中
            }

            return fileByteArray;
        }

        /// <summary>
        ///     根据文件路径获取内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileConten(string filePath)
        {
            if (!FileExists(filePath))
            {
                return string.Empty;
            }

            using (var stream = new StreamReader(filePath, Encoding.Default))
            {
                return stream.ReadToEnd();
            }
        }

        /// <summary>
        ///     判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        ///     文件流转换成内容
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static string GetFileConten(byte[] byteData)
        {
            var result = string.Empty;
            if (byteData != null)
            {
                result = Encoding.Default.GetString(byteData);
            }

            return result;
        }

        public static string StreamToString(Stream s)
        {
            return StreamToString(s, null);
        }

        public static string StreamToString(Stream s, Encoding e)
        {
            s.Position = 0;
            StreamReader sr = null;
            sr = null == e ? new StreamReader(s) : new StreamReader(s, e);
            var result = sr.ReadToEnd();
            return result;
        }

        /// <summary>
        ///     文件流转换成文件，并返回文件路径
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string SaveFile(string filename, Stream stream, string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var tempFilepath = string.Empty;
            if (stream != null)
            {
                tempFilepath = filePath + @"\" + filename;
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始   
                stream.Seek(0, SeekOrigin.Begin);
                if (FileExists(tempFilepath))
                {
                    File.Delete(tempFilepath);
                }

                using (var fs = new FileStream(tempFilepath, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
            }

            return tempFilepath;
        }

        public static void SaveFile(string filePath, string fileContent, Encoding encode)
        {
            using (var strW = new StreamWriter(filePath, false, encode))
            {
                strW.WriteLine(fileContent);
                strW.Close();
            }
        }

        public static void SaveFile(string filePath, string folderPath, string fileContent)
        {
            IOHelper.CreateDir(folderPath);
            var path = $"{folderPath}\\{filePath}";
            SaveFile(path, fileContent);
        }

        public static void SaveFile(string filePath, string fileContent)
        {
            SaveFile(filePath, fileContent, Encoding.GetEncoding("gb2312"));
        }


        public static string GetFileType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            //获取文件名后缀
            var index = fileName.LastIndexOf(".") + 1;
            var filetype = fileName.Substring(index);

            return filetype;
        }

        /// <summary>
        ///     内容签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetSignature(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            var result = MD5Helper.MD5MethodUTF8(content);
            return result;
        }
    }
}