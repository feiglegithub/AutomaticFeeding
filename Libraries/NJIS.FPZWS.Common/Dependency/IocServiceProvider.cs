// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IocServiceProvider.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     默认IoC服务提供者实现
    /// </summary>
    public class IocServiceProvider : IServiceProvider
    {
        private readonly IIocResolver _resolver;

        /// <summary>
        ///     初始化一个<see cref="IocServiceProvider" />类型的新实例
        /// </summary>
        public IocServiceProvider(IIocResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        ///     获取指定类型的服务对象。
        /// </summary>
        /// <returns>
        ///     <paramref name="serviceType" /> 类型的服务对象。 - 或 - 如果没有 <paramref name="serviceType" /> 类型的服务对象，则为 null。
        /// </returns>
        /// <param name="serviceType">一个对象，它指定要获取的服务对象的类型。</param>
        /// <filterpriority>2</filterpriority>
        public object GetService(Type serviceType)
        {
            return _resolver.Resolve(serviceType);
        }
    }
}