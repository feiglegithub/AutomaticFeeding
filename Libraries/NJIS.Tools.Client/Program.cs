// ************************************************************************************
//  解决方案：NJIS.FPZWS.Sorting.Client
//  项目名称：NJIS.Tools.Client
//  文 件 名：Program.cs
//  创建时间：2017-11-02 16:39
//  作    者：
//  说    明：
//  修改时间：2017-11-03 8:40
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Windows.Forms;
using NJIS.Tools.Client.UI;
using NJIS.Windows.TemplateBase;
using Telerik.WinControls;
using System.Reflection;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;
using NJIS.Tools.Client.Helper;

#endregion

namespace NJIS.Tools.Client
{
    internal static class Program
    {

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                UIUtils.HandleException("ERROR", ex);
            }
            finally
            {
                System.Environment.Exit(0);
            }
        }

    }
}