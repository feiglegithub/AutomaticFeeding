// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：WcfStandardProxy.cs
//  创建时间：2017-12-29 14:43
//  作    者：
//  说    明：
//  修改时间：2018-01-17 9:01
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Threading;
using NJIS.FPZWS.Wcf.MessageHeader;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    /// <summary>
    ///     wcf代理拦截，自动处理wcf连接池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WcfStandardProxy<T> : RealProxy
    {
        /// <summary>
        ///     终结点
        /// </summary>
        private readonly EndpointAddress _address;

        /// <summary>
        ///     bing模式
        /// </summary>
        private readonly NetTcpBinding _binging;

        /// <summary>
        ///     是否启用连接池，默认不启用
        /// </summary>
        private readonly bool _isUseWcfPool;

        /// <summary>
        ///     Wcf连接池最大值，默认为100
        /// </summary>
        private readonly int _wcfMaxPoolSize = 100;

        /// <summary>
        ///     Wcf获取连接过期时间(默认一分钟)
        /// </summary>
        private readonly long _wcfOutTime = 60;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="address"></param>
        /// <param name="isUseWcfPool"></param>
        /// <param name="binding"></param>
        public WcfStandardProxy(NetTcpBinding binding, EndpointAddress address, bool isUseWcfPool)
            : base(typeof(T))
        {
            _isUseWcfPool = isUseWcfPool;
            ServerName = typeof(T).FullName;

            _binging = binding;
            _address = address;

            //初始话连接池
            WcfPoolCache.Init(_isUseWcfPool, _wcfMaxPoolSize, _wcfOutTime, 60 * 1000 * 10000, ServerName, 30);
        }

        /// <summary>
        ///     服务器节点名称
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        ///     拦截器处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodReturnMessage methodReturn;
            var methodCall = (IMethodCallMessage)msg;

            var contract = typeof(T).FullName;

            //此处使用wcf连接池技术，获取当前wcf连接池
            var pool = _isUseWcfPool ? WcfPoolCache.GetWcfPool(ServerName) : null;

            //获取的池子索引
            int? index = null;
            var channel = default(T);
            OperationContextScope scope = null;
            if (!_isUseWcfPool) //不启用连接池
            {
                var factory = WcfCacheData.GetFactory<T>(_binging, _address);
                channel = factory.CreateChannel();
            }
            else
            {
                #region 传统模式

                //是否超时
                var isouttime = false;
                //超时计时器
                var sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    var isReap = true;
                    //先判断池子中是否有此空闲连接
                    if (pool.GetFreePoolNums(contract) > 0)
                    {
                        isReap = false;
                        var commobj = pool.GetChannel<T>();
                        if (commobj != null)
                        {
                            index = commobj.Index;
                            channel = (T)commobj.CommucationObject;
                            //Console.WriteLine(contract + "获取空闲索引:" + index);
                        }
                    }

                    //如果没有空闲连接判断是否池子是否已满，未满，则创建新连接并装入连接池
                    if (channel == null && !pool.IsPoolFull)
                    {
                        //创建新连接
                        var factory = WcfCacheData.GetFactory<T>(_binging, _address);

                        //装入连接池
                        var flag = pool.AddPool(factory, out channel, out index, isReap);
                    }

                    //如果当前契约无空闲连接，并且队列已满，并且非当前契约有空闲，则踢掉一个非当前契约
                    if (channel == null && pool.IsPoolFull && pool.GetFreePoolNums(contract) == 0 &&
                        pool.GetUsedPoolNums(contract) != _wcfMaxPoolSize)
                    {
                        //创建新连接
                        var factory = WcfCacheData.GetFactory<T>(_binging, _address);
                        pool.RemovePoolOneNotAt(factory, out channel, out index);
                    }

                    if (channel != null)
                        break;

                    //如果还未获取连接判断是否超时，如果超时抛异常
                    if (sw.Elapsed >= new TimeSpan(_wcfOutTime * 1000 * 10000))
                    {
                        isouttime = true;
                        break;
                    }
                    Thread.Sleep(100);
                }
                sw.Stop();

                if (isouttime)
                {
                    throw new Exception("获取连接池中的连接超时，请配置WCF客户端常量配置文件中的WcfOutTime属性，Server name=\"" + ServerName + "\"");
                }

                #endregion
            }

            #region 传递上下文

            var wcfappname = typeof(T).FullName;

            if (wcfappname != null)
                HeaderOperater.SetClientWcfAppNameHeader(wcfappname);

            #endregion

            try
            {
                var copiedArgs = new object[methodCall.Args.Length];
                methodCall.Args.CopyTo(copiedArgs, 0);
                var returnValue = methodCall.MethodBase.Invoke(channel, copiedArgs);
                methodReturn = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length,
                    methodCall.LogicalCallContext, methodCall);

                //如果启用连接池，使用完后把连接回归连接池
                if (_isUseWcfPool)
                {
                    if (index != null)
                        pool.ReturnPool<T>((int)index);
                }
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex.InnerException != null)
                    exception = ex.InnerException;
                methodReturn = new ReturnMessage(exception, methodCall);

                //如果启用连接池，出错则关闭连接，并删除连接池中的连接
                if (_isUseWcfPool)
                {
                    if (index != null)
                        pool.RemovePoolAt<T>((int)index);
                }
            }
            finally
            {
                if (!_isUseWcfPool) //不启用连接池
                {
                    if (scope != null)
                        scope.Dispose();
                    var disposable = channel as IDisposable;
                    if (disposable != null)
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                //清除wcf应用程序名上下文
                HeaderOperater.ClearClientWcfAppNameHeader();
            }

            return methodReturn;
        }
    }
}