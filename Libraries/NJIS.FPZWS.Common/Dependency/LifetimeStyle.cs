// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：LifetimeStyle.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Common.Dependency
{
    /// <summary>
    ///     表示依赖注入的对象生命周期
    /// </summary>
    public enum LifetimeStyle
    {
        /// <summary>
        ///     实时模式，每次获取都创建不同对象
        /// </summary>
        Transient,

        /// <summary>
        ///     局部模式，同一生命周期获得相同对象，不同生命周期获得不同对象（PerRequest）
        /// </summary>
        Scoped,

        /// <summary>
        ///     单例模式，在第一次获取实例时创建，之后都获得相同对象
        /// </summary>
        Singleton
    }
}