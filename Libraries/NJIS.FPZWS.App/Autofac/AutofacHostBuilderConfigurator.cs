// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：AutofacHostBuilderConfigurator.cs
//  创建时间：2016-11-11 12:50
//  作    者：
//  说    明：
//  修改时间：2017-07-10 12:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using Autofac;
using Topshelf.Builders;
using Topshelf.Configurators;
using Topshelf.HostConfigurators;

#endregion

namespace NJIS.FPZWS.App.Autofac
{
    public class AutofacHostBuilderConfigurator : HostBuilderConfigurator
    {
        #region Constructors and Destructors

        public AutofacHostBuilderConfigurator(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
            {
                throw new ArgumentNullException("lifetimeScope");
            }

            LifetimeScope = lifetimeScope;
        }

        #endregion

        #region Public Properties

        public static ILifetimeScope LifetimeScope { get; private set; }

        #endregion

        #region Static Fields

        #endregion

        #region Public Methods and Operators

        public HostBuilder Configure(HostBuilder builder)
        {
            return builder;
        }

        public IEnumerable<ValidateResult> Validate()
        {
            yield break;
        }

        #endregion
    }
}