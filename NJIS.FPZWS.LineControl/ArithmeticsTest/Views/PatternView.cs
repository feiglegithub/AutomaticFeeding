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
    public partial class PatternView : UserControl,IView
    {
        private PatternPresenter presenter = new PatternPresenter();
        public PatternView()
        {
            InitializeComponent();
            this.RegisterTipsMessage();
            View();
            dtpPlanDate.Value = DateTime.Today;
            this.Register<List<Pattern>>(SearchPresenterBase.BindingData, data =>
            {
                gvbPattern.DataSource = null;
                gvbPattern.DataSource = data;
            });
            this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void View()
        {
            Pattern pattern;
            gvbPattern.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            gvbPattern.AddColumns(new List<ColumnInfo>()
            {

                new ColumnInfo(){ HeaderText="计划日期", FieldName=nameof(pattern.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="批次", FieldName=nameof(pattern.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="锯切图号", FieldName=nameof(pattern.PatternId), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(pattern.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(pattern.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(){ HeaderText="耗时", FieldName=nameof(pattern.TotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="已分配设备", FieldName=nameof(pattern.DeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="计划设备", FieldName=nameof(pattern.PlanDeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="实际设备", FieldName=nameof(pattern.ActualDeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="计划垛号", FieldName=nameof(pattern.PlanStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="实际垛号", FieldName=nameof(pattern.ActuallyStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(pattern.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="生效状态", FieldName=nameof(pattern.IsEnable), DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{true,"生效"},{false,"失效"}},DisplayMember = "Value",ValueMember = "Key"},
                new ColumnInfo(-20){ HeaderText="工件数", FieldName=nameof(pattern.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="余料数", FieldName=nameof(pattern.OffPartCount), DataType=typeof(int), ReadOnly=true },

            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = this.dtpPlanDate.Value.Date;
            var isCheck = isDataBase.Checked;
            this.Send(SearchPresenterBase.GetData, new Tuple<DateTime,bool>(planDate, isCheck));
            gvbPattern.BeginWait();
        }
    }
}
