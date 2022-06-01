using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WCS.Common;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class FRIDControl : Form
    {
        Dictionary<string, string> gobalExecs;
        public FRIDControl()
        {
            InitializeComponent();
            this.Load += FRIDControl_Load;
            gobalExecs = new Dictionary<string, string>();
            gobalExecs.Add("RFIDCA20011", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA20011\bin\Debug\RFIDCA20011.exe");
            gobalExecs.Add("RFIDCA20012", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA20012\bin\Debug\RFIDCA20012.exe");
            gobalExecs.Add("RFIDCA20021", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA20021\bin\Debug\RFIDCA20021.exe");
            gobalExecs.Add("RFIDCA20022", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA20022\bin\Debug\RFIDCA20022.exe");
            gobalExecs.Add("RFIDCA2007", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA2007\bin\Debug\RFIDCA2007.exe");
            gobalExecs.Add("RFIDCA2015", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA2015\bin\Debug\RFIDCA2015.exe");
            gobalExecs.Add("RFIDCA2017", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA2017\bin\Debug\RFIDCA2017.exe");
            gobalExecs.Add("RFIDCA2019", @"E:\Project\Source2\RFIDCA2019\bin\Debug\RFIDCA2019.exe");
            gobalExecs.Add("RFIDCA2010", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA2010\bin\Debug\RFIDCA2010.exe");
            gobalExecs.Add("RFIDCA2012", @"E:\Project\Source2\黄冈二期立库WCS\RFIDCA2012\bin\Debug\RFIDCA2012.exe");
        }

        #region 自定义窗体边框
        private void pic_MouseEnter(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;

            pic.BackColor = Color.FromArgb(8, 70, 95);
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            pic.BackColor = Color.FromArgb(0, 35, 50);
        }

        //退出
        private void pic_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        //最小化
        private void pic_minsize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //最大化
        private void pic_maxsize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private void panel_head_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }
        #endregion

        private void FRIDControl_Load(object sender, EventArgs e)
        {
            var msg = "";
            foreach(Control c in this.Controls)
            {
                if(c is Button)
                {
                    var btn = (Button)c;
                    if (c.Tag != null)
                    {
                        var ip = c.Tag.ToString().Split('|')[0];
                        if (ProcessHelper.ping(ip) == false)
                        {
                            //MessageBox.Show($"{btn.Text}的IP：{ip}Ping不通！请检查网络通讯！");
                            msg += $"{btn.Text}的IP：{ip}Ping不通！请检查网络通讯！\r\n";
                            btn.Enabled = false;
                        }
                        else
                        {
                            var exeName = c.Tag.ToString().Split('|')[1];
                            var rlt = ProcessHelper.SearchProc(exeName);
                            if (rlt)
                            {
                                btn.BackColor = Color.Lime;
                            }
                            else
                            {
                                btn.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
            }
        }
      
        private void button_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.BackColor == Color.Lime)
            {
                ProcessHelper.KillProc(btn.Tag.ToString().Split('|')[1]);
                btn.BackColor = Color.Red;
            }
            else if (btn.BackColor == Color.Red)
            {
                var rt = ProcessHelper.StartProc(gobalExecs[btn.Tag.ToString().Split('|')[1]]);
                btn.BackColor = rt ? Color.Lime : Color.Red;
            }
        }
    }
}
