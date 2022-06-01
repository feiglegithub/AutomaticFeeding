using System;
using System.Windows.Forms;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms
{
    public partial class PassWordForm : Telerik.WinControls.UI.RadForm
    {
        public PassWordForm()
        {
            InitializeComponent();
        }

        private string passWord = "00944";
        private bool _IsPass = false;
        public bool IsPass => _IsPass;

        private void btnSure_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {
                RadMessageBox.Show(this, "密码不可为空");
                return;
            }

            if (passWord == txtPassword.Text.Trim())
            {
                _IsPass = true;
            }
            else
            {
                RadMessageBox.Show(this, "密码错误");
                return;
            }
            OpDialogResult = DialogResult.Yes;

            Close();
        }
        public DialogResult OpDialogResult = DialogResult.Cancel;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
