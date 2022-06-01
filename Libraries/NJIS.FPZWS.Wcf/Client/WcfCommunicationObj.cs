// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Client
//  文 件 名：WcfCommunicationObj.cs
//  创建时间：2017-12-29 8:25
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.ServiceModel;

#endregion

namespace NJIS.FPZWS.Wcf.Client
{
    /// <summary>
    ///     Wcf通讯实体
    /// </summary>
    internal class WcfCommunicationObj
    {
        /// <summary>
        ///     索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     契约类型
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        ///     通讯对象
        /// </summary>
        public ICommunicationObject CommucationObject { get; set; }

        /// <summary>
        ///     是否在使用
        /// </summary>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     最后使用时间
        /// </summary>
        public DateTime LastUsedTime { get; set; }

        /// <summary>
        ///     使用次数
        /// </summary>
        public int UsedNums { get; set; }
    }
}