//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：AppSession.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Net;
using System.Net.Sockets;
using NJIS.PLC.Communication.Core.Thread;

namespace NJIS.PLC.Communication.Core.Net.StateOne
{
    /// <summary>
    ///     网络会话信息
    /// </summary>
    public class AppSession
    {
        /// <summary>
        ///     UDP通信中的远程端
        /// </summary>
        internal EndPoint UdpEndPoint = null;

        #region Constructor

        /// <summary>
        ///     实例化一个构造方法
        /// </summary>
        public AppSession()
        {
            ClientUniqueID = Guid.NewGuid().ToString("N");
            HybirdLockSend = new SimpleHybirdLock();
        }

        #endregion


        /// <summary>
        ///     传输数据的对象
        /// </summary>
        internal Socket WorkSocket { get; set; }

        internal SimpleHybirdLock HybirdLockSend { get; set; }

        /// <summary>
        ///     IP地址
        /// </summary>
        public string IpAddress { get; internal set; }

        /// <summary>
        ///     此连接对象连接的远程客户端
        /// </summary>
        public IPEndPoint IpEndPoint { get; internal set; }

        /// <summary>
        ///     远程对象的别名
        /// </summary>
        public string LoginAlias { get; set; }

        /// <summary>
        ///     心跳验证的时间点
        /// </summary>
        public DateTime HeartTime { get; set; } = DateTime.Now;

        /// <summary>
        ///     客户端的类型
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        ///     客户端唯一的标识
        /// </summary>
        public string ClientUniqueID { get; }


        /// <summary>
        ///     指令头缓存
        /// </summary>
        internal byte[] BytesHead { get; set; } = new byte[NjisProtocol.HeadByteLength];

        /// <summary>
        ///     已经接收的指令头长度
        /// </summary>
        internal int AlreadyReceivedHead { get; set; }

        /// <summary>
        ///     数据内容缓存
        /// </summary>
        internal byte[] BytesContent { get; set; }

        /// <summary>
        ///     已经接收的数据内容长度
        /// </summary>
        internal int AlreadyReceivedContent { get; set; }

        /// <summary>
        ///     用于关键字分类使用
        /// </summary>
        internal string KeyGroup { get; set; }

        /// <summary>
        ///     清除本次的接收内容
        /// </summary>
        internal void Clear()
        {
            BytesHead = new byte[NjisProtocol.HeadByteLength];
            AlreadyReceivedHead = 0;
            BytesContent = null;
            AlreadyReceivedContent = 0;
        }


        #region Object Override

        /// <summary>
        ///     返回表示当前对象的字符串，以IP，端口，客户端名称组成
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(LoginAlias))
            {
                return $"AppSession[{IpEndPoint}]";
            }

            return $"AppSession[{IpEndPoint}] [{LoginAlias}]";
        }

        #endregion
    }
}