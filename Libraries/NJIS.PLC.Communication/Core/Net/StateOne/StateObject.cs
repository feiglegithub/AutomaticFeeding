//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：StateObject.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Net.Sockets;

namespace NJIS.PLC.Communication.Core.Net.StateOne
{
    /// <summary>
    ///     网络中的异步对象
    /// </summary>
    internal class StateObject : StateOneBase
    {
        #region Public Method

        /// <summary>
        ///     清空旧的数据
        /// </summary>
        public void Clear()
        {
            IsError = false;
            IsClose = false;
            AlreadyDealLength = 0;
            Buffer = null;
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     实例化一个对象
        /// </summary>
        public StateObject()
        {
        }

        /// <summary>
        ///     实例化一个对象，指定接收或是发送的数据长度
        /// </summary>
        /// <param name="length"></param>
        public StateObject(int length)
        {
            DataLength = length;
            Buffer = new byte[length];
        }

        /// <summary>
        ///     唯一的一串信息
        /// </summary>
        public string UniqueId { get; set; }

        #endregion

        #region Public Member

        /// <summary>
        ///     网络套接字
        /// </summary>
        public Socket WorkSocket { get; set; }


        /// <summary>
        ///     是否关闭了通道
        /// </summary>
        public bool IsClose { get; set; }

        #endregion
    }
}