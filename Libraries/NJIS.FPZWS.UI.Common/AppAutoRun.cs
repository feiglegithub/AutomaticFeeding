// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：AppAutoRun.cs
//  创建时间：2018-09-01 17:31
//  作    者：
//  说    明：
//  修改时间：2018-09-01 16:12
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NJIS.FPZWS.UI.Common
{
    public class AppAutoRun
    {
        /// <summary>
        ///     添加到注册表中
        /// </summary>
        /// <param name="app">应用程序</param>
        public void AutoRunAfterStart(string app)
        {
            //获取当前应用程序的路径
            var localPath = Application.ExecutablePath;
            if (!File.Exists(localPath)) //判断指定文件是否存在
                return;
            var reg = Registry.LocalMachine;
            var run = reg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //判断注册表中是否存在当前名称和值
            if (run.GetValue(app) == null)
            {
                try
                {
                    run.SetValue(app, localPath);
                    reg.Close();
                }
                catch (Exception ex)
                {
                    var eventLog = new EventLog("LogName");
                    eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        /// <summary>
        ///     删除注册表钟的特定值
        /// </summary>
        public void DeleteSubKey(string app)
        {
            var reg = Registry.LocalMachine;
            var run = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                run.DeleteValue(app);
                reg.Close();
            }
            catch (Exception ex)
            {
                var eventLog = new EventLog("LogName");
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}