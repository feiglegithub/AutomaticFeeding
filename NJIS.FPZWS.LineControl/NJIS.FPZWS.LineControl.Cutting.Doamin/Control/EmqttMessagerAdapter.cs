using System;
using System.Collections.Concurrent;
using NJIS.FPZWS.LineControl.PLC.Message;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control
{
    public class EmqttMessagerAdapter: IMessagerAdapter
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
