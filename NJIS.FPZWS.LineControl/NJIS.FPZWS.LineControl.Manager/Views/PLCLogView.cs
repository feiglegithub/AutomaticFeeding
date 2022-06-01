using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class PLCLogView : UserControl
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;
        public PLCLogView()
        {
            InitializeComponent();
            ViewInit();

            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        }

        private void ViewInit()
        {
            radDateTimePicker1.Text = System.DateTime.Now.ToShortDateString();

            PLCLog plcLog;
            gridViewBase1.AddColumns(new List<ColumnInfo>() {
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(plcLog.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="工位/站台", FieldName=nameof(plcLog.Station), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="触发类型", FieldName=nameof(plcLog.TriggerType), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="详细信息", FieldName=nameof(plcLog.Detail), DataType=typeof(string), ReadOnly=true,TextAlignment = ContentAlignment.MiddleLeft },
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(plcLog.CreatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="日志类型", FieldName=nameof(plcLog.LogType), DataType=typeof(string), ReadOnly=true }
            });
        }

        private void radButtonSearch_Click(object sender, EventArgs e)
        {
            gridViewBase1.BeginWait();

            new Thread(new ThreadStart(()=> {
                DateTime date = radDateTimePicker1.Value.Date;
                List<PLCLog> listPLCLog = lineControlCuttingServicePlus.GetPLCLogByDate(date.Date);
                
                gridViewBase1.Invoke(new Action(delegate() {
                    gridViewBase1.DataSource = listPLCLog;
                }));
            })).Start();
            
        }
    }
}
