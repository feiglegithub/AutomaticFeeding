using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl
{
    public partial class CurTaskDetailControl : UserControl, IView
    {
        /// <summary>
        /// 绑定当前任务 接收数据类型
        /// </summary>
        public const string BindingCurTask = nameof(BindingCurTask);

        public const string BindingDatas = nameof(BindingDatas);

        public const string BindingDeviceInfos = nameof(BindingDeviceInfos);

        private CurTaskDetailPresenter presenter = null;

        private int VerticalScrollIndex = 0;
        private int HorizontalOffset = 0;

        public bool IsAutoRefresh { get; set; } = true;
            

        public CurTaskDetailControl()
        {
            InitializeComponent();
            Register();
            ViewInit();
            cmbCurStatus.DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription();
            cmbNextStatus.DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription();
            cmbCurStatus.DisplayMember = "Item2";
            cmbNextStatus.DisplayMember = "Item2";
            cmbCurStatus.ValueMember = "Item1";
            cmbNextStatus.ValueMember = "Item1";
            this.RegisterTipsMessage();
            presenter = CurTaskDetailPresenter.GetInstance();
            this.BindingPresenter(presenter);
            //gvwPatterns.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            this.Disposed += (sender, args) => this.UnBindingPresenter();
            gvbStackDetail.Scroll += GvbStackDetail_Scroll;
        }

        private void GvbStackDetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                VerticalScrollIndex = e.NewValue;
            }
            else if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                HorizontalOffset = e.NewValue;
            }
        }

        public void BeginListen()
        {
            this.Send(CurTaskDetailPresenter.Begin, "");
        }

        private void RadGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //if (cuttingTaskDetails == null || ofCuttingTaskDetails == null) return;
            //if (e.Row.DataBoundItem is CuttingPattern cuttingPattern)
            //{
            //    int PTN_INDEX = Convert.ToInt32(cuttingPattern.NewPatternName);
            //    gvwParts.DataSource =
            //        cuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
            //    gvwOffParts.DataSource =
            //        ofCuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
            //    PartFinishedStatusView();
            //}
        }

        private void ViewClear()
        {
            //this.gvwParts.DataSource = null;
            //this.gvwPatterns.DataSource = null;
            //this.gvwOffParts.DataSource = null;
        }



        private void ViewInit()
        {
            StackDetail stackDetail;
            gvbStackDetail.ReadOnly = true;
            gvbStackDetail.AutoGenerateColumns = false;
            //gvbStackDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gvbStackDetail.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn(){ HeaderText="计划日期",DataPropertyName= nameof(stackDetail.PlanDate), ValueType= typeof(DateTime), ReadOnly=true,Name = nameof(stackDetail.PlanDate)},
                new DataGridViewTextBoxColumn(){HeaderText="垛号", DataPropertyName=nameof(stackDetail.StackName), ValueType=typeof(string), ReadOnly=true,Name = nameof(stackDetail.StackName),DefaultCellStyle = new DataGridViewCellStyle(){BackColor = Color.White} },
                new DataGridViewTextBoxColumn(){HeaderText="花色", DataPropertyName=nameof(stackDetail.Color), ValueType=typeof(string), ReadOnly=true ,Name = nameof(stackDetail.Color)},
                new DataGridViewComboBoxColumn(){HeaderText="状态", DataPropertyName=nameof(stackDetail.Status), ValueType=typeof(int), ReadOnly=true
                    ,DataSource = BookStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1",Name = nameof(stackDetail.Status)}, 
                new DataGridViewTextBoxColumn(){HeaderText="锯切图", DataPropertyName=nameof(stackDetail.PatternId), ValueType=typeof(int), ReadOnly=true ,Name = nameof(stackDetail.PatternId)},
                new DataGridViewTextBoxColumn(){ HeaderText="计划批次", DataPropertyName=nameof(stackDetail.PlanBatchName), ValueType=typeof(string), ReadOnly=true ,Name = nameof(stackDetail.PlanBatchName)},
                new DataGridViewTextBoxColumn(){HeaderText="实际批次", DataPropertyName=nameof(stackDetail.ActualBatchName), ValueType=typeof(string), ReadOnly=true,Name = nameof(stackDetail.ActualBatchName)},
                new DataGridViewTextBoxColumn(){ HeaderText="顺序", DataPropertyName=nameof(stackDetail.StackIndex), ValueType=typeof(int), ReadOnly=true ,Name = nameof(stackDetail.StackIndex)},
                new DataGridViewTextBoxColumn(){ HeaderText="更新时间", DataPropertyName=nameof(stackDetail.UpdatedTime), ValueType=typeof(DateTime), ReadOnly=true,Name = nameof(stackDetail.UpdatedTime)},

            });
            //gvbStackDetail.radGridView.SelectionMode = GridViewSelectionMode.CellSelect;
            //gvbStackDetail.AddColumns(new List<ColumnInfo>()
            //{
            //    new ColumnInfo(){ HeaderText="计划日期", FieldName=nameof(stackDetail.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="垛号", FieldName=nameof(stackDetail.StackName), DataType=typeof(string), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="花色", FieldName=nameof(stackDetail.Color), DataType=typeof(string), ReadOnly=true },
            //    new ColumnInfo(EColumnType.ComboBox)
            //    { HeaderText="状态", FieldName=nameof(stackDetail.Status), DataType=typeof(int), ReadOnly=true
            //        ,DataSource = BookStatus.Cutting.GetAllFinishStatusDescription()
            //        ,DisplayMember = "Item2",ValueMember = "Item1"},
            //    new ColumnInfo(){ HeaderText="锯切图", FieldName=nameof(stackDetail.PatternId), DataType=typeof(int), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="计划批次", FieldName=nameof(stackDetail.PlanBatchName), DataType=typeof(string), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="实际批次", FieldName=nameof(stackDetail.ActualBatchName), DataType=typeof(string), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="顺序", FieldName=nameof(stackDetail.StackIndex), DataType=typeof(int), ReadOnly=true },
            //    new ColumnInfo(){ HeaderText="更新时间", FieldName=nameof(stackDetail.UpdatedTime), DataType=typeof(DateTime), ReadOnly=true },

            //});
            //gvwPatterns.ReadOnly = true;
            //gvwOffParts.ReadOnly = true;
            //gvwParts.ReadOnly = true;
        }

        private void Register()
        {
            this.Register<DeviceInfos>(BindingDeviceInfos, ExecuteBindingDeviceInfo);
            this.Register<Tuple<Pattern, List<PatternDetail>, Pattern, List<StackDetail>>>(BindingDatas, ExecuteBindingDatas);
        }

        private void ExecuteBindingDeviceInfo(DeviceInfos deviceInfos)
        {
            if (deviceInfos == null)
            {
                btnDeviceError.Enabled = false;
                return;
            }
            btnDeviceError.ForeColor = deviceInfos.State.Value == 0 ? Color.Red : Color.Black;
            btnDeviceError.Text = deviceInfos.State.Value == 0 ? "设备修复" : "设备异常";
            btnDeviceError.Enabled = true;


        }

        private void ExecuteBindingDatas(Tuple<Pattern, List<PatternDetail>, Pattern, List<StackDetail>> tuple)
        {
            if (!IsAutoRefresh) return;
            var cuttingTask = tuple.Item1;
            var patternDetails = tuple.Item2;
            var nextTask = tuple.Item3;
            var stackDetails = tuple.Item4;
            txtCurTask.Text = cuttingTask==null?"":cuttingTask.MdbName;
            txtNextTask.Text = nextTask == null ? "" : nextTask.MdbName;

            cmbCurStatus.SelectedValue =
                cuttingTask == null ? PatternStatus.Undistributed.GetHashCode() : cuttingTask.Status;
            cmbNextStatus.SelectedValue =
                nextTask == null ? PatternStatus.Undistributed.GetHashCode() : nextTask.Status;

            gvbStackDetail.DataSource = null;
            gvbStackDetail.DataSource = stackDetails;

            if (stackDetails != null && stackDetails.Count > VerticalScrollIndex)
            {
                gvbStackDetail.FirstDisplayedScrollingRowIndex = VerticalScrollIndex;
            }
            
            //gvbStackDetail.HorizontalScrollingOffset = HorizontalOffset;
            
            //List<Pattern> cuttingPatterns = new List<Pattern>();
            //if (cuttingTask != null){

            //    cuttingPatterns.Add(cuttingTask);
            //}
            //this.gvwPatterns.DataSource = null;
            //this.gvwPatterns.DataSource = cuttingPatterns;

            //this.gvwParts.DataSource = null;
            //this.gvwParts.DataSource = patternDetails.FindAll(item=>!item.IsOffPart);

            //this.gvwOffParts.DataSource = null;
            //this.gvwOffParts.DataSource = patternDetails.FindAll(item => item.IsOffPart);

            PartFinishedStatusView();
        }

        private void PartFinishedStatusView()
        {
            //bool isScroll = false;
            foreach (DataGridViewRow rowInfo in gvbStackDetail.Rows)
            {
                if (rowInfo.DataBoundItem is StackDetail stackDetail)
                {
                    if (stackDetail.Status == BookStatus.Cut.GetHashCode())
                    {
                        
                        foreach (DataGridViewCell rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Green;
                            //rowInfoCell.Style.BackColor = Color.Green;
                        }
                    }

                    
                    if (stackDetail.Status == BookStatus.Cutting.GetHashCode())
                    {
                        //gvbStackDetail.FirstDisplayedScrollingRowIndex = rowInfo.Index;
                        //isScroll = true;
                        foreach (DataGridViewCell rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Orange;
                            //rowInfoCell.Style.BackColor = Color.Orange;
                        }
                    }

                    if (stackDetail.Status == BookStatus.DistributedPattern.GetHashCode())
                    {
                        //if (!isScroll)
                        //{
                        //    gvbStackDetail.FirstDisplayedScrollingRowIndex = rowInfo.Index;
                        //    isScroll = true;
                        //}
                        foreach (DataGridViewCell rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.DeepPink;
                            //rowInfoCell.Style.BackColor = Color.DeepPink;
                        }
                    }
                }
            }

            //foreach (var rowInfo in gvwParts.Rows)
            //{
            //    if (rowInfo.DataBoundItem is PatternDetail patternDetail)
            //    {
            //        if (patternDetail.IsFinished)
            //        {
            //            foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
            //            {
            //                rowInfoCell.Style.ForeColor = Color.Green;
            //            }
            //        }
            //    }
            //}

            //foreach (var rowInfo in gvwOffParts.Rows)
            //{
            //    if (rowInfo.DataBoundItem is PatternDetail patternDetail)
            //    {
            //        if (patternDetail.IsFinished)
            //        {
            //            foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
            //            {
            //                rowInfoCell.Style.ForeColor = Color.Green;
            //            }
            //        }
            //    }
            //}

        }

        private void btnPatternError_Click(object sender, EventArgs e)
        {
            DialogResult ret = RadMessageBox.Show(this, "提交后，本任务将会终止，本任务没有加工完成的工件将会被设置为NG工件，请确认是否提交？", "提示", MessageBoxButtons.YesNo);
            if (ret == DialogResult.No) return;
            string taskName = txtCurTask.Text.Trim();
            this.Send(CurTaskDetailPresenter.SetTaskError, taskName);
        }

        private void btnDeviceError_Click(object sender, EventArgs e)
        {
            bool deviceStatus = false;
            if (btnDeviceError.Text == "设备异常")
            {
                deviceStatus = false;
                DialogResult ret = RadMessageBox.Show(this, "提交后，系统将不再为本设备推送任务！请确认设备是否异常？", "提示", MessageBoxButtons.YesNo);
                if (ret == DialogResult.No) return;
            }
            else
            {
                deviceStatus = true;
                DialogResult ret = RadMessageBox.Show(this, "提交后，系统将自动为本设备推送任务！请确认设备是否已经恢复正常？", "提示", MessageBoxButtons.YesNo);
                if (ret == DialogResult.No) return;
            }
            this.Send(CurTaskDetailPresenter.SetDeviceStatus, deviceStatus);
            btnDeviceError.Enabled = false;
        }
    }
}
