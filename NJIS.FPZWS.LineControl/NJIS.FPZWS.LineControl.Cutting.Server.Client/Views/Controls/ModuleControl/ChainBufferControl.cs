using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class ChainBufferControl : UserControl,IView
    {
        public const string BindingDatas = nameof(BindingDatas);
        private readonly ChainBufferControlPresenter _presenter = new ChainBufferControlPresenter();
        public ChainBufferControl()
        {
            InitializeComponent();
            Register();
            this.BindingPresenter(_presenter);
        }

        private void Register()
        {
            this.Register<ChainBufferArgs>(BindingDatas, ExecuteBindingDatas);
        }

        private void ExecuteBindingDatas(ChainBufferArgs datas)
        {
            if (datas.CuttingChainBuffer.Code == "CuttingChainBuffer-01")
            {
                this.chainBufferSubControl1.BindingDatas(datas);
            }
            if (datas.CuttingChainBuffer.Code == "CuttingChainBuffer-02")
            {
                this.chainBufferSubControl2.BindingDatas(datas);
            }
            if (datas.CuttingChainBuffer.Code == "CuttingChainBuffer-03")
            {
                this.chainBufferSubControl3.BindingDatas(datas);
            }
            if (datas.CuttingChainBuffer.Code == "CuttingChainBuffer-04")
            {
                this.chainBufferSubControl4.BindingDatas(datas);
            }

        }
    }
}
