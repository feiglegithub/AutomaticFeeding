using AutoFeedBackWMSService.Common;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace AutoFeedBackWMSService
{
    public static class Program
    {
        public static bool IsServiceMode
        {
            get { return !Environment.UserInteractive; }
        }

        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                if (arg == "/install")
                {
                    ServiceHelper.InstallService(filePath);
                    return;
                }
                if (arg == "/uninstall")
                {
                    ServiceHelper.UnInstallService(filePath);
                    return;
                }
                if (arg == "/debug")
                {
                    //var ad = new AutoDoing();
                    //ad.Go();
                    //var lh = new LEDHelper();
                    //lh.SendAll();
                }
            }

            if (IsServiceMode)
            {
                // 运行服务对象
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new ServiceWCS()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
            }
        }
    }
}
