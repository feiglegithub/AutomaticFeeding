// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：WaitForm.cs
//  创建时间：2018-07-23 18:00
//  作    者：
//  说    明：
//  修改时间：2018-04-13 11:50
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    public partial class WaitForm : BaseForm
    {
        private readonly DoWorkEventHandler _doWorkhandler;

        private readonly RunWorkerCompletedEventHandler _runWorkerCompleted;
        private readonly WaitExecutor _waitExecutor;
        private readonly BackgroundWorker _worker = new BackgroundWorker();

        public WaitForm()
        {
            InitializeComponent();
        }


        public WaitForm(DoWorkEventHandler doWorkhandler, RunWorkerCompletedEventHandler runWorkerCompleted)
        {
            InitializeComponent();
            _runWorkerCompleted = runWorkerCompleted;
            _doWorkhandler = doWorkhandler;

            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }


        public WaitForm(WaitExecutor wait, string message = "加载中...")
        {
            InitializeComponent();
            _waitExecutor = wait;
            dotsRingWaitingBarIndicatorElement1.Text = message;

            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _doWorkhandler?.Invoke(sender, e);

            if (_waitExecutor != null)
            {
                _waitExecutor.Execute();
                if (_waitExecutor != null) e.Result = _waitExecutor.Result;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseForm();

            _runWorkerCompleted?.Invoke(sender, e);

            _waitExecutor?.OnCallBack();
        }

        public void CloseForm()
        {
            Hide(); // 隐藏窗口
            if (ParentForm != null)
            {
                ParentForm.Controls.Remove(this); // 从父窗口中移除改窗口
                Close();
            }
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            radWaitingBar1.WaitingStarted += RadWaitingBar1_WaitingStarted;
            radWaitingBar1.WaitingStopped += RadWaitingBar1_WaitingStopped;

            Opacity = 0.5;
            radWaitingBar1.StartWaiting();
            _worker.RunWorkerAsync();
        }

        private void RadWaitingBar1_WaitingStopped(object sender, EventArgs e)
        {
        }

        private void RadWaitingBar1_WaitingStarted(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        public static WaitForm ShowWait(Form form)
        {
            var wait = new WaitForm()
            {
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };

            form.Controls.Add(wait);
            wait.Show();
            wait.BringToFront();
            return wait;
        }

        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="doWorkhandler"></param>
        /// <param name="runWorkerCompletedEventHandler"></param>
        public static WaitForm ShowWait(Form form, DoWorkEventHandler doWorkhandler,
            RunWorkerCompletedEventHandler runWorkerCompletedEventHandler)
        {
            var wait = new WaitForm(doWorkhandler, runWorkerCompletedEventHandler)
            {
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };

            form.Controls.Add(wait);
            wait.Show();
            wait.BringToFront();
            return wait;
        }

        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="waitExecutor"></param>
        /// <param name="message"></param>
        public static WaitForm ShowWait(Control form, WaitExecutor waitExecutor, string message = "加载中..")
        {
            var wait = new WaitForm(waitExecutor, message)
            {
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };
            form.Controls.Add(wait);
            wait.Show();
            wait.BringToFront();
            return wait;
        }


        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static WaitForm ShowWait(Control form, Action action, string message = "加载中..")
        {
            var waitForm = new WaitForm(new WaitExecutor(action), message)
            {
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };
            form.Controls.Add(waitForm);
            waitForm.Show();
            waitForm.BringToFront();
            return waitForm;
        }

        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        /// <param name="message"></param>
        public static WaitForm ShowWait(Control form, Action action, Action<object> callback, string message = "加载中..")
        {
            var wait = new WaitForm(new WaitExecutor(action, callback), message)
            {
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };
            form.Controls.Add(wait);
            wait.Show();
            wait.BringToFront();
            return wait;
        }
    }
}