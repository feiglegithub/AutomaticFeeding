using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class CurTaskDetailControl : UserControl, IView
    {
        /// <summary>
        /// 绑定当前任务 接收数据类型
        /// </summary>
        public const string BindingCurTask = nameof(BindingCurTask);

        public const string BindingDatas = nameof(BindingDatas);

        public const string BindingDeviceInfos = nameof(BindingDeviceInfos);

        private CurTaskDetailPresenter presenter = null;//new CurTaskDetailPresenter();

        private List<CuttingTaskDetail> ofCuttingTaskDetails = null;
        private List<CuttingTaskDetail> cuttingTaskDetails = null;

        public bool IsAutoRefresh => switchAutoRefresh.Value;



        public CurTaskDetailControl()
        {
            InitializeComponent();
            
            ViewInit();
            
            gvwPatterns.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            this.HandleCreated += CurTaskDetailControl_HandleCreated;
        }

        private void CurTaskDetailControl_HandleCreated(object sender, EventArgs e)
        {
            Register();
            this.RegisterTipsMessage();
            presenter = CurTaskDetailPresenter.GetInstance();
            this.BindingPresenter(presenter);
            this.Disposed += (s, args) => this.UnBindingPresenter();
            CurTaskListenService.GetInstance().CanStart = true;
        }

        public void BeginListen()
        {
            this.Send(CurTaskDetailPresenter.Begin, "");
        }

        private void RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (cuttingTaskDetails == null || ofCuttingTaskDetails == null) return;
            if (e.Row.DataBoundItem is CuttingPattern cuttingPattern)
            {
                int PTN_INDEX = Convert.ToInt32(cuttingPattern.NewPatternName);
                gvwParts_UDI.DataSource =
                    cuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
                gvwOffCuts.DataSource =
                    ofCuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
                PartFinishedStatusView();
            }
        }

        private void ViewClear()
        {
            this.gvwMaterials.DataSource = null;
            this.gvwParts_UDI.DataSource = null;
            this.gvwPatterns.DataSource = null;
            this.gvwOffCuts.DataSource = null;
        }

        private void ViewInit()
        {

            gvwPatterns.ReadOnly = true;
            gvwOffCuts.ReadOnly = true;
            gvwParts_UDI.ReadOnly = true;
            gvwMaterials.ReadOnly = true;
            CuttingPattern cp;
            gvwPatterns.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(20){FieldName = nameof(cp.BatchName),DataType = typeof(string),HeaderText = "批次"},
                new ColumnInfo(){FieldName =CuttingServerSettings.Current.DisplayOldPTN_INDEX? nameof(cp.PatternName):nameof(cp.NewPatternName),DataType = typeof(short),HeaderText = "锯切图", ReadOnly = true},
                //new ColumnInfo(){FieldName = nameof(cp.NewPatternName),DataType = typeof(short),HeaderText = "新锯切图", ReadOnly = true},
                new ColumnInfo(){FieldName = nameof(cp.FinishedBookCount),DataType = typeof(int),HeaderText = "完成大板数"},
                new ColumnInfo(){FieldName = nameof(cp.BookCount),DataType = typeof(int),HeaderText = "切割大板数"},
                new ColumnInfo(){FieldName = nameof(cp.PartCount),DataType = typeof(short),HeaderText = "切割工件数"},
                new ColumnInfo(){FieldName = nameof(cp.FinishedPartCount),DataType = typeof(short),HeaderText = "完成工件数"},
                new ColumnInfo(){FieldName = nameof(cp.FinishedProgress),DataType = typeof(string),HeaderText = "进度"},
            });
            gvwPatterns.radGridView.ViewCellFormatting += (sender, args) =>
            {
                if (args.Column.FieldName == "FinishedProgress" && args.RowIndex > -1)
                {
                    double d = Convert.ToDouble(args.CellElement.Value);
                    args.CellElement.FormatString = d.ToString("P1");
                }


            };

            CuttingTaskDetail ctd;
            gvwParts_UDI.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){FieldName =CuttingServerSettings.Current.DisplayOldPTN_INDEX?nameof(ctd.OldPTN_INDEX): nameof(ctd.NewPTN_INDEX),DataType = typeof(int),HeaderText = "锯切图"},
                new ColumnInfo(+30){FieldName = nameof(ctd.PartId),DataType = typeof(string),HeaderText = "板件号"},
                new ColumnInfo(-10){FieldName = nameof(ctd.Length),DataType = typeof(double),HeaderText = "长"},
                new ColumnInfo(-10){FieldName = nameof(ctd.Width),DataType = typeof(double),HeaderText = "宽"},
                new ColumnInfo(-10){FieldName = nameof(ctd.PartFinishedStatus),DataType = typeof(bool),HeaderText = "完成状态"},

            });
            gvwOffCuts.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(){FieldName =CuttingServerSettings.Current.DisplayOldPTN_INDEX?nameof(ctd.OldPTN_INDEX): nameof(ctd.NewPTN_INDEX),DataType = typeof(int),HeaderText = "锯切图"},
                new ColumnInfo(+30){FieldName = nameof(ctd.PartId),DataType = typeof(string),HeaderText = "板件号"},
                new ColumnInfo(-10){FieldName = nameof(ctd.Length),DataType = typeof(double),HeaderText = "长"},
                new ColumnInfo(-10){FieldName = nameof(ctd.Width),DataType = typeof(double),HeaderText = "宽"},
                new ColumnInfo(-10){FieldName = nameof(ctd.PartFinishedStatus),DataType = typeof(bool),HeaderText = "完成状态"},
            });
            DeviceInfos deviceInfos;
            cmbDevice.ValueMember = nameof(deviceInfos.DeviceName);
            cmbDevice.DisplayMember = nameof(deviceInfos.DeviceDescription);
        }

        private void Register()
        {
            this.Register<DeviceInfos>(BindingDeviceInfos, ExecuteBindingDeviceInfo);
            this.Register<List<DeviceInfos>>(BindingDeviceInfos, (list =>
            {
                if (list == null || list.Count == 0)
                {
                    cmbDevice.Enabled = false;
                    return;
                }
                cmbDevice.DataSource = list;
                cmbDevice.Enabled = true;
            } ));
            this.Register<SpiltMDBResult>(BindingCurTask, ExecuteBindingCurTask);
            //this.Register<DataSet>(BindingDatas, ExecuteBindingDatas);
            this.Register<Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>>>(BindingDatas, ExecuteBindingDatas);
            //this.Register<List<CuttingPattern>>(BindingDatas, ExecuteBindingDatas);
        }

        private void ExecuteBindingDeviceInfo(DeviceInfos deviceInfos)
        {
            if (deviceInfos == null)
            {
                btnDeviceError.Enabled = false;
                
                return;
            }
            btnDeviceError.ForeColor = deviceInfos.State.Value == 0 ? Color.Red : Color.Black;
            btnDeviceError.Text = deviceInfos.State.Value == 0 ? "设备修复" : "设备异常";
            btnDeviceError.Enabled = true;


        }

        private void ExecuteBindingCurTask(SpiltMDBResult result)
        {
            if (result != null)
            {
                txtCurTask.Text = result.ItemName;
                txtBatchName.Text = result.BatchName;
            }
            else
            {
                txtCurTask.Text = "";
                txtBatchName.Text = "";
                ViewClear();
            }
        }

        private void ExecuteBindingDatas(List<CuttingPattern> cuttingPatterns)
        {
            this.gvwPatterns.DataSource = cuttingPatterns;
        }

        private void ExecuteBindingDatas(DataSet data)
        {
            this.gvwMaterials.DataSource = data.Tables["MATERIALS"];
        }
        private void ExecuteBindingDatas(Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>> tuple)
        {
            if (!IsAutoRefresh) return;
            gvwPatterns.DataSource = tuple.Item1;
            gvwMaterials.DataSource = tuple.Item2.Tables["MATERIALS"];
            ofCuttingTaskDetails = tuple.Item4;
            cuttingTaskDetails = tuple.Item3;

            CuttingPattern curTask = GetCurTask(tuple.Item1);
            CuttingPattern nextTask = GetNextTask(tuple.Item1);
            var errorTasks = GetErrorTasks(tuple.Item1);

            lbNextCuttingPattern.Text = nextTask == null ? "无" :
                (CuttingServerSettings.Current.DisplayOldPTN_INDEX ? nextTask.PatternName.ToString() : nextTask.NewPatternName.ToString());
            lbCurCuttingPattern.Text = curTask == null ? "无" :
                (CuttingServerSettings.Current.DisplayOldPTN_INDEX ? curTask.PatternName.ToString() : curTask.NewPatternName.ToString());

            var finishedTasks = tuple.Item1.FindAll(item => item.FinishedProgress >= 1);
            if (curTask != null)
            {
                foreach (GridViewRowInfo rowInfo in gvwPatterns.Rows)
                {
                    if (rowInfo.DataBoundItem is CuttingPattern cuttingPattern)
                    {
                        //标记异常锯切图
                        if (errorTasks.Exists(item => item.PatternName == cuttingPattern.PatternName))
                        {
                            foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                            {
                                rowInfoCell.Style.ForeColor = Color.Red;
                            }

                            continue;
                        }

                        if (finishedTasks.Exists(item => item.PatternName == cuttingPattern.PatternName))
                        {
                            foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                            {
                                rowInfoCell.Style.ForeColor = Color.Green;
                            }
                        }

                        if (cuttingPattern.PatternName == curTask.PatternName)
                        {
                            rowInfo.IsCurrent = rowInfo.IsSelected = true;

                        }
                    }
                }

                gvwParts_UDI.DataSource = cuttingTaskDetails.FindAll(item => item.OldPTN_INDEX == curTask.PatternName)
                    .ToList();
                gvwOffCuts.DataSource = ofCuttingTaskDetails.FindAll(item => item.OldPTN_INDEX == curTask.PatternName)
                    .ToList();
            }

            PartFinishedStatusView();
        }

        private void PartFinishedStatusView()
        {
            foreach (var rowInfo in gvwParts_UDI.Rows)
            {
                if (rowInfo.DataBoundItem is CuttingTaskDetail cuttingTaskDetail)
                {
                    if (cuttingTaskDetail.PartFinishedStatus)
                    {
                        foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Green;
                        }
                    }
                }
            }

            foreach (var rowInfo in gvwOffCuts.Rows)
            {
                if (rowInfo.DataBoundItem is CuttingTaskDetail cuttingTaskDetail)
                {
                    if (cuttingTaskDetail.PartFinishedStatus)
                    {
                        foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Green;
                        }
                    }
                }
            }

        }

        private CuttingPattern GetCurTask(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>().Concat(cuttingPatterns).ToList();

            if (IsBegin(tmpList))
            {
                var errorTasks = GetErrorTasks(tmpList);
                tmpList.RemoveAll(item => errorTasks.Contains(item));
                tmpList.RemoveAll(item => item.FinishedProgress >= 1);
                if (tmpList.Count == 0) return null;
                if (tmpList.Exists(item => item.FinishedProgress > 0 && item.FinishedProgress < 1))
                {
                    return tmpList.Find(item => item.FinishedProgress > 0 && item.FinishedProgress < 1);
                }
                tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                return tmpList[0];

            }

            return cuttingPatterns[0];
        }

        private CuttingPattern GetNextTask(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>().Concat(cuttingPatterns).ToList();
            if (IsBegin(tmpList))
            {
                var errorTasks = GetErrorTasks(tmpList);
                tmpList.RemoveAll(item => errorTasks.Contains(item));
                tmpList.RemoveAll(item => item.FinishedProgress >= 1);
                if (tmpList.Count == 0) return null;
                if (tmpList.Exists(item => item.FinishedProgress > 0 && item.FinishedProgress < 1))
                {
                    tmpList.RemoveAll(item => item.FinishedProgress > 0 && item.FinishedProgress < 1);
                    tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                }
                else
                {
                    tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                    tmpList.RemoveAt(0);
                }
                return tmpList.Count > 0 ? tmpList[0] : null;
            }

            return cuttingPatterns.Count > 1 ? cuttingPatterns[1] : null;
        }

        /// <summary>
        /// 获取异常锯切图任务
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private List<CuttingPattern> GetErrorTasks(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>(cuttingPatterns);
            if (IsBegin(cuttingPatterns))
            {
                var cuttingTasks = GetCuttingTasks(cuttingPatterns);
                tmpList.Sort((x, y) => x.UpdatedTime.CompareTo(y.UpdatedTime));
                var lastCuttingTask = tmpList.Last();
                if (lastCuttingTask.FinishedProgress > 0 && lastCuttingTask.FinishedProgress < 1)
                {
                    cuttingTasks.RemoveAll(item => item.PatternName == lastCuttingTask.PatternName);
                }

                return cuttingTasks;
            }

            return new List<CuttingPattern>();
        }

        /// <summary>
        /// 任务是否开始执行
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private bool IsBegin(List<CuttingPattern> cuttingPatterns)
        {
            return cuttingPatterns.FindAll(item => item.FinishedProgress > 0).Count > 0;
        }

        /// <summary>
        /// 检查是否有异常锯切图任务
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private bool HadErrorTask(List<CuttingPattern> cuttingPatterns)
        {
            return GetErrorTasks(cuttingPatterns).Count > 1;
        }

        /// <summary>
        /// 获取正在开料的锯切图任务
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private List<CuttingPattern> GetCuttingTasks(List<CuttingPattern> cuttingPatterns)
        {
            return cuttingPatterns.FindAll(item => item.FinishedProgress < 1 && item.FinishedProgress > 0).ToList();
        }

        private void btnPatternError_Click(object sender, EventArgs e)
        {
            DialogResult ret = RadMessageBox.Show(this, "提交后，本任务将会终止，本任务没有加工完成的工件将会被设置为NG工件，请确认是否提交？", "提示", MessageBoxButtons.YesNo);
            if (ret == DialogResult.No) return;
            string taskName = txtCurTask.Text.Trim();
            this.Send(CurTaskDetailPresenter.SetTaskError, taskName);
        }

        private void btnDeviceError_Click(object sender, EventArgs e)
        {
            bool deviceStatus = false;
            if (btnDeviceError.Text == "设备异常")
            {
                deviceStatus = false;
                DialogResult ret = RadMessageBox.Show(this, "提交后，系统将不再为本设备推送任务！请确认设备是否异常？", "提示", MessageBoxButtons.YesNo);
                if (ret == DialogResult.No) return;
            }
            else
            {
                deviceStatus = true;
                DialogResult ret = RadMessageBox.Show(this, "提交后，系统将自动为本设备推送任务！请确认设备是否已经恢复正常？", "提示", MessageBoxButtons.YesNo);
                if (ret == DialogResult.No) return;
            }
            this.Send(CurTaskDetailPresenter.SetDeviceStatus, deviceStatus);
            btnDeviceError.Enabled = false;
        }

        private void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curDeviceName = cmbDevice.SelectedValue.ToString();
            if (CuttingServerSettings.Current.CurrDeviceName == curDeviceName) return;
            gvwPatterns.DataSource = null;
            gvwMaterials.DataSource = null;
            gvwOffCuts.DataSource = null;
            gvwParts_UDI.DataSource = null;
            txtCurTask.Text = "";
            lbCurCuttingPattern.Text = "";
            lbNextCuttingPattern.Text = "";
            txtBatchName.Text = "";
            CuttingServerSettings.Current.CurrDeviceName = curDeviceName;
            CuttingServerSettings.Current.Save();
        }

        private void switchAutoRefresh_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
