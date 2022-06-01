using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Controls;
using System.Threading;
using Telerik.WinControls.UI;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.Utils;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class CuttingSawFileRelationView : UserControl
    {
        LineControlCuttingService lineControlCuttingService;
        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public CuttingSawFileRelationView()
        {
            InitializeComponent();

            lineControlCuttingService = new LineControlCuttingService();
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
            ViewInit();
        }

        private void ViewInit()
        {
            radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

            Cutting.Model.CuttingSawFileRelation sawFile;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="计划日期", FieldName=nameof(sawFile.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(sawFile.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(sawFile.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="SAW文件序号", FieldName=nameof(sawFile.StackIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="叠板数量", FieldName=nameof(sawFile.BoardCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Saw文件名", FieldName=nameof(sawFile.SawFileName), DataType=typeof(string), ReadOnly=true }
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
                List<Cutting.Model.CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingService
                .GetCuttingSawFileRelationByPlanDate(date.Date);

                gridViewBase1.Invoke(new Action(delegate () {
                    gridViewBase1.DataSource = listCuttingSawFileRelation;
                }));
            })).Start();
        }

        private void radButtonDistribution_Click(object sender, EventArgs e)
        {
            string error;
            List<GridViewRowInfo> list = gridViewBase1.SelectedRows;
            if (list.Count < 1)
                MessageBox.Show("未选择数据");
            else
            {
                GridViewRowInfo gridViewRowInfo = list[0];
                string sawFileName = gridViewRowInfo.Cells[6].Value.ToString();

                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus
                    .GetCuttingSawFileRelationPlusBySawFileName(sawFileName);

                if (listCuttingSawFileRelationPlus.Count > 0)
                {
                    MessageBox.Show($"该Saw文件：{sawFileName}已经分配，不能再分配！");
                    return;
                }

                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus
                    .GetCuttingSawFileRelationBySawFileName(sawFileName);

                //bool flag = CuttingSawFileRelationPlusDBUtils.AddAssignedByCuttingSawFileRelation(lineControlCuttingServicePlus,
                //    listCuttingSawFileRelation[0]);

                if (listCuttingSawFileRelation.Count > 0)
                {
                    bool flag = PublicUtils.ManuallyIssueSawFile(lineControlCuttingServicePlus, listCuttingSawFileRelation[0],
                    out error);

                    if (flag)
                        MessageBox.Show("操作成功");
                    else
                        MessageBox.Show(error);
                }else
                    MessageBox.Show($"SAW文件：{sawFileName}在CuttingSawFileRelation表中不存在！");

            }
        }
    }
}
