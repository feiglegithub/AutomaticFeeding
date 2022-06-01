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
using WCS.OPC;

namespace WCS.Forms
{
    public partial class InWareFrm : Form
    {

        public InWareFrm()
        {
            InitializeComponent();
            this.Load += InWareFrm_Load;
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
            //DialogResult result = MessageBox.Show("是否退出？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
                this.Close();
            //}
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

        private void InWareFrm_Load(object sender, EventArgs e)
        {
            this.lbAmount.Text = "0";
            this.lbCode.Text = "";
            this.lbPiler.Text = "";

            var dt_code = WCSSql.GetProductCode();
            this.cboCode.DataSource = dt_code;
            this.cboCode.DisplayMember = "ProductCode";
            this.cboCode.ValueMember = "ProductCode";

            var dt = WCSSql.GetInWareList();

            this.dgvTask.DataSource = dt;
            this.lbAmount.Text = dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
                var row1 = dt.Rows[0];
                this.lbPiler.Text = row1["PilerNo"].ToString();
                this.lbCode.Text = row1["ProductCode"].ToString();
            }

            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var dt = WCSSql.GetInWareList();

            this.dgvTask.DataSource = dt;
            this.lbAmount.Text = dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
                var row1 = dt.Rows[0];
                this.lbPiler.Text = row1["PilerNo"].ToString();
                this.lbCode.Text = row1["ProductCode"].ToString();
            }
            else
            {
                this.lbPiler.Text = "";
                this.lbCode.Text = "";
            }

            //GT101前进按钮
            if (OpcHsc.GT101GoHeadCmd())
            {
                //OpcHsc.FeedBackGT101Cmd(2);
                if (!this.cboCode.Text.Equals(this.lbCode.Text))
                {
                    this.lbWarm.Visible = true;
                    OpcHsc.FeedBackGT101Cmd(3);
                }
                else
                {
                    this.lbWarm.Visible = false;
                    OpcHsc.FeedBackGT101Cmd(2);
                }
            }
        }
    }
}
