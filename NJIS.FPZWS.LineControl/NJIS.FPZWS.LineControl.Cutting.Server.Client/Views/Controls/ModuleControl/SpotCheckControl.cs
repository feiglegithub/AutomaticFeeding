using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class SpotCheckControl : UserControl,IView
    {

        private readonly SpotCheckControlPresenter _presenter = new SpotCheckControlPresenter();
        public const string ReciveCanCheckPartInfos = nameof(ReciveCanCheckPartInfos);
        public SpotCheckControl()
        {
            InitializeComponent();
            //this.gridViewBase1.ShowCheckBox = true;
            this.RegisterTipsMessage();
            Register();
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void Register()
        {
            this.Register<List<CutPartInfoCollector>>(ReciveCanCheckPartInfos, ExecuteBindingDatas);
        }

        private void ExecuteBindingDatas(List<CutPartInfoCollector> cutPartInfoCollectors)
        {
            //this.gridViewBase1.DataSource = cutPartInfoCollectors;
        }


    }
}
