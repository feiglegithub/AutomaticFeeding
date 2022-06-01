using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace ReaderDemo
{
    class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReaderDate
        {
            public byte Year;			//年
            public byte Month;			//月
            public byte Day;			//日
            public byte Hour;			//时
            public byte Minute;			//分
            public byte Second;			//秒
        }

        public struct ReaderBasicParam
        {
            public byte BaudRate;			//串口的通信速率，取值：00H~08H，代表速率同"设定波特率"命令。
            public byte Power;				//发射功率值，取值：20~30dBm。
            public byte Min_Frequence;		//发射微波信号频率的起始点，取值： 0~59。
            public byte Max_Frequence;		//发射微波信号频率的终止点，取值： 0~59。
            public byte Reserve5;			//保留
            public byte WorkMode;			//读写器工作方式：0-主动方式，1-命令方式
            public byte ReaderAddress;		//RS485地址:1--254
            public byte NumofCard;			//最多读卡数目。
            public byte TagType;			//标签种类：01H－ISO18000-6B，02H－EPCC1，04H－EPCC1G2，08H－EM4442。
            public byte Reserve10;		    //保留
            public byte ReadTimes;			//读卡次数M：收到上位机读卡命令，读写器执行M次此命令。
            public byte EnableBuzzer;		//1:使能蜂鸣器0:不使能蜂鸣器
            public byte IP1;			    //读写器IP地址
            public byte IP2;			    //
            public byte IP3;			    //
            public byte IP4;			    //
            public byte Port1;				//读写器端口高位
            public byte Port2;				//
            public byte Mask1;				//读写器掩码1
            public byte Mask2;				//读写器掩码2
            public byte Mask3;				//读写器掩码3
            public byte Mask4;				//读写器掩码4
            public byte Gateway1;			//读写器地址网关
            public byte Gateway2;			//
            public byte Gateway3;			//
            public byte Gateway4;			//
            public byte MAC1;			    //读写器MAC地址
            public byte MAC2;			    //
            public byte MAC3;			    //
            public byte MAC4;			    //
            public byte MAC5;			    //
            public byte MAC6;			    //

        }

        //读写器主动工作参数
        public struct ReaderAutoParam
        {
            public byte AutoMode;			//读标签模式：0-定时方式，1-触发方式。
            public byte TimeH;				//标签保留时间：单位：秒s。缺省值为1。
            public byte TimeL;				//
            public byte Interval;			//0-10ms，1-20ms，2-30ms，3-50ms，4-100ms。缺省值为2。每隔设定时间主动读取一次标签。
            public byte NumH;				//标签保留数目：缺省值为1。已读取的标签ID在读写器内存中保留的数目。
            public byte NumL;				//
            public byte OutputManner;	    //数据输出格式：0-简化格式，1-标准格式，2-XML格式。缺省值为0。
            public byte OutInterface;		//输出接口：0－RS232，1－RS485，2－RJ45,3- Wiegand26,4- Wiegand34。缺省值为0。
            public byte WiegandWidth;		//Weigand脉冲宽度值。
            public byte WiegandInterval;	//Weigand脉冲间隔值。
            public byte ID_Start;			//输出卡号的起始位，取值0～4。
            public byte IDPosition;			//卡号在电子标签上的存放地址。
            public byte Report_Interval;	//通知间隔：单位秒s。缺省值为1。每隔设定时间主动通知上位机一次。
            public byte Report_Condition;	//通知条件：缺省值为1。0-被动通知，1-定时通知，2-增加新标签，3-减少标签，4-标签数变化	
            public byte Report_Output;		//通知输出端
            public byte Antenna;			//天线选择。1-ant1,2-ant2,4-ant4,8-ant8
            public byte TriggerMode;	    //触发方式(缺省值为0): 0-低电平 1-高电平
            public byte HostIP1;			//主机IP地址
            public byte HostIP2;			//
            public byte HostIP3;			//
            public byte HostIP4;			//
            public byte Port1;				//主机端口
            public byte Port2;				//
            public byte Reserve24;			//
            public byte Reserve25;			//
            public byte Reserve26;			//
            public byte Reserve27;			//
            public byte Reserve28;			//
            public byte Reserve29;			//
            public byte Alarm;				//0-不报警，1-报警。在定时和触发方式下是否检测报警。
            public byte Reserve31;		    //
            public byte EnableRelay;		//自动工作模式是否控制继电器1:控制 0:不控制
        }

        #region Controlling command

        //连接读写器
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ConnectScanner(ref int hScanner, string PortNum, ref int nBaudRate);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ConnectScanner(ref int hSocket, string nTargetAddress, uint nTargetPort, string nHostAddress, uint nHostPort);
        //485连接
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ConnectScanner485(ref int hScanner, string PortNum, int nBaudRate, int address);
        //断开读写器连接
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int DisconnectScanner(int hScanner);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_DisconnectScanner();
        //获得读写器版本号        
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReaderVersion(int hScanner, ref int hardver, ref int Softver, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReaderVersion(int hSocket, ref int hardver, ref int Softver);
        //选择工作天线        
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetAntenna(int hScanner, int m_antenna_sel, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetAntenna(int hSocket, int m_antenna_sel);

        //设置读写器继电器状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetRelay(int hScanner, int Relay, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetRelay(int hSocket, int Relay);

        //设定输出功率
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetOutputPower(int hScanner, int nPower1, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetOutputPower(int hSocket, int nPower1);

        //获取读写器基本工作参数
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ReadBasicParam(int hScanner, ref ReaderBasicParam pParam, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ReadBasicParam(int hSocket, ref ReaderBasicParam pParam);

        //设置读写器基本工作参数
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int WriteBasicParam(int hScanner, ref ReaderBasicParam pParam, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_WriteBasicParam(int hSocket, ref ReaderBasicParam pParam);

        //读取读写器自动工作参数
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ReadAutoParam(int hScanner, ref ReaderAutoParam pParam, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ReadAutoParam(int hSocket, ref ReaderAutoParam pParam);

        //设置读写器自动工作参数
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int WriteAutoParam(int hScanner, ref ReaderAutoParam pParam, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_WriteAutoParam(int hSocket, ref ReaderAutoParam pParam);

        //读取读写器出厂参数
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ReadFactoryParameter(int hScanner);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ReadFactoryParameter(int hSocket);

        //复位读写器
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Reboot(int hScanner, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_Reboot(int hSocket);

        //启动/停止读写器自动模式
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int AutoMode(int hScanner, int mode, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_AutoMode(int hSocket, int mode);


        //设置时间
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetReaderTime(int hScanner, ReaderDate time, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetReaderTime(int hSocket, ReaderDate time);

        //获得时间
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReaderTime(int hScanner, ref ReaderDate time, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReaderTime(int hSocket, ref ReaderDate time);

        //设置标签过滤器
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetReportFilter(int hScanner, int len, int ptr, byte[] mask, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetReportFilter(int hSocket, int len, int Ptr, byte[] mask);

        //获得标签过滤器
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReportFilter(int hScanner, ref int len, ref int ptr, byte[] mask, int address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReportFilter(int hSocket, ref int len, ref int ptr, byte[] mask);

        //获得读写器ID
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReaderID(int hScanner, byte[] ReaderID, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReaderID(int hSocket, byte[] ReaderID);

        //==============================网络命令==============================
        //设置读写器IP地址
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetReaderNetwork(int hScanner, byte[] IP_Address, int Port, byte[] Mask, byte[] Gateway);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetReaderNetwork(int hSocket, byte[] IP_Address, int Port, byte[] Mask, byte[] Gateway);

        //获得读写器IP地址
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReaderNetwork(int hScanner, byte[] IP_Address, ref int Port, byte[] Mask, byte[] Gateway);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReaderNetwork(int hSocket, byte[] IP_Address, ref int Port, byte[] Mask, byte[] Gateway);

        //设置读写器MAC地址
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int SetReaderMAC(int hScanner, byte[] MAC);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_SetReaderMAC(int hSocket, byte[] MAC);

        //获得读写器MAC地址
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int GetReaderMAC(int hScanner, byte[] MAC);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_GetReaderMAC(int hSocket, byte[] MAC);
        #endregion

        #region EPCC1G2
        //==============================EPCC1G2数据读写命令==============================
        //识别标签的EPC
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_ReadLabelIDRSSI(int hScanner, int mem, int ptr, int len, byte[] mask, byte[] IDBuffer, ref int nCounter, int Address);



        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_ReadLabelID(int hScanner, int mem, int ptr, int len, byte[] mask, byte[] IDBuffer, ref int nCounter, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadLabelID(int hSocket, int mem, int ptr, int len, byte[] mask, byte[] IDBuffer, ref int nCounter);
        [DllImport("ReaderDynamicLib.dll")]
        unsafe public static extern int EPC1G2_ReadLabelTID(int hScanner, int mem, int ptr, int len, byte[] mask, byte** IDBuffer, ref int nCounter, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        unsafe public static extern int Net_EPC1G2_ReadLabelTID(int hSocket, int mem, int ptr, int len, byte[] mask, byte** IDBuffer, ref int nCounter);
        //读一块数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_ReadWordBlock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_ReadWordBlock(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword);

        //写一块数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_WriteWordBlock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_WriteWordBlock(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword);
        //设置读写保护状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_SetLock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte Lock, byte[] AccessPassword, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_SetLock(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte mem, byte Lock, byte[] AccessPassword);
        //永久休眠标签
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_KillTag(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte[] KillPassword, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_KillTag(int hSocket, byte EPC_WORD, byte[] IDBuffer, byte[] KillPassword);
        //写EPC
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int EPC1G2_WriteEPC(int hScanner, byte len, byte[] Data, byte[] AccessPassword, int Address);
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_EPC1G2_WriteEPC(int hSocket, byte len, byte[] Data, byte[] AccessPassword);

        //==============================ISO-6B数据读写命令==============================
        //检测标签存在
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_LabelPresent(int hScanner, ref int nCounter);

        //读取ISO6B标签ID号
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_ReadLabelID(int hSocket, byte[,] IDBuffer, ref int nCounter);

        //列出选定标签
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_ListSelectedID(int hSocket, int Cmd, int ptr, byte Mask, byte[] Data, byte[,] IDBuffer, ref int nCounter);

        //列出选定标签
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_ListSelectedID(int hScanner, int Cmd, int ptr, byte Mask, byte[] Data, byte[,] IDBuffer, ref int nCounter, int Address);


        //读一块数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_ReadByteBlock(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data);

        //读一块数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_ReadByteBlock(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data, int Address);

        //一次写4字节数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_WriteByteBlock(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data, int Address);

        //一次写4字节数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_WriteByteBlock(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data);

        //一次写一字节数据
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_WriteAByte(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data);

        //写大块数据，字节数超过16
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_WriteLongBlock(int hScanner, byte[] IDBuffer, byte ptr, byte len, byte[] Data);

        //置写保护状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_WriteProtect(int hScanner, byte[] IDBuffer, byte ptr);

        //置写保护状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_WriteProtect(int hScanner, byte[] IDBuffer, byte ptr, int Address);


        //读写保护状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_ReadWriteProtect(int hScanner, byte[] IDBuffer, byte ptr, ref byte Protected);

        //读写保护状态
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_ReadWriteProtect(int hScanner, byte[] IDBuffer, byte ptr, ref byte Protected, int Address);


        //全部清除
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int Net_ISO6B_ClearMemory(int hScanner, byte CardType, byte[] IDBuffer);

        //读取ISO6B标签ID号
        [DllImport("ReaderDynamicLib.dll")]
        public static extern int ISO6B_ReadLabelID(int hScanner, byte[,] IDBuffer, ref int nCounter, int Address);


        #endregion


        #region systemAPI
        public delegate void TimerProc(IntPtr hWnder, uint nMsg, int nIDEvent, int dwTime);
        [DllImport("user32.dll")]
        public static extern int SetTimer(int hwnd, int nIDEvent, int uElapse, TimerProc CB);
        [DllImport("user32.dll")]
        public static extern int KillTimer(int hwnd, int nIDEvent);

        [DllImport("kernel32.dll")]
        public static extern bool GetCommState(
         int hFile,
         ref DCB lpDCB
       );

        [DllImport("kernel32.dll")]
        public static extern bool SetCommState(
         int hFile,  // 通信设备句柄 
         ref DCB lpDCB    // 设备控制块 
       );

        [DllImport("kernel32.dll")]
        public static extern bool PurgeComm(
        int hFile,  // 通信设备句柄 
        uint dwFlags
        );


        //        [DllImport("kernel32.dll")]
        //        public static extern bool Beep(int frequency, int duration);

        [DllImport("user32.dll")]
        public static extern bool MessageBeep(int beepType);


        [StructLayout(LayoutKind.Sequential)]
        public struct DCB
        {
            //taken from c struct in platform sdk  
            public int DCBlength;           // sizeof(DCB)  
            public int BaudRate;            // 指定当前波特率 current baud rate 
            // these are the c struct bit fields, bit twiddle flag to set 
            public int fBinary;          // 指定是否允许二进制模式,在windows95中必须主TRUE binary mode, no EOF check  
            public int fParity;          // 指定是否允许奇偶校验 enable parity checking  
            public int fOutxCtsFlow;      // 指定CTS是否用于检测发送控制，当为TRUE是CTS为OFF，发送将被挂起。 CTS output flow control  
            public int fOutxDsrFlow;      // 指定CTS是否用于检测发送控制 DSR output flow control  
            public int fDtrControl;       // DTR_CONTROL_DISABLE值将DTR置为OFF, DTR_CONTROL_ENABLE值将DTR置为ON, DTR_CONTROL_HANDSHAKE允许DTR"握手" DTR flow control type  
            public int fDsrSensitivity;   // 当该值为TRUE时DSR为OFF时接收的字节被忽略 DSR sensitivity  
            public int fTXContinueOnXoff; // 指定当接收缓冲区已满,并且驱动程序已经发送出XoffChar字符时发送是否停止。TRUE时，在接收缓冲区接收到缓冲区已满的字节XoffLim且驱动程序已经发送出XoffChar字符中止接收字节之后，发送继续进行。　FALSE时，在接收缓冲区接收到代表缓冲区已空的字节XonChar且驱动程序已经发送出恢复发送的XonChar之后，发送继续进行。XOFF continues Tx  
            public int fOutX;          // TRUE时，接收到XoffChar之后便停止发送接收到XonChar之后将重新开始 XON/XOFF out flow control  
            public int fInX;           // TRUE时，接收缓冲区接收到代表缓冲区满的XoffLim之后，XoffChar发送出去接收缓冲区接收到代表缓冲区空的XonLim之后，XonChar发送出去 XON/XOFF in flow control  
            public int fErrorChar;     // 该值为TRUE且fParity为TRUE时，用ErrorChar 成员指定的字符代替奇偶校验错误的接收字符 enable error replacement  
            public int fNull;          // eTRUE时，接收时去掉空（0值）字节 enable null stripping  
            public int fRtsControl;     // RTS flow control 
            /*RTS_CONTROL_DISABLE时,RTS置为OFF 
             RTS_CONTROL_ENABLE时, RTS置为ON 
           RTS_CONTROL_HANDSHAKE时, 
           当接收缓冲区小于半满时RTS为ON 
              当接收缓冲区超过四分之三满时RTS为OFF 
           RTS_CONTROL_TOGGLE时, 
           当接收缓冲区仍有剩余字节时RTS为ON ,否则缺省为OFF*/
            public int fAbortOnError;   // TRUE时,有错误发生时中止读和写操作 abort on error  
            public int fDummy2;        // 未使用 reserved  

            public uint flags;
            public ushort wReserved;          // 未使用,必须为0 not currently used  
            public ushort XonLim;             // 指定在XON字符发送这前接收缓冲区中可允许的最小字节数 transmit XON threshold  
            public ushort XoffLim;            // 指定在XOFF字符发送这前接收缓冲区中可允许的最小字节数 transmit XOFF threshold  
            public byte ByteSize;           // 指定端口当前使用的数据位   number of bits/byte, 4-8  
            public byte Parity;             // 指定端口当前使用的奇偶校验方法,可能为:EVENPARITY,MARKPARITY,NOPARITY,ODDPARITY  0-4=no,odd,even,mark,space  
            public byte StopBits;           // 指定端口当前使用的停止位数,可能为:ONESTOPBIT,ONE5STOPBITS,TWOSTOPBITS  0,1,2 = 1, 1.5, 2  
            public char XonChar;            // 指定用于发送和接收字符XON的值 Tx and Rx XON character  
            public char XoffChar;           // 指定用于发送和接收字符XOFF值 Tx and Rx XOFF character  
            public char ErrorChar;          // 本字符用来代替接收到的奇偶校验发生错误时的值 error replacement character  
            public char EofChar;            // 当没有使用二进制模式时,本字符可用来指示数据的结束 end of input character  
            public char EvtChar;            // 当接收到此字符时,会产生一个事件 received event character  
            public ushort wReserved1;         // 未使用 reserved; do not use  
        }

        #endregion

    }

}