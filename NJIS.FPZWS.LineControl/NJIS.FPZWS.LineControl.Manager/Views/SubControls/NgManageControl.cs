using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.LocalServices;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Manager.Views.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Manager.Views.SubControls
{
    public partial class NgManageControl : UserControl,IView
    {
        private NgManageControlPresenter presenter = new NgManageControlPresenter();
        private bool isReenter = false;
        private List<string> PortNames = null;
        public NgManageControl()
        {
            InitializeComponent();
            radPageView1.Pages.RemoveAt(1);

            this.RegisterTipsMessage();
            this.BindingPresenter(presenter);
            this.Register<List<string>>(NgManageControlPresenter.BindingData, (list => PortNames = list), EExecutionMode.Synchronization);
            this.Register<PartFeedBack>(NgManageControlPresenter.BindingData, partInfo =>
            {
                if (partInfo != null)
                {
                    txtDeviceName.Text = partInfo.DeviceName;
                    txtBatchName.Text = partInfo.BatchName;
                    txtPartLength.Text = partInfo.Length.ToString();
                    txtPartWidth.Text = partInfo.Width.ToString();
                    txtColor.Text = partInfo.Color;
                    txtUpi.Text = partInfo.PartId;
                    txtNgType.Text = partInfo.PartId.Contains("X") ? "余料板" : "正常板";

                }
                else
                {
                    txtDeviceName.Text = string.Empty;
                    txtBatchName.Text = string.Empty;
                    txtPartLength.Text = string.Empty;
                    txtPartWidth.Text = string.Empty;
                    txtColor.Text = string.Empty;
                    txtUpi.Text = string.Empty;
                    txtNgType.Text = string.Empty;
                }
            });
            this.Register<bool>(NgManageControlPresenter.BindingData, ret =>
            {
                txtPartId.Text = "";
                if (ret)
                {
                    panelResult.BackColor = Color.FromArgb(0, 153, 0);
                    panelResult.BackgroundImage = Properties.Resources.Ok;
                }
                else
                {
                    panelResult.BackColor = Color.FromArgb(255, 0, 0);
                    panelResult.BackgroundImage = Properties.Resources.Ng;
                }

                isReenter = false;
                if (!checkAuto.Checked)
                {
                    btnExecute.Enabled = true;
                }
            });

            this.Register<Tuple<int, int>>(NgManageControlPresenter.BindingData, (tuple =>
            {
                txtRequest.Text = tuple.Item1.ToString();
                txtResponse.Text = tuple.Item2.ToString();
                if (checkAuto.Checked)
                {
                    if (tuple.Item1 > tuple.Item2)
                    {
                        string partId = txtPartId.Text.Trim();
                        if (string.IsNullOrWhiteSpace(partId)) return;
                        if (isReenter) return;
                        isReenter = true;
                        this.Send(NgManageControlPresenter.RequestReenter, partId);
                    }
                }


            }));
            this.Register<string>(NgManageControlPresenter.BindingData, (s =>
            {
                txtPartId.Text = s;
                if (checkAuto.Checked)
                {
                    int request = int.Parse(txtRequest.Text.Trim());
                    int response = int.Parse(txtResponse.Text.Trim());
                    if (request > response)
                    {

                        this.Send(NgManageControlPresenter.RequestReenter, s);

                    }
                }

            }));
            this.Register<string>(NgManageControlPresenter.BindingSerialMsg, s =>
             {
                 lbPortMsg.Text = s;
             }, EExecutionMode.Synchronization);
            this.Register<string>(NgManageControlPresenter.BindingPlcMsg, (s =>
            {
                lbPlcMsg.Text = s;
            }), EExecutionMode.Synchronization);
            this.BindingPresenter(presenter);
            this.Disposed += (sender, arg) =>
            {
                this.UnBindingPresenter();
            };
            this.HandleCreated += NGManageControl_HandleCreated;
        }

        private void NGManageControl_HandleCreated(object sender, EventArgs e)
        {
            if (!SerialListenService.GetInstance().CanStart)
            {
                SerialListenService.GetInstance().CanStart = true;
            }

            if (!PlcListenService.GetInstance().CanStart)
            {
                PlcListenService.GetInstance().CanStart = true;
            }
        }

        private void btnSerialSetting_Click(object sender, EventArgs e)
        {
            SerialPortSettingForm form = new SerialPortSettingForm();
            this.Send(NgManageControlPresenter.GetData, 0);
            form.BindingData(PortNames);
            var result = form.ShowDialog((RadButton)sender);
            if (result != DialogResult.Cancel)
            {
                this.Send(NgManageControlPresenter.UpdatedSerialSetting, form.SerialPortSetting);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            btnExecute.Enabled = false;
            int request = int.Parse(txtRequest.Text.Trim());
            int response = int.Parse(txtResponse.Text.Trim());
            string partId = txtPartId.Text.Trim();
            if (string.IsNullOrWhiteSpace(partId))
            {
                Tips("板件码不能为空");
                btnExecute.Enabled = true;
                return;
            }

            if (request <= response)
            {
                Tips("未收到入板请求");
                btnExecute.Enabled = true;
                return;
            }
            this.Send(NgManageControlPresenter.RequestReenter, partId);
        }


        private void Tips(string msg)
        {
            BeginInvoke((Action)(()=> RadMessageBox.Show(this, msg)));
        }

        private void txtPartId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string partId = txtPartId.Text.Trim();
                if (string.IsNullOrWhiteSpace(partId))
                {
                    Tips("板件码不能为空");
                    return;
                }
                this.Send(NgManageControlPresenter.GetData,partId);
            }
        }

        private void checkAuto_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            btnExecute.Enabled = !checkAuto.Checked;
        }

        
    }
}
