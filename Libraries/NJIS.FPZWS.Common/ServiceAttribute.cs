// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ServiceAttribute.cs
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

namespace NJIS.FPZWS.Common
{
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(string service, string serviceName, string displayName) : this(service, serviceName,
            displayName, "")
        {
        }

        public ServiceAttribute(string service, string serviceName, string displayName, string instanceName) : this(
            service, serviceName, displayName, instanceName, "")
        {
        }

        public ServiceAttribute(string service, string serviceName, string displayName, string instanceName,
            string description)
        {
            Service = service;
            ServiceName = serviceName;
            DisplayName = displayName;
            Description = description;
            InstanceName = instanceName;
        }

        public string Service { get; set; }
        public string ServiceName { get; set; }
        public string InstanceName { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}