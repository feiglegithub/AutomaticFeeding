// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IMessageDispatcher.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:36
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;

namespace NJIS.FPZWS.Common.Bus
{
    /// <summary>
    ///     消息处理器
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        ///     清除注册消息处理程序
        /// </summary>
        void Clear();

        /// <summary>
        ///     分派消息
        /// </summary>
        /// <param name="message">消息类型.</param>
        void DispatchMessage<T>(T message);

        /// <summary>
        ///     注册消息到消息处理器
        /// </summary>
        /// <typeparam name="T">消息类型.</typeparam>
        /// <param name="handler">注册的消息处理程序.</param>
        void Register<T>(IHandler<T> handler);

        /// <summary>
        ///     从消息处理器中 解绑消息处理程序
        /// </summary>
        /// <typeparam name="T">消息类型.</typeparam>
        /// <param name="handler">注册的消息处理程序.</param>
        void UnRegister<T>(IHandler<T> handler);

        /// <summary>
        ///     发送消息前
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatching;

        /// <summary>
        ///     消息发送失败
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        ///     消息发送成果
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatched;
    }
}