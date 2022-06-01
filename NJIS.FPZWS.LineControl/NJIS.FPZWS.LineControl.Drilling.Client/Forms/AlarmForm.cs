//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：AlarmForm.cs
//   创建时间：2018-11-28 11:25
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:25
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class AlarmForm : DrillingForm
    {
        public AlarmForm()
        {
            InitializeComponent();
        }

        private void AlarmForm_Load(object sender, System.EventArgs e)
        {
            rgvMain.ShowRowNumber();

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsAlarmRep, new MessageHandler<PcsAlarmArgs>(
                (s, command) =>
                {
                    rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(DrillingSetting.Current.AlarmMaxRecordCount, true);
                        var flag = true;
                        foreach (var dr in rgvMain.Rows)
                        {
                            if (dr.Cells["AlarmId"].Value != null && dr.Cells["AlarmId"].Value.ToString() == command.AlarmId)
                            {
                                dr.Cells["StarTime"].Value = command.StarTime;
                                dr.Cells["EndTime"].Value = command.EndTime;
                                dr.Cells["AlarmId"].Value = command.AlarmId;
                                dr.Cells["Category"].Value = command.Category;
                                dr.Cells["ParamName"].Value = command.ParamName;
                                dr.Cells["Value"].Value = command.Value;
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            GridViewRowInfo rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                            rgvMain.Rows.Insert(0, rgr);
                            rgr.Cells["StarTime"].Value = command.StarTime;
                            rgr.Cells["EndTime"].Value = command.EndTime;
                            rgr.Cells["AlarmId"].Value = command.AlarmId;
                            rgr.Cells["Category"].Value = command.Category;
                            rgr.Cells["ParamName"].Value = command.ParamName;
                            rgr.Cells["Value"].Value = command.Value;
                        }
                    });
                }));
        }

        private void tsmiDelete_Click(object sender, System.EventArgs e)
        {
            rgvMain.DeleteRecordRow(0, true);
        }
    }
}