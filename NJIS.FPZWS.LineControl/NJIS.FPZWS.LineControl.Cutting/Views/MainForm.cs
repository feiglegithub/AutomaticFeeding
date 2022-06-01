using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.UI.LocalServices;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using RadMessageBox = Telerik.WinControls.RadMessageBox;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm, IView
    {

        private bool isFull = true;
        private MinModeControl _minModeControl = new MinModeControl(){Location = new Point(1, 1)};
        private RadPageView pageView = null;
        private Size fullSize;
        private Size minSize = new Size(742, 58);
        public const string ErrorMessage = nameof(ErrorMessage);
        public MainForm()
        {
            InitializeComponent();
            pageView = this.radPageView1;
            fullSize = this.Size;
            this.Move += MainForm_Move;
            this.Disposed += MainForm_Disposed;
            this.Shown += SpiltForm_Shown;
            this.Closing += (sender, args) => args.Cancel = true;
            BroadcastMessage.Register<Exception>(ErrorMessage,this, ExecuteTips);
            //this.itemNamesControl1.BindingPresenter(MainFormPresenter.GetInstance());

        }

        private void ExecuteTips(Exception e)
        {
            BeginInvoke((Action) (() => RadMessageBox.Show(this, e.Message, "提示")));

        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            CuttingSetting.Current.StartPositionX = Location.X;
            CuttingSetting.Current.StartPositionY = Location.Y;
            CuttingSetting.Current.Save();
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            ListenService.Current.Stop();

        }

        private void SpiltForm_Shown(object sender, EventArgs e)
        {
            this.Location = new Point(Math.Abs(CuttingSetting.Current.StartPositionX)>1000?200:CuttingSetting.Current.StartPositionX,
                Math.Abs(CuttingSetting.Current.StartPositionY)>1000?200: CuttingSetting.Current.StartPositionY);
            ListenService.Current.Start();
            curTaskDetailControl1.BeginListen();
            this.Text = CuttingSetting.Current.CurrDeviceName;
            if (CuttingSetting.Current.DefaultMinModel)
            {
                MinModel();
            }
            else
            {
                FullModel();
            }

        }

        private void MinModel()
        {
            if (isFull)
            {
                this.Controls.Remove(pageView);
                this.Controls.Add(_minModeControl);
                this.Size = minSize;
                isFull = false;
            }
        }
        private void 迷你模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinModel();
        }

        private void FullModel()
        {
            if (!isFull)
            {
                this.Controls.Remove(_minModeControl);
                this.Controls.Add(pageView);
                this.Size = fullSize;
                isFull = true;
            }
        }

        private void 完整模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullModel();
        }

        private void 取消窗体至前ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;

        }

        private void 窗体至顶层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = true;

        }

        private void 取消自动刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.curTaskDetailControl1.IsAutoRefresh = false;
        }

        private void 启用自动刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.curTaskDetailControl1.IsAutoRefresh = true;
        }
    }
}
