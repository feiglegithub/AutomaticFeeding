// ************************************************************************************
//  解决方案：NJIS.FPZWS.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：Singleton.cs
//  创建时间：2017-10-19 8:17
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.MqttClient.Setting
{
    /// <summary>
    ///     单列模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        //public static readonly T Instance = new T();

        #region Singleton

        public static T Current
        {
            get { return Nested.Instance; }
        }

        private sealed class Nested
        {
            internal static readonly T Instance = (T) Activator.CreateInstance(typeof(T), true);

            static Nested()
            {
            }
        }

        #endregion
    }
}