//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：MessageRegisterManager.cs
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
    internal class MessageRegisterManager
    {
        /// <summary>
        ///     注册对象与其注册的消息标签列表 -- int为注册对象的HashCode Tuple（WeakReference, List（string))Item1为注册对象的弱引用，Item2为注册对象所注册的消息标签列表
        /// </summary>
        private static readonly Dictionary<int, Tuple<WeakReference, List<string>>> RecipientMsgsDictionary =
            new Dictionary<int, Tuple<WeakReference, List<string>>>();

        private static readonly Dictionary<string, HashSet<WeakReference>> DisableMessage =
            new Dictionary<string, HashSet<WeakReference>>();

        private static bool ContainsRecipient(object recipient)
        {
            var hashCode = recipient.GetHashCode();
            return RecipientMsgsDictionary.ContainsKey(hashCode);
        }

        public static bool ContainsRecipient(int hashCode)
        {
            return RecipientMsgsDictionary.ContainsKey(hashCode);
        }

        /// <summary>
        ///     注册对象添加消息标签
        /// </summary>
        /// <param name="messageTab"></param>
        /// <param name="weakReference"></param>
        public static void RecipientAddMessage(string messageTab, WeakReference weakReference)
        {
            var recipient = weakReference.Target;
            var recipientHashCode = recipient.GetHashCode();
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                RecipientMsgsDictionary[recipientHashCode].Item2.Add(messageTab);
            }
            else
            {
                RecipientMsgsDictionary.Add(recipientHashCode,
                    new Tuple<WeakReference, List<string>>(weakReference, new List<string> {messageTab}));
            }
        }

        /// <summary>
        ///     获取该消息注册对象的弱引用
        /// </summary>
        /// <param name="recipient">消息注册对象</param>
        /// <returns></returns>
        public static WeakReference RecipientGetWeakReference(object recipient)
        {
            var recipientHashCode = recipient.GetHashCode();
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                return RecipientMsgsDictionary[recipientHashCode].Item1;
            }

            return null;
        }

        /// <summary>
        ///     获取注册对象的所有消息标签
        /// </summary>
        /// <param name="recipient">注册对象</param>
        /// <returns></returns>
        public static List<string> RecipientGetMessages(object recipient)
        {
            var recipientHashCode = recipient.GetHashCode();
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                return RecipientMsgsDictionary[recipientHashCode].Item2;
            }

            return default(List<string>);
        }

        /// <summary>
        ///     获取注册对象的所有消息标签
        /// </summary>
        /// <param name="recipientWeakReference">注册对象</param>
        /// <returns></returns>
        private static Tuple<int, List<string>> _recipientGetMessages(WeakReference recipientWeakReference)
        {
            foreach (var item in RecipientMsgsDictionary)
            {
                if (item.Value.Item1.Equals(recipientWeakReference))
                {
                    return new Tuple<int, List<string>>(item.Key, item.Value.Item2);
                }
            }

            return null;
        }

        /// <summary>
        ///     获取注册对象的所有消息标签
        /// </summary>
        /// <param name="recipientWeakReference">注册对象</param>
        /// <returns></returns>
        private static List<string> RecipientGetMessages(WeakReference recipientWeakReference)
        {
            var tuple = _recipientGetMessages(recipientWeakReference);
            if (tuple != null)
            {
                return tuple.Item2;
            }

            return null;
        }

        /// <summary>
        ///     移除对象所注册的消息标签
        /// </summary>
        /// <param name="messageTab">消息标签</param>
        /// <param name="recipient">注册的对象</param>
        public static void RecipientRemoveMessage(string messageTab, object recipient)
        {
            var recipientHashCode = recipient.GetHashCode();
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                RecipientMsgsDictionary[recipientHashCode].Item2.RemoveAll(item => item == messageTab);
            }
        }

        public static void AddDisableMessage(string mesasageTab, WeakReference weakReference)
        {
            if (DisableMessage.ContainsKey(mesasageTab))
            {
                var set = DisableMessage[mesasageTab];
                if (!set.Contains(weakReference))
                {
                    set.Add(weakReference);
                }
            }
            else
            {
                DisableMessage.Add(mesasageTab, new HashSet<WeakReference> {weakReference});
            }
        }

        public static void RemoveDisableMessage(string mesasageTab)
        {
            if (ContainsDisableMessageTab(mesasageTab))
            {
                DisableMessage.Remove(mesasageTab);
            }
        }

        public static bool ContainsDisableMessageTab(string messageTab)
        {
            return DisableMessage.ContainsKey(messageTab);
        }

        public static HashSet<WeakReference> GetDisableRecipent(string messageTab)
        {
            if (ContainsDisableMessageTab(messageTab))
            {
                return DisableMessage[messageTab];
            }

            return null;
        }

        /// <summary>
        ///     移除注册对象的所有消息标签
        /// </summary>
        /// <param name="recipient">注册对象</param>
        public static void RecipientRemoveMessage(object recipient)
        {
            var recipientHashCode = recipient.GetHashCode();
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                var weakReference = RecipientMsgsDictionary[recipientHashCode].Item1;
                var lst = RecipientMsgsDictionary[recipientHashCode].Item2;
                foreach (var messageTab in lst)
                {
                    AddDisableMessage(messageTab, weakReference);
                }

                RecipientMsgsDictionary.Remove(recipientHashCode);
            }
        }

        /// <summary>
        ///     移除注册对象的所有消息标签
        /// </summary>
        /// <param name="recipientHashCode">注册对象</param>
        public static void RecipientRemoveMessage(int recipientHashCode)
        {
            if (RecipientMsgsDictionary.ContainsKey(recipientHashCode))
            {
                var weakReference = RecipientMsgsDictionary[recipientHashCode].Item1;
                var lst = RecipientMsgsDictionary[recipientHashCode].Item2;
                foreach (var messageTab in lst)
                {
                    AddDisableMessage(messageTab, weakReference);
                }

                RecipientMsgsDictionary.Remove(recipientHashCode);
            }
        }

        /// <summary>
        ///     移除注册对象的所有消息标签
        /// </summary>
        /// <param name="recipientWeakReference">注册对象</param>
        public static void RecipientRemoveMessage(WeakReference recipientWeakReference)
        {
            var tuple = _recipientGetMessages(recipientWeakReference);
            if (tuple != null)
            {
                RecipientRemoveMessage(tuple.Item1);
            }
        }
    }
}