// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：FormExtension.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-05-17 8:18
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace NJIS.FPZWS.UI.Common.Extension
{
    public static class FormExtension
    {
        public static void AddMdiChild(this RadFormControlBase parent, RadDock radContainer, RadForm form)
        {
            //radContainer.SizeChanged -= RadContainer_SizeChanged;
            var currWindows = radContainer.DockWindows.FirstOrDefault(f => f.Name == form.Text);
            if (currWindows == null)
            {
                currWindows = new DocumentWindow
                {
                    Text = form.Text,
                    Name = form.Text,
                    AutoSize = true,
                    Padding = new Padding(0, 0, 0, 0),
                    Margin = new Padding(0, 0, 0, 0)
                    //Dock = DockStyle.Fill
                };
                radContainer.AddDocument(currWindows);
                form.TopLevel = false;
                currWindows.Controls.Add(form);

                form.AutoSize = true;
                form.Padding = new Padding(0, 0, 0, 0);
                form.Margin = new Padding(0, 0, 0, 0);
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }

            currWindows.Select();
            //radContainer.SizeChanged += RadContainer_SizeChanged;
        }

        private static void RadContainer_SizeChanged(object sender, EventArgs e)
        {
            var radContainer = (RadDock) sender;
            radContainer.SuspendLayout();
            radContainer.SuspendUpdate();
            var currActiveDoc = radContainer.ActiveWindow;
            var docList = radContainer.DocumentManager.DocumentArray.ToList();
            radContainer.RemoveAllWindows();
            foreach (var item in docList)
            {
                var currForm = new RadForm();
                foreach (var ctrl in item.Controls)
                {
                    if (!(ctrl is RadForm)) continue;
                    var radForm = (RadForm) ctrl;
                    currForm = radForm;
                }

                var currWindows = new DocumentWindow
                {
                    Text = item.Text,
                    Name = item.Text,
                    AutoSize = true
                };
                radContainer.AddDocument(currWindows);
                if (currActiveDoc != null && currWindows.Text == currActiveDoc.Text) currActiveDoc = currWindows;
                currForm.TopLevel = false;
                currWindows.Controls.Add(currForm);

                currForm.Dock = DockStyle.Fill;
                currForm.FormBorderStyle = FormBorderStyle.None;
                currForm.WindowState = FormWindowState.Maximized;
                currForm.Show();
            }

            radContainer.ResumeUpdate();
            radContainer.ResumeLayout();
            currActiveDoc?.Select();
        }
    }
}