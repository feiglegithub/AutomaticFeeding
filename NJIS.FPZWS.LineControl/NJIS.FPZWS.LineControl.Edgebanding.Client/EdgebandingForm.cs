//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：EdgebandingForm.cs
//   创建时间：2018-12-13 14:35
//   作    者：
//   说    明：
//   修改时间：2018-12-13 14:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Windows.Forms;
using NJIS.FPZWS.UI.Common;

namespace NJIS.FPZWS.LineControl.Edgebanding.Client
{
    public partial class EdgebandingForm : BaseForm
    {
        public EdgebandingForm()
        {
            InitializeComponent();
        }

        public Keys ShortcutKeys { get; set; }
    }
}