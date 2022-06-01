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

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Controls
{
    public partial class CuttingSimulationControl : UserControl,IView
    {
        private CuttingSimulationControlPresenter _presenter = new CuttingSimulationControlPresenter();
        public const string BindingCurrTasks = nameof(BindingCurrTasks);
        public const string BindingTaskParts = nameof(BindingTaskParts);
        public CuttingSimulationControl()
        {
            InitializeComponent();
            Register();
            cmbDevice.SelectedIndexChanged += CmbDevice_SelectedIndexChanged;
            dtpPlanDate.ValueChanged += (sender, args) => cmbDevice.DataSource = null;
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }


        private void Register()
        {
            this.Register< List<Tuple<string, List<SpiltMDBResult>>>>(BindingCurrTasks, ExecuteBindingCurrTasks);
            this.Register<DataTable>(BindingTaskParts, ExecuteBindingTaskParts);
        }

        private void CmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDevice.SelectedIndex != -1)
            {
                cmbItemName.DisplayMember = "ItemName";
                cmbItemName.DataSource = cmbDevice.SelectedValue;
                
            }
            
        }

        private void ExecuteBindingTaskParts(DataTable datas)
        {
            gridViewBase1.DataSource = datas;
        }

        private void ExecuteBindingCurrTasks(List<Tuple<string,List<SpiltMDBResult>>> currTasks)
        {
            cmbDevice.DisplayMember = "Item1";
            cmbDevice.ValueMember = "Item2";
            cmbDevice.DataSource = currTasks;
            
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
                    return;
                }

                this.Send(CuttingSimulationControlPresenter.GetTaskParts,spiltMdbResult);
            }
        }

        private void btnStartCutting_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Enabled = false;
        }
    }
}
