// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：DefaultClientBuilder.cs
//  创建时间：2018-01-08 19:01
//  作    者：
//  说    明：
//  修改时间：2018-01-16 17:55
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using Newtonsoft.Json;
using NJIS.FPZWS.Wcf.Config;
using System.Linq;
using NJIS.FPZWS.Wcf.Service;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    public class DefaultClientBuilder : IClientBuilder
    {
        private const string DefaultBindgingConfigurationName = "defaulttpcbinding";
        public DefaultClientBuilder()
        {
            GetConfig();
        }

        public System.ServiceModel.NetTcpBinding GetBinding<T>()
        {
            var ep = GetEndPoint<T>();
            if (string.IsNullOrEmpty(ep.BindingConfiguration))
            {
                ep.BindingConfiguration = DefaultBindgingConfigurationName;
            }
            var c = _config.Bindings.FirstOrDefault(m => m.Name.ToLower() == ep.BindingConfiguration.ToLower());
            var binding = new System.ServiceModel.NetTcpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(c.SendTimeout),
                ListenBacklog = c.ListenBacklog,
                MaxBufferPoolSize = c.MaxBufferPoolSize,
                MaxBufferSize = c.MaxBufferSize,
                MaxReceivedMessageSize = c.MaxReceivedMessageSize,
                MaxConnections = c.MaxConnections,
                ReaderQuotas =
                {
                    MaxDepth = c.ReaderQuotasMaxDepth,
                    MaxStringContentLength = c.ReaderQuotasMaxStringContentLength,
                    MaxArrayLength = c.ReaderQuotasMaxArrayLength,
                    MaxBytesPerRead = c.ReaderQuotasMaxBytesPerRead,
                    MaxNameTableCharCount = c.ReaderQuotasMaxNameTableCharCount
                },
                Security =
                {
                    Mode = (SecurityMode) Enum.Parse(typeof(SecurityMode), c.SecurityMode),
                    Transport =
                    {
                        ClientCredentialType = (TcpClientCredentialType) Enum.Parse(typeof(TcpClientCredentialType),
                            c.SecurityTransportClientCredentialType)
                    },
                    Message =
                    {
                        ClientCredentialType = (MessageCredentialType) Enum.Parse(typeof(MessageCredentialType),
                            c.SecurityMessageClientCredentialType)
                    }
                }
            };
            return binding;
        }

        private EndPoint GetEndPoint<T>()
        {
            var ep = _config.Endpoints.FirstOrDefault(m => m.Contract.ToLower() == typeof(T).FullName.ToLower());
            if (ep == null)
            {
                var bas = GetBaseAddress(WcfType.Tcp);
                ep = new EndPoint {Contract = typeof(T).FullName};
                ep.Address = bas +  ep.Contract.Replace('.', '/');
                ep.BindingConfiguration = "defaultTpcBinding";
                ep.Binding = "netTcpBinding";
            }
            return ep;
        }



        /// <summary>
        ///     获取服务基地址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private Uri GetBaseAddress(WcfType type)
        {
            Uri uri =null;
            foreach (var serviceBaseAddress in _config.BaseAddresses)
            {
                if (type == WcfType.Tcp && !serviceBaseAddress.StartsWith("net.tcp:")) continue;
                if (type == WcfType.Http && !serviceBaseAddress.StartsWith("http:")) continue;
                uri = new Uri(serviceBaseAddress);
                break;
            }
            return uri;
        }

        public EndpointAddress GetAddress<T>()
        {
            var ep = GetEndPoint<T>();
            return new EndpointAddress(ep.Address);
        }

        public bool IsInit { get; private set; }

        private ClientConfig _config;
        private void GetConfig()
        {
            IsInit = true;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = Path.Combine(filePath, "client.json");
            if (!File.Exists(fileName))
            {
                var json = JsonConvert.SerializeObject(new ClientConfig());
                File.WriteAllText(fileName, json);
            }
            var jsonText = File.ReadAllText(fileName);
            _config = JsonConvert.DeserializeObject<ClientConfig>(jsonText);
        }

    }
}