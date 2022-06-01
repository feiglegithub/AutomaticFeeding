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
using NJIS.FPZWS.UI.Common.Controls;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class NotCuttingDataControl : UserControl
    {
        LineControlCuttingService lineControlCuttingService;
        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public NotCuttingDataControl()
        {
            InitializeComponent();
            lineControlCuttingService = new LineControlCuttingService();
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

            ViewInit();
        }

        private void ViewInit()
        {
            NotCuttingData notCuttingData;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="计划时间", FieldName=nameof(notCuttingData.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(notCuttingData.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(notCuttingData.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="SAW文件名", FieldName=nameof(notCuttingData.SawFileName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="SAW文件序号", FieldName=nameof(notCuttingData.StackIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="叠板数量", FieldName=nameof(notCuttingData.BoardCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(notCuttingData.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = Cutting.ModelPlus.CuttingSawFileRelationPlusStatus.Unassigned.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"}
            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            string batchName = radTextBoxBatchName.Text;
            if (string.IsNullOrEmpty(batchName))
                return;

            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(()=> {
                gridViewBase1.Invoke(new Action(()=> {
                    string pattern = "";
                    List<NJIS.FPZWS.LineControl.Cutting.ModelPlus.PartFeedBack> listPattern = 
                    lineControlCuttingServicePlus.GetPattern(batchName);
                    for (int i = 0; i < listPattern.Count; i++)
                    {
                        pattern += $"'{listPattern[i].PATTERN}.saw'";
                        if (i < listPattern.Count - 1)
                            pattern += ",";
                    }

                    if (string.IsNullOrEmpty(pattern))
                        pattern = "''";

                    gridViewBase1.DataSource = lineControlCuttingService.GetNotCuttingData(batchName,pattern);
                }));
            })).Start();
        }
    }
}
