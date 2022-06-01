// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IServiceCollection.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     定义服务映射信息集合，用于装载注册类型映射的描述信息
    /// </summary>
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        /// <summary>
        ///     克隆创建当前集合的副本
        /// </summary>
        /// <returns></returns>
        IServiceCollection Clone();
    }
}