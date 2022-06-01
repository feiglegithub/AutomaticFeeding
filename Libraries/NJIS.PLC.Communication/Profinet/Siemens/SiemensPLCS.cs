//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：SiemensPLCS.cs
//   创建时间：2018-11-08 16:14
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Profinet.Siemens
{
    /// <summary>
    ///     西门子的PLC类型，目前支持的访问类型
    /// </summary>
    public enum SiemensPLCS
    {
        /// <summary>
        ///     1200系列
        /// </summary>
        S1200 = 1,

        /// <summary>
        ///     300系列
        /// </summary>
        S300 = 2,

        /// <summary>
        ///     400系列
        /// </summary>
        S400 = 3,

        /// <summary>
        ///     1500系列PLC
        /// </summary>
        S1500 = 4,

        /// <summary>
        ///     200的smart系列
        /// </summary>
        S200Smart = 5,

        /// <summary>
        ///     200系统，需要额外配置以太网模块
        /// </summary>
        S200 = 6
    }
}