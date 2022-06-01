using System;
using System.Drawing;
using System.Windows.Forms;
using WCS.DataBase;
using WCS.model;

namespace WCS.Forms
{
    public partial class TaskFrm : Form
    {

        InWareFrm iwf; //入库窗口
        public TaskFrm()
        {
            InitializeComponent();
            AutoScales(this);
            this.Load += TaskFrm_Load;
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


        private void TaskFrm_Load(object sender, EventArgs e)
        {
            this.dtpStart1.Value = DateTime.Now;
            //this.dtpStart2.Value = this.dtpStart1.Value;

            this.dtpEnd1.Value = this.dtpStart1.Value;
            //this.dtpEnd2.Value = this.dtpStart1.Value;
        }

        //任务查询
        private void btnSelect_Click(object sender, EventArgs e)
        {
            string where = "";
            where += $" and wt.CreateTime between '{this.dtpStart1.Value.ToString("yyyy-MM-dd 0:00:00")}' and '{this.dtpEnd1.Value.ToString("yyyy-MM-dd 23:59:59")}' ";

            if (this.cboTaskType.Text == "入库")
            {
                where += " and wt.TaskType=1 ";
            }
            else if (this.cboTaskType.Text == "出库")
            {
                where += " and wt.TaskType=2 ";
            }
            else if (this.cboTaskType.Text == "拣选")
            {
                where += " and wt.TaskType=3 ";
            }
            else if (this.cboTaskType.Text == "垛转移")
            {
                where += " and wt.TaskType=6 ";
            }
            else if (this.cboTaskType.Text == "上料")
            {
                where += " and wt.TaskType=4 ";
            }

            if (this.cboStatus.Text == "已下发")
            {
                where += " and wt.TaskStatus=1 ";
            }
            else if (this.cboStatus.Text == "执行中")
            {
                where += " and wt.TaskStatus>1 and wt.TaskStatus<98 ";
            }
            else if (this.cboStatus.Text == "WCS已完成")
            {
                where += " and wt.TaskStatus=98 ";
            }
            else if (this.cboStatus.Text == "WMS已完成")
            {
                where += " and wt.TaskStatus=99 ";
            }
            else if (this.cboStatus.Text == "已取消")
            {
                where += " and wt.TaskStatus=999 ";
            }

            int pilerno;
            if (int.TryParse(this.txtPilerNo.Text, out pilerno))
            {
                where += $" and wt.PilerNo={pilerno} ";
            }

            if (this.txtFromPostion.Text != "")
            {
                where += $" and wt.FromPosition='{this.txtFromPostion.Text}' ";
            }

            if (this.txtToPosition.Text != "")
            {
                where += $" and wt.ToPosition='{this.txtToPosition.Text}' ";
            }

            int ddjNo;
            if (int.TryParse(this.txtDdj.Text, out ddjNo))
            {
                where += $" and wt.DdjNo={ddjNo} ";
            }

            this.dvTasks.DataSource = WCSSql.SelectTaskList(where).Tables[0];
            this.dvTasks.Columns["TaskType"].Visible = false;
        }

        //WCS请求查询
        private void btnSelectReq_Click(object sender, EventArgs e)
        {
            //this.dvRequests.DataSource = WCSSql.SelectReqList("").Tables[0];
        }

        //手动过账 任务
        private void btnFinishByHand_Click(object sender, EventArgs e)
        {
            if (this.dvTasks.SelectedRows.Count > 0)
            {
                var row = this.dvTasks.SelectedRows[0];
                if (Convert.ToInt32(row.Cells["TaskStatus"].Value) >= 98)
                {
                    MessageBox.Show("任务不能过账！");
                    return;
                }
                var rlt = WCSSql.FinishTaskByHand(row.Cells["TaskId"].Value.ToString());
                if (rlt > 0)
                {
                    MessageBox.Show("任务过账成功！");
                }
                else
                {
                    MessageBox.Show("任务过账失败！"); 
                }
            }
        }


        //模拟WMS任务请求
        private void button1_Click(object sender, EventArgs e)
        {
            //var request = new RequestInfo();
            //request.ReqType = int.Parse(this.tb1.Text);
            //if (request.ReqType == 1)
            //{
            //    //拣选入库请求
            //    request.TaskId = long.Parse(this.tb2.Text); //拣选任务ID
            //    request.FromPosition = this.tb3.Text;
            //}
            //else if (request.ReqType == 2)
            //{
            //    //要料请求
            //    request.ProductCode = this.tb4.Text;
            //    request.Amount = int.Parse(this.tb5.Text);
            //    request.ToPosition = this.tb6.Text;
            //}
            //else if (request.ReqType == 3)
            //{
            //    //空垫板入库请求
            //    request.Amount = int.Parse(this.tb5.Text);
            //    request.FromPosition = this.tb3.Text;
            //}
            //else if (request.ReqType == 4)
            //{
            //    //余料回库请求
            //    request.Amount = int.Parse(this.tb5.Text);
            //    request.FromPosition = this.tb3.Text;
            //    request.PilerNo = int.Parse(this.tb7.Text);
            //}

            //var feedback = WCSSql.RequestTask(request);

            //if (feedback.Status == 200)
            //{
            //    MessageBox.Show(feedback.ReqId.ToString() + feedback.Message);
            //}
            //else
            //{
            //    MessageBox.Show(feedback.Status.ToString() + feedback.Message);
            //}
        }

        private void btnInWare_Click(object sender, EventArgs e)
        {
            if(iwf==null || iwf.IsDisposed)
            {
                iwf = new InWareFrm();
            }

            iwf.Show();
        }
    }
}
