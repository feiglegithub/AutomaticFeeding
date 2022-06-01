using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views
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
