// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：StartParametersExtensions.cs
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
using Topshelf.HostConfigurators;

#endregion

namespace NJIS.FPZWS.App.StartParameters
{
    public static class StartParametersExtensions
    {
        private const string Prefix = "tssp";

        private static readonly Dictionary<HostConfigurator, List<Tuple<string, string>>> ActionTable =
            new Dictionary<HostConfigurator, List<Tuple<string, string>>>();

        public static HostConfigurator EnableStartParameters(this HostConfigurator configurator)
        {
            configurator.UseEnvironmentBuilder(e => new SpWindowsHostEnvironmentBuilder(e));
            return configurator;
        }

        public static HostConfigurator WithStartParameter(this HostConfigurator configurator, string name,
            Action<string> action)
        {
            configurator.AddCommandLineDefinition(Prefix + name, action);
            configurator.AddCommandLineDefinition(name, s => Add(configurator, Prefix + name, s));

            return configurator;
        }

        public static HostConfigurator WithCustomStartParameter(this HostConfigurator configurator, string argName,
            string paramName, Action<string> action)
        {
            configurator.AddCommandLineDefinition(paramName, action);
            configurator.AddCommandLineDefinition(argName, s => Add(configurator, paramName, s));

            return configurator;
        }

        public static HostConfigurator WithStartParameter(this HostConfigurator configurator, string name, string value,
            Action<string> action)
        {
            Add(configurator, name, value);
            configurator.AddCommandLineDefinition(name, action);

            return configurator;
        }

        private static void Add(HostConfigurator configurator, string name, string value)
        {
            List<Tuple<string, string>> pairs;
            if (!ActionTable.TryGetValue(configurator, out pairs))
            {
                pairs = new List<Tuple<string, string>>();
                ActionTable.Add(configurator, pairs);
            }

            pairs.Add(new Tuple<string, string>(name, value));
        }

        internal static List<Tuple<string, string>> Commands(HostConfigurator configurator)
        {
            List<Tuple<string, string>> pairs;
            ActionTable.TryGetValue(configurator, out pairs);

            return pairs;
        }
    }
}