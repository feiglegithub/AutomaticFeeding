// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ServicesBuilder.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Linq;
using NJIS.FPZWS.Common.Reflection;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     服务器映射集合创建功能
    /// </summary>
    public class ServicesBuilder : IServicesBuilder
    {
        private readonly ServiceBuildOptions _options;

        /// <summary>
        ///     初始化一个<see cref="ServicesBuilder" />类型的新实例
        /// </summary>
        public ServicesBuilder()
            : this(new ServiceBuildOptions())
        {
        }

        /// <summary>
        ///     初始化一个<see cref="ServicesBuilder" />类型的新实例
        /// </summary>
        public ServicesBuilder(ServiceBuildOptions options)
        {
            _options = options;
        }

        /// <summary>
        ///     将当前服务创建为
        /// </summary>
        /// <returns>服务映射集合</returns>
        public IServiceCollection Build()
        {
            IServiceCollection services = new ServiceCollection();
            var options = _options;

            //添加即时生命周期类型的映射
            var dependencyTypes = options.TransientTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, LifetimeStyle.Transient);

            //添加局部生命周期类型的映射
            dependencyTypes = options.ScopeTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, LifetimeStyle.Scoped);

            //添加单例生命周期类型的映射
            dependencyTypes = options.SingletonTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, LifetimeStyle.Singleton);

            //全局服务
            AddGlobalTypes(services);

            return services;
        }

        /// <summary>
        ///     以类型实现的接口进行服务添加，需排除
        ///     <see cref="ITransientDependency" />、
        ///     <see cref="IScopeDependency" />、
        ///     <see cref="ISingletonDependency" />、
        ///     <see cref="IDependency" />、
        ///     <see cref="IDisposable" />等非业务接口，如无接口则注册自身
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="implementationTypes">要注册的实现类型集合</param>
        /// <param name="lifetime">注册的生命周期类型</param>
        protected virtual void AddTypeWithInterfaces(IServiceCollection services, Type[] implementationTypes,
            LifetimeStyle lifetime)
        {
            foreach (var implementationType in implementationTypes)
            {
                if (implementationType.IsAbstract || implementationType.IsInterface)
                {
                    continue;
                }

                var interfaceTypes = GetImplementedInterfaces(implementationType);
                if (interfaceTypes.Length == 0)
                {
                    services.Add(implementationType, implementationType, lifetime);
                    continue;
                }

                foreach (var interfaceType in interfaceTypes)
                {
                    services.Add(interfaceType, implementationType, lifetime);
                }
            }
        }

        /// <summary>
        ///     重写以实现添加全局特殊类型映射
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        protected virtual void AddGlobalTypes(IServiceCollection services)
        {
            services.AddSingleton<IAllAssemblyFinder, DirectoryAssemblyFinder>();
        }

        private static Type[] GetImplementedInterfaces(Type type)
        {
            Type[] exceptInterfaces =
            {
                typeof(IDisposable),
                typeof(IDependency),
                typeof(ITransientDependency),
                typeof(IScopeDependency),
                typeof(ISingletonDependency)
            };
            var interfaceTypes = type.GetInterfaces().Where(m => !exceptInterfaces.Contains(m)).ToArray();
            for (var index = 0; index < interfaceTypes.Length; index++)
            {
                var interfaceType = interfaceTypes[index];
                if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition &&
                    interfaceType.FullName == null)
                {
                    interfaceTypes[index] = interfaceType.GetGenericTypeDefinition();
                }
            }

            return interfaceTypes;
        }
    }
}