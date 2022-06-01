//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.PLC.Communication
//   文 件 名：OmronFinsDataType.cs
//   创建时间：2018-11-08 16:14
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Profinet.Omron
{
    /// <summary>
    ///     欧姆龙的Fins协议的数据类型
    /// </summary>
    public class OmronFinsDataType
    {
        /// <summary>
        ///     DM Area
        /// </summary>
        public static readonly OmronFinsDataType DM = new OmronFinsDataType(0x02, 0x82);

        /// <summary>
        ///     CIO Area
        /// </summary>
        public static readonly OmronFinsDataType CIO = new OmronFinsDataType(0x30, 0xB0);

        /// <summary>
        ///     Work Area
        /// </summary>
        public static readonly OmronFinsDataType WR = new OmronFinsDataType(0x31, 0xB1);

        /// <summary>
        ///     Holding Bit Area
        /// </summary>
        public static readonly OmronFinsDataType HR = new OmronFinsDataType(0x32, 0xB2);

        /// <summary>
        ///     Auxiliary Bit Area
        /// </summary>
        public static readonly OmronFinsDataType AR = new OmronFinsDataType(0x33, 0xB3);

        /// <summary>
        ///     实例化一个Fins的数据类型
        /// </summary>
        /// <param name="bitCode">进行位操作的指令</param>
        /// <param name="wordCode">进行字操作的指令</param>
        public OmronFinsDataType(byte bitCode, byte wordCode)
        {
            BitCode = bitCode;
            WordCode = wordCode;
        }


        /// <summary>
        ///     进行位操作的指令
        /// </summary>
        public byte BitCode { get; }

        /// <summary>
        ///     进行字操作的指令
        /// </summary>
        public byte WordCode { get; }
    }
}