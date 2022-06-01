using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.UI.Common.Controls;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class PartFeedBackView : UserControl
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;
        public PartFeedBackView()
        {
            InitializeComponent();
            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
            ViewInit();
        }

        private void ViewInit()
        {
            PartFeedBack partFeedBack;
            gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(partFeedBack.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="设备名称", FieldName=nameof(partFeedBack.DeviceName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Pattern", FieldName=nameof(partFeedBack.PATTERN), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Parts", FieldName=nameof(partFeedBack.PartId), DataType=typeof(string), ReadOnly=true }
            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            fillData();
        }
        
        private void fillData()
        {
            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(() => {
                string batchName = radTextBoxBatchName.Text;
                List<PartFeedBack> listPartFeedBack = lineControlCuttingServicePlus.GetPartFeedBackByBatchName(batchName);

                gridViewBase1.Invoke(new Action(delegate () {
                    gridViewBase1.DataSource = listPartFeedBack;
                }));
            })).Start();
        }
    }
}
