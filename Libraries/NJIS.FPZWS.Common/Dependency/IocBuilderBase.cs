// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IocBuilderBase.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Reflection;
using NJIS.FPZWS.Common.Reflection;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     依赖注入构建器基类，从程序集中反射进行依赖注入接口与实现的注册
    /// </summary>
    public abstract class IocBuilderBase : IIocBuilder
    {
        private readonly IServiceCollection _services;
        private bool _isBuilded;

        /// <summary>
        ///     初始化一个<see cref="IocBuilderBase" />类型的新实例
        /// </summary>
        /// <param name="services">服务信息集合</param>
        protected IocBuilderBase(IServiceCollection services)
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
            _services = services.Clone();
            _isBuilded = false;
        }

        /// <summary>
        ///     获取或设置 程序集查找器
        /// </summary>
        public IAllAssemblyFinder AssemblyFinder { get; set; }

        /// <summary>
        ///     获取 服务提供者
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///     开始构建依赖注入映射
        /// </summary>
        /// <returns>服务提供者</returns>
        public IServiceProvider Build()
        {
            if (_isBuilded)
            {
                return ServiceProvider;
            }

            //设置各个框架的DependencyResolver
            var assemblies = AssemblyFinder.FindAll();

            AddCustomTypes(_services);

            ServiceProvider = Build(_services, assemblies);
            _isBuilded = true;
            return ServiceProvider;
        }

        /// <summary>
        ///     添加自定义服务映射
        /// </summary>
        /// <param name="services">服务信息集合</param>
        protected virtual void AddCustomTypes(IServiceCollection services)
        {
        }

        /// <summary>
        ///     重写以实现构建服务
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="assemblies">要检索的程序集集合</param>
        /// <returns>服务提供者</returns>
        protected abstract IServiceProvider Build(IServiceCollection services, Assembly[] assemblies);
    }
}