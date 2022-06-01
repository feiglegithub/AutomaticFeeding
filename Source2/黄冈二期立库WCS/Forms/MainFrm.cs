using SpeechLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WCS.Common;
using WCS.DataBase;
using WCS.Forms;
using WCS.Mod;
using WCS.model;

namespace WCS
{
    public partial class MainFrm : Form
    {
        //主题颜色
        private Color Subject = Color.FromArgb(8, 70, 95);
        private Color color1 = Color.FromArgb(70, 105, 120);
        private Color color2 = Color.Yellow;
        static DateTime StartTime = Convert.ToDateTime("2018/10/01");

        public MainFrm()
        {
            InitializeComponent();
            Thread_Init();
            ActivateHomeFrm();
            AutoScales(this);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(0, 0);         //窗体的起始位置为(x,y)
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
            DialogResult result = MessageBox.Show("是否退出？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
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

        #region 窗体事件
        private void ActivateHomeFrm()
        {
            var frm = new HomeFrm();
            //ShowChildForm(frm, "HomeFrm");
        }

        private void tsmHome_Click(object sender, EventArgs e)
        {
            var frm = new HomeFrm();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor =Color.Silver; 
                }
            }

            ShowChildForm(frm, "HomeFrm");
        }

        private void ShowChildForm(Form f, string formName)
        {

            System.Windows.Forms.Form[] mdiform = this.MdiChildren;
            bool openFlag = false;  //窗体的打开标志
            foreach (Form fr in mdiform)
            {
                if (fr.Name == formName)
                {
                    fr.Activate();
                    openFlag = true;
                    break;
                }
            }
            if (!openFlag)
            {
                f.MdiParent = this;
                //f.StartPosition = FormStartPosition.CenterParent;
                f.Show();
                f.Dock = DockStyle.Fill;
            }
        }

