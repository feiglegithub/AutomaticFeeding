using ArithmeticsTest.Services;
using System;
using Telerik.WinControls.UI;

namespace ArithmeticsTest
{
    public partial class MainForm : RadForm
    {
        private ILocalService _localService = null;
        private ILocalService Service => _localService ?? (_localService = LocalService.GetInstance());
        public MainForm()
        {
            InitializeComponent();
            this.Shown += MainForm_Shown;
            this.Disposed += MainForm_Disposed;
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            Service.Stop();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Service.Start();
        }
    }
}
