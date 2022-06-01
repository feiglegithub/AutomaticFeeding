// ************************************************************************************
//  解决方案：NJIS.FPZWS.Packing
//  项目名称：NJIS.FPZWS.App
//  文 件 名：SetForm.cs
//  创建时间：2017-07-24 11:20
//  作    者：
//  说    明：
//  修改时间：2017-07-24 11:44
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Windows.Forms;
using NJIS.FPZWS.App.Settings;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Reflection;

#endregion

namespace NJIS.FPZWS.App
{
    public partial class SetForm : Form
    {
        public SetForm()
        {
            InitializeComponent();
        }

        private void SetForm_Load(object sender, EventArgs e)
        {
            //var finder = new TypeFinder<IService>();
            //var types = finder.FindAll();
            //foreach (Type type in types)
            //{
            //    cmbApp.Items.Add(type.FullName);
            //}
            //cmbApp.SelectedIndex = 0;

            cmbApp.Text = AppSetting.Current.App;
            txtService.Text = AppSetting.Current.Service;
            txtServiceName.Text = AppSetting.Current.ServiceName;
            txtDescription.Text = AppSetting.Current.Description;
            txtDisplayName.Text = AppSetting.Current.DisplayName;
            txtHelpTextPrefix.Text = AppSetting.Current.HelpTextPrefix;
            txtInstanceName.Text = AppSetting.Current.InstanceName;
            txtStartTimeout.Text = AppSetting.Current.StartTimeout.ToString();
            txtStopTimeout.Text = AppSetting.Current.StopTimeout.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            AppSetting.Current.App = cmbApp.Text.Trim();
            AppSetting.Current.Service= txtService.Text.Trim();
            AppSetting.Current.ServiceName= txtServiceName.Text.Trim();
            AppSetting.Current.Description= txtDescription.Text.Trim();
            AppSetting.Current.DisplayName= txtDisplayName.Text.Trim();
            AppSetting.Current.HelpTextPrefix= txtHelpTextPrefix.Text.Trim();
            AppSetting.Current.InstanceName = txtInstanceName.Text.Trim();
            AppSetting.Current.StartTimeout = int.Parse(txtStartTimeout.Text.Trim());
            AppSetting.Current.StopTimeout = int.Parse(txtStopTimeout.Text.Trim());

            if (!AppSetting.Current.Save())
            {
                MessageBox.Show(@"保存配置失败！", @"提示");
            }
        }

        private void cmbApp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}