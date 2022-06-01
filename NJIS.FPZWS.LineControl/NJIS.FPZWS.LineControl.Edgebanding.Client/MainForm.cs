//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：MainForm.cs
//   创建时间：2018-12-13 14:35
//   作    者：
//   说    明：
//   修改时间：2018-12-13 14:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Edgebanding.Client.Forms;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Edgebanding.Client
{
    public partial class MainForm : BaseForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            rpvMain.Pages.Clear();
            if (EdgebandingSetting.Current.EnablePartInfoQueueForm)
            {
                AddView(new PartInfoQueueForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShortcutKeys = Keys.F1
                });
            }

            if (EdgebandingSetting.Current.EnableCommandForm)
            {
                AddView(new CommandForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShortcutKeys = Keys.F2
                });
            }

            if (EdgebandingSetting.Current.EnableAlarmForm)
            {
                AddView(new AlarmForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShortcutKeys = Keys.F3
                });
            }

            if (EdgebandingSetting.Current.EnableMsgForm)
            {
                AddView(new MsgForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShortcutKeys = Keys.F4
                });
            }

            //if (EdgebandingSetting.Current.EnableMachine)
            //{
            //    AddView(new MachineForm
            //    {
            //        FormBorderStyle = FormBorderStyle.None,
            //        ShortcutKeys = Keys.F5
            //    });
            //}


            if (EdgebandingSetting.Current.EnableAnalyzeForm)
            {
                AddView(new AnalyzeForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShortcutKeys = Keys.F7
                });
            }


            if (rpvMain.Pages.Count > 0)
            {
                rpvMain.SelectedPage = rpvMain.Pages[0];
            }
        }

        public void AddView(EdgebandingForm form)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.TopLevel = false;
            var text = $"{form.Text}({form.ShortcutKeys.ToString()})";
            if (form.ShortcutKeys == Keys.None)
            {
                text = $"{form.Text}";
            }

            var page = new RadPageViewPage(text)
            {
                TextAlignment = ContentAlignment.MiddleCenter,
                Image = form.Icon.ToBitmap()
            };
            page.Padding = new Padding(0);
            page.Controls.Add(form);
            rpvMain.Pages.Add(page);
            rpvMain.SelectedPage = page;
        }

        private void rpvMain_SelectedPageChanging(object sender, RadPageViewCancelEventArgs e)
        {
            if (e.Page != null && e.Page.Controls.Count > 0)
            {
                var form = e.Page.Controls[0] as Form;
                if (form != null)
                {
                    form.Show();
                }
            }
        }


        private void rpvMain_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (var rpvMainPage in rpvMain.Pages)
            {
                if (rpvMainPage.Controls.Count == 0) continue;
                var form = rpvMainPage.Controls[0] as EdgebandingForm;
                if (form != null)
                {
                    if (e.KeyData == form.ShortcutKeys)
                    {
                        rpvMain.SelectedPage = rpvMainPage;
                        break;
                    }
                }
            }
        }
    }
}