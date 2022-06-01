// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：NjisMessageBox.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2017-11-02 11:27
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    /// <summary>
    ///     消息框
    /// </summary>
    public class NjisMessageBox
    {
        /// <summary>
        ///     成功提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="timeout"></param>
        public static DialogResult SucessShow(string msg, string caption = "系统提示", int timeout = 0)
        {
            ThreadPool.QueueUserWorkItem(CloseMessageBox, new CloseState(caption, timeout < 1000 ? 1000 : timeout));
            // timeout,1000是毫秒

            return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     警告提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="timeout"></param>
        public static DialogResult WarnShow(string msg, string caption = "系统提示", int timeout = 3000)
        {
            ThreadPool.QueueUserWorkItem(CloseMessageBox, new CloseState(caption, timeout < 1000 ? 1000 : timeout));

            return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        ///     询问提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="timeout"></param>
        public static DialogResult AskShow(string msg, string caption = "系统提示", int timeout = 30000)
        {
            ThreadPool.QueueUserWorkItem(CloseMessageBox, new CloseState(caption, timeout < 1000 ? 1000 : timeout));
            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static void CloseMessageBox(object state)
        {
            var closeState = state as CloseState;
            Thread.Sleep(closeState.Timeout);
            var dlg = FindWindow(null, closeState.Caption);

            if (dlg != IntPtr.Zero)
            {
                IntPtr result;
                EndDialog(dlg, out result);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool EndDialog(IntPtr hDlg, out IntPtr nResult);
    }

    /// <summary>
    ///     消息框状态
    /// </summary>
    internal class CloseState
    {
        public CloseState(string caption, int timeout)
        {
            Timeout = timeout;
            Caption = caption;
        }

        /// <summary>
        ///     In millisecond
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        ///     Caption of dialog
        /// </summary>
        public string Caption { get; }
    }
}