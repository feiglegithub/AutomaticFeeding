//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：DrillingForm.cs
//   创建时间：2018-11-28 11:53
//   作    者：
//   说    明：
//   修改时间：2018-11-28 11:53
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Windows.Forms;
using NJIS.FPZWS.UI.Common;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class DrillingForm : BaseForm
    {
        public DrillingForm()
        {
            InitializeComponent();
        }

        public Keys ShortcutKeys { get; set; }
    }
}