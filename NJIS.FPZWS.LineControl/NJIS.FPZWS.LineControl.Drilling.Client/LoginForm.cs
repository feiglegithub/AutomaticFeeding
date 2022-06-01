// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.CR
//  项目名称：NJIS.FPZWS.App.Client
//  文 件 名：LoginForm.cs
//  创建时间：2019-09-26 9:07
//  作    者：
//  说    明：
//  修改时间：2019-09-26 8:56
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Windows.Forms;          
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    public partial class LoginForm : RadForm
    {
        public LoginForm()
        {
            InitializeComponent();

            if (RegistryHelper.IsExistValueName(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath,
                WinRegistryHelper.UserNamePath))
            {
                var userName = RegistryHelper.GetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath,
                    WinRegistryHelper.UserNamePath);
                var password = RegistryHelper.GetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath,
                    WinRegistryHelper.PasswordPath);
                rtxtUserName.Text = userName == null ? "" : userName.ToString();
                rtxtPassword.Text = password == null ? "" : password.ToString();
            }
        }

        private void rbtnLogin_Click(object sender, EventArgs e)
        {

            if (Login())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        public static bool ExitLogin()
        {
            try
            {

                RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.UserNamePath, "");
                RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.LoginTimePath, "");
                RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.PasswordPath, "");
                RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.DepartPath, "");

            }
            catch (Exception e)
            {
            }

            Application.Exit();
            return true;
        }


        public static bool CheckLogin()
        {
            var pdt = RegistryHelper.GetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.LoginTimePath);

            // 未设置登录时间，或最后登录为7天前，则需要重新登录
            if (pdt == null || DateTime.Parse(pdt.ToString()).AddDays(7) < DateTime.Now)
            {
                return false;
            }

            var depts = RegistryHelper.GetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.DepartPath);

            if (depts == null || string.IsNullOrEmpty(depts.ToString()))
            {
                return false;
            }
            var form = new LoginForm();
            return form.Login();
        }

        public bool Login()
        {

            rlblMsg.Text = "";
            RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.DepartPath, "");
            var userName = rtxtUserName.Text.Trim();
            var password = rtxtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                rlblMsg.Text = "请输入用户名！";
                return false;
            }
            if (string.IsNullOrEmpty(userName))
            {
                rlblMsg.Text = "请输入密码！";
                return false;
            }
            try
            {
                DomainPasswordValidator dpv = new DomainPasswordValidator();
                if (dpv.Validate(userName,password))
                {
                    RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.UserNamePath, userName);

                    if (rcbRemember.Checked)
                    {
                        RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.PasswordPath, password);
                    }
                    RegistryHelper.SetValue(RegistryBaseKey.CurrentUser, WinRegistryHelper.AppPath, WinRegistryHelper.LoginTimePath, DateTime.Now);

                    return true;
                }
            }
            catch (Exception exception)
            {
                RadMessageBox.Show(this, $"登录异常：{exception}.");
                return false;
            }

            return false;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}