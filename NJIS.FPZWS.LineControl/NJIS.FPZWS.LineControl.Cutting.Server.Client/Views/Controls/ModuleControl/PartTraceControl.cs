using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class PartTraceControl : UserControl,IView
    {

        private readonly PartTraceControlPresenter _presenter = new PartTraceControlPresenter();

        private readonly Dictionary<int,RadPanel> _panels = new Dictionary<int, RadPanel>();

        public const string ReceiveDatas = nameof(ReceiveDatas);

        public PartTraceControl()
        {
            InitializeComponent();
            InitGridView();
            Init();
            this.RegisterTipsMessage();
            Register();
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
            
        }

        private void Init()
        {
            _panels.Add(10,pnl10);
            _panels.Add(11,pnl11);
            _panels.Add(12,pnl12);
            _panels.Add(13,pnl13);
            _panels.Add(14,pnl14);
            _panels.Add(15,pnl15);
            _panels.Add(16,pnl16);
            _panels.Add(17,pnl17);
            _panels.Add(18,pnl18);
            _panels.Add(19,pnl19);
            _panels.Add(40,pnl1End);

            _panels.Add(20,pnl20);
            _panels.Add(21,pnl21);
            _panels.Add(22,pnl22);
            _panels.Add(23,pnl23);
            _panels.Add(24,pnl24);
            _panels.Add(25,pnl25);

            _panels.Add(30, pnl30);
            _panels.Add(31, pnl31);
            _panels.Add(32, pnl32);
            _panels.Add(33, pnl33);
            _panels.Add(34, pnl34);
            _panels.Add(35, pnl35);
            _panels.Add(36, pnl36);
            _panels.Add(37, pnl37);
            _panels.Add(38, pnl38);
            _panels.Add(39, pnl39);
            _panels.Add(41, pnl3End);

        }

        private void Register()
        {
            this.Register<List<PartInfoPositionArgs>>(ReceiveDatas, ExecuteBindingDatas);
        }

        private void ExecuteBindingDatas(List<PartInfoPositionArgs> datas)
        {
            gridViewBase1.DataSource = datas;
            foreach (var item in datas)
            {
                _panels[item.Position].Text = item.PartId;
            }
            
        }

        private void InitGridView()
        {
            this.gridViewBase1.ShowCheckBox = false;
            gridViewBase1.ReadOnly = true;
            gridViewBase1.ShowRowNumber = false;
            //gridViewBase1.AddColumns();
        }

        
    }
}
