// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：TypeFinder.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-08-31 16:23
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.Common.Dependency;

#endregion

namespace NJIS.FPZWS.Common.Reflection
{
    /// <summary>
    ///     <see cref="ISingletonDependency" />接口实现类查找器
    /// </summary>
    public class TypeFinder<T> : ITypeFinder
    {
        public TypeFinder()
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
        }

        public TypeFinder(string path)
        {
            AssemblyFinder = new DirectoryAssemblyFinder(path);
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

            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return types.Distinct().ToArray();
        }


        /// <summary>
        ///     查找所有项
        /// </summary>
        /// <returns></returns>
        public Type[] FindInterfacesAll()
        {
            var assemblies = AssemblyFinder.FindAll();

            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetInterface(typeof(T).Name) != null && !type.IsAbstract)
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return types.Distinct().ToArray();
        }


        /// <summary>
        ///     查找所有项
        /// </summary>
        /// <returns></returns>
        public Type[] FindInterfacesAll(Type t)
        {
            var assemblies = AssemblyFinder.FindAll();

            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!type.IsAbstract)
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return types.Distinct().ToArray();
        }
    }
}