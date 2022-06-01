// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：Singleton.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Common
{
    /// <summary>
    ///     单列模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        //public static readonly T Instance = new T();

        #region Singleton

        public static T Current => Nested.Instance;

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