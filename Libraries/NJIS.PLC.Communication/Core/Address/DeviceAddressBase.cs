//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：DeviceAddressBase.cs
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
    ///     所有设备通信类的地址基础类
    /// </summary>
    public class DeviceAddressBase
    {
        /// <summary>
        ///     起始地址
        /// </summary>
        public ushort Address { get; set; }


        /// <summary>
        ///     解析字符串的地址
        /// </summary>
        /// <param name="address">地址信息</param>
        public virtual void Parse(string address)
        {
            Address = ushort.Parse(address);
        }


        /// <summary>
        ///     返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString()
        {
            return Address.ToString();
        }
    }
}