// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools
//  项目名称：NJIS.Tools.Client
//  文 件 名：MainForm.cs
//  创建时间：2017-12-07 10:12
//  作    者：
//  说    明：
//  修改时间：2017-12-08 8:41
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using NJIS.Tools.Client.UI.Dialogs;
using NJIS.Tools.Client.UI.Forms;
using NJIS.Windows.TemplateBase;
using NJIS.Windows.TemplateBase.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

#endregion

namespace NJIS.Tools.Client.UI
{
    public partial class MainForm : RadRibbonForm
#if __ISMDICONTAINER__
#else
        , IContainer
#endif
    {
        private RadMenuItem _currentItem;

        public MainForm()
        {
            InitializeComponent();

#if __ISMDICONTAINER__
            IsMdiContainer = true;
#else
#endif


            InitializeThemesMenuItems();


#if __ISMDICONTAINER__
            var pluginsXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.Instance.PluginsXml);
            var doc = new XmlDocument();
            doc.Load(pluginsXml);
            var nodes = doc.SelectNodes("Plugins/RibbonTab");
            foreach (XmlNode n in nodes)
            {
                //string name = n.SelectSingleNode("@name").InnerText;
                var typeName = n.SelectSingleNode("@type").InnerText;
                var path = n.SelectSingleNode("@path").InnerText;

                var type = Type.GetType(typeName);
                if (type == null) continue;

                var oe = (IOperationEntry)Activator.CreateInstance(type, this);
                var group = oe.ButtonGroup;
                var found = GetBarGroup(path);
                if (found != null)
                {
                    found.Items.AddRange(group.Items);
                }
            }
#else
            var finder = new TypeFinder<IMenuContainer>();

            var types = finder.FindInterfacesAll();

            foreach (var type in types)
            {
                var install = (IMenuContainer)System.Activator.CreateInstance(type, this);
                var commandTabs = install.GetCommandTabCollection();
                foreach (var commandTabEventArg in commandTabs)
                {
                    this.radRibbonBar1.CommandTabs.Add(commandTabEventArg);
                }
            }
#endif

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MqttManager.Current.Disconnect();
        }

        private RadRibbonBarGroup GetBarGroup(string path)
        {
            var ary = path.Split('/');
            if (ary.Length != 2) return null;
            RibbonTab foundTab = null;
            RadRibbonBarGroup foundGroup = null;
            foreach (var q in RibbonBar.CommandTabs)
            {
                if (q.Text == ary[0])
                {
                    foundTab = (RibbonTab)q;
                    foreach (var g in foundTab.Items)
                    {
                        if (g.Text == ary[1])
                        {
                            foundGroup = (RadRibbonBarGroup)g;
                            break;
                        }
                    }
                    break;
                }
            }

            if (foundGroup != null)
                return foundGroup;
            if (foundTab == null)
            {
                foundTab = new RibbonTab(ary[0]);
                RibbonBar.CommandTabs.Add(foundTab);
            }

            foundGroup = new RadRibbonBarGroup { Text = ary[1] };
            foundTab.Items.Add(foundGroup);
            return foundGroup;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (LoginHelper.IsLoginSuccess)
            {
                if (DialogResult.OK == UIUtils.Ask(this, "注销确认", "确认要注销当前登录?"))
                {
                    LoginHelper.LoginOut();
                    btnLogin.Text = "登录";
                    lb_loginInfo.Text = "未登录";
                    foreach (var frm in MdiChildren)
                    {
                        frm.Close();
                    }
                }
            }
            else
            {
                var frmLogin = new LoginForm();
                if (DialogResult.OK == frmLogin.ShowDialog(this))
                {
                    lb_loginInfo.Text = $"当前用户:{LoginHelper.CurrLoginUser}";
                    btnLogin.Text = string.Format("注销 {0}", LoginHelper.CurrLoginUser);
                }
            }
        }

