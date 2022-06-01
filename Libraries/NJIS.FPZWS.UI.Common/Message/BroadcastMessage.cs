using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.UI.Common.Message
{
    /// <summary>
    /// 进程内消息广播
    /// </summary>
    public class BroadcastMessage
    {
        private static IBroadcastMessage broadcastMessage = MessageManager.GetInstance();

        /// <summary>
        /// 进程内消息广播
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageTab"></param>
        /// <param name="message"></param>
        public static void Send<TMessage>(string messageTab, TMessage message)
        {
            broadcastMessage.SendBroadcastMessage(messageTab, message);
        }

        /// <summary>
        /// 注册广播消息
        /// </summary>
        /// <typeparam name="TMessage">消息数据类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">接收者,默认传递this即可，也可以指定固定对象</param>
        /// <param name="action">接收后执行的委托</param>
        /// <param name="eExecutionMode">执行方式</param>
        public static void Register<TMessage>(string messageTab, object recipient, Action<TMessage> action,
            EExecutionMode eExecutionMode = EExecutionMode.Asynchronization)
        {
            broadcastMessage.RegisterBroadcastMessage(messageTab, recipient, action, eExecutionMode);
        }

        /// <summary>
        /// 注册广播消息
        /// </summary>
        /// <typeparam name="TMessage">消息数据类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">接收者,默认传递this即可，也可以指定固定对象</param>
        /// <param name="actionWithOperated">接收后执行的委托</param>
        /// <param name="operatedArgs">委托执行所需的固定参数</param>
        /// <param name="eExecutionMode">执行方式</param>
        public static void Register<TMessage>(string messageTab, object recipient,
            Action<TMessage, object[]> actionWithOperated, object[] operatedArgs, EExecutionMode eExecutionMode = EExecutionMode.Asynchronization)
        {
            broadcastMessage.RegisterBroadcastMessage(messageTab, recipient, actionWithOperated, operatedArgs, eExecutionMode);
        }
    }
}
