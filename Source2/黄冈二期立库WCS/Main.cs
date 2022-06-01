using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using WCS.Common;

namespace WCS
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            //TestDemo();
            //Thread_Init();
        }

        private void TestDemo()
        {
            ShowDDJStatue();
            //Item item = OpcBaseManage.opcBase.GetItem(0, 2);
            //var rlt = item.ReadString();
            //MessageBox.Show(rlt.ToString());
        }

        #region 线程定义及初始化

        private System.Threading.Thread thMain;
        private Thread thDmgOther;
        private Thread thDdj;
        private Thread thDmgIn;
        private Thread thMonitor;

        int timeOut = 1000;
        static bool isClose = false;
        DateTime nowTime;

        /// <summary>
        /// 线程初始化 
        /// </summary>
        private void Thread_Init()
        {
            thMain = new Thread(MainDoing);
            thMain.IsBackground = true;
            thMain.Name = "thMain";
            thMain.Start();

            thDdj = new Thread(new ThreadStart(DdjDoing));
            thDdj.IsBackground = true;
            thDdj.Name = "thDdj";
            thDdj.Start();

            thDmgIn = new Thread(new ThreadStart(DmgInDoing));
            thDmgIn.IsBackground = true;
            thDmgIn.Name = "thDmgIn";
            thDmgIn.Start();

            thDmgOther = new Thread(new ThreadStart(DmgOtherDoing));
            thDmgOther.IsBackground = true;
            thDmgOther.Name = "thDmgOther";
            thDmgOther.Start();

            thMonitor = new Thread(new ThreadStart(MonitorDoing));
            thMonitor.IsBackground = true;
            thMonitor.Name = "th_Wcs_Monitor";
            thMonitor.Start();
        }
        #endregion

        #region 主线程
        static DateTime StartTime = Convert.ToDateTime("2018/10/01");
        void MainDoing()
        {
            statusStrip1.Items[0].Text = "立库WCS控制系统运行中.....              ";

            do
            {
                if (isClose)
                    break;
                GetTime();

                ShowTime();



                int deffTime = Math.Abs(ControlTime.DiffTime(StartTime, DateTime.Now, "MM"));
                if (deffTime > 50)
                {
                    ControlTime.SetSysTime(StartTime);
                }

                ShowDDJStatue();

                //地面线入库口错误信息监控
                //Monitor.HsInError();

                //Item item = OpcBaseManage.opcBase.GetItem(1, 0);
                //bool a = item.ReadBool();

                //OpcHs.WriteDeviceInfo(" Id%4=0 ");  //设备信息监控
                System.Threading.Thread.Sleep(timeOut);
            }
            while (true);
        }

        void GetTime()
        {
            try
            {
                if (nowTime < DateTime.Now)
                    nowTime = WCSSq.GetTime();
            }
            catch
            { }
        }

        void ShowTime()
        {
            statusStrip1.Items[1].Text = "当前时间：" + nowTime.ToString();
        }

        void ShowDDJStatue()
        {
            if (OpcBaseManage.opcBase.GetCpu(0).isConnect)
            {
                lblStatus5.Text = "连接成功";
                lblStatus5.BackColor = Color.Lime;
            }
            else
            {
                lblStatus5.Text = "连接失败";
                lblStatus5.BackColor = Color.Red;
            }

            //if (OpcBaseManage.opcBase.GetCpu(1).isConnect)
            //{
            //    lblStatus6.Text = "连接成功";
            //    lblStatus6.BackColor = Color.Lime;
            //}
            //else
            //{
            //    lblStatus6.Text = "连接失败";
            //    lblStatus6.BackColor = Color.Red;
            //}
        }

        #endregion

        void DdjDoing()
        {
            while (true)
            {
                ScControl.DDJFinishAll();

                ScControl.DdjOutAll();

                ScControl.DdjAllIn();

                Thread.Sleep(500);
            }
        }

        #region 地面线入库
        void DmgInDoing()
        {
            while (true)
            {
                //InWare.AllIn();   //整盘入库

                //OpcHs.WriteDeviceInfo(" Id%4=1 ");  //设备信息监控
                //System.Threading.Thread.Sleep(timeOut);
            }
        }
        #endregion

        #region 其他地面线业务
        void DmgOtherDoing()
        {
            while (true)
            {
                ////托盘走向判断
                //if (OpcHs.IsRequestPalletWay())
                //{
                //    string pallet = OpcHs.ReadPalletNo();
                //    if (MonitorSql.GetTaskType(pallet) > 0)
                //    {
                //        //标准件出库
                //        OpcHs.WritePalletWay(2);
                //        Sql.UpdateTaskState(pallet, 32, null);
                //    }
                //    else
                //    {
                //        //空托盘出库
                //        OpcHs.WritePalletWay(1);
                //        //OpcHs.WritePalletWay(2);
                //    }
                //}

                //FinishOut.OutFinish_2();
                //OpcHs.WriteDeviceInfo(" Id%4=2 ");  //设备信息监控
                //System.Threading.Thread.Sleep(timeOut);
            }
        }
        #endregion


        #region 监控
        void MonitorDoing()
        {
            while (true)
            {
                try
                {
                    Monitor.RGVMonitor();  //RGV监控
                    //OpcHs.WriteDeviceInfo(" Id%4=3 ");  //设备信息监控
                    Monitor.HsMonitor();  //地面线信息监控
                }
                catch (Exception ex)
                {
                    WriteError(ex.Message);
                }

                //OpcHs.WriteDeviceInfo(" Id%2=1 ");  //1区2区3区4区 设备信息监控
                //System.Threading.Thread.Sleep(timeOut * 5);
            }
        }
        #endregion


        #region 界面显示及日志存储

        List<string> optMsgList = new List<string>();
        List<string> errorMsgList = new List<string>();
        private void WriteLog(string log)
        {
            try
            {
                tbLog.Text = "";

                if (optMsgList.RemoveAll(p => p.Contains(log)) == 0)
                {
                    Sql.InsertSysLog(log);
                }

                if (optMsgList.Count > 50)
                    optMsgList.RemoveRange(0, optMsgList.Count - 15);

                optMsgList.Add(Sql.GetTime().ToString("HH:mm:ss") + log);

                foreach (var item in optMsgList)
                {
                    tbLog.Text += item + "\r\n";
                }

                tbLog.SelectionStart = tbLog.Text.Length;
                tbLog.ScrollToCaret();
            }
            catch { }
        }

        private void WriteError(string log)
        {
            try
            {
                tbError.Text = "";

                if (errorMsgList.Exists(p => p.Contains(log)))
                {
                    Sql.InsertErrorLog(log);
                    errorMsgList.RemoveAll(p => p.Contains(log));
                }
                else
                {
                    Sql.InsertErrorLog(log);
                }

                errorMsgList.Add(Sql.GetTime().ToString("HH:mm:ss") + log);

                //if (errorMsgList.Count > 50)
                //    errorMsgList.RemoveRange(0, errorMsgList.Count - 15);

                //if (errorMsgList.Count >= 3)
                //    errorMsgList.RemoveRange(0, 2);

                foreach (var item in errorMsgList)
                {
                    tbError.Text += item + "\r\n";
                }

                tbError.SelectionStart = tbError.Text.Length;
                tbError.ScrollToCaret();
            }
            catch
            { }
        }

        #endregion

        private void Mail_Load(object sender, EventArgs e)
        {
            //LogWrite.WriteLogBack += new LogWrite.WriteLogDelegate(WriteLog);
            //LogWrite.ErrorLog += new LogWrite.ErrorLogDelegate(WriteError);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClose = true;
        }

        private void btlCloseIn_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseOut_Click(object sender, EventArgs e)
        {

        }

        private void tmTime_Tick(object sender, EventArgs e)
        {
            ShowDDJStatue();
            //    nowTime = nowTime.AddSeconds(1);

            //    OpcHs.WriteDeviceOrder();  //执行设备手动下发命令

            //    //重入库处理
            //    try
            //    {
            //        string rlt = Sql.GetRework();
            //        if (!string.IsNullOrEmpty(rlt))
            //        {
            //            string msg = Sql.appleReworkLocator(rlt.Split('|')[0], int.Parse(rlt.Split('|')[1]));
            //            if (msg != "") 
            //            {
            //                LogWrite.WriteError(msg);
            //            }
            //        }

            //        //OpcHs.WriteDeviceInfo(" Id%5=4 ");  //设备信息监控
            //    }
            //    catch { }
            //}
            //#endregion
        }

        //每隔30分钟重置系统时间，是为了防止UaExpert连接超过1小时后过期,用管理员身份运行程序才能生效
        //private void SystemTimeSet_Tick(object sender, EventArgs e)
        //{
        //    var localTime = new SystemTimeWin32.Systemtime()
        //    {
        //        wYear = 2018,
        //        wMonth = 9,
        //        wDay = 30,
        //        wHour = 01,
        //        wMinute = 00,
        //        wMiliseconds = 00

        //    };

        //    var result = SystemTimeWin32.SetSystemTime(ref localTime);
        //}
    }
}

