using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using System.Threading;
using Telerik.WinControls.UI;
using System.Configuration;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class TaskManger6Control : UserControl
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public TaskManger6Control()
        {
            InitializeComponent();
            ViewInit();

            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        }

        private void ViewInit()
        {
            radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

            CuttingSawFileRelationPlus cuttingSawFileRelationPlus;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(cuttingSawFileRelationPlus.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="计划日期", FieldName=nameof(cuttingSawFileRelationPlus.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(cuttingSawFileRelationPlus.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(cuttingSawFileRelationPlus.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛顺序", FieldName=nameof(cuttingSawFileRelationPlus.StackIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="板件数量", FieldName=nameof(cuttingSawFileRelationPlus.BoardCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(cuttingSawFileRelationPlus.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = CuttingSawFileRelationPlusStatus.Unassigned.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(cuttingSawFileRelationPlus.CreatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="更新时间", FieldName=nameof(cuttingSawFileRelationPlus.UpdatedTime), DataType=typeof(string), ReadOnly=true },

            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(() =>
            {
                gridViewBase1.Invoke(new Action(delegate ()
                {

                    gridViewBase1.DataSource = getData();
                }));
            })).Start();
        }

        private List<CuttingSawFileRelationPlus> getData()
        {
            DateTime date = radDateTimePicker1.Value.Date;

            //已经分配给自动线1-5号锯的批次
            List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlus(date, "BatchName");
            
            string batchNames = "";
            for (int i = 0; i < listBatchGroupPlus.Count; i++)
            {
                batchNames += $"'{listBatchGroupPlus[i].BatchName}'";
                if (i < listBatchGroupPlus.Count - 1)
                {
                    batchNames += ",";
                }
            }
            if (string.IsNullOrEmpty(batchNames))
                batchNames = "''";

            //已分配给6号锯的垛
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlus(date, "Id,PlanDate,BatchName,StackName,StackIndex,BoardCount,Status," +
                "CreatedTime,UpdatedTime", SawType.TYPE2);
            
            string stackNames = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                stackNames += $"'{listCuttingSawFileRelationPlus[i].StackName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                {
                    stackNames += ",";
                }
            }

            if (string.IsNullOrEmpty(stackNames))
                stackNames = "''";


            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                GetCuttingSawFileRelation(date, stackNames, batchNames, "Id,PlanDate,BatchName,StackName,StackIndex,BoardCount," +
                "CreatedTime,UpdatedTime", SawType.TYPE2);
            foreach (var item in listCuttingSawFileRelation)
            {
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
                cuttingSawFileRelationPlus.Id = item.Id;
                cuttingSawFileRelationPlus.PlanDate = item.PlanDate;
                cuttingSawFileRelationPlus.BatchName = item.BatchName;
                cuttingSawFileRelationPlus.StackName = item.StackName;
                cuttingSawFileRelationPlus.StackIndex = item.StackIndex;
                cuttingSawFileRelationPlus.BoardCount = item.BoardCount;
                cuttingSawFileRelationPlus.CreatedTime = item.CreatedTime;
                cuttingSawFileRelationPlus.UpdatedTime = item.UpdatedTime;
                cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Unassigned;

                listCuttingSawFileRelationPlus.Add(cuttingSawFileRelationPlus);
            }

            return listCuttingSawFileRelationPlus;
        }

        private void radButtonAssign_Click(object sender, EventArgs e)
        {
            List<GridViewRowInfo> list = gridViewBase1.SelectedRows;
            if (list.Count < 1)
            {
                MessageBox.Show("未选择数据");
                return;
            }

            GridViewRowInfo gridViewRowInfo = list[0];
            if(gridViewRowInfo.DataBoundItem is CuttingSawFileRelationPlus cuttingSawFileRelationPlus)
            {
                string batchName= cuttingSawFileRelationPlus.BatchName;
                string stackName = cuttingSawFileRelationPlus.StackName;

                List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.
                    GetBatchGroupPlusByBatchName(batchName);
                if (listBatchGroupPlus.Count > 0)
                {
                    MessageBox.Show($"该垛：{stackName}所属的批次已经下发，不允许将该垛分配给6号锯！");
                    return;
                }

                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelationPlusByStackName(stackName, SawType.TYPE2);
                if(listCuttingSawFileRelationPlus.Count > 0)
                {
                    MessageBox.Show($"该垛：{stackName}已经下发给6号锯！");
                    return;
                }

                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelationByStackName(stackName, SawType.TYPE2);
                if (listCuttingSawFileRelation.Count > 0)
                {
                    CuttingSawFileRelation cuttingSawFileRelation = listCuttingSawFileRelation[0];

                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus1 = new CuttingSawFileRelationPlus();
                    cuttingSawFileRelationPlus1.BatchName = cuttingSawFileRelation.BatchName;
                    cuttingSawFileRelationPlus1.BoardCount = cuttingSawFileRelation.BoardCount;
                    cuttingSawFileRelationPlus1.CreatedTime = DateTime.Now;
                    cuttingSawFileRelationPlus1.DeviceName = ConfigurationManager.AppSettings["deviceName6"];
                    //cuttingSawFileRelationPlus1.Id = cuttingSawFileRelation.Id;
                    cuttingSawFileRelationPlus1.PlanDate = cuttingSawFileRelation.PlanDate;
                    cuttingSawFileRelationPlus1.SawFile = cuttingSawFileRelation.SawFile;
                    cuttingSawFileRelationPlus1.SawFileName = cuttingSawFileRelation.SawFileName;
                    cuttingSawFileRelationPlus1.SawType = cuttingSawFileRelation.SawType;
                    cuttingSawFileRelationPlus1.StackIndex = cuttingSawFileRelation.StackIndex;
                    cuttingSawFileRelationPlus1.StackName = cuttingSawFileRelation.StackName;
                    cuttingSawFileRelationPlus1.Status = (int)CuttingSawFileRelationPlusStatus.ManualAllocation;
                    cuttingSawFileRelationPlus1.TaskDistributeId = cuttingSawFileRelation.TaskDistributeId;
                    cuttingSawFileRelationPlus1.TaskId = cuttingSawFileRelation.TaskId;
                    cuttingSawFileRelationPlus1.UpdatedTime = DateTime.Now;

                    bool flag = lineControlCuttingServicePlus.InsertCuttingSawFileRelationPlus(cuttingSawFileRelationPlus1);
                    if (flag)
                    {
                        gridViewBase1.BeginWait();

                        new Thread(new ThreadStart(() =>
                        {
                            gridViewBase1.Invoke(new Action(delegate ()
                            {
                                gridViewBase1.DataSource = getData();
                            }));
                        })).Start();
                    }
                    else { MessageBox.Show("CuttingSawFileRelationPlus新增数据失败！"); }

                }
                else
                    MessageBox.Show($"CuttingSawFileRelation表中没有该垛：{stackName}数据");
            }else
                MessageBox.Show($"gridViewRowInfo.DataBoundItem is not CuttingSawFileRelationPlus");
        }
    }
}
