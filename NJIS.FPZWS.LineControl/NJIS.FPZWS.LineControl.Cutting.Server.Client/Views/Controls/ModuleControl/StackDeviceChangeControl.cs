using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class StackDeviceChangeControl : UserControl,IView
    {

        private StackDeviceChangeControlPresenter presenter = new StackDeviceChangeControlPresenter();
        public StackDeviceChangeControl()
        {
            InitializeComponent();
            ViewInit();
            dtpPlanDate.Value = DateTime.Today;
            this.RegisterTipsMessage();
            this.Register<List<SpiltMDBResult>>(StackDeviceChangeControlPresenter.BindingData, data =>
                {
                    gridViewBase1.DataSource = data;
                    btnSearch.Enabled = true;
                    btnDelete.Enabled = true;
                });
            this.Register<bool>(StackDeviceChangeControlPresenter.BindingData,result=>btnSave.Enabled=true);
            this.Register<List<DeviceInfos>>(StackDeviceChangeControlPresenter.BindingData, ExecuteData);
            this.BindingPresenter(presenter);
            Disposed += (sender, args) => this.UnBindingPresenter();
        }

        List<Tuple<int, string>> taskTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();
        List<Tuple<int, string>> mdbTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();

        private void ExecuteData(List<DeviceInfos> deviceInfoses)
        {

            GridViewDataColumn column = gridViewBase1.radGridView.Columns["DeviceName"];
            if (column is GridViewComboBoxColumn)
            {
                (column as GridViewComboBoxColumn).DataSource = deviceInfoses;
            }
        }

        private void ViewInit()
        {
            SpiltMDBResult sl;
            DeviceInfos dis;
            gridViewBase1.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo {FieldName = nameof(sl.PlanDate), DataType = typeof(DateTime), HeaderText = "计划日期"},
                new ColumnInfo {FieldName = nameof(sl.BatchName), DataType = typeof(string), HeaderText = "批次号"},
                new ColumnInfo {FieldName = nameof(sl.ItemName), DataType = typeof(string), HeaderText = "垛号"},
                new ColumnInfo(-40) {FieldName = nameof(sl.BatchIndex), DataType = typeof(int), HeaderText = "批次顺序"},
                new ColumnInfo(-40) {FieldName = nameof(sl.ItemIndex), DataType = typeof(string), HeaderText = "垛顺序"},
                new ColumnInfo(EColumnType.ComboBox) {FieldName = nameof(sl.FinishedStatus),DataSource = taskTuples
                    ,DisplayMember = "Item2",ValueMember = "Item1",DataType = typeof(int), HeaderText = "任务状态"},
                new ColumnInfo(EColumnType.ComboBox) {FieldName = nameof(sl.MdbStatus),DataSource = mdbTuples
                    ,DisplayMember = "Item2",ValueMember = "Item1",DataType = typeof(int), HeaderText = "Mdb状态"},
                //new ColumnInfo {FieldName = nameof(sl.StartLoadingTime), DataType = typeof(DateTime), HeaderText = "允许上料时间",ReadOnly = false},
                new ColumnInfo(-30,EColumnType.ComboBox){HeaderText = "设备",FieldName = nameof(sl.DeviceName),DataType = typeof(string),DisplayMember = nameof(dis.DeviceDescription),ValueMember = nameof(dis.DeviceName),ReadOnly = false}
            });

            GridViewDateTimeColumn timeColumn = new GridViewDateTimeColumn(nameof(sl.StartLoadingTime))
            {
                HeaderText = "上料时间",
                DataType = typeof(DateTime),
                ReadOnly = false,
                Width = 100,
                Format = DateTimePickerFormat.Time
            };
            gridViewBase1.AddColumns(timeColumn);
            gridViewBase1.radGridView.CellEndEdit += RadGridView_CellEndEdit;
        }

        private void RadGridView_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.HeaderText == "上料时间")
                {
                    if (e.Row.DataBoundItem is SpiltMDBResult spiltMdbResult)
                    {
                        var batchName = spiltMdbResult.BatchName;
                        foreach (var row in gridViewBase1.Rows)
                        {
                            if (row.Cells[nameof(spiltMdbResult.BatchName)].Value.ToString() == batchName)
                            {
                                row.Cells[nameof(spiltMdbResult.StartLoadingTime)].Value = e.Value;
                            }
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Send(StackDeviceChangeControlPresenter.GetData,dtpPlanDate.Value);
            this.Send(StackDeviceChangeControlPresenter.GetDevices, "");
            btnSearch.Enabled = false;
            gridViewBase1.BeginWait();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            var changeCollection = gridViewBase1.ChangeRows;
            List<SpiltMDBResult> batchIndexInfos = new List<SpiltMDBResult>();
            foreach (var rowInfo in changeCollection.CurrEidtedRows)
            {
                if (rowInfo.DataBoundItem is SpiltMDBResult bii)
                {
                    batchIndexInfos.Add(bii);
                }
            }

            if (batchIndexInfos.Count == 0)
            {
                Tips("没有设备任务发生变更，无需保存！");
                btnSave.Enabled = true;
                return;
            }

            //foreach (var group in batchIndexInfos.GroupBy(item=>item.TRX_NUMBER))
            //{
            //    if (group.ToList().GroupBy(item => item.DeviceName).Count() > 1)
            //    {
            //        Tips("同一个领料单的任务无法分配到不同设备！");
            //        btnSave.Enabled = true;
            //        return;
            //    }
            //}


            this.Send(StackDeviceChangeControlPresenter.Save, batchIndexInfos);
            btnSave.Enabled = false;
        }

        private void Tips(string tips)
        {
            BeginInvoke((Action)(() => RadMessageBox.Show(this, tips, "提示")));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            if (gridViewBase1.SelectedRows.Count == 0)
            {
                Tips("请勾选需要删除的任务");
                btnDelete.Enabled = true;
                return;
            }

            List<SpiltMDBResult> stackInfos = new List<SpiltMDBResult>();
            foreach (var row in gridViewBase1.SelectedRows)
            {
                if (row.DataBoundItem is SpiltMDBResult si)
                {
                    stackInfos.Add(si);
                }
            }
            var form = new PassWordForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            DialogResult ret = form.ShowDialog(this);
            if (form.OpDialogResult != DialogResult.Yes)
            {
                btnDelete.Enabled = true;
                return;
            }
            var passWord = form.IsPass;
            if (passWord == false)
            {
                btnDelete.Enabled = true;
                return;
            }
            this.Send(StackDeviceChangeControlPresenter.Delete,stackInfos);
            gridViewBase1.BeginWait();
            btnDelete.Enabled = false;
        }
    }
}
