// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：Client.cs
//  创建时间：2018-01-08 14:20
//  作    者：
//  说    明：
//  修改时间：2018-01-08 14:21
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class ClientConfig
    {
        public ClientConfig()
        {
            Bindings = new List<NetTcpBinding>();
            Endpoints = new List<EndPoint>();
            BaseAddresses = new List<string>();
        }
        public List<string> BaseAddresses { get; set; }
        public List<NetTcpBinding> Bindings { get; set; }

        public List<EndPoint> Endpoints { get; set; }
    }

    [Serializable]
    public class Server
    {
        public Server()
        {
            Contracts = new List<Contract>();
        }

        public string Address { get; set; }
        public string BindingConfiguration { get; set; }
        public bool IsUseWcfPool { get; set; }

        public List<Contract> Contracts { get; set; }
    }

    [Serializable]
    public class Contract
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}