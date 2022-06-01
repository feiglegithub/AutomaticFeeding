//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：MsgForm.cs
//   创建时间：2018-11-28 11:26
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class MsgForm : DrillingForm
    {
        public MsgForm()
        {
            InitializeComponent();
        }

        private void MsgForm_Load(object sender, System.EventArgs e)
        {

            rgvMain.ShowRowNumber();

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsMsg, new MessageHandler<MsgArgs>(
                (s, command) =>
                {
                    this.rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(DrillingSetting.Current.MsgMaxRecordCount, true);
                        GridViewRowInfo rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                        rgvMain.Rows.Insert(0, rgr);

                        rgr.Cells["CreatedTime"].Value = command.CreatedTime;
                        rgr.Cells["Message"].Value = command.Message;
                    });
                }));
            
        }
    }
}