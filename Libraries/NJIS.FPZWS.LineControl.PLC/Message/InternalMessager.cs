//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：InternalMessager.cs
//   创建时间：2018-11-23 16:40
//   作    者：
//   说    明：
//   修改时间：2018-11-23 16:40
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Message
{
    /// <summary>
    ///     日志记录者，日志记录输入端
    /// </summary>
    internal sealed class InternalMessager : IMessager
    {
        private readonly ICollection<IMessager> _messages;

        /// <summary>
        ///     初始化一个<see cref="InternalMessager" />新实例
        /// </summary>
        /// <param name="type"></param>
        public InternalMessager(Type type)
            : this(type.FullName)
        {
        }

        /// <summary>
        ///     初始化一个<see cref="InternalMessager" />新实例
        /// </summary>
        /// <param name="name">指定名称</param>
        public InternalMessager(string name)
        {
            _messages = MessageManager.Adapters.Select(adapter => adapter.GetMessager(name)).ToList();
        }

        public void Publish<T>(string topic, T boj)
        {
            foreach (var msg in _messages)
            {
                msg.Publish(topic, msg);
            }
        }
    }
}