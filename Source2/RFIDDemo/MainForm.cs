using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowChildForm(Form f, string formName)
        {

            System.Windows.Forms.Form[] mdiform = this.MdiChildren;
            bool openFlag = false;  //窗体的打开标志
            foreach (Form fr in mdiform)
            {
                if (fr.Name == formName)
                {
                    fr.Activate();
                    openFlag = true;
                    break;
                }
            }
            if (!openFlag)
            {
                f.MdiParent = this;
                //f.StartPosition = FormStartPosition.CenterParent;
                f.Show();
                f.Dock = DockStyle.Fill;
            }
        }

        private void rFID2001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form1();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form1");
        }

        private void rFID2002ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form2();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form2");
        }

        private void rFID2007ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form5();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form5");
        }

        private void rFID2008ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form3();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form3");
        }

        private void rFID2010ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new Form4();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form4");
        }

        private void rFID2012ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form6();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form6");
        }

        private void rFID2013ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form7();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form7");
        }

        private void rFID2015ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form8();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form8");
        }

        private void rFID2017ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form9();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form9");
        }

        private void rFID2019ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form10();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem it in this.menuStrip1.Items)
            {
                if (item.Name == it.Name)
                {
                    it.BackColor = Color.FromArgb(135, 206, 250);
                    //it.ForeColor = Color.White;
                }
                else
                {
                    it.BackColor = Color.Transparent;
                    //it.ForeColor = Color.Silver;
                }
            }

            ShowChildForm(frm, "Form10");
        }
    }
}
