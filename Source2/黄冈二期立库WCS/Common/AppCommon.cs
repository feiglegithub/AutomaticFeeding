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

        public static string WCSConnstr = Properties.Settings.Default.DBWCSProd;
        //public static string WCSConnstr = Properties.Settings.Default.DBWCS;

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

        //根据逻辑编号找到对应的设备编号
        public static int GetDeviceNoByWMSNo(int wms_no)
        {
            var no = 0;

            switch (wms_no)
            {
                case 1001:
                    no = 1181;
                    break;
                case 1002:
                    no = 1181;
                    break;
                case 1003:
                    no = 1181;
                    break;
                case 1004:
                    no = 1181;
                    break;
                case 1005:
                    no = 1181;
                    break;
            }

            return no;
        }

        public static string GetDeviceErrorMsg(int code10)
        {
            if (code10 == 0) { return ""; }
            var code2 = Convert.ToString(code10, 2);
            var msg = "";

            int idx = 1;
            // 10
            for (int i = code2.Length - 1; i >= 0; i--)
            {
                if (code2[i] == '1')
                {
                    if (idx == 1)
                    {
                        msg += "超时|";
                    }
                    else if (idx == 2)
                    {
                        msg += "电机保护|";
                    }
                    else if (idx == 3)
                    {
                        msg += "传感器异常|";
                    }
                    else if (idx == 4)
                    {
                        msg += "急停|";
                    }
                    else if (idx == 5)
                    {
                        msg += "超宽|";
                    }
                    else if (idx == 6)
                    {
                        msg += "超长|";
                    }
                    else if (idx == 7)
                    {
                        msg += "超高|";
                    }
                    else if (idx == 8)
                    {
                        msg += "超重|";
                    }
                }

                idx++;
            }


            return msg.Substring(0, msg.Length - 1);
        }

        //验证入库托盘的格式
        public static string ValidPallet(string pallet)
        {
            if (pallet.Length != 11)
            {
                return "托盘号读取失败！";
            }

            if (pallet.StartsWith("TPZ0037") == false)
            {
                return "进立库B的托盘号必须以TPZ0037开头！";
            }

            int num;
            if (int.TryParse(pallet.Substring(7, 4), out num) == false)
            {
                return "托盘号格式错误，需要重写！";
            }

            return "";
        }
    }
}
