//  ************************************************************************************
//   解决方案：NJIS.RFID.HBSEPD
//   项目名称：NJIS.RFID.HBSEPD
//   文 件 名：RfidHelper.cs
//   创建时间：2019-05-02 8:09
//   作    者：
//   说    明：
//   修改时间：2019-05-02 8:09
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace NJIS.RFID.HBSEPD
{
    public class UdpRfider : IRfider
    {

        public string Id { get; set; }
        public bool IsConnected
        {
            get { return SocketId > 0; }
        }

        private const int OK = 0;
        public int SocketId { get; private set; }

        public int ConnectRetryCount { get; set; } = 3;

        public UdpRfider(string readerIp, uint readerPort, string hostIp, uint hostPort)
        {
            ReaderIp = readerIp;
            ReaderPort = readerPort;
            HostIp = hostIp;
            HostPort = hostPort;
            Id = readerIp;
            SocketId = -1;
        }


        public string ReaderIp { get; set; }
        public uint ReaderPort { get; set; }
        public string HostIp { get; set; }
        public uint HostPort { get; set; }


        /// <summary>
        /// 读取偏移位
        /// </summary>
        public byte ReadPrt { get; set; } = 4;

        /// <summary>
        /// 读取长度
        /// </summary>
        public byte ReadLen { get; set; } = 6;

        /// <summary>
        ///     连接扫码器
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            var res = -1;
            int i;
            for (i = 0; i < ConnectRetryCount; i++)
            {
                var mId = -1;
                res = RfidReader.Net_ConnectScanner(ref mId, ReaderIp, ReaderPort, HostIp, HostPort);
                if (res == OK)
                {
                    SocketId = mId;
                    break;
                }
            }
            //RFID扫码器连接成功
            return true;
        }

        /// <summary>
        ///     断开连接
        /// </summary>
        /// <returns></returns>
        public bool DisConnect()
        {
            var result = RfidReader.Net_DisconnectScanner();
            return result > 0;
        }

        public string ReadString()
        {
            var lst = ReadStrings();
            if (lst.Length > 0) return lst[0];
            return "";
        }

        public string[] ReadStrings()
        {
            var list = new List<string>();
            var db = new byte[128];
            var idBuffer = new byte[7680];
            int nCounter = 0;
            int idLen = 0;
            byte[] mask = new byte[96];
            int mem = 0;

            //读EPC标签(nCounter:表示读取到的数据组，1个RFID结果1组）
            var res = RfidReader.Net_EPC1G2_ReadLabelID(SocketId, mem, ReadPrt, ReadLen, mask, idBuffer, ref nCounter);
            if (res == OK)
            {
                // 访问密码
                var accessPassWord = new byte[4] { 0, 0, 0, 0 };

                byte[,] tagBuffer = new byte[100, 130];

                // 解析读取到的数据，nCounter
                for (var i = 0; i < nCounter; i++)
                {
                    byte[] idTemp = new byte[12];
                    var idLenTemp = idBuffer[idLen] * 2 + 1;

                    for (var j = 0; j < idLenTemp; j++)
                    {
                        tagBuffer[i, j] = idBuffer[idLen + j];
                    }
                    // 获取第组数据包
                    var epcWord = tagBuffer[i, 0];
                    for (var k = 0; k < tagBuffer[i, 0] * 2; k++)
                    {
                        idTemp[k] = tagBuffer[0, k + 1];
                    }
                    System.Threading.Thread.Sleep(50);
                    //通过标签读取托盘号
                    res = RfidReader.Net_EPC1G2_ReadWordBlock(SocketId, epcWord, idTemp, 1, ReadPrt, ReadLen, db, accessPassWord);
                    if (res == OK)
                    {
                        var nPalletId = Encoding.UTF8.GetString(db).TrimEnd('\0');
                        list.Add(nPalletId);
                    }

                    idLen += idLenTemp;
                }

            }
            if (SocketId == -1)
            {
                RfidReader.Net_DisconnectScanner();
                RfidReader.DisconnectScanner(SocketId);
                SocketId = 0;
            }

            return list.ToArray();
        }
    }
}