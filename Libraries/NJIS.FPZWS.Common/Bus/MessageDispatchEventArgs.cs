// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：MessageDispatchEventArgs.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:37
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;

namespace NJIS.FPZWS.Common.Bus
{
    /// <summary>
    ///     分派消息数据.
    /// </summary>
    public class MessageDispatchEventArgs : EventArgs
    {
        #region Public Properties

        public dynamic Message { get; set; }

        public Type HandlerType { get; set; }

        public object Handler { get; set; }

        #endregion

        #region Ctor

        public MessageDispatchEventArgs()
        {
        }

        public MessageDispatchEventArgs(dynamic message, Type handlerType, object handler)
        {
            Message = message;
            HandlerType = handlerType;
            Handler = handler;
        }

        #endregion
    }
}