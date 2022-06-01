using System;
using System.Drawing;
using System.Windows.Forms;
using WCS.DataBase;
using CommomLY1._0;
using System.Data;
using System.IO;

namespace WCS.Forms
{
    public partial class TaskFrm : Form
    {
        public string rightpsd = "190512";
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
            this.dtpEnd1.Value = this.dtpStart1.Value;
        }

        private void btnSCControl_Click(object sender, EventArgs e)
        {
            var frmCheck = new FrmCheckIn();
            frmCheck.ShowDialog();

            if (!rightpsd.Equals(frmCheck.PassWord))
            {
                MessageBox.Show("操作密码错误！");
                return;
            }

            var scfrm = new SCControlFrm();
            scfrm.ShowDialog();
        }

        int pageIndex = 1;
        int pageCount = 36;
        //任务查询
        private void btnSelect_Click(object sender, EventArgs e)
        {
            string where = "";
            where += $" and DoptDate between '{this.dtpStart1.Value.ToString("yyyy-MM-dd HH:mm:ss")}' and '{this.dtpEnd1.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ";

            if (this.cboTaskType.Text == "整盘入库")
            {
                where += " and NordID=1 ";
            }
            else if (this.cboTaskType.Text == "整盘出库")
            {
                where += " and NordID=3 ";
            }
            else if (this.cboTaskType.Text == "空盘入库")
            {
                where += " and NordID=5 ";
            }
            else if (this.cboTaskType.Text == "空盘出库")
            {
                where += " and NordID=6 ";
            }

            if (this.cboStatus.Text == "已下发")
            {
                where += " and NwkStatus=1 ";
            }
            else if (this.cboStatus.Text == "执行中")
            {
                where += " and NwkStatus>1 and NwkStatus<98 ";
            }
            else if (this.cboStatus.Text == "WCS已完成")
            {
                where += " and NwkStatus=98 ";
            }
            else if (this.cboStatus.Text == "WMS已完成")
            {
                where += " and NwkStatus=99 ";
            }
            else if (this.cboStatus.Text == "已取消")
            {
                where += " and NwkStatus=999 ";
            }

            int tid;
            if (int.TryParse(this.txtTaskId.Text, out tid))
            {
                where += $" and SeqID like '%{tid}' ";
            }

            if (this.txtPallet.Text != "")
            {
                where += $" and NPalletID like '%{this.txtPallet.Text}' ";
            }

            if(this.txtCell.Text != "")
            {
                where += $" and (CPosidFrom='{this.txtCell.Text}' or CPosidTo='{this.txtCell.Text}') ";
            }

            int ddjNo;
            if (int.TryParse(this.txtDdj.Text, out ddjNo))
            {
                where += $" and Roadway={ddjNo} ";
            }

            if (this.txtNoptStation.Text != "")
            {
                where += $" and NoptStation={this.txtNoptStation.Text} ";
            }

            var dt = WcsSqlB.SelectTaskList(where);

            //分页
            if (sender != null) { pageIndex = 1; }
            dvTasks.DataSource = dt.TakeRows(pageCount, pageIndex);
            this.lbRowsCount.Text = "总行数：" + dt.Rows.Count.ToString();
            this.lbPages.Text = "总页数：" + (dt.Rows.Count % pageCount == 0 ? (dt.Rows.Count / pageCount) : (dt.Rows.Count / pageCount + 1)).ToString();
            this.lbPageIndex.Text = "当前页：" + pageIndex.ToString();
        }

        //上一页
        private void lkbPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (pageIndex >= 2)
            {
                pageIndex--;
                btnSelect_Click(null, null);
            }
        }

        //下一页
        private void lkbNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pageIndex++;
            btnSelect_Click(null, null);
        }

        //WCS请求查询
        private void btnSelectReq_Click(object sender, EventArgs e)
        {
            //this.dvRequests.DataSource = WCSSql.SelectReqList("").Tables[0];
        }

        //手动过账 任务
        private void btnFinishByHand_Click(object sender, EventArgs e)
        {
            var frmCheck = new FrmCheckIn();
            frmCheck.ShowDialog();

            if (!rightpsd.Equals(frmCheck.PassWord))
            {
                MessageBox.Show("操作密码错误！");
                return;
            }

            if (this.dvTasks.SelectedRows.Count > 0)
            {
                var row = this.dvTasks.SelectedRows[0];
                if (Convert.ToInt32(row.Cells["NwkStatus"].Value) >= 98)
                {
                    return;
                }
                var rlt = WcsSqlB.FinishTaskByHand(row.Cells["SeqID"].Value.ToString());
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

        //手动还原任务
        private void btnReback_Click(object sender, EventArgs e)
        {
            var frmCheck = new FrmCheckIn();
            frmCheck.ShowDialog();

            if (!rightpsd.Equals(frmCheck.PassWord))
            {
                MessageBox.Show("操作密码错误！");
                return;
            }


            if (this.dvTasks.SelectedRows.Count > 0)
            {
                var row = this.dvTasks.SelectedRows[0];
                if (Convert.ToInt32(row.Cells["NwkStatus"].Value) >= 31)
                {
                    return;
                }
                var rlt = WcsSqlB.UpdateWMSTask(row.Cells["SeqID"].Value.ToString(), 1);
                //var rlt = WcsSqlB.ReBackTaskByHand(row.Cells["SeqID"].Value.ToString());
                if (rlt.Length == 0)
                {
                    MessageBox.Show("任务还原成功！");
                }
                else
                {
                    MessageBox.Show("任务还原失败！");
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.dvTasks.Rows.Count > 0)
            {
                var rlt = this.folderBrowserDialog1.ShowDialog();
                if (rlt == DialogResult.OK)
                {
                    //this.folderBrowserDialog1.SelectedPath
                    var path = $@"{this.folderBrowserDialog1.SelectedPath}\{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
                    if (SaveCSV((DataTable)this.dvTasks.DataSource, path))
                    {
                        MessageBox.Show("数据导出成功！");
                    }
                }
            }
        }

        //dt 要导出的DataTable
        //fullFileName CSV的保存路径 例：D:\\1.csv
        public static bool SaveCSV(DataTable dt, string fullFileName)
        {
            bool r = false;
            FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString().Replace(",", "，");
                    //data = data.Replace(",", "，");
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();

            r = true;
            return r;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frmCheck = new FrmCheckIn();
            frmCheck.ShowDialog();

            if (!rightpsd.Equals(frmCheck.PassWord))
            {
                MessageBox.Show("操作密码错误！");
                return;
            }

            var frm = new FRIDControl();
            frm.ShowDialog();
        }
    }
}
