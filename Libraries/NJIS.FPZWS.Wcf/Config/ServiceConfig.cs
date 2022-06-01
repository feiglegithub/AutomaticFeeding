// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：ServiceConfig.cs
//  创建时间：2018-01-06 8:20
//  作    者：
//  说    明：
//  修改时间：2018-01-06 9:53
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class ServiceConfig
    {
        public ServiceConfig()
        {
            Services = new List<Service>();
            Behaviors = new List<ServiceBehavior>();
            Bindings = new List<Binding>();
            Services.Add(new Service()
            {
                BehaviorConfiguration = "defaultbehavior",
                Name = "NJIS.FPZWS.Wcf.Monitor.MonitorControl",
                BaseAddresses = new List<string>(),
                Endpoints = new List<EndPoint>()
                {
                   new EndPoint(){
                       Binding ="netTcpBinding",
                       BindingConfiguration ="defaultTcpBinding",
                       
                   }
                }
            });
        }

        public List<string> BaseAddresses { get; set; }

        public List<Service> Services { get; set; }


        public List<ServiceBehavior> Behaviors { get; set; }

        public List<Binding> Bindings { get; set; }



    }

}