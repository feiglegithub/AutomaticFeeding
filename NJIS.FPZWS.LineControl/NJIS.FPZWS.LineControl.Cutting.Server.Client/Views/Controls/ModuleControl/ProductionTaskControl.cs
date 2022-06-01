using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class ProductionTaskControl : UserControl,IView
    {
        public const string ReceiveDatas = nameof(ReceiveDatas);

        public const string ReceiveDeviceInfos = nameof(ReceiveDeviceInfos);

        private readonly Telerik.WinControls.UI.GridViewComboBoxColumn _deviceNameColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
        {
            HeaderText = "设备号",
            FieldName = "DeviceName",
            DataType = typeof(string),
            DisplayMember = "DeviceName",
            Width = 100,
            ValueMember = "DeviceName",
            DropDownStyle = RadDropDownStyle.DropDownList,
            AllowSort = true,
            ReadOnly = false
        };

        private ProductionTaskControlPresenter _presenter = new ProductionTaskControlPresenter();
        public ProductionTaskControl()
        {
            InitializeComponent();
            dtpPlanDate.Value = DateTime.Today;
            gridViewBase1.radGridView.ReadOnly = false;
            List<Tuple<int, string>> taskTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();
            List<Tuple<int, string>> mdbTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();

            gridViewBase1.AddColumns(new List<UI.Common.Controls.ColumnInfo>()
            {
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="批次号", FieldName="BatchName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="生产日期", FieldName="PlanDate", DataType=typeof(DateTime), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="堆垛号", FieldName="ItemName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="预计耗时", FieldName="EstimatedTime", DataType=typeof(TimeSpan), ReadOnly=true },
               
            });
            Telerik.WinControls.UI.GridViewComboBoxColumn taskCmBoxColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
            {
                DataSource = taskTuples,
                DisplayMember = "Item2",
                ValueMember = "Item1",
                HeaderText = "任务状态",
                FieldName = "FinishedStatus",
                DataType = typeof(int),
                Width = 100,
                ReadOnly = true
            };

            Telerik.WinControls.UI.GridViewComboBoxColumn mdbCmBoxColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
            {
                DataSource = mdbTuples,
                DisplayMember = "Item2",
                ValueMember = "Item1",
                HeaderText = "mdb状态",
                FieldName = "MdbStatus",
                DataType = typeof(int),
                Width = 100,
                ReadOnly = true
            };
            gridViewBase1.AddColumns(taskCmBoxColumn);
            gridViewBase1.AddColumns(mdbCmBoxColumn);
            gridViewBase1.ShowCheckBox=true;
            gridViewBase1.radGridView.Columns.Insert(2, _deviceNameColumn);
            this.Disposed += ProductionTaskControl_Disposed;
            this.Register<List<SpiltMDBResult>>(ReceiveDatas, ExecuteBindingDatas);
            this.Register<List<DeviceInfos>>(ReceiveDeviceInfos, ExecuteBindingDeviceInfos);
            this.RegisterTipsMessage();
            this.BindingPresenter(_presenter);
        }

        private void ProductionTaskControl_Disposed(object sender, EventArgs e)
        {
            this.UnBindingPresenter();
        }

        private void ExecuteBindingDatas(List<SpiltMDBResult> spiltMdbResults)
        {
            this.gridViewBase1.DataSource = spiltMdbResults;
            foreach (var row in gridViewBase1.radGridView.Rows)
            {

                var data = (row.DataBoundItem as SpiltMDBResult);
                if (data.FinishedStatus >= Convert.ToInt32(FinishedStatus.CreatingSaw))//设备已下载
                {
                    
                    row.Cells["DeviceName"].ReadOnly = true;
                    row.Cells[0].ReadOnly = true;
                }
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.Cut))
                {
                    row.Cells[0].ReadOnly = true;
                    foreach (Telerik.WinControls.UI.GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.DarkGreen;
                    }
                    continue;
                }
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.MdbLoaded))
                {
                    //row.Cells[0].ReadOnly = true;
                    foreach (Telerik.WinControls.UI.GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.DeepSkyBlue;
                    }
                    continue;
                }
            }
            this.gridViewBase1.EndWait();
        }

        private void ExecuteBindingDeviceInfos(List<DeviceInfos> deviceInfos)
        {
            _deviceNameColumn.DataSource = deviceInfos;
        }

        //private void btnPushMDB_Click(object sender, EventArgs e)
        //{

        //    List<SpiltMDBResult> spiltMdbResults = GetSelectedItem();
        //    if (spiltMdbResults.Count == 0)
        //    {
        //        this.BeginInvoke((Action)(() => RadMessageBox.Show(this, "请选择推送的任务或者检查设备是否为空")));
        //        return;
        //    }
        //    this.Send(ProductionTaskControlPresenter.PushMdbFile, spiltMdbResults);
        //}

        private void btnGetStackList_Click(object sender, EventArgs e)
        {
            this.Send(ProductionTaskControlPresenter.GetDeviceInfos, "");
            this.Send(ProductionTaskControlPresenter.GetStackLists, dtpPlanDate.Value);
            this.gridViewBase1.BeginWait();
        }



        private List<SpiltMDBResult> GetSelectedItem()
        {
            List<SpiltMDBResult> spiltMdbResults = new List<SpiltMDBResult>();
            foreach(var row in gridViewBase1.SelectedRows)
            {
                SpiltMDBResult spiltMdbResult = row.DataBoundItem as SpiltMDBResult;
                if (string.IsNullOrWhiteSpace(spiltMdbResult.DeviceName))
                {
                    continue;
                }
                spiltMdbResults.Add(spiltMdbResult);
            }
            return spiltMdbResults;
        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(this, msg)));
        }
    }
}
