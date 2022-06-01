using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDControl
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dic;
        public Form1()
        {
            InitializeComponent();
            this.btnAction2001.BackColor = Color.Red;
            this.btnAction20012.BackColor = Color.Red;
            this.btnAction2002.BackColor = Color.Red;
            this.btnAction20022.BackColor = Color.Red;
            this.btnAction2007.BackColor = Color.Red;
            this.btnAction2019.BackColor = Color.Red;
            this.btnAction2015.BackColor = Color.Red;
            this.btnAction2017.BackColor = Color.Red;
            dic = new Dictionary<string, string>();
            dic["2001"] = Properties.Settings.Default.ExecPath2001;
            dic["20012"] = Properties.Settings.Default.ExecPath20012;
            dic["2002"] = Properties.Settings.Default.ExecPath2002;
            dic["20022"] = Properties.Settings.Default.ExecPath20022;
            dic["2007"] = Properties.Settings.Default.ExecPath2007;
            dic["2015"] = Properties.Settings.Default.ExecPath2015;
            dic["2017"] = Properties.Settings.Default.ExecPath2017;
            dic["2019"] = Properties.Settings.Default.ExecPath2019;
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Process exep = new Process();
            exep.StartInfo.FileName = dic[btn.Tag.ToString()];
            exep.EnableRaisingEvents = true;
            exep.Exited += new EventHandler(exep_Exited);
            var rlt = exep.Start();
            //MessageBox.Show(rlt.ToString());

            btn.BackColor = Color.Lime;
            btn.Enabled = false;
        }

        void exep_Exited(object sender, EventArgs e)
        {
            //var btn = (Button)sender;
            //btn.BackColor = Color.Red;
            //btn.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] names = { "RFIDApp2001", "RFIDApp20012", "RFIDApp2002", "RFIDApp20022", "RFIDApp2007", "RFIDApp2019", "RFIDApp2015", "RFIDApp2017" };
            foreach (string exeName in names)
            {
                Process[] myprocess = Process.GetProcessesByName(exeName);
                if (myprocess.Length > 0)
                {
                    myprocess[0].Kill();
                }
            }
        }

    }
}
