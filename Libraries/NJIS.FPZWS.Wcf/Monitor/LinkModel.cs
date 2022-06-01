// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：LinkModel.cs
//  创建时间：2017-12-29 14:20
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace NJIS.FPZWS.Wcf.Monitor
{
    /// <summary>
    ///     wcf链接监控信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class LinkModel
    {
        /// <summary>
        ///     链接的客户端IP
        /// </summary>
        [DataMember]
        public string ClientIp { get; set; }

        /// <summary>
        ///     请求的地址列表
        /// </summary>
        [DataMember]
        public Dictionary<string, UrlInfo> UrlInfoList { get; set; }
    }

    /// <summary>
    /// </summary>
    [DataContract]
    [Serializable]
    public class UrlInfo
    {
        /// <summary>
        ///     当前链接数
        /// </summary>
        [DataMember]
        public int ConnNums { get; set; }

        /// <summary>
        ///     调用次数列表
        ///     key:操作名
        ///     value:操作次数
        /// </summary>
        [DataMember]
        public Dictionary<string, long> OperateNums { get; set; }
    }
}