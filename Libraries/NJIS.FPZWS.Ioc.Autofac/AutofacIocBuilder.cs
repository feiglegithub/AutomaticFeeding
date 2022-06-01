//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：AutofacIocBuilder.cs
//   创建时间：2018-11-26 9:15
//   作    者：
//   说    明：
//   修改时间：2018-11-26 9:15
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Reflection;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.Common.Dependency;

#endregion

namespace NJIS.FPZWS.Ioc.Autofac
{
    /// <summary>
    ///     Autofac依赖注入初始化类
    /// </summary>
    public class AutofacIocBuilder : IocBuilderBase
    {
        /// <summary>
        ///     初始化一个<see cref="AutofacIocBuilder" />类型的新实例
        /// </summary>
        /// <param name="services">服务信息集合</param>
        public AutofacIocBuilder(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        ///     获取 依赖注入解析器
        /// </summary>
        public IIocResolver Resolver { get; private set; }

        /// <summary>
        ///     添加自定义服务映射
        /// </summary>
        /// <param name="services">服务信息集合</param>
        protected override void AddCustomTypes(IServiceCollection services)
        {
            services.AddInstance(this);
            services.AddSingleton<IIocResolver, IocResolver>();
        }

        /// <summary>
        ///     构建服务
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="assemblies">程序集集合</param>
        /// <returns>服务提供者</returns>
        protected override IServiceProvider Build(IServiceCollection services, Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var container = builder.Build();
            IocResolver.Container = container;
            Resolver = container.Resolve<IIocResolver>();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            return Resolver.Resolve<IServiceProvider>();
        }
    }
}