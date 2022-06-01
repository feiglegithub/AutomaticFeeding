using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS.Forms
{
    public partial class RGVTaskFrm : Form
    {
        public RGVTaskFrm()
        {
            InitializeComponent();
            this.Load += RGVTaskFrm_Load;
        }

        private void RGVTaskFrm_Load(object sender, EventArgs e)
        {
            this.dtpStart.Value = DateTime.Now;
            this.dtpEnd.Value = this.dtpStart.Value;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string where = $" and CreateTime between '{this.dtpStart.Value.ToString("yyyy-MM-dd 0:00:00")}' and '{this.dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59")}' ";

            if (this.cboStatus.Text == "已下发")
            {
                where += " and [Status]=1 ";
            }
            else if (this.cboStatus.Text == "执行中")
            {
                where += " and [Status]=20 ";
            }
            else if (this.cboStatus.Text == "已完成")
            {
                where += " and [Status]=98 ";
            }

            int pilerno;
            if (int.TryParse(this.txtPilerNo.Text, out pilerno))
            {
                where += $" and PilerNo={pilerno} ";
            }

            int FromPostion;
            if (int.TryParse(this.txtFromPostion.Text, out FromPostion))
            {
                where += $" and FromPosition={FromPostion} ";
            }

            int ToPosition;
            if (int.TryParse(this.txtToPosition.Text, out ToPosition))
            {
                where += $" and ToPosition={ToPosition} ";
            }

            BindDataList(where);
        }

        void BindDataList(string where)
        {
            this.dvTasks.DataSource = WCSSql.GetRGVTaskList(where);
            this.dvTasks.Columns["TaskId"].Visible = false;
            this.dvTasks.Columns["TaskType"].Visible = false;
        }

        //模拟上料
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.cboFrom.Text) == 105)
            {
                MessageBox.Show("Rgv任务起始位置不能为105");
                return;
            }
            var ran = new Random();
            var p = ran.Next(1000, 99999999);

            WCSSql.CreateRGVTask(new RGVTask() { TaskType = 1, FromPosition = int.Parse(this.cboFrom.Text), ToPosition = int.Parse(this.cboTo.Text), PilerNo = p });
            WCSSql.InsertLog($"手动创建RGV任务成功！垛号：{p}，起始位：{this.cboFrom.Text}，目标位：{this.cboTo.Text}", "LOG");

            MessageBox.Show("RGV任务创建成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //OpcHs.WriteInWareData(105, 0, 1001);
        }

        private void btnAutoLoading_Click(object sender, EventArgs e)
        {


            int startStation = int.Parse(this.cboFrom.Text);

            if (startStation != 105)
            {
                MessageBox.Show("半自动任务起始位置只能为：105");
                return;
            }

            int targetStation = int.Parse(this.cboTo.Text);

            if (!(targetStation == 3006 || targetStation == 3007))
            {
                MessageBox.Show("目标位置只能为：3006和3007");
                return;
            }

            try
            {
                WCSSql.CreatedCutting67ManualTask(targetStation.ToString());
                MessageBox.Show("创建成功！");
            }
            catch (Exception exception)
            {
                MessageBox.Show("创建失败！" + exception.Message);
                return;
            }


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            OpcHsc.FeedBackRGV();
            //OPCExecute.AsyncWrite(5, 3, 1);
        }

        private void btnDeleteRgvTask_Click(object sender, EventArgs e)
        {
            if (dvTasks.SelectedRows.Count > 0)
            {

                var rTaskId = int.Parse(dvTasks.SelectedRows[0].Cells["RTaskId"].Value.ToString());
;
                var ret = WCSSql.DeleteRgvTask(rTaskId);
                MessageBox.Show($"删除任务{(ret ? "成功" : "失败")}");
            }
            
        }
    }
}
