using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.PartMill.Model;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Views
{
    public partial class MachineHandTaskView : UserControl,IView
    {
        private MhTaskPresenter presenter =new MhTaskPresenter();
        private List<string> tipsList=new List<string>();

        public MachineHandTaskView()
        {
            InitializeComponent();
            this.HandleCreated += MachineHandTaskView_HandleCreated;
        }

        private void MachineHandTaskView_HandleCreated(object sender, EventArgs e)
        {
            this.RegisterTipsMessage();
            this.Register<bool>(MhTaskPresenter.FinishedResult, ret => { btnExecute.Enabled = true; });
            this.Register<List<mh_task>>(MhTaskPresenter.BindingData, data =>
            {
                this.gridViewBase1.DataSource = null;
                this.gridViewBase1.DataSource = data;
                this.btnSearch.Enabled = true;
            });

            this.Register<string>(MhTaskPresenter.BindingData, tips =>
            {
                tipsList.Add(tips);
                tipList.DataSource = null;
                tipList.DataSource = tipsList;
            });

            this.BindingPresenter(presenter);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var tasks =this.gridViewBase1.GetSelectItemsData<mh_task>();
            if (tasks.Count == 0)
            {
                Tips("请选择机械手任务");
                return;
            }

            this.Send(MhTaskPresenter.Execute, tasks[0]);
            btnExecute.Enabled = false;
        }


        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Send(MhTaskPresenter.GetData,"");

            this.btnSearch.Enabled = false;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            this.Send(MhTaskPresenter.Auto,"");
            btnAuto.Enabled = false;
            btnStopAuto.Enabled = true;
        }

        private void btnStopAuto_Click(object sender, EventArgs e)
        {
            this.Send(MhTaskPresenter.StopAuto, "");
            btnAuto.Enabled = true;
            btnStopAuto.Enabled = false;
        }
    }
}
