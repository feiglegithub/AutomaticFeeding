using System;
using System.Drawing;
using System.Windows.Forms;
using WCS.DataBase;
using WCS.model;

namespace WCS.Forms
{
    public partial class HomeFrm : Form
    {
        Color error = Color.Red;
        Color busy = Color.Yellow;
        Color relax = Color.Lime;
        public HomeFrm()
        {
            InitializeComponent();
            this.Activated += HomeFrm_Activated;
            this.Deactivate += HomeFrm_Deactivate;
            AutoScales(this);
        }

        //窗体被激活
        private void HomeFrm_Deactivate(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void HomeFrm_Activated(object sender, EventArgs e)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                BindLog();
                MonitorSC();
            }
            catch { }
        }

        //加载日志信息
        private void BindLog()
        {
            this.dvLog.DataSource = WcsSqlB.GetLog();
            //this.dvLog.Columns["StartDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //this.dvLog.Columns["EndDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            this.lbError.Text = WcsSqlB.GetLastErrorMsg();
        }

        //堆垛机监控
        private void MonitorSC()
        {
            if (!OPCExecute.IsConn) { return; }

            ShowDdjInfo(1, 1072, 5079, 5036, this.lbActivate1, this.lbAuto1, this.lbStatus1, this.lbTaskNo1, this.lbTaskTpye1, this.lbTaskStatus1, this.lbCurrent1, this.lbOut1F1, this.lbReq1, this.lbOut2F1, this.txtSCE1);
            ShowDdjInfo(2, 1065, 5087, 5044, this.lbActivate2, this.lbAuto2, this.lbStatus2, this.lbTaskNo2, this.lbTaskTpye2, this.lbTaskStatus2, this.lbCurrent2, this.lbOut1F2, this.lbReq2, this.lbOut2F2, this.txtSCE2);
            ShowDdjInfo(3, 1058, 5095, 5052, this.lbActivate3, this.lbAuto3, this.lbStatus3, this.lbTaskNo3, this.lbTaskTpye3, this.lbTaskStatus3, this.lbCurrent3, this.lbOut1F3, this.lbReq3, this.lbOut2F3, this.txtSCE3);
            ShowDdjInfo(4, 1051, 5103, 5060, this.lbActivate4, this.lbAuto4, this.lbStatus4, this.lbTaskNo4, this.lbTaskTpye4, this.lbTaskStatus4, this.lbCurrent4, this.lbOut1F4, this.lbReq4, this.lbOut2F4, this.txtSCE4);
            ShowDdjInfo(5, 1044, 5111, 5068, this.lbActivate5, this.lbAuto5, this.lbStatus5, this.lbTaskNo5, this.lbTaskTpye5, this.lbTaskStatus5, this.lbCurrent5, this.lbOut1F5, this.lbReq5, this.lbOut2F5, this.txtSCE5);
            ShowDdjInfo(6, 1037, 5119, 5076, this.lbActivate6, this.lbAuto6, this.lbStatus6, this.lbTaskNo6, this.lbTaskTpye6, this.lbTaskStatus6, this.lbCurrent6, this.lbOut1F6, this.lbReq6, this.lbOut2F6, this.txtSCE6);
            ShowDdjInfo(7, 1029, 4122, 4037, this.lbActivate7, this.lbAuto7, this.lbStatus7, this.lbTaskNo7, this.lbTaskTpye7, this.lbTaskStatus7, this.lbCurrent7, this.lbOut1F7, this.lbReq7, this.lbOut2F7, this.txtSCE7);
            ShowDdjInfo(8, 1022, 4131, 4030, this.lbActivate8, this.lbAuto8, this.lbStatus8, this.lbTaskNo8, this.lbTaskTpye8, this.lbTaskStatus8, this.lbCurrent8, this.lbOut1F8, this.lbReq8, this.lbOut2F8, this.txtSCE8);
            ShowDdjInfo(9, 1015, 4139, 4023, this.lbActivate9, this.lbAuto9, this.lbStatus9, this.lbTaskNo9, this.lbTaskTpye9, this.lbTaskStatus9, this.lbCurrent9, this.lbOut1F9, this.lbReq9, this.lbOut2F9, this.txtSCE9);
            ShowDdjInfo(10, 1008, 4147, 4016, this.lbActivate10, this.lbAuto10, this.lbStatus10, this.lbTaskNo10, this.lbTaskTpye10, this.lbTaskStatus10, this.lbCurrent10, this.lbOut1F10, this.lbReq10, this.lbOut2F10, this.txtSCE10);
            ShowDdjInfo(11, 1001, 4155, 4009, this.lbActivate11, this.lbAuto11, this.lbStatus11, this.lbTaskNo11, this.lbTaskTpye11, this.lbTaskStatus11, this.lbCurrent11, this.lbOut1F11, this.lbReq11, this.lbOut2F11, this.txtSCE11);
        }

        private void ShowDdjInfo(int ddjno, int out1no, int out2no, int stationin, Label lb1, Label lb2, Label lb3, Label lb4,
            Label lb5, Label lb6, Label lb7, Label lb8, Label lb9, Label lb10, TextBox txt)
        {
            if (OPCHelper.IsAlarm(ddjno))
            {
                lb1.Text = "报警";
                lb1.ForeColor = error;
            }
            else
            {
                lb1.Text = "正常";
                lb1.ForeColor = relax;
            }

            if (OPCHelper.IsAuto(ddjno))
            {
                lb2.Text = "自动";
                lb2.ForeColor = relax;
            }
            else
            {
                lb2.Text = "手动";
                lb2.ForeColor = error;
            }

            if (OPCHelper.IsFree(ddjno))
            {
                lb3.Text = "空闲";
                lb3.ForeColor = relax;
            }
            else
            {
                lb3.Text = "忙碌";
                lb3.ForeColor = busy;
            }

            lb4.Text = OPCHelper.ReadSCTaskId(ddjno).ToString();
            lb5.Text = OPCHelper.ReadSCTaskType(ddjno);
            lb6.Text = OPCHelper.ReadTaskStatus(ddjno) == 0 ? "已接收" : "已完成";
            lb7.Text = $"{OPCHelper.ReadColumn(ddjno)}.{OPCHelper.ReadLayer(ddjno)}";

            if (OPCHelper.ReadTarget(out1no) > 0)
            {
                lb8.Text = "占用";
                lb8.ForeColor = busy;
            }
            else
            {
                lb8.Text = "空闲";
                lb8.ForeColor = relax;
            }

            if (OPCHelper.ReadTarget(stationin) == 999)
            {
                lb9.Text = "请求";
                lb9.ForeColor = busy;
            }
            else
            {
                lb9.Text = "空闲";
                lb9.ForeColor = relax;
            }

            if (OPCHelper.ReadTarget(out2no) > 0)
            {
                lb10.Text = "占用";
                lb10.ForeColor = busy;
            }
            else
            {
                lb10.Text = "空闲";
                lb10.ForeColor = relax;
            }

            //var arr = OPCHelper.ReadSCCode(i);
            //var msg = WcsSqlB.GetSCMsg(arr);
            var arr = OPCHelper.ReadSCCode(ddjno);
            var msg = WcsSqlB.GetSCMsg(arr);
            txt.Text = msg;
        }

        private void MonitorM()
        {
            //if(OpcHs)
        }

        private void HomeFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
