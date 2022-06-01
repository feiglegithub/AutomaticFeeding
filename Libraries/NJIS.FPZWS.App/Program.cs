// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.App
//  文 件 名：Program.cs
//  创建时间：2016-11-17 8:22
//  作    者：
//  说    明：
//  修改时间：2017-07-15 16:55
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Management;
using System.ServiceProcess;
using System.Windows.Forms;
using NJIS.FPZWS.App.Settings;
using NJIS.FPZWS.Common;
using Topshelf;

#endregion

namespace NJIS.FPZWS.App
{
    internal class Program : AppBase<Program>
    {
        public static bool IsServiceMode
        {
            get { return !Environment.UserInteractive; }
        }

        private static void Main(string[] args)
        {
            //if (true)
            if (!IsServiceMode)
            {
                if (args.Length > 0 && args[0].ToLower() == "debug")
                {
                    var app = GetApp();
                    if (app != null)
                    {
                        app.Start();
                    }
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadExit += Application_ThreadExit;
                Application.Run(new MainForm());
            }
            else
            {
                InitializeServiceServiceHostFactory();
            }
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {

        }

        private static IService GetApp()
        {
            var type = Type.GetType(AppSetting.Current.App);
            if (type == null)
            {
                return null;
            }
            var app = (IService)Activator.CreateInstance(type, null);
            return app;
        }

        private static void InitializeServiceServiceHostFactory()
        {
            var app = GetApp();
            if (app == null) return;

            HostFactory.Run(c =>
            {
                //c.EnableStartParameters();
                //c.UseAutofacContainer(container);
                c.Service<IService>(s =>
                {
                    s.ConstructUsing(name => app);
                    s.WhenStarted((service, control) =>
                    {
                        var res = false;
                        try
                        {
                            res = service.Start();
                        }
                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server.txt"), ex.ToString());
                        }
                        return res;
                    });
                    s.WhenStopped((service, control) =>
                    {
                        var res = false;
                        try
                        {
                            res = service.Stop();
                        }
                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server.txt"), ex.ToString());
                        }
                        return res;
                    });
                });

                c.StartAutomatically(); // 自动启动
                c.RunAsLocalSystem(); // 本地系统服务
                //c.RunAsLocalService();                // 本地服务
                //c.EnablePauseAndContinue();           // 启用暂停继续
                c.AfterInstall(s =>
                {
                    var coOptions = new ConnectionOptions { Impersonation = ImpersonationLevel.Impersonate };
                    var mgmtScope = new ManagementScope(@"root\CIMV2", coOptions);
                    mgmtScope.Connect();
                    var service = new ServiceController(s.ServiceName);
                    var manage = new ManagementObject(string.Format("Win32_Service.Name='{0}'", service.ServiceName));
                    var inParam = manage.GetMethodParameters("Change");
                    inParam["DesktopInteract"] = true;
                    manage.InvokeMethod("Change", inParam, null);
                    service.Start();
                });
                c.SetServiceName(AppSetting.Current.ServiceName);
                c.SetDescription(AppSetting.Current.Description);
                c.SetInstanceName(AppSetting.Current.InstanceName);
                c.SetDisplayName(AppSetting.Current.DisplayName);
                c.SetStartTimeout(TimeSpan.FromSeconds(AppSetting.Current.StartTimeout));
                c.SetStopTimeout(TimeSpan.FromSeconds(AppSetting.Current.StartTimeout));
                c.SetHelpTextPrefix(AppSetting.Current.HelpTextPrefix);
            });
        }
    }
}