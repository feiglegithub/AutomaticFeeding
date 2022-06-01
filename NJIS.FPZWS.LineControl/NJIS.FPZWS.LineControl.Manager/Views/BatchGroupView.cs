using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class BatchGroupView : UserControl, UI.Common.Message.Extensions.Interfaces.IView
    {
        private BatchGroupPresenter presenter = new BatchGroupPresenter();
        public BatchGroupView()
        {
            InitializeComponent();
            this.RegisterTipsMessage();
            this.Register<List<BatchGroup>>(BatchGroupPresenter.BindingData,data=>
            {
                gvbBatchGroup.DataSource = data;
                btnSearch.Enabled = true;
            });

            this.Register<bool>(BatchGroupPresenter.BindingData, data => { btnSave.Enabled = true; });

            this.BindingPresenter(presenter);
            dtpPlanDate.Value = DateTime.Today;
            ViewInit();
        }

        private void ViewInit()
        {
            BatchGroup bg;
            gvbBatchGroup.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(-30){ HeaderText="组号", FieldName=nameof(bg.GroupId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="批次顺序", FieldName=nameof(bg.BatchIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="前批次号", FieldName=nameof(bg.FrontBatchName), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(+5){ HeaderText="上料时间", FieldName=nameof(bg.StartLoadingTime), DataType=typeof(DateTime), ReadOnly=false },
                new ColumnInfo(+5){ HeaderText="批次号", FieldName=nameof(bg.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="后批次号", FieldName=nameof(bg.NextBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(bg.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = BatchGroupStatus.Cutting.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
            });

            GridViewDateTimeColumn timeColumn = new GridViewDateTimeColumn(nameof(bg.StartLoadingTime))
            {
                HeaderText = "上料时间",
                DataType = typeof(DateTime),
                ReadOnly = false,
                Width = 100,
                Format = DateTimePickerFormat.Time
            };
            gvbBatchGroup.AddColumns(timeColumn);
            gvbBatchGroup.radGridView.CellEndEdit += RadGridView_CellEndEdit;
        }

        private void RadGridView_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.HeaderText == "上料时间")
                {
                    if (e.Row.DataBoundItem is BatchGroup batchGroup)
                    {
                        var groupId = batchGroup.GroupId;
                        foreach (var row in gvbBatchGroup.Rows)
                        {
                            if (Convert.ToInt32(row.Cells[nameof(batchGroup.GroupId)].Value) == groupId)
                            {
                                row.Cells[nameof(batchGroup.StartLoadingTime)].Value = e.Value;
                            }
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = dtpPlanDate.Value.Date;
            this.Send(BatchGroupPresenter.GetData,planDate);
            gvbBatchGroup.BeginWait();
            btnSearch.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<BatchGroup> batchGroups = new List<BatchGroup>();
            foreach (var row in gvbBatchGroup.ChangeRows.CurrEidtedRows)
            {
                if (row.DataBoundItem is BatchGroup batchGroup)
                {
                    batchGroups.Add(batchGroup);
                }
            }

            if (batchGroups.Count == 0)
            {
                Tips("没有需要保存的数据！");
                return;
            }

            this.Send(BatchGroupPresenter.Save, batchGroups);
            btnSave.Enabled = false;
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }
    }
}
