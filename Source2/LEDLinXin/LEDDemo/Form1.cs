using LXLedControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEDDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Thread thread = new Thread(new ThreadStart(SendLedContent));
            thread.IsBackground = true;
            thread.Start();

            m_ipLst = new List<string>();
            m_disIpLst = new List<string>();
            m_logLst = new List<string>();
        }

        List<string> m_ipLst = null;
        List<string> m_disIpLst = null;
        List<string> m_logLst = null;
        string m_sendContent = string.Empty;
        bool m_isStart = false;


        void SendLedContent()
        {
            while (true)
            {
                if (!m_isStart)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                try
                {
                    if (m_ipLst.Count > 0 && !string.IsNullOrWhiteSpace(m_sendContent))
                    {
                        for (int i = 0; i < m_ipLst.Count; i++)
                        {
                            var item = m_ipLst[i];
                            var msg = string.Empty;
                            var result = LXLedCenter.sendTextToLED(item, m_sendContent, out msg);
                            if (result)
                            {
                                if (m_disIpLst.Contains(item))
                                    m_disIpLst.Remove(item);
                                WriteLog($"发送LED成功,IP:{item},发送时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            }
                            else
                            {
                                if (!m_disIpLst.Contains(item))
                                    m_disIpLst.Add(item);
                                WriteLog($"发送LED失败,IP:{item},异常原因:{msg},发送时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        TxtDisConnIP.Text = string.Join("\r\n", m_disIpLst);
                    }));
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message);
                    Thread.Sleep(2000);
                }
            }
        }

        void WriteLog(string str)
        {
            DH.FileLog.StatisLog.LogMessage(str);
            m_logLst.Add(str);
            if (m_logLst.Count > 400)
                m_logLst.RemoveRange(0, 100);
            var logs = new string[m_logLst.Count];
            m_logLst.CopyTo(logs);
            logs.Reverse();
            this.Invoke(new Action(() =>
            {
                TxtLog.Text = string.Join("\r\n", logs);
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "开始")
                m_isStart = true;
            else
                m_isStart = false;
            button3.Text = m_isStart ? "暂停" : "开始";
        }

        //初始化
        private void button1_Click(object sender, EventArgs e)
        {
            m_isStart = false;
            m_ipLst.Clear();
            m_disIpLst.Clear();
            try
            {
                if (string.IsNullOrWhiteSpace(TxtIP.Text))
                    throw new Exception("IP地址不能为空");
                var ips = TxtIP.Text.Split('\n');
                if (ips != null && ips.Count() > 0)
                {
                    foreach (var ip in ips)
                    {
                        m_ipLst.Add(ip.Trim());
                    }
                }
                if (string.IsNullOrWhiteSpace(TxtSendContent.Text))
                    throw new Exception("发送内容不能为空");
                m_sendContent = TxtSendContent.Text.Trim();
                MessageBox.Show("初始化成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}