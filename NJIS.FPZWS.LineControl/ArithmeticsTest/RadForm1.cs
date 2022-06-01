using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Controls;
using Telerik.WinControls;

namespace ArithmeticsTest
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public RadForm1()
        {
            InitializeComponent();
            gvbSolution.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            gvbSolutionDetail.radGridView.CellDoubleClick += RadGridView_CellDoubleClick1;
            ViewInit();
        }

        private void ViewInit()
        {
            SolutionDetail sd;
            gvbSolutionDetail.AddColumns(new List<NJIS.FPZWS.UI.Common.Controls.ColumnInfo>()
            {
                new ColumnInfo()
                {
                    HeaderText = "设备号", FieldName = nameof(sd.DeviceName), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "总时间", FieldName = nameof(sd.TotalTime), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "期望时间", FieldName = nameof(sd.ExpectTotalTime), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "余料板", FieldName = nameof(sd.OffPartNum), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "工件", FieldName = nameof(sd.PartNum), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "叠板率", FieldName = nameof(sd.Overlapping), DataType = typeof(string),
                    ReadOnly = false
                },
                new ColumnInfo()
                {
                    HeaderText = "速度(s/块)", FieldName = nameof(sd.PartSpeed), DataType = typeof(string),
                    ReadOnly = false
                },

            });
        }


        private void RadGridView_CellDoubleClick1(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            var details = gvbSolutionDetail.GetSelectItemsData<SolutionDetail>();

            gvbTask.DataSource = details[0].AllTasks;
        }


        private void RadGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            var solution = gvbSolution.GetSelectItemsData<Solution>();

            gvbSolutionDetail.DataSource =
                solutionDetails.FindAll(item => item.SolutionId == solution[0].SolutionId);
        }

        private List<Solution> solutions = null;
        private List<SolutionDetail> solutionDetails = null;
        public void BindingData(List<Solution> solutions, List<SolutionDetail> solutionDetails)
        {
            this.solutions = solutions;
            this.solutionDetails = solutionDetails;
            this.gvbSolution.DataSource = solutions;
            //this.gvbSolutionDetail.DataSource = solutionDetails;
        }

        private void gvbSolution_Load(object sender, EventArgs e)
        {
            
        }
    }
}
