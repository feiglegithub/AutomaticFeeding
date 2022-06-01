// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IIocBuilder.cs
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
    ///     定义依赖注入构建器，解析依赖注入服务映射信息进行构建
    /// </summary>
    public interface IIocBuilder
    {
        /// <summary>
        ///     获取 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        ///     开始构建依赖注入映射
        /// </summary>
        /// <returns>服务提供者</returns>
        IServiceProvider Build();
    }
}