using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls
{
    public partial class NGManageControl : UserControl,IView
    {
        private NGManageControlPresenter presenter = new NGManageControlPresenter();
        private bool isReenter = false;
        private List<string> PortNames = null;
        public NGManageControl()
        {
            InitializeComponent();
            this.RegisterTipsMessage();
            this.BindingPresenter(presenter);
            this.Register<List<string>>(NGManageControlPresenter.BindingData,(list => PortNames=list), EExecutionMode.Synchronization);
            this.Register<CuttingTaskDetail>(NGManageControlPresenter.BindingData, partInfo =>
            {
                if (partInfo != null)
                {
                    txtDeviceName.Text = partInfo.DeviceName;
                    txtBatchName.Text = partInfo.BatchName;
                    txtPartLength.Text = partInfo.Length.ToString();
                    txtPartWidth.Text = partInfo.Width.ToString();
                    txtColor.Text = partInfo.Color;
                    txtUpi.Text = partInfo.PartId;
                    txtNgType.Text = partInfo.IsOffPart ? "余料板" : "正常板";

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
            this.Register<bool>(NGManageControlPresenter.BindingData, ret =>
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
            this.Register<Tuple<int, int>>(NGManageControlPresenter.BindingData, (tuple =>
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
                        this.Send(NGManageControlPresenter.RequestReenter, partId); 
                    }
                }


            }));
            this.Register<string>(NGManageControlPresenter.BindingData, (s =>
            {
                txtPartId.Text = s;
                if (checkAuto.Checked)
                {
                    int request = int.Parse(txtRequest.Text.Trim());
                    int response = int.Parse(txtResponse.Text.Trim());
                    if (request > response)
                    {

                        this.Send(NGManageControlPresenter.RequestReenter, s);

                    }
                }
                
            }));
            this.Register<string>(NGManageControlPresenter.BindingSerialMsg,s =>
            {
                lbPortMsg.Text = s;
            }, EExecutionMode.Synchronization);
            this.Register<string>(NGManageControlPresenter.BindingPlcMsg, (s =>
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
            this.Send(NGManageControlPresenter.GetData,0);
            form.BindingData(PortNames);
            var result = form.ShowDialog((RadButton) sender);
            if (result != DialogResult.Cancel)
            {
                this.Send(NGManageControlPresenter.UpdatedSerialSetting, form.SerialPortSetting);
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
            this.Send(NGManageControlPresenter.RequestReenter, partId);
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
                this.Send(NGManageControlPresenter.GetData,partId);
            }
        }

        private void checkAuto_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            btnExecute.Enabled = !checkAuto.Checked;
        }

        
    }
}
