using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Controls;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Views
{
    public partial class MachineHandView : UserControl,IView
    {
        private MachineHandPresenter presenter = new MachineHandPresenter();
        public MachineHandView()
        {
            InitializeComponent();
            Register();
            this.RegisterTipsMessage();
            this.BindingPresenter(presenter);
        }

        private List<string> ExecuteMsgs = new List<string>();
        private void Register()
        {
            this.Register<bool>(MachineHandPresenter.ConnectResult, result =>
            {
                btnConnect.Enabled = !result;
                radGroupBox1.Enabled = radGroupBox2.Enabled = radPanel1.Enabled = btnClose.Enabled = result;
                
            });
            this.Register<bool>(MachineHandPresenter.CloseResult, result =>
            {
                radGroupBox1.Enabled = radGroupBox2.Enabled = radPanel1.Enabled = btnClose.Enabled = !result;
                btnConnect.Enabled = result;
            });
            this.Register<Tuple<short, short, short>>(MachineHandPresenter.BindingData, data =>
                {
                    txtStatus.Text = data.Item1.ToString();
                    txtWrite.Text = data.Item2.ToString();
                    txtRead.Text = data.Item3.ToString();
                });
            this.Register<string>(MachineHandPresenter.ExecuteMsg, s =>
                {
                    ExecuteMsgs.Add(s);
                    listCommandResult.DataSource = null;
                    listCommandResult.DataSource = ExecuteMsgs;
                });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                RadMessageBox.Show(this, "ip地址不能为空");
                return;
            }
            
            this.Send(MachineHandPresenter.Connect, ip);
        }

        private List<MachineHandCommandControl> GetSelectCommandControls(FlowLayoutPanel flowLayoutPanel)
        {
            List<MachineHandCommandControl> list = new List<MachineHandCommandControl>();
            foreach (var control in flowLayoutPanel.Controls)
            {
                if (control is MachineHandCommandControl commandControl)
                {
                    if (commandControl.IsChecked)
                    {
                        list.Add(commandControl);
                    }
                }
            }

            return list;
        }

        private void btnAddCommand1_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel panel= null;
            panel = sender.Equals(btnAddCommand1) ? panelCommandList1 : panelCommandList2;
            var control = new MachineHandCommandControl();
            control.ExecuteButton.Click += ExecuteButton_Click;
            panel.Controls.Add(control);
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (((Control) sender).Parent is MachineHandCommandControl commandControl)
            {
                var selectItem = commandControl.SelectedItem;
                if (selectItem != null)
                {
                    if (selectItem.Value is EMachineHandCommand eCommand)
                    {
                        this.Send(MachineHandPresenter.Write, eCommand);
                    }
                    //int value = Convert.ToInt32(selectItem.Value);
                    
                }
            }
        }

        private void btnRemoveCommand1_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel panel = null;
            panel = sender.Equals(btnRemoveCommand1) ? panelCommandList1 : panelCommandList2;

            var selectCommandControl = GetSelectCommandControls(panel);
            foreach (var commandControl in selectCommandControl)
            {
                panel.Controls.Remove(commandControl);
            }
            
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(tips)));
        }

        private void btnUpCommand1_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel panel = null;
            panel = sender.Equals(btnUpCommand1) ? panelCommandList1 : panelCommandList2;
            var selectCommandControl = GetSelectCommandControls(panel);
            if (selectCommandControl.Count == 0)
            {
                Tips("请选择需要上移的命令");
                return;
            }
            if (selectCommandControl.Count > 1)
            {
                Tips("只能单个命令上移，无法批量上移");
                return;
            }

            var oldIndex = panel.Controls.GetChildIndex(selectCommandControl[0]);
            if(oldIndex==0) return;

            panel.Controls.SetChildIndex(selectCommandControl[0],oldIndex-1);
            
        }

        private void btnDownCommand1_Click(object sender, EventArgs e)
        {

            FlowLayoutPanel panel = null;
            panel = sender.Equals(btnDownCommand1) ? panelCommandList1 : panelCommandList2;
            var selectCommandControl = GetSelectCommandControls(panel);
            if (selectCommandControl.Count == 0)
            {
                Tips("请选择需要下移的命令");
                return;
            }
            if (selectCommandControl.Count > 1)
            {
                Tips("只能单个命令下移，无法批量下移");
                return;
            }

            var oldIndex = panel.Controls.GetChildIndex(selectCommandControl[0]);
            if (oldIndex == panel.Controls.Count - 1) return;
            panel.Controls.SetChildIndex(selectCommandControl[0], oldIndex + 1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Send(MachineHandPresenter.ClearLastTask,(short)0);
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Send(MachineHandPresenter.FinishedTask, (short)0);
        }

        private void btnBeginListen_Click(object sender, EventArgs e)
        {
            this.Send(MachineHandPresenter.BeginListen,"");
        }

        private void btnEndListen_Click(object sender, EventArgs e)
        {
            this.Send(MachineHandPresenter.StopListen, "");
        }

        private List<Command> GetCommands(FlowLayoutPanel panel)
        {
            List<Command> commands = new List<Command>();
            foreach (var commandControl in panel.Controls)
            {
                if (commandControl is MachineHandCommandControl command)
                {
                    var selectItem = command.SelectedItem;
                    if (selectItem != null)
                    {
                        if (selectItem.Value is EMachineHandCommand eCommand)
                        {
                            commands.Add(new Command() { CommandValue = Convert.ToInt16(eCommand), Name = selectItem.Text.ToString(),Index = panel.Controls.GetChildIndex(command)});
                        }

                    }
                }
            }

            return commands;
        }

        private void btnAuto1_Click(object sender, EventArgs e)
        {
            var comands = GetCommands(this.panelCommandList1);
            if (comands.Count == 0)
            {
                Tips("命令组不能为空");
                return;
            }
            this.Send(MachineHandPresenter.Auto,comands);
            btnAuto1.Enabled = radGroupBox1.Enabled = false;
            btnStop1.Enabled = true;
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            this.Send(MachineHandPresenter.Stop,"");
            btnAuto1.Enabled = radGroupBox1.Enabled = true;
            btnStop1.Enabled = false;
        }
    }
}
