// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：SpHostServiceInstaller.cs
//  创建时间：2016-11-11 12:50
//  作    者：
//  说    明：
//  修改时间：2017-07-10 12:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Logging;
using Topshelf.Runtime;
using Topshelf.Runtime.Windows;

#endregion

namespace NJIS.FPZWS.App.StartParameters
{
    public class SpHostServiceInstaller : IDisposable
    {
        private readonly HostConfigurator _hostConfigurator;
        private readonly Installer _installer;
        private readonly LogWriter _log = HostLogger.Get<SpHostServiceInstaller>();
        private readonly TransactedInstaller _transactedInstaller;

        public SpHostServiceInstaller(InstallHostSettings settings, HostConfigurator configurator)
        {
            _hostConfigurator = configurator;

            _installer = CreateInstaller(settings);

            _transactedInstaller = CreateTransactedInstaller(_installer);
        }

        public SpHostServiceInstaller(HostSettings settings, HostConfigurator configurator)
        {
            _hostConfigurator = configurator;

            _installer = CreateInstaller(settings);

            _transactedInstaller = CreateTransactedInstaller(_installer);
        }

        public void Dispose()
        {
            try
            {
                _transactedInstaller.Dispose();
            }
            finally
            {
                _installer.Dispose();
            }
        }

        public void InstallService(Action<InstallEventArgs> beforeInstall, Action<InstallEventArgs> afterInstall,
            Action<InstallEventArgs> beforeRollback, Action<InstallEventArgs> afterRollback)
        {
            if (beforeInstall != null)
                _installer.BeforeInstall += (sender, args) => beforeInstall(args);
            if (afterInstall != null)
                _installer.AfterInstall += (sender, args) => afterInstall(args);
            if (beforeRollback != null)
                _installer.BeforeRollback += (sender, args) => beforeRollback(args);
            if (afterRollback != null)
                _installer.AfterRollback += (sender, args) => afterRollback(args);

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            _transactedInstaller.Install(new Hashtable());
        }

        public void UninstallService(Action<InstallEventArgs> beforeUninstall, Action<InstallEventArgs> afterUninstall)
        {
            if (beforeUninstall != null)
                _installer.BeforeUninstall += (sender, args) => beforeUninstall(args);
            if (afterUninstall != null)
                _installer.AfterUninstall += (sender, args) => afterUninstall(args);

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            _transactedInstaller.Uninstall(null);
        }

        private Installer CreateInstaller(InstallHostSettings settings)
        {
            var installers = new Installer[]
            {
                ConfigureServiceInstaller(settings, settings.Dependencies, settings.StartMode),
                ConfigureServiceProcessInstaller(settings.Account, settings.Username, settings.Password)
            };

            foreach (var installer in installers)
            {
                var eventLogInstallers = installer.Installers.OfType<EventLogInstaller>().ToArray();
                foreach (var eventLogInstaller in eventLogInstallers)
                {
                    installer.Installers.Remove(eventLogInstaller);
                }
            }

            return CreateHostInstaller(settings, installers);
        }

        private Installer CreateInstaller(HostSettings settings)
        {
            var installers = new Installer[]
            {
                ConfigureServiceInstaller(settings, new string[] {}, HostStartMode.Automatic),
                ConfigureServiceProcessInstaller(ServiceAccount.LocalService, "", "")
            };

            return CreateHostInstaller(settings, installers);
        }

        private Installer CreateHostInstaller(HostSettings settings, Installer[] installers)
        {
            var arguments = " ";

            if (!string.IsNullOrEmpty(settings.InstanceName))
                arguments += string.Format(" -instance \"{0}\"", settings.InstanceName);

            if (!string.IsNullOrEmpty(settings.DisplayName))
                arguments += string.Format(" -displayname \"{0}\"", settings.DisplayName);

            if (!string.IsNullOrEmpty(settings.Name))
                arguments += string.Format(" -servicename \"{0}\"", settings.Name);

            var pairs = StartParametersExtensions.Commands(_hostConfigurator);
            if (pairs != null)
                foreach (var pair in pairs)
                {
                    _log.DebugFormat("Start parameter '{0}' with value '{1}' added to {2}", pair.Item1, pair.Item2,
                        string.IsNullOrEmpty(settings.InstanceName)
                            ? settings.ServiceName
                            : settings.ServiceName + "$" + settings.InstanceName);

                    arguments += string.Format(" -{0} \"{1}\"", pair.Item1, pair.Item2);
                }

            return new HostInstaller(settings, arguments, installers);
        }

        private static TransactedInstaller CreateTransactedInstaller(Installer installer)
        {
            var transactedInstaller = new TransactedInstaller();

            transactedInstaller.Installers.Add(installer);

            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
                throw new TopshelfException("Assembly.GetEntryAssembly() is null for some reason.");

            var path = string.Format("/assemblypath={0}", assembly.Location);
            string[] commandLine = {path};

            var context = new InstallContext(null, commandLine);
            transactedInstaller.Context = context;

            return transactedInstaller;
        }

        private static ServiceInstaller ConfigureServiceInstaller(HostSettings settings, string[] dependencies,
            HostStartMode startMode)
        {
            var installer = new ServiceInstaller
            {
                ServiceName = settings.ServiceName,
                Description = settings.Description,
                DisplayName = settings.DisplayName,
                ServicesDependedOn = dependencies
            };

            SetStartMode(installer, startMode);

            return installer;
        }

        private static void SetStartMode(ServiceInstaller installer, HostStartMode startMode)
        {
            switch (startMode)
            {
                case HostStartMode.Automatic:
                    installer.StartType = ServiceStartMode.Automatic;
                    break;

                case HostStartMode.Manual:
                    installer.StartType = ServiceStartMode.Manual;
                    break;

                case HostStartMode.Disabled:
                    installer.StartType = ServiceStartMode.Disabled;
                    break;

                case HostStartMode.AutomaticDelayed:
                    installer.StartType = ServiceStartMode.Automatic;
#if !NET35
                    installer.DelayedAutoStart = true;
#endif
                    break;
            }
        }

        private static ServiceProcessInstaller ConfigureServiceProcessInstaller(ServiceAccount account, string username,
            string password)
        {
            var installer = new ServiceProcessInstaller
            {
                Username = username,
                Password = password,
                Account = account
            };

            return installer;
        }
    }
}