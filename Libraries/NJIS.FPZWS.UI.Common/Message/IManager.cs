//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：IManager.cs
//   创建时间：2018-10-18 7:54
//   作    者：
//   说    明：
//   修改时间：2018-10-18 7:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.UI.Common.Message
{
    public interface IManager
    {
        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="recipient">消息接收者</param>
        /// <param name="actionWithoutSender">收到消息时需要执行的委托</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        /// <param name="allowReceiveBroadcast">是否接收广播消息</param>
        void Register<TMessage>(string messageTab, object recipient, Action<TMessage> actionWithoutSender,
            EExecutionMode eExecutionMode,bool allowReceiveBroadcast=false);

        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="recipient">消息接收者</param>
        /// <param name="action">收到消息时需要执行的委托</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        void Register<TMessage>(string messageTab, object recipient, Action<object,TMessage> action,
            EExecutionMode eExecutionMode);

        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="recipient">消息接收者</param>
        /// <param name="actionWithOperated">收到消息时需要执行的委托</param>
        /// <param name="operatedArgs">委托执行时需要操作的对象（一些指定的参数）</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        /// <param name="allowReceiveBroadcast">是否接收广播消息</param>
        void Register<TMessage>(string messageTab, object recipient, Action<TMessage, object[]> actionWithOperated,
            object[] operatedArgs, EExecutionMode eExecutionMode,bool allowReceiveBroadcast=false);


        /// <summary>
        ///     注销消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">订阅的消息标识</param>
        /// <param name="recipient">消息接收者</param>
        void UnRegister<TMessage>(string messageTab, object recipient);

        /// <summary>
        ///     发送消息 -- Old API 不建议使用
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="message">内容</param>
        [Obsolete("此方法已被弃用")]
        void Send<TMessage>(string messageTab, TMessage message);

        /// <summary>
        ///     发送消息 -- Old API 不建议使用
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">指定的消息订阅者（接收者）</param>
        /// <param name="message">消息内容</param>
        void Send<TMessage>(string messageTab, object recipient, TMessage message);

        /// <summary>
        ///     发送消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="sender">消息发送者</param>
        /// <param name="messageTab">消息标签</param>
        /// <param name="message">消息内容</param>
        void Send<TMessage>(object sender, string messageTab, TMessage message);

        /// <summary>
        ///     发送消息(Presenter使用，View无法使用此API)
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="sender">消息发送者</param>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">消息接收者</param>
        /// <param name="message">消息内容</param>
        void Send<TMessage>(object sender, string messageTab,object recipient, TMessage message);

        /// <summary>
        ///     view-presenter绑定
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="presenter">presenter</param>
        void BindingPresenter(object view, object presenter);

        /// <summary>
        ///     view-presenter解绑
        /// </summary>
        /// <param name="view">view</param>
        void UnBindPresenter(object view);
    }
}