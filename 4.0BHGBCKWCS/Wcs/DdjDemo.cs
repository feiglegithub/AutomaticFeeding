using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WCS.Common;
using WCS.model;

namespace WCS
{
    public partial class DdjDemo : Form
    {
        private Thread th1;
        private Thread th2;
        private Thread th3;
        //private Thread th4;
        static DateTime StartTime = Convert.ToDateTime("2018/10/01");

        public DdjDemo()
        {
            InitializeComponent();
            this.Load += DdjDemo_Load;
            Thread_Init();
        }

        private void Thread_Init()
        {
            th1 = new Thread(MainThread);
            th1.IsBackground = true;
            th1.Name = "th1";
            th1.Start();

            th2 = new Thread(DdjMonitor);
            th2.IsBackground = true;
            th2.Name = "th2";
            th2.Start();

            th3 = new Thread(DdjAction);
            th3.IsBackground = true;
            th3.Name = "th3";
            th3.Start();
        }

        //主线程
        private void MainThread()
        {
            while (true)
            {
                try
                {
                    if (!OPCExecute.IsConn)
                    {
                        OPCExecute.OPCServerAdd();
                    }

                    int deffTime = Math.Abs(ControlTime.DiffTime(StartTime, DateTime.Now, "MM"));
                    if (deffTime > 50)
                    {
                        ControlTime.SetSysTime(StartTime);
                    }

                    ShowPLCConnectionStatus();

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    //LogWrite.WriteError(ex.Message);
                }
            }
        }

        private void ShowPLCConnectionStatus()
        {
            if (OpcBaseManage.opcBase.GetCpu(0).isConnect)
            {
                this.button2.BackColor = Color.Lime;
            }
            else
            {
                this.button2.BackColor = Color.Red;
            }
        }

