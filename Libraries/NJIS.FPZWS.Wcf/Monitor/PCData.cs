// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：PCData.cs
//  创建时间：2017-12-29 14:20
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Runtime.Serialization;

#endregion

namespace NJIS.FPZWS.Wcf.Monitor
{
    /// <summary>
    ///     服务器性能
    /// </summary>
    [DataContract]
    [Serializable]
    public class PcData
    {
        /// <summary>
        ///     进程ID
        /// </summary>
        [DataMember]
        public int ProcessId { get; set; }

        /// <summary>
        ///     cpu
        /// </summary>
        [DataMember]
        public double Cpu { get; set; }

        /// <summary>
        ///     内存
        /// </summary>
        [DataMember]
        public double Mem { get; set; }

        /// <summary>
        ///     当前工作线程数
        /// </summary>
        [DataMember]
        public int ThreadCount { get; set; }
    }
}