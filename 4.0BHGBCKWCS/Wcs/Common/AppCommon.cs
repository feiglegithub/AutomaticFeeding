using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;



namespace WCS
{
    public  class AppCommon
    {
        public static string INIPath = Application.StartupPath + Properties.Settings.Default.INIPath;

        public static string WCSConnstr = Properties.Settings.Default.DBWCSPROD;  //正式
        //public static string WCSConnstr = Properties.Settings.Default.DBWCS;  //测试

        public static int DdjNum = 4;

        //public static DateTime dt_start = new DateTime(2018, 9, 30, 9, 00, 00);
        public static DateTime dt_stop = new DateTime(2018, 9, 30, 9, 10, 00);

        public static bool IsTest = false;
        public static bool IsOpenIn = true;
        public static bool IsOpenOut = true;
        public static bool IsOpen205 = true;

        public static bool IsDdjRun(int ddj)
        {
            if (ddj >= 1)
                return true;
            return false;
        }

        public static bool IsFinish1Run(int station)
        {
            if (station <= 18)
                return true;
            return false;
        }


        public static bool IsLoopRun(int loop)
        {
            if (loop <= 10)
                return true;
            return false;
        }

        public static bool PingIp(string ip)
        {
            Ping p1 = new Ping();
            PingReply reply = p1.Send(ip, 100);
            p1.Dispose();

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
