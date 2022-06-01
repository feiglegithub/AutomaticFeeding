using System;
using System.Linq;
using System.Windows.Forms;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.Edgebandingg.Monitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RadForm1());
        }
    }
}