        private void 站台管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new StationManaFrm();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "StationManaFrm");
        }

        private void 日志管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new LogFrm();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "LogFrm");
        }

        private void lED信息监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new LedInfoFrm();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "LedInfoFrm");
        }

        private void tsmTask_Click(object sender, EventArgs e)
        {
            var frm = new TaskFrm();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "TaskFrm");
        }   

        private void tsmMonitor_Click(object sender, EventArgs e)
        {
            //var frm = new DeviceMonitor();

            //ToolStripMenuItem item = (ToolStripMenuItem)sender;

            //foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            //{
            //    if (item.Name == it.Name)
            //    {
            //        it.BackColor = Subject;
            //        it.ForeColor = Color.White;
            //    }
            //    else
            //    {
            //        it.BackColor = color1;
            //        it.ForeColor = Color.Silver;
            //    }
            //}

            //ShowChildForm(frm, "DeviceMonitor");
        }

        private void 一楼发货区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DeviceMonitor();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "DeviceMonitor");
        }

        private void 二楼理货区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DeviceMonitor2();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "DeviceMonitor2");
        }

        private void 空盘回流ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DeviceMonitor3();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Subject;
                    it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = color1;
                    it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "DeviceMonitor3");
        }
        #endregion

        #region 多线程控制
        private Thread th1;
        private Thread th2;
        private Thread th3;
        private Thread th4;
        private Thread th5;
        private Thread th6;
        private Thread th7;

        private Thread th8;
        private Thread th9;
        private Thread th10;
        private Thread th11;
        private Thread th12;
        private Thread th13;
        private Thread th14;
        private Thread th15;

        private void Thread_Init()
        {
            th1 = new Thread(MainThread);
            th1.IsBackground = true;
            th1.Name = "th1";
            th1.Start();

            th2 = new Thread(Monitor);
            th2.IsBackground = true;
            th2.Name = "th2";
            th2.Start();

            th3 = new Thread(DdjRun);
            th3.IsBackground = true;
            th3.Name = "th3";
            th3.Start();

            th4 = new Thread(DMXRun);
            th4.IsBackground = true;
            th4.Name = "th4";
            th4.Start();

            th5 = new Thread(EmptyMana);
            th5.IsBackground = true;
            th5.Name = "th5";
            th5.Start();

            th6 = new Thread(WMSLEDControl);
            th6.IsBackground = true;
            th6.Name = "th6";
            th6.Start();

            th7 = new Thread(SpeedVoice);
            th7.IsBackground = true;
            th7.Name = "th7";
            th7.Start();

            th8 = new Thread(InWare2001);
            th8.IsBackground = true;
            th8.Name = "th8";
            th8.Start();

            th9 = new Thread(InWare2002);
            th9.IsBackground = true;
            th9.Name = "th9";
            th9.Start();

            th10 = new Thread(InWare2007);
            th10.IsBackground = true;
            th10.Name = "th10";
            th10.Start();

            th11 = new Thread(InWare2015);
            th11.IsBackground = true;
            th11.Name = "th11";
            th11.Start();

            th12 = new Thread(InWare2017);
            th12.IsBackground = true;
            th12.Name = "th12";
            th12.Start();

            th13 = new Thread(InWare2019);
            th13.IsBackground = true;
            th13.Name = "th13";
            th13.Start();

            th14 = new Thread(InWare2010);
            th14.IsBackground = true;
            th14.Name = "th14";
            th14.Start();

            th15 = new Thread(InWare2012);
            th15.IsBackground = true;
            th15.Name = "th15";
            th15.Start();
        }

        //主线程
        private void MainThread()
        {
            while (true)
            {
                try
                {
                    var rt = OPCExecute.IsConn;
                    if (rt == false)
                    {
                        OPCExecute.OPCServerAdd();
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    WcsSqlB.InsertLog("MainThread：" + ex.Message, 2);
                }
            }
        }

        //信号监控
        string[] msgs = { "", "", "", "" };
        string[] dates = { "", "", "", "" };
        private void Monitor()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    ScMonitor();
                    HsMonitor();
                }
                Thread.Sleep(500);
            }
        }

        //堆垛机监控
        //string[] errors = { "", "", "", "", "", "", "", "", "", "", "" };
        //string[] times = { "", "", "", "", "", "", "", "", "", "", "" };
        private void ScMonitor()
        {
            try
            {
                for (byte i = 1; i <= 11; i++)
                {
                    var sc = new Wcs_DdjInfo();
                    sc.ScNo = i;
                    sc.AlarmStatus = OPCHelper.IsAlarm(i);
                    sc.AutoStatus = OPCHelper.IsAuto(i);
                    sc.IsFree = OPCHelper.IsFree(i);
                    sc.CColumn = Convert.ToByte(OPCHelper.ReadColumn(i));
                    sc.CLayer = Convert.ToByte(OPCHelper.ReadLayer(i));
                    sc.TaskId = OPCHelper.ReadSCTaskId(i);
                    sc.NPalletID = OPCHelper.ReadSCPallet(i);
                    sc.TaskStatus = OPCHelper.ReadTaskStatus(i) == 1;

                    var arr = OPCHelper.ReadSCCode(i);
                    var msg = WcsSqlB.GetSCMsg(arr);

                    //if (msg.Length > 0)
                    //{
                    //    errors[i - 1] = msg;
                    //    if (times[i - 1] == "")
                    //    {
                    //        times[i - 1] = DateTime.Now.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    if (errors[i - 1].Length > 0)
                    //    {
                    //        WcsSqlB.InsertScError($"堆垛机[{i}]故障 ：{errors[i - 1]}", times[i - 1], DateTime.Now.ToString());
                    //        errors[i - 1] = "";
                    //        times[i - 1] = "";
                    //    }
                    //}

                    if (msg.Length > 0)
                    {
                        WcsSqlB.InsertLog($"[{i}]号堆垛机故障：{msg}", 2, "", "SC" + i);
                    }

                    WcsSqlB.UpdateDdjInfo(sc);
                }         
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog(ex.Message, 2);
            }
        }

        //地面线监控
        private void HsMonitor()
        {
            try
            {
                //var dt1 = DateTime.Now;
                OPCHelper.MonitorDevice();
                //var dt2 = DateTime.Now;
                //var s = (dt2 - dt1).TotalMilliseconds;
            }
            catch { }
        }

        //堆垛机运行
        private void DdjRun()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    ScControl.DDJFinishAll();
                    ScControl.DdjOutAll();
                    ScControl.DdjAllIn();
                }
                Thread.Sleep(1000);
            }
        }

        //地面线运行
        private void DMXRun()
        {
            while (true)
            {
                var rt = OPCExecute.IsConn;
                try
                {
                    if (rt)
                    {
                        //任务完成
                        FinishTask.FinisheAllTask();

                        //出循环口判断
                        LoopControl.Loop();

                        //入口错误信息监控
                        //InWare.InWareErrorAll();
                    }

                    Thread.Sleep(500);
                }
                catch { }
            }
        }

        //空托盘管理
        private void EmptyMana()
        {
            while (true)
            {
                try
                {
                    var rt = OPCExecute.IsConn;
                    if (rt)
                    {
                        //入库申请
                        //InWare.AllInWare();
                        //空托盘调度
                        EmptyControl.EptyRequestAll();
                    }

                    Thread.Sleep(500);
                }
                catch { }
            }
        }

        //LED信息发送
        private void LEDSend()
        {
            while (true)
            {
                try
                {
                    var lst = WcsSqlB.GetLEDSendList();
                    foreach (LED_Content lc in lst)
                    {
                        if (lc.LIP != null)
                        {
                            if (!ProcessHelper.ping(lc.LIP))
                            {
                                WcsSqlB.InsertLog($"Ping：{lc.LIP}失败！{DateTime.Now.ToString()}", 2);
                                lc.LStatus = 999;
                                WcsSqlB.UpdateLEDStatus(lc);
                                continue;
                            }

                            var rlt = LEDHelper.Send((short)lc.DeviceId, lc.LIP, lc.LContent, lc.Port);
                            lc.LStatus = rlt ? 200 : 500;
                            WcsSqlB.UpdateLEDStatus(lc);
                            WcsSqlB.InsertLog($"LED发送{(rlt ? "成功" : "失败")}！IP：{lc.LIP}，Port：{lc.Port}，DeviceID：{lc.DeviceId}", 1);
                        }
                        else
                        {
                            lc.LStatus = 999;
                            WcsSqlB.UpdateLEDStatus(lc);
                        }
                    }

                    Thread.Sleep(2000);
                }
                catch { }
            }
        }

        //语音播报
        private void SpeedVoice()
        {
            try
            {
                string content = "";
                SpVoice voice = new SpVoice();
                while (true)
                {
                    content = WcsSqlB.ReadVoice().Replace("(设备原因)", "").Replace("(人为原因)", "");
                    if (content.Length > 0)
                    {
                        voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
                        voice.Speak(content);
                    }

                    Thread.Sleep(5000);
                }
            }
            catch { }
        }

        //发货口的LED屏信息推送
        private void WMSLEDControl()
        {
            LedService.LEDMsgQueryServiceClient ledFunOut = new LedService.LEDMsgQueryServiceClient();
            LedService.queryOutsideHBZNCStationTasks paraOut = new LedService.queryOutsideHBZNCStationTasks();
            paraOut.in0 = new LedService.StageInfo();
            paraOut.in0.invcode = "HBB_ZNC";

            LedService.LEDMsgQueryServiceClient ledFunIn = new LedService.LEDMsgQueryServiceClient();
            LedService.queryHBZNCStationTasks paraIn = new LedService.queryHBZNCStationTasks();
            paraIn.in0 = new LedService.StageInfo();
            paraIn.in0.invcode = "HBB_ZNC";

            var ledbuff = new Dictionary<string, string>();
            for (int i = 1001; i <= 1019; i++)
            {
                ledbuff.Add($"{i}1", "");
                if (i >= 1009)
                {
                    ledbuff.Add($"{i}2", "");
                }
            }

            while (true)
            {
                try
                {
                    //所有外屏
                    for (int port = 1009; port <= 1019; port++)
                    {
                        paraOut.in0.station = port.ToString();
                        var data = ledFunOut.queryOutsideHBZNCStationTasks(paraOut);
                        if (data.out1.rows_2 != null && data.out1.rows_2.Count() > 0)
                        {
                            var leddata = data.out1.rows_2[0];
                            var content = $"   {leddata.area}\r\n车牌：{leddata.act_plate_number}\r\n仓管：{leddata.responser}\r\n电话：{leddata.telephone}";
                            if (ledbuff[$"{port}2"].Equals(content)) { continue; }
                            ledbuff[$"{port}2"] = content;
                            WcsSqlB.InsertLED(content, port, 2);
                        }
                        else
                        {
                            var def = $" 立库B-{port - 1000}号口";
                            if (ledbuff[$"{port}2"].Equals(def)) { continue; }
                            ledbuff[$"{port}2"] = def;
                            WcsSqlB.InsertLED(def, port, 2);
                        }
                    }

                    //所有内屏
                    for (int port = 1001; port <= 1019; port++)
                    {
                        paraIn.in0.station = port.ToString();
                        var data = ledFunIn.queryHBZNCStationTasks(paraIn);
                        if (data.out1.rows != null && data.out1.rows.Count() > 0)
                        {
                            var leddata = data.out1.rows[0];
                            var content = $"站台名称：{leddata.stage}\r\n区域：{leddata.area}\r\n经销商编码：{leddata.customer_number}\r\n已装车数量：{leddata.downtasks}\r\n未装车数量：{leddata.pendingtasks}\r\n发运总托数：{leddata.totalpacks}\r\nPDA未提交：{leddata.notsubmit}";
                            if (ledbuff[$"{port}1"].Equals(content)) { continue; }
                            ledbuff[$"{port}1"] = content;
                            WcsSqlB.InsertLED(content, port, 1);
                        }
                        else
                        {
                            var def = $"   索菲亚智能仓库{port - 1000}";
                            if (ledbuff[$"{port}1"].Equals(def)) { continue; }
                            ledbuff[$"{port}1"] = def;
                            WcsSqlB.InsertLED(def, port, 1);
                        }
                    }
                    Thread.Sleep(10 * 1000);
                }
                catch { }
            }
        }


        #region 入库
        private void InWare2001()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2001();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2002()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2002();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2007()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2007();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2015()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2015();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2017()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2017();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2019()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2019();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2010()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2010();
                }

                Thread.Sleep(500);
            }
        }

        private void InWare2012()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    InWare.InWare2012();
                }

                Thread.Sleep(500);
            }
        }

        #endregion
        #endregion
    }
}
