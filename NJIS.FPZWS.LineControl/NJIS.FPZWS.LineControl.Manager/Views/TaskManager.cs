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
using NJIS.FPZWS.LineControl.Manager.Utils;
using NJIS.FPZWS.LineControl.Manager.Views.Dialog;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class TaskManager : UserControl
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;
        public TaskManager()
        {
            InitializeComponent();
            ViewInit();

            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        }

        private void ViewInit()
        {
            radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

            BatchGroup batchGroup;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(batchGroup.LineId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="计划日期", FieldName=nameof(batchGroup.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(batchGroup.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次顺序", FieldName=nameof(batchGroup.BatchIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="状态", FieldName=nameof(batchGroup.Status), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(EColumnType.ComboBox)
                //{ HeaderText="状态", FieldName=nameof(batchGroup.Status), DataType=typeof(int), ReadOnly=true
                //    ,DataSource = BatchGroupStatus.Cut.GetAllFinishStatusDescription()
                //    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(batchGroup.CreatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="更新时间", FieldName=nameof(batchGroup.UpdatedTime), DataType=typeof(string), ReadOnly=true },
                
            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            labelInfo.Text = "";

            gridViewBase1.BeginWait();
            
            new Thread(new ThreadStart(()=> {
                gridViewBase1.Invoke(new Action(delegate () {
                    gridViewBase1.DataSource = getBatchGroupPlusCopy();
                }));
            })).Start();
            
        }
        

        private void radButtonDetail_Click(object sender, EventArgs e)
        {
            List<GridViewRowInfo> list = gridViewBase1.SelectedRows;
            if (list.Count < 1)
                MessageBox.Show("未选择项");
            else
            {
                GridViewRowInfo gridViewRowInfo = list[0];
                GridViewCellInfo gridViewCellInfo = gridViewRowInfo.Cells[3];
                string batchName = gridViewCellInfo.Value.ToString();

                //MessageBox.Show(gridViewCellInfo.Value.ToString());

                BatchProductionDetailsForm batchProductionDetailsForm = new BatchProductionDetailsForm(batchName);
                batchProductionDetailsForm.ShowDialog();
                /*
                列出所以批次的生产情况    
             */
            }
        }

        /// <summary>
        /// 生产按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonProduce_Click(object sender, EventArgs e)
        {
            labelInfo.Text = "";

            List<GridViewRowInfo> list = gridViewBase1.SelectedRows;
            if (list.Count < 1)
                MessageBox.Show("未选择数据");
            else
            {
                GridViewRowInfo gridViewRowInfo = list[0];

                string batchName = gridViewRowInfo.Cells[3].Value.ToString();

                List<BatchGroupPlus> listBatchGroupPlus1 = lineControlCuttingServicePlus
                    .GetBatchGroupPlusByBatchName(batchName);

                if (!gridViewRowInfo.Cells[5].Value.ToString().Equals("未生产"))
                {
                    MessageBox.Show("只能操作未生产任务！");
                    return;
                }

                if (listBatchGroupPlus1.Count > 0)
                {
                    MessageBox.Show($"只能操作未生产任务！该批次号：{batchName}在BatchGroupPlus表中已存在");
                    return;
                }
                
                //新增待生产批次信息
                BatchGroupPlus batchGroupPlus = new BatchGroupPlus();
                batchGroupPlus.BatchIndex = int.Parse(gridViewRowInfo.Cells[4].Value.ToString());
                batchGroupPlus.BatchName = gridViewRowInfo.Cells[3].Value.ToString();
                batchGroupPlus.CreatedTime = DateTime.Now;//Convert.ToDateTime(gridViewRowInfo.Cells[6].Value.ToString());
                batchGroupPlus.PlanDate = Convert.ToDateTime(gridViewRowInfo.Cells[2].Value.ToString());
                //batchGroupPlus.StartLoadingTime = DateTime.Now;
                batchGroupPlus.Status = (int)BatchGroupPlusStatus.WaitingForProduction;
                batchGroupPlus.UpdatedTime = DateTime.Now;
                //batchGroupPlus.LineId = int.Parse(gridViewRowInfo.Cells[1].Value.ToString());

                List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
                listBatchGroupPlus.Add(batchGroupPlus);

                //SawType=1的数据为已分配给6号锯，统计数量时应将其剔除
                string stackNames = "";
                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelationPlusByBatchNameAndSawType(batchGroupPlus.BatchName,SawType.TYPE2);
                for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
                {
                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[i];
                    stackNames += $"'{cuttingSawFileRelationPlus.StackName}'";

                    if (i < listCuttingSawFileRelationPlus.Count - 1)
                        stackNames += ",";
                }
                if (string.IsNullOrEmpty(stackNames))
                    stackNames += "''";

                //统计批次需要板件数量
                //List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus
                //    .GetCuttingSawFileRelationByBatchName(batchGroupPlus.BatchName);
                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus
                    .GetCuttingSawFileRelation(batchGroupPlus.BatchName,stackNames,SawType.TYPE1);
                int total = 0;
                if (listCuttingSawFileRelation.Count > 0)
                {
                    foreach (var item in listCuttingSawFileRelation)
                    {
                        total += item.BoardCount;
                    }

                    List<CuttingStackList> listCuttingStackList = lineControlCuttingServicePlus.
                        GetCuttingStackListByBatchName(batchGroupPlus.BatchName);
                    if (listCuttingStackList.Count > 0)
                    {
                        CuttingStackList cuttingStackList = listCuttingStackList[0];

                        BatchProductionDetails batchProductionDetails = new BatchProductionDetails();
                        batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                        batchProductionDetails.DifferenceNumber = total;
                        batchProductionDetails.ProductCode = "";//cuttingStackList.RawMaterialID;
                        batchProductionDetails.Status = (int)BatchGroupPlusStatus.WaitingForProduction;
                        batchProductionDetails.Total = total;

                        List<BatchProductionDetails> listBactchProductionDetails = new List<BatchProductionDetails>();
                        listBactchProductionDetails.Add(batchProductionDetails);

                        lineControlCuttingServicePlus.BulkInsertBatchProductionDetails(listBactchProductionDetails);

                        //将批次组信息新增到BatchGroupPlus
                        lineControlCuttingServicePlus.BulkInsertBatchGroupPlus(listBatchGroupPlus);
                    }
                    else
                    {
                        string info = $"CuttingStackList表中不存在批次号为：{batchGroupPlus.BatchName}的数据";

                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("local", TriggerType.LineControl,info,LogType.ABNORMAL));
                        labelInfo.Text = info;
                    }
                }
                else
                {
                    string info = $"CuttingSawFileRelation表中不存在批次号为：{batchGroupPlus.BatchName}的数据";

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("local", TriggerType.LineControl,info,LogType.ABNORMAL));
                    labelInfo.Text = info;
                }

                gridViewBase1.BeginWait();

                new Thread(new ThreadStart(() => {
                    gridViewBase1.Invoke(new Action(delegate () {

                        gridViewBase1.DataSource = getBatchGroupPlusCopy();
                    }));
                })).Start();
            }
        }

        private List<BatchGroupPlusCopy> getBatchGroupPlusCopy()
        {
            DateTime date = radDateTimePicker1.Value.Date;
            //List<BatchGroup> listBatchGroup = lineControlCuttingServicePlus.GetBatchGroupsByPlanDate(date);
            List<CuttingStackList> listCuttingStatckList = lineControlCuttingServicePlus.
                GetCuttingStackListGroupByBatchNameByPlanDate(date,CuttingStackListBatchType.AUTO);

            List<BatchGroupPlusCopy> listBatchGroupCopy = new List<BatchGroupPlusCopy>();

            foreach (var item in listCuttingStatckList)
            {


                BatchGroupPlusCopy batchGroupPlusCopy = new BatchGroupPlusCopy();
                batchGroupPlusCopy.BatchIndex = (int)item.BatchProductIndex;
                batchGroupPlusCopy.BatchName = item.BatchName;
                batchGroupPlusCopy.PlanDate = date;
                //batchGroupPlusCopy.CreatedTime = item.CreatedTime;
                //batchGroupPlusCopy.LineId = item.LineID;

                //batchGroupPlusCopy.StartLoadingTime = item.StartLoadingTime;
                //batchGroupPlusCopy.UpdatedTime = item.UpdatedTime;

                List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.
                    GetBatchGroupPlusByBatchName(item.BatchName);
                if (listBatchGroupPlus.Count < 1)
                {
                    batchGroupPlusCopy.Status = "未生产";
                    //batchGroupPlusCopy.CreatedTime = item.CreatedTime;
                    //batchGroupPlusCopy.UpdatedTime = (DateTime)item.LastUpdatedTime;
                }
                else
                {
                    //int TargetNumber = 0;
                    //int ProductionQuantity = 0;

                    //List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                    //GetCuttingSawFileRelationByBatchName(item.BatchName);

                    //foreach (var item1 in listCuttingSawFileRelation)
                    //{
                    //    TargetNumber += item1.BoardCount;
                    //}

                    //List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                    //GetCuttingSawFileRelationPlusByBatchNameAndStatus(item.BatchName,(int)CuttingSawFileRelationPlusStatus.Downloaded);

                    //foreach (var item2 in listCuttingSawFileRelationPlus)
                    //{
                    //    ProductionQuantity += item2.BoardCount;
                    //}

                    //if (TargetNumber - ProductionQuantity == 0)
                    //    batchGroupPlusCopy.Status = "生产完成";
                    //else
                    //    batchGroupPlusCopy.Status = "生产中";

                    BatchGroupPlus batchGroupPlus = listBatchGroupPlus[0];
                    batchGroupPlusCopy.LineId = batchGroupPlus.LineId;
                    batchGroupPlusCopy.Status = BatchGroupPlusStatus.InProduction.GetAllFinishStatusDescription()[batchGroupPlus.Status].Item2;
                    batchGroupPlusCopy.CreatedTime = (DateTime)batchGroupPlus.CreatedTime;
                    batchGroupPlusCopy.UpdatedTime = (DateTime)batchGroupPlus.UpdatedTime;
                }

                listBatchGroupCopy.Add(batchGroupPlusCopy);
            }
            return listBatchGroupCopy;
        }
    }
}
