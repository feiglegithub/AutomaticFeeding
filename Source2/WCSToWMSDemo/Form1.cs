using System;
using System.Windows.Forms;

namespace WCSToWMSDemo
{
    public partial class cboType : Form
    {
        public cboType()
        {
            InitializeComponent();
        }

        //查询
        private void button3_Click(object sender, EventArgs e)
        {
            string where = $" and DoptDate between '{this.dt1.Value.ToString("yyyy-MM-dd 0:00:00")}' and '{this.dt2.Value.ToString("yyyy-MM-dd 23:59:59")}' ";

            //任务类型
            if (this.cbType.Text.Trim().Length > 0)
            {
                if (this.cbType.Text.Equals("整盘入库"))
                {
                    where += " and NordID=1 ";
                }
                else if (this.cbType.Text.Equals("整盘出库"))
                {
                    where += " and NordID=3 ";
                }
                else if (this.cbType.Text.Equals("空盘入库"))
                {
                    where += " and NordID=5 ";
                }
                else if (this.cbType.Text.Equals("空盘出库"))
                {
                    where += " and NordID=6 ";
                }
            }

            //任务状态
            if (this.cbStatus.Text.Trim().Length > 0)
            {
                if (this.cbStatus.Text.Equals("已分配"))
                {
                    where += " and NwkStatus=1 ";
                }
                else if (this.cbStatus.Text.Equals("执行中"))
                {
                    where += " and NwkStatus=10 ";
                }
                else if (this.cbStatus.Text.Equals("已过汇流口"))
                {
                    where += " and NwkStatus=12 ";
                }
                else if (this.cbStatus.Text.Equals("已完成"))
                {
                    where += " and NwkStatus=99 ";
                }
                else if (this.cbStatus.Text.Equals("已取消"))
                {
                    where += " and NwkStatus=999 ";
                }
            }

            var dt = DemoSql.GetTaskDemo(where);
            this.dvTask.DataSource = dt;
        }

        //调用WMS接口执行中
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dvTask.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择任务！");
                return;
            }

            var row = this.dvTask.SelectedRows[0];
            var msg = DemoSql.UpdateWMSTask(row.Cells["SeqId"].Value.ToString(), 10);

            MessageBox.Show(msg.Length > 0 ? msg : "更新状态成功！");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dvTask.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择任务！");
                return;
            }

            var row = this.dvTask.SelectedRows[0];
            var msg = DemoSql.UpdateWMSTask(row.Cells["SeqId"].Value.ToString(), 98);

            MessageBox.Show(msg.Length > 0 ? msg : "更新状态成功！");
        }

        //上架申请
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        //更新汇流口
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.dvTask.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择任务！");
                return;
            }

            var row = this.dvTask.SelectedRows[0];

            if (!row.Cells["NordID"].Value.ToString().Equals("整盘出库"))
            {
                MessageBox.Show("只有整盘出库才能过汇流口！");
                return;
            }

            var msg = DemoSql.UpdateWMSTask(row.Cells["SeqId"].Value.ToString(), 12);

            MessageBox.Show(msg.Length > 0 ? msg : "更新状态成功！");
        }

        //空盘下架申请
        private void button6_Click(object sender, EventArgs e)
        {
            int port;
            if(!int.TryParse(this.txtEmptyPort.Text,out port))
            {
                MessageBox.Show("空托盘出库口请输入数字！");
                return;
            }

            var msg = DemoSql.CreateEmptyOutTask(port);

            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
            }
            else
            {
                MessageBox.Show("空托盘下架申请成功！");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();

            f.ShowDialog();
        }
    }
}
