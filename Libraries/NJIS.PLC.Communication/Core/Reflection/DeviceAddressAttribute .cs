//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：DeviceAddressAttribute .cs
//   创建时间：2019-08-19 13:13
//   作    者：
//   说    明：
//   修改时间：2019-08-19 13:13
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.PLC.Communication.Core.Reflection
{
    /// <summary>
    ///     应用于组件库读取的动态地址解析
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DeviceAddressAttribute : Attribute
    {
        /// <summary>
        ///     实例化一个地址特性，指定地址信息
        /// </summary>
        /// <param name="address">真实的地址信息</param>
        public DeviceAddressAttribute(string address)
        {
            this.address = address;
            length = -1;
            deviceType = null;
        }

        /// <summary>
        ///     实例化一个地址特性，指定地址信息
        /// </summary>
        /// <param name="address">真实的地址信息</param>
        /// <param name="deviceType">设备的地址信息</param>
        public DeviceAddressAttribute(string address, Type deviceType)
        {
            this.address = address;
            length = -1;
            this.deviceType = deviceType;
        }

        /// <summary>
        ///     实例化一个地址特性，指定地址信息和数据长度，通常应用于数组的批量读取
        /// </summary>
        /// <param name="address">真实的地址信息</param>
        /// <param name="length">读取的数据长度</param>
        public DeviceAddressAttribute(string address, int length)
        {
            this.address = address;
            this.length = length;
            deviceType = null;
        }

        /// <summary>
        ///     实例化一个地址特性，指定地址信息和数据长度，通常应用于数组的批量读取
        /// </summary>
        /// <param name="address">真实的地址信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <param name="deviceType">设备类型</param>
        public DeviceAddressAttribute(string address, int length, Type deviceType)
        {
            this.address = address;
            this.length = length;
            this.deviceType = deviceType;
        }

        /// <summary>
        ///     设备的类似，这将决定是否使用当前的PLC地址
        /// </summary>
        public Type deviceType { get; set; }

        /// <summary>
        ///     数据的地址信息
        /// </summary>
        public string address { get; }

        /// <summary>
        ///     数据长度
        /// </summary>
        public int length { get; }
    }
}