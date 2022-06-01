using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RFIDControl
{
    public class Reader
    {
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ConnectScanner(ref int hSocket, string nTargetAddress, uint nTargetPort, string nHostAddress, uint nHostPort);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_DisconnectScanner();

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int DisconnectScanner(int hScanner);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadWordBlock(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetAntenna(int hSocket, int m_antenna_sel);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadLabelID(int hSocket, int mem, int ptr, int len, byte[] mask, byte[] IDBuffer, ref int nCounter);

        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_WriteWordBlock(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword);
    }
}
