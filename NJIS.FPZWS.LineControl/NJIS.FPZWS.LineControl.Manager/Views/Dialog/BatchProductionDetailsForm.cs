using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Utils;
using NJIS.FPZWS.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    public partial class BatchProductionDetailsForm : Form
    {
        string batchName;
        LineControlCuttingServicePlus lineControlCuttingServicePlus;
        public BatchProductionDetailsForm(string batchName)
        {
            this.batchName = batchName;
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

            InitializeComponent();
            ViewInit();
            DataInit();
        }

        private void DataInit()
        {
            
            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(() => {
                while (!this.IsHandleCreated)
                {

                }

                gridViewBase1.Invoke(new Action(delegate () {

                    gridViewBase1.DataSource = getBatchProductionDetails(batchName);
                }));
            })).Start();
        }

        private List<BatchProductionDetails> getBatchProductionDetails(string batchName)
        {
            return lineControlCuttingServicePlus.GetBatchProductionDetailsByBatchName(batchName);
        }

        private void ViewInit()
        {
            BatchProductionDetails batchProductionDetails;

            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(batchProductionDetails.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(batchProductionDetails.BatchName), DataType=typeof(string), ReadOnly=true },
                //new ColumnInfo(50){ HeaderText="物料编码", FieldName=nameof(batchProductionDetails.ProductCode), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="板件总数", FieldName=nameof(batchProductionDetails.Total), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="仍需数量", FieldName=nameof(batchProductionDetails.DifferenceNumber), DataType=typeof(int), ReadOnly=false },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(batchProductionDetails.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = BatchGroupPlusStatus.InProduction.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
            });
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();

            if (passwordValidationForm.dialogResult == DialogResult.OK)
            {
                if (passwordValidationForm.result)
                {
                    List<BatchProductionDetails> listBatchProductionDetails = new List<BatchProductionDetails>();
                    List<PLCLog> listPLCLog = new List<PLCLog>();

                    foreach (var row in gridViewBase1.ChangeRows.CurrEidtedRows)
                    {
                        if (row.DataBoundItem is BatchProductionDetails batchProductionDetails)
                        {
                            listBatchProductionDetails.Add(batchProductionDetails);

                            PLCLog plcLog = PublicUtils.newPLCLog("System", TriggerType.LineControl, $"手动更改，" +
                                $"BatchProductionDetails仍需数量：{batchProductionDetails.DifferenceNumber}", LogType.GENERAL);
                            listPLCLog.Add(plcLog);
                        }
                    }

                    if (listBatchProductionDetails.Count == 0)
                    {
                        Tips("没有需要保存的数据！");
                        return;
                    }

                    lineControlCuttingServicePlus.BulkUpdateBatchProductionDetails(listBatchProductionDetails);
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

        private void radButtonBatchPilerDetail_Click(object sender, EventArgs e)
        {
            BatchNamePilerNoBindForm batchNamePilerNoBindForm = new BatchNamePilerNoBindForm(batchName);
            batchNamePilerNoBindForm.ShowDialog();
        }
    }
}
