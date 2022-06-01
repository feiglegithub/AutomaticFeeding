//  ************************************************************************************
//   解决方案：NJIS.RFID.HBSEPD
//   项目名称：NJIS.RFID.HBSEPD
//   文 件 名：RFIDReader.cs
//   创建时间：2019-05-02 8:10
//   作    者：
//   说    明：
//   修改时间：2019-05-02 8:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Runtime.InteropServices;

namespace NJIS.RFID.HBSEPD
{
    internal class RfidReader
    {
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ConnectScanner(ref int hSocket, string nTargetAddress, uint nTargetPort,
            string nHostAddress, uint nHostPort);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_DisconnectScanner();

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int DisconnectScanner(int hScanner);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadWordBlock(int hSocket, byte epcWord, byte[] idBuffer, byte mem,
            byte ptr, byte len, byte[] Data, byte[] accessPassword);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetAntenna(int hSocket, int mAntennaSel);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadLabelID(int hSocket, int mem, int ptr, int len, byte[] mask, byte[] idBuffer, ref int nCounter);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_WriteWordBlock(int hSocket, byte epcWord, byte[] idBuffer, byte mem, byte ptr, byte len, byte[] data, byte[] accessPassword);
    }
}