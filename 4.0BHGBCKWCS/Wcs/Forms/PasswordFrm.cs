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
    public partial class PasswordFrm : Form
    {
        public PasswordFrm()
        {
            InitializeComponent();
        }
        public DialogResult DialogResult { get; private set; }
        private string _password = "";
        public string Password => _password;

        private void btnSure_Click(object sender, EventArgs e)
        {
            var password = this.txtPaasWord.Text.Trim();
            if(string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("密码不可为空");
                return;
            }
            _password = password;
            if(password!="00544")
            {
                MessageBox.Show("密码不正确");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
