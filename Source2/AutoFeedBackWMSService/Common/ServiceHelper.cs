using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AutoFeedBackWMSService.Common
{
    public class ServiceHelper
    {
        /// <summary>
        ///     检查服务存在的存在性
        /// </summary>
        /// <param name="nameService">服务名</param>
        /// <returns>存在返回 true,否则返回 false;</returns>
        public static bool IsServiceExisted(string nameService)
        {
            var services = ServiceController.GetServices();
            foreach (var s in services)
            {
                if (s.ServiceName.ToLower() == nameService.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     安装Windows服务
        /// </summary>
        /// <param name="filepath">程序文件路径</param>
        public static void InstallService(string filepath)
        {
            IDictionary stateSaver = new Hashtable();
            var installer = new AssemblyInstaller
            {
                UseNewContext = true,
                Path = filepath
            };
            stateSaver.Clear();
            installer.Install(stateSaver);
            installer.Commit(stateSaver);
            installer.Dispose();
        }

        /// <summary>
        ///     卸载Windows服务
        /// </summary>
        /// <param name="filepath">程序文件路径</param>
        public static void UnInstallService(string filepath)
        {
            var installer = new AssemblyInstaller
            {
                UseNewContext = true,
                Path = filepath
            };
            installer.Uninstall(null);
            installer.Dispose();
        }

        /// <summary>
        ///     检查Windows服务是否在运行
        /// </summary>
        /// <param name="serviceName">程序的服务名</param>
        public static bool IsRunning(string serviceName)
        {
            var isRun = false;
            try
            {
                if (!IsServiceExisted(serviceName))
                {
                    return false;
                }
                var sc = new ServiceController(serviceName);
                if (sc.Status == ServiceControllerStatus.StartPending ||
                    sc.Status == ServiceControllerStatus.Running)
                {
                    isRun = true;
                }
                sc.Close();
            }
            catch
            {
                isRun = false;
            }
            return isRun;
        }

        /// <summary>
        ///     启动Windows服务
        /// </summary>
        /// <param name="serviceName">程序的服务名</param>
        /// <returns>启动成功返回 true,否则返回 false;</returns>
        public static bool StartService(string serviceName)
        {
            var sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.StopPending
                )
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));
            }
            sc.Close();
            return true;
        }

        /// <summary>
        ///     停止Windows服务
        /// </summary>
        /// <param name="serviceName">程序的服务名</param>
        /// <returns>停止成功返回 true,否则返回 false;</returns>
        public static bool StopService(string serviceName)
        {
            var sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Running ||
                sc.Status == ServiceControllerStatus.StartPending)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));
            }
            sc.Close();
            return true;
        }

        /// <summary>
        ///     重启Windows服务
        /// </summary>
        /// <param name="serviceName">程序的服务名</param>
        /// <returns>重启成功返回 true,否则返回 false;</returns>
        public static bool RefreshService(string serviceName)
        {
            return StopService(serviceName) && StartService(serviceName);
        }
    }
}
