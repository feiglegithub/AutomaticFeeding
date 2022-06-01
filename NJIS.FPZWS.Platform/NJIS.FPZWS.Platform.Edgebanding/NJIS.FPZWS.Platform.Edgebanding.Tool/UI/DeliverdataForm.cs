using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using NJIS.FPZWS.Platform.Contract;
using NJIS.FPZWS.Platform.Edgebanding.Tool.Settings;
using NJIS.FPZWS.Platform.Edgebanding.Tool;
using NJIS.FPZWS.Platform.Service;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;



namespace NJIS.FPZWS.LineControl.DeliverData.Tool.UI
{
    public partial class DeliverdataForm : Telerik.WinControls.UI.RadForm 
    {
        private readonly IEdgebandingContract _edgebandingContract =
          new EdgebandingService();

        //private readonly IEdgebandingContract _edgebandingContract =
        //   ServiceLocator.Current.GetInstance<IEdgebandingContract>();

        //private readonly IEdgebandingFeedBackContract _edgebandingFeedBackContract =
        //    ServiceLocator.Current.GetInstance<IEdgebandingFeedBackContract>();

        //private readonly IEdgebandingMachineFeedbackContract _edgebandingMachineFeedbackContract =
        //    ServiceLocator.Current.GetInstance<IEdgebandingMachineFeedbackContract>();

        public DeliverdataForm()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
            starttime.Format = DateTimePickerFormat.Custom;
            starttime.CustomFormat = @"yyyy-MM-dd";
            starttime.Value = DateTime.Now;
            endtime.Format = DateTimePickerFormat.Custom;
            endtime.CustomFormat = @"yyyy-MM-dd";
            endtime.Value = DateTime.Now;
           
            radGridView1.ShowRowNumber();
        }
            
        private void radButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(starttime.Text) || starttime.Value > DateTime.Now)
            {
                MessageBox.Show(@"开始时间请输入小于当前时间的日期！");
                return;
            }
            if (string.IsNullOrEmpty(endtime.Text) || endtime.Value > DateTime.Now)
            {
                MessageBox.Show(@"结束时间请输入小于当前时间的日期！");
                return;
            }
            var startTime = starttime.Value;
            var endTime = endtime.Value;

            radGridView1.Rows.Clear();
            WaitForm.ShowWait(this, () =>
             {
                 var edgebanding = _edgebandingContract.GetTaskDistributeId(startTime, endTime);
                 radGridView1.DataBind(edgebanding);
                 //foreach (var item in edgebanding)
                 //{
                 //    radGridView1.Rows.Add("", item.BatchName, item.CreatedTime);
                 //}
                 //if (radGridView1.Rows.Count != 0)
                 //{
                 //    radButton1.Enabled = true;
                 //    radButton2.Enabled = true;
                 //}
                 //else
                 //{
                 //    MessageBox.Show("查询无数据！", "提示");
                 //}
             });         
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count == 0)
            {
                MessageBox.Show("无数据可下发","提示");
                return;
            }
            if (MessageBox.Show("确认下发数据？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = 0; i < radGridView1.Rows.Count; i++)
                {
                    if (radGridView1.Rows[i].IsSelected == true)
                    {
                        string taskDistributeId = radGridView1.Rows[i].Cells[1].Value.ToString(); 
                        StringBuilder edgebandingsql = new StringBuilder("values");
                        var delole = _edgebandingContract.DelectOldData(DeliverDataSetting.Current.AdressIP, taskDistributeId);
                        if (delole == false)
                        {
                            MessageBox.Show("删除旧数据失败！", "错误");
                            return;
                        }
                        var edgebanding = _edgebandingContract.FindAll(m => m.TaskDistributeId == taskDistributeId);

                        DataTable datatable = new DataTable();
                        datatable.Columns.Add(new DataColumn("ID", typeof(long)));
                        datatable.Columns.Add(new DataColumn("BarCode", typeof(string)));
                        datatable.Columns.Add(new DataColumn("Description", typeof(string)));
                        datatable.Columns.Add(new DataColumn("BatchName", typeof(string)));
                        datatable.Columns.Add(new DataColumn("OrderNumber", typeof(string)));
                        datatable.Columns.Add(new DataColumn("TaskId", typeof(string)));
                        datatable.Columns.Add(new DataColumn("TaskDistributeId", typeof(string)));
                        datatable.Columns.Add(new DataColumn("CreatedTime", typeof(DateTime)));
                        datatable.Columns.Add(new DataColumn("UpdatedTime", typeof(DateTime)));
                        datatable.Columns.Add(new DataColumn("Width", typeof(int)));
                        datatable.Columns.Add(new DataColumn("Length", typeof(int)));
                        datatable.Columns.Add(new DataColumn("Thickness", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L1_OFFCUT", typeof(float)));
                        datatable.Columns.Add(new DataColumn("L1_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L1_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_GROOVE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L2_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_OFFCUT", typeof(float)));
                        datatable.Columns.Add(new DataColumn("C1_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("C1_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_GROOVE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("C2_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_EDGECODE", typeof(string)));

                        foreach (var item in edgebanding)
                        {
                            DataRow dr = datatable.NewRow();
                            dr["ID"] = item.Id;
                            dr["BarCode"] = item.BarCode;
                            dr["Description"] = item.Description;
                            dr["BatchName"] = item.BatchName;
                            dr["OrderNumber"] = item.OrderNumber;
                            dr["TaskId"] = item.TaskId;
                            dr["TaskDistributeId"] = item.TaskDistributeId;
                            dr["CreatedTime"] = item.CreatedTime;
                            dr["UpdatedTime"] = item.UpdatedTime;
                            dr["Width"] = item.Width;
                            dr["Length"] = item.Length;
                            dr["Thickness"] = item.Thickness;
                            dr["L1_OFFCUT"] = item.L1_OFFCUT;
                            dr["L1_FORMAT"] = item.L1_FORMAT;
                            dr["L1_EDGE"] = item.L1_EDGE;
                            dr["L1_CORNER"] = item.L1_CORNER;
                            dr["L1_GROOVE"] = item.L1_GROOVE;
                            dr["L1_EDGECODE"] = item.L1_EDGECODE;
                            dr["L2_FORMAT"] = item.L2_FORMAT;
                            dr["L2_EDGE"] = item.L2_EDGE;
                            dr["L2_CORNER"] = item.L2_CORNER;
                            dr["L2_EDGECODE"] = item.L2_EDGECODE;
                            dr["C1_OFFCUT"] = item.C1_OFFCUT;
                            dr["C1_FORMAT"] = item.C1_FORMAT;
                            dr["C1_EDGE"] = item.C1_EDGE;
                            dr["C1_CORNER"] = item.C1_CORNER;
                            dr["C1_EDGECODE"] = item.C1_EDGECODE;
                            dr["C1_GROOVE"] = item.C1_GROOVE;
                            dr["C2_FORMAT"] = item.C2_FORMAT;
                            dr["C2_EDGE"] = item.C2_EDGE;
                            dr["C2_CORNER"] = item.C2_CORNER;
                            dr["C2_EDGECODE"] = item.C2_EDGECODE;
                        }

                        NJIS.Dapper.Repositories.Extensions.TableSchema.BulkInsert(DeliverDataSetting.Current.SqlConnectIP,"Edgebanding", datatable);
                        //foreach (var item in edgebanding)
                        //{
                        //    edgebandingsql.Append("(" + item.ToString() + "),");
                        //}
                        //var edgebandingsqlinsert = _edgebandingContract.Insert(DeliverDataSetting.Current.AdressIP, edgebandingsql);
                    }
                    else
                    {
                        MessageBox.Show("没有选择批次！", "错误");
                    }
                }
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count == 0)
            {
                MessageBox.Show("无数据可下发", "提示");
                return;
            }
            if (MessageBox.Show("确认下发数据？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = 0; i < radGridView1.Rows.Count; i++)
                {
                    if (radGridView1.Rows[i].IsSelected == true)
                    {
                        string taskDistributeId = radGridView1.Rows[i].Cells[1].Value.ToString(); ;
                        StringBuilder edgebandingsql = new StringBuilder("values");
                        var delole = _edgebandingContract.DelectOldData(DeliverDataSetting.Current.AdressIP2, taskDistributeId);
                        if (delole == false)
                        {
                            MessageBox.Show("删除旧数据失败！", "错误");
                            return;
                        }
                        var edgebanding = _edgebandingContract.FindAll(m => m.TaskDistributeId == taskDistributeId);
                        DataTable datatable = new DataTable();
                        datatable.Columns.Add(new DataColumn("ID", typeof(long)));
                        datatable.Columns.Add(new DataColumn("BarCode", typeof(string)));
                        datatable.Columns.Add(new DataColumn("Description", typeof(string)));
                        datatable.Columns.Add(new DataColumn("BatchName", typeof(string)));
                        datatable.Columns.Add(new DataColumn("OrderNumber", typeof(string)));
                        datatable.Columns.Add(new DataColumn("TaskId", typeof(string)));
                        datatable.Columns.Add(new DataColumn("TaskDistributeId", typeof(string)));
                        datatable.Columns.Add(new DataColumn("CreatedTime", typeof(DateTime)));
                        datatable.Columns.Add(new DataColumn("UpdatedTime", typeof(DateTime)));
                        datatable.Columns.Add(new DataColumn("Width", typeof(int)));
                        datatable.Columns.Add(new DataColumn("Length", typeof(int)));
                        datatable.Columns.Add(new DataColumn("Thickness", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L1_OFFCUT", typeof(float)));
                        datatable.Columns.Add(new DataColumn("L1_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L1_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_GROOVE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L1_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("L2_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("L2_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_OFFCUT", typeof(float)));
                        datatable.Columns.Add(new DataColumn("C1_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("C1_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_EDGECODE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C1_GROOVE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_FORMAT", typeof(int)));
                        datatable.Columns.Add(new DataColumn("C2_EDGE", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_CORNER", typeof(string)));
                        datatable.Columns.Add(new DataColumn("C2_EDGECODE", typeof(string)));

                        foreach (var item in edgebanding)
                        {
                            DataRow dr = datatable.NewRow();
                            dr["ID"] = item.Id;
                            dr["BarCode"] = item.BarCode;
                            dr["Description"] = item.Description;
                            dr["BatchName"] = item.BatchName;
                            dr["OrderNumber"] = item.OrderNumber;
                            dr["TaskId"] = item.TaskId;
                            dr["TaskDistributeId"] = item.TaskDistributeId;
                            dr["CreatedTime"] = item.CreatedTime;
                            dr["UpdatedTime"] = item.UpdatedTime;
                            dr["Width"] = item.Width;
                            dr["Length"] = item.Length;
                            dr["Thickness"] = item.Thickness;
                            dr["L1_OFFCUT"] = item.L1_OFFCUT;
                            dr["L1_FORMAT"] = item.L1_FORMAT;
                            dr["L1_EDGE"] = item.L1_EDGE;
                            dr["L1_CORNER"] = item.L1_CORNER;
                            dr["L1_GROOVE"] = item.L1_GROOVE;
                            dr["L1_EDGECODE"] = item.L1_EDGECODE;
                            dr["L2_FORMAT"] = item.L2_FORMAT;
                            dr["L2_EDGE"] = item.L2_EDGE;
                            dr["L2_CORNER"] = item.L2_CORNER;
                            dr["L2_EDGECODE"] = item.L2_EDGECODE;
                            dr["C1_OFFCUT"] = item.C1_OFFCUT;
                            dr["C1_FORMAT"] = item.C1_FORMAT;
                            dr["C1_EDGE"] = item.C1_EDGE;
                            dr["C1_CORNER"] = item.C1_CORNER;
                            dr["C1_EDGECODE"] = item.C1_EDGECODE;
                            dr["C1_GROOVE"] = item.C1_GROOVE;
                            dr["C2_FORMAT"] = item.C2_FORMAT;
                            dr["C2_EDGE"] = item.C2_EDGE;
                            dr["C2_CORNER"] = item.C2_CORNER;
                            dr["C2_EDGECODE"] = item.C2_EDGECODE;
                        }

                        NJIS.Dapper.Repositories.Extensions.TableSchema.BulkInsert(DeliverDataSetting.Current.SqlConnectIP2, "Edgebanding", datatable);
                        //foreach (var item in edgebanding)
                        //{
                        //    edgebandingsql.Append("(" + item.ToString() + "),");
                        //}
                        //var edgebandingsqlinsert = _edgebandingContract.Insert(DeliverDataSetting.Current.AdressIP2, edgebandingsql);
                    }
                    else
                    {
                        MessageBox.Show("没有选择批次", "错误!");
                    }
                }
            }
        }
    }    
}
