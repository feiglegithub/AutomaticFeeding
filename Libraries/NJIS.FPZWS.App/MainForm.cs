// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：MainForm.cs
//  创建时间：2016-11-17 8:22
//  作    者：
//  说    明：
//  修改时间：2017-07-10 12:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NJIS.FPZWS.App.Settings;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Hosts;
using Topshelf.Runtime;
using Topshelf.Runtime.Windows;

#endregion

namespace NJIS.FPZWS.App
{
    public partial class MainForm : Form
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private WindowsHostEnvironment _env;
        private int _maxLine = 1000;
        private WindowsHostSettings _settings;
        private FileSystemWatcher _watcher;

        public MainForm()
        {
            InitializeComponent();
            cbMaxLine.SelectedIndex = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _env = new WindowsHostEnvironment(new HostConfiguratorImpl().RunAsLocalService());
            _settings = new WindowsHostSettings(AppSetting.Current.ServiceName,
                AppSetting.Current.InstanceName)
            {
                Description = AppSetting.Current.Description,
                DisplayName = AppSetting.Current.DisplayName,
                StartTimeOut = TimeSpan.FromSeconds(AppSetting.Current.StartTimeout),
                StopTimeOut = TimeSpan.FromSeconds(AppSetting.Current.StopTimeout)
            };
            ControlInit();
        }

