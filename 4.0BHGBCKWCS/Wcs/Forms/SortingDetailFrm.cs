using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS.Forms
{
    public partial class SortingDetailFrm : Form
    {
        public SortingDetailFrm()
        {
            InitializeComponent();
            this.Load += RGVTaskFrm_Load;
        }

        private void RGVTaskFrm_Load(object sender, EventArgs e)
        {
            this.dtpStart.Value = DateTime.Now;
            this.dtpEnd.Value = this.dtpStart.Value;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
        }
       
    }
}
