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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class NgForm : DrillingForm
    {
        public BindingList<PcsNgArgs> _datas = new BindingList<PcsNgArgs>();
        public NgForm()
        {
            InitializeComponent();
            rgvMain.RowsChanged += RgbMain_RowsChanged;
            rgvMain.UserDeletingRow += rgbMain_UserDeletingRow;
            rgvMain.UserDeletedRow += rgbMain_UserDeletedRow;
            rgvMain.UserAddingRow += RgbMain_UserAddingRow;
            rgvMain.KeyDown += RgbMain_KeyDown;
            rgvMain.ShowRowNumber();
        }
        private void RgbMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && char.IsLetter((char)e.KeyCode) && e.KeyCode == Keys.V)
            {
                var iData = Clipboard.GetDataObject();
                if (iData != null && iData.GetDataPresent(DataFormats.Text))
                {
                    var buffers = (string)iData.GetData(DataFormats.Text);
                    var splits = buffers.Split(new []{'\n','\r'});
                    foreach (var split in splits)
                    {
                        try
                        {
                            var rst = split.Split('\t');
                            if (rst[0].Length == 13)
                            {
                                var ngEntity = new PcsNgArgs
                                {
                                    PartId = rst[0],
                                    Status = 0,
                                    CreatedTime = DateTime.Now,
                                    UpdatedTime = DateTime.Now,
                                    Msg = "NG登记成功！"
                                };
                                _datas.Add(ngEntity); 
                                MqttManager.Current.Publish(EmqttSetting.Current.PcsNgReq, ngEntity);
                            }
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                }
            }
        }

        private void RgbMain_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {

            try
            {
                var spr = new PcsNgArgs();
                spr.PartId = e.Rows[0].Cells["PartId"].Value.ToString();
                spr.Status = 0;
                spr.CreatedTime=DateTime.Now;
                spr.UpdatedTime=DateTime.Now;
                
                _datas.Add(spr);

                MqttManager.Current.Publish(EmqttSetting.Current.PcsNgReq, new PcsNgArgs()
                {
                    PartId = spr.PartId,
                    CreatedTime = spr.CreatedTime,
                    UpdatedTime = spr.UpdatedTime,
                    Status = spr.Status,
                    Msg = "NG登记成功！"
                });
            }
            catch (Exception exception)
            {
                RadMessageBox.Show(this, exception.Message, "数据格式不正确");
                e.Cancel = true;
            }
        }

        private void RgbMain_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            var lst = new List<PcsNgArgs>();
            foreach (var rgbMainSelectedRow in rgvMain.SelectedRows)
            {
                var entity = rgbMainSelectedRow.DataBoundItem as PcsNgArgs;
                if (entity != null)
                {
                    lst.Add(entity);
                }
            }

            //foreach (var sysPackingRule in lst)
            //{
            //    _contract.UpdateSysPackingParameter(sysPackingRule);
            //}
        }
        private void rgbMain_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (System.Windows.Forms.DialogResult.OK != RadMessageBox.Show(this, "确定删除选择的数据吗？", "提示",
                    System.Windows.Forms.MessageBoxButtons.OKCancel))
            {
                e.Cancel = true;
            }
            else
            {
                var lst = new List<PcsNgArgs>();
                foreach (var rgbMainSelectedRow in e.Rows)
                {
                    var entity = rgbMainSelectedRow.DataBoundItem as PcsNgArgs;
                    if (entity != null)
                    {
                        lst.Add(entity);
                    }
                }
                WaitForm.ShowWait(this, () =>
                {
                    foreach (var sysPackingParameter in lst)
                    {
                        MqttManager.Current.Publish(EmqttSetting.Current.DeletePcsNgReq, new PcsNgArgs()
                        {
                            PartId = sysPackingParameter.PartId,
                        });
                    }
                });
            }
        }

        private void rgbMain_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                var spr = new PcsNgArgs();
                spr.PartId = e.Row.Cells["PartId"].Value.ToString();
                spr.Status = 0;
                spr.CreatedTime = DateTime.Now;
                spr.UpdatedTime = DateTime.Now;

                _datas.Add(spr);

                MqttManager.Current.Publish(EmqttSetting.Current.PcsNgReq, new PcsNgArgs()
                {
                    PartId = spr.PartId,
                    CreatedTime = spr.CreatedTime,
                    UpdatedTime = spr.UpdatedTime,
                    Status = spr.Status,
                    Msg = "NG登记成功！"
                });
            }
            catch (Exception exception)
            {

            }
        }

        private void MsgForm_Load(object sender, System.EventArgs e)
        {
            rgvMain.ShowRowNumber();
            rgvMain.DataBind(_datas);

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsNgRep, new MessageHandler<List<PcsNgArgs>>(
                (s, datas) =>
                {
                    this.rgvMain.InvokeExecute(() =>
                    {
                        rgvMain.Rows.Clear();
                        foreach (var command in datas)
                        {
                            _datas.Add(command);
                            //GridViewRowInfo rgr = new GridViewDataRowInfo(rgvMain.MasterView);
                            //rgvMain.Rows.Insert(0, rgr);
                            //rgr.Cells["PartId"].Value = command.PartId;
                            //rgr.Cells["Status"].Value = command.Status;
                            //rgr.Cells["Msg"].Value = command.Msg;
                        }
                    });
                }));
            this.rgvMain.ContextMenuOpening += RgvMain_ContextMenuOpening;
            MqttManager.Current.Publish(EmqttSetting.Current.PcsNgInitReq, "");
        }

        private void RgvMain_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            //e.ContextMenu.Items.Clear();
            var customMenuItem = new RadMenuItem { Text = @"初始化" };
            customMenuItem.Click += (s, d) =>
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsNgInitReq, "");
            };
            e.ContextMenu.Items.Add(customMenuItem);

            var deleteMenuItem = new RadMenuItem { Text = @"删除" };
            deleteMenuItem.Click += (s, d) =>
            {
                MqttManager.Current.Publish(EmqttSetting.Current.DeletePcsNgReq, new PcsNgArgs()
                {
                    PartId = this.rgvMain.SelectedRows[0].Cells["PartId"].Value.ToString(),
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    Status = 0,
                    Msg = ""
                });
            };
            e.ContextMenu.Items.Add(deleteMenuItem);
        }
    }
}