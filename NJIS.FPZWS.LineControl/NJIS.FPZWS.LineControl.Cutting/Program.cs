﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.UI.Views;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new MainForm();
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                RadMessageBox.Show(form, "程序已经启动，无需再次打开");
                form.Close();
                form.Dispose();
                //System.Threading.Thread.Sleep(3000);
                return;
            }
            
            
            Application.Run(form);
        }
    }
}