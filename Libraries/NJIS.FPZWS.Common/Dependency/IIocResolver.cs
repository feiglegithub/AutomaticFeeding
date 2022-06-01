// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IIocResolver.cs
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

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     依赖注入对象解析获取器
    /// </summary>
    public interface IIocResolver
    {
        /// <summary>
        ///     获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        T Resolve<T>();

        ///// <summary>
        ///// 使用参数获取指定类型的实例
        ///// </summary>
        ///// <typeparam name="T">类型</typeparam>
        ///// <param name="args">参数</param>
        ///// <returns></returns>
        //T Resolve<T>(params KeyValuePair<string, object>[] args);

        /// <summary>
        ///     获取指定类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        object Resolve(Type type);

        ///// <summary>

        ///// 使用参数获取指定类型的实例
        ///// </summary>
        ///// <param name="type">类型</param>
        ///// <param name="args">参数</param>
        //object Resolve(Type type, params KeyValuePair<string, object>[] args);
    }
}