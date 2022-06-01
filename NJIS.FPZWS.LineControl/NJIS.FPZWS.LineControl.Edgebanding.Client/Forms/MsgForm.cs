//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：MsgForm.cs
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
    public partial class MsgForm : EdgebandingForm
    {
        public MsgForm()
        {
            InitializeComponent();
        }

        private void MsgForm_Load(object sender, EventArgs e)
        {
            rgvMain.ShowRowNumber();

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsMsg, new MessageHandler<MsgArgs>(
                (s, command) =>
                {
                    rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(EdgebandingSetting.Current.MsgMaxRecordCount, true);
                        GridViewRowInfo rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                        rgvMain.Rows.Insert(0, rgr);

                        rgr.Cells["CreatedTime"].Value = command.CreatedTime;
                        rgr.Cells["Message"].Value = command.Message;
                    });
                }));
        }
    }
}