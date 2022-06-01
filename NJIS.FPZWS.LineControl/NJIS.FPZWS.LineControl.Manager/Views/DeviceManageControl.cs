using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.LineControl.Manager.Views.Dialog;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class DeviceManageControl : UserControl,IView
    {
        public const string ReceiveDatas = nameof(ReceiveDatas);
        public const string ReceiveProcessNames = nameof(ReceiveProcessNames);
        public const string SaveDeviceInfos = nameof(SaveDeviceInfos);
        private DeviceManageControlPresenter _presenter = null;

        private LineControlCuttingService lineControlCuttingService = new LineControlCuttingService();

        /// <summary>
        /// Presenter 是否已经初始化
        /// </summary>
        private bool _presenterInited = false;

        public DeviceManageControl()
        {
            InitializeComponent();
            gridViewBase1.ReadOnly = false;
            DeviceInfos dis;
            gridViewBase1.AddColumns(new List<UI.Common.Controls.ColumnInfo>()
            {
                #region old code
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="设备号", FieldName=nameof(dis.DeviceName), DataType=typeof(string), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="设备类型", FieldName=nameof(dis.DeviceType), DataType=typeof(string), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="部门", FieldName=nameof(dis.DepartmentId), DataType=typeof(int), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="产线", FieldName=nameof(dis.ProductionLine), DataType=typeof(string), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="位置", FieldName=nameof(dis.PlaceNo), DataType=typeof(string), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="状态", FieldName=nameof(dis.State), DataType=typeof(int), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="工段", FieldName=nameof(dis.ProcessName), DataType=typeof(string), ReadOnly=true },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="设备描述", FieldName=nameof(dis.DeviceDescription), DataType=typeof(string), ReadOnly=false },
                //new UI.Common.Controls.ColumnInfo(){ HeaderText="备注", FieldName=nameof(dis.Remark), DataType=typeof(string), ReadOnly=false },
                


                #endregion
                
                new ColumnInfo(){ HeaderText="设备号", FieldName=nameof(dis.DeviceName), DataType=typeof(string), ReadOnly=false },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="设备类型", FieldName=nameof(dis.DeviceType)
                    , DataType=typeof(string), ReadOnly=false,DataSource = new Dictionary<string,string>(){{ "CuttingMachine", "CuttingMachine" } }
                    ,DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){ HeaderText="部门", FieldName=nameof(dis.DepartmentId), DataType=typeof(int), ReadOnly=false },
                new ColumnInfo(){ HeaderText="产线", FieldName=nameof(dis.ProductionLine), DataType=typeof(string), ReadOnly=false },
                new ColumnInfo(){ HeaderText="位置", FieldName=nameof(dis.PlaceNo), DataType=typeof(string), ReadOnly=false },
                new ColumnInfo(){ HeaderText="效率比", FieldName=nameof(dis.Ratio), DataType=typeof(float), ReadOnly=false },
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(dis.State)
                    , DataType=typeof(int), ReadOnly=false,DataSource = new Dictionary<int,string>(){{0,"禁用"},{1,"启用"}}
                    ,DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){ HeaderText="工段", FieldName=nameof(dis.ProcessName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="设备描述", FieldName=nameof(dis.DeviceDescription), DataType=typeof(string), ReadOnly=false },
                new ColumnInfo(){ HeaderText="备注", FieldName=nameof(dis.Remark), DataType=typeof(string), ReadOnly=false },



            });

            this.VisibleChanged += DeviceManageControl_VisibleChanged;
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void DeviceManageControl_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible&&!_presenterInited)
            {
                _presenter = new DeviceManageControlPresenter();
                this.Register<List<DeviceInfos>>(ReceiveDatas, ExecuteBindingDatas);
                this.Register<List<string>>(ReceiveProcessNames, ExecuteBindingProcessNames);
                this.Register<bool>(SaveDeviceInfos, ExecuteSaveDeviceInfo);
                this.RegisterTipsMessage();
                this.BindingPresenter(_presenter);
                this.Send(DeviceManageControlPresenter.GetProcessNames, "");
                _presenterInited = true;
            }
        }

        private void ExecuteSaveDeviceInfo(bool result)
        {
            if(result)
            {
                Tips("保存成功!");
                //this.Send(DeviceManageControlPresenter.GetDeviceInfos, cmbProcessName.Text.Trim());
                var deviceInfos = lineControlCuttingService.GetDeviceInfosByProcessName(cmbProcessName.Text.Trim());
                gridViewBase1.DataSource = deviceInfos;
            }
            else
            {
                Tips("保存失败!");
            }
        }

        private void ExecuteBindingProcessNames(List<string> processNames)
        {
            cmbProcessName.DataSource = processNames;
            
        }

        private void InitGridView()
        {
            //foreach(var row in  gridViewBase1.Rows)
            //{
            //    //row.Cells["DeviceName"].ReadOnly = true; 
            //    //row.Cells["ProductionLine"].ReadOnly = true;
            //    //row.Cells["DepartmentId"].ReadOnly = true;
            //}
        }

        private void ExecuteBindingDatas(List<DeviceInfos> deviceInfos)
        {
            gridViewBase1.DataSource = deviceInfos;
            InitGridView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //gridViewBase1.BeginWait();

            //this.Send(DeviceManageControlPresenter.GetDeviceInfos, cmbProcessName.Text.Trim());
            var deviceInfos = lineControlCuttingService.GetDeviceInfosByProcessName(cmbProcessName.Text.Trim());
            gridViewBase1.DataSource = deviceInfos;
        }

        private void btnAddDeviceInfo_Click(object sender, EventArgs e)
        {
            var gridView = this.gridViewBase1.radGridView;
            gridView.FilterDescriptors.Clear();
            gridView.ReadOnly = false;
            var newRow = gridView.Rows.AddNew();
            newRow.Cells["ProcessName"].Value = cmbProcessName.Text.Trim();
            newRow.Cells["ProcessName"].ReadOnly = true;
        }

        private void btnSaveDeviceInfo_Click(object sender, EventArgs e)
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();

            if (passwordValidationForm.dialogResult == DialogResult.OK)
            {
                if (passwordValidationForm.result)
                {
                    var changeRows = this.gridViewBase1.ChangeRows;
                    List<DeviceInfos> newDeviceInfos = new List<DeviceInfos>();
                    List<DeviceInfos> editDeviceInfos = new List<DeviceInfos>();
                    foreach (var newRow in changeRows.CurrNewAddRows)
                    {
                        var obj = newRow.DataBoundItem as DeviceInfos;
                        newDeviceInfos.Add(obj);
                    }
                    if (newDeviceInfos.Count > 0)
                    {
                        //this.Send(DeviceManageControlPresenter.AddDeviceInfo, newDeviceInfos);
                        bool ret = lineControlCuttingService.BulkInsertDeviceInfos(newDeviceInfos);
                        ExecuteSaveDeviceInfo(ret);
                    }
                    foreach (var editRow in changeRows.CurrEidtedRows)
                    {
                        var obj = editRow.DataBoundItem as DeviceInfos;
                        editDeviceInfos.Add(obj);
                    }
                    if (editDeviceInfos.Count > 0)
                    {
                        //this.Send(DeviceManageControlPresenter.UpdateDeviceInfo, editDeviceInfos);
                        bool ret = lineControlCuttingService.BulkUpdateDeviceInfos(editDeviceInfos);
                        ExecuteSaveDeviceInfo(ret);
                    }
                }
                else
                {
                    Tips("密码验证失败！");
                }
            }
            
             
        }

        private void Tips(string message)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(this, message)));
        }
    }
}
