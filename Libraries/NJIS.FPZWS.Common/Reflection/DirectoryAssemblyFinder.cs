﻿// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：DirectoryAssemblyFinder.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace NJIS.FPZWS.Common.Reflection
{
    /// <summary>
    ///     目录程序集查找器
    /// </summary>
    public class DirectoryAssemblyFinder : IAllAssemblyFinder
    {
        private static readonly IDictionary<string, Assembly[]> AssembliesesDict = new Dictionary<string, Assembly[]>();
        private readonly string _path;

        /// <summary>
        ///     初始化一个<see cref="DirectoryAssemblyFinder" />类型的新实例
        /// </summary>
        public DirectoryAssemblyFinder()
            : this(GetBinPath())
        {
        }

        /// <summary>
        ///     初始化一个<see cref="DirectoryAssemblyFinder" />类型的新实例
        /// </summary>
        public DirectoryAssemblyFinder(string path)
        {
            _path = path;
        }

        /// <summary>
        ///     查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public Assembly[] Find(Func<Assembly, bool> predicate)
        {
            return FindAll().Where(predicate).ToArray();
        }

        /// <summary>
        ///     查找所有项
        /// </summary>
        /// <returns></returns>
        public Assembly[] FindAll()
        {
            if (AssembliesesDict.ContainsKey(_path))
            {
                return AssembliesesDict[_path];
            }

            var files = Directory.GetFiles(_path, "*.dll", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(_path, "*.exe", SearchOption.AllDirectories))
                .ToArray();
            var assemblies = new List<Assembly>();
            foreach (var file in files)
            {
                try
                {
                    var ass = Assembly.LoadFrom(file);
                    assemblies.Add(ass);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            AssembliesesDict.Add(_path, assemblies.ToArray());
            return assemblies.ToArray();
        }

        private static string GetBinPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
            //var path = AppDomain.CurrentDomain.BaseDirectory;
            //return path == Environment.CurrentDirectory + "\\" ? path : Path.Combine(path, "bin");
        }
    }
}