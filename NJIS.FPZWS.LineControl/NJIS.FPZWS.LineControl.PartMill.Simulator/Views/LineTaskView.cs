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
    public partial class LineTaskView : UserControl,IView
    {
        private LineTaskPresenter presenter = new LineTaskPresenter();
        private List<string> tipsList = new List<string>();
        public LineTaskView()
        {
            InitializeComponent();
            this.HandleCreated += LineTaskView_HandleCreated;
        }

        private void LineTaskView_HandleCreated(object sender, EventArgs e)
        {
            this.RegisterTipsMessage();
            this.Register<bool>(LineTaskPresenter.FinishedResult, result => { btnExecute.Enabled = true; });
            this.Register<List<line_task>>(MhTaskPresenter.BindingData, data =>
            {
                this.gridViewBase1.DataSource = null;
                this.gridViewBase1.DataSource = data;
                this.btnSearch.Enabled = true;
            });

            this.Register<string>(MhTaskPresenter.BindingData, tips =>
            {
                if(tipsList.Count==20) tipsList.Clear();
                tipsList.Add(tips);
                tipList.DataSource = null;
                tipList.DataSource = tipsList;
            });
            this.BindingPresenter(presenter);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var tasks = this.gridViewBase1.GetSelectItemsData<line_task>();
            if (tasks.Count == 0)
            {
                Tips("请选择线体任务");
                return;
            }

            this.Send(LineTaskPresenter.Execute, tasks[0]);
            btnExecute.Enabled = false;
        }


        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Send(LineTaskPresenter.GetData, "");

            this.btnSearch.Enabled = false;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            this.Send(LineTaskPresenter.AutoExecute,"");
            btnAuto.Enabled = false;
            btnStopAuto.Enabled = true;
        }

        private void btnStopAuto_Click(object sender, EventArgs e)
        {
            this.Send(LineTaskPresenter.StopAuto, "");
            btnAuto.Enabled = true;
            btnStopAuto.Enabled = false;
        }
    }
}
