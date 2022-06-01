//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.MprImport
//   文 件 名：Program.cs
//   创建时间：2019-07-25 9:26
//   作    者：
//   说    明：
//   修改时间：2019-07-25 9:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Threading;
using System.Windows.Forms;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.MprImport
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;
            Application.Run(new MainForm());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            //LogHelper.Error(ex.Exception);

            var message = string.Format("{0}\r\n操作发生错误，您需要退出系统么？", ex.Exception.Message);
            if (DialogResult.Yes == MessageBox.Show(message, @"系统错误", MessageBoxButtons.YesNo))
            {
                Application.Exit();
            }
        }
    }
}