// ************************************************************************************
//  解决方案：NJIS.FPZWS.MDQ.Tool
//  项目名称：NJIS.FPZWS.MDQ.Tool
//  文 件 名：MainForm.cs
//  创建时间：2017-12-23 8:10
//  作    者：
//  说    明：
//  修改时间：2017-12-23 8:49
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Windows.Forms;
using NJIS.Windows.TemplateBase;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

#endregion

namespace NJIS.Windows.TemplateBase
{
    public partial class RadRibbonBaseForm : RadRibbonForm, IMenuContainer, IContainer
    {
        public RadRibbonBaseForm()
        {
            InitializeComponent();
        }

        public void AddChild(RadForm from)
        {
            var window = new DocumentWindow {Text = from.Text};
            from.TopLevel = false;
            from.Dock = DockStyle.Fill;
            from.FormBorderStyle = FormBorderStyle.None;
            from.Show();
            window.Controls.Add(from);
            radDock1.AddDocument(window);
            window.Select();
        }

        public RadRibbonBarCommandTabCollection GetCommandTabCollection()
        {
            return RibbonBar.CommandTabs;
        }

        public IContainer Container { get; set; }
    }
}