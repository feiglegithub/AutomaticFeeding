using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl
{
    public partial class CurTaskDetailControlOld : UserControl, IView
    {
        /// <summary>
        /// 绑定当前任务 接收数据类型
        /// </summary>
        public const string BindingCurTask = nameof(BindingCurTask);

        public const string BindingDatas = nameof(BindingDatas);

        public const string BindingDeviceInfos = nameof(BindingDeviceInfos);

        private CurTaskDetailPresenter presenter = null;



        public bool IsAutoRefresh { get; set; } = true;
            

        public CurTaskDetailControlOld()
        {
            InitializeComponent();
            Register();
            ViewInit();
            this.RegisterTipsMessage();
            presenter = CurTaskDetailPresenter.GetInstance();
            this.BindingPresenter(presenter);
            //gvwPatterns.radGridView.CellDoubleClick += RadGridView_CellDoubleClick;
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        public void BeginListen()
        {
            this.Send(CurTaskDetailPresenter.Begin, "");
        }

        private void RadGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //if (cuttingTaskDetails == null || ofCuttingTaskDetails == null) return;
            //if (e.Row.DataBoundItem is CuttingPattern cuttingPattern)
            //{
            //    int PTN_INDEX = Convert.ToInt32(cuttingPattern.NewPatternName);
            //    gvwParts.DataSource =
            //        cuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
            //    gvwOffParts.DataSource =
            //        ofCuttingTaskDetails.FindAll(item => item.NewPTN_INDEX == PTN_INDEX).ToList();
            //    PartFinishedStatusView();
            //}
        }

        private void ViewClear()
        {
            this.gvwParts.DataSource = null;
            this.gvwPatterns.DataSource = null;
            this.gvwOffParts.DataSource = null;
        }

        private void ViewInit()
        {
            gvwPatterns.ReadOnly = true;
            gvwOffParts.ReadOnly = true;
            gvwParts.ReadOnly = true;
        }

        private void Register()
        {
            this.Register<DeviceInfos>(BindingDeviceInfos, ExecuteBindingDeviceInfo);
            this.Register<Tuple<Pattern, List<PatternDetail>, Pattern>>(BindingDatas, ExecuteBindingDatas);
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

        private void ExecuteBindingDatas(Tuple<Pattern, List<PatternDetail>, Pattern> tuple)
        {
            if (!IsAutoRefresh) return;
            var cuttingTask = tuple.Item1;
            var patternDetails = tuple.Item2;
            var nextTask = tuple.Item3;

            txtCurTask.Text = cuttingTask==null?"":cuttingTask.MdbName;
            txtNextTask.Text = nextTask == null ? "" : nextTask.MdbName;

            List<Pattern> cuttingPatterns = new List<Pattern>();
            if (cuttingTask != null){
                
                cuttingPatterns.Add(cuttingTask);
            }
            this.gvwPatterns.DataSource = null;
            this.gvwPatterns.DataSource = cuttingPatterns;

            this.gvwParts.DataSource = null;
            this.gvwParts.DataSource = patternDetails.FindAll(item=>!item.IsOffPart);

            this.gvwOffParts.DataSource = null;
            this.gvwOffParts.DataSource = patternDetails.FindAll(item => item.IsOffPart);

            PartFinishedStatusView();
        }

        private void PartFinishedStatusView()
        {
            foreach (var rowInfo in gvwParts.Rows)
            {
                if (rowInfo.DataBoundItem is PatternDetail patternDetail)
                {
                    if (patternDetail.IsFinished)
                    {
                        foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Green;
                        }
                    }
                }
            }

            foreach (var rowInfo in gvwOffParts.Rows)
            {
                if (rowInfo.DataBoundItem is PatternDetail patternDetail)
                {
                    if (patternDetail.IsFinished)
                    {
                        foreach (GridViewCellInfo rowInfoCell in rowInfo.Cells)
                        {
                            rowInfoCell.Style.ForeColor = Color.Green;
                        }
                    }
                }
            }

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
    }
}
