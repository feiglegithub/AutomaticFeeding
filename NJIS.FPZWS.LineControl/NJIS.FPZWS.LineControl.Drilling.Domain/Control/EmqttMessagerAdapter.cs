//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：EmqttMessagerAdapter.cs
//   创建时间：2018-11-29 8:25
//   作    者：
//   说    明：
//   修改时间：2018-11-29 8:25
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Concurrent;
using NJIS.FPZWS.LineControl.PLC.Message;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control
{
    public class EmqttMessagerAdapter : IMessagerAdapter
    {
        private readonly ConcurrentDictionary<string, IMessager> _cacheMessagers;

        public EmqttMessagerAdapter()
        {
            _cacheMessagers = new ConcurrentDictionary<string, IMessager>();
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
            IMessager messager;
            if (_cacheMessagers.TryGetValue(name, out messager))
            {
                return messager;
            }

            messager = new PlcMqttMessager();
            _cacheMessagers[name] = messager;
            return messager;
        }
    }
}