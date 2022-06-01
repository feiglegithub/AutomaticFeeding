//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：PartInfoQueueForm.cs
//   创建时间：2018-11-28 11:05
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class PartInfoQueueForm : DrillingForm
    {
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
            rgr.Cells["Position"].Value = command.Place;
            rgr.Cells["NextPlace"].Value = command.NextPlace;
            rgr.Cells["BatchName"].Value = command.BatchName;
            rgr.Cells["OrderNumber"].Value = command.OrderNumber;
            rgr.Cells["FinishLength"].Value = command.Length;
            rgr.Cells["FinishWidth"].Value = command.Width;
            rgr.Cells["DrillingRouting"].Value = command.DrillingRouting;
            rgr.Cells["TriggerIn"].Value = command.Plc;
            rgr.Cells["TriggerOut"].Value = command.Pcs;
            rgr.Cells["IsNg"].Value = command.IsNg; 
            rgr.Cells["Status"].Value = command.Status;
            rgr.Cells["Msg"].Value = command.PcsMessage;
        }

        private void PartInfoQueueForm_Load(object sender, System.EventArgs e)
        {
            rgvMain.ShowRowNumber();
            this.rgvMain.ContextMenuOpening += RgvMain_ContextMenuOpening;  

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsInitQueueRep, new MessageHandler<PartInfoQueueArgs>(
                (s, command) =>
                {
                    if (command == null || string.IsNullOrEmpty(command.PartId)) return;
                    this.rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.DeleteRecordRow(DrillingSetting.Current.PartInfoQueueMaxRecordCount, true);
                        AddPartQueue(command);
                    });
                }));

            //队列删除事件
            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsDeleteQueueRep, new MessageHandler<PartInfoQueueArgs>(
                (s, command) =>
                {
                    this.rgvMain.InvokeExecute(() =>
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

            MqttManager.Current.Subscribe($"{EmqttSetting.Current.PcsInitQueueRep}/{_clientId}", new MessageHandler<List<PartInfoQueueArgs>>(
                (s, qus) =>
                {
                    if (qus == null || qus.Count == 0) return;
                    this.rgvMain.InvokeExecute(() =>
                    {
                        this.rgvMain.Rows.Clear();
                        
                        foreach (var command in qus.OrderBy(m=>m.CreatedTime))
                        {
                            AddPartQueue(command);
                        }
                    });
                }));

            MqttManager.Current.Subscribe($"{EmqttSetting.Current.PcsPositionRep}", new MessageHandler<PositionArgs>(
               (s, pa) =>
               {
                   if (pa == null) return;
                   this.rgvMain.InvokeExecute(() =>
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
                           rgr.Cells["IsNg"].Value = pa.IsNg;
                           rgr.Cells["Status"].Value = pa.Status;
                           rgr.Cells["Msg"].Value = pa.PcsMessage;
                       }
                   });
               }));

            MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueReq, _clientId);
        }

        private void RgvMain_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            //e.ContextMenu.Items.Clear();
            var initMenu = new RadMenuItem() { Text = @"初始化" };
            initMenu.Click += (s, d) => { MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueReq, _clientId); };
            e.ContextMenu.Items.Add(initMenu);

            var deleteMenu = new RadMenuItem(){Text = @"删除"};
            deleteMenu.Click += (s, d) =>
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
            };
            e.ContextMenu.Items.Add(deleteMenu);
        }

        private readonly Guid _clientId = Guid.NewGuid();
    }
}