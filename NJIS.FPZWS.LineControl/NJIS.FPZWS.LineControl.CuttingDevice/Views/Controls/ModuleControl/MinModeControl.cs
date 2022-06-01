using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl
{
    public partial class MinModeControl : UserControl,IView
    {
        /// <summary>
        /// 绑定当前任务 接收数据类型
        /// </summary>
        public const string BindingCurTask = nameof(BindingCurTask);

        public const string BindingDatas = nameof(BindingDatas);
        private CurTaskDetailPresenter presenter = null;
        public MinModeControl()
        {
            InitializeComponent();
            Register();
            cmbCurStatus.DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription();
            cmbNextStatus.DataSource = PatternStatus.Cutting.GetAllFinishStatusDescription();
            cmbCurStatus.DisplayMember = "Item2";
            cmbNextStatus.DisplayMember = "Item2";
            cmbCurStatus.ValueMember = "Item1";
            cmbNextStatus.ValueMember = "Item1";
            this.RegisterTipsMessage();
            presenter = CurTaskDetailPresenter.GetInstance();
            this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void Register()
        {
            this.Register<Tuple<Pattern, List<PatternDetail>, Pattern, List<StackDetail>>>(BindingDatas, ExecuteBindingDatas);
        }

        private void ExecuteBindingDatas(Tuple<Pattern, List<PatternDetail>, Pattern, List<StackDetail>> tuple)
        {

            var cuttingTask = tuple.Item1;
            var nextTask = tuple.Item3;
            
            txtCurTask.Text = cuttingTask == null ? "" : cuttingTask.MdbName;
            txtNextTask.Text = nextTask == null ? "" : nextTask.MdbName;
            cmbCurStatus.SelectedValue =
                cuttingTask == null ? PatternStatus.Undistributed.GetHashCode() : cuttingTask.Status;
            cmbNextStatus.SelectedValue =
                nextTask == null ? PatternStatus.Undistributed.GetHashCode() : nextTask.Status;
        }


    }
}
