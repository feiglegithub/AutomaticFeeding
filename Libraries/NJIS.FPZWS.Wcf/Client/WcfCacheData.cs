// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Client
//  文 件 名：WcfCacheData.cs
//  创建时间：2017-12-29 8:13
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using Newtonsoft.Json;
using NJIS.FPZWS.Wcf.Client.SerializeBehavior;
using NJIS.FPZWS.Wcf.Config;
using NJIS.FPZWS.Wcf.MessageHeader;
using NetTcpBinding = System.ServiceModel.NetTcpBinding;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    internal class WcfCacheData
    {
        /// <summary>
        ///     通道工厂缓存
        /// </summary>
        private static readonly Hashtable Factorycache = new Hashtable();
        
        /// <summary>
        ///     获取缓存的通道工厂
        /// </summary>
        /// <typeparam name="T">契约</typeparam>
        /// <param name="binging">绑定信息</param>
        /// <param name="address">终结点地址</param>
        /// <param name="maxItemsInObjectGraph">序列化大小</param>
        /// <param name="enableBinaryFormatterBehavior"></param>
        /// <returns>通道工厂</returns>
        public static ChannelFactory<T> GetFactory<T>(NetTcpBinding binging, EndpointAddress address,
            int? maxItemsInObjectGraph, bool enableBinaryFormatterBehavior)
        {
            var contract = typeof(T).FullName;
            ChannelFactory<T> client = null;
            if (Factorycache.ContainsKey(contract))
            {
                client = (ChannelFactory<T>)Factorycache[contract];
                if (client.State != CommunicationState.Opened)
                {
                    client = null;
                }
            }
            if (client == null)
            {
                client = new ChannelFactory<T>(binging, address);
                //增加头信息行为
                var msgBehavior = new ClientMessageInspector();
                client.Endpoint.Behaviors.Add(msgBehavior);
                //如果启用自定义二进制序列化器
                if (enableBinaryFormatterBehavior)
                {
                    if (client.Endpoint.Behaviors.Find<BinaryFormatterBehavior>() == null)
                    {
                        var serializeBehavior = new BinaryFormatterBehavior();
                        client.Endpoint.Behaviors.Add(serializeBehavior);
                    }
                }
                //如果有MaxItemsInObjectGraph配置指定配置此行为
                if (maxItemsInObjectGraph != null)
                {
                    foreach (var op in client.Endpoint.Contract.Operations)
                    {
                        var dataContractBehavior =
                            op.Behaviors.Find<DataContractSerializerOperationBehavior>();
                        if (dataContractBehavior != null)
                        {
                            dataContractBehavior.MaxItemsInObjectGraph = (int)maxItemsInObjectGraph;
                        }
                    }
                }
                Factorycache[contract] = client;
            }
            return client;
        }

        /// <summary>
        ///     获取缓存的通道工厂
        /// </summary>
        /// <typeparam name="T">契约</typeparam>
        /// <returns>通道工厂</returns>
        public static ChannelFactory<T> GetFactory<T>(NetTcpBinding binding, EndpointAddress address)
        {
            var contract = typeof(T).FullName;
            ChannelFactory<T> client = null;
            if (Factorycache.ContainsKey(contract))
            {
                client = (ChannelFactory<T>)Factorycache[contract];
                if (client.State != CommunicationState.Opened)
                {
                    client = null;
                }
            }
            if (client == null)
            {
                client = new ChannelFactory<T>(binding, address);

                //增加头信息行为
                var msgBehavior = new ClientMessageInspector();
                client.Endpoint.Behaviors.Add(msgBehavior);


                Factorycache[contract] = client;
            }
            return client;
        }


    }
}