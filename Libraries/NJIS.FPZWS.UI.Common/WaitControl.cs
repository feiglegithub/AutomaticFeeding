using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.FPZWS.UI.Common
{
    public partial class WaitControl : UserControl
    {
        public WaitControl()
        {
            InitializeComponent();
        }
        public WaitControl(Control win32Window)
        {
            InitializeComponent();
            win32Window.Controls.Add(this);
        }

        /// <summary>
        /// 更新等待提示
        /// </summary>
        /// <param name="tips"></param>
        public void UpdateTips(string tips = "")
        {
            dotsRingWaitingBarIndicatorElement1.Text = tips;
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        public void StarWaiting()
        {
            this.radWaitingBar1.StartWaiting();
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void StopWaiting()
        {
            this.radWaitingBar1.StopWaiting();
        }
    }
}
