using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS.Forms
{
    public partial class WriteDataFrm : Form
    {
        private int _no;
        public WriteDataFrm(int no)
        {
            InitializeComponent();
            this._no = no;
            this.lbDeviceNo.Text = no.ToString();
            BindData();
        }

        private void BindData()
        {
            var d = OPCHelper.GetDeviceByNo(_no);
            this.txtPilerNo.Text = d.DWorkId.ToString();
            this.txtPallet.Text = d.DPalletId;
            this.txtTarget.Text = d.DTarget.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                OPCHelper.WriteTaskId(_no, int.Parse(this.txtPilerNo.Text));
                OPCHelper.WriteTarget(_no, int.Parse(this.txtTarget.Text));
                OPCHelper.WritePallet(_no, this.txtPallet.Text);
                MessageBox.Show("写入数据成功！");
            }
            catch
            {
                MessageBox.Show("写入数据失败！");
            }
        }
    }
}
