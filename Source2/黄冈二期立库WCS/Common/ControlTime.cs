using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WCS
{
    class ControlTime
    {
        [DllImport("Kernel32.dll")]
        static extern bool SetLocalTime(ref SystemTime sysTime);
        public static bool SetSysTime(DateTime dt)
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();
            sysTime.wYear = Convert.ToUInt16(dt.Year);
            sysTime.wMonth = Convert.ToUInt16(dt.Month);
            sysTime.wDay = Convert.ToUInt16(dt.Day);
            sysTime.wHour = Convert.ToUInt16(dt.Hour);
            sysTime.wMinute = Convert.ToUInt16(dt.Minute);
            sysTime.wSecond = Convert.ToUInt16(dt.Second);
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            catch (Exception e)
            {
                Console.WriteLine("SetSystemDateTime函数执行异常" + e.Message);
            }
            return flag;
        }

        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="type">DD,HH,MM,SS,MS</param>
        /// <returns></returns>
        public static int DiffTime(DateTime begin,DateTime end,string type)
        {
            int day = 0, hour = 0, minute = 0;

            TimeSpan span = (TimeSpan)(end - begin);

            day = span.Days;
            hour = day * 24 + span.Hours;
            minute = hour * 60 + span.Minutes;

            switch(type)
            {
                case "DD":
                    return day;
                case "HH":
                    return hour;
                case "MM":
                    return minute;
                case "SS":
                    return span.Seconds + minute * 60;
                case "MS":
                    return (span.Seconds + minute * 60) + 1000 + span.Milliseconds;
                default:
                    throw new Exception("无此时间类型！");

            }
        }

    }

    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMiliseconds;
    }
}
