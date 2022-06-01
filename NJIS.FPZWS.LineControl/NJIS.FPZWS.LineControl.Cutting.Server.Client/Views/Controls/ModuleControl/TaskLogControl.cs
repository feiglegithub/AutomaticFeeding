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
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    public partial class TaskLogControl : UserControl,IView
    {
        public const string BindingDatas = nameof(BindingDatas);
        private readonly TaskLogControlPresenter _presenter = new TaskLogControlPresenter();
        public TaskLogControl()
        {
            InitializeComponent();
            ViewInit();
            this.dtpPlanDate.Value =DateTime.Now;
            this.Register<List<CuttingTaskLog>>(BindingDatas, ExecuteBindingDatas);
            this.BindingPresenter(_presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void ViewInit()
        {
            List<Tuple<int, string>> tuples = FinishedStatus.Cut.GetAllFinishStatusDescription();
            //new List<Tuple<int, string>>()
            //{
            //    FinishedStatus.Cut.GetFinishStatusDescription(),
            //    FinishedStatus.Cutting.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLost.GetFinishStatusDescription(),
            //    FinishedStatus.LoadedMaterial.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLoaded.GetFinishStatusDescription(),
            //    FinishedStatus.CreatingSaw.GetFinishStatusDescription(),
            //    FinishedStatus.CreatedSaw.GetFinishStatusDescription(),
            //    FinishedStatus.MdbLoading.GetFinishStatusDescription(),
            //    FinishedStatus.MdbUnloaded.GetFinishStatusDescription(),
            //    FinishedStatus.WaitMaterial.GetFinishStatusDescription(),
            //    FinishedStatus.LoadingMaterial.GetFinishStatusDescription(),

            //};

            gridViewBase1.AddColumns(new List<UI.Common.Controls.ColumnInfo>()
            {
                new UI.Common.Controls.ColumnInfo(70){ HeaderText="任务号", FieldName="TaskDistributeId", DataType=typeof(Guid), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="批次号", FieldName="BatchName", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-10){ HeaderText="生产日期", FieldName="PlanDate", DataType=typeof(DateTime), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(){ HeaderText="堆垛号", FieldName="ItemName", DataType=typeof(string), ReadOnly=true },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="设备切换", FieldName="IsChangedDevice", DataType=typeof(bool), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-30){ HeaderText="操作结果", FieldName="IsSuccess", DataType=typeof(string), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-10){ HeaderText="操作时间", FieldName="CreatedTime", DataType=typeof(DateTime), ReadOnly=true },
                new UI.Common.Controls.ColumnInfo(-40){ HeaderText="操作信息", FieldName="Msg", DataType=typeof(string), ReadOnly=true },

            });
            Telerik.WinControls.UI.GridViewComboBoxColumn cmBoxColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
            {
                DataSource = tuples,
                DisplayMember = "Item2",
                ValueMember = "Item1",
                HeaderText = "状态",
                FieldName = "FinishedStatus",
                DataType = typeof(int),
                Width = 70,
                ReadOnly = true
            };
            
            gridViewBase1.AddColumns(cmBoxColumn);

            cmBoxColumn = new Telerik.WinControls.UI.GridViewComboBoxColumn()
            {
                DataSource = new List<Tuple<bool, string>>() { new Tuple<bool, string>(true,"是"),new Tuple<bool, string>(false,"否")},
                DisplayMember = "Item2",
                ValueMember = "Item1",
                HeaderText = "设备切换",
                FieldName = "IsChangedDevice",
                DataType = typeof(bool),
                Width = 60,
                ReadOnly = true
            };
            
            gridViewBase1.AddColumns(cmBoxColumn);
        }

        private void ExecuteBindingDatas(List<CuttingTaskLog> cuttingTaskLogs)
        {
            gridViewBase1.DataSource = cuttingTaskLogs;
            gridViewBase1.EndWait();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Send(TaskLogControlPresenter.GetTaskLog, dtpPlanDate.Value.Date);
            this.gridViewBase1.BeginWait();
        }
    }
}
