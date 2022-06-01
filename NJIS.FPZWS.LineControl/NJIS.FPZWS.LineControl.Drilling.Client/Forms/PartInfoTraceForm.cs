//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：SearchForm.cs
//   创建时间：2018-11-28 15:08
//   作    者：
//   说    明：
//   修改时间：2018-11-28 15:08
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class PartInfoTraceForm : DrillingForm
    {
        public PartInfoTraceForm()
        {
            InitializeComponent();
        }

        private WaitForm _waitForm = null;
        private void rbtnSearch_Click(object sender, System.EventArgs e)
        {
            // 显示等待窗口
            _waitForm = WaitForm.ShowWait(this);
            MqttManager.Current.Publish(EmqttSetting.Current.PcsPartInfoPositionReq, rtxtUpi.Text.Trim());
        }

        private void SearchForm_Load(object sender, System.EventArgs e)
        {
            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsPartInfoPositionRep, new MessageHandler<List<PartInfoPositionArgs>>(
                (s, datas) =>
                {
                    rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.Rows.Clear();
                        foreach (var partInfoPositionArgse in datas)
                        {
                            var dgr = new GridViewDataRowInfo(rgvMain.MasterView);
                            dgr.Cells["Time"].Value = partInfoPositionArgse.Time;
                            dgr.Cells["PartId"].Value = partInfoPositionArgse.PartId;
                            dgr.Cells["Position"].Value = partInfoPositionArgse.Position;
                            dgr.Cells["Msg"].Value = partInfoPositionArgse.Msg;

                            rgvMain.Rows.Add(dgr);
                        }

                        if (_waitForm != null)
                        {
                            _waitForm.CloseForm();
                            _waitForm = null;
                        }
                    });
                }));
        }
    }
}