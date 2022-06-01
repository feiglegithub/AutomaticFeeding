//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：MessageInfo.cs
//   创建时间：2018-10-18 7:54
//   作    者：
//   说    明：
//   修改时间：2018-10-18 7:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NJIS.FPZWS.UI.Common.Message
{
    internal class MessageInfo<TMessage>
    {
        /// <summary>
        ///     后续更新撤销list，将list换成单体 -- 目的是确保一个消息标签且同一种参数类型，在同一个接收对象里确保只能注册一个方法
        /// </summary>
        private static readonly Dictionary<string, List<MessageInfo<TMessage>>> MsgDictionary =
            new Dictionary<string, List<MessageInfo<TMessage>>>();

        #region 成员

        public EExecutionMode EExecutionMode { get; set; }

        public Action<TMessage> ActionWithoutSender { get; set; }

        public Action<object,TMessage> Action { get; set; }

        public WeakReference RecipientWeakReference { get; set; }

        public Action<TMessage, object[]> ActionWithOperated { get; set; }
        public WeakReference[] WeakReferences { get; set; }

        public object[] OperatedArgs
        {
            get
            {
                if (WeakReferences == null)
                    return new object[0];
                var length = WeakReferences.Length;
                var objs = new object[length];
                for (var i = 0; i < length; i++)
                {
                    objs[i] = WeakReferences[i].IsAlive ? WeakReferences[i].Target : null;
                }

                return objs;
            }
            set
            {
                var length = value.Length;
                WeakReferences = new WeakReference[length];
                for (var i = 0; i < length; i++)
                {
                    WeakReferences[i] = new WeakReference(value[i]);
                }
            }
        }

        /// <summary>
        ///     接收信息者
        /// </summary>
        public object Recipient => RecipientWeakReference.IsAlive ? RecipientWeakReference.Target : null;

        /// <summary>
        ///     执行委托的参数
        /// </summary>
        public TMessage Message { get; set; }


        /// <summary>
        /// 是否接收广播信息
        /// </summary>
        public bool AllowReceiveBroadcast { get; set; }

        public void Execute(object sender, TMessage message)
        {
            if (!RecipientWeakReference.IsAlive)
            {
                return;
            }
            if (Action != null)
            {
                if (Action.Target is Control)
                {
                    var actiontmp = Action.Target as Control;
                    var hadWinHandle = actiontmp.IsHandleCreated;
                    if (!hadWinHandle) return;
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        actiontmp.Invoke(Action, message);
                    }
                    else
                    {
                        actiontmp.BeginInvoke(Action, message);
                    }

                    #region Old Code
                    //if (actiontmp.InvokeRequired)
                    //{
                    //Message = message;
                    //if (EExecutionMode == EExecutionMode.Synchronization)
                    //{
                    //    actiontmp.Invoke(Action, message);
                    //}
                    //else
                    //{
                    //    actiontmp.BeginInvoke(Action, message);
                    //}
                    //}
                    //else
                    //{
                    //    Message = message;
                    //    if (EExecutionMode == EExecutionMode.Synchronization)
                    //    {
                    //        Action(sender, message);
                    //    }
                    //    else
                    //    {
                    //        Action.BeginInvoke(sender, message, null, null);
                    //    }
                    //}


                    #endregion

                }
                else
                {
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        Action(sender, message);
                    }
                    else
                    {
                        Action.BeginInvoke(sender, message, null, null);
                    }
                }
            }

            Execute(message);

        }

        /// <summary>
        ///     执行委托
        /// </summary>
        public void Execute(TMessage message)
        {
            if (!RecipientWeakReference.IsAlive)
            {
                return;
            }

            if (ActionWithoutSender != null)
            {
                if (ActionWithoutSender.Target is Control)
                {
                    var actiontmp = ActionWithoutSender.Target as Control;
                    var hadWinHandle = actiontmp.IsHandleCreated;
                    if (!hadWinHandle) return;
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        actiontmp.Invoke(ActionWithoutSender, message);
                    }
                    else
                    {
                        actiontmp.BeginInvoke(ActionWithoutSender, message);
                    }

                    #region Old Code
                    //if (actiontmp.InvokeRequired)
                    //{
                    //Message = message;
                    //if (EExecutionMode == EExecutionMode.Synchronization)
                    //{
                    //    actiontmp.Invoke(ActionWithoutSender, message);
                    //}
                    //else
                    //{
                    //    actiontmp.BeginInvoke(ActionWithoutSender, message);
                    //}
                    //}
                    //else
                    //{
                    //    Message = message;
                    //    if (EExecutionMode == EExecutionMode.Synchronization)
                    //    {
                    //        ActionWithoutSender(message);
                    //    }
                    //    else
                    //    {
                    //        ActionWithoutSender.BeginInvoke(message, null, null);
                    //    }
                    //}


                    #endregion

                }
                else
                {
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        ActionWithoutSender(message);
                    }
                    else
                    {
                        ActionWithoutSender.BeginInvoke(message, null, null);
                    }
                }
            }

            if (ActionWithOperated != null)
            {
                if (ActionWithOperated.Target is Control)
                {
                    var actiontmp = ActionWithOperated.Target as Control;
                    var hadWinHandle = actiontmp.IsHandleCreated;
                    if (!hadWinHandle) return;
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        actiontmp.Invoke(ActionWithOperated, message, OperatedArgs);
                    }
                    else
                    {
                        actiontmp.BeginInvoke(ActionWithOperated, message, OperatedArgs);
                    }

                    #region Old Code
                    //if (actiontmp.InvokeRequired)
                    //{
                    //Message = message;
                    //if (EExecutionMode == EExecutionMode.Synchronization)
                    //{
                    //    actiontmp.Invoke(ActionWithOperated, message, OperatedArgs);
                    //}
                    //else
                    //{
                    //    actiontmp.BeginInvoke(ActionWithOperated, message, OperatedArgs);
                    //}
                    //}
                    //else
                    //{
                    //    Message = message;
                    //    if (EExecutionMode == EExecutionMode.Synchronization)
                    //    {
                    //        ActionWithOperated(message, OperatedArgs);
                    //    }
                    //    else
                    //    {
                    //        ActionWithOperated.BeginInvoke(message, OperatedArgs, null, null);
                    //    }
                    //}


                    #endregion

                }
                else
                {
                    Message = message;
                    if (EExecutionMode == EExecutionMode.Synchronization)
                    {
                        ActionWithOperated(message, OperatedArgs);
                    }
                    else
                    {
                        ActionWithOperated.BeginInvoke(message, OperatedArgs, null, null);
                    }
                }
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        ///     添加消息
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">消息订阅者（消息接收者）</param>
        /// <param name="actionWithoutSender">接收消息后执行的委托</param>
        /// <param name="eExecutionMode">接收到信息后执行的方式</param>
        /// <param name="allowReceiveBroadcast">是否接收广播消息</param>
        public static void AddMessage(string messageTab, object recipient, Action<TMessage> actionWithoutSender,
            EExecutionMode eExecutionMode, bool allowReceiveBroadcast=false)
        {
            if (!ContainsMsg(messageTab, recipient))
            {
                Clear(messageTab);
                var recipientWeakReference = new WeakReference(recipient);
                if (HasMessage(messageTab))
                {
                    MsgDictionary[messageTab].Add(new MessageInfo<TMessage>
                    {
                        /*Recipient = recipient*/
                        RecipientWeakReference = recipientWeakReference,
                        ActionWithoutSender = actionWithoutSender,
                        EExecutionMode = eExecutionMode,
                        AllowReceiveBroadcast = allowReceiveBroadcast
                    });
                }
                else
                {
                    MsgDictionary.Add(messageTab, new List<MessageInfo<TMessage>>
                    {
                        new MessageInfo<TMessage>
                        {
                            /*Recipient = recipient*/
                            RecipientWeakReference = recipientWeakReference,
                            ActionWithoutSender = actionWithoutSender,
                            EExecutionMode = eExecutionMode,
                            AllowReceiveBroadcast=allowReceiveBroadcast
                        }
                    });
                }

                MessageRegisterManager.RecipientAddMessage(messageTab, recipientWeakReference);
            }
        }

        public static void AddMessage(string messageTab, object recipient, Action<object,TMessage> action,
            EExecutionMode eExecutionMode)
        {
            if (!ContainsMsg(messageTab, recipient))
            {
                Clear(messageTab);
                var recipientWeakReference = new WeakReference(recipient);
                if (HasMessage(messageTab))
                {
                    MsgDictionary[messageTab].Add(new MessageInfo<TMessage>
                    {
                        /*Recipient = recipient*/
                        RecipientWeakReference = recipientWeakReference,
                        Action = action,
                        EExecutionMode = eExecutionMode,
                        //AllowReceiveBroadcast = allowReceiveBroadcast
                    });
                }
                else
                {
                    MsgDictionary.Add(messageTab, new List<MessageInfo<TMessage>>
                    {
                        new MessageInfo<TMessage>
                        {
                            /*Recipient = recipient*/
                            RecipientWeakReference = recipientWeakReference,
                            Action = action,
                            EExecutionMode = eExecutionMode,
                            //AllowReceiveBroadcast=allowReceiveBroadcast
                        }
                    });
                }

                MessageRegisterManager.RecipientAddMessage(messageTab, recipientWeakReference);
            }
        }

        /// <summary>
        ///     添加消息
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">消息订阅者（消息接收者）</param>
        /// <param name="actionWithOperated">收到消息时需要执行的委托</param>
        /// <param name="operatedArgs">委托执行时需要操作的对象（一些指定的参数）</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        /// <param name="allowReceiveBroadcast">是否接收广播消息</param>
        public static void AddMessage(string messageTab, object recipient,
            Action<TMessage, object[]> actionWithOperated, object[] operatedArgs, EExecutionMode eExecutionMode, bool allowReceiveBroadcast=false)
        {
            if (!ContainsMsg(messageTab, recipient))
            {
                Clear(messageTab);
                var recipientWeakReference = new WeakReference(recipient);
                if (HasMessage(messageTab))
                {
                    MsgDictionary[messageTab].Add(new MessageInfo<TMessage>
                    {
                        RecipientWeakReference = recipientWeakReference,
                        ActionWithOperated = actionWithOperated,
                        OperatedArgs = operatedArgs,
                        EExecutionMode = eExecutionMode,
                        AllowReceiveBroadcast=allowReceiveBroadcast
                       
                    });
                }
                else
                {
                    MsgDictionary.Add(messageTab,
                        new List<MessageInfo<TMessage>>
                        {
                            new MessageInfo<TMessage>
                            {
                                RecipientWeakReference = recipientWeakReference,
                                ActionWithOperated = actionWithOperated,
                                OperatedArgs = operatedArgs,
                                EExecutionMode = eExecutionMode,
                                AllowReceiveBroadcast=allowReceiveBroadcast
                            }
                        });
                }

                MessageRegisterManager.RecipientAddMessage(messageTab, recipientWeakReference);
            }
        }

        private static void Clear(string messageTab)
        {
            if (HasMessage(messageTab))
            {
                MsgDictionary[messageTab].RemoveAll(item => !item.RecipientWeakReference.IsAlive);

                var disableRecipents = MessageRegisterManager.GetDisableRecipent(messageTab);
                if (disableRecipents == null)
                {
                    return;
                }

                if (disableRecipents.Count == 0)
                {
                    MessageRegisterManager.RemoveDisableMessage(messageTab);
                }

                MsgDictionary[messageTab].RemoveAll(item => disableRecipents.Contains(item.RecipientWeakReference));
            }
        }

        private static void RemoveMessage(WeakReference recipientWeakReference)
        {
            foreach (var item in MsgDictionary)
            {
                item.Value.RemoveAll(msginfo => msginfo.RecipientWeakReference.Equals(recipientWeakReference));
            }
        }

        /// <summary>
        ///     移除view及其绑定的presenter的所有消息标签订阅
        /// </summary>
        /// <param name="view"></param>
        public static void RemoveMessage(object view)
        {
            var presenterWeakReference = ViewPresenterManager.FindPreenterByView(view);
            var viewWeakReference = MessageRegisterManager.RecipientGetWeakReference(view);
            if (presenterWeakReference != null)
            {
                if (presenterWeakReference.IsAlive)
                {
                    var presenter = presenterWeakReference.Target;
                    var viewWeakReferences = ViewPresenterManager.FindViewsByPresenter(presenter);
                    if (viewWeakReferences.Count <= 1 && viewWeakReferences[0].Equals(viewWeakReference))
                    {
                        RemoveMessage(presenterWeakReference);
                    }
                }
                else
                {
                    RemoveMessage(presenterWeakReference);
                }
            }

            RemoveMessage(viewWeakReference);
        }

        /// <summary>
        ///     删除消息
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">消息订阅者</param>
        public static void RemoveMessage(string messageTab, object recipient)
        {
            if (ContainsMsg(messageTab, recipient))
            {
                MsgDictionary[messageTab]
                    .RemoveAll(item => item.Recipient.Equals(recipient) /*|| !item.weakReference.IsAlive*/);
                MessageRegisterManager.RecipientRemoveMessage(messageTab, recipient);
            }
        }

        /// <summary>
        ///     是否存在有该消息的订阅
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <returns></returns>
        private static bool HasMessage(string messageTab)
        {
            return MsgDictionary.ContainsKey(messageTab);
        }

        /// <summary>
        ///     查找该消息是否有该订阅者订阅
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">消息订阅者（消息接收者）</param>
        /// <returns></returns>
        private static bool ContainsMsg(string messageTab, object recipient)
        {
            return HasMessage(messageTab) && MsgDictionary[messageTab].Exists(item => item.Recipient.Equals(recipient));
        }

        public static List<MessageInfo<TMessage>> GetBroadcastMessageInfos(string messageTab)
        {
            if (HasMessage(messageTab))
            {
                return MsgDictionary[messageTab].ToList().FindAll(item=>item.AllowReceiveBroadcast);
            }
            return new List<MessageInfo<TMessage>>();
        }

        /// <summary>
        ///     获取所有接收者
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        public static List<MessageInfo<TMessage>> GetMessageInfos(string messageTab)
        {
            if (HasMessage(messageTab))
            {
                return MsgDictionary[messageTab].ToList();
            }

            return new List<MessageInfo<TMessage>>();
        }

        /// <summary>
        ///     获取绑定的接收者
        /// </summary>
        /// <param name="sender">消息发送者</param>
        /// <param name="messageTab">消息标签</param>
        /// <returns></returns>
        public static List<MessageInfo<TMessage>> GetMessageInfos(object sender, string messageTab)
        {
            var msgs = new List<MessageInfo<TMessage>>();
            if (HasMessage(messageTab))
            {
                //获取该消息发送者注册的消息弱引用对象,以确定该发送是否注册消息
                var weakReference = MessageRegisterManager.RecipientGetWeakReference(sender);
                if (weakReference == null)
                {
                    return msgs; 
                }

                //获取与发送者绑定的对象列表
                if (ViewPresenterManager.IsPresenter(sender))
                {
                    var viewWeakReferences = ViewPresenterManager.FindViewsByPresenter(sender);
                    foreach (var viewWeakReference in viewWeakReferences)
                    {
                        if (!viewWeakReference.IsAlive)
                        {
                            continue;
                        }

                        var view = viewWeakReference.Target;
                        var msg = GetMessageInfos(messageTab, view);
                        if (msg == null)
                        {
                            continue;
                        }
                        msgs.Add(msg);
                    }

                    return msgs;
                }

                if (ViewPresenterManager.IsView(sender))
                {
                    var presenterWeakReference = ViewPresenterManager.FindPreenterByView(sender);

                    if (presenterWeakReference.IsAlive)
                    {
                        var presenter = presenterWeakReference.Target;
                        var msg = GetMessageInfos(messageTab, presenter);
                        msgs.Add(msg);
                        return msgs;
                    }
                }
            }

            return msgs; 
        }

        /// <summary>
        ///     获取绑定的接收者
        /// </summary>
        /// <param name="sender">消息发送者（presenter）</param>
        /// <param name="recipient">消息接收者（View）</param>
        /// <param name="messageTab">消息标签</param>
        /// <returns></returns>
        public static List<MessageInfo<TMessage>> GetMessageInfos(object sender, object recipient,string messageTab)
        {
            var msgs = new List<MessageInfo<TMessage>>();
            if (HasMessage(messageTab))
            {
                //获取该消息发送者注册的消息弱引用对象,以确定该发送是否注册消息
                var weakReference = MessageRegisterManager.RecipientGetWeakReference(sender);
                if (weakReference == null)
                {
                    return msgs; 
                }

                //获取与发送者绑定的对象列表
                if (ViewPresenterManager.IsPresenter(sender))
                { 
                    var view = recipient;
                    var msg = GetMessageInfos(messageTab, view);
                    if (msg != null)
                    {
                        msgs.Add(msg);
                    }
                    return msgs;
                }
            }

            return msgs; 
        }

        /// <summary>
        ///     获取指定接收者
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">发送对象（消息订阅者）</param>
        public static MessageInfo<TMessage> GetMessageInfos(string messageTab, object recipient)
        {
            if (ContainsMsg(messageTab, recipient))
            {
                return MsgDictionary[messageTab].Find(item => { return item.Recipient.Equals(recipient); });
            }

            return default(MessageInfo<TMessage>);
            //return new MessageInfo<TMessage>();
        }

        #endregion

    }
}