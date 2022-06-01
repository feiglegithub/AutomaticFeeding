// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Client
//  文 件 名：WcfClient.cs
//  创建时间：2017-12-29 8:09
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NJIS.FPZWS.Wcf.Config;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{

    public class WcfClient : WcfClient<DefaultClientBuilder>
    {

    }

    /// <summary>
    ///     连接池客户端封装
    /// </summary>
    public class WcfClient<T> where T : IClientBuilder,new()
    {
        static WcfClient()
        {
            Builder = new T();
        }
        public static T Builder { get; set; }

        /// <summary>
        ///     缓存的代理拦截器
        /// </summary>
        private static volatile IDictionary<string, object> _proxyDic = new Dictionary<string, object>();

        private static volatile object _lockproxyDic = new object();

        /// <summary>
        ///     获取客户端代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TT GetProxy<TT>()
        {
            return SetProxy<TT>();
        }

        /// <summary>
        ///     获取客户端代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static TT SetProxy<TT>()
        {
            var contract = typeof(TT).FullName;
            var proxy = default(TT);
            if (_proxyDic.ContainsKey(contract))
            {
                proxy = (TT)_proxyDic[contract];
            }
            else
            {
                lock (_lockproxyDic)
                {
                    if (!_proxyDic.ContainsKey(contract))
                    {
                        var binding = Builder.GetBinding<TT>();
                        var epAddress = Builder.GetAddress<TT>();
                        proxy = (TT)new WcfStandardProxy<TT>(binding, epAddress, false).GetTransparentProxy();

                        _proxyDic.Add(contract, proxy);
                    }
                    else
                    {
                        proxy = (TT)_proxyDic[contract];
                    }
                }
            }

            return proxy;
        }
        
    }
}