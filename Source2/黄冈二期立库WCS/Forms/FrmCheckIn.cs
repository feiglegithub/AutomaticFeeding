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
    public partial class FrmCheckIn : Form
    {
        public string PassWord = "";

        public FrmCheckIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PassWord = this.textBox1.Text;
            this.Close();
        }
    }
}
