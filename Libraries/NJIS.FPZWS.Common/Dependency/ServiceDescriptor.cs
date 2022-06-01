// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ServiceDescriptor.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Diagnostics;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     依赖注入映射描述信息
    /// </summary>
    [DebuggerDisplay("Lifetime = {Lifetime}, ServiceType = {ServiceType}, ImplementationType = {ImplementationType}")]
    public class ServiceDescriptor
    {
        /// <summary>
        ///     创建即时生命周期类型的描述
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImplementation">服务实现类型</typeparam>
        /// <returns></returns>
        public static ServiceDescriptor Transient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(LifetimeStyle.Transient);
        }

        /// <summary>
        ///     创建即时生命周期类型的描述
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">服务实现类型</param>
        /// <returns></returns>
        public static ServiceDescriptor Transient(Type serviceType, Type implementationType)
        {
            return Describe(serviceType, implementationType, LifetimeStyle.Transient);
        }

        /// <summary>
        ///     创建即时生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Transient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> factory)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Transient);
        }

        /// <summary>
        ///     创建即时生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Transient);
        }

        /// <summary>
        ///     创建即时生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Transient(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return Describe(serviceType, factory, LifetimeStyle.Transient);
        }

        /// <summary>
        ///     创建局部生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Scoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(LifetimeStyle.Scoped);
        }

        /// <summary>
        ///     创建局部生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Scoped(Type serviceType, Type implementationType)
        {
            return Describe(serviceType, implementationType, LifetimeStyle.Scoped);
        }

        /// <summary>
        ///     创建局部生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Scoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> factory)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Scoped);
        }

        /// <summary>
        ///     创建局部生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Scoped);
        }

        /// <summary>
        ///     创建局部生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Scoped(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return Describe(serviceType, factory, LifetimeStyle.Scoped);
        }

        /// <summary>
        ///     创建单例生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Singleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return Describe<TService, TImplementation>(LifetimeStyle.Singleton);
        }

        /// <summary>
        ///     创建单例生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Singleton(Type serviceType, Type implementationType)
        {
            return Describe(serviceType, implementationType, LifetimeStyle.Singleton);
        }

        /// <summary>
        ///     创建单例生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Singleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> factory)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Singleton);
        }

        /// <summary>
        ///     创建单例生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return Describe(typeof(TService), factory, LifetimeStyle.Singleton);
        }

        /// <summary>
        ///     创建单例生命周期类型的描述
        /// </summary>
        public static ServiceDescriptor Singleton(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return Describe(serviceType, factory, LifetimeStyle.Singleton);
        }

        /// <summary>
        ///     创建单例生命周期实例的描述
        /// </summary>
        public static ServiceDescriptor Instance<TService>(TService instance)
            where TService : class
        {
            return Instance(typeof(TService), instance);
        }

        /// <summary>
        ///     创建单例生命周期实例的描述
        /// </summary>
        public static ServiceDescriptor Instance(Type serviceType, object instance)
        {
            return Describe(serviceType, instance);
        }

        private static ServiceDescriptor Describe<TService, TImplementation>(LifetimeStyle lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            return Describe(typeof(TService), typeof(TImplementation), lifetime);
        }

        private static ServiceDescriptor Describe(Type serviceType, Type implementationType, LifetimeStyle lifetime)
        {
            return new ServiceDescriptor(serviceType, implementationType, lifetime);
        }

        private static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> factory,
            LifetimeStyle lifetime)
        {
            return new ServiceDescriptor(serviceType, factory, lifetime);
        }

        private static ServiceDescriptor Describe(Type serviceType, object instance)
        {
            return new ServiceDescriptor(serviceType, instance);
        }

        internal Type GetImplementationType()
        {
            if (ImplementationType != null)
            {
                return ImplementationType;
            }

            if (ImplementationInstance != null)
            {
                return ImplementationInstance.GetType();
            }

            if (ImplementationFactory != null)
            {
                var typeArgs = ImplementationFactory.GetType().GenericTypeArguments;
                if (typeArgs.Length == 2)
                {
                    return typeArgs[1];
                }
            }

            throw new ArgumentException(string.Format("类型“{0}”的实现类型无法找到", ServiceType));
        }

        #region 构造函数

        /// <summary>
        ///     初始化一个<see cref="ServiceDescriptor" />类型的新实例
        /// </summary>
        public ServiceDescriptor(Type serviceType, Type implementationType, LifetimeStyle lifetime)
            : this(serviceType, lifetime)
        {
            ImplementationType = implementationType;
        }

        /// <summary>
        ///     初始化一个<see cref="ServiceDescriptor" />类型的新实例
        /// </summary>
        public ServiceDescriptor(Type serviceType, object instance)
            : this(serviceType, LifetimeStyle.Singleton)
        {
            ImplementationInstance = instance;
        }

        /// <summary>
        ///     初始化一个<see cref="ServiceDescriptor" />类型的新实例
        /// </summary>
        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, LifetimeStyle lifetime)
            : this(serviceType, lifetime)
        {
            ImplementationFactory = factory;
        }

        /// <summary>
        ///     初始化一个<see cref="ServiceDescriptor" />类型的新实例
        /// </summary>
        public ServiceDescriptor(Type serviceType, LifetimeStyle lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     获取 生命周期类型的描述
        /// </summary>
        public LifetimeStyle Lifetime { get; }

        /// <summary>
        ///     获取 服务类型
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        ///     获取 服务实现类型
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        ///     获取 服务实例
        /// </summary>
        public object ImplementationInstance { get; }

        /// <summary>
        ///     获取 服务实例创建工厂
        /// </summary>
        public Func<IServiceProvider, object> ImplementationFactory { get; }

        #endregion
    }
}