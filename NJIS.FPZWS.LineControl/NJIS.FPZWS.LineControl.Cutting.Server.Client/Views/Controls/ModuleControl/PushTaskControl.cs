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
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using NJIS.FPZWS.UI.Common.Message.Extensions;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class PushTaskControl : UserControl,IView
    {
        public const string ReceiveDatas = nameof(ReceiveDatas);

        public const string ReceiveDeviceInfos = nameof(ReceiveDeviceInfos);

        //private readonly Telerik.WinControls.UI.GridViewComboBoxColumn _deviceNameColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
        //{
        //    HeaderText = "设备号",
        //    FieldName = "DeviceName",
        //    DataType = typeof(string),
        //    DisplayMember = "DeviceName",
        //    Width = 100,
        //    ValueMember = "DeviceName",
        //    DropDownStyle = RadDropDownStyle.DropDownList,
        //    AllowSort = true,
        //    ReadOnly = true
        //};

        private PushTaskControlPresenter _presenter = new PushTaskControlPresenter();
        public PushTaskControl()
        {
            InitializeComponent();
            gridViewBase1.radGridView.ReadOnly = false;
            List<Tuple<int, string>> tuples = FinishedStatus.Cut.GetAllFinishStatusDescription();

            //new List<Tuple<int, string>>()
            //{
            //    FinishedStatus.Cut.GetFinishStatusDescription(),
            //    FinishedStatus.Cutting.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLost.GetFinishStatusDescription(),
            //    FinishedStatus.NeedToSaw.GetFinishStatusDescription(),
            //    FinishedStatus.LoadedMaterial.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLoaded.GetFinishStatusDescription(),
            //    FinishedStatus.CreatingSaw.GetFinishStatusDescription(),
            //    FinishedStatus.CreatedSaw.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLoading.GetFinishStatusDescription(),
            //    FinishedStatus.MdbUnloaded.GetFinishStatusDescription(),
            //    FinishedStatus.WaitMaterial.GetFinishStatusDescription(),

            //};

            gridViewBase1.AddColumns(new List<UI.Common.Controls.ColumnInfo>()
            {
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="设备号", FieldName="DeviceName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="批次号", FieldName="BatchName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="生产日期", FieldName="PlanDate", DataType=typeof(DateTime), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="堆垛号", FieldName="ItemName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="预计耗时", FieldName="EstimatedTime", DataType=typeof(TimeSpan), ReadOnly=true },

            });
            Telerik.WinControls.UI.GridViewComboBoxColumn cmBoxColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
            {
                DataSource = tuples,
                DisplayMember = "Item2",
                ValueMember = "Item1",
                HeaderText = "状态",
                FieldName = "FinishedStatus",
                DataType = typeof(int),
                Width = 100,
                ReadOnly = true
            };
            gridViewBase1.AddColumns(cmBoxColumn);
            gridViewBase1.ShowCheckBox = true;
            //gridViewBase1.radGridView.Columns.Insert(2, _deviceNameColumn);
            this.Disposed += ProductionTaskControl_Disposed;
            this.Register<List<SpiltMDBResult>>(ReceiveDatas, ExecuteBindingDatas);
            //this.Register<List<DeviceInfos>>(ReceiveDeviceInfos, ExecuteBindingDeviceInfos);
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
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.Stocked))
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

        //private void ExecuteBindingDeviceInfos(List<DeviceInfos> deviceInfos)
        //{
        //    _deviceNameColumn.DataSource = deviceInfos;
        //}

        private void btnPush_Click(object sender, EventArgs e)
        {

            List<SpiltMDBResult> spiltMdbResults = GetSelectedItem();
            if (spiltMdbResults.Count == 0)
            {
                this.BeginInvoke((Action)(() => RadMessageBox.Show(this, "请勾选推送的任务")));
                return;
            }
            this.Send(PushTaskControlPresenter.PushTask, spiltMdbResults);
        }

        private void btnGetStackList_Click(object sender, EventArgs e)
        {
            //this.Send(ProductionTaskControlPresenter.GetDeviceInfos, "");
            this.Send(PushTaskControlPresenter.GetCanPushTask, dtpPlanDate.Value);
            this.gridViewBase1.BeginWait();
        }



        private List<SpiltMDBResult> GetSelectedItem()
        {
            List<SpiltMDBResult> spiltMdbResults = new List<SpiltMDBResult>();
            foreach (var row in gridViewBase1.SelectedRows)
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
    }
}
