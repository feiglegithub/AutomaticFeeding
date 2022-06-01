// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：SpWindowsHostEnvironmentBuilder.cs
//  创建时间：2016-11-11 12:50
//  作    者：
//  说    明：
//  修改时间：2017-07-10 12:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using Topshelf.Builders;
using Topshelf.HostConfigurators;
using Topshelf.Runtime;

#endregion

namespace NJIS.FPZWS.App.StartParameters
{
    public class SpWindowsHostEnvironmentBuilder : EnvironmentBuilder
    {
        private readonly HostConfigurator _hostConfigurator;

        public SpWindowsHostEnvironmentBuilder(HostConfigurator configurator)
        {
            _hostConfigurator = configurator;
        }

        public HostEnvironment Build()
        {
            return new SpWindowsHostEnvironment(_hostConfigurator);
        }
    }
}