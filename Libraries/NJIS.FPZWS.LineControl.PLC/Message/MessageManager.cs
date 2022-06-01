//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：MessageManager.cs
//   创建时间：2018-11-23 16:40
//   作    者：
//   说    明：
//   修改时间：2018-11-23 16:40
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Message
{
    public class MessageManager
    {
        private static readonly ConcurrentDictionary<string, IMessager> Messagers;
        private static readonly object LockObj = new object();

        /// <summary>
        ///     初始化一个<see cref="MessageManager" />实例
        /// </summary>
        static MessageManager()
        {
            Messagers = new ConcurrentDictionary<string, IMessager>();
            Adapters = new List<IMessagerAdapter>();
        }

        /// <summary>
        ///     获取 消息适配器集合
        /// </summary>
        internal static ICollection<IMessagerAdapter> Adapters { get; }

        /// <summary>
        ///     添加消息适配器
        /// </summary>
        public static void AddMessagerAdapter(IMessagerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.Any(m => m == adapter))
                {
                    return;
                }

                Adapters.Add(adapter);
                Messagers.Clear();
            }
        }

        /// <summary>
        ///     移除消息适配器
        /// </summary>
        public static void RemoveMessagerAdapter(IMessagerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.All(m => m != adapter))
                {
                    return;
                }

                Adapters.Remove(adapter);
                Messagers.Clear();
            }
        }

        /// <summary>
        ///     获取消息记录者实例
        /// </summary>
        public static IMessager GetMessager(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            lock (LockObj)
            {
                IMessager Messager;
                if (Messagers.TryGetValue(name, out Messager))
                {
                    return Messager;
                }

                Messager = new InternalMessager(name);
                Messagers[name] = Messager;
                return Messager;
            }
        }


        /// <summary>
        ///     获取指定类型的消息记录实例
        /// </summary>
        public static IMessager GetMessager(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return GetMessager(type.FullName);
        }

        /// <summary>
        ///     获取指定类型的消息记录实例
        /// </summary>
        public static IMessager GetMessager<T>()
        {
            return GetMessager(typeof(T));
        }
    }
}