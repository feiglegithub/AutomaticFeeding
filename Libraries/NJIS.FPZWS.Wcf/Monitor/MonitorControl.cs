// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：MonitorControl.cs
//  创建时间：2017-12-29 14:20
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using NJIS.FPZWS.Wcf.Service;

#endregion

namespace NJIS.FPZWS.Wcf.Monitor
{
    [Server("/njis/wcf/monitor/imonitorcontrol")]
    public class MonitorControl : IMonitorControl
    {
        /// <summary>
        ///     系统内存大小
        /// </summary>
        private static double _memcount = 1;

        private static volatile object _lockhelper = new object();

        /// <summary>
        ///     获取wcf监控信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, LinkModel> GetMonitorInfo(out PcData pcdata, out double memCount)
        {
            var currentProcess = Process.GetCurrentProcess();
            pcdata = new PcData();
            try
            {
                pcdata.ProcessId = currentProcess.Id;
            }
            catch
            {
            }
            try
            {
                //创建性能计数器
                using (var pCpu = new PerformanceCounter("Process", "% Processor Time", currentProcess.ProcessName))
                {
                    //注意除以CPU数量
                    pcdata.Cpu = pCpu.NextValue() / Environment.ProcessorCount;
                }
            }
            catch
            {
            }
            try
            {
                using (var pMem = new PerformanceCounter("Process", "Working Set - Private",
                    currentProcess.ProcessName))
                {
                    pcdata.Mem = pMem.NextValue() / (1024 * 1024);
                }
            }
            catch
            {
            }
            try
            {
                pcdata.ThreadCount = currentProcess.Threads.Count;
            }
            catch
            {
            }
            try
            {
                #region 获取系统总内存

                if (_memcount == 1)
                {
                    lock (_lockhelper)
                    {
                        if (_memcount == 1)
                        {
                            var searcher = new ManagementObjectSearcher(); //用于查询一些如系统信息的管理对象 
                            searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new[] {"Capacity"}); //设置查询条件 
                            var collection = searcher.Get(); //获取内存容量 
                            var em = collection.GetEnumerator();

                            long capacity = 0;
                            while (em.MoveNext())
                            {
                                var baseObj = em.Current;
                                if (baseObj.Properties["Capacity"].Value != null)
                                {
                                    try
                                    {
                                        capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            _memcount = capacity / (double) 1024 / 1024;
                        }
                    }
                }

                #endregion
            }
            catch
            {
            }

            memCount = _memcount;
            return MonitorData.Instance.getMonitorInfo();
        }
    }
}