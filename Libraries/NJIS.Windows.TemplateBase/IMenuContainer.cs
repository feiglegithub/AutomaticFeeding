// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools
//  项目名称：NJIS.Tools.Client
//  文 件 名：IMenuContainer.cs
//  创建时间：2017-12-15 11:52
//  作    者：
//  说    明：
//  修改时间：2017-12-15 11:52
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using Telerik.WinControls.UI;

namespace NJIS.Windows.TemplateBase
{
    public interface IMenuContainer
    {
        RadRibbonBarCommandTabCollection GetCommandTabCollection();

        IContainer Container { get; set; }
    }
}