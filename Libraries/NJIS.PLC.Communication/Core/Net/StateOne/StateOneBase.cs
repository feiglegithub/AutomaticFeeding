//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：StateOneBase.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Threading;

namespace NJIS.PLC.Communication.Core.Net.StateOne
{
    /// <summary>
    ///     异步消息的对象
    /// </summary>
    internal class StateOneBase
    {
        /// <summary>
        ///     本次接收或是发送的数据长度
        /// </summary>
        public int DataLength { get; set; } = 32;

        /// <summary>
        ///     已经处理的字节长度
        /// </summary>
        public int AlreadyDealLength { get; set; }

        /// <summary>
        ///     操作完成的信号
        /// </summary>
        public ManualResetEvent WaitDone { get; set; }


        /// <summary>
        ///     缓存器
        /// </summary>
        public byte[] Buffer { get; set; }


        /// <summary>
        ///     是否发生了错误
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        ///     错误消息
        /// </summary>
        public string ErrerMsg { get; set; }
    }
}