// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：Behavior.cs
//  创建时间：2018-01-06 10:07
//  作    者：
//  说    明：
//  修改时间：2018-01-06 10:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class Behavior
    {
        public BehaviorType Type { get; set; }

        public string Name { get; set; }
    }

    /// <summary>
    ///     行为类别
    /// </summary>
    [Serializable]
    public enum BehaviorType
    {
        ServiceBehaviors = 0,
        EndpointBehaviors = 1
    }

    [Serializable]
    public class ServiceBehavior : Behavior
    {
        public ServiceBehavior()
        {
            Type = BehaviorType.ServiceBehaviors;
            MaxConcurrentCalls = 1000;
            MaxConcurrentSessions = 1000;
            MaxConcurrentInstances = 1000;
            IncludeExceptionDetailInFaults = true;
            HttpGetEnabled = true;
            HttpsGetEnabled = false;
        }


        public int MaxConcurrentCalls { get; set; }
        public int MaxConcurrentSessions { get; set; }
        public int MaxConcurrentInstances { get; set; }
        public bool IncludeExceptionDetailInFaults { get; set; }
        public bool HttpGetEnabled { get; set; }
        public bool HttpsGetEnabled { get; set; }
        public string HttpGetBinding { get; set; }
        public string HttpGetBindingConfiguration { get; set; }
    }
}