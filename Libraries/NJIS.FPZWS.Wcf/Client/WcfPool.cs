// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Client
//  文 件 名：WcfPool.cs
//  创建时间：2017-12-29 8:24
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    internal class WcfPool
    {
        private readonly object _lockhelper = new object();

        /// <summary>
        ///     监控时间间隔（单位：秒）
        /// </summary>
        private readonly int _monitorTimeSpan = 30;

        /// <summary>
        ///     Wcf连接失效时间(默认一分钟)
        /// </summary>
        private readonly TimeSpan _wcfFailureTime = new TimeSpan((long) 60 * 1000 * 10000);

        /// <summary>
        ///     Wcf连接池最大值，默认为100
        /// </summary>
        private readonly int _wcfMaxPoolSize = 100;

        /// <summary>
        ///     池子里个类型连接总数
        /// </summary>
        private IDictionary<string, int> _countNumsDic;

        /// <summary>
        ///     索引
        /// </summary>
        private int _index;

        /// <summary>
        ///     通讯实体列表
        /// </summary>
        private List<WcfCommunicationObj> _poollist;

        /// <summary>
        ///     已经使用的数量
        /// </summary>
        private IDictionary<string, int> _usedNumsDic;

        /// <summary>
        ///     Wcf获取连接过期时间(默认一分钟)
        /// </summary>
        private TimeSpan _wcfOutTime = new TimeSpan((long) 60 * 1000 * 10000);

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="wcfMaxPoolSize">wcf池子最大数</param>
        /// <param name="wcfOutTime">wcf获取连接超时时间，以秒为单位</param>
        /// <param name="wcfFailureTime"></param>
        /// <param name="wcfPoolMonitorReapTime"></param>
        public WcfPool(int wcfMaxPoolSize, long wcfOutTime, long wcfFailureTime, int wcfPoolMonitorReapTime)
        {
            _wcfMaxPoolSize = wcfMaxPoolSize;
            _wcfOutTime = new TimeSpan(wcfOutTime * 1000 * 10000);
            _wcfFailureTime = new TimeSpan(wcfFailureTime * 1000 * 10000);
            _monitorTimeSpan = wcfPoolMonitorReapTime;
            _poollist = new List<WcfCommunicationObj>(_wcfMaxPoolSize);
            _usedNumsDic = new Dictionary<string, int>();
            _countNumsDic = new Dictionary<string, int>();
        }

        /// <summary>
        ///     当前池子连接总数
        /// </summary>
        public int CurrentPoolNums
        {
            get { return _poollist.Count; }
        }

        /// <summary>
        ///     判断池子是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsPoolFull
        {
            get { return _poollist.Count >= _wcfMaxPoolSize; }
        }

        ~WcfPool()
        {
            ClearPool();
        }

        /// <summary>
        ///     监控逻辑
        /// </summary>
        public void MonitorExec()
        {
            while (true)
            {
                //write("F:\\1.txt", "monitor");
                Thread.Sleep(_monitorTimeSpan * 1000);
                try
                {
                    ReapPool();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     清空连接池
        /// </summary>
        public void ClearPool()
        {
            lock (_lockhelper)
            {
                foreach (var obj in _poollist)
                {
                    try
                    {
                        //obj.Scope.Dispose();
                        obj.CommucationObject.Close();
                    }
                    catch
                    {
                        obj.CommucationObject.Abort();
                    }
                }
                _poollist.Clear();
                _poollist = null;
                _countNumsDic.Clear();
                _countNumsDic = null;
                _usedNumsDic.Clear();
                _usedNumsDic = null;
                _index = 0;
            }
        }

        /// <summary>
        ///     当前正在使用的池子数量
        /// </summary>
        public int GetUsedPoolNums(string contract)
        {
            if (_usedNumsDic.ContainsKey(contract))
            {
                return _usedNumsDic[contract];
            }
            return 0;
        }

        /// <summary>
        ///     当前空闲池子数量
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int GetFreePoolNums(string contract)
        {
            return GetCountPoolNums(contract) - GetUsedPoolNums(contract);
        }

        /// <summary>
        ///     判断非当前契约是否有空闲池子
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public bool GetFreePoolNumsNotCurrent(string contract)
        {
            var flag = false;
            foreach (var obj in _poollist)
            {
                if (!obj.IsUsed && obj.Contract != contract)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        ///     获取池子总数
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int GetCountPoolNums(string contract)
        {
            if (_countNumsDic.ContainsKey(contract))
            {
                return _countNumsDic[contract];
            }
            return 0;
        }

        /// <summary>
        ///     处理连接池
        /// </summary>
        private void ReapPool()
        {
            lock (_lockhelper)
            {
                //string content = "";
                for (var i = 0; i < _poollist.Count; i++)
                {
                    var obj = _poollist[i];
                    if (!obj.IsUsed && DateTime.Now - obj.CreatedTime > _wcfFailureTime ||
                        obj.CommucationObject.State != CommunicationState.Opened)
                    {
                        try
                        {
                            //obj.Scope.Dispose();
                            obj.CommucationObject.Close();
                        }
                        catch
                        {
                            obj.CommucationObject.Abort();
                        }
                        _poollist.Remove(obj);
                        if (_countNumsDic.ContainsKey(obj.Contract))
                            _countNumsDic[obj.Contract] = _countNumsDic[obj.Contract] == 0
                                ? 0
                                : _countNumsDic[obj.Contract] - 1;
                        if (_usedNumsDic.ContainsKey(obj.Contract))
                            _usedNumsDic[obj.Contract] = _usedNumsDic[obj.Contract] == 0
                                ? 0
                                : _usedNumsDic[obj.Contract] - 1;

                        i--;
                    }
                }
                //write("F:\\2.txt", content);
            }
        }

        /// <summary>
        ///     加入连接池
        /// </summary>
        /// <typeparam name="T">连接契约类型</typeparam>
        /// <param name="factory"></param>
        /// <param name="channel">连接</param>
        /// <param name="index">返回的连接池索引</param>
        /// <param name="isReap"></param>
        /// <returns></returns>
        public bool AddPool<T>(ChannelFactory<T> factory, out T channel, out int? index, bool isReap)
        {
            var flag = false;
            index = null;
            channel = default(T);

            if (_poollist.Count < _wcfMaxPoolSize)
            {
                lock (_lockhelper)
                {
                    if (_poollist.Count < _wcfMaxPoolSize)
                    {
                        channel = factory.CreateChannel();
                        var communicationobj = channel as ICommunicationObject;
                        communicationobj.Open();
                        var obj = new WcfCommunicationObj();
                        _index = _index >= int.MaxValue ? 1 : _index + 1;
                        index = _index;
                        obj.Index = _index;
                        obj.UsedNums = 1;
                        obj.CommucationObject = communicationobj;
                        obj.Contract = typeof(T).FullName;
                        obj.CreatedTime = DateTime.Now;
                        obj.LastUsedTime = DateTime.Now;
                        obj.IsUsed = true;
                        //obj.Scope = new OperationContextScope(((IClientChannel)channel));
                        _poollist.Add(obj);
                        _countNumsDic[obj.Contract] = _countNumsDic.ContainsKey(obj.Contract)
                            ? _countNumsDic[obj.Contract] + 1
                            : 1;
                        _usedNumsDic[obj.Contract] = _usedNumsDic.ContainsKey(obj.Contract)
                            ? _usedNumsDic[obj.Contract] + 1
                            : 1;
                        flag = true;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        ///     从连接池中获取一个连接
        /// </summary>
        /// <typeparam name="T">获取的契约</typeparam>
        /// <returns></returns>
        public WcfCommunicationObj GetChannel<T>()
        {
            //先做一次清理
            //ReapPool();

            var t = typeof(T).FullName;
            WcfCommunicationObj channel = null;
            if (GetFreePoolNums(t) > 0)
            {
                lock (_lockhelper)
                {
                    if (GetFreePoolNums(t) > 0)
                    {
                        for (var i = 0; i < _poollist.Count; i++)
                        {
                            var obj = _poollist[i];
                            if (!obj.IsUsed && DateTime.Now - obj.CreatedTime < _wcfFailureTime && t == obj.Contract)
                            {
                                if (obj.CommucationObject.State == CommunicationState.Opened)
                                {
                                    obj.IsUsed = true;
                                    obj.UsedNums++;
                                    obj.LastUsedTime = DateTime.Now;
                                    _usedNumsDic[obj.Contract] =
                                        _usedNumsDic.ContainsKey(obj.Contract) ? _usedNumsDic[obj.Contract] + 1 : 1;

                                    channel = obj;
                                    break;
                                }
                                try
                                {
                                    //obj.Scope.Dispose();
                                    obj.CommucationObject.Close();
                                }
                                catch
                                {
                                    obj.CommucationObject.Abort();
                                }
                                _poollist.Remove(obj);
                                if (_countNumsDic.ContainsKey(obj.Contract))
                                    _countNumsDic[obj.Contract] =
                                        _countNumsDic[obj.Contract] == 0 ? 0 : _countNumsDic[obj.Contract] - 1;
                                if (_usedNumsDic.ContainsKey(obj.Contract))
                                    _usedNumsDic[obj.Contract] =
                                        _usedNumsDic[obj.Contract] == 0 ? 0 : _usedNumsDic[obj.Contract] - 1;
                                i--;
                            }
                        }
                    }
                }
            }

            return channel;
        }

        /// <summary>
        ///     把连接放回池子
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ReturnPool<T>(int index)
        {
            var t = typeof(T).FullName;
            lock (_lockhelper)
            {
                foreach (var obj in _poollist)
                {
                    if (index == obj.Index && t == obj.Contract)
                    {
                        obj.IsUsed = false;
                        obj.LastUsedTime = DateTime.Now;
                        if (_usedNumsDic.ContainsKey(obj.Contract))
                            _usedNumsDic[obj.Contract] = _usedNumsDic[obj.Contract] == 0
                                ? 0
                                : _usedNumsDic[obj.Contract] - 1;
                        break;
                    }
                }
            }

            //做一次清理
            //ReapPool();
        }

        /// <summary>
        ///     移除索引的连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemovePoolAt<T>(int index)
        {
            var flag = false;
            var t = typeof(T).FullName;
            lock (_lockhelper)
            {
                var len = _poollist.Count;
                for (var i = 0; i < _poollist.Count; i++)
                {
                    var obj = _poollist[i];
                    if (index == obj.Index && t == obj.Contract)
                    {
                        try
                        {
                            //obj.Scope.Dispose();
                            obj.CommucationObject.Close();
                        }
                        catch
                        {
                            obj.CommucationObject.Abort();
                        }
                        _poollist.Remove(obj);
                        if (_countNumsDic.ContainsKey(obj.Contract))
                            _countNumsDic[obj.Contract] = _countNumsDic[obj.Contract] == 0
                                ? 0
                                : _countNumsDic[obj.Contract] - 1;
                        if (_usedNumsDic.ContainsKey(obj.Contract))
                            _usedNumsDic[obj.Contract] = _usedNumsDic[obj.Contract] == 0
                                ? 0
                                : _usedNumsDic[obj.Contract] - 1;

                        flag = true;
                        i--;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        ///     踢掉一个非当前契约的空闲连接
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="channel"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemovePoolOneNotAt<T>(ChannelFactory<T> factory, out T channel, out int? index)
        {
            var flag = false;
            index = null;
            channel = default(T);

            var contract = typeof(T).FullName;
            lock (_lockhelper)
            {
                var len = _poollist.Count;
                //如果池子满了，先踢出一个非当前创建契约的连接
                if (_poollist.Count >= _wcfMaxPoolSize)
                {
                    for (var i = 0; i < _poollist.Count; i++)
                    {
                        var obj = _poollist[i];
                        if (!obj.IsUsed && obj.Contract != contract)
                        {
                            try
                            {
                                //obj.Scope.Dispose();
                                obj.CommucationObject.Close();
                            }
                            catch
                            {
                                obj.CommucationObject.Abort();
                            }
                            _poollist.Remove(obj);
                            if (_countNumsDic.ContainsKey(obj.Contract))
                                _countNumsDic[obj.Contract] =
                                    _countNumsDic[obj.Contract] == 0 ? 0 : _countNumsDic[obj.Contract] - 1;
                            if (_usedNumsDic.ContainsKey(obj.Contract))
                                _usedNumsDic[obj.Contract] =
                                    _usedNumsDic[obj.Contract] == 0 ? 0 : _usedNumsDic[obj.Contract] - 1;

                            flag = true;
                            i--;
                            break;
                        }
                    }
                }
                //增加一个连接到池子
                if (_poollist.Count < _wcfMaxPoolSize)
                {
                    channel = factory.CreateChannel();
                    var communicationobj = channel as ICommunicationObject;
                    communicationobj.Open();
                    var obj = new WcfCommunicationObj();
                    _index = _index >= int.MaxValue ? 1 : _index + 1;
                    index = _index;
                    obj.Index = _index;
                    obj.UsedNums = 1;
                    obj.CommucationObject = communicationobj;
                    obj.Contract = contract;
                    obj.CreatedTime = DateTime.Now;
                    obj.LastUsedTime = DateTime.Now;
                    obj.IsUsed = true;
                    //obj.Scope = new OperationContextScope(((IClientChannel)channel));
                    _poollist.Add(obj);
                    _countNumsDic[obj.Contract] = _countNumsDic.ContainsKey(obj.Contract)
                        ? _countNumsDic[obj.Contract] + 1
                        : 1;
                    _usedNumsDic[obj.Contract] = _usedNumsDic.ContainsKey(obj.Contract)
                        ? _usedNumsDic[obj.Contract] + 1
                        : 1;
                    flag = true;
                }
            }

            return flag;
        }
    }
}