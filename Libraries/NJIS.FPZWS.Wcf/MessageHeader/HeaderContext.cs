// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：HeaderContext.cs
//  创建时间：2017-12-29 14:06
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Wcf.MessageHeader
{
    /// <summary>
    ///     自定义消息头实体
    /// </summary>
    public class HeaderContext
    {
        /// <summary>
        ///     会话唯一性标识
        /// </summary>
        public string CorrelationState { get; set; }

        /// <summary>
        ///     根id
        /// </summary>
        public string RootId { get; set; }

        /// <summary>
        ///     父id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        ///     客户端ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///     消费机应用名称
        /// </summary>
        public string AppName { get; set; }
    }
}