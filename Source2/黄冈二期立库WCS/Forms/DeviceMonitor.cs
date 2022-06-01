using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class DeviceMonitor : Form
    {
        public DeviceMonitor()
        {
            InitializeComponent();
            AutoScales(this);

            this.Activated += DeviceMonitor_Activated;
            this.Deactivate += DeviceMonitor_Deactivate;
        }

        private void DeviceMonitor_Deactivate(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void DeviceMonitor_Activated(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        #region 控件大小随像素分辨率自适应
        //这里是让所有的控件随窗体的变化而变化
        public void AutoScales(Form frm)
        {
            frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
            frm.SizeChanged += new EventHandler(MainFrm_SizeChanged);
        }

        void MainFrm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                string[] tmp = new string[2];
                tmp = ((Form)sender).Tag.ToString().Split(',');
                if ((int)((Form)sender).Width > 200)
                {
                    float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
                    float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);

                    ((Form)sender).Tag = ((Form)sender).Width.ToString() + "," + ((Form)sender).Height;

                    foreach (Control control in ((Form)sender).Controls)
                    {
                        control.Scale(new SizeF(width, heigth));
                    }
                }
            }
            catch
            {
            }

        }
        #endregion

        //设备监控
        private void timer1_Tick(object sender, EventArgs e)
        {
            DdjPaint();
            DevicePaint();
            BindOutInfo();
            BindDelayTask();
        }

        private void BindOutInfo()
        {
            var dt = WcsSqlB.GetOutInfo();
            this.lb1001.Text = dt.Rows[0]["strcn"].ToString();
            this.lb1002.Text = dt.Rows[1]["strcn"].ToString();
            this.lb1003.Text = dt.Rows[2]["strcn"].ToString();
            this.lb1004.Text = dt.Rows[3]["strcn"].ToString();
            this.lb1005.Text = dt.Rows[4]["strcn"].ToString();
            this.lb1006.Text = dt.Rows[5]["strcn"].ToString();
            this.lb1007.Text = dt.Rows[6]["strcn"].ToString();
            this.lb1008.Text = dt.Rows[7]["strcn"].ToString();
            this.lb1009.Text = dt.Rows[8]["strcn"].ToString();
            this.lb1010.Text = dt.Rows[9]["strcn"].ToString();
            this.lb1011.Text = dt.Rows[10]["strcn"].ToString();
            this.lb1012.Text = dt.Rows[11]["strcn"].ToString();
            this.lb1013.Text = dt.Rows[12]["strcn"].ToString();
            this.lb1014.Text = dt.Rows[13]["strcn"].ToString();
            this.lb1015.Text = dt.Rows[14]["strcn"].ToString();
            this.lb1016.Text = dt.Rows[15]["strcn"].ToString();
            this.lb1017.Text = dt.Rows[16]["strcn"].ToString();
            this.lb1018.Text = dt.Rows[17]["strcn"].ToString();
            this.lb1019.Text = dt.Rows[18]["strcn"].ToString();
        }

        private void BindDelayTask()
        {
            var dt = WcsSqlB.GetTaskDelay();
            if (dt.Rows.Count == 3)
            {
                this.lbTask1.Text = $"任务号：{dt.Rows[0]["SeqID"].ToString()}，状态：{dt.Rows[0]["NwkStatus"].ToString()}，托盘号：{dt.Rows[0]["NPalletID"].ToString()}，执行时间：{dt.Rows[0]["T"].ToString()}";
                this.lbTask2.Text = $"任务号：{dt.Rows[1]["SeqID"].ToString()}，状态：{dt.Rows[1]["NwkStatus"].ToString()}，托盘号：{dt.Rows[1]["NPalletID"].ToString()}，执行时间：{dt.Rows[1]["T"].ToString()}";
                this.lbTask3.Text = $"任务号：{dt.Rows[2]["SeqID"].ToString()}，状态：{dt.Rows[2]["NwkStatus"].ToString()}，托盘号：{dt.Rows[2]["NPalletID"].ToString()}，执行时间：{dt.Rows[2]["T"].ToString()}";
            }
            else if (dt.Rows.Count == 2)
            {
                this.lbTask1.Text = $"任务号：{dt.Rows[0]["SeqID"].ToString()}，状态：{dt.Rows[0]["NwkStatus"].ToString()}，托盘号：{dt.Rows[0]["NPalletID"].ToString()}，执行时间：{dt.Rows[0]["T"].ToString()}";
                this.lbTask2.Text = $"任务号：{dt.Rows[1]["SeqID"].ToString()}，状态：{dt.Rows[1]["NwkStatus"].ToString()}，托盘号：{dt.Rows[1]["NPalletID"].ToString()}，执行时间：{dt.Rows[1]["T"].ToString()}";
                this.lbTask3.Text = "";
            }
            else if (dt.Rows.Count == 1)
            {
                this.lbTask1.Text = $"任务号：{dt.Rows[0]["SeqID"].ToString()}，状态：{dt.Rows[0]["NwkStatus"].ToString()}，托盘号：{dt.Rows[0]["NPalletID"].ToString()}，执行时间：{dt.Rows[0]["T"].ToString()}";
                this.lbTask2.Text = "";
                this.lbTask3.Text = "";
            }
            else
            {
                this.lbTask1.Text = "";
                this.lbTask2.Text = "";
                this.lbTask3.Text = "";
            }
        }

        //监控线体的设备
        private void DevicePaint()
        {
            var dt = WcsSqlB.GetRunDevice();
            var DWorkId = 0;
            var DPalletId = "";
            var DTarget = 0;
            var DErrorNo = 0;
            var DId = 0;
            foreach (Control c in this.panel1.Controls)
            {
                if (c is Button)
                {
                    var btn = (Button)c;

                    if (btn.Tag != null)
                    {
                        var row = dt.Select($" DNo={btn.Text.Replace("\r\n", "")} ");
                        //var d = OPCHelper.GetDeviceByNo(int.Parse(btn.Text.Replace("\r\n", "")));
                        if (row.Count() == 0) { btn.BackColor = Color.DimGray; continue; }
                        DWorkId = int.Parse(row[0]["DWorkId"].ToString());
                        DPalletId = row[0]["DPalletId"].ToString();
                        DTarget = int.Parse(row[0]["DTarget"].ToString());
                        DErrorNo = int.Parse(row[0]["DErrorNo"].ToString());
                        DId = int.Parse(row[0]["DId"].ToString());
                        if (DErrorNo > 0)
                        {
                            btn.BackColor = Color.Red;
                            btn.Tag = $"设备号：{DId}\r\n任务号：{DWorkId}\r\n托盘号：{DPalletId}\r\n目标号：{DTarget}\r\n错误信息：{AppCommon.GetDeviceErrorMsg(DErrorNo)}";
                        }
                        else
                        {
                            if (DWorkId > 0)
                            {
                                btn.BackColor = Color.LimeGreen;
                                btn.Tag = $"设备号：{DId}\r\n任务号：{DWorkId}\r\n托盘号：{DPalletId}\r\n目标号：{DTarget}";
                            }
                            else
                            {
                                btn.BackColor = Color.DimGray;
                            }
                        }
                    }
                }
            }
        }

        //监控堆垛机的位置
        private void DdjPaint()
        {

        }

        private void DrawDdj(int column, Button btn)
        {

        }

        private void button104_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag != null && btn.BackColor != Color.DimGray)
            {
                this.toolTip1.Active = true;
                this.toolTip1.ReshowDelay = 500;
                //var no = btn.Text.Replace("\r\n", "");
                //var d = WcsSqlB.GetWD(no);
                //var showString = "";
                //if (d != null)
                //{
                //    if (d.DErrorNo > 0)
                //    {
                //        showString = $"任务号：{d.DWorkId}\r\n托盘号：{d.DPalletId}\r\n目标号：{d.DTarget}\r\n错误信息：{AppCommon.GetDeviceErrorMsg(d.DErrorNo)}";
                //    }
                //    else
                //    {
                //        if (d.DWorkId > 0)
                //        {
                //            showString = $"任务号：{d.DWorkId}\r\n托盘号：{d.DPalletId}\r\n目标号：{d.DTarget}";
                //        }
                //    }
                this.toolTip1.Show(btn.Tag.ToString(), btn, 5000);
                //}
            }
            else
            {
                this.toolTip1.Active = false;
            }
        }

        private void button121_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var frm = new WriteDataFrm(int.Parse(btn.Text.Replace("\r\n", "")));
            frm.ShowDialog();
        }
    }
}
