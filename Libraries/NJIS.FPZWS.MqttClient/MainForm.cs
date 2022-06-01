//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.WinCc.Sorting
//   项目名称：NJIS.FPZWS.MqttClient
//   文 件 名：MainForm.cs
//   创建时间：2019-04-28 19:29
//   作    者：
//   说    明：
//   修改时间：2019-04-28 19:29
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Windows.Forms;
using NJIS.FPZWS.MqttClient.Setting;

namespace NJIS.FPZWS.MqttClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtIp.Text = TaskCenterSetting.Current.IP;
            txtPort.Text = TaskCenterSetting.Current.Port.ToString();
            txtUser.Text = TaskCenterSetting.Current.User;
            txtPassword.Text = TaskCenterSetting.Current.Password;
            txtClientId.Text = TaskCenterSetting.Current.ClientId;
            txtTopic.Text = "";
            txtMsg.Text = "";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIp.Text.Trim()))
            {
                MessageBox.Show(this, @"Ip 不能为空！", @"提示");
                return;
            }
            if (string.IsNullOrEmpty(txtPort.Text.Trim()))
            {
                short port = 1883;
                if (!short.TryParse(txtPort.Text.Trim(), out port))
                {
                    MessageBox.Show(this, @"Port 格式错误！", @"提示");
                    return;
                }
                MessageBox.Show(this, @"Port 不能为空！", @"提示");
                return;
            }
            if (string.IsNullOrEmpty(txtClientId.Text.Trim()))
            {
                MessageBox.Show(this, @"ClientId 不能为空！", @"提示");
                return;
            }

            TaskCenterSetting.Current.IP = txtIp.Text.Trim();
            TaskCenterSetting.Current.Port = short.Parse(txtPort.Text.Trim());
            TaskCenterSetting.Current.User = txtUser.Text.Trim();
            TaskCenterSetting.Current.Password = txtPassword.Text.Trim();
            TaskCenterSetting.Current.ClientId = txtClientId.Text.Trim();
            if (!string.IsNullOrEmpty(txtTopic.Text.Trim()))
            {
                MqttManager.Current.Subscribe(txtTopic.Text.Trim(), new MqttHander((msg) =>
                {
                    txtMsg.Invoke(new Action(() => { txtMsg.Text = $"{DateTime.Now.ToString("HH:mm:ss:fff")}:\r\n" + txtMsg.Text + "\r\n"; }));
                }));
            }


            TaskCenterSetting.Current.Save();
            MqttManager.Current.Connect();
            btnDisConnect.Enabled = true;
            btnConnect.Enabled = false;
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            MqttManager.Current.Disconnect();
            btnDisConnect.Enabled = false;
            btnConnect.Enabled = true;
        }
    }
}