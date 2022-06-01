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
using NJIS.FPZWS.LineControl.Cutting.Simulator.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Controls
{
    public partial class WMSStockSimulationControl : UserControl,IView
    {
        public const string BindingDatas = nameof(BindingDatas);

        private WMSStockSimulationControlPresenter presenter = new WMSStockSimulationControlPresenter();
        public WMSStockSimulationControl()
        {
            InitializeComponent();
            cmbWay.SelectedIndexChanged += CmbWay_SelectedIndexChanged;
            ViewInit();
            this.Register<List<WMSCuttingStackList>>(BindingDatas,datas=> gridViewBase1.DataSource=datas);
            this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void ViewInit()
        {
            WMSCuttingStackList wms;
            gridViewBase1.AddColumns(new List<UI.Common.Controls.ColumnInfo>()
            {
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="批次", FieldName=nameof(wms.BatchName), DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="堆垛", FieldName=nameof(wms.StackName), DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-20){ HeaderText="备料状态", FieldName=nameof(wms.WMSStatus), DataType=typeof(bool), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="生产日期", FieldName=nameof(wms.PlanDate), DataType=typeof(DateTime), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="花色", FieldName=nameof(wms.RawMaterialID), DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="堆垛位置", FieldName=nameof(wms.StackIndex), DataType=typeof(int), ReadOnly=true },
            });
            
        }
        private void CmbWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbWay.SelectedIndex;
            switch (index)
            {
                case 0:
                    btnBeginStockMaterial.Enabled = true;
                    btnStockMaterial.Enabled = false; break;
                case 1:
                    btnBeginStockMaterial.Enabled = false;
                    btnStockMaterial.Enabled = true; break;
                default:
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int index = cmbWay.SelectedIndex;
            switch (index)
            {
                case 0:
                     this.Send(WMSStockSimulationControlPresenter.GetUnStockDatas, dtpPlanDate.Value.Date);
                    break;
                case 1:
                    this.Send(WMSStockSimulationControlPresenter.GetStockingDatas, dtpPlanDate.Value.Date);
                    break;
                default:
                    break;
            }
        }

        private List<WMSCuttingStackList> GetSelectedItems()
        {
            List<WMSCuttingStackList> selectedList = new List<WMSCuttingStackList>();
            var rows = gridViewBase1.SelectedRows;
            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    if (row.DataBoundItem is WMSCuttingStackList itemData)
                    {
                        selectedList.Add(itemData);
                    }
                }
            }

            return selectedList;
        }

        private void btnBeginStockMaterial_Click(object sender, EventArgs e)
        {
            var selectDatas = GetSelectedItems();
            if (selectDatas.Count == 0)
            {
                Tips("请勾选开始需要备料的任务");
                return;
            }
            this.Send(WMSStockSimulationControlPresenter.BeginStock,selectDatas);
        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(this, msg)));
        }

        private void btnStockMaterial_Click(object sender, EventArgs e)
        {
            var selectDatas = GetSelectedItems();
            if (selectDatas.Count == 0)
            {
                Tips("请勾选备料完成的任务");
                return;
            }
            this.Send(WMSStockSimulationControlPresenter.EndStock, selectDatas);
        }
    }
}
