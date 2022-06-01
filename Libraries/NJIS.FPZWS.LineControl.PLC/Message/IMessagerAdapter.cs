//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：IMessagerAdapter.cs
//   创建时间：2018-11-23 16:40
//   作    者：
//   说    明：
//   修改时间：2018-11-23 16:40
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Message
{
    /// <summary>
    ///     消息适配器
    /// </summary>
    public interface IMessagerAdapter
    {
        /// <summary>
        ///     由指定类型获取<see cref="IMessager" />消息实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        IMessager GetMessager(Type type);

        /// <summary>
        ///     由指定名称获取<see cref="IMessager" />消息实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        IMessager GetMessager(string name);
    }
}