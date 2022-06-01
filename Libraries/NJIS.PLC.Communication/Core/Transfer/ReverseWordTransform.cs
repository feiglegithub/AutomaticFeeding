//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：ReverseWordTransform.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Text;
using NJIS.PLC.Communication.BasicFramework;

namespace NJIS.PLC.Communication.Core.Transfer
{
    /// <summary>
    ///     按照字节错位的数据转换类
    /// </summary>
    public class ReverseWordTransform : ByteTransformBase
    {
        #region Constructor

        /// <summary>
        ///     实例化一个默认的对象
        /// </summary>
        public ReverseWordTransform()
        {
            DataFormat = DataFormat.ABCD;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     字符串数据是否按照字来反转
        /// </summary>
        public bool IsStringReverse { get; set; }

        #endregion

        #region Private Method

        /// <summary>
        ///     按照字节错位的方法
        /// </summary>
        /// <param name="buffer">实际的字节数据</param>
        /// <param name="index">起始字节位置</param>
        /// <param name="length">数据长度</param>
        /// <returns>处理过的数据信息</returns>
        private byte[] ReverseBytesByWord(byte[] buffer, int index, int length)
        {
            if (buffer == null) return null;

            // copy data
            var tmp = new byte[length];
            for (var i = 0; i < length; i++)
            {
                tmp[i] = buffer[index + i];
            }

            // change
            for (var i = 0; i < length / 2; i++)
            {
                var b = tmp[i * 2 + 0];
                tmp[i * 2 + 0] = tmp[i * 2 + 1];
                tmp[i * 2 + 1] = b;
            }

            return tmp;
        }

        private byte[] ReverseBytesByWord(byte[] buffer)
        {
            return ReverseBytesByWord(buffer, 0, buffer.Length);
        }

        #endregion

        #region Get Value From Bytes

        /// <summary>
        ///     从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public override short TransInt16(byte[] buffer, int index)
        {
            return base.TransInt16(ReverseBytesByWord(buffer, index, 2), 0);
        }


        /// <summary>
        ///     从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public override ushort TransUInt16(byte[] buffer, int index)
        {
            return base.TransUInt16(ReverseBytesByWord(buffer, index, 2), 0);
        }


        /// <summary>
        ///     从缓存中提取string结果，使用指定的编码
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        public override string TransString(byte[] buffer, int length, Encoding encoding)
        {
            var index = 0;
            var tmp = TransByte(buffer, index, length);

            if (IsStringReverse)
            {
                return encoding.GetString(ReverseBytesByWord(tmp)).Trim('\0').Trim();
            }

            return encoding.GetString(tmp);
        }

        #endregion

        #region Get Bytes From Value

        /// <summary>
        ///     short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(short[] values)
        {
            var buffer = base.TransByte(values);
            return ReverseBytesByWord(buffer);
        }


        /// <summary>
        ///     ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(ushort[] values)
        {
            var buffer = base.TransByte(values);
            return ReverseBytesByWord(buffer);
        }


        /// <summary>
        ///     使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(string value, Encoding encoding)
        {
            if (value == null) return null;
            var buffer = encoding.GetBytes(value);
            buffer = SoftBasic.ArrayExpandToLengthEven(buffer);
            if (IsStringReverse)
            {
                return ReverseBytesByWord(buffer);
            }

            return buffer;
        }

        #endregion
    }
}