//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.MprImport
//   文 件 名：MainForm.cs
//   创建时间：2019-07-25 9:26
//   作    者：
//   说    明：
//   修改时间：2019-07-25 9:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Service;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.MprImport
{
    public partial class MainForm : BaseForm
    {
        MprImportApp app = new MprImportApp();
        private ILogger log = LogManager.GetLogger(nameof(MainForm));
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            rtxtHomagCsvCsvPath.Text = MprImportSetting.Current.HomagCsvPath;

            rtxtHomagDataServer.Text = MprImportSetting.Current.HomagDataService;
            rtxtHomagDataDatabase.Text = MprImportSetting.Current.HomagDataDatabase;
            rtxtHomagDataUser.Text = MprImportSetting.Current.HomagDataUser;
            rtxtHomagDataPwd.Text = MprImportSetting.Current.HomagDataPwd;

            rtxtMprDownOnePath.Text = MprImportSetting.Current.OneMprPath;
            rtxtMprDownMprOneStoreDay.Text = MprImportSetting.Current.OneMprStorageDay.ToString();
            rtxtMprDownDoubleMprPath.Text = MprImportSetting.Current.DoubleMprPath;
            rtxtMprDownMprDoubleStoreDay.Text = MprImportSetting.Current.DoubleMprStorageDay.ToString();

            app.WriteMessageToHomagCsvMessage += App_WriteMessageToHomagCsvMessage;
            app.WriteMessageToHomagDataMessage += App_WriteMessageToHomagDataMessage;
            app.WriteMessageToMprDownMessage += App_WriteMessageToMprDownMessage;

            rtxtHomagCsvCsvPath.DataBindings.Add(new Binding("Enabled", rcbHomagCsvCsv, "Checked"));
            rbtnHomagCsvSelect.DataBindings.Add(new Binding("Enabled", rcbHomagCsvCsv, "Checked"));
            rtxtHomagCsvCsvStoreDay.DataBindings.Add(new Binding("Enabled", rcbHomagCsvCsv, "Checked"));

            rtxtHomagDataPwd.DataBindings.Add(new Binding("Enabled", rcbHomagDataServer, "Checked")); 
            rtxtHomagDataServer.DataBindings.Add(new Binding("Enabled", rcbHomagDataServer, "Checked"));
            rtxtHomagDataDatabase.DataBindings.Add(new Binding("Enabled", rcbHomagDataServer, "Checked"));
            rtxtHomagDataUser.DataBindings.Add(new Binding("Enabled", rcbHomagDataServer, "Checked"));
            rbtnHomagDataTest.DataBindings.Add(new Binding("Enabled", rcbHomagDataServer, "Checked"));
            
            rtxtMprDownOnePath.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));
            rtxtMprDownMprOneStoreDay.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));
            rtxtMprDownDoubleMprPath.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));
            rtxtMprDownMprDoubleStoreDay.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));
            rbtnMprDownOneSelect.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));
            rbtnMprDownDoubleSelect.DataBindings.Add(new Binding("Enabled", rcbMprDown, "Checked"));

            rcbHomagDataServer.Checked = MprImportSetting.Current.HomagDataServerEnable;
            rcbMprDown.Checked = MprImportSetting.Current.MprDownEnable;
            rcbHomagCsvCsv.Checked = MprImportSetting.Current.HomagCsvCsvEnable;
            rcbHomagCsvCsv.CheckStateChanged += rcbEnable_CheckStateChanged;
            rcbHomagDataServer.CheckStateChanged += rcbEnable_CheckStateChanged;
            rcbMprDown.CheckStateChanged += rcbEnable_CheckStateChanged;

        }

        private void App_WriteMessageToMprDownMessage(object sender, string e)
        {
            WriteMessageToMprDown(e);
        }

        private void App_WriteMessageToHomagDataMessage(object sender, string e)
        {
            WriteMessageToHomagData(e);
        }

        private void App_WriteMessageToHomagCsvMessage(object sender, string e)
        {
            WriteMessageToHomagCsv(e);
        }

        private void rbtnHomagCsvSelect_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(rtxtHomagCsvCsvPath.Text) && System.IO.Directory.Exists(rtxtHomagCsvCsvPath.Text))
            {
                dialog.SelectedPath = rtxtHomagCsvCsvPath.Text;
            }
            if (string.IsNullOrWhiteSpace(MprImportSetting.Current.HomagCsvPath))
            {
                MprImportSetting.Current.HomagCsvPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoads");
            }
            dialog.SelectedPath = MprImportSetting.Current.HomagCsvPath;
            dialog.Description = @"请选择文件路径";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            MprImportSetting.Current.HomagCsvPath = dialog.SelectedPath;
            MprImportSetting.Current.Save();

            rtxtHomagCsvCsvPath.Text = MprImportSetting.Current.HomagCsvPath;
        }

        private void rbtnMprDownOneSelect_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(rtxtMprDownOnePath.Text) && System.IO.Directory.Exists(rtxtMprDownOnePath.Text))
            {
                dialog.SelectedPath = rtxtMprDownOnePath.Text;
            }
            if (string.IsNullOrWhiteSpace(MprImportSetting.Current.OneMprPath))
            {
                MprImportSetting.Current.OneMprPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoads");
            }
            dialog.SelectedPath = MprImportSetting.Current.OneMprPath;
            dialog.Description = @"请选择文件路径";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            MprImportSetting.Current.OneMprPath = dialog.SelectedPath;
            rtxtMprDownOnePath.Text = MprImportSetting.Current.OneMprPath;
            MprImportSetting.Current.Save();
        }

        private void rbtnMprDownDoubleSelect_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(rtxtMprDownDoubleMprPath.Text) && System.IO.Directory.Exists(rtxtMprDownDoubleMprPath.Text))
            {
                dialog.SelectedPath = rtxtMprDownDoubleMprPath.Text;
            }
            if (string.IsNullOrWhiteSpace(MprImportSetting.Current.DoubleMprPath))
            {
                MprImportSetting.Current.DoubleMprPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoads");
            }
            dialog.SelectedPath = MprImportSetting.Current.DoubleMprPath;
            dialog.Description = @"请选择文件路径";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            MprImportSetting.Current.DoubleMprPath = dialog.SelectedPath;
            rtxtMprDownDoubleMprPath.Text = MprImportSetting.Current.DoubleMprPath;
            MprImportSetting.Current.Save();
        }

        private void rbtnMprDownExec_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtMprDownBatchNumber.Text))
            {
                RadMessageBox.Show(this, "批次不能为空");
                return;
            }
            if (string.IsNullOrEmpty(MprImportSetting.Current.OneMprPath) || string.IsNullOrEmpty(MprImportSetting.Current.DoubleMprPath))
            {
                RadMessageBox.Show(this, "请设置Mpr文件保存路径");
                return;
            }

            var batchCode = rtxtMprDownBatchNumber.Text.Trim();

            WaitForm.ShowWait(this, () =>
            {
                app.ImportMprDown(batchCode);
            });
        }

        private void rbtnHomagDataExec_Click(object sender, EventArgs e)
        {
            WaitForm.ShowWait(this, () =>
            {
                if (string.IsNullOrEmpty(rbtnHomagDataBatchNumber.Text))
                {
                    RadMessageBox.Show(this, "批次不能为空");
                    return;
                }
                var batchCode = rbtnHomagDataBatchNumber.Text.Trim();
                app.ImportHomagData(batchCode);

            });
        }

        private void rbtnHomagCsvExec_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtHomagCsvBatchNumber.Text))
            {
                RadMessageBox.Show(this, "批次不能为空");
                return;
            }

            WaitForm.ShowWait(this, () =>
            {
                var batchCode = rtxtHomagCsvBatchNumber.Text.Trim();
                app.ImportHomagCsv(batchCode);
            });
        }
        public void WriteMessageToHomagData(string text)
        {
            WriteMessage(rbtnHomagDataMessage, text);
        }
        public void WriteMessageToMprDown(string text)
        {
            WriteMessage(rtxtMprDownMessage, text);
        }

        public void WriteMessageToHomagCsv(string text)
        {
            WriteMessage(rtxtHomagCsvMessage, text);
        }

        private void WriteMessage(RadRichTextEditor ctl, string text)
        {
            log.Info(text);
            ctl.Invoke(new Action(() => { ctl.Text += $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {text}\r"; }));
        }

        private void rbtnHomagDataTest_Click(object sender, EventArgs e)
        {
            string connStr = $"Data Source={MprImportSetting.Current.HomagDataService};Initial Catalog={MprImportSetting.Current.HomagDataDatabase};" +
                             $"User ID={MprImportSetting.Current.HomagDataUser};Pwd={MprImportSetting.Current.HomagDataPwd}";
            using (var connection = new System.Data.SqlClient.SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    WriteMessageToHomagData($"连接成功");
                }
                catch (Exception exception)
                {
                    WriteMessageToHomagData($"连接失败"+exception.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void rtxtHomagDataServer_Leave(object sender, EventArgs e)
        {
            MprImportSetting.Current.HomagDataPwd = rtxtHomagDataPwd.Text.Trim();
            MprImportSetting.Current.HomagDataService = rtxtHomagDataServer.Text.Trim();
            MprImportSetting.Current.HomagDataUser = rtxtHomagDataUser.Text.Trim();
            MprImportSetting.Current.HomagDataDatabase = rtxtHomagDataDatabase.Text.Trim();
            MprImportSetting.Current.HomagCsvCsvStoreDay = int.Parse(rtxtHomagCsvCsvStoreDay.Text.Trim());
            MprImportSetting.Current.OneMprStorageDay = int.Parse(rtxtMprDownMprOneStoreDay.Text.Trim());
            MprImportSetting.Current.DoubleMprStorageDay = int.Parse(rtxtMprDownMprDoubleStoreDay.Text.Trim());
            MprImportSetting.Current.Save();
        }

        private void rcbEnable_CheckStateChanged(object sender, EventArgs e)
        {
            MprImportSetting.Current.HomagCsvCsvEnable = rcbMprDown.Checked;
            MprImportSetting.Current.HomagDataServerEnable = rcbHomagDataServer.Checked;
            MprImportSetting.Current.MprDownEnable = rcbMprDown.Checked;
            MprImportSetting.Current.Save();
        }
    }
}