using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class CuttingStackListView : UserControl
    {
        LineControlCuttingService lineControlCuttingService;

        public CuttingStackListView()
        {
            InitializeComponent();

            lineControlCuttingService = new LineControlCuttingService();
            ViewInit();
        }

        private void ViewInit()
        {
            radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

            CuttingStackList cuttingStackList;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(cuttingStackList.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(cuttingStackList.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="花色", FieldName=nameof(cuttingStackList.RawMaterialID), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="OptimizationRun", FieldName=nameof(cuttingStackList.OptimizationRun), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="BoardsQty", FieldName=nameof(cuttingStackList.StackIndex), DataType=typeof(string), ReadOnly=true }
            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            fillData();
        }

        //获取选定日期的数据并填充到控件
        private void fillData()
        {
            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(() => {
                DateTime date = radDateTimePicker1.Value.Date;
                List<CuttingStackList> listCuttingStackList = lineControlCuttingService.GetCuttingStackListByPlanDate(date.Date);

                gridViewBase1.Invoke(new Action(delegate () {
                    gridViewBase1.DataSource = listCuttingStackList;
                }));
            })).Start();
        }
    }
}
