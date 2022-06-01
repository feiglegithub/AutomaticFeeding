//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：MachineUserControl.cs
//   创建时间：2018-11-28 15:34
//   作    者：
//   说    明：
//   修改时间：2018-11-28 15:34
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using Telerik.WinControls.UI.Data;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class MachineUserControl : UserControl
    {
        public MachineUserControl(MachineArgs machine)
        {
            InitializeComponent();
            Machine = machine;
            rtxtCode.DataBindings.Add(new Binding("Text", Machine, "Code"));
            rtxtSN.DataBindings.Add(new Binding("Text", Machine, "SN"));
            rtxtName.DataBindings.Add(new Binding("Text", Machine, "Name"));
            rddlStatus.DataBindings.Add(new Binding("Text", this, "StatusText"));
            rcbDouble.DataBindings.Add(new Binding("Checked", this, "IsProcessDouble"));
            rcbSignle.DataBindings.Add(new Binding("Checked", this, "IsProcessSingle"));
        }

        public MachineArgs Machine { get; }

        public bool IsProcessSingle
        {
            get
            {
                if (Machine != null)
                {
                    return Machine.IsProcessSingle > 0;
                }
                return false;
            }
            set
            {
                if (Machine != null)
                {
                    Machine.IsProcessSingle = value ? 1 : 0;
                }
            }
        }
        public bool IsProcessDouble
        {
            get
            {
                if (Machine != null)
                {
                    return Machine.IsProcessDouble > 0;
                }
                return false;
            }
            set
            {
                if (Machine != null)
                {
                    Machine.IsProcessDouble = value ? 1 : 0;
                }
            }
        }

        public string StatusText
        {
            get
            {
                if (Machine != null)
                {
                    if (Machine.Status == 0) return "启用";
                    if (Machine.Status == 1) return "禁用";
                    if (Machine.Status == 2) return "异常";
                }
                SetMachineColor();

                return "未知";
            }
        }

        public void SetMachineColor()
        {
            lock (this)
            {
                if (Machine == null) return;
                if (Machine.Status == 0)
                {
                    this.BackColor = Color.Green;
                }
                if (Machine.Status == 1)
                {
                    this.BackColor = Color.Gray;
                }
                if (Machine.Status == 2)
                {
                    this.BackColor = Color.Red;
                }
            }
        }

        private void MachineUserControl_Load(object sender, EventArgs e)
        {
            SetMachineColor();
        }

        private void RddlStatus_SelectedIndexChanged(object sender, PositionChangedEventArgs e)
        {
            Machine.Status = rddlStatus.SelectedIndex;
            Machine.IsProcessSingle = rcbSignle.Checked?1:0;
            Machine.IsProcessDouble = rcbDouble.Checked ? 1 : 0;
            MqttManager.Current.Publish(EmqttSetting.Current.PcsMachineUpdateReq, Machine);
            SetMachineColor();
        }

        private void MachineUserControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rcbDouble_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            Machine.Status = rddlStatus.SelectedIndex;
            Machine.IsProcessSingle = rcbSignle.Checked ? 1 : 0;
            Machine.IsProcessDouble = rcbDouble.Checked ? 1 : 0;
            MqttManager.Current.Publish(EmqttSetting.Current.PcsMachineUpdateReq, Machine);
        }

        private void rcbSignle_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            Machine.Status = rddlStatus.SelectedIndex;
            Machine.IsProcessSingle = rcbSignle.Checked ? 1 : 0;
            Machine.IsProcessDouble = rcbDouble.Checked ? 1 : 0;
            MqttManager.Current.Publish(EmqttSetting.Current.PcsMachineUpdateReq, Machine);
        }
    }
}