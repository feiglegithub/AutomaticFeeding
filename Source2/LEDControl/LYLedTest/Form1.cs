using LYLedControl;
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

namespace LYLedTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short LedId = 0;
            string IP = "192.168.1.100";
            string LedText = "     司米厨柜欢迎您\r\n   Welcome to Schmidt\r\n Bienvenue chez Schmidt\r\n   " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + Week();
            var rlt = SendTextToLED(LedId, IP, LedText, 27694);
            if (rlt)
            {
                MessageBox.Show("发送成功！");
            }
        }

        public static string Week()
        {
            return new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" }[Convert.ToInt32(DateTime.Now.DayOfWeek)];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //short LedId = 6205;
            string IP = "192.168.1.139";
            string initText = "                                                                                                                                                                            ";
            bool result = MiniLedCenter.sendTextToLED(0, IP, initText, 8888);
            var rlt = SendTextToLED(0, IP, richTextBox1.Text, 8888);
            //string LedText = string.Empty;
            //for (int i = 0; i < richTextBox1.Lines.Length; i++)
            //{
            //    LedText += richTextBox1.Lines[i] + "\r\n";
            //}
            //SendTextToLED(LedId, IP, LedText); ;
        }

        public bool SendTextToLED(short deviceId, string ip, string text, short port)
        {
            //string initText = "Hello 索菲亚智能家居！Hello 索菲亚智能家居！Hello 索菲亚智能家居！Hello 索菲亚智能家居！Hello 索菲亚智能家居！Hello 索菲亚智能家居！Hello 索菲亚智能家居！";
            //Thread.Sleep(120);
            //bool result = MiniLedCenter.sendTextToLED(deviceId, ip, initText, Convert.ToInt16("6" + deviceId));
            bool result = MiniLedCenter.sendTextToLED(deviceId, ip, text, port);
            //if (result)
            //{
            //    //Thread.Sleep(120);
            //    result = MiniLedCenter.sendTextToLED(deviceId, ip, initText, 27694);
            //}
            return result;
        }

        int i = 0;
        string[] arr = { "192.168.1.125", "192.168.1.126", "192.168.1.127", "192.168.1.128", "192.168.1.129", "192.168.1.130" };
        private void timer1_Tick(object sender, EventArgs e)
        {
            //i++;
            //if (i % 10 == 0)
            //{
            string initText = "                                                                                                                                                                            ";
            //var dts = DateTime.Now;
            //var rlt1 = MiniLedCenter.sendTextToLED(0, "192.168.1.126", initText, 8888);
            //var rlt2 = MiniLedCenter.sendTextToLED(0, "192.168.1.125", initText, 8888);
            //var rlt3 = MiniLedCenter.sendTextToLED(0, "192.168.1.127", initText, 8888);
            //var rlt4 = MiniLedCenter.sendTextToLED(0, "192.168.1.128", initText, 8888);
            foreach (string str in arr)
            {
                bool rlt1 = false;
                bool rlt2 = false;
                //Task t = new Task(() =>
                // {
                rlt1 = MiniLedCenter.sendTextToLED(0, str, initText, 8888);
                rlt2 = MiniLedCenter.sendTextToLED(0, str, "索菲亚智能仓库", 8888);
                //});
                if (rlt1 && rlt2)
                {
                    this.richTextBox1.AppendText($"{str}发送成功！\r\n");
                }
                else
                {
                    this.richTextBox1.AppendText($"{str}发送失败！\r\n");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }
    }
}
