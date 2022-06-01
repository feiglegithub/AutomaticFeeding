// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：HeaderOperater.cs
//  创建时间：2017-12-29 14:06
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;

#endregion

namespace NJIS.FPZWS.Wcf.MessageHeader
{
    /// <summary>
    ///     消息头操作对象
    /// </summary>
    public class HeaderOperater
    {
        #region 自定义消息头 internal

        /// <summary>
        ///     获取客户端消息头
        /// </summary>
        /// <returns></returns>
        internal static System.ServiceModel.Channels.MessageHeader GetClientWcfHeader()
        {
            var msgheader = (System.ServiceModel.Channels.MessageHeader) CallContext.LogicalGetData("msgheader");
            return msgheader;
        }

        /// <summary>
        ///     删除消息头
        /// </summary>
        /// <param name="headers"></param>
        internal static void RemoveHeader(MessageHeaders headers)
        {
            if (headers.FindHeader("HeaderContext", "session") >= 0)
                headers.RemoveAt(headers.FindHeader("HeaderContext", "session"));
        }

        internal static void AddHeader(MessageHeaders headers, System.ServiceModel.Channels.MessageHeader hContext)
        {
            headers.Add(hContext);
        }

        #endregion

        #region 自定义消息头 public

        /// <summary>
        ///     获取服务器端消息头
        /// </summary>
        /// <returns></returns>
        public static HeaderContext GetServiceWcfHeader(OperationContext context)
        {
            if (context == null)
                return null;

            if (context.IncomingMessageHeaders.FindHeader("HeaderContext", "session") >= 0)
                return context.IncomingMessageHeaders.GetHeader<HeaderContext>("HeaderContext", "session");
            return null;
        }

        /// <summary>
        ///     创建客户端消息头
        /// </summary>
        public static void SetClientWcfHeader(HeaderContext context)
        {
            if (context == null)
            {
                CallContext.LogicalSetData("msgheader", null);
                return;
            }
            var msgheader =
                System.ServiceModel.Channels.MessageHeader.CreateHeader("HeaderContext", "session", context);
            //MessageHeader msgheader = MessageHeader.CreateHeader("HeaderContext", "session", context);
            CallContext.LogicalSetData("msgheader", msgheader);
        }

        /// <summary>
        ///     删除客户端消息头信息
        /// </summary>
        public static void ClearClientWcfHeader()
        {
            CallContext.LogicalSetData("msgheader", null);
        }

        #endregion

        #region wcf应用程序名

        /// <summary>
        ///     获取客户端消息头
        /// </summary>
        /// <returns></returns>
        internal static System.ServiceModel.Channels.MessageHeader GetClientWcfAppNameHeader()
        {
            var msgheader = (System.ServiceModel.Channels.MessageHeader) CallContext.LogicalGetData("appnamemsgheader");
            return msgheader;
        }

        /// <summary>
        ///     删除消息头
        /// </summary>
        /// <param name="headers"></param>
        internal static void RemoveAppNameHeader(MessageHeaders headers)
        {
            if (headers.FindHeader("AppNameHeaderContext", "appnamesession") >= 0)
                headers.RemoveAt(headers.FindHeader("AppNameHeaderContext", "appnamesession"));
        }

        internal static void AddAppNameHeader(MessageHeaders headers,
            System.ServiceModel.Channels.MessageHeader hContext)
        {
            headers.Add(hContext);
        }

        /// <summary>
        ///     获取服务器端消息头
        /// </summary>
        /// <returns></returns>
        public static string GetServiceWcfAppNameHeader(OperationContext context)
        {
            if (context == null)
                return null;

            if (context.IncomingMessageHeaders.FindHeader("AppNameHeaderContext", "appnamesession") >= 0)
                return context.IncomingMessageHeaders.GetHeader<string>("AppNameHeaderContext", "appnamesession");
            return null;
        }

        /// <summary>
        ///     创建客户端消息头
        /// </summary>
        internal static void SetClientWcfAppNameHeader(string context)
        {
            if (context == null)
            {
                CallContext.LogicalSetData("appnamemsgheader", null);
                return;
            }
            var msgheader =
                System.ServiceModel.Channels.MessageHeader.CreateHeader("AppNameHeaderContext", "appnamesession",
                    context);
            CallContext.LogicalSetData("appnamemsgheader", msgheader);
        }

        /// <summary>
        ///     删除客户端消息头信息
        /// </summary>
        public static void ClearClientWcfAppNameHeader()
        {
            CallContext.LogicalSetData("appnamemsgheader", null);
        }

        #endregion
    }
}