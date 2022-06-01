//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.PLC.Communication
//   文 件 名：MelsecMcDataType.cs
//   创建时间：2018-11-08 16:14
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Profinet.Melsec
{
    /// <summary>
    ///     三菱PLC的数据类型，此处包含了几个常用的类型
    /// </summary>
    public class MelsecMcDataType
    {
        /// <summary>
        ///     X输入寄存器
        /// </summary>
        public static readonly MelsecMcDataType X = new MelsecMcDataType(0x9C, 0x01, "X*", 16);

        /// <summary>
        ///     Y输出寄存器
        /// </summary>
        public static readonly MelsecMcDataType Y = new MelsecMcDataType(0x9D, 0x01, "Y*", 16);

        /// <summary>
        ///     M中间寄存器
        /// </summary>
        public static readonly MelsecMcDataType M = new MelsecMcDataType(0x90, 0x01, "M*", 10);

        /// <summary>
        ///     D数据寄存器
        /// </summary>
        public static readonly MelsecMcDataType D = new MelsecMcDataType(0xA8, 0x00, "D*", 10);

        /// <summary>
        ///     W链接寄存器
        /// </summary>
        public static readonly MelsecMcDataType W = new MelsecMcDataType(0xB4, 0x00, "W*", 16);

        /// <summary>
        ///     L锁存继电器
        /// </summary>
        public static readonly MelsecMcDataType L = new MelsecMcDataType(0x92, 0x01, "L*", 10);

        /// <summary>
        ///     F报警器
        /// </summary>
        public static readonly MelsecMcDataType F = new MelsecMcDataType(0x93, 0x01, "F*", 10);

        /// <summary>
        ///     V边沿继电器
        /// </summary>
        public static readonly MelsecMcDataType V = new MelsecMcDataType(0x94, 0x01, "V*", 10);

        /// <summary>
        ///     B链接继电器
        /// </summary>
        public static readonly MelsecMcDataType B = new MelsecMcDataType(0xA0, 0x01, "B*", 16);

        /// <summary>
        ///     R文件寄存器
        /// </summary>
        public static readonly MelsecMcDataType R = new MelsecMcDataType(0xAF, 0x00, "R*", 10);

        /// <summary>
        ///     S步进继电器
        /// </summary>
        public static readonly MelsecMcDataType S = new MelsecMcDataType(0x98, 0x01, "S*", 10);

        /// <summary>
        ///     变址寄存器
        /// </summary>
        public static readonly MelsecMcDataType Z = new MelsecMcDataType(0xCC, 0x00, "Z*", 10);

        /// <summary>
        ///     定时器的值
        /// </summary>
        public static readonly MelsecMcDataType T = new MelsecMcDataType(0xC2, 0x00, "TN", 10);

        /// <summary>
        ///     计数器的值
        /// </summary>
        public static readonly MelsecMcDataType C = new MelsecMcDataType(0xC5, 0x00, "CN", 10);

        /// <summary>
        ///     文件寄存器ZR区
        /// </summary>
        public static readonly MelsecMcDataType ZR = new MelsecMcDataType(0xB0, 0x00, "ZR", 16);

        /// <summary>
        ///     如果您清楚类型代号，可以根据值进行扩展
        /// </summary>
        /// <param name="code">数据类型的代号</param>
        /// <param name="type">0或1，默认为0</param>
        /// <param name="asciiCode">ASCII格式的类型信息</param>
        /// <param name="fromBase">指示地址的多少进制的，10或是16</param>
        public MelsecMcDataType(byte code, byte type, string asciiCode, int fromBase)
        {
            DataCode = code;
            AsciiCode = asciiCode;
            FromBase = fromBase;
            if (type < 2) DataType = type;
        }

        /// <summary>
        ///     类型的代号值
        /// </summary>
        public byte DataCode { get; }

        /// <summary>
        ///     数据的类型，0代表按字，1代表按位
        /// </summary>
        public byte DataType { get; }

        /// <summary>
        ///     当以ASCII格式通讯时的类型描述
        /// </summary>
        public string AsciiCode { get; }

        /// <summary>
        ///     指示地址是10进制，还是16进制的
        /// </summary>
        public int FromBase { get; }
    }
}