        public void InitializeThemesMenuItems()
        {
            var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "themes"),
                "Telerik.WinControls.Themes.*.dll");
            foreach (var f in files)
            {
                var types = Assembly.LoadFile(f).GetTypes();
                foreach (var t in types)
                {
                    if (t.Name.EndsWith("Theme"))
                    {
                        var isDefault = false;
                        var mi = new RadMenuItem(GetThemeName(t.Name, out isDefault))
                        {
                            Font = new Font(new FontFamily("微软雅黑"), Font.Size,
                                isDefault ? FontStyle.Bold : FontStyle.Regular),
                            Tag = t
                        };
                        mi.Click += Mi_Click;
                        miThemes.Items.Add(mi);
                        if (isDefault)
                            _currentItem = mi;
                    }
                }
            }
        }

        private string GetThemeName(string typeName, out bool isDefault)
        {
            var text = typeName;
            isDefault = false;
            text = text.Substring(0, text.Length - 5);
            //if (string.Equals(typeName, Config.Instance.Theme + "Theme"))
            //{
            //    text += " (默认)";
            //    isDefault = true;
            //}
            text = text.Replace("Telerik", "");
            text = Regex.Replace(text, "([A-Z])", " $1");
            text = Regex.Replace(text, "([a-z])([0-9])", "$1 $2");
            return text.Trim();
        }

        private void Mi_Click(object sender, EventArgs e)
        {
            var mi = (RadMenuItem)sender;
            if (_currentItem == null)
                _currentItem = mi;
            else
            {
                _currentItem.Font = new Font(new FontFamily("微软雅黑"), Font.Size, FontStyle.Regular);
                _currentItem = mi;
            }
            mi.Font = new Font(new FontFamily("微软雅黑"), Font.Size, FontStyle.Bold);

            var t = (Type)mi.Tag;
            ThemeName = t.Name.Substring(0, t.Name.Length - 5);
            Activator.CreateInstance(t);
            ThemeResolutionService.ApplicationThemeName = ThemeName;
            this.ResumeLayout(false);
#if __ISMDICONTAINER__
#else
            this.radDock2.ResumeLayout(false);
#endif

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            var au = new AboutBox();
            au.ShowDialog(this);
        }

        private void miOptions_Click(object sender, EventArgs e)
        {
            UIUtils.ShowMdiChild<OptionsForm>(this);
        }

        private void radButtonElement6_Click(object sender, EventArgs e)
        {
            //var pluginsXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.Instance.PluginsXml);
            //var doc = new XmlDocument();
            //doc.Load(pluginsXml);
            //var nodes = doc.SelectNodes("Plugins/RibbonTab");
            //foreach (XmlNode n in nodes)
            //{
            //    var name = n.SelectSingleNode("@name").InnerText;
            //    var asm = n.SelectSingleNode("@assembly").InnerText;
            //    var typeName = n.SelectSingleNode("@type").InnerText;
            //    var quickStartGroup = n.SelectSingleNode("@quickStartGroup").InnerText;

            //    var assembly = Assembly.Load(asm);
            //    if (quickStartGroup == "Group1")
            //    {
            //        var oe = (IOperationEntry)Activator.CreateInstance(assembly.GetType(typeName), this);
            //        oe.QuickStart();
            //    }
            //}
        }

        private void btnWindows_Click(object sender, EventArgs e)
        {
            var screenPoint = MousePosition; //鼠标相对于屏幕左上角的坐标
            var formPoint = PointToClient(MousePosition); //鼠标相对于窗体左上角的坐标
            var contextMenuPoint = WindowsMenuStrip.PointToClient(MousePosition); //鼠标相对于contextMenuStrip1左上角的坐标
            WindowsMenuStrip.Show(contextMenuPoint);
        }

        private void toolStripTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void toolStripTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void toolStripCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
#if __ISMDICONTAINER__

#else
        public void AddChild(RadForm from)
        {
            var window = new DocumentWindow { Text = from.Text };
            from.TopLevel = false;
            from.Dock = DockStyle.Fill;
            from.FormBorderStyle = FormBorderStyle.None;
            from.Show();
            window.Controls.Add(from);
            this.radDock2.AddDocument(window);
            window.Select();
        }

#endif
    }
}