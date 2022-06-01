using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArithmeticsTest.Presenters;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls.UI;

namespace ArithmeticsTest.Views
{
    public partial class StackView : UserControl,IView
    {
        private StackPresenter presenter = new StackPresenter();
        public StackView()
        {
            InitializeComponent();
            View();
            dtpPlanDate.Value = DateTime.Today;
            this.RegisterTipsMessage();
            this.Register<List<Stack>>(SearchPresenterBase.BindingData,data=>
            {
                gvbStack.DataSource = null;
                gvbStack.DataSource = data;
            });
            this.BindingPresenter(presenter);
        }

        private void View()
        {
            Stack stack;
            gvbStack.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            gvbStack.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText="计划日期", FieldName=nameof(stack.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="垛号", FieldName=nameof(stack.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="主花色", FieldName=nameof(stack.MainColor), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(stack.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = StackStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},

                new ColumnInfo(-20){ HeaderText="计划设备", FieldName=nameof(stack.PlanDeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="实际设备", FieldName=nameof(stack.ActualDeviceName), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(){ HeaderText="开始时间", FieldName=nameof(stack.StartTime), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(){ HeaderText="完成时间", FieldName=nameof(stack.FinishedTime), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(stack.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },

                new ColumnInfo(+20){ HeaderText="上一垛", FieldName=nameof(stack.LastStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="下一垛", FieldName=nameof(stack.NextStackName), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(-20){ HeaderText="板材数", FieldName=nameof(stack.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="第一批次", FieldName=nameof(stack.FirstBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="第一批次数", FieldName=nameof(stack.FirstBatchBookCount), DataType=typeof(int), ReadOnly=true },

                new ColumnInfo(+20){ HeaderText="第二批次", FieldName=nameof(stack.SecondBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="第二批次数", FieldName=nameof(stack.SecondBatchBookCount), DataType=typeof(int), ReadOnly=true },
            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = this.dtpPlanDate.Value.Date;
            var isCheck = isDataBase.Checked;

            this.Send(SearchPresenterBase.GetData, new Tuple<DateTime, bool>(planDate, isCheck));
            gvbStack.BeginWait();
            //this.Send(SearchPresenterBase.GetData,planDate);
        }
    }
}
