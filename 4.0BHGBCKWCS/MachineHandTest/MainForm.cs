using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WCS.OPC;

namespace MachineHandTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cmbStart.DataSource = tuples;
            cmbStart.DisplayMember = "Name";
            cmbStart.ValueMember = "Value";
            cmbTarget.DataSource = tuples1;
            cmbTarget.DisplayMember = "Name";
            cmbTarget.ValueMember = "Value";
        }

        List<Station> tuples = new List<Station>
        {
            new Station{ Name=2001,Value=2001},
            new Station{ Name=2002,Value=2002},
            new Station{ Name=2003,Value=2003},
            new Station{ Name=2004,Value=2004},
            new Station{ Name=2005,Value=2005},
        };
        List<Station> tuples1 = new List<Station>
        {
            new Station{ Name=2001,Value=2001},
            new Station{ Name=2002,Value=2002},
            new Station{ Name=2003,Value=2003},
            new Station{ Name=2004,Value=2004},
            new Station{ Name=2005,Value=2005},
        };
        private void btn_Click(object sender, EventArgs e)
        {
            int fromStation = Convert.ToInt32(cmbStart.SelectedValue);
            int toStation = Convert.ToInt32(cmbTarget.SelectedValue);
            if (fromStation == toStation)
            {
                MessageBox.Show(this, "起始位置与目标位置不能一样");
                return;
            }

            if (!OpcHsc.RMCanDo(1))
            {
                MessageBox.Show(this, "机械手处于繁忙");
                return;
            }
            OpcHsc.WriteToMainpulator(fromStation, toStation);
        }

        internal class Station
        {
            public int Name { get; set; }

            public int Value { get; set; }
        }
        
    }
}
