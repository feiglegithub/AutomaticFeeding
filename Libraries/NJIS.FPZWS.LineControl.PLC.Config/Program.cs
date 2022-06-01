//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：Program.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Windows.Forms;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    internal class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "app")
            {
                LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}