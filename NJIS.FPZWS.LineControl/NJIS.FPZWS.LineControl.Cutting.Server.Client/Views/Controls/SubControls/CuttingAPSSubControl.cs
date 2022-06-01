using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls
{
    public partial class CuttingAPSSubControl : UserControl
    {
        public CuttingAPSSubControl()
        {
            InitializeComponent();
            ViewInit();
            cmbStack.DisplayMember = "item1";
            cmbStack.ValueMember = "item2";
            cmbDevice.DisplayMember = "item1";
            cmbDevice.ValueMember = "item2";
            cmbBatch.DisplayMember = "item1";
            cmbBatch.ValueMember = "item2";

            gridViewBase2.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            cmbBatch.SelectedIndexChanged += CmbBatch_SelectedIndexChanged;
            cmbDevice.SelectedIndexChanged += CmbDevice_SelectedIndexChanged;
            cmbStack.SelectedIndexChanged += CmbStack_SelectedIndexChanged;

        }

        private void CmbStack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStack.SelectedIndex >= 0)
            {
                if (cmbStack.SelectedValue is List<AllTask> allTasks)
                {
                    gridViewBase3.DataSource = allTasks;
                    txtTotalTime.Text = allTasks.Sum(item => item.TotalTime).ToString();
                }
                    
            }
        }

        private void CmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDevice.SelectedIndex >=0)
            {
                if (cmbDevice.SelectedValue is DeviceTaskInfo deviceTaskInfo)
                {
                    List<Tuple<string,List<AllTask>>> lst = new List<Tuple<string, List<AllTask>>>();
                    //Dictionary<string, List<AllTask>> dictionary = new Dictionary<string, List<AllTask>>();
                    foreach (var group in deviceTaskInfo.NewTasks.GroupBy(item => item.ItemName))
                    {
                        lst.Add(new Tuple<string, List<AllTask>>(group.Key,group.ToList()));
                        //dictionary.Add(group.Key, group.ToList());
                    }
                    cmbStack.DataSource = lst;
                    foreach (var row in gridViewBase2.Rows)
                    {
                        if (row.DataBoundItem is DeviceTaskInfo data)
                        {
                            if (data.BatchName == cmbBatch.Text && data.DeviceName == cmbDevice.Text)
                            {
                                row.IsSelected = true;
                                break;
                            }
                        }
                    }
                   
                }
            }
        }

        private void CmbBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBatch.SelectedIndex >=0)
            {
                if (cmbBatch.SelectedValue is List<DeviceTaskInfo> deviceTaskInfos)
                {
                    List<Tuple<string,DeviceTaskInfo>> lst = new List<Tuple<string, DeviceTaskInfo>>();
                    //Dictionary<string,DeviceTaskInfo> dictionary = new Dictionary<string, DeviceTaskInfo>();
                    foreach (var group in deviceTaskInfos.GroupBy(item => item.DeviceName))
                    {
                        lst.Add(new Tuple<string, DeviceTaskInfo>(group.Key,group.ToList()[0]));
                        //dictionary.Add(group.Key, group.ToList()[0]);
                    }
                    cmbDevice.DataSource = lst;
                    
                }
            }
        }

        public void BindingDatas(List<DeviceTaskInfo> datas)
        {
            List<Tuple<string, List<DeviceTaskInfo>>> lst = new List<Tuple<string, List<DeviceTaskInfo>>>();
            //Dictionary<string,List<DeviceTaskInfo>> dictionary = new Dictionary<string, List<DeviceTaskInfo>>();
            foreach (var group in datas.GroupBy(item => item.BatchName))
            {
                lst.Add(new Tuple<string, List<DeviceTaskInfo>>(group.Key,group.ToList()));
                //dictionary.Add(group.Key,group.ToList());
            }
            cmbBatch.DataSource = lst;

            gridViewBase2.DataSource = datas;
        }

        private void ViewInit()
        {
            DeviceTaskInfo dti;
            gridViewBase2.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+20){ HeaderText = "批次",FieldName = nameof(dti.BatchName),DataType = typeof(string)},
                new ColumnInfo(){ HeaderText = "预计耗时",FieldName = nameof(dti.Time), DataType = typeof(TimeSpan)},
                new ColumnInfo(){ HeaderText = "板材数",FieldName = nameof(dti.TotalBook) , DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "切割周期",FieldName = nameof(dti.CutTimes) , DataType = typeof(int)},
                new ColumnInfo(-20){ HeaderText = "堆垛容量",FieldName =nameof(dti.MaxCount) , DataType = typeof(int)},
                new ColumnInfo(-10){ HeaderText = "堆垛数",FieldName = nameof(dti.StackCount), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "设备名",FieldName = nameof(dti.DeviceName), DataType = typeof(int)},
            });
            AllTask allTask;
            gridViewBase3.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText = "批次",FieldName = nameof(allTask.BatchName),DataType = typeof(string)},
                new ColumnInfo(){ HeaderText = "板材数",FieldName =nameof(allTask.BookNum), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "花色",FieldName =nameof(allTask.RawMaterialID), DataType = typeof(string)},
                new ColumnInfo(){ HeaderText = "单次时间",FieldName = nameof(allTask.CYCLE_TIME), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "切割周期",FieldName = nameof(allTask.CutTimes), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "预计耗时",FieldName = nameof(allTask.TotalTime), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "锯切图",FieldName = nameof(allTask.PTN_INDEX), DataType = typeof(short)},
                new ColumnInfo(){ HeaderText = "堆垛号",FieldName = nameof(allTask.ItemName), DataType = typeof(string)},
                
            });
        }

        private void RadGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is DeviceTaskInfo data)
            {
                gridViewBase3.DataSource = null;
                //gridViewBase3.DataSource = data.Tasks;
                gridViewBase3.DataSource = data.NewTasks;
                txtTotalTime.Text = data.NewTasks.Sum(item => item.TotalTime).ToString();
                cmbBatch.SelectedText = data.BatchName;
                cmbDevice.SelectedText = data.DeviceName;
                

            }
        }
    }
}
