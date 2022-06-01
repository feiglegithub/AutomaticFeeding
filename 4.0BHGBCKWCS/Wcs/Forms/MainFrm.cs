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
using WcsModel;
using WCS.Commands;
using WCS.Commands.Args;
using WCS.DataBase;
using WCS.Forms;
using WCS.Mod;
using WCS.model;
using WCS.OPC;

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
            this.menuStrip1.Enabled = false;
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
                //Application.Exit();
                Environment.Exit(0);
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
            ShowChildForm(frm, "HomeFrm");
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

            Form[] mdiform = this.MdiChildren;
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

        private void tsmSort_Click(object sender, EventArgs e)
        {
            var frm = new SortFrm();

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

            ShowChildForm(frm, "SortFrm");
        }


        private void rGVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new RGVTaskFrm();

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

            ShowChildForm(frm, "RGVTaskFrm");
        }

        private void tsmMonitor_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 堆垛车开始命令
        /// </summary>
        private Dictionary<EPiler, ICommand> PilerBeginCommands { get; } = new Dictionary<EPiler, ICommand>()
        {
            {EPiler.First, new PilerBeginCommand(EPiler.First)},
            {EPiler.Second, new PilerBeginCommand(EPiler.Second)},
            {EPiler.Third, new PilerBeginCommand(EPiler.Third)},
            {EPiler.Fourth, new PilerBeginCommand(EPiler.Fourth)},
        };
        /// <summary>
        /// 堆垛车完成命令
        /// </summary>
        private Dictionary<EPiler, ICommand> PilerFinishedCommands { get; } = new Dictionary<EPiler, ICommand>()
        {
            {EPiler.First, new PilerFinishedCommand(EPiler.First)},
            {EPiler.Second, new PilerFinishedCommand(EPiler.Second)},
            {EPiler.Third, new PilerFinishedCommand(EPiler.Third)},
            {EPiler.Fourth, new PilerFinishedCommand(EPiler.Fourth)},
        };

        /// <summary>
        /// 拣选入库命令
        /// </summary>
        private Dictionary<ESortingStation,ICommand> SortingInStockCommands { get; } = new Dictionary<ESortingStation, ICommand>()
        {
            {ESortingStation.SortingStation2001, new SortingBeginInStockCommand(ESortingStation.SortingStation2001)},
            {ESortingStation.SortingStation2002, new SortingBeginInStockCommand(ESortingStation.SortingStation2002)},
            {ESortingStation.SortingStation2003, new SortingBeginInStockCommand(ESortingStation.SortingStation2003)},
            {ESortingStation.SortingStation2004, new SortingBeginInStockCommand(ESortingStation.SortingStation2004)},
            {ESortingStation.SortingStation2005, new SortingBeginInStockCommand(ESortingStation.SortingStation2005)},
        };

        /// <summary>
        /// 要料完成命令
        /// </summary>
        private Dictionary<ESortingStation, ICommand> SortingLoadingFinishedCommands { get; } = new Dictionary<ESortingStation, ICommand>()
        {
            {ESortingStation.SortingStation2001, new SortingLoadingFinishedCommand(ESortingStation.SortingStation2001)},
            {ESortingStation.SortingStation2002, new SortingLoadingFinishedCommand(ESortingStation.SortingStation2002)},
            //{ESortingStation.SortingStation2003, new SortingLoadingFinishedCommand(ESortingStation.SortingStation2003)},
            {ESortingStation.SortingStation2004, new SortingLoadingFinishedCommand(ESortingStation.SortingStation2004)},
            //{ESortingStation.SortingStation2005, new SortingLoadingFinishedCommand(ESortingStation.SortingStation2005)},
        };

        /// <summary>
        /// 拣选出垛完成命令
        /// </summary>
        private Dictionary<ESortingStation, ICommand> SortingOutStackCommands { get; } = new Dictionary<ESortingStation, ICommand>()
        {
            {ESortingStation.SortingStation2001, new SortingOutStackCommand(ESortingStation.SortingStation2001)},
            {ESortingStation.SortingStation2002, new SortingOutStackCommand(ESortingStation.SortingStation2002)},
            {ESortingStation.SortingStation2003, new SortingOutStackCommand(ESortingStation.SortingStation2003)},
            {ESortingStation.SortingStation2004, new SortingOutStackCommand(ESortingStation.SortingStation2004)},
            {ESortingStation.SortingStation2005, new SortingOutStackCommand(ESortingStation.SortingStation2005)},
        };
        
        /// <summary>
        /// 板材出入库命令
        /// </summary>
        private Dictionary<EInOutStation, ICommand> BoardInOutStockCommands { get; } = new Dictionary<EInOutStation, ICommand>()
        {
            {EInOutStation.InStockStationGt102, new BoardInStockCommand()},
            {EInOutStation.OutStockStationGt116, new BoardOutStockCommand(EInOutStation.OutStockStationGt116)},
            {EInOutStation.OutStockStationGt208, new BoardOutStockCommand(EInOutStation.OutStockStationGt208)},

        };

        private Dictionary<ECuttingStation,ICommand> CuttingLoadingCommands = new Dictionary<ECuttingStation, ICommand>()
        {
            {ECuttingStation.CuttingStation3001,new OutWareFinishedCommand(ECuttingStation.CuttingStation3001) },
            {ECuttingStation.CuttingStation3002,new OutWareFinishedCommand(ECuttingStation.CuttingStation3002) },
            {ECuttingStation.CuttingStation3003,new OutWareFinishedCommand(ECuttingStation.CuttingStation3003) },
            {ECuttingStation.CuttingStation3004,new OutWareFinishedCommand(ECuttingStation.CuttingStation3004) },
            {ECuttingStation.CuttingStation3005,new OutWareFinishedCommand(ECuttingStation.CuttingStation3005) },
            {ECuttingStation.CuttingStation3006,new OutWareFinishedCommand(ECuttingStation.CuttingStation3006) },
            {ECuttingStation.CuttingStation3007,new OutWareFinishedCommand(ECuttingStation.CuttingStation3007) },
        };

        /// <summary>
        /// Rgv命令
        /// </summary>
        private List<ICommand> RgvCommands { get; } = new List<ICommand>()
        {
            new RgvBeginCommand(),
            new RgvFinishedCommand(),
        };

        /// <summary>
        /// 拣选命令
        /// </summary>
        private List<ICommand> SortingCommands { get; } = new List<ICommand>()
        {
            new MhBeginCommand(),
            new MhFinishedCommand(),
        };

        /// <summary>
        /// 请求命令
        /// </summary>
        private List<ICommand> RequestCommands { get; } = new List<ICommand>()
        {
            new EmptyBoardRequestCommand(),
            new EmptySubplateInStockRequestCommand(ECuttingEmptyStation.CuttingEmptyStationGt307),
            new EmptySubplateInStockRequestCommand(ECuttingEmptyStation.CuttingEmptyStationGt317),
            new OffBoardInStockRequestCommand(),
            new SortingInStockRequestCommand(),
            new RequestMaterialCommand()
        };

        /// <summary>
        /// 执行请求命令
        /// </summary>
        private void ExecuteRequestCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {

                    if (OPCExecute.IsConn)
                    {
                        foreach (var cmd in RequestCommands)
                        {
                            command = cmd;
                            command.Execute();
                            Thread.Sleep(1000);
                        }
                    }

                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command + ex.StackTrace, "Error");
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 执行拣选命令
        /// </summary>
        private void ExecuteSortingCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        foreach (var cmd in SortingCommands)
                        {
                            command = cmd;
                            command.Execute();
                            Thread.Sleep(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command + ex.StackTrace, "Error");
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 执行Rgv命令
        /// </summary>
        private void ExecuteRgvCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        foreach (var cmd in RgvCommands)
                        {
                            command = cmd;
                            command.Execute();
                            Thread.Sleep(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command + ex.StackTrace, "Error");
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 执行Rgv命令
        /// </summary>
        private void ExecuteInOutStockCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        foreach (var keyValue in BoardInOutStockCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                            Thread.Sleep(20);
                        }

                        foreach (var keyValue in CuttingLoadingCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command+ ex.StackTrace, "Error");
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 执行堆垛车命令
        /// </summary>
        private void ExecutePilerCommand()
        {
            while (true)
            {
                ICommand command=null;
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        foreach (var keyValue in PilerBeginCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                            Thread.Sleep(20);
                        }

                        foreach (var keyValue in PilerFinishedCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                            Thread.Sleep(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command + ex.StackTrace, "Error");
                }
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 执行拣选的其他相关命令
        /// </summary>
        private void ExecuteSortingOtherCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {
                    
                    if (OPCExecute.IsConn)
                    {
                        foreach (var keyValue in SortingInStockCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                            Thread.Sleep(20);
                        }

                        foreach (var keyValue in SortingOutStackCommands)
                        {
                            command = keyValue.Value;
                            command.Execute();
                            Thread.Sleep(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command+ ex.StackTrace, "Error");
                }
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 执行拣选的其他相关命令
        /// </summary>
        private void ExecuteSortingLoadingFinishedCommand()
        {
            while (true)
            {
                ICommand command = null;
                try
                {

                    if (OPCExecute.IsConn)
                    {
                        foreach (var keyValue in SortingLoadingFinishedCommands)
                        {
                            //WCSSql.InsertLog($"开始执行{keyValue.Key}", "LOG");
                            command = keyValue.Value;
                            command.Execute();
                            //WCSSql.InsertLog($"完成执行{keyValue.Key}", "LOG");
                            Thread.Sleep(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(command + ex.StackTrace, "Error");
                }
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 创建拣选明细命令
        /// </summary>
        private ICommand createdSortingDetailCommand = new CreatedSortingDetailCommand();

        /// <summary>
        /// 执行创建拣选明细命令
        /// </summary>
        private void ExecuteCreatedSortingDetailCommand()
        {
            while (true)
            {
                try
                {
                    createdSortingDetailCommand.Execute();
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(createdSortingDetailCommand + ex.StackTrace, "Error");
                    Thread.Sleep(5000);
                }
                Thread.Sleep(2000);
            }
        }


        private void Thread_Init()
        {
            th1 = new Thread(MainThread);
            th1.IsBackground = true;
            th1.Name = "th1";
            th1.Start();

            th2 = new Thread(HsMonitor);
            th2.IsBackground = true;
            th2.Name = "th2";
            th2.Start();

            //th3 = new Thread(DdjRun);
            th3 = new Thread(ExecutePilerCommand);
            th3.IsBackground = true;
            th3.Name = "th3";
            th3.Start();

            //th4 = new Thread(RGVRun);
            th4 = new Thread(ExecuteRgvCommand);
            th4.IsBackground = true;
            th4.Name = "th4";
            th4.Start();

            //th5 = new Thread(DMXRun);
            th5 = new Thread(ExecuteRequestCommand);
            th5.IsBackground = true;
            th5.Name = "th5";
            th5.Start();

            //th6 = new Thread(JXSRun);
            th6 = new Thread(ExecuteSortingCommand);
            th6.IsBackground = true;
            th6.Name = "th6";
            //th6.Start();

            th8 = new Thread(ExecuteInOutStockCommand);
            th8.IsBackground = true;
            th8.Name = "th8";
            th8.Start();

            th9 = new Thread(ExecuteSortingOtherCommand);
            th9.IsBackground = true;
            th9.Name = "th9";
            th9.Start();

            th7 = new Thread(DeviceM);
            th7.IsBackground = true;
            th7.Name = "th7";
            th7.Start();

            th10 = new Thread(ExecuteCreatedSortingDetailCommand);
            th10.IsBackground = true;
            th10.Name = "th10";
            th10.Start();

            th11 = new Thread(ExecuteSortingLoadingFinishedCommand);
            th11.IsBackground = true;
            th11.Name = "th11";
            th11.Start();

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
                    else
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.menuStrip1.Enabled = true;
                        });
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog("MainThread：" + ex.Message, "ERROR");
                }
            }
        }

        //信号监控
        private void HsMonitor()
        {
            //int ddj = 1;
            while (true)
            {
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        DdjMonitor();
                        RgvMonitor();
                        JxsMonitor();
                        //LogJXS(1);
                    }
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    WCSSql.InsertLog(GetType()+ex.Message, "Error");
                }
            }
        }

        #region 设备监控
        void JxsMonitor()
        {
            for (int jxsno = 1; jxsno <= 3; jxsno++)
            {
                var jxs_msg = OpcHsc.GetJXSErrorMsg(jxsno);

                if (jxs_msg.Length > 0)
                {
                    WCSSql.InsertLog($"机械手[{jxsno}]故障：{jxs_msg}", "ERROR", 0, $"JXS{jxsno}");
                }
            }
        }

        //string[] msgs = { "", "", "", "" };
        //string[] dates = { "", "", "", "" };
        void DdjMonitor()
        {
            //堆垛机故障记录
            for (int j = 1; j <= 4; j++)
            {
                LogDdjInfo(j);

                var msg = OpcSc.GetSCErrorMsg(j);
                if (msg.Length > 0)
                {
                    var msgcn = WCSSql.GetSCErrorMsg(msg);
                    WCSSql.InsertLog($"堆垛机[{j}]故障：{msgcn}", "ERROR", 0, $"SC{j}");
                }
                //if (msg.Length > 0)
                //{
                //    msgs[j - 1] = msg;
                //    if (dates[j - 1].Length == 0)
                //    {
                //        dates[j - 1] = DateTime.Now.ToString();
                //    }
                //}
                //else
                //{
                //    if (msgs[j - 1].Length > 0)
                //    {
                //        var str = WCSSql.GetSCErrorMsg(msgs[j - 1]);
                //        str = str.Replace("\r\n", "|");
                //        WCSSql.InsertLog($"堆垛机[{j}]故障：{str}", dates[j - 1], DateTime.Now.ToString(), 0);
                //        msgs[j - 1] = "";
                //        dates[j - 1] = "";
                //    }
                //}
            }
        }

        void LogDdjInfo(int scNo)
        {
            var Info = new SCInfo();

            Info.IsActivation = OpcSc.RIsActivation(scNo) == true ? 1: 0 ;
            Info.IsAuto = OpcSc.IsAuto(scNo) == true ? 1 : 0;
            Info.IsFree = OpcSc.RIsFree(scNo) == true ? 1 : 0;
            Info.PilerNo= Convert.ToInt64(OpcSc.RTask(scNo));
            Info.TaskType= OpcSc.RTaskType(scNo);
            Info.TaskStatus = OpcSc.RTaskType(scNo);
            Info.CurrentPos= OpcSc.RLocation(scNo);
            Info.FromPos = "";
            Info.ToPos = OpcSc.RTo(scNo);
            Info.InStationState = OpcHsc.IsStationInRequest(scNo) ==true? 1 : 0;
            Info.OutStationState = OpcHsc.IsOutStationFree(scNo) == true ? 1 : 0;
            var ids = OpcSc.GetSCErrorMsg(scNo);
            Info.ErrorMsg = WCSSql.GetSCErrorMsg(ids);

            WCSSql.UpdateSCInfo(scNo, Info);
        } 

        void LogJXS(int sno)
        {
            //var info = new SCInfo();

            //info.IsActivation = OpcHsc.RMActivity(sno) ? 1 : 0;
            //info.IsAuto = OpcHsc.RMAuto(sno) ? 1 : 0;
            //info.IsFree = OpcHsc.RMFree(sno) ? 1 : 0;
            //info.FromPos = OpcHsc.RMFrom().ToString();
            //info.ToPos = OpcHsc.RMTo().ToString();

            //WCSSql.UpdateSCInfo(sno, info);
        }

        //static string GetHsMsgStr(int code)
        //{
        //    switch (code)
        //    {
        //        case 0:
        //            return "停止";
        //        case 1:
        //            return "急停";
        //        case 2:
        //            return "手动";
        //        case 4:
        //            return "线体故障";
        //        default:
        //            return "未知错误";
        //    }         
        //}

        //int scode = 0;
        //string sdate = "";

        void RgvMonitor()
        {
            LogRGVInfo();

            var rgv_msg = OpcHsc.GetRGVErrorMsg();
            if (rgv_msg.Length > 0)
            {
                WCSSql.InsertLog($"RGV故障：{rgv_msg}", "ERROR", 0, "RGV");
            }
            //if (code > 0)
            //{
            //    scode = code;
            //    if (sdate.Length == 0)
            //    {
            //        sdate = DateTime.Now.ToString();
            //    }
            //}
            //else
            //{
            //    if (scode > 0)
            //    {
            //        var code_2 = Convert.ToString(scode, 2);//十进制转二进制
            //        var msglst = "";
            //        for(int i = 0; i < code_2.Length; i++)
            //        {
            //            if (code_2[code_2.Length - 1 - i] == '1')
            //            {
            //                msglst += WCSSql.GetRGVErrorCommentByIndex(i + 1) + "|";
            //            }
            //        }
            //        WCSSql.InsertLog($"RGV故障：{msglst}", sdate, DateTime.Now.ToString(), 0);
            //        scode =0;
            //        sdate = "";
            //    }
            //}
        }

        void LogRGVInfo()
        {
            var Info = new SCInfo();

            Info.IsActivation = OpcHsc.IsRGVActication() ? 1 : 0;
            Info.IsAuto = OpcHsc.IsRGVAuto() ? 1 : 0;
            Info.IsFree = OpcHsc.IsRGVFree() ? 1 : 0;
            Info.PilerNo = OpcHsc.ReadRGVPilerNo();
            Info.TaskType = 0;
            Info.TaskStatus = OpcHsc.ReadRGVTaskStatus() ? 1 : 0;
            Info.CurrentPos = OpcHsc.ReadRGVPostion();
            Info.FromPos = OpcHsc.ReadRGVFromPostion();
            Info.ToPos = OpcHsc.ReadRGVToPostion();
            Info.InStationState = 0;
            Info.OutStationState = 0;

            WCSSql.UpdateSCInfo(5, Info);
        }
        #endregion

        //堆垛机运行
        private void DdjRun()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    //堆垛机完成任务
                    ScControl.GetInstance().DdjFinishAll();

                    //堆垛机出库
                    ScControl.GetInstance().DdjOutAll();

                    //堆垛机入库
                    ScControl.GetInstance().DdjAllIn();
                }

                Thread.Sleep(1000);
            }
        }

        private void RGVRun()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    //OpcHsc.ClearRGVTask();

                    //RGV完成任务
                    RGVControl.GetInstance().RgvFinishTask();

                    //给RGV写上料任务
                    RGVControl.GetInstance().WriteWmsTaskToRgv();

                    //RGV写任务
                    RGVControl.GetInstance().WriteRgvTask();
                }

                Thread.Sleep(1000);
            }
        
        }

        private void DMXRun()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    //空垫板补料
                    EmptyReq.ReqAll();

                    //入库信号
                    InWare.GetInstance().AllInWare();

                    //出库信号
                    OutWare.OutWareAll();
                }

                Thread.Sleep(1000);
            }
        }

        private void JXSRun()
        {
            while (true)
            {
                if (OPCExecute.IsConn)
                {
                    //机械手控制
                    Sorting.GetInstance().DoCmd();
                }

                Thread.Sleep(500);
            }
        }

        private void DeviceM()
        {
            while (true)
            {
                try
                {
                    if (OPCExecute.IsConn)
                    {
                        OpcHsc.MonitorDevice();

                        //设备错误监控
                        var dt = WCSSql.GetAllDevice();
                        var d_msg = "";
                        var dno = "";
                        foreach(DataRow dr in dt.Rows)
                        {
                            dno = dr["DeviceNo"].ToString();
                            d_msg = OpcHsc.ReadDeiviceErrorMsg(dno);
                            if (d_msg.Length > 0)
                            {
                                WCSSql.InsertLog($"[{dno}]{d_msg}", "ERROR", 0, dno);
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
                catch
                {
                }
            }
        }

        private void DemoOPCUA()
        {

            //if (OpcBaseManage.opcBase.GetCpu(0).isConnect == false) { return; }

            //DateTime s = DateTime.Now;

            //for (int i = 0; i < 50; i++)
            //{
            //    OpcHs.IsRGVActication();
            //}

            //DateTime e = DateTime.Now;

            //var c = (e - s).TotalMilliseconds;
        }

        private void DemoOPC()
        {

            if (OPCExecute.IsConn == false) { return; }

            DateTime s1 = DateTime.Now;

            for (int i = 0; i < 50; i++)
            {
                OpcSc.RIsFree(4);
            }

            DateTime e1 = DateTime.Now;

            var c1 = (e1 - s1).TotalMilliseconds;
        }

        #endregion
    }
}
