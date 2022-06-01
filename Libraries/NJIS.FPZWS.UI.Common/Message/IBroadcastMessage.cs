using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.UI.Common.Message
{
    public interface IBroadcastMessage
    {
        /// <summary>
        /// 发送广播消息
        /// </summary>
        /// <typeparam name="TMessage">消息数据类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="message">消息</param>
        void SendBroadcastMessage<TMessage>(string messageTab, TMessage message);

        /// <summary>
        /// 注册广播消息
        /// </summary>
        /// <typeparam name="TMessage">消息数据类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">接收者</param>
        /// <param name="action">接收后执行的委托</param>
        /// <param name="eExecutionMode">执行方式</param>
        void RegisterBroadcastMessage<TMessage>(string messageTab, object recipient, Action<TMessage> action,
            EExecutionMode eExecutionMode);

        /// <summary>
        /// 注册广播消息
        /// </summary>
        /// <typeparam name="TMessage">消息数据类型</typeparam>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">接收者</param>
        /// <param name="actionWithOperated">接收后执行的委托</param>
        /// <param name="operatedArgs">委托执行所需的固定参数</param>
        /// <param name="eExecutionMode">执行方式</param>
        void RegisterBroadcastMessage<TMessage>(string messageTab, object recipient,
            Action<TMessage, object[]> actionWithOperated, object[] operatedArgs, EExecutionMode eExecutionMode);


    }
}
