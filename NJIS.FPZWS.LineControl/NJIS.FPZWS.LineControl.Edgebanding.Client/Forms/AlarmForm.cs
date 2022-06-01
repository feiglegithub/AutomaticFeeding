//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：AlarmForm.cs
//   创建时间：2018-12-13 16:44
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:44
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Edgebanding.Client.Forms
{
    public partial class AlarmForm : EdgebandingForm
    {
        public AlarmForm()
        {
            InitializeComponent();
        }

        private void AlarmForm_Load(object sender, EventArgs e)
        {
            rgvMain.ShowRowNumber();

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsAlarmRep, new MessageHandler<PcsAlarmArgs>(
                (s, command) =>
                {
                    rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(EdgebandingSetting.Current.AlarmMaxRecordCount, true);

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

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            rgvMain.DeleteRecordRow(0, true);

        }
    }
}