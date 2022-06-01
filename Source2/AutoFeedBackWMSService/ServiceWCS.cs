using AutoFeedBackWMSService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFeedBackWMSService
{
    partial class ServiceWCS : ServiceBase
    {
        AutoDoing ad = null;
        public ServiceWCS()
        {
            InitializeComponent();
            ad = new AutoDoing();
        }

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            Task.Factory.StartNew(DoWork, cancelTokenSource.Token);
            LogWrite.WriteLog("WCS自动过账服务已启动！");
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
            LogWrite.WriteLog("WCS自动过账服务已关闭！");
        }

        private void DoWork(object arg)
        {
            while (!cancelTokenSource.IsCancellationRequested)  // Worker thread loop
            {
                ad.Go();
                Thread.Sleep(2000);
            }
        }
    }
}
