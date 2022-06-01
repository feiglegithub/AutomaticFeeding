using NJIS.Windows.TemplateBase.UI.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace NJIS.Windows.TemplateBase.UI.Dialogs
{
    public partial class FormDlg : Telerik.WinControls.UI.RadForm
    {
        UserControlBase UC;
        public FormDlg()
        {
            InitializeComponent();
        }
        public FormDlg(string text, UserControlBase control)
        {
            InitializeComponent();
            Width = control.Width + pnlContainer.Left*2;
            Height = control.Height + pnlContainer.Top+35;
            Text = text;
            UC = control;
            UC.Dock = DockStyle.Fill;
            radLabel1.Text = text;

            UC.DataRequire();

            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(UC);
        }


        public void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogEventArgs arg = new DialogEventArgs() { Result = DialogResult.OK, ReturnValue = UC.Arguments };
            UC.DataCommitted(arg);
            if (!arg.Cancel)
                DialogResult = DialogResult.OK;
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //UC.DataCommitted(new DialogEventArgs() { Result = DialogResult.Cancel, ReturnValue = UC.Arguments });
            //DialogResult = DialogResult.Cancel;
        }
    }
}
