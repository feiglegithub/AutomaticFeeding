// ************************************************************************************
//  解决方案：NJIS.FPZWS.Sorting.Client
//  项目名称：NJIS.Tools.Client
//  文 件 名：LoginForm.cs
//  创建时间：2017-11-02 16:39
//  作    者：
//  说    明：
//  修改时间：2017-11-03 8:40
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using NJIS.FPZWS.Log;
using NJIS.Tools.Client.Helper;
using NJIS.Windows.TemplateBase;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.UI;

#endregion

namespace NJIS.Tools.Client.UI.Dialogs
{
    public partial class LoginForm : RadForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                LoginHelper.ValidateUserAccount("sogal", txtAccount.Text, txtPassword.Text);

                if (LoginHelper.IsLoginSuccess)
                {
                    //-----------------------------------------------------------------------------------------

                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                    LogManager.GetLogger("AuthorityButton").Info($"ActionUser:{LoginHelper.CurrLoginUser}  ActionPath:{this.GetType().FullName} Method:{ st.GetFrame(0).ToString()} ");
                    //-----------------------------------------------------------------------------------------
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    labMsg.Text = "登录失败, 输入的帐号或密码有误, 请查正再输入.";
                    labMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                labMsg.Text = "登录出错. " + ex.Message;
                labMsg.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtAccount_Validating(object sender, CancelEventArgs e)
        {
        }
    }
}