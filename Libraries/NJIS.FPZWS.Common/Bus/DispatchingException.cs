// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：DispatchingException.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:34
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace NJIS.FPZWS.Common.Bus
{
    /// <summary>
    ///     Represents the errors occur when dispatching the messages.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class DispatchingException : Exception
    {
        #region Ctor

        /// <summary>
        ///     Initializes a new instance of the <c>DispatcherException</c> class.
        /// </summary>
        public DispatchingException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>DispatcherException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DispatchingException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>DispatcherException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public DispatchingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>DispatcherException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public DispatchingException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        #endregion
    }
}