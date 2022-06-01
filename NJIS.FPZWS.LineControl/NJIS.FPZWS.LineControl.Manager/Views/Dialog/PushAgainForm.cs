using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    public partial class PushAgainForm : Form
    {
        public DialogResult dialogResult = DialogResult.Cancel;
        public int boardCount = 0;
        public PushAgainForm()
        {
            InitializeComponent();
        }

        private void radTextBoxBoardCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void radButtonOK_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            string countStr = radTextBoxBoardCount.Text;
            if (!string.IsNullOrEmpty(countStr))
            {
                boardCount = int.Parse(countStr);
                if (boardCount > 0 )
                {
                    this.Close();
                }
            }
        }

        private void radButtonCancel_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
