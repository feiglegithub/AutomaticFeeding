using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class MessageControl : UserControl,IView
    {
        private MessageControlPresenter presenter = new MessageControlPresenter();
        public MessageControl()
        {
            InitializeComponent();

            this.RegisterTipsMessage();
            ViewInit();
            this.BindingPresenter(presenter);
            this.Register<PcsLogicAlarmArgs>(MessageControlPresenter.BindingDatas,
                data=>
                {
                    PcsLogicAlarmArgses.Insert(0,data);
                    gridViewBase1.DataSource = null;
                    gridViewBase1.DataSource = PcsLogicAlarmArgses;
                });
            this.Register<PartInfoQueueArgs>(MessageControlPresenter.BindingDatas, data =>
            {
                PartInfoQueueArgses.Insert(0, data);
                //var newRow = gridViewBase2.radGridView.Rows.AddNew();
                //newRow.ViewInfo.ViewTemplate.DataSource = data;
                //gridViewBase2.radGridView.Rows.Insert(0, newRow);
                gridViewBase2.DataSource = null;
                gridViewBase2.DataSource = PartInfoQueueArgses;
            });
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private List<PcsLogicAlarmArgs> PcsLogicAlarmArgses { get; } = new List<PcsLogicAlarmArgs>();
        private List<PartInfoQueueArgs> PartInfoQueueArgses { get; }= new List<PartInfoQueueArgs>();

        private void ViewInit()
        {
            PcsLogicAlarmArgs pcs;
            gridViewBase1.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){FieldName = nameof(pcs.Value),HeaderText = "报警值",DataType = typeof(string)},
                new ColumnInfo(){FieldName = nameof(pcs.Category),HeaderText = "报警类型",DataType = typeof(string)},
                new ColumnInfo(){FieldName = nameof(pcs.ParamName),HeaderText = "报警参数",DataType = typeof(string)}
            });
        }
    }
}
