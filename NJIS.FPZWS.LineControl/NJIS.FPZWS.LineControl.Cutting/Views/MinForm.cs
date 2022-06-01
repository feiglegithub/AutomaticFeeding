using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views
{
    public partial class MinForm : Telerik.WinControls.UI.RadForm
    {
        public MinForm()
        {
            InitializeComponent();
            this.Closing += MinForm_Closing;
        }

        private void MinForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
