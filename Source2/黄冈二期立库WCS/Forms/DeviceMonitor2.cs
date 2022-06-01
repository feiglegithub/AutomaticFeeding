using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class DeviceMonitor2 : Form
    {
        public DeviceMonitor2()
        {
            InitializeComponent();
            this.Activated += DeviceMonitor2_Activated;
            this.Deactivate += DeviceMonitor2_Deactivate;
        }

        private void DeviceMonitor2_Deactivate(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void DeviceMonitor2_Activated(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void DeviceMonitor2_Load(object sender, EventArgs e)
        {
           
        }

        private void button69_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            //if (btn.BackColor == Color.DimGray) { 
            var frm = new WriteDataFrm(int.Parse(btn.Text.Replace("\r\n", "")));
            frm.ShowDialog();
        }

        //int errorcode = 0;
        //int workid = 0;
        //int target = 0;
        //int did = 0;
        //string pallet = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            var dt = WcsSqlB.GetRunDevice();
            var DWorkId = 0;
            var DPalletId = "";
            var DTarget = 0;
            var DErrorNo = 0;
            var DId = 0;

            foreach (Control c in this.groupBox7.Controls)
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

            foreach (Control c in this.Controls)
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

        private void button69_MouseHover(object sender, EventArgs e)
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
    }
}
