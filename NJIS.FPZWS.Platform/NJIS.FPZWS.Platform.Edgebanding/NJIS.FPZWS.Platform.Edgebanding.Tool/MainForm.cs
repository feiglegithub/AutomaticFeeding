using NJIS.FPZWS.LineControl.DeliverData.Tool.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace NJIS.FPZWS.Platform.Edgebanding.Tool
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public void AddChild(RadForm from)
        {
            var window = new DocumentWindow { Text = from.Text };

            from.TopLevel = false;
            from.Dock = DockStyle.Fill;
            from.FormBorderStyle = FormBorderStyle.None;
            from.Show();
            window.Controls.Add(from);
            radDock1.AddDocument(window);
            window.Select();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            var infoform = new DeliverdataForm();
            AddChild(infoform);
        }
    }
}
