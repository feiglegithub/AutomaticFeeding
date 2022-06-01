// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：DefaultServiceConfigBuilder.cs
//  创建时间：2018-01-08 19:01
//  作    者：
//  说    明：
//  修改时间：2018-01-16 11:55
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Newtonsoft.Json;
using NJIS.FPZWS.Wcf.Config;
using Binding = System.ServiceModel.Channels.Binding;
using NetTcpBinding = System.ServiceModel.NetTcpBinding;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    [Serializable]
    public class DefaultServiceConfigBuilder : IServiceBuilder
    {
        public ServiceConfig Config { get; private set; }
        public ServiceHost Build<T>() where T : IWcfServiceContract, new()
        {
            Config = LoadConfig();
            var serviceHost = CreateServiceHost<T>();
            AddServiceBehavior(serviceHost);
            return serviceHost;
        }

        /// <summary>
        ///     获取服务配置
        /// </summary>
        /// <param name="serviceHost"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private Config.Service GetServiceConfig(ServiceHost serviceHost)
        {
            var sc = Config.Services.FirstOrDefault(
                mbox => mbox.Name.ToLower() == serviceHost.Description.ServiceType.FullName.ToLower());
            return sc;
        }
        
        private Binding GetBinding(ServiceHost serviceHost)
        {
            var sc = GetServiceConfig(serviceHost);


            var binding = new NetTcpBinding();
            if (Config.Bindings.Any(m => m.Type == BindingType.NetTcpBinding &&
                                         sc.Endpoints.Any(n => n.BindingConfiguration.ToLower() == m.Name)))
            {
                var bc = Config.Bindings.FirstOrDefault(m => m.Type == BindingType.NetTcpBinding &&
                                                             sc.Endpoints.Any(
                                                                 n => n.BindingConfiguration.ToLower() == m.Name));

                
                if (bc != null && bc is Wcf.Config.NetTcpBinding)
                {
                    var c = bc as Config.NetTcpBinding;

                    binding.PortSharingEnabled = true;
                    binding.SendTimeout = TimeSpan.FromSeconds(c.SendTimeout);
                    binding.ListenBacklog = c.ListenBacklog;
                    binding.MaxBufferPoolSize = c.MaxBufferPoolSize;
                    binding.MaxBufferSize = c.MaxBufferSize;
                    binding.MaxReceivedMessageSize = c.MaxReceivedMessageSize;
                    binding.MaxConnections = c.MaxConnections;
                    binding.ReaderQuotas.MaxDepth = c.ReaderQuotasMaxDepth;
                    binding.ReaderQuotas.MaxStringContentLength = c.ReaderQuotasMaxStringContentLength;
                    binding.ReaderQuotas.MaxArrayLength = c.ReaderQuotasMaxArrayLength;
                    binding.ReaderQuotas.MaxBytesPerRead = c.ReaderQuotasMaxBytesPerRead;
                    binding.ReaderQuotas.MaxNameTableCharCount = c.ReaderQuotasMaxNameTableCharCount;
                    binding.Security.Mode = (SecurityMode)Enum.Parse(typeof(SecurityMode), c.SecurityMode);
                    binding.Security.Transport.ClientCredentialType =
                        (TcpClientCredentialType)Enum.Parse(typeof(TcpClientCredentialType),
                            c.SecurityTransportClientCredentialType);
                    binding.Security.Message.ClientCredentialType =
                        (MessageCredentialType)Enum.Parse(typeof(MessageCredentialType),
                            c.SecurityMessageClientCredentialType);
                }
            }
            else
            {
                binding.PortSharingEnabled = true;
                binding.SendTimeout = TimeSpan.FromSeconds(600);
                binding.ListenBacklog = 200;
                binding.MaxBufferPoolSize = 2147483647;
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.MaxConnections = 1000;
                binding.ReaderQuotas.MaxDepth = 64;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
                binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
                binding.Security.Mode = SecurityMode.None;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            }

            return binding;
        }

        /// <summary>
        ///     获取默认终结地址
        /// </summary>
        /// <param name="serviceHost"></param>
        /// <returns></returns>
        private string GetDefaultEndpointAddress(ServiceHost serviceHost)
        {
            var addresses = serviceHost.Description.ServiceType.GetCustomAttributes(typeof(ServerAttribute), false);

            ServerAttribute sa = null;
            foreach (var address in addresses)
            {
                if (address != null)
                {
                    sa = address as ServerAttribute;
                    break;
                }
            }

            if (sa == null)
            {
                var ct = GetServiceContract(serviceHost.Description.ServiceType);
                var adr = "/" + ct.FullName.Replace('.', '/');
                sa = new ServerAttribute(adr);
            }
            return sa.AelativeAddress;
        }

        

        /// <summary>
        ///     获取服务基地址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<Uri> GetBaseAddress<T>(ServiceConfig config, WcfType type)
            where T : IWcfServiceContract, new()
        {
            var uris = new List<Uri>();
            var service = config.Services.FirstOrDefault(m => m.Name.ToLower() == typeof(T).FullName.ToLower());
            if (service != null)
            {
                if (!service.BaseAddresses.Any())
                {
                    service.BaseAddresses.AddRange(config.BaseAddresses);
                }
                foreach (var serviceBaseAddress in service.BaseAddresses)
                {
                    if (type == WcfType.Tcp && !serviceBaseAddress.StartsWith("net.tcp:")) continue;
                    if (type == WcfType.Http && !serviceBaseAddress.StartsWith("http:")) continue;
                    uris.Add(new Uri(serviceBaseAddress));
                }
            }
            else
            {
                foreach (var serviceBaseAddress in config.BaseAddresses)
                {
                    if (type == WcfType.Tcp && !serviceBaseAddress.StartsWith("net.tcp:")) continue;
                    if (type == WcfType.Http && !serviceBaseAddress.StartsWith("http:")) continue;
                    uris.Add(new Uri(serviceBaseAddress));
                }
            }
            return uris;
        }
        private Type GetServiceContract(Type type)
        {
            Type t = null;
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface == typeof(IWcfServiceContract)) continue;
                if (@interface.GetInterfaces().Any(n => n == typeof(IWcfServiceContract)))
                {
                    t = @interface;
                    break;
                }
            }
            if (t == null)
            {
                throw new Exception("not  implement IWcfServiceContract. ");
            }
            return t;
        }

        private Type GetServiceContract<T>()
        {
            Type t = null;
            var type = typeof(T);
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface == typeof(IWcfServiceContract)) continue;
                if (@interface.GetInterfaces().Any(n => n == typeof(IWcfServiceContract)))
                {
                    t = @interface;
                    break;
                }
            }
            if (t == null)
            {
                throw new Exception("not  implement IWcfServiceContract. ");
            }
            return t;
        }

        /// <summary>
        ///     创建服务主机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        private ServiceHost CreateServiceHost<T>()
            where T : IWcfServiceContract, new()
        {
            var bas = GetBaseAddress<T>(Config, WcfType.Tcp);

            var serviceHost = new ServiceHost(typeof(T), bas.ToArray());

            var sc = GetServiceConfig(serviceHost);
            if (sc != null)
            {
                serviceHost.Description.Endpoints.Clear();
                serviceHost.Description.Name = typeof(T).FullName;
                foreach (var scEndpoint in sc.Endpoints)
                {
                    var us = GetDefaultEndpointAddress(serviceHost);
                    var binding = GetBinding(serviceHost);

                    serviceHost.AddServiceEndpoint(GetServiceContract<T>(), binding, us);
                }
                // 添加基地址
            }
            else
            {
                var binding = GetBinding(serviceHost);

                serviceHost.AddServiceEndpoint(GetServiceContract<T>(), binding,
                    GetDefaultEndpointAddress(serviceHost));
            }

            return serviceHost;
        }


        public void AddServiceBehavior(ServiceHost serviceHost)
        {
            var sc = GetServiceConfig(serviceHost);

            // 如果配置了行为，并且存在行为配置
            if (sc != null && !string.IsNullOrEmpty(sc.BehaviorConfiguration) &&
                Config.Behaviors.All(m => m.Name.ToLower() == sc.BehaviorConfiguration.ToLower()))
            {
                // 加载配置行为
                var behavior =
                    Config.Behaviors.FirstOrDefault(m => m.Name.ToLower() == sc.BehaviorConfiguration.ToLower() &&
                                                         m.Type == 0);

                
                var sb = behavior as ServiceBehavior;
                if (sb != null)
                {
                    var serviceDebugBehavior = new ServiceDebugBehavior
                    {
                        IncludeExceptionDetailInFaults = sb.IncludeExceptionDetailInFaults
                    };
                    var serviceThrottlingBehavior = new ServiceThrottlingBehavior
                    {
                        MaxConcurrentCalls = sb.MaxConcurrentCalls,
                        MaxConcurrentSessions = sb.MaxConcurrentSessions,
                        MaxConcurrentInstances = sb.MaxConcurrentInstances
                    };
                    var serviceMetadata = new ServiceMetadataBehavior
                    {
                        HttpGetEnabled = sb.HttpGetEnabled,
                        HttpsGetEnabled = sb.HttpsGetEnabled
                        
                    };
                    var baseAddress = Config.BaseAddresses.FirstOrDefault(m => m.ToString().StartsWith("http:"));
                    if (sb.HttpGetEnabled && baseAddress != null)
                    {
                        serviceMetadata.HttpGetUrl = new Uri(baseAddress +
                                                             GetDefaultEndpointAddress(serviceHost));
                    }
                    if (serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>() != null)
                        serviceHost.Description.Behaviors.Remove<ServiceDebugBehavior>();
                    if (serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>() != null)
                        serviceHost.Description.Behaviors.Remove<ServiceThrottlingBehavior>();
                    if (serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>() != null)
                        serviceHost.Description.Behaviors.Remove<ServiceMetadataBehavior>();
                    serviceHost.Description.Behaviors.Add(serviceDebugBehavior);
                    serviceHost.Description.Behaviors.Add(serviceThrottlingBehavior);
                    serviceHost.Description.Behaviors.Add(serviceMetadata);
                }
            }
            else
            {
                if (serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>() != null)
                    serviceHost.Description.Behaviors.Remove<ServiceDebugBehavior>();
                if (serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>() != null)
                    serviceHost.Description.Behaviors.Remove<ServiceThrottlingBehavior>();
                if (serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>() != null)
                    serviceHost.Description.Behaviors.Remove<ServiceMetadataBehavior>();

                // 使用默认行为
                var serviceDebugBehavior = new ServiceDebugBehavior
                {
                    IncludeExceptionDetailInFaults = true
                };
                var serviceThrottlingBehavior = new ServiceThrottlingBehavior
                {
                    MaxConcurrentCalls = 1000,
                    MaxConcurrentSessions = 1000,
                    MaxConcurrentInstances = 1000
                };
                var baseAddress = Config.BaseAddresses.FirstOrDefault(m => m.ToString().StartsWith("http:"));
                var serviceMetadata = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpsGetEnabled = false
                };

                if (baseAddress != null)
                {
                    serviceMetadata.HttpGetUrl = new Uri(baseAddress +
                                                         GetDefaultEndpointAddress(serviceHost));
                }
                serviceHost.Description.Behaviors.Add(serviceDebugBehavior);
                serviceHost.Description.Behaviors.Add(serviceThrottlingBehavior);
                serviceHost.Description.Behaviors.Add(serviceMetadata);
            }
        }

        /// <summary>
        ///     加载配置
        /// </summary>
        /// <returns></returns>
        private ServiceConfig LoadConfig()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = Path.Combine(filePath, "service.json");
            if (!File.Exists(fileName))
            {
                var json = JsonConvert.SerializeObject(new ServiceConfig());
                File.WriteAllText(fileName, json);
            }
            var jsonText = File.ReadAllText(fileName);
            var config = JsonConvert.DeserializeObject<ServiceConfig>(jsonText);
            return config;
        }
    }

    public enum WcfType
    {
        Http,
        Tcp
    }
}