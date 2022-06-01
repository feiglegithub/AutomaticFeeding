//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：DataFormat.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Core.Transfer
{
    /// <summary>
    ///     应用于多字节数据的解析或是生成格式
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        ///     按照顺序排序
        /// </summary>
        ABCD = 0,

        /// <summary>
        ///     按照单字反转
        /// </summary>
        BADC = 1,

        /// <summary>
        ///     按照双字反转
        /// </summary>
        CDAB = 2,

        /// <summary>
        ///     按照倒序排序
        /// </summary>
        DCBA = 3
    }
}