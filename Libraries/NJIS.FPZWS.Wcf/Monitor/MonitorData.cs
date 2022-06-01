// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：MonitorData.cs
//  创建时间：2017-12-29 14:20
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.Wcf.Monitor
{
    /// <summary>
    ///     wcf监控操作类(服务器端加入监控信息)
    /// </summary>
    public class MonitorData
    {
        /// <summary>
        ///     单例
        /// </summary>
        private static volatile MonitorData _instance;

        private static volatile object lockhelper = new object();


        private static volatile object lockupdateconnnums = new object();

        private static volatile object lockupdateoperatenums = new object();

        /// <summary>
        ///     监控信息
        /// </summary>
        private readonly Dictionary<string, LinkModel> monitorInfo;

        /// <summary>
        ///     似有构造函数
        /// </summary>
        private MonitorData()
        {
            monitorInfo = new Dictionary<string, LinkModel>();
        }

        /// <summary>
        ///     实体
        /// </summary>
        public static MonitorData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockhelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new MonitorData();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        ///     获取监控信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, LinkModel> getMonitorInfo()
        {
            return monitorInfo;
        }

        /// <summary>
        ///     更新某地址的链接数
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="Url">请求的url绝对地址</param>
        /// <param name="isadd">是否增加，否则就是减</param>
        public void UpdateUrlConnNums(string ip, string Url, bool isadd)
        {
            if (!monitorInfo.ContainsKey(ip))
            {
                lock (lockupdateconnnums)
                {
                    if (!monitorInfo.ContainsKey(ip))
                    {
                        var model = new LinkModel();
                        model.ClientIp = ip;
                        model.UrlInfoList = new Dictionary<string, UrlInfo>();
                        var url = new UrlInfo();
                        url.ConnNums = 0;
                        url.OperateNums = new Dictionary<string, long>();
                        model.UrlInfoList[Url] = url;

                        monitorInfo[ip] = model;
                    }
                }
            }

            if (monitorInfo.ContainsKey(ip))
            {
                lock (lockupdateconnnums)
                {
                    if (!monitorInfo[ip].UrlInfoList.ContainsKey(Url))
                    {
                        var url = new UrlInfo();
                        url.ConnNums = 0;
                        url.OperateNums = new Dictionary<string, long>();
                        monitorInfo[ip].UrlInfoList[Url] = url;
                    }

                    if (isadd)
                        monitorInfo[ip].UrlInfoList[Url].ConnNums += 1;
                    else
                        monitorInfo[ip].UrlInfoList[Url].ConnNums -= 1;

                    //if (this.monitorInfo[ip].UrlInfoList[Url].ConnNums < 0)
                    //    this.monitorInfo[ip].UrlInfoList[Url].ConnNums = 0;
                }
            }
        }

        /// <summary>
        ///     更新操作次数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="Url"></param>
        /// <param name="operateName"></param>
        public void UpdateOperateNums(string ip, string Url, string operateName)
        {
            if (!monitorInfo.ContainsKey(ip))
            {
                lock (lockupdateconnnums)
                {
                    if (!monitorInfo.ContainsKey(ip))
                    {
                        var model = new LinkModel();
                        model.ClientIp = ip;
                        model.UrlInfoList = new Dictionary<string, UrlInfo>();
                        var url = new UrlInfo();
                        url.ConnNums = 0;
                        url.OperateNums = new Dictionary<string, long>();
                        model.UrlInfoList[Url] = url;

                        monitorInfo[ip] = model;
                    }
                }
            }

            lock (lockupdateoperatenums)
            {
                if (!monitorInfo[ip].UrlInfoList.ContainsKey(Url))
                {
                    var url = new UrlInfo();
                    url.ConnNums = 0;
                    url.OperateNums = new Dictionary<string, long>();
                    monitorInfo[ip].UrlInfoList[Url] = url;
                }

                monitorInfo[ip].UrlInfoList[Url].OperateNums[operateName] =
                    monitorInfo[ip].UrlInfoList[Url].OperateNums.ContainsKey(operateName)
                        ? (monitorInfo[ip].UrlInfoList[Url].OperateNums[operateName] == long.MaxValue
                            ? 1
                            : monitorInfo[ip].UrlInfoList[Url].OperateNums[operateName] + 1)
                        : 1;
            }
        }
    }
}