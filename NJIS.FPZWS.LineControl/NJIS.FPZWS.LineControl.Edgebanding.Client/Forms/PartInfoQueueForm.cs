//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：PartInfoQueueForm.cs
//   创建时间：2018-12-13 16:44
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:44
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Edgebanding.Client.Forms
{
    public partial class PartInfoQueueForm : EdgebandingForm
    {
        private readonly Guid _clientId = Guid.NewGuid();

        public PartInfoQueueForm()
        {
            InitializeComponent();
            ShortcutKeys = Keys.F1;
        }

        private void AddPartQueue(PartInfoQueueArgs command)
        {
            GridViewRowInfo rgr = null;
            foreach (var rgvMainRow in rgvMain.Rows)
            {
                if (rgvMainRow.Cells["PartId"].Value.ToString() == command.PartId)
                {
                    rgr = rgvMainRow;
                    break;
                }
            }

            if (rgr == null)
            {
                rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                rgvMain.Rows.Insert(0, rgr);
            }

            rgr.Cells["CreatedTime"].Value = command.CreatedTime;
            rgr.Cells["PartId"].Value = command.PartId;
            rgr.Cells["BatchName"].Value = command.BatchName;
            rgr.Cells["OrderNumber"].Value = command.OrderNumber;
            rgr.Cells["FinishLength"].Value = command.Length;
            rgr.Cells["FinishWidth"].Value = command.Width;
            rgr.Cells["TriggerIn"].Value = command.Plc;
            rgr.Cells["TriggerOut"].Value = command.Pcs;
            rgr.Cells["Msg"].Value = command.PcsMessage;
            rgr.Cells["L1_FORMAT"].Value = command.L1_FORMAT;
            rgr.Cells["L1_EDGE"].Value = command.L1_EDGE;
            rgr.Cells["L1_CORNER"].Value = command.L1_CORNER;
            rgr.Cells["L1_GROOVE"].Value = command.L1_GROOVE;
            rgr.Cells["L1_EDGECODE"].Value = command.L1_EDGECODE;
            rgr.Cells["L2_FORMAT"].Value = command.L2_FORMAT;
            rgr.Cells["L2_EDGE"].Value = command.L2_EDGE;
            rgr.Cells["L2_CORNER"].Value = command.L2_CORNER;
            rgr.Cells["L2_EDGECODE"].Value = command.L2_EDGECODE;
            rgr.Cells["C1_FORMAT"].Value = command.C1_FORMAT;
            rgr.Cells["C1_EDGE"].Value = command.C1_EDGE;
            rgr.Cells["C1_CORNER"].Value = command.C1_CORNER;
            rgr.Cells["C1_EDGECODE"].Value = command.C1_EDGECODE;
            rgr.Cells["C1_GROOVE"].Value = command.C1_GROOVE;
            rgr.Cells["C2_FORMAT"].Value = command.C2_FORMAT;
            rgr.Cells["C2_EDGE"].Value = command.C2_EDGE;
            rgr.Cells["C2_CORNER"].Value = command.C2_CORNER;
            rgr.Cells["C2_EDGECODE"].Value = command.C2_EDGECODE;
        }

        private void PartInfoQueueForm_Load(object sender, EventArgs e)
        {
            rgvMain.ShowRowNumber();

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsInitQueueRep, new MessageHandler<PartInfoQueueArgs>(
                (s, command) =>
                {
                    if (command == null || string.IsNullOrEmpty(command.PartId)) return;
                    rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(EdgebandingSetting.Current.PartInfoQueueMaxRecordCount, true);
                        AddPartQueue(command);
                    });
                }));

            //队列删除事件
            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsDeleteQueueRep, new MessageHandler<PartInfoQueueArgs>(
                (s, command) =>
                {
                    rgvMain.InvokeExecute(() =>
                    {
                        GridViewRowInfo rgr = null;
                        foreach (var rgvMainRow in rgvMain.Rows)
                        {
                            if (rgvMainRow.Cells["PartId"].Value.ToString() == command.PartId)
                            {
                                rgr = rgvMainRow;
                                break;
                            }
                        }

                        if (rgr != null)
                        {
                            //rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                            rgvMain.Rows.Remove(rgr);
                        }
                    });
                }));

            MqttManager.Current.Subscribe($"{EmqttSetting.Current.PcsInitQueueRep}/{_clientId}",
                new MessageHandler<List<PartInfoQueueArgs>>(
                    (s, qus) =>
                    {
                        if (qus == null || qus.Count == 0) return;
                        rgvMain.InvokeExecute(() =>
                        {
                            rgvMain.Rows.Clear();
                            foreach (var command in qus)
                            {
                                AddPartQueue(command);
                            }
                        });
                    }));

            MqttManager.Current.Subscribe($"{EmqttSetting.Current.PcsPositionRep}", new MessageHandler<PositionArgs>(
                (s, pa) =>
                {
                    if (pa == null) return;
                    rgvMain.InvokeExecute(() =>
                    {
                        GridViewRowInfo rgr = null;
                        foreach (var rgvMainRow in rgvMain.Rows)
                        {
                            if (rgvMainRow.Cells["PartId"].Value.ToString() == pa.PartId)
                            {
                                rgr = rgvMainRow;
                                break;
                            }
                        }

                        if (rgr != null)
                        {
                            rgr.Cells["Position"].Value = pa.Place;
                            rgr.Cells["NextPlace"].Value = pa.NextPlace;
                        }
                    });
                }));

            MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueReq, _clientId);
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == RadMessageBox.Show(this, "确定要删除选择的数据吗？", "警告"))
            {
                if (rgvMain.SelectedRows.Count > 0)
                {
                    foreach (var rgvMainSelectedRow in rgvMain.SelectedRows)
                    {
                        var partId = rgvMainSelectedRow.Cells["PartId"].Value.ToString();
                        if (!string.IsNullOrEmpty(partId))
                        {
                            MqttManager.Current.Publish(EmqttSetting.Current.PcsDeleteQueueReq, partId);
                        }
                    }
                }
                else
                {
                    RadMessageBox.Show(this, "请选择要删除的数据？", "提示");
                }
            }
        }

        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueReq, _clientId);
        }
    }
}