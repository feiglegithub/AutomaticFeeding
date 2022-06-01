// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc
//  项目名称：NJIS.FPZWS.LineControl.WinCc.Core
//  文 件 名：WinCcApp.cs
//  创建时间：2017-08-25 11:18
//  作    者：
//  说    明：
//  修改时间：2017-10-11 8:29
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

#endregion

namespace NJIS.FPZWS.App
{
    public class BatApp : AppBase<BatApp>, IService
    {
        private ILogger log = LogManager.GetLogger(typeof(BatApp).Name);

        public BatApp()
        {
        }


        public override bool Start()
        {
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());
            log = LogManager.GetLogger(typeof(BatApp).Name);
            return base.Start();
        }

        public IServiceProvider ServiceProvider { get; set; }

        protected sealed override IConfig Config { get; set; }

        public override void Initializer()
        {
            var finder = new DirectoryReflectionFinder();
            var initializerTypes = finder.GetTypeFromAssignable<IAppInitializer>();

            foreach (var type in initializerTypes)
            {
                if (type == null)
                {
                    throw new Exception("Load AppInitializer failed");
                }
                var aps = Activator.CreateInstance(type) as IAppInitializer;
                if (aps == null)
                {
                    throw new Exception("App initialization failed");
                }

                aps.Initializer(Config);
            }


            base.Initializer();
        }

        public override void StartModular(IModularStarter instance)
        {
            log = LogManager.GetLogger(typeof(BatApp).Name);
            log.Info($"start module[{instance.GetType().FullName}]");
            try
            {
                base.StartModular(instance);
            }
            catch (Exception e)
            {
                log.Info($"startup module failed：{e.Message}");
                log.Error(e);
            }
            log.Info($"start module[{instance.GetType().FullName}] success.");
        }
    }

    public class IBatExecter : IModularStarter
    {
        private readonly string _batsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bats");
        private ILogger log = LogManager.GetLogger(typeof(IBatExecter).Name);

        private List<System.Diagnostics.Process> pgs = new List<System.Diagnostics.Process>();
        public void Start()
        {
            log.Info(_batsDirectory);
            if (!Directory.Exists(_batsDirectory)) return;

            var files = Directory.GetFiles(_batsDirectory, "*.bat").OrderBy(m => m);

            foreach (var file in files)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    try
                    {
                        log.Info($"Start file [{file}]");
                        //var wordProcess = new Process
                        //{
                        //    StartInfo =
                        //    {
                        //        FileName = "cmd",
                        //        UseShellExecute = false,
                        //        RedirectStandardInput = true,
                        //        RedirectStandardOutput = true,
                        //        RedirectStandardError = true,
                        //        CreateNoWindow = true
                        //    }
                        //};
                        //wordProcess.OutputDataReceived += WordProcess_OutputDataReceived;
                        //wordProcess.Start();//启动程序
                        //var cmdWriter = wordProcess.StandardInput;
                        //wordProcess.BeginOutputReadLine();
                        //var lines = File.ReadAllLines(file);
                        //foreach (var line in lines)
                        //{
                        //    cmdWriter.WriteLine(line);
                        //    cmdWriter.Flush();
                        //}

                        //log.Info($"PID=>[{wordProcess.Id}]");
                        //cmdWriter.Close();
                        //pgs.Add(wordProcess);
                        //wordProcess.WaitForExit();

                        var c = new Process()
                        {
                            StartInfo =
                            {
                                FileName = "cmd",
                                UseShellExecute = false,
                                RedirectStandardInput = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            }
                        };
                        c.StartInfo.FileName = file;
                        c.OutputDataReceived += WordProcess_OutputDataReceived;
                        if (c.Start())
                        {
                            c.BeginOutputReadLine();
                            log.Info($"启动bat文件{file}");
                            c.WaitForExit(60);
                        }

                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                        throw;
                    }
                });
            }
        }



        private void WordProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                log.Info(e.Data);
            }
        }

        public void Stop()
        {
            log.Info($"stop server{pgs.Count}");
            foreach (var process in pgs)
            {
                if (!process.HasExited)
                {
                    log.Info($"kill process [{process.Id}]");
                    process.Kill();
                }
            }
        }

        public StarterLevel Level { get; } = StarterLevel.High;
    }
}