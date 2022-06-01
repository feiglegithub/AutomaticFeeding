//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：AutofacServiceLocator.cs
//   创建时间：2018-11-26 9:15
//   作    者：
//   说    明：
//   修改时间：2018-11-26 9:15
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace NJIS.FPZWS.Ioc.Autofac
{
    /// <summary>
    ///     Autofac implementation of the Microsoft CommonServiceLocator.
    /// </summary>
    public class AutofacServiceLocator : ServiceLocatorImplBase
    {
        /// <summary>
        ///     The <see cref="T:Autofac.IComponentContext" /> from which services
        ///     should be located.
        /// </summary>
        private readonly IComponentContext _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Autofac.Extras.CommonServiceLocator.AutofacServiceLocator" /> class.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="T:Autofac.IComponentContext" /> from which services
        ///     should be located.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Thrown if <paramref name="container" /> is <see langword="null" />.
        /// </exception>
        public AutofacServiceLocator(IComponentContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
        }

        /// <summary>
        ///     Resolves the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be <see langword="null" />.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Thrown if <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (key == null)
            {
                return _container.Resolve(serviceType);
            }

            return _container.ResolveNamed(key, serviceType);
        }

        /// <summary>
        ///     Resolves all requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Thrown if <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            var type = typeof(IEnumerable).MakeGenericType(serviceType);
            return ((IEnumerable) _container.Resolve(type)).Cast<object>();
        }
    }
}