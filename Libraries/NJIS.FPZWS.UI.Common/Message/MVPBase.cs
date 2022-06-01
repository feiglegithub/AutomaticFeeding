//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：MVPBase.cs
//   创建时间：2018-11-19 16:06
//   作    者：
//   说    明：
//   修改时间：2018-11-19 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.UI.Common.Message
{
    public class MVPBase
    {
        public IManager Manager = MessageManager.GetInstance();
        /// <summary>
        ///     注册消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="actionWithoutSender">收到消息时需要执行的委托</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        /// <param name="allowReceiveBroadcast">是否允许接收广播消息</param>
        public void Register<TMessage>(string messageTab, Action<TMessage> actionWithoutSender,
            EExecutionMode eExecutionMode = EExecutionMode.Asynchronization, bool allowReceiveBroadcast=false)
        {
            Manager.Register(messageTab, this, actionWithoutSender, eExecutionMode, allowReceiveBroadcast);
        }

        /// <summary>
        ///     注册消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="action">收到消息时需要执行的委托</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        public void Register<TMessage>(string messageTab, Action<object,TMessage> action,
            EExecutionMode eExecutionMode = EExecutionMode.Asynchronization)
        {
            Manager.Register(messageTab, this, action, eExecutionMode);
        }

        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="actionWithOperated">收到消息时需要执行的委托</param>
        /// <param name="operatedArgs">委托执行时需要操作的对象（一些指定的参数）</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        /// <param name="allowReceiveBroadcast">是否允许接收广播消息</param>
        private void Register<TMessage>(string messageTab, Action<TMessage, object[]> actionWithOperated,
            object[] operatedArgs, EExecutionMode eExecutionMode = EExecutionMode.Asynchronization, bool allowReceiveBroadcast=false)
        {
            Manager.Register(messageTab, this, actionWithOperated, operatedArgs, eExecutionMode, allowReceiveBroadcast);
        }

        /// <summary>
        ///     向向绑定对象发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageTab"></param>
        /// <param name="message"></param>
        public void Send<TMessage>(string messageTab, TMessage message)
        {
            Manager.Send(this, messageTab, message);
        }

    }
}