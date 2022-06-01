// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IOHelper.cs
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

#endregion

namespace NJIS.FPZWS.Common
{
    public class IOHelper
    {
        /// <summary>
        ///     从文件读取字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetStringFromFile(string filePath)
        {
            try
            {
                if (!FileExists(filePath))
                    return null;

                using (var stream = File.OpenText(filePath))
                {
                    return stream.ReadToEnd();
                    //byte[] bytes = new byte[stream.Length];
                    //return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                return string.Empty;
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
        ///     创建文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
        }

        /// <summary>
        ///     文件夹是否存在
        /// </summary>
        /// <param name="dirString"></param>
        public static bool DirectoryExists(string dirString)
        {
            return Directory.Exists(dirString);
        }

        /// <summary>
        ///     创建文件夹
        /// </summary>
        /// <param name="dirString"></param>
        public static void CreateDir(string dirString)
        {
            if (!DirectoryExists(dirString))
                Directory.CreateDirectory(dirString);
        }

        /// <summary>
        ///     获取目录下的所有文件
        /// </summary>
        /// <returns></returns>
        public static string[] GetDirFile(string dirPath)
        {
            return Directory.GetFiles(dirPath);
        }

        /// <summary>
        ///     合并路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        ///     删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void DeleteFile(string filePath)
        {
            if (FileExists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        ///     获取文件夹下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        ///     返回与指定搜索匹配的文件（包括其路径）的名称,指定目录中的模式
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        /// <summary>
        ///     返回匹配的子目录的名称（包括其路径）,指定的目录中指定的搜索模式，并可选择搜索,子目录。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

        /// <summary>
        ///     返回指定的子目录的名称（包括其路径）目录。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        /// <summary>
        ///     //返回匹配指定的子目录的名称（包括其路径）  //在指定目录中的搜索模式。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string path, string searchPattern)
        {
            return Directory.GetDirectories(path, searchPattern);
        }

        /// <summary>
        ///     //返回匹配的子目录的名称（包括其路径）
        ///     指定的目录中指定的搜索模式，并可选择。搜索子目录。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }
    }
}