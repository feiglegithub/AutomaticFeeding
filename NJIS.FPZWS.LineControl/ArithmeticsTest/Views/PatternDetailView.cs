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
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ArithmeticsTest.Views
{
    public partial class PatternDetailView : UserControl,IView
    {
        private PatternDetailPresenter presenter = new PatternDetailPresenter();
        public PatternDetailView()
        {
            InitializeComponent();
            View();
            this.RegisterTipsMessage();
            this.Register<List<PatternDetail>>(SearchPresenterBase.BindingData, data =>
                {
                    gvbPatternDetail.DataSource = null;
                    gvbPatternDetail.DataSource = data;
                });
            this.BindingPresenter(presenter);
        }

        private void View()
        {
            PatternDetail patternDetail;
            gvbPatternDetail.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            gvbPatternDetail.AddColumns(new List<ColumnInfo>()
            {

                new ColumnInfo(){ HeaderText="批次", FieldName=nameof(patternDetail.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="锯切图号", FieldName=nameof(patternDetail.PatternId), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件号", FieldName=nameof(patternDetail.PartId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(patternDetail.Color), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(EColumnType.ComboBox)
                //{ HeaderText="状态", FieldName=nameof(pattern.Status), DataType=typeof(int), ReadOnly=true
                //    ,DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription()
                //    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(){ HeaderText="长", FieldName=nameof(patternDetail.Length), DataType=typeof(float), ReadOnly=true },
                new ColumnInfo(){ HeaderText="宽", FieldName=nameof(patternDetail.Width), DataType=typeof(float), ReadOnly=true },
                new ColumnInfo(){ HeaderText="厚", FieldName=nameof(patternDetail.Thickness), DataType=typeof(float), ReadOnly=true },
                //new ColumnInfo(){ HeaderText="实际设备", FieldName=nameof(pattern.ActualDeviceName), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(){ HeaderText="计划垛号", FieldName=nameof(pattern.PlanStackName), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(){ HeaderText="实际垛号", FieldName=nameof(pattern.ActuallyStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(patternDetail.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="完成状态", FieldName=nameof(patternDetail.IsFinished), DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{true,"完成"},{false,"未完成"}},DisplayMember = "Value",ValueMember = "Key"},
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="类型", FieldName=nameof(patternDetail.IsOffPart), DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{true,"余板"},{false,"正常"}},DisplayMember = "Value",ValueMember = "Key"},
                new ColumnInfo(){ HeaderText="工件数", FieldName=nameof(patternDetail.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="完成工件数", FieldName=nameof(patternDetail.FinishedPartCount), DataType=typeof(int), ReadOnly=true },

            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var batchName = txtBatchName.Text.Trim();
            if (string.IsNullOrWhiteSpace(batchName))
            {
                this.BeginInvoke((Action) (() => RadMessageBox.Show(this, "批次号不可为空")));
                return;
            }
            this.Send(SearchPresenterBase.GetData,batchName);
            gvbPatternDetail.BeginWait();
        }
    }
}
