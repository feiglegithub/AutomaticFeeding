//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：MachineForm.cs
//   创建时间：2018-11-28 11:05
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class MachineForm : DrillingForm
    {
        public MachineForm()
        {
            InitializeComponent();
        }

        public List<MachineUserControl> MUCS = new List<MachineUserControl>();

        private void MachineForm_Load(object sender, System.EventArgs e)
        {
            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsMachineInitRep, new MessageHandler<List<MachineArgs>>(
                (s, datas) =>
                {
                    foreach (var data in datas)
                    {
                        var machine = MUCS.FirstOrDefault(m => m.Machine.Code == data.Code);
                        if (machine == null)
                        {
                            this.InvokeExecute(() =>
                            {
                                machine = new MachineUserControl(data) {};
                                this.flowLayoutPanel1.Controls.Add(machine);
                                this.flowLayoutPanel1.Show();
                                MUCS.Add(machine);
                            });
                        }
                        else
                        {
                            machine.Machine.Code = data.Code;
                            machine.Machine.Name = data.Name;
                            machine.Machine.SN = data.SN;
                            machine.Machine.IsProcessDouble = data.IsProcessDouble;
                            machine.Machine.IsProcessSingle = data.IsProcessSingle;
                            machine.Machine.Status = data.Status;
                            machine.SetMachineColor();
                        }
                    }
                }));
            MqttManager.Current.Publish(EmqttSetting.Current.PcsMachineInitReq, "");
        }
    }
}