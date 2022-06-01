using System;
using System.Collections.Generic;

using System.Windows.Forms;
using ArithmeticsTest.Presenters;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls.UI;

namespace ArithmeticsTest.Views
{
    public partial class SimulatorView : UserControl, IView
    {

        private SimulatorPresenter presenter = SimulatorPresenter.GetInstance();

        public SimulatorView()
        {
            InitializeComponent();
            ViewInit();
            dtpPlanDate.Value = DateTime.Today;
            this.RegisterTipsMessage();
            Register();
            this.HandleCreated += (sender, args) => this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
            
        }

        private void Register()
        {
            this.Register<SimulatorPresenter.TmpClass>(SimulatorPresenter.BindingData, (data) =>
            {
                //this.gridViewBase1.DataSource = null;
                //this.gridViewBase1.DataSource = data.DataSet.Tables["CUTS"];
                if(data.DeviceName != DeviceName) return;
                txtCurrent.Text = data.CurrentIndex.ToString();
                txtColor.Text = data.Color;
                txtBatchName.Text = data.BatchName;
            });

            this.Register<List<SimulatorPresenter.BatchStop>>(SimulatorPresenter.BindingData, data =>
            {
                var t = data.FindAll(item => item.DeviceName == DeviceName);
                gridViewBase1.DataSource = null;
                gridViewBase1.DataSource = t;
                btnSearch.Enabled = true;
            });
            this.Register<bool>(SimulatorPresenter.BindingData, data =>
            {
                if (data)
                {
                    btnDis.Text = btnDis.Text == "调" ? "不调" : "调";
                }
            });
        }

        private void ViewInit()
        {
            cmbSpeed.DataSource = new int[] { 50, 45, 40, 35, 30, 25, 20, 15, 10,};

            SimulatorPresenter.BatchStop bs;
            gridViewBase1.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+70)
                    {HeaderText = "批次", FieldName = nameof(bs.BatchName), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "设备", FieldName = nameof(bs.DeviceName), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "锯切图数", FieldName = nameof(bs.PatternCount), DataType = typeof(int), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "停止时间（分）", FieldName = nameof(bs.StopMin), DataType = typeof(int), ReadOnly = false},

            });
        }


        public string DeviceName
        {
            get => txtDeviceName.Text;
            set => txtDeviceName.Text = value;
        }

        private List<SimulatorPresenter.BatchStop> GetStopSetting()
        {
            List<SimulatorPresenter.BatchStop> stops = new List<SimulatorPresenter.BatchStop>();
            foreach (var row in gridViewBase1.ChangeRows.CurrEidtedRows)
            {
                var stop = row.DataBoundItem as SimulatorPresenter.BatchStop;
                if (stop != null)
                {
                    stops.Add(stop);
                }
            }

            return stops;
        }
        

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "开始")
            {
                var stops = GetStopSetting();
                if (stops.Count > 0)
                {
                    this.Send(SimulatorPresenter.Begin, stops);
                }
                this.Send(SimulatorPresenter.Begin, DeviceName);

                btnBegin.Text = "停止";
            }
            else
            {
                this.Send(SimulatorPresenter.Stop, DeviceName);
                btnBegin.Text = "开始";
            }

            
            //this.btnBegin.Enabled = false;
            //this.btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Send(SimulatorPresenter.Stop, DeviceName);
            //this.btnBegin.Enabled = true;
            //this.btnStop.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = dtpPlanDate.Value.Date;
            this.Send(SimulatorPresenter.GetData, planDate);
            btnSearch.Enabled = false;
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
            if (btnDis.Text == "调")
            {
                var speed = int.Parse(cmbSpeed.SelectedValue.ToString());
                this.Send(SimulatorPresenter.BeginDistribute, speed);
                //btnDis.Text = "不调";
            }
            else
            {
                this.Send(SimulatorPresenter.StopDistribute, "");
                //btnDis.Text = "调";
            }
        }
    }
}
