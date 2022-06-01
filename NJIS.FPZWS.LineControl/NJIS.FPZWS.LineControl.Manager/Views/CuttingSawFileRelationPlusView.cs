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
using Telerik.WinControls;
using System.Threading;
using NJIS.FPZWS.LineControl.Manager.Views.Dialog;
using Telerik.WinControls.UI;
using NJIS.FPZWS.LineControl.Manager.Utils;
using NJIS.FPZWS.LineControl.Cutting.Service;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class CuttingSawFileRelationPlusView : UserControl, UI.Common.Message.Extensions.Interfaces.IView
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;
        LineControlCuttingService lineControlCuttingService;
        public CuttingSawFileRelationPlusView()
        {
            InitializeComponent();
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
            lineControlCuttingService = new LineControlCuttingService();
            ViewInit();
        }

        private void ViewInit()
        {
            try
            {
                radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

                List<NJIS.FPZWS.LineControl.Cutting.Model.DeviceInfos> listDeviceInfos = lineControlCuttingService.
                    GetCuttingDeviceInfos();
                List<string> listDeviceName = new List<string>();
                foreach (var item in listDeviceInfos)
                {
                    listDeviceName.Add(item.DeviceName);
                }

                CuttingSawFileRelationPlus sawFile;
                gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(sawFile.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="计划日期", FieldName=nameof(sawFile.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(sawFile.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(sawFile.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="SAW文件序号", FieldName=nameof(sawFile.StackIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="叠板数量", FieldName=nameof(sawFile.BoardCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Saw文件名", FieldName=nameof(sawFile.SawFileName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Saw文件类型", FieldName=nameof(sawFile.SawType), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(sawFile.CreatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="更新时间", FieldName=nameof(sawFile.UpdatedTime), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(50){ HeaderText="任务Id", FieldName=nameof(sawFile.TaskId), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(50){ HeaderText="子任务Id", FieldName=nameof(sawFile.TaskDistributeId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(sawFile.Status), DataType=typeof(int), ReadOnly=false
                    ,DataSource = CuttingSawFileRelationPlusStatus.Unassigned.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                //new ColumnInfo(50){HeaderText="开料设备",FieldName=nameof(sawFile.DeviceName),DataType=typeof(string),ReadOnly=true},
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="开料设备", FieldName=nameof(sawFile.DeviceName), DataType=typeof(string), ReadOnly=false
                    ,DataSource = listDeviceName}
            });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            fillData();
        }

        //获取选定日期的数据并填充到控件
        private void fillData()
        {
            try
            {
                gridViewBase1.BeginWait();

                new Thread(new ThreadStart(() => {
                    DateTime date = radDateTimePicker1.Value.Date;
                    List<CuttingSawFileRelationPlus> listCuttingSawFileRelation = lineControlCuttingServicePlus
                    .GetCuttingSawFileRelationPlusByCreateTime(date.Date, "Id,PlanDate,BatchName,StackName,StackIndex," +
                    "BoardCount,SawFileName,SawType,CreatedTime,UpdatedTime,Status,DeviceName");

                    gridViewBase1.Invoke(new Action(delegate () {
                        gridViewBase1.DataSource = listCuttingSawFileRelation;
                    }));
                })).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void radButton2Search_Click(object sender, EventArgs e)
        {
            string sawFileName = radTextBoxControlSAWFileName.Text;
            if (!"".Equals(sawFileName.Trim()))
            {
                gridViewBase1.BeginWait();

                try
                {
                    List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus
                        .GetCuttingSawFileRelationPlusBySawFileName(sawFileName);
                    gridViewBase1.DataSource = listCuttingSawFileRelationPlus;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                MessageBox.Show("SAW文件名不能为空");
            }
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();

            try
            {
                if (passwordValidationForm.dialogResult == DialogResult.OK)
                {
                    if (passwordValidationForm.result)
                    {
                        List<CuttingSawFileRelationPlus> listCuttingSawFileRelation = new List<CuttingSawFileRelationPlus>();
                        List<BatchNamePilerNoBind> listBatchNamePilerNoBind = new List<BatchNamePilerNoBind>();
                        List<PLCLog> listPLCLog = new List<PLCLog>();

                        foreach (var row in gridViewBase1.ChangeRows.CurrEidtedRows)
                        {
                            if (row.DataBoundItem is CuttingSawFileRelationPlus cuttingSawFileRelationPlus)
                            {
                                cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                                listCuttingSawFileRelation.Add(cuttingSawFileRelationPlus);

                                //如果修改状态为未分配，更新BatchNamePilerNoBind表中的数量
                                //if (cuttingSawFileRelationPlus.Status == (int)CuttingSawFileRelationPlusStatus.Unassigned)
                                //{
                                //    List<BatchNamePilerNoBind> listBatchNamePilerNoBind2 = lineControlCuttingServicePlus.
                                //    GetBatchNamePilerNoBindByStackName(cuttingSawFileRelationPlus.StackName);
                                //    if (listBatchNamePilerNoBind2.Count > 0)
                                //    {
                                //        BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind2[0];
                                //        batchNamePilerNoBind.Count += cuttingSawFileRelationPlus.BoardCount;
                                //        listBatchNamePilerNoBind.Add(batchNamePilerNoBind);
                                //    }
                                //}

                                PublicUtils.updatePilerCount(lineControlCuttingServicePlus, cuttingSawFileRelationPlus);

                                string status = CuttingSawFileRelationPlusStatus.Assigned.GetAllFinishStatusDescription()[cuttingSawFileRelationPlus.Status].Item2;

                                PLCLog plcLog = newPLCLog("System", TriggerType.LineControl, $"手动更改SAW文件：" +
                                    $"{cuttingSawFileRelationPlus.SawFileName}，状态：{status}，开料设备：{cuttingSawFileRelationPlus.DeviceName}", LogType.GENERAL);
                                listPLCLog.Add(plcLog);
                            }
                        }

                        if (listCuttingSawFileRelation.Count == 0)
                        {
                            Tips("没有需要保存的数据！");
                            return;
                        }

                        lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelation);
                        lineControlCuttingServicePlus.BulkInsertPLCLog(listPLCLog);

                        if (listBatchNamePilerNoBind.Count > 0)
                        {
                            lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);
                        }

                        fillData();
                    }
                    else
                    {
                        Tips("密码验证失败！");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }

        private PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = detail;
            plcLog.LogType = logType.GetFinishStatusDescription().Item2;
            return plcLog;
        }

        private void radButtonPushAgain_Click(object sender, EventArgs e)
        {
            List<GridViewRowInfo> list = gridViewBase1.SelectedRows;
            if (list.Count < 1)
            {
                MessageBox.Show("未选择数据");
                return;
            }

            PushAgainForm pushAgainForm = new PushAgainForm();
            pushAgainForm.ShowDialog();

            try
            {
                if (pushAgainForm.dialogResult == DialogResult.OK)
                {
                    GridViewRowInfo gridViewRowInfo = list[0];
                    string sawFileName = gridViewRowInfo.Cells[7].Value.ToString();

                    List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus
                        .GetCuttingSawFileRelationPlusBySawFileName(sawFileName);
                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];

                    string newSawFileName = sawFileName.Split('.')[0] + "T.saw";

                    List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus2 = lineControlCuttingServicePlus
                        .GetCuttingSawFileRelationPlusBySawFileName(newSawFileName);
                    if (listCuttingSawFileRelationPlus2.Count > 0)
                    {
                        MessageBox.Show($"此Saw文件：{sawFileName}，已经补推过一次！不能再补推");
                        return;
                    }

                    if (pushAgainForm.boardCount > cuttingSawFileRelationPlus.BoardCount)
                    {
                        MessageBox.Show($"叠板数量不能大于：{cuttingSawFileRelationPlus.BoardCount}");
                        return;
                    }

                    //cuttingSawFileRelationPlus.BoardCount -= pushAgainForm.boardCount;
                    //lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);

                    cuttingSawFileRelationPlus.BoardCount = pushAgainForm.boardCount;
                    cuttingSawFileRelationPlus.CreatedTime = DateTime.Now;
                    cuttingSawFileRelationPlus.DeviceName = "";
                    cuttingSawFileRelationPlus.SawFileName = newSawFileName;
                    cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Unassigned;
                    cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;

                    bool flag = lineControlCuttingServicePlus
                        .BulkInsertCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", TriggerType.LineControl,
                        $"手动补推Saw文件：{cuttingSawFileRelationPlus.SawFile}", LogType.GENERAL));

                    fillData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
