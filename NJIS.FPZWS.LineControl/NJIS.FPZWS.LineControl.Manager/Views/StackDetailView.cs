using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class StackDetailView : UserControl, UI.Common.Message.Extensions.Interfaces.IView
    {
        private StackDetailPresenter presenter = new StackDetailPresenter();
        public StackDetailView()
        {
            InitializeComponent();
            View();
            dtpPlanDate.Value = DateTime.Today;
            this.RegisterTipsMessage();
            this.Register<List<StackDetail>>(SearchPresenterBase.BindingData, data =>
            {
                gvbStackDetail.DataSource = null;
                gvbStackDetail.DataSource = data;
            });
            this.BindingPresenter(presenter);
        }

        private void View()
        {
            StackDetail stackDetail;
            gvbStackDetail.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            gvbStackDetail.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="计划日期", FieldName=nameof(stackDetail.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(){ HeaderText="垛号", FieldName=nameof(stackDetail.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(stackDetail.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(stackDetail.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = BookStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(){ HeaderText="锯切图", FieldName=nameof(stackDetail.PatternId), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="计划批次", FieldName=nameof(stackDetail.PlanBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="实际批次", FieldName=nameof(stackDetail.ActualBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="顺序", FieldName=nameof(stackDetail.StackIndex), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(stackDetail.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },

            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = this.dtpPlanDate.Value.Date;
            this.Send(SearchPresenterBase.GetData, planDate);
            gvbStackDetail.BeginWait();
        }
    }
}
