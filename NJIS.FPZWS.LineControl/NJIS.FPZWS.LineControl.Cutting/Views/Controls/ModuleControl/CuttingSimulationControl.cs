using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    public partial class CuttingSimulationControl : UserControl,IView
    {
        private CuttingSimulationControlPresenter _presenter = new CuttingSimulationControlPresenter();
        public const string BindingCurrTasks = nameof(BindingCurrTasks);
        public const string BindingTaskParts = nameof(BindingTaskParts);
        public CuttingSimulationControl()
        {
            InitializeComponent();
            dtpPlanDate.ValueChanged += DtpPlanDate_ValueChanged;
            ViewInit();
            Register();
            cmbDevice.SelectedIndexChanged += CmbDevice_SelectedIndexChanged;
            dtpPlanDate.ValueChanged += (sender, args) => cmbDevice.DataSource = null;
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void DtpPlanDate_ValueChanged(object sender, EventArgs e)
        {
            cmbDevice.DataSource = null;
            gridViewBase1.DataSource = null;
            cmbItemName.DataSource = null;
        }

        private void ViewInit()
        {
            gridViewBase1.AddColumns(new List<FPZWS.UI.Common.Controls.ColumnInfo>()
            {
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="小任务", FieldName="TaskDistributeId", DataType=typeof(Guid), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="JOB_INDEX", FieldName="JOB_INDEX", DataType=typeof(int), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="PART_INDEX", FieldName="PART_INDEX", DataType=typeof(int), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="批次", FieldName="BatchName", DataType=typeof(string), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="板件号", FieldName="PartId", DataType=typeof(string), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="设备号", FieldName="DeviceName", DataType=typeof(string), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="堆垛号", FieldName="ItemName", DataType=typeof(string), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="生产时间", FieldName="PlanDate", DataType=typeof(DateTime), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="板件状态", FieldName="PartFinishedStatus", DataType=typeof(string), ReadOnly=true },
                new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="任务状态", FieldName="TaskEnable", DataType=typeof(string), ReadOnly=true },
                //new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="INFO35", FieldName="INFO35", DataType=typeof(string), ReadOnly=true },
                //new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="INFO36", FieldName="INFO36", DataType=typeof(string), ReadOnly=true },
                //new FPZWS.UI.Common.Controls.ColumnInfo(){ HeaderText="INFO37", FieldName="INFO37", DataType=typeof(string), ReadOnly=true },

            });
        }


        private void Register()
        {
            this.Register< List<Tuple<string, List<SpiltMDBResult>>>>(BindingCurrTasks, ExecuteBindingCurrTasks);
            this.Register<List<CuttingTaskDetail>>(BindingTaskParts, ExecuteBindingTaskParts);
        }

        private void CmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDevice.SelectedIndex != -1)
            {
                cmbItemName.DisplayMember = "ItemName";
                cmbItemName.DataSource = cmbDevice.SelectedValue;
                
            }
            
        }

        private void ExecuteBindingTaskParts(List<CuttingTaskDetail> datas)
        {
            gridViewBase1.DataSource = datas;
            foreach (var rowInfo in gridViewBase1.Rows)
            {
                if (rowInfo.DataBoundItem is CuttingTaskDetail ctd)
                {
                    if (ctd.PartFinishedStatus)
                    {
                        foreach (GridViewCellInfo cell in rowInfo.Cells)
                        {
                            cell.Style.ForeColor = Color.Green;
                        }
                    }
                }
            }
        }

        private void ExecuteBindingCurrTasks(List<Tuple<string,List<SpiltMDBResult>>> currTasks)
        {
            cmbDevice.DisplayMember = "Item1";
            cmbDevice.ValueMember = "Item2";
            cmbDevice.DataSource = currTasks;
            this.flowLayoutPanel1.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbDevice.DataSource == null)
            {
                this.Send(CuttingSimulationControlPresenter.GetCurrTasks, dtpPlanDate.Value.Date);
            }
            else
            {
                SpiltMDBResult spiltMdbResult = cmbItemName.SelectedItem as SpiltMDBResult;
                if (spiltMdbResult == null)
                {
                    Tips("暂时无当前任务");
                    cmbDevice.DataSource = null;
                }
                else
                {
                    this.Send(CuttingSimulationControlPresenter.GetTaskDetail,spiltMdbResult);
                    
                }
            }
        }

        private void btnStartCutting_Click(object sender, EventArgs e)
        {
            if(gridViewBase1.DataSource ==null)
                return;
            
            this.flowLayoutPanel1.Enabled = false;
            this.Send(CuttingSimulationControlPresenter.BeginCutting, gridViewBase1.DataSource as List<CuttingTaskDetail>);
        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(this, msg)));
        }
    }
}
