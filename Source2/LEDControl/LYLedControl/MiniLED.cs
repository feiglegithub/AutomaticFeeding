using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LYLedControl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClockType
    {
        public byte Second;
        public byte Minute;
        public byte Hour;
        public byte Day;
        public byte Month;
        public byte Week;
        public byte Year;
        public byte NC;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProgItemType
    {
        public short Flag;
        public short PicFIndex;
        public int Effect;
        public int SpeedStay;
        public int Schedule;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PicFileHdr
    {
        public byte Type;
        public byte PicCount;
        public short PicHeight;
        public short PicWidth;
        public short PicOffset;
        public short LastPicH;
        public short LastPicW;
    }

    public static class MiniLED
    {
        [DllImport("MiniLED.dll")]
        public static extern bool MC_ChangeGroup(short id, short group, byte flag, ref byte param);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_Close(short id);
        [DllImport("MiniLED.dll")]
        public static extern int MC_ComInitial(short id, int comPort, int baudRate, int timeoutSecond, int retries, int ledNum);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_ControlPlay(short id, long ctrl);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_GetClock(short id, ClockType clk);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_GetRTViewPkt(short id, ref byte buff, short offset, short len);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_GetRunTimeInfo(short id, ref byte buff, short offset, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MC_NetInitial(short id, string password, string remoteIp, int timeoutSecond, int retries, short udpPort);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_PicToXMPXFile(ref byte[] picBuff, ref Bitmap bitmap, short width, short height, bool dblColor);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_Reset(short id, long func);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SendFontInfo(short id, ref byte buff, short len);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SendFontLibPkt(short id, byte fIdx, ref byte buff, short len, long offset);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_SendProgList(short id, IntPtr progList, short progCount);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SendRTView(short id, ref byte buff, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_SendXMPXPic(short id, short picIndex, ref byte picBuff, long picLength);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SetAutoPower(short id, byte onHour, byte onMinute, byte offHour, byte offMinute);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SetBright(short id, byte brightness);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SetClock(short id);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SetLEDNum(short id, short ledNum);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_SetPowerMode(short id, byte mode);
        [DllImport("MiniLED.dll", EntryPoint = "MC_SetLEDNum")]
        public static extern bool MC_SetRemoteIP(short id, short ledNum);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_ShowString(short id, short left, short top, short width, short height, short xPos, short yPos, short color, ref byte str, byte opt);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_ShowXMPXPic(short id, short left, short top, short width, short height, ref byte picBuff, long picLength);
        [DllImport("MiniLED.dll")]
        public static extern bool MC_TxtToXMPXFile(short id, short picFIndex, short width, short height, short color, ref byte str, byte encode, byte mode);

    }
}
