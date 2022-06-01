using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Simulator.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Controls
{
    public partial class WMSLoadSimulationControl : UserControl,IView
    {
        public const string ReceiveDatas = nameof(ReceiveDatas);
        private WMSSimulationControlPresenter _presenter = new WMSSimulationControlPresenter();
        public WMSLoadSimulationControl()
        {
            InitializeComponent();
            ViewInit();
            this.RegisterTipsMessage();
            this.Register<List<SpiltMDBResult>>(ReceiveDatas, ExecuteBindingDatas);
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void ViewInit()
        {
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
            this.cmbWay.SelectedIndexChanged += CmbWay_SelectedIndexChanged;

        }

        private void CmbWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbWay.SelectedIndex;
            switch (index)
            {
                case 0: btnLoadMaterial.Enabled = true;
                    btnLoadedMaterial.Enabled = false;
                    btnLoadingMaterial.Enabled = false;break;
                case 1: btnLoadMaterial.Enabled = false;
                    btnLoadedMaterial.Enabled = false;
                    btnLoadingMaterial.Enabled = true;break;
                case 2: btnLoadMaterial.Enabled = false;
                    btnLoadedMaterial.Enabled = true;
                    btnLoadingMaterial.Enabled = false;break;
                default:break;
                    ;
                
            }
        }

        private void ExecuteBindingDatas(List<SpiltMDBResult> spiltMdbResults)
        {
            this.gridViewBase1.DataSource = spiltMdbResults;
            this.gridViewBase1.EndWait();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbWay.Text == "待上料")
            {
                this.Send(WMSSimulationControlPresenter.GetLoadDatas,dtpPlanDate.Value.Date);
            }

            if (cmbWay.Text == "请求上料")
            {
                this.Send(WMSSimulationControlPresenter.GetLoadingDatas, dtpPlanDate.Value.Date);
            }

            if (cmbWay.Text == "上料中")
            {
                this.Send(WMSSimulationControlPresenter.GetLoadedDatas, dtpPlanDate.Value.Date);
            }
        }

        private void btnPushMDB_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                Tips("请勾选需要上料的任务！");
                return;
            }

            if (CheckCanLoadMaterial(selectedItems))
            {
                this.Send(WMSSimulationControlPresenter.LoadMaterial,selectedItems);
            }
        }

        private bool CheckCanLoadMaterial(List<SpiltMDBResult> spiltMdbResults)
        {
            List<string> deviceNameList = new List<string>();
            var groups = spiltMdbResults.GroupBy(item => item.DeviceName);
            foreach (var group in groups)
            {
                if (group.Count() > 1)
                {
                    deviceNameList.Add(group.Key);
                }
            }

            if (deviceNameList.Count > 0)
            {
                string msg = string.Join("\n", deviceNameList);
                Tips("同一个设备同时只能上料一个任务：\n"+msg);
                return false;
            }

            return true;
        }

        private bool CheckCanLoadingMaterial(List<SpiltMDBResult> spiltMdbResults)
        {
            return !spiltMdbResults.Exists(item => item.FinishedStatus != Convert.ToInt32(FinishedStatus.WaitMaterial));
        }
        private bool CheckCanLoadedMaterial(List<SpiltMDBResult> spiltMdbResults)
        {
            return !spiltMdbResults.Exists(item => item.FinishedStatus != Convert.ToInt32(FinishedStatus.LoadingMaterial));
        }

        private List<SpiltMDBResult> GetSelectedItems()
        {
            List<SpiltMDBResult> selectedList = new List<SpiltMDBResult>();
            var rows = gridViewBase1.SelectedRows;
            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    if (row.DataBoundItem is SpiltMDBResult itemData)
                    {
                        selectedList.Add(itemData);
                    }
                }
            }

            return selectedList;
        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(this, msg)));
        }

        private void btnLoadingMaterial_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                Tips("请勾选需要上料的任务！");
                return;
            }
            if (CheckCanLoadingMaterial(selectedItems))
            {
                this.Send(WMSSimulationControlPresenter.LoadingMaterial, selectedItems);
            }
        }

        private void btnLoadedMaterial_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                Tips("请勾选需要上料的任务！");
                return;
            }
            if (CheckCanLoadedMaterial(selectedItems))
            {
                this.Send(WMSSimulationControlPresenter.LoadedMaterial, selectedItems);
            }
        }
    }
}
