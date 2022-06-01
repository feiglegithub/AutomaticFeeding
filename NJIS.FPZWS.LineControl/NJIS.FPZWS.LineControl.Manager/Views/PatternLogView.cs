using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;


namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class PatternLogView : UserControl, UI.Common.Message.Extensions.Interfaces.IView
    {
        private PatternLogPresenter _presenter = null;
        private PatternLogPresenter Presenter => _presenter ?? (_presenter = new PatternLogPresenter());
        public PatternLogView()
        {
            InitializeComponent();
            View();
            dtpPlanDate.Value = DateTime.Today;
            this.RegisterTipsMessage();
            this.Register<List<PatternLog>>(SearchPresenterBase.BindingData, patternLogs =>
            {
                gvbPattern.DataSource = null;
                gvbPattern.DataSource = patternLogs;
                gvbPattern.EndWait();
                btnSearch.Enabled = true;
            });
            this.BindingPresenter(Presenter);
            HandleCreated += PatternLogView_HandleCreated;
            Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void PatternLogView_HandleCreated(object sender, EventArgs e)
        {
            
        }

        private void View()
        {
            PatternLog patternLog;
            gvbPattern.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            gvbPattern.AddColumns(new List<ColumnInfo>()
            {

                new ColumnInfo(){ HeaderText="计划日期", FieldName=nameof(patternLog.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="批次", FieldName=nameof(patternLog.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="锯切图号", FieldName=nameof(patternLog.PatternId), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(patternLog.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(patternLog.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(){ HeaderText="耗时", FieldName=nameof(patternLog.TotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="已分配设备", FieldName=nameof(patternLog.DeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="计划设备", FieldName=nameof(patternLog.PlanDeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="实际设备", FieldName=nameof(patternLog.ActualDeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="计划垛号", FieldName=nameof(patternLog.PlanStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+20){ HeaderText="实际垛号", FieldName=nameof(patternLog.ActuallyStackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(patternLog.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="生效状态", FieldName=nameof(patternLog.IsEnable), DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{true,"生效"},{false,"失效"}},DisplayMember = "Value",ValueMember = "Key"},
                new ColumnInfo(-20){ HeaderText="工件数", FieldName=nameof(patternLog.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="余料数", FieldName=nameof(patternLog.OffPartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="操作", FieldName=nameof(patternLog.OperationName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="日志时间", FieldName=nameof(patternLog.LogTime), DataType=typeof(DateTime), ReadOnly=true },

            });
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = dtpPlanDate.Value.Date;

            this.Send(SearchPresenterBase.GetData, planDate);
            gvbPattern.BeginWait();
            btnSearch.Enabled = false;
        }
    }
}
