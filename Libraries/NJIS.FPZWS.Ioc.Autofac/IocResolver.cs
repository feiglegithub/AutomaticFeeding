//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：IocResolver.cs
//   创建时间：2018-11-26 9:15
//   作    者：
//   说    明：
//   修改时间：2018-11-26 9:15
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NJIS.FPZWS.Common.Dependency;

#endregion

namespace NJIS.FPZWS.Ioc.Autofac
{
    /// <summary>
    ///     本地应用依赖注入解析
    /// </summary>
    public class IocResolver : IIocResolver
    {
        /// <summary>
        ///     获取 依赖注入容器
        /// </summary>
        internal static IContainer Container { get; set; }

        /// <summary>
        ///     获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        /// <summary>
        ///     获取指定类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return Container.ResolveOptional(type);
        }

        /// <summary>
        ///     获取指定类型的所有实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> Resolves<T>()
        {
            return Container.ResolveOptional<IEnumerable<T>>();
        }

        /// <summary>
        ///     获取指定类型的所有实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IEnumerable<object> Resolves(Type type)
        {
            var typeToResolve = typeof(IEnumerable<>).MakeGenericType(type);
            var array = Container.ResolveOptional(typeToResolve) as Array;
            if (array != null)
            {
                return array.Cast<object>();
            }

            return new object[0];
        }
    }
}