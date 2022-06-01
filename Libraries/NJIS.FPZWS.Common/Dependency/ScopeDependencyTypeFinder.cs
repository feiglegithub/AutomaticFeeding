// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ScopeDependencyTypeFinder.cs
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
using System.Linq;
using NJIS.FPZWS.Common.Reflection;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     <see cref="IScopeDependency" />接口实现类查找器
    /// </summary>
    public class ScopeDependencyTypeFinder : ITypeFinder
    {
        /// <summary>
        ///     初始化一个<see cref="ScopeDependencyTypeFinder" />类型的新实例
        /// </summary>
        public ScopeDependencyTypeFinder()
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
        }

        /// <summary>
        ///     获取或设置 程序集查找器
        /// </summary>
        public IAllAssemblyFinder AssemblyFinder { get; set; }

        /// <summary>
        ///     查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public Type[] Find(Func<Type, bool> predicate)
        {
            return FindAll().Where(predicate).ToArray();
        }

        /// <summary>
        ///     查找所有项
        /// </summary>
        /// <returns></returns>
        public Type[] FindAll()
        {
            var assemblies = AssemblyFinder.FindAll();
            var lst = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(IScopeDependency).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            lst.Add(type);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return lst.Distinct().ToArray();
        }
    }
}