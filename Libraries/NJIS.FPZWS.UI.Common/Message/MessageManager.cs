//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：MessageManager.cs
//   创建时间：2018-10-18 7:54
//   作    者：
//   说    明：
//   修改时间：2018-10-18 7:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.UI.Common.Message
{
    /// <summary>
    ///     消息管理类
    /// </summary>
    internal class MessageManager : IManager, IBroadcastMessage
    {
        private static MessageManager _messageManager;
        private static readonly object CreateLock = new object();

        //注册、注销与发送不能同时进行
        private static readonly object ObjLock = new object();

        public void Register<TMessage>(string messageTab, object recipient, Action<TMessage> action,
            EExecutionMode eExecutionMode, bool allowReceiveBroadcast = false)
        {
            lock (ObjLock)
            {
                MessageInfo<TMessage>.AddMessage(messageTab, recipient, action, eExecutionMode, allowReceiveBroadcast);
            }
        }

        public void Register<TMessage>(string messageTab, object recipient, Action<object, TMessage> action, EExecutionMode eExecutionMode)
        {
            lock (ObjLock)
            {
                MessageInfo<TMessage>.AddMessage(messageTab, recipient, action, eExecutionMode);
            }
        }

        public void Register<TMessage>(string messageTab, object recipient,
            Action<TMessage, object[]> actionWithOperated, object[] operatedArgs, EExecutionMode eExecutionMode, bool allowReceiveBroadcast = false)
        {
            lock (ObjLock)
            {
                MessageInfo<TMessage>.AddMessage(messageTab, recipient, actionWithOperated, operatedArgs,
                    eExecutionMode, allowReceiveBroadcast);
            }
        }

        public void Send<TMessage>(string messageTab, TMessage message)
        {
            List<MessageInfo<TMessage>> lst = null;
            lock (ObjLock)
            {
                lst = MessageInfo<TMessage>.GetMessageInfos(messageTab);
            }

            lst.ForEach(item => { item.Execute(message); });
        }

        public void Send<TMessage>(object sender, string messageTab, TMessage message)
        {
            List<MessageInfo<TMessage>> lst = null;
            lock (ObjLock)
            {
                lst = MessageInfo<TMessage>.GetMessageInfos(sender, messageTab);
            }

            lst.ForEach(item =>
            {
                item.Execute(sender, message);
            });
        }

        public void Send<TMessage>(object sender, string messageTab, object recipient, TMessage message)
        {
            List<MessageInfo<TMessage>> lst = null;
            lock (ObjLock)
            {
                lst = MessageInfo<TMessage>.GetMessageInfos(sender, recipient, messageTab);
            }

            lst.ForEach(item =>
            {
                item.Execute(sender, message);
            });
        }

        public void Send<TMessage>(string messageTab, object recipient, TMessage message)
        {
            MessageInfo<TMessage> messageInfo = null;
            lock (ObjLock)
            {
                messageInfo = MessageInfo<TMessage>.GetMessageInfos(messageTab, recipient);
            }

            if (messageInfo != null)
            {
                messageInfo.Execute(message);
            }
        }

        public void UnRegister<TMessage>(string messageTab, object recipient)
        {
            //throw new NotImplementedException();
            lock (ObjLock)
            {
                MessageInfo<TMessage>.RemoveMessage(messageTab, recipient);
            }
        }

        public void BindingPresenter(object view, object presenter)
        {
            lock (ObjLock)
            {
                ViewPresenterManager.BindingPresenter(view, presenter);
            }
        }

        public void UnBindPresenter(object view)
        {
            lock (ObjLock)
            {
                ViewPresenterManager.UnBindPresenter(view);
            }
        }

        public static MessageManager GetInstance()
        {
            if (_messageManager == null)
            {
                lock (CreateLock)
                {
                    if (_messageManager == null)
                    {
                        _messageManager = new MessageManager();
                    }
                }
            }

            return _messageManager;
        }



        public void SendBroadcastMessage<TMessage>(string messageTab, TMessage message)
        {
            List<MessageInfo<TMessage>> lst = null;
            lock (ObjLock)
            {
                lst = MessageInfo<TMessage>.GetBroadcastMessageInfos(messageTab);
            }

            lst.ForEach(item => { item.Execute(message); });
        }

        public void RegisterBroadcastMessage<TMessage>(string messageTab, object recipient, Action<TMessage> action,
            EExecutionMode eExecutionMode)
        {
            Register<TMessage>(messageTab, recipient, action, eExecutionMode, true);
        }

        public void RegisterBroadcastMessage<TMessage>(string messageTab, object recipient, Action<TMessage, object[]> actionWithOperated,
            object[] operatedArgs, EExecutionMode eExecutionMode)
        {
            Register<TMessage>(messageTab, recipient, actionWithOperated, operatedArgs, eExecutionMode, true);
        }
    }
}