        //堆垛机监控
        private void DdjMonitor()
        {
            //int ddj = 1;
            while (true)
            {
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        //为了从其它线程访问到控件而使用委托
                        this.Invoke((EventHandler)delegate
                        {
                            this.lbAuto1.Text = OpcSc.IsAuto(1) ? "自动" : "手动";
                            this.lbStatus1.Text = OpcSc.RIsFree(1) ? "空闲" : "正忙";
                            this.lbActivate1.Text = OpcSc.RIsActivation(1) ? "激活" : "关闭";
                            this.lbPilerNo1.Text = OpcSc.RTask(1);
                            var t1 = OpcSc.RTaskType(1);
                            this.lbType1.Text = t1 == 1 ? "入库" : (t1 == 2 ? "出库" : "无工作");
                            this.lbCurrent1.Text = OpcSc.RLocation(1);
                            this.lbFrom1.Text = OpcSc.RFrom(1);
                            this.lbTo1.Text = OpcSc.RTo(1);
                            this.lbError1.Text = OpcSc.GetSCErrorMsg(1);

                            this.lbAuto2.Text = OpcSc.IsAuto(2) ? "自动" : "手动";
                            this.lbStatus2.Text = OpcSc.RIsFree(2) ? "空闲" : "正忙";
                            this.lbActivate2.Text = OpcSc.RIsActivation(2) ? "激活" : "关闭";
                            this.lbPilerNo2.Text = OpcSc.RTask(2);
                            var t2 = OpcSc.RTaskType(2);
                            this.lbType2.Text = t2 == 1 ? "入库" : (t2 == 2 ? "出库" : "无工作");
                            this.lbCurrent2.Text = OpcSc.RLocation(2);
                            this.lbFrom2.Text = OpcSc.RFrom(2);
                            this.lbTo2.Text = OpcSc.RTo(2);
                            this.lbError2.Text = OpcSc.GetSCErrorMsg(2);

                            this.lbAuto3.Text = OpcSc.IsAuto(3) ? "自动" : "手动";
                            this.lbStatus3.Text = OpcSc.RIsFree(3) ? "空闲" : "正忙";
                            this.lbActivate3.Text = OpcSc.RIsActivation(3) ? "激活" : "关闭";
                            this.lbPilerNo3.Text = OpcSc.RTask(3);
                            var t3 = OpcSc.RTaskType(3);
                            this.lbType3.Text = t3 == 1 ? "入库" : (t3 == 2 ? "出库" : "无工作");
                            this.lbCurrent3.Text = OpcSc.RLocation(3);
                            this.lbFrom3.Text = OpcSc.RFrom(3);
                            this.lbTo3.Text = OpcSc.RTo(3);
                            this.lbError3.Text = OpcSc.GetSCErrorMsg(3);

                            this.lbAuto4.Text = OpcSc.IsAuto(4) ? "自动" : "手动";
                            this.lbStatus4.Text = OpcSc.RIsFree(4) ? "空闲" : "正忙";
                            this.lbActivate4.Text = OpcSc.RIsActivation(4) ? "激活" : "关闭";
                            this.lbPilerNo4.Text = OpcSc.RTask(4);
                            var t4 = OpcSc.RTaskType(4);
                            this.lbType4.Text = t4 == 1 ? "入库" : (t4 == 2 ? "出库" : "无工作");
                            this.lbCurrent4.Text = OpcSc.RLocation(4);
                            this.lbFrom4.Text = OpcSc.RFrom(4);
                            this.lbTo4.Text = OpcSc.RTo(4);
                            this.lbError4.Text = OpcSc.GetSCErrorMsg(4);
                        });
                    }
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    //LogWrite.WriteError(ex.Message);
                }
            }
        }

        //堆垛机运行
        private void DdjAction()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    ScControl.GetInstance().DdjFinishAll();
                }
                Thread.Sleep(1000);
            }

        }

        #region 窗口事件、日志记录 
        private void DdjDemo_Load(object sender, EventArgs e)
        {
            //LogWrite.WriteLogBack += WriteLog;
            //LogWrite.ErrorLog += WriteErrorLog;
        }

        private void WriteLog(string content)
        {
            this.Invoke((EventHandler)delegate
            {
                if (this.lvLog.Items.Count > 50)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        this.lvLog.Items.RemoveAt(0);
                    }
                }

                var item = new ListViewItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(content);
                this.lvLog.Items.Add(item);
            });
        }

        private void WriteErrorLog(string content)
        {
            this.Invoke((EventHandler)delegate
            {
                if (this.lvLogE.Items.Count > 50)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        this.lvLogE.Items.RemoveAt(0);
                    }
                }

                var log_count = this.lvLogE.Items.Count;
                if (log_count > 0 && this.lvLogE.Items[log_count - 1].SubItems[0].Text.Equals(content))
                {
                    this.lvLogE.Items[log_count - 1].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    var item = new ListViewItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(content);
                    this.lvLogE.Items.Add(item);
                }
            });
        }
        #endregion

        //手动写入堆垛机任务 测试专用
        private void button1_Click(object sender, EventArgs e)
        {
            TaskInfo task = new TaskInfo();
            int type = int.Parse(this.txtType.Text);
            int Ddj = int.Parse(this.txtDdj.Text);
            task.PilerNo = int.Parse(this.txtPiler.Text);

            if (!OpcSc.CanRun(Ddj)) { return; }

            if (type == 1)
            {
                //入库任务
                task.ToRow = int.Parse(this.txtRow.Text);
                task.ToColumn = int.Parse(this.txtColumn.Text);
                task.ToLayer = int.Parse(this.txtLayer.Text);
                if (OpcSc.WTaskIn(task, Ddj))
                {
                    //LogWrite.WriteLogToMain($"堆垛机[{Ddj}]：入库开始，垛号：[{task.PilerNo}]");
                }
                else
                {
                    //LogWrite.WriteLogToMain($"堆垛机[{Ddj}]：入库接收失败，垛号：[{task.PilerNo}]");
                }
            }
            else if (type == 2)
            {
                //出库任务
                task.FromRow = int.Parse(this.txtRow.Text);
                task.FromColumn = int.Parse(this.txtColumn.Text);
                task.FromLayer = int.Parse(this.txtLayer.Text);
                if (OpcSc.WTaskOut(task, Ddj))
                {
                    //LogWrite.WriteLogToMain($"堆垛机[{Ddj}]：出库开始，垛号：[{task.PilerNo}]");
                }
                else
                {
                    //LogWrite.WriteLogToMain($"堆垛机[{Ddj}]：出库接收失败，垛号：[{task.PilerNo}]");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 231);
            this.txtAddr.Text = item.ReadString();
        }
    }
}
