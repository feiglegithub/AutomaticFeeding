// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：MessageDispatcher.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:37
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NJIS.FPZWS.Common.Bus
{
    /// <summary>
    ///    消息分配器
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();


        #region Private Methods

        /// <summary>
        ///     注册将指定的处理程序类型到消息分配器。
        /// </summary>
        /// <param name="messageDispatcher">消息调度器.</param>
        /// <param name="handlerType">处理程序类型.</param>
        private static void RegisterType(IMessageDispatcher messageDispatcher, Type handlerType)
        {
            var methodInfo = messageDispatcher.GetType()
                .GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo != null)
            {
                var handlerIntfTypeQuery = from p in handlerType.GetInterfaces()
                                           where p.IsGenericType &&
                                                 p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))
                                           select p;
                if (handlerIntfTypeQuery != null)
                {
                    foreach (var handlerIntfType in handlerIntfTypeQuery)
                    {
                        var handlerInstance = Activator.CreateInstance(handlerType);
                        var messageType = handlerIntfType.GetGenericArguments().First();
                        MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(messageType);
                        genericMethodInfo.Invoke(messageDispatcher, new[] { handlerInstance });
                    }
                }
            }
        }

        /// <summary>
        ///     注册将指定程序集中所有的处理程序类型到消息分配器
        /// </summary>
        /// <param name="messageDispatcher">消息调度器.</param>
        /// <param name="assembly">程序集.</param>
        private static void RegisterAssembly(IMessageDispatcher messageDispatcher, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                var intfs = type.GetInterfaces();
                if (intfs.Any(p =>
                        p.IsGenericType &&
                        p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))) &&
                    intfs.Any(p =>
                        p.IsDefined(typeof(RegisterDispatchAttribute), true)))
                {
                    RegisterType(messageDispatcher, type);
                }
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnDispatching(MessageDispatchEventArgs e)
        {
            var temp = Dispatching;
            if (temp != null)
                temp(this, e);
        }

        protected virtual void OnDispatchFailed(MessageDispatchEventArgs e)
        {
            var temp = DispatchFailed;
            if (temp != null)
                temp(this, e);
        }
        protected virtual void OnDispatched(MessageDispatchEventArgs e)
        {
            var temp = Dispatched;
            if (temp != null)
                temp(this, e);
        }

        #endregion

        #region IMessageDispatcher Members

        /// <summary>
        ///     清除注册消息处理程序
        /// </summary>
        public virtual void Clear()
        {
            _handlers.Clear();
        }

        /// <summary>
        ///     分派消息
        /// </summary>
        /// <param name="message">消息类型.</param>
        public virtual void DispatchMessage<T>(T message)
        {
            var messageType = typeof(T);
            if (_handlers.ContainsKey(messageType))
            {
                var messageHandlers = _handlers[messageType];
                foreach (var messageHandler in messageHandlers)
                {
                    var dynMessageHandler = (IHandler<T>)messageHandler;
                    var evtArgs = new MessageDispatchEventArgs(message, messageHandler.GetType(), messageHandler);
                    OnDispatching(evtArgs);
                    try
                    {
                        dynMessageHandler.Handle(message);
                        OnDispatched(evtArgs);
                    }
                    catch
                    {
                        OnDispatchFailed(evtArgs);
                    }
                }
            }
        }

        /// <summary>
        ///     注册消息到消息处理器
        /// </summary>
        /// <typeparam name="T">消息类型.</typeparam>
        /// <param name="handler">注册的消息处理程序.</param>
        public virtual void Register<T>(IHandler<T> handler)
        {
            var keyType = typeof(T);

            if (_handlers.ContainsKey(keyType))
            {
                var registeredHandlers = _handlers[keyType];
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                        registeredHandlers.Add(handler);
                }
                else
                {
                    registeredHandlers = new List<object>();
                    registeredHandlers.Add(handler);
                    _handlers.Add(keyType, registeredHandlers);
                }
            }
            else
            {
                var registeredHandlers = new List<object>();
                registeredHandlers.Add(handler);
                _handlers.Add(keyType, registeredHandlers);
            }
        }

        /// <summary>
        ///     从消息处理器中 解绑消息处理程序
        /// </summary>
        /// <typeparam name="T">消息类型.</typeparam>
        /// <param name="handler">注册的消息处理程序.</param>
        public virtual void UnRegister<T>(IHandler<T> handler)
        {
            var keyType = typeof(T);
            if (_handlers.ContainsKey(keyType) &&
                _handlers[keyType] != null &&
                _handlers[keyType].Count > 0 &&
                _handlers[keyType].Contains(handler))
            {
                _handlers[keyType].Remove(handler);
            }
        }

        /// <summary>
        ///     发送消息前
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatching;

        /// <summary>
        ///    消息发送失败
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        ///     消息发送成果
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatched;

        #endregion
    }
}