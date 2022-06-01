using System;
using System.Drawing;
using System.Windows.Forms;
using WCS.DataBase;
using System.Threading;
using System.Threading.Tasks;
using WCS.model;
using WCS.OPC;

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
                this.lbErrorMsg.Text = WCSSql.GetLastErrorMsg();
                //var s = DateTime.Now;

                MonitorSC();
                //var s1 = DateTime.Now;
                //var a1 = (s1 - s).TotalMilliseconds;

                MonitorRGV();
                //var s2 = DateTime.Now;
                //var a2 = (s2 - s1).TotalMilliseconds;

                if (OPCExecute.IsConn)
                {
                    MonitorJXS();
                }
                //var s3 = DateTime.Now;
                //var a3 = (s3 - s2).TotalMilliseconds;

                BindLog();
                //var s4 = DateTime.Now;
                //var a4 = (s4 - s3).TotalMilliseconds;

                //this.lbErrorMsg.Text = WCSSql.GetErrorMsgNow();
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog(ex.Message, "ERROR");
            }
        }

        //测试程序 OPC UA
        private void TestOPCUA(int times)
        {
            if (OpcBaseManage.opcBase.GetCpu(0).isConnect == false) { return; }

            int i = 0;
            while (i < times)
            {
                //OpcHs.IsRGVAuto();

                i++;
            }
        }

        //测试程序OPC Server
        private void TestOPC(int times)
        {
            if (OPCExecute.IsConn==false) { return; }

            int i = 0;
            while (i < times)
            {
                OpcSc.IsAuto(1);

                i++;
            }
        }

        //加载日志信息
        private void BindLog()
        {
            this.dvLog.DataSource = WCSSql.GetLogLst("LOG");
            this.dvLog.Columns["PilerNo"].Visible = false;
            this.dvLog.Columns["StartDate1"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dvLog2.DataSource = WCSSql.GetLogLst("ERROR");
            this.dvLog2.Columns["StartDate2"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dvLog2.Columns["EndDate2"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
        }

        private void MonitorSC()
        {
            for (int i = 1; i <= 4; i++)
            {
                var ddjInfo = WCSSql.GetSCInfo(i);
                var lbAuto = Controls.Find("lbAuto" + i, true)[0];
                var lbStatus = Controls.Find("lbStatus" + i, true)[0];
                var lbActivate = Controls.Find("lbActivate" + i, true)[0];
                var lbPilerNo = Controls.Find("lbPilerNo" + i, true)[0];
                var lbType = Controls.Find("lbType" + i, true)[0];
                var lbTaskStatus = Controls.Find("lbTaskStatus" + i, true)[0];
                var lbCurrent = Controls.Find("lbCurrent" + i, true)[0];
                var lbTo = Controls.Find("lbTo" + i, true)[0];
                var lbReq = Controls.Find("lbReq" + i, true)[0];
                var lbOut = Controls.Find("lbOut" + i, true)[0];
                //var lbError = Controls.Find("lbError" + i, true)[0];

                if (ddjInfo.IsAuto == 1)
                {
                    lbAuto.Text = "自动";
                    lbAuto.ForeColor = relax;
                }
                else
                {
                    lbAuto.Text = "手动";
                    lbAuto.ForeColor = error;
                }

                if (ddjInfo.IsFree == 1)
                {
                    lbStatus.Text = "空闲";
                    lbStatus.ForeColor = relax;
                }
                else
                {
                    lbStatus.Text = "正忙";
                    lbStatus.ForeColor = busy;
                }

                if (ddjInfo.IsActivation == 1)
                {
                    lbActivate.Text = "激活";
                    lbActivate.ForeColor = relax;
                }
                else
                {
                    lbActivate.Text = "关闭";
                    lbActivate.ForeColor = error;
                }

                lbPilerNo.Text = ddjInfo.PilerNo.ToString();
                lbType.Text = ddjInfo.TaskType == 1 ? "入库" : (ddjInfo.TaskType == 2 ? "出库" : "无工作");
                lbTaskStatus.Text = ddjInfo.TaskStatus == 1 ? "已接收" : "已完成";
                lbCurrent.Text = ddjInfo.CurrentPos;
                lbTo.Text = ddjInfo.ToPos;
                lbReq.Text = ddjInfo.InStationState == 1 ? "请求" : "";
                lbOut.Text = ddjInfo.OutStationState == 1 ? "占用" : "";
                //lbError.Text = ddjInfo.ErrorMsg;
            }
        }

        private void MonitorRGV()
        {
            var rgvInfo = WCSSql.GetSCInfo(5);

            if (rgvInfo.IsAuto == 1)
            {
                this.lbRGVAuto.Text = "自动";
                this.lbRGVAuto.ForeColor = relax;
            }
            else
            {
                this.lbRGVAuto.Text = "手动";
                this.lbRGVAuto.ForeColor = error;
            }

            if (rgvInfo.IsActivation == 1)
            {
                this.lbRGVActivatity.Text = "激活";
                this.lbRGVActivatity.ForeColor = relax;
            }
            else
            {
                this.lbRGVActivatity.Text = "关闭";
                this.lbRGVActivatity.ForeColor = error;
            }

            if (rgvInfo.IsFree == 1)
            {
                this.lbRGVFree.Text = "空闲";
                this.lbRGVFree.ForeColor = relax;
            }
            else
            {
                this.lbRGVFree.Text = "正忙";
                this.lbRGVFree.ForeColor = busy;
            }

            this.lbRGVPilerNo.Text = rgvInfo.PilerNo.ToString();
            this.lbRGVFrom.Text = rgvInfo.FromPos;
            this.lbRGVTo.Text = rgvInfo.ToPos;
            this.lbRGVPosition.Text = rgvInfo.CurrentPos;
            this.lbRGVStatus.Text = rgvInfo.TaskStatus == 1 ? "已接收" : "已完成";

            //int code = OpcHs.RRGVMsg();
            //if (code == 0)
            //{
            //    this.lbRGVMsg.Text = "";
            //}
            //else
            //{
            //    this.lbRGVMsg.Text = code.ToString();
            //}
        }

        private void MonitorJXS()
        {
            //1#机械手
            var IsActivation1 = OpcHsc.RMActivity(1);
            if (IsActivation1)
            {
                this.lbMActication1.Text = "激活";
                this.lbMActication1.ForeColor = relax;
            }
            else
            {
                this.lbMActication1.Text = "关闭";
                this.lbMActication1.ForeColor = error;
            }

            var IsAuto1 = OpcHsc.RMAuto(1);
            if (IsAuto1)
            {
                this.lbMAuto1.Text = "自动";
                this.lbMAuto1.ForeColor = relax;
            }
            else
            {
                this.lbMAuto1.Text = "手动";
                this.lbMAuto1.ForeColor = error;
            }

            var IsFree1 = OpcHsc.RMFree(1);
            if (IsFree1)
            {
                this.lbMFree1.Text = "空闲";
                this.lbMFree1.ForeColor = relax;
            }
            else
            {
                this.lbMFree1.Text = "正忙";
                this.lbMFree1.ForeColor = busy;
            }

            this.lbMFrom1.Text = OpcHsc.RMFrom().ToString();
            this.lbMTo1.Text = OpcHsc.RMTo().ToString();

            //2#机械手
            var IsActivation2 = OpcHsc.RMActivity(2);
            if (IsActivation2)
            {
                this.lbMActication2.Text = "激活";
                this.lbMActication2.ForeColor = relax;
            }
            else
            {
                this.lbMActication2.Text = "关闭";
                this.lbMActication2.ForeColor = error;
            }

            var IsAuto2 = OpcHsc.RMAuto(2);
            if (IsAuto2)
            {
                this.lbMAuto2.Text = "自动";
                this.lbMAuto2.ForeColor = relax;
            }
            else
            {
                this.lbMAuto2.Text = "手动";
                this.lbMAuto2.ForeColor = error;
            }

            var IsFree2 = OpcHsc.RMFree(2);
            if (IsFree2)
            {
                this.lbMFree2.Text = "空闲";
                this.lbMFree2.ForeColor = relax;
            }
            else
            {
                this.lbMFree2.Text = "正忙";
                this.lbMFree2.ForeColor = busy;
            }
            this.lbEmptCount2.Text = OpcHsc.ReadGT308Borads();

            //3#机械手
            var IsActivation3 = OpcHsc.RMActivity(3);
            if (IsActivation3)
            {
                this.lbMActication3.Text = "激活";
                this.lbMActication3.ForeColor = relax;
            }
            else
            {
                this.lbMActication3.Text = "关闭";
                this.lbMActication3.ForeColor = error;
            }

            var IsAuto3 = OpcHsc.RMAuto(3);
            if (IsAuto3)
            {
                this.lbMAuto3.Text = "自动";
                this.lbMAuto3.ForeColor = relax;
            }
            else
            {
                this.lbMAuto3.Text = "手动";
                this.lbMAuto3.ForeColor = error;
            }

            var IsFree3 = OpcHsc.RMFree(3);
            if (IsFree3)
            {
                this.lbMFree3.Text = "空闲";
                this.lbMFree3.ForeColor = relax;
            }
            else
            {
                this.lbMFree3.Text = "正忙";
                this.lbMFree3.ForeColor = busy;
            }
            this.lbEmptCount3.Text = OpcHsc.ReadGT318Borads();
        }

        private void MonitorHs()
        {

        }

        private void MonitorM()
        {
            //if(OpcHs)
        }

    }
}
