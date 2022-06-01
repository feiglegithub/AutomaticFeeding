//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：AlienSession.cs
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
    ///     异形客户端的异步对象
    /// </summary>
    public class AlienSession
    {
        /// <summary>
        ///     实例化一个默认的参数
        /// </summary>
        public AlienSession()
        {
            IsStatusOk = true;
        }


        /// <summary>
        ///     网络套接字
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        ///     唯一的标识
        /// </summary>
        public string DTU { get; set; }

        /// <summary>
        ///     指示当前的网络状态
        /// </summary>
        public bool IsStatusOk { get; set; }
    }
}