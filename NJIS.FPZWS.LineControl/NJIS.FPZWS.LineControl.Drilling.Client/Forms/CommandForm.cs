//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：CommandForm.cs
//   创建时间：2018-11-28 11:26
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using Newtonsoft.Json;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class CommandForm : DrillingForm
    {
        public CommandForm()
        {
            InitializeComponent();
            rgvTaskOfBuffer.ShowRowNumber();
        }

        private void CommandForm_Load(object sender, System.EventArgs e)
        {
            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsCommand, new MessageHandler<CommandArgs>((s, command) =>AddCommand(command)));
            //MqttManager.Current.Subscribe(EmqttSetting.Current.PcsCommandEnd, new MessageHandler<CommandArgs>((s, command) => AddCommand(command)));
        }

        private void AddCommand(CommandArgs command)
        {
            if (command == null) return;
            rgvTaskOfBuffer.InvokeExecute(() =>
            {
                rgvTaskOfBuffer.DeleteRecordRow(DrillingSetting.Current.CommandMaxRecordCount, true);
                if (command.Type == CommandType.S)
                {
                    var dataRowInfo = new GridViewDataRowInfo(rgvTaskOfBuffer.MasterView);
                    dataRowInfo.Cells[1].Value = command.CreatedTime;
                    dataRowInfo.Cells[3].Value = command.CommandCode;
                    var json = JsonConvert.SerializeObject(command.Input);
                    dataRowInfo.Cells[4].Value = json;
                    rgvTaskOfBuffer.Rows.Insert(0, dataRowInfo);
                }
                else if (command.Type == CommandType.E)
                {
                    if (rgvTaskOfBuffer.Rows.Count < 1) return;
                    rgvTaskOfBuffer.Rows[0].Cells[2].Value = command.CreatedTime;
                    var json = JsonConvert.SerializeObject(command.Output);
                    rgvTaskOfBuffer.Rows[0].Cells[5].Value = json;
                }
            });
        }
    }
}