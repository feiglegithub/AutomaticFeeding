// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ServiceBuildOptions.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region namespace

#endregion

#region

using NJIS.FPZWS.Common.Reflection;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     服务创建配置信息
    /// </summary>
    public class ServiceBuildOptions
    {
        /// <summary>
        ///     初始化一个<see cref="ServiceBuildOptions" />类型的新实例
        /// </summary>
        public ServiceBuildOptions()
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
            TransientTypeFinder = new TransientDependencyTypeFinder();
            ScopeTypeFinder = new ScopeDependencyTypeFinder();
            SingletonTypeFinder = new SingletonDependencyTypeFinder();
        }

        /// <summary>
        ///     获取或设置 程序集查找器
        /// </summary>
        public IAllAssemblyFinder AssemblyFinder { get; set; }

        /// <summary>
        ///     获取或设置 即时生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder TransientTypeFinder { get; set; }

        /// <summary>
        ///     获取或设置 范围生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder ScopeTypeFinder { get; set; }

        /// <summary>
        ///     获取或设置 单例生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder SingletonTypeFinder { get; set; }
    }
}