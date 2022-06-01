using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;
using WCS.OPC;

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

        public void demo()
        {
            //多线程 
            //tasks.Clear();
            //foreach (Control c in this.panel1.Controls)
            //{
            //    tasks.Add(Task.Factory.StartNew(() =>
            //    {
            //        if (c is Button)
            //        {
            //            var btn = (Button)c;
            //            if (c.Tag != null)
            //            {
            //                var item = OpcBaseManage.opcBase.GetItem(0, int.Parse(c.Tag.ToString()) + 1);
            //                var state = item.ReadBool();
            //                btn.BackColor = state ? Color.LimeGreen : Color.DimGray;
            //            }
            //        }
            //    }));

            //    if (tasks.Count >= 10)
            //    {
            //        Task.WaitAll(tasks.ToArray());
            //        tasks.Clear();
            //    }
            //}
            //if (tasks.Count > 0)
            //{
            //    Task.WaitAll(tasks.ToArray());
            //}
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

        //监控线体的设备
        private void DevicePaint()
        {
            //var dic = WCSSql.GetRunDevice();
            int n = 0;
            foreach (Control c in this.panel1.Controls)
            {
                if (c is Button)
                {
                    var btn = (Button)c;

                    var d = OpcHsc.ReadDeviceDataByNo(btn.Text);
                    if (d.Staus)
                    {
                        btn.BackColor = Color.LimeGreen;
                        btn.Tag = $"垛   号：{d.PilerNo}\r\n目标号：{d.Target}";
                    }
                    else
                    {
                        btn.BackColor = Color.DimGray;
                    }

                    var d_msg = OpcHsc.ReadDeiviceErrorMsg(btn.Text);
                    if (d_msg.Length > 0)
                    {
                        btn.BackColor = Color.Red;
                        btn.Tag = $"垛   号：{d.PilerNo}\r\n目标号：{d.Target}\r\n错   误：{d_msg}";
                    }
                }
            }
        }

        //监控堆垛机的位置
        private void DdjPaint()
        {
            var c1 = OpcSc.RColumn(1);
            DrawDdj(c1, this.btnSC1);

            var c2 = OpcSc.RColumn(2);
            DrawDdj(c2, this.btnSC2);

            var c3 = OpcSc.RColumn(3);
            DrawDdj(c3, this.btnSC3);

            var c4 = OpcSc.RColumn(4);
            DrawDdj(c4, this.btnSC4);
        }

        private void DrawDdj(int column, TextBox btn)
        {
            if (column == 0)
            {
                return;
            }

            int Y = btn.Location.Y;

            if (column == 1)
            {
                btn.Location = new Point(this.btnEnd.Location.X, Y);
            }
            else if (column == 22)
            {
                btn.Location = new Point(this.btnStart.Location.X, Y);
            }
            else
            {
                btn.Location = new Point((22 - column) * (this.btnStart.Width - 1) + this.btnStart.Location.X, Y);
            }
        }

        //当数据意外丢失时，手动给设备写入数据：目标值和垛号
        private void btnWriteDeviceData_Click(object sender, EventArgs e)
        {
            var wdf = new WriteDataFrm((Button)sender);
            wdf.ShowDialog();
        }

        private void button104_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn.BackColor != Color.DimGray)
                {
                    this.toolTip1.Active = true;
                    this.toolTip1.ReshowDelay = 500;
                    this.toolTip1.Show(btn.Tag.ToString(), btn, 5000);
                }
                else
                {
                    this.toolTip1.Active = false;
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                DevicePaint();
                DdjPaint();
            }
            catch { }
        }
    }
}
