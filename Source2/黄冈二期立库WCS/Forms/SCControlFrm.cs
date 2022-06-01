using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class SCControlFrm : Form
    {
        public SCControlFrm()
        {
            InitializeComponent();
            this.Load += SCControlFrm_Load;
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

        private void SCControlFrm_Load(object sender, EventArgs e)
        {
            var dt = WcsSqlB.GetDdjStatus();

            foreach (DataRow dr in dt.Rows)
            {
                var no = dr["No"].ToString();
                var status = dr["Status"].ToString();
                switch (no)
                {
                    case "1":
                        this.btn1.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "2":
                        this.btn2.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "3":
                        this.btn3.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "4":
                        this.btn4.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "5":
                        this.btn5.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "6":
                        this.btn6.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "7":
                        this.btn7.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "8":
                        this.btn8.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "9":
                        this.btn9.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "10":
                        this.btn10.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "11":
                        this.btn11.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    default:
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var no = btn.Tag.ToString();
            var msg = "";

            if (btn.BackColor == Color.LimeGreen)
            {
                msg = WcsSqlB.UpdateScStatus(btn.Text, 0, no);
                if (msg.Length == 0)
                {
                    btn.BackColor = Color.Red;
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
            else
            {
                msg = WcsSqlB.UpdateScStatus(btn.Text, 1, no);
                if (msg.Length == 0)
                {
                    btn.BackColor = Color.LimeGreen;
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }
    }
}
