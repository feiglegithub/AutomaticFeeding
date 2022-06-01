//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：Program.cs
//   创建时间：2018-11-07 11:08
//   作    者：
//   说    明：
//   修改时间：2018-11-07 11:08
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    internal class Program
    {
        public class Stu
        {
            public Stu(string name)
            {
                Name = name;
            }
            public string Name { get; set; }
        }
        [STAThread]
        private static void Main(string[] args)
        {

            var processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 1)
            {
                DialogResult result = MessageBox.Show(@"程序正在运行中!是否关闭旧程序！", @"警告", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    foreach (var process in processes)
                    {
                        if (process.Id == Process.GetCurrentProcess().Id) continue;

                        RadMessageBox.Show(string.Join(";", processes.Select(a => a.Id).ToArray()) + "_" +
                                        Process.GetCurrentProcess().Id);
                        process.Kill();
                    }
                }
                else
                {
                    //Application.Current.Shutdown(); // 关闭当前应用程序
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;

            var dlg = new LoginForm { StartPosition = FormStartPosition.CenterScreen };
            if (DialogResult.OK == dlg.ShowDialog())
            {
                Splasher.Show(typeof(SplashScreenForm));

                //Splasher.Status = "正在检查网络状况...";
                //if (!NetCheck.PingIpOrDomainName(DrillingSetting.Current.CheckIp, 
                //    DrillingSetting.Current.CheckTimeOut, DrillingSetting.Current.CheckCnt))
                //{
                //    MessageBox.Show(@"网络连接错误", @"系统错误", MessageBoxButtons.YesNo);
                //    Application.Exit();
                //}
                //else
                {
                    var mainform = new MainForm();
                    Application.Run(mainform);
                }

            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            //LogHelper.Error(ex.Exception);

            var message = string.Format("{0}\r\n操作发生错误，您需要退出系统么？", ex.Exception.Message);
            if (DialogResult.Yes == MessageBox.Show(message, @"系统错误", MessageBoxButtons.YesNo))
            {
                Application.Exit();
            }
        }
    }
}