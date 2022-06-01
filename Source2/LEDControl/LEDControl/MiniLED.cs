using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LEDControl
{
    internal static class MiniLED
    {

        #region 引用dll

        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MC_NetInitial(short ID, string Password, string RemoteIP, int TimeOut, int Retries, short UDPPort);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetLEDNum(short ID, int LedNum);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetRemoteIP(short ID, string RemoteIP);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_Close(short ID);

        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_GetClock(short ID, ref string clock);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetClock(short ID);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetPowerMode(short ID, Byte Mode);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetAutoPower(short ID, Byte OnHour, Byte OnMinute, Byte OffHour, Byte OffMinute);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SetBright(short ID, Byte Brightness);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_ControlPlay(short ID, long ctrl);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_Reset(short ID, long func);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_GetRunTimeInfo(short ID, ref Byte buff, short offset, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_ChangeGroup(short ID, short group, Byte flag, ref Byte Param);

        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SendXMPXPic(short ID, short PicIndex, ref Byte PicBuff, long PicLength);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SendProgList(short ID, IntPtr ProgList, short ProgCount);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SendRTView(short ID, Byte buff, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_GetRTViewPkt(short ID, Byte buff, short offset, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SendFontInfo(short ID, Byte buff, short len);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_SendFontLibPkt(short ID, Byte FIdx, ref Byte buff, short len, long Offset);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_ShowString(short ID, short Left, short Top, short Width, short Height, short XPos, short YPos, short Color, string Str, Byte Opt);
        //[DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern Boolean MC_TxtToXMPXFile(short ID, short PicFIndex, short Width, short Height, short Color, ref Byte[] Str, Byte Encode, Byte Mode);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean MC_ShowXMPXPic(short ID, short Left, short Top, short Width, short Height, ref Byte PicBuff, long PicLength);
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MC_TxtToXMPXFile(short ID, short PicFIndex, short Width, short Height, short Color, string Str, Byte Encode, Byte Mode);

        #endregion

    }
}
