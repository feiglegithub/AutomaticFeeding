using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls.UI;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class MainForm : RadForm,IView
    {
        
        private readonly MainFormPresenter _presenter = MainFormPresenter.GetInstance();


        public const string ConnectResult = nameof(ConnectResult);

        public const string CloseResult = nameof(CloseResult);

        public MainForm()
        {
            InitializeComponent();
            this.RegisterTipsMessage();

            //readPlcControl2.DefaultType = 1;
            Register();
            this.BindingPresenter(_presenter);
            this.Closing += MainForm_Closing;
            this.Closed += (sender, args) => this.UnBindingPresenter();
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            foreach (var control in flowLayoutPanel4.Controls)
            {
                if (control is IStop iStop)
                {
                    iStop.Stop();
                }
            }
            this.Send(MainFormPresenter.Close, "");
        }

        private void Register()
        {
            
            this.Register<bool>(ConnectResult, ExecuteConnectResult);
            this.Register<bool>(CloseResult, ExecuteCloseResult);
        }

        private void ExecuteCloseResult(bool result)
        {
            btnConnect.Enabled = result;
            btnClose.Enabled = !result;
            if (result)
            {
                flowLayoutPanel4.Enabled = false;
            }
            //radGroupBox2.Enabled = radGroupBox1.Enabled = btnClose.Enabled = !result;
        }

        private void ExecuteConnectResult(bool result)
        {
            btnConnect.Enabled = !result;
            btnClose.Enabled = result;
            flowLayoutPanel4.Enabled = result;
            //radGroupBox2.Enabled = radGroupBox1.Enabled = btnClose.Enabled = result;
            if (result)
            {
                //flowLayoutPanel4.Enabled = true;
                //this.Send(MainFormPresenter.BeginReadPlc,"");
            }
            
        }

        private void Tips(string msg)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(this, msg)));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                Tips("请输入ip！");
            }

            //this.radGroupBox1.Enabled = false;
            //this.radGroupBox2.Enabled = false;
            this.Send(MainFormPresenter.Connect, ip);
            btnConnect.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            foreach (var control in flowLayoutPanel4.Controls)
            {
                if (control is IStop iStop)
                {
                    iStop.Stop();
                }
            }
            this.Send(MainFormPresenter.Close, "");
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
