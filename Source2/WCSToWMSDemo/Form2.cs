using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCSToWMSDemo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(this.txtPort.Text, out port))
            {
                MessageBox.Show("上架入口请输入数字！");
                return;
            }

            if (this.txtPallet.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入托盘条码！");
                return;
            }

            var msg = "";
            if (this.cboType.Text == "整盘入库")
            {
                msg = DemoSql.CreateInTask(this.txtPallet.Text, port, int.Parse(this.cboH.Text));
            }
            else if (this.cboType.Text == "空盘入库")
            {
                msg = DemoSql.CreateEmptyInTask("", port, int.Parse(this.cboH.Text));
            }
            else if (this.cboType.Text == "空盘出库")
            {
                msg = DemoSql.CreateEmptyOutTask(port);
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
            }
            else
            {
                MessageBox.Show("WMS任务分配成功！");
            }
        }
    }
}