        private void LogLitsenerInit()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
            }
            _watcher = new FileSystemWatcher(path);

            _watcher.Changed += Watcher_Changed;

            _watcher.EnableRaisingEvents = true;
        }


        private void ControlInit()
        {
            dtpStart.Value = DateTime.Now.Date;
            dtpEnd.Value = DateTime.Now.Date.AddDays(1);
            var files = GetFileList();
            cbFileName.DataSource = files;
            //cbFileName.SelectedIndex = 0;
            gbServiceControl.Text = string.Format("{1}({0})", AppSetting.Current.ServiceName, gbServiceControl.Text);
            InitServiceControl();
        }

        private void InitServiceControl()
        {
            btnInstall.Enabled = false;
            btnUnInstall.Enabled = false;
            btnStop.Enabled = false;
            btnStart.Enabled = false;
            btnRestart.Enabled = false;
            if (_env == null) return;
            if (!_env.IsServiceInstalled(AppSetting.Current.ServiceName))
            {
                btnInstall.Enabled = true;
            }
            else
            {
                btnUnInstall.Enabled = true;

                if (!_env.IsServiceStopped(AppSetting.Current.ServiceName))
                {
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                    btnRestart.Enabled = true;
                }
                else
                {
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                    btnRestart.Enabled = false;
                }
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            AppendText(Search(e.FullPath));
        }

        private List<string> GetFileList()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return new List<string>();
            }
            return Directory.GetFiles(path).Select(m => m.Substring(path.Length + 1)).ToList();
        }


        private void cbMaxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaxLine.Text.Trim() == "ALL")
            {
                _maxLine = int.MaxValue;
            }
            else
            {
                _maxLine = int.Parse(cbMaxLine.Text.Trim());
            }
        }

        private List<string> Search(string fileFullName, string search = "")
        {
            do
            {
                try
                {
                    if (!File.Exists(fileFullName)) return new List<string>();

                    var texts = File.ReadAllLines(fileFullName).Where(m => Regex.IsMatch(m, search)).ToList();
                    if (texts.Count > _maxLine)
                    {
                        texts = texts.Skip(texts.Count - _maxLine).Take(_maxLine).ToList();
                    }
                    return texts;
                }
                catch (IOException )
                {
                }
            } while (true);
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            var fullName = Path.Combine(path, cbFileName.Text);
            AppendText(Search(fullName, txtContent.Text.Trim()), txtContent.Text.Trim());
        }

        private void AppendText(List<string> lines, string search = "")
        {
            if (lines == null || lines.Count <= 0) return;

            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<List<string>, string>(AppendText), lines, search);
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var line in lines)
                    {
                        richTextBox1.Append(line, true);
                    }
                }
                else
                {
                    foreach (var line in lines)
                    {
                        var s = Regex.Split(line, search);
                        for (var i = 0; i < s.Length; i++)
                        {
                            richTextBox1.Append(s[i], false);
                            if (i != s.Length - 1)
                            {
                                richTextBox1.AppendTextColorful(search, Color.Red, false);
                            }
                        }
                        richTextBox1.Append("");
                    }
                }
            }
        }


        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(@"Non-administrator status running, unable to install the service!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_env.IsServiceInstalled(AppSetting.Current.ServiceName))
            {
                MessageBox.Show(this, string.Format("service {0} has been installed", AppSetting.Current.ServiceName),
                    @"information");
            }
            else
            {
                var install = new InstallHost(_env, _settings,
                    HostStartMode.Automatic, new string[0],
                    new Credentials(null, null, ServiceAccount.LocalSystem),
                    new List<Action<InstallHostSettings>>(),
                    new List<Action<InstallHostSettings>>(),
                    new List<Action<InstallHostSettings>>(),
                    new List<Action<InstallHostSettings>>(), false);

                var ret = install.Run();
                if (ret == TopshelfExitCode.Ok)
                {
                    MessageBox.Show(this,
                        string.Format("Installation service \"{0}\" success", AppSetting.Current.ServiceName),
                        @"information");
                }
            }

            InitServiceControl();
        }

        private void btnUnInstall_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(@"Operation of non-administrator, unable to uninstall the service!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!_env.IsServiceInstalled(AppSetting.Current.ServiceName))
            {
                MessageBox.Show(this,
                    string.Format("service {0} does not exist", AppSetting.Current.ServiceName),
                    @"information");
            }
            else
            {
                var unInstall = new UninstallHost(_env, _settings, new List<Action>(),
                    new List<Action>(), false);
                var ret = unInstall.Run();
                if (ret == TopshelfExitCode.Ok)
                {
                    MessageBox.Show(this,
                        string.Format("Uninstall service \"{0}\" success ", AppSetting.Current.ServiceName),
                        @"information");
                }
            }

            InitServiceControl();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(@"Non-administrator operation, unable to start the service!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var start = new StartHost(_env, _settings);
            var ret = start.Run();
            if (ret == TopshelfExitCode.StartServiceFailed)
            {
                MessageBox.Show(this,
                    string.Format("Start service \"{0}\" success ", AppSetting.Current.ServiceName), @"information");
            }

            InitServiceControl();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(@"Operation of non-administrator, unable to stop service!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var stop = new StopHost(_env, _settings);
            var ret = stop.Run();
            if (ret == TopshelfExitCode.Ok)
            {
                MessageBox.Show(this,
                    string.Format("Stop service \"{0}\" success", AppSetting.Current.ServiceName), @"information");
            }

            InitServiceControl();
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            LogLitsenerInit();
        }

        /// <summary>
        ///     判断当前程序是否以管理员身份运行
        /// </summary>
        /// <returns></returns>
        private bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        ///     以管理员身份重启程序
        /// </summary>
        private static void ReStart()
        {
            var psi = new ProcessStartInfo();
            psi.FileName = Application.ExecutablePath;
            psi.Verb = "runas";
            try
            {
                Process.Start(psi);

                //Application.Exit()后，程序还是有消息循环的，直到退出事件处理完成才会将应用程序退出
                //Environment.Exit(0) 则是直接断掉线程，类似任务管理器的结束进程。

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            new SetForm().ShowDialog(this);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(@"Operation of non-administrator, unable to restart service!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var stop = new StopHost(_env, _settings);
            var ret = stop.Run();
            if (ret == TopshelfExitCode.Ok)
            {
                var start = new StartHost(_env, _settings);
                ret = start.Run();
                if (ret == TopshelfExitCode.Ok)
                {
                    MessageBox.Show(this,
                        string.Format("Stop service \"{0}\" success", AppSetting.Current.ServiceName), @"information");
                }
            }

            InitServiceControl();
        }
    }

    public static class RichTextBoxExtension
    {
        public static void AppendTextColorful(this RichTextBox rtBox, string text, Color color, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            rtBox.SelectionStart = rtBox.TextLength;
            rtBox.SelectionLength = 0;
            rtBox.SelectionColor = color;
            rtBox.AppendText(text);
            rtBox.SelectionColor = rtBox.ForeColor;
        }

        public static void Append(this RichTextBox rtBox, string text, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            rtBox.AppendText(text);
        }


        public static void Append(this RichTextBox rtBox, List<string> lines, bool addNewLine = true)
        {
            if (lines == null || lines.Count <= 0) return;
            foreach (var line in lines)
            {
                rtBox.Append(line, addNewLine);
            }
        }
    }
}