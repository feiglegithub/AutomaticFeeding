using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using LEDControl;

namespace LEDTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ledCenter = new MiniLedCenter();

            var ledIP = TxtIP.Text.Trim();
            var ledPort = short.Parse(TxtPort.Text.Trim());
            bool result = ledCenter.LED_MC_NetInitial(ledIP, ledPort);
            if (!result)
            {
                MessageBox.Show("初始化失败");
                return;
            }
        }

        MiniLedCenter ledCenter = null;

        public void button1_Click(object sender, EventArgs e)
        {
            var result = ledCenter.LED_MC_ShowString(TxtContent1.Text.Trim(), TxtContent2.Text.Trim(), TxtContent3.Text.Trim());
            if (!result)
            {
                MessageBox.Show("发送失败");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = TxtContent1.Text.Trim();
            var result = ledCenter.LED_MC_TxtToXMPXFile(str);
            if (!result)
            {
                MessageBox.Show("发送失败");
            }
        }
    }
}
