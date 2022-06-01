// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IInitializer.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Common.Initialize
{
    /// <summary>
    ///     Definition initialization
    /// </summary>
    public interface IInitializer
    {
        /// <summary>
        ///     初始化器级
        /// </summary>
        InitializeLevel Level { get; }

        void Initializer(IConfig dgConfig);
    }

    public enum InitializeLevel
    {
        Low = 0,
        Middle = 2,
        High = 3
    }
}