//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：AsyncStateSend.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Net.Sockets;
using NJIS.PLC.Communication.Core.Thread;

namespace NJIS.PLC.Communication.Core.Net.StateOne
{
    internal class AsyncStateSend
    {
        /// <summary>
        ///     传输数据的对象
        /// </summary>
        internal Socket WorkSocket { get; set; }

        /// <summary>
        ///     发送的数据内容
        /// </summary>
        internal byte[] Content { get; set; }

        /// <summary>
        ///     已经发送长度
        /// </summary>
        internal int AlreadySendLength { get; set; }


        internal SimpleHybirdLock HybirdLockSend { get; set; }
    }
}