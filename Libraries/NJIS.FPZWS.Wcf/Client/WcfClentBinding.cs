// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Client.Wcf
//  文 件 名：WcfClentBinding.cs
//  创建时间：2017-12-29 8:12
//  作    者：
//  说    明：
//  修改时间：2017-12-29 9:49
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    /// <summary>
    ///     客户端常量配值实体
    /// </summary>
    internal class WcfClentBinding
    {
        public Binding BindingType { get; set; }

        public int MaxBufferPoolSize { get; set; }

        public int MaxBufferSize { get; set; }

        public int MaxReceivedMessageSize { get; set; }

        /// <summary>
        ///     客户端池子最大缓存数目
        /// </summary>
        public int MaxConnections { get; set; }

        public int ListenBacklog { get; set; }

        public TimeSpan SendTimeout { get; set; }

        public TimeSpan OpenTimeout { get; set; }

        public TimeSpan ReceiveTimeout { get; set; }

        public TransferMode TransferMode { get; set; }

        public SecurityMode SecurityMode { get; set; }

        public string Uri { get; set; }

        public int ReaderQuotasMaxDepth { get; set; }

        public int ReaderQuotasMaxStringContentLength { get; set; }

        public int ReaderQuotasMaxArrayLength { get; set; }

        public int ReaderQuotasMaxBytesPerRead { get; set; }

        public int ReaderQuotasMaxNameTableCharCount { get; set; }

        public bool ReliableSessionOrdered { get; set; }

        public bool ReliableSessionEnabled { get; set; }

        public TimeSpan ReliableSessionInactivityTimeout { get; set; }

        /// <summary>
        ///     是否启用自定义序列化
        /// </summary>
        public bool EnableBinaryFormatterBehavior { get; set; }

        /// <summary>
        ///     是否启用Wcf连接池
        /// </summary>
        public bool IsUseWcfPool { get; set; }

        /// <summary>
        ///     Wcf连接池最大值
        /// </summary>
        public int WcfMaxPoolSize { get; set; }

        /// <summary>
        ///     Wcf获取连接过期时间
        /// </summary>
        public long WcfOutTime { get; set; }

        /// <summary>
        ///     Wcf连接失效时间
        /// </summary>
        public long WcfFailureTime { get; set; }

        /// <summary>
        ///     Wcf连接池监控线程扫描间隔时间（秒为单位）
        /// </summary>
        public int WcfPoolMonitorReapTime { get; set; }
    }
}