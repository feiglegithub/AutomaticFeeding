using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WCS.Common
{
    internal class SystemTimeWin32
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern bool SetSystemTime(ref Systemtime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref Systemtime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref Systemtime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref Systemtime sysTime);

        /// <summary>
        /// 时间结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Systemtime
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
}
