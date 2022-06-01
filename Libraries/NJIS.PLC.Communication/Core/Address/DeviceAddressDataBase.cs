//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：DeviceAddressDataBase.cs
//   创建时间：2019-08-19 12:35
//   作    者：
//   说    明：
//   修改时间：2019-08-19 12:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Core.Address
{
    /// <summary>
    ///     设备地址数据的信息，通常包含起始地址，数据类型，长度
    /// </summary>
    public class DeviceAddressDataBase
    {
        /// <summary>
        ///     数字的起始地址，也就是偏移地址
        /// </summary>
        public int AddressStart { get; set; }

        /// <summary>
        ///     读取的数据长度
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        ///     从指定的地址信息解析成真正的设备地址信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public virtual void Parse(string address, ushort length)
        {
        }
    }
}