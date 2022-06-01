using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
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
    public partial class PasswordValidationForm : Form
    {
        public string psw = null;
        public bool result = false;
        public DialogResult dialogResult = DialogResult.Cancel;

        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public PasswordValidationForm()
        {
            InitializeComponent();

            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        }
        
        private void radButtonOK_Click(object sender, EventArgs e)
        {
            psw = radTextBoxPassword.Text;
            if (!string.IsNullOrEmpty(psw))
            {
                dialogResult = DialogResult.OK;

                List<users> listUsers = lineControlCuttingServicePlus.GetUsers();
                if (listUsers.Count > 0)
                {
                    users u = listUsers[0];
                    if (psw.Equals(u.password))
                    {
                        result = true;
                    }
                }
                this.Close();
            }
        }

        private void radButtonCancel_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            psw = null;
            this.Close();
        }

        private void PasswordValidationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
