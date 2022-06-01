//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：MessagerAdapterBase.cs
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

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Message
{
    public abstract class MessagerAdapterBase : IMessagerAdapter
    {
        private readonly ConcurrentDictionary<string, IMessager> _cacheMessager;


        /// <summary>
        ///     初始化一个<see cref="MessagerAdapterBase" />类型的新实例
        /// </summary>
        protected MessagerAdapterBase()
        {
            _cacheMessager = new ConcurrentDictionary<string, IMessager>();
        }

        public IMessager GetMessager(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return GetMessager(type.FullName);
        }

        public IMessager GetMessager(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);

            return GetMessagerInternal(name);
        }

        /// <summary>
        ///     创建指定名称的缓存实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        protected abstract IMessager CreateMessager(string name);


        /// <summary>
        ///     获取指定名称的Logger实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns>日志实例</returns>
        /// <exception cref="NotSupportedException">指定名称的日志缓存实例不存在则返回异常<see cref="NotSupportedException" /></exception>
        protected virtual IMessager GetMessagerInternal(string name)
        {
            IMessager log;
            if (_cacheMessager.TryGetValue(name, out log))
            {
                return log;
            }

            log = CreateMessager(name);
            if (log == null)
            {
                throw new NotSupportedException($"创建名称为“{name}”的消息实例时“{GetType().FullName}”返回空实例。");
            }

            _cacheMessager[name] = log;
            return log;
        }

        /// <summary>
        ///     清除缓存中的日志实例
        /// </summary>
        protected virtual void ClearLoggerCache()
        {
            _cacheMessager.Clear();
        }
    }
}