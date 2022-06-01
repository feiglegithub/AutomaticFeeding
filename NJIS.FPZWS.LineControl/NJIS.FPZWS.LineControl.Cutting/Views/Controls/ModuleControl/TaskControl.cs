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
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    public partial class TaskControl : UserControl,IView
    {
        public const string ReceiveDatas = nameof(ReceiveDatas);
        public const string ReceiveDeviceId = nameof(ReceiveDeviceId);

        public const string SelectItemName = nameof(SelectItemName);

        private TaskControlPresenter _presenter = TaskControlPresenter.GetInstance();
        public TaskControl()
        {
            InitializeComponent();

            List<Tuple<int, string>> taskTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();
            List<Tuple<int, string>> mdbTuples = FinishedStatus.Cut.GetAllFinishStatusDescription();

            this.gridViewBase1.AddColumns(new List<FPZWS.UI.Common.Controls.ColumnInfo>()
            {
                new FPZWS.UI.Common.Controls.ColumnInfo() { DataType = typeof(string), HeaderText = "批次号", FieldName = "BatchName" },
                new FPZWS.UI.Common.Controls.ColumnInfo(){DataType = typeof(string),HeaderText="堆垛号", FieldName="ItemName"},
                new FPZWS.UI.Common.Controls.ColumnInfo(){DataType = typeof(TimeSpan),HeaderText="预计耗时", FieldName="EstimatedTime"},
                
            });
            GridViewComboBoxColumn taskCmBoxColumn = new GridViewComboBoxColumn()
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
            GridViewComboBoxColumn mdbCmBoxColumn = new GridViewComboBoxColumn()
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
            gridViewBase1.ReadOnly = false;
            gridViewBase1.ShowCheckBox = true;
            Register();
            this.RegisterTipsMessage();
            this.BindingPresenter(_presenter);
            this.VisibleChanged += TaskControl_VisibleChanged;
            //this.itemNamesControl1.BindingPresenter(MainFormPresenter.GetInstance());

        }

        private void TaskControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Send(TaskControlPresenter.GetDeviceName, "");
            }
        }

        private void Register()
        {
            this.Register<List<SpiltMDBResult>>(ReceiveDatas, ExecuteBindingDatas);
            this.Register<string>(ReceiveDeviceId, ExecuteBindingDeviceName);

            this.Register<string>(SelectItemName, ExecuteSelectItem);
        }



        private void ExecuteSelectItem(string itemName)
        {
            FindTask(itemName);
        }



        private void ExecuteBindingDatas(List<SpiltMDBResult> spiltMdbResults)
        {
            this.gridViewBase1.DataSource = spiltMdbResults;
            this.itemNamesControl1.Enabled = !spiltMdbResults.Exists(item =>
                item.FinishedStatus >=
                Convert.ToInt32(FinishedStatus.NeedToSaw) && item.FinishedStatus < Convert.ToInt32(FinishedStatus.Cut));
            foreach (var row in gridViewBase1.radGridView.Rows)
            {
                var data = (row.DataBoundItem as SpiltMDBResult);
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.Cut))
                {
                    row.Cells[0].ReadOnly = true;
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.DarkGreen;
                    }
                    continue;
                }
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.Stocked))
                {
                    //row.Cells[0].ReadOnly = true;
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.DeepSkyBlue;
                    }
                    continue;
                }
                if (data.FinishedStatus == Convert.ToInt32(FinishedStatus.CreatingSaw) || data.FinishedStatus == Convert.ToInt32(FinishedStatus.CreatedSaw))
                {
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.Coral;
                    }
                    continue;
                }
            }
            this.gridViewBase1.EndWait();
        }


        private void ExecuteBindingDeviceName(string deviceName)
        {
            this.lbDeviceName.Text = deviceName;
        }

        private void btnGetStackList_Click(object sender, EventArgs e)
        {
            this.Send(TaskControlPresenter.GetStackLists, dtpPlanDate.Value);
            this.gridViewBase1.BeginWait();
        }

        private List<SpiltMDBResult> GetSelectedItem()
        {
            List<SpiltMDBResult> spiltMdbResults = new List<SpiltMDBResult>();
            var Rows = gridViewBase1.SelectedRows;
            foreach (var row in Rows)
            {
                //if (Convert.ToBoolean(row.Cells[0].Value))
                //{
                SpiltMDBResult spiltMdbResult = row.DataBoundItem as SpiltMDBResult;
                //if(spiltMdbResult.FinishedStatus>=Convert.ToInt32(FinishedStatus.NeedToSaw))
                //{
                //    continue;
                //}
                spiltMdbResults.Add(spiltMdbResult);
                //}
            }
            return spiltMdbResults;
        }

        private void btnGetMDB_Click(object sender, EventArgs e)
        {
            var spiltMDBResults = GetSelectedItem();
            if (spiltMDBResults.Count == 0)
            {
                this.BeginInvoke((Action)(() => RadMessageBox.Show(this, "请选择需要下载的MDB")));
                return;
            }
            this.Send(TaskControlPresenter.GetMdbFile, spiltMDBResults);
        }

        private void FindTask(string itemName)
        {
            foreach (var row in gridViewBase1.Rows)
            {
                if (row.Cells["ItemName"].Value.ToString().Trim() == itemName)
                {
                    row.IsSelected = true;
                    row.IsCurrent = true;
                    return;
                }
            }
        }
    }
}
