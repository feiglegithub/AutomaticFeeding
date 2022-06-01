using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.OPC;

namespace WCS.Forms
{
    public partial class WriteDataFrm : Form
    {
        private Button _btn;
        public WriteDataFrm(Button btn)
        {
            InitializeComponent();
            this._btn = btn;
            this.lbDeviceNo.Text = btn.Text;
            this.Load += WriteDataFrm_Load;
        }

        private void WriteDataFrm_Load(object sender, EventArgs e)
        {
            var dd = OpcHsc.ReadDeviceDataByNo(this._btn.Text);
            this.txtPilerNo.Text = dd.PilerNo.ToString();
            this.txtTarget.Text = dd.Target.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                OpcHsc.WriteDeviceData(this._btn.Text, int.Parse(this.txtPilerNo.Text), int.Parse(this.txtTarget.Text));
                MessageBox.Show("写入数据成功！");
            }
            catch
            {
                MessageBox.Show("写入数据失败！");
            }
        }
    }
}
