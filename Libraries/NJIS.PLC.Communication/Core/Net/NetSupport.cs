//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：NetSupport.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.IO;
using System.Net.Sockets;
using NJIS.PLC.Communication.Core.Types;

namespace NJIS.PLC.Communication.Core.Net
{
    /*******************************************************************************
     * 
     *    网络通信类的基础类，提供所有相关的基础方法和功能
     *
     *    Network communication base class of the class, provides the basis of all relevant methods and functions
     * 
     *******************************************************************************/

    #region Network Helper

    /// <summary>
    ///     静态的方法支持类，提供一些网络的静态支持，支持从套接字从同步接收指定长度的字节数据，并支持报告进度。
    /// </summary>
    /// <remarks>
    ///     在接收指定数量的字节数据的时候，如果一直接收不到，就会发生假死的状态。接收的数据时保存在内存里的，不适合大数据块的接收。
    /// </remarks>
    /// <example>
    ///     一个接收的示例
    ///     <code lang="cs"
    ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
    ///         region="ReadBytesFromSocketExample2" title="ReadBytesFromSocket示例" />
    /// </example>
    public static class NetSupport
    {
        /// <summary>
        ///     Socket传输中的缓冲池大小
        /// </summary>
        internal const int SocketBufferSize = 4096;

        /// <summary>
        ///     检查是否超时的静态方法
        /// </summary>
        /// <param name="timeout">数据封送对象</param>
        /// <param name="millisecond">超时的时间</param>
        internal static void ThreadPoolCheckConnect(NjisTimeOut timeout, int millisecond)
        {
            while (!timeout.IsSuccessful)
            {
                if ((DateTime.Now - timeout.StartTime).TotalMilliseconds > millisecond)
                {
                    // 连接超时或是验证超时
                    if (!timeout.IsSuccessful) timeout.WorkSocket?.Close();
                    break;
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        ///     检查是否超时的方法信息
        /// </summary>
        /// <param name="obj">socket对象</param>
        internal static void ThreadPoolCheckTimeOut(object obj)
        {
            if (obj is NjisTimeOut)
            {
                var timeout = obj as NjisTimeOut;
                while (!timeout.IsSuccessful)
                {
                    if ((DateTime.Now - timeout.StartTime).TotalMilliseconds > timeout.DelayTime)
                    {
                        // 连接超时或是验证超时
                        if (!timeout.IsSuccessful)
                        {
                            timeout.Operator?.Invoke();
                            timeout.WorkSocket?.Close();
                        }

                        break;
                    }
                }
            }
        }


        /// <summary>
        ///     读取socket数据的基础方法，只适合用来接收指令头，或是同步数据
        /// </summary>
        /// <param name="socket">通信对象</param>
        /// <param name="receive">接收的长度</param>
        /// <returns>接收到的字节数据</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <example>
        ///     接收数据的举例，简单的接收20个字节长度的数据。
        ///     <code lang="cs"
        ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
        ///         region="ReadBytesFromSocketExample1" title="ReadBytesFromSocket示例" />
        ///     如何接收不定长度的数据呢？我们可以将一条数据拆分成2次接收，第一次是接收8个固定的字节，解析成长度，再接收真实的数据。
        ///     <code lang="cs"
        ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
        ///         region="ReadBytesFromSocketExample3" title="ReadBytesFromSocket示例" />
        /// </example>
        public static byte[] ReadBytesFromSocket(Socket socket, int receive)
        {
            return ReadBytesFromSocket(socket, receive, null, false, false);
        }


        /// <summary>
        ///     读取socket数据的基础方法，只适合用来接收指令头，或是同步数据
        /// </summary>
        /// <param name="socket">通信对象</param>
        /// <param name="receive">接收的长度</param>
        /// <param name="report">用于报告接收进度的对象</param>
        /// <param name="reportByPercent">是否按照百分比报告进度</param>
        /// <param name="response">是否回发接收数据长度</param>
        /// <returns>接收到的字节数据</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <example>
        ///     接收数据的举例，输出报告，不根据百分比来产生报告，不回复接收进度。
        ///     <code lang="cs"
        ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
        ///         region="ReadBytesFromSocketExample2" title="ReadBytesFromSocket示例" />
        /// </example>
        public static byte[] ReadBytesFromSocket(Socket socket, int receive, Action<long, long> report,
            bool reportByPercent, bool response)
        {
            var bytes_receive = new byte[receive];
            var count_receive = 0;
            long percent = 0;
            while (count_receive < receive)
            {
                // 分割成2KB来接收数据
                var receive_length = receive - count_receive >= SocketBufferSize
                    ? SocketBufferSize
                    : receive - count_receive;
                count_receive += socket.Receive(bytes_receive, count_receive, receive_length, SocketFlags.None);
                if (reportByPercent)
                {
                    var percentCurrent = (long) count_receive * 100 / receive;
                    if (percent != percentCurrent)
                    {
                        percent = percentCurrent;
                        // 报告进度
                        report?.Invoke(count_receive, receive);
                    }
                }
                else
                {
                    // 报告进度
                    report?.Invoke(count_receive, receive);
                }

                // 回发进度
                if (response) socket.Send(BitConverter.GetBytes((long) count_receive));
            }

            return bytes_receive;
        }


        /// <summary>
        ///     从socket套接字读取数据并写入流中，必然报告进度
        /// </summary>
        /// <param name="socket">通信对象</param>
        /// <param name="stream">stream</param>
        /// <param name="receive">接收的长度</param>
        /// <param name="report">用于报告接收进度的对象</param>
        /// <param name="reportByPercent">是否按照百分比报告进度</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <example>
        ///     举例从socket读取数据，然后写入到文件流中
        ///     <code lang="cs"
        ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
        ///         region="WriteStreamFromSocketExample" title="WriteStreamFromSocket示例" />
        /// </example>
        public static void WriteStreamFromSocket(Socket socket, Stream stream, long receive, Action<long, long> report,
            bool reportByPercent)
        {
            var buffer = new byte[SocketBufferSize];
            long count_receive = 0;
            long percent = 0;
            while (count_receive < receive)
            {
                // 分割成4KB来接收数据
                var current = socket.Receive(buffer, 0, SocketBufferSize, SocketFlags.None);
                count_receive += current;
                stream.Write(buffer, 0, current);
                if (reportByPercent)
                {
                    var percentCurrent = count_receive * 100 / receive;
                    if (percent != percentCurrent)
                    {
                        percent = percentCurrent;
                        // 报告进度
                        report?.Invoke(count_receive, receive);
                    }
                }
                else
                {
                    // 报告进度
                    report?.Invoke(count_receive, receive);
                }

                // 回发进度
                socket.Send(BitConverter.GetBytes(count_receive));
            }

            buffer = null;
        }


        /// <summary>
        ///     读取流并将数据写入socket
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="socket">连接的套接字</param>
        /// <param name="length">返回的文件长度</param>
        /// <param name="report">发送的进度报告</param>
        /// <param name="reportByPercent">是否按照百分比报告进度</param>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <example>
        ///     举例从文件读取数据，然后写入到套接字中，相当于发送文件到socket
        ///     <code lang="cs"
        ///         source="NJIS.PLC.Communication_Net45.Test\Documentation\Samples\Core\NetSupport.cs"
        ///         region="WriteSocketFromStreamExample" title="WriteSocketFromStream示例" />
        /// </example>
        public static void WriteSocketFromStream(Socket socket, Stream stream, long length, Action<long, long> report,
            bool reportByPercent)
        {
            var buffer = new byte[SocketBufferSize];
            long count_send = 0;
            stream.Position = 0;
            long percent = 0;

            while (count_send < length)
            {
                var count = stream.Read(buffer, 0, SocketBufferSize);
                count_send += count;
                socket.Send(buffer, 0, count, SocketFlags.None);

                while (count_send != BitConverter.ToInt64(ReadBytesFromSocket(socket, 8), 0)) ;

                var received = count_send;

                if (reportByPercent)
                {
                    var percentCurrent = received * 100 / length;
                    if (percent != percentCurrent)
                    {
                        percent = percentCurrent;
                        // 报告进度
                        report?.Invoke(received, length);
                    }
                }
                else
                {
                    // 报告进度
                    report?.Invoke(received, length);
                }

                // 双重接收验证
                if (count == 0)
                {
                    break;
                }
            }

            buffer = null;
        }
    }

    #endregion
}