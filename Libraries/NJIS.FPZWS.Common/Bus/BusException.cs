// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：BusException.cs
//  创建时间：2018-09-06 13:44
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace NJIS.FPZWS.Common.Bus
{
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class BusException : Exception
    {
        #region Ctor

        public BusException()
        {
        }

        public BusException(string message) : base(message)
        {
        }

        public BusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BusException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        #endregion
    }
}