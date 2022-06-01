using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Utils;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    public partial class BatchNamePilerNoBindForm : Form
    {
        string batchName;
        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public BatchNamePilerNoBindForm(string batchName)
        {
            this.batchName = batchName;
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

            InitializeComponent();
            ViewInit();
            DataInit();
        }

        private void ViewInit()
        {
            List<Tuple<bool, string>> listHasUpProtect = new List<Tuple<bool, string>>();
            listHasUpProtect.Add(new Tuple<bool, string>(false, "无"));
            listHasUpProtect.Add(new Tuple<bool, string>(true, "有"));

            BatchNamePilerNoBind batchNamePilerNoBind;

            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(batchNamePilerNoBind.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="ReqId", FieldName=nameof(batchNamePilerNoBind.ReqId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(batchNamePilerNoBind.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="MES垛号", FieldName=nameof(batchNamePilerNoBind.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="WMS垛号", FieldName=nameof(batchNamePilerNoBind.PilerNo), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="任务ID", FieldName=nameof(batchNamePilerNoBind.TaskId), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="板件数量", FieldName=nameof(batchNamePilerNoBind.Count), DataType=typeof(int), ReadOnly=false },
                new ColumnInfo(50){ HeaderText="物料编码", FieldName=nameof(batchNamePilerNoBind.ProductCode), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="上保护板",
                    FieldName =nameof(batchNamePilerNoBind.HasUpProtect), DataType=typeof(bool), ReadOnly=false,
                DataSource = listHasUpProtect,DisplayMember = "Item2",ValueMember ="Item1"},
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(batchNamePilerNoBind.CreateTime), DataType=typeof(string), ReadOnly=true },
               
            });
        }

        private void DataInit()
        {

            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(() => {
                while (!this.IsHandleCreated)
                {

                }

                gridViewBase1.Invoke(new Action(delegate () {

                    gridViewBase1.DataSource = getBatchNamePilerNoBind(batchName);
                }));
            })).Start();
        }

        private List<BatchNamePilerNoBind> getBatchNamePilerNoBind(string batchName)
        {
            return lineControlCuttingServicePlus.GetBatchNamePilerNoBindByBatchName(batchName);
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();

            if (passwordValidationForm.dialogResult == DialogResult.OK)
            {
                if (passwordValidationForm.result)
                {
                    List<BatchNamePilerNoBind> listBatchNamePilerNoBind = new List<BatchNamePilerNoBind>();
                    List<PLCLog> listPLCLog = new List<PLCLog>();

                    foreach (var row in gridViewBase1.ChangeRows.CurrEidtedRows)
                    {
                        if (row.DataBoundItem is BatchNamePilerNoBind batchNamePilerNoBind)
                        {
                            listBatchNamePilerNoBind.Add(batchNamePilerNoBind);

                            PLCLog plcLog = PublicUtils.newPLCLog("System", TriggerType.LineControl, $"手动更改，" +
                                $"BatchNamePilerNoBind(PilerNo:{batchNamePilerNoBind.PilerNo})板件数量：{batchNamePilerNoBind.Count}，上保护板：{batchNamePilerNoBind.HasUpProtect}", LogType.GENERAL);
                            listPLCLog.Add(plcLog);
                        }
                    }

                    if (listBatchNamePilerNoBind.Count == 0)
                    {
                        Tips("没有需要保存的数据！");
                        return;
                    }

                    lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);
                    lineControlCuttingServicePlus.BulkInsertPLCLog(listPLCLog);
                }
                else
                {
                    Tips("密码验证失败！");
                }
            }
            
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }

        private void radButtonDelete_Click(object sender, EventArgs e)
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();

            if (passwordValidationForm.dialogResult == DialogResult.OK)
            {
                if (passwordValidationForm.result)
                {
                    List<GridViewRowInfo> listGridViewRowInfo = gridViewBase1.SelectedRows;
                    if (listGridViewRowInfo.Count > 0)
                    {
                        GridViewRowInfo gridViewRowInfo = listGridViewRowInfo[0];
                        if (gridViewRowInfo.DataBoundItem is BatchNamePilerNoBind batchNamePilerNoBind)
                        {
                            bool flag = lineControlCuttingServicePlus.DeleteBatchNamePilerNoBind(batchNamePilerNoBind);
                            if (flag)
                            {
                                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", 
                                    TriggerType.LineControl, $"手动删除BatchNamePilerNoBind，Id:{batchNamePilerNoBind.Id}" +
                                    $"，PilerNo:{batchNamePilerNoBind.PilerNo}，StackName:{batchNamePilerNoBind.StackName}"
                                    ,LogType.GENERAL));
                                DataInit();
                            }
                            else
                            {
                                MessageBox.Show("删除失败！");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有选择任何项！");
                    }
                }
                else
                {
                    Tips("密码验证失败！");
                }
            }
        }
    }
}
