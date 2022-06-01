// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：Binding.cs
//  创建时间：2018-01-06 10:08
//  作    者：
//  说    明：
//  修改时间：2018-01-06 10:08
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class Binding
    {
        public string Name { get; set; }
        public BindingType Type { get; set; }
    }

    [Serializable]
    public enum BindingType
    {
        BaseHttpBinding=0,
        BasicHttpsBindings=1,
        NetTcpBinding=2,
        UdpBinding=3
    }

    [Serializable]
    public class NetTcpBinding : Binding
    {
        public NetTcpBinding()
        {
            ListenBacklog = 200;
            SendTimeout = 600;
            MaxBufferPoolSize = 2147483647;
            MaxBufferSize = 2147483647;
            MaxConnections = 1000;
            MaxReceivedMessageSize = 2147483647;
            ReaderQuotasMaxDepth = 64;
            ReaderQuotasMaxStringContentLength = 8192;
            ReaderQuotasMaxArrayLength = 16384;
            ReaderQuotasMaxBytesPerRead = 4096;
            ReaderQuotasMaxNameTableCharCount = 16384;
            SecurityMode = "None";
            SecurityTransportClientCredentialType = "None";
            SecurityMessageClientCredentialType = "None";
        }

        public int SendTimeout { get; set; }
        public int ListenBacklog { get; set; }
        public int MaxBufferPoolSize { get; set; }
        public int MaxBufferSize { get; set; }
        public int MaxConnections { get; set; }
        public int MaxReceivedMessageSize { get; set; }
        public int ReaderQuotasMaxDepth { get; set; }
        public int ReaderQuotasMaxStringContentLength { get; set; }
        public int ReaderQuotasMaxArrayLength { get; set; }
        public int ReaderQuotasMaxBytesPerRead { get; set; }
        public int ReaderQuotasMaxNameTableCharCount { get; set; }
        public string SecurityMode { get; set; }
        public string SecurityTransportClientCredentialType { get; set; }
        public string SecurityMessageClientCredentialType { get; set; }
    }
}