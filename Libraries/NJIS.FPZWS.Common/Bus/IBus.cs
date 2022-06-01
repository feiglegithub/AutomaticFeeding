// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IBus.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.Common.Bus
{
    /// <summary>
    ///     消息总线.
    /// </summary>
    public interface IBus : IDisposable
    {
        /// <summary>
        ///     发布消息到消息总线
        /// </summary>
        /// <typeparam name="TMessage">要发布的消息类型.</typeparam>
        /// <param name="message">要发布的消息.</param>
        void Publish<TMessage>(TMessage message);

        /// <summary>
        ///     发布一组消息到消息总线
        /// </summary>
        /// <typeparam name="TMessage">要发布的消息类型.</typeparam>
        /// <param name="messages">要发布的消息.</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages);

        /// <summary>
        ///     清除已经发布的，待提交的消息
        /// </summary>
        void Clear();
    }
}