using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using AllTask = NJIS.FPZWS.LineControl.Cutting.Model.AllTask;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class CuttingAPSControl : UserControl, IView
    {
        public const string BindingDatas = nameof(BindingDatas);
        /// <summary>
        /// 接收计算结果
        /// </summary>
        public const string BindingBatchDatas = nameof(BindingBatchDatas);
        public const string BindingCuttingDeviceInfos = nameof(BindingCuttingDeviceInfos);
        public const string SaveResult = nameof(SaveResult);
        private CuttingAPSControlPresenter _presenter = new CuttingAPSControlPresenter();

        private List<DeviceTaskInfo> _datas = null;

        private CuttingAPSSubControl _cuttingApsSubControl = null;

        private DetailForm _detailForm = null;
        //private bool _curIsAPS = false;
        public CuttingAPSControl()
        {
            InitializeComponent();
            dtpPlanDate.Value = DateTime.Today;
            //gridViewBase2.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            dtpPlanDate.ValueChanged += DtpPlanDate_ValueChanged;
            Register();
            ViewInit();
            this.RegisterTipsMessage();
            this.BindingPresenter(_presenter);

        }

        private void DtpPlanDate_ValueChanged(object sender, EventArgs e)
        {
            gridViewBase1.DataSource = null;
        }

        //private void RadGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        //{
        //    if (e.Row.DataBoundItem is DeviceTaskInfo data)
        //    {
        //        gridViewBase3.DataSource = null;
        //        //gridViewBase3.DataSource = data.Tasks;
        //        gridViewBase3.DataSource = data.Tasks;

        //    }
        //}

        private void ViewInit()
        {

            AllTask allTask;
            gridViewBase1.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){ HeaderText = "批次",FieldName = nameof(allTask.BatchName),DataType = typeof(string)},
                new ColumnInfo(){ HeaderText = "板材数",FieldName =nameof(allTask.BookNum), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "需求数",FieldName =nameof(allTask.MdbCount), DataType = typeof(int)},
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(allTask.IsError)
                    , DataType=typeof(bool), ReadOnly=true,DataSource = new Dictionary<bool,string>(){{false,"正常"},{true,"异常"}},DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){ HeaderText = "板件数",FieldName = nameof(allTask.PartCount),DataType = typeof(int)},
                //new ColumnInfo(){ HeaderText = "余板数",FieldName =nameof(allTask.OffPartCount), DataType = typeof(int)},
                new ColumnInfo(){ HeaderText = "信息",FieldName =nameof(allTask.Msg), DataType = typeof(string)},
                
                //new ColumnInfo(){ HeaderText = "备注",FieldName =nameof(allTask.Msg), DataType = typeof(string)},
            });
            gridViewBase1.ShowCheckBox = true;

            DeviceInfos di;
            gvbCuttingDevices.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(-20){FieldName = nameof(di.DepartmentId),HeaderText = "部门",DataType = typeof(int)},
                new ColumnInfo(5){FieldName = nameof(di.DeviceName),HeaderText = "设备名",DataType = typeof(string)},
                new ColumnInfo(20){FieldName = nameof(di.DeviceType),HeaderText = "设备类型",DataType = typeof(string)},
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(di.State)
                    , DataType=typeof(int), ReadOnly=true,DataSource = new Dictionary<int,string>(){{0,"禁用"},{1,"启用"}}
                    ,DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){FieldName = nameof(di.ProcessName),HeaderText = "工段",DataType = typeof(string)},
                new ColumnInfo(){ HeaderText="设备描述", FieldName=nameof(di.DeviceDescription), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){FieldName = nameof(di.Remark),HeaderText = "备注",DataType = typeof(string)},
            });
            gvbCuttingDevices.ShowCheckBox = true;
            gvbCuttingDevices.SetDefaultSelectRows(row => row.DataBoundItem is DeviceInfos ? ((DeviceInfos)row.DataBoundItem).State != 0 : false);
        }

        private void Register()
        {
            this.Register<List<AllTask>>(BindingDatas, ExecuteBindingDatas);
            this.Register<bool>(SaveResult, ExecueSaveResult);
            this.Register<List<DeviceInfos>>(BindingCuttingDeviceInfos, ExecuteBindingCuttingDeviceInfos);
            this.Register<List<DeviceTaskInfo>>(BindingBatchDatas, ExecuteBindingBatchDatas);
        }

        private void ExecueSaveResult(bool result)
        {
            flowLayoutPanel1.Enabled = true;
        }

        private void ExecuteBindingCuttingDeviceInfos(List<DeviceInfos> deviceInfoses)
        {
            gvbCuttingDevices.DataSource = deviceInfoses;
            //foreach (var row in gvbCuttingDevices.Rows)
            //{
            //    var data = row.DataBoundItem as DeviceInfos;
            //    if(data==null) continue;
            //    if (data.State != 0)
            //    {
            //        row.IsSelected = true;
            //    }
            //    else
            //    {
            //        row.IsSelected = false;
            //    }
            //}
        }

        private void ExecuteBindingBatchDatas(List<DeviceTaskInfo> datas)
        {
            this._datas = datas;
            this.btnDetail.Enabled = true;
            int totalTime = 0;
            var groups = datas.GroupBy(item => item.BatchName);
            foreach (var group in groups)
            {
                totalTime += group.Max(item => item.TotalTime);
            }
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp.AddSeconds(totalTime) - tmp;
            txtTotalTime.Text = ts.ToString();
            btnDetail_Click(this.btnDetail,null);
        }


        private void ExecuteBindingDatas(List<AllTask> datas)
        {
            gridViewBase1.DataSource = null;
            gridViewBase1.DataSource = datas;
            int totalTime = datas.Sum(item => item.TotalTime);
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp.AddSeconds(totalTime) - tmp;
            txtTotalTime.Text = ts.ToString();
            gridViewBase1.EndWait();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            gvbCuttingDevices.BeginWait();
            this.Send(CuttingAPSControlPresenter.GetAllTask, dtpPlanDate.Value.Date);
            this.Send(CuttingAPSControlPresenter.GetCuttingDeviceInfos, "");
            gridViewBase1.BeginWait();
        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (gridViewBase1.DataSource == null)
            {
                Tips("暂无数据，无法计算");
                return;
            }
            int deviceCount = gvbCuttingDevices.SelectedRows.Count;

            if (deviceCount == 0)
            {
                Tips("请选择分配的设备");
                return;
            }

            List<AllTask> allTasks = gridViewBase1.GetSelectItemsData<AllTask>();
            if (allTasks.Exists(item => item.IsError))
            {
                var task = allTasks.FirstOrDefault(item => item.IsError);
                Tips($"批次：{task?.BatchName}的数据异常，无法建垛!");
                return;
            }

            List<DeviceInfos> dis = gvbCuttingDevices.GetSelectItemsData<DeviceInfos>();
            int maxPartCount = Convert.ToInt32(txtMaxPartCount.Text.Trim());
            int maxCutCount = Convert.ToInt32(txtMaxCutCount.Text.Trim());
            txtDeviceCount.Text = deviceCount.ToString();
            btnDetail.Enabled = false;
            this.Send(CuttingAPSControlPresenter.CalculateCutTimes, new Tuple<int, List<DeviceInfos>, int, List<AllTask>>(maxPartCount, dis, maxCutCount, allTasks));


        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(msg)));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var result = RadMessageBox.Show(this, "保存后将会生成备料数据通知WMS备料，保存成功后将不可修改", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            this.Send(CuttingAPSControlPresenter.SaveSatck, "");
            flowLayoutPanel1.Enabled = false;

        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (this._datas == null)
            {
                Tips("没有可查看的计算结果！");
                return;
            }


            if (_detailForm != null)
            {

                _detailForm.Close();
            }

            _detailForm = null;
            _detailForm = new DetailForm();
            _detailForm.BindingDatas(_datas);
            _detailForm.Show();
            _detailForm.Closed += (o, args) => _detailForm = null;

        }
    }
}
