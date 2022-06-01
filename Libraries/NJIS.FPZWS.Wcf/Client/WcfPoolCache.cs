// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Client
//  文 件 名：WcfPoolCache.cs
//  创建时间：2017-12-29 8:30
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;
using System.Threading;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    internal class WcfPoolCache
    {
        /// <summary>
        ///     Wcf连接池
        /// </summary>
        private static volatile IDictionary<string, WcfPool> _poolDic = new Dictionary<string, WcfPool>();

        private static volatile object _lockpool = new object();

        /// <summary>
        ///     监控线程
        /// </summary>
        private static volatile IDictionary<string, Thread> _thDic = new Dictionary<string, Thread>();

        private static volatile object _lockth = new object();

        /// <summary>
        ///     初始化连接池
        /// </summary>
        /// <param name="isUseWcfPool">是否使用连接池</param>
        /// <param name="wcfMaxPoolSize">池子最大值</param>
        /// <param name="wcfOutTime">获取连接超时时间</param>
        /// <param name="wcfFailureTime">连接池回收时间</param>
        /// <param name="serverName">服务器名</param>
        /// <param name="wcfPoolMonitorReapTime"></param>
        public static void Init(bool isUseWcfPool, int wcfMaxPoolSize, long wcfOutTime, long wcfFailureTime,
            string serverName, int wcfPoolMonitorReapTime)
        {
            //装在连接池
            if (isUseWcfPool && !_poolDic.ContainsKey(serverName))
            {
                lock (_lockpool)
                {
                    if (isUseWcfPool && !_poolDic.ContainsKey(serverName))
                    {
                        var pool = new WcfPool(wcfMaxPoolSize, wcfOutTime, wcfFailureTime, wcfPoolMonitorReapTime);
                        _poolDic.Add(serverName, pool);
                    }
                }
            }
            //开启监控线程
            if (isUseWcfPool && !_thDic.ContainsKey(serverName))
            {
                lock (_lockth)
                {
                    if (!_thDic.ContainsKey(serverName))
                    {
                        var poolMonitorTh = new Thread(_poolDic[serverName].MonitorExec);
                        poolMonitorTh.Start();
                        _thDic.Add(serverName, poolMonitorTh);
                    }
                }
            }
        }

        /// <summary>
        ///     获取连接池
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static WcfPool GetWcfPool(string serverName)
        {
            return _poolDic[serverName];
        }
    }
}