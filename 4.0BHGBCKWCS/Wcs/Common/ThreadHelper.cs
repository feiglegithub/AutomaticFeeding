using System.Threading;
using System.Windows.Forms;

namespace WCS.Common
{
    public static class ThreadHelper
    {
        /// <summary>
        /// 跨线程访问控件 在控件上执行委托
        /// </summary>
        /// <param name="ctl">控件</param>
        /// <param name="del">执行的委托</param>
        public static void CrossThreadCalls(this Control ctl, ThreadStart proxy)
        {
            if (proxy == null) return;
            if (ctl.InvokeRequired)
                ctl.Invoke(proxy, null);
            else
                proxy();
        }
    }
}
