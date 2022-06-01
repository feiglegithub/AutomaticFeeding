//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：ReverseBytesTransform.cs
//   创建时间：2019-04-22 19:58
//   作    者：
//   说    明：
//   修改时间：2019-04-22 19:58
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Text;

/**********************************************************************************************
 * 
 *    说明：一般的转换类
 *    日期：2018年3月14日 17:05:30
 * 
 **********************************************************************************************/

namespace NJIS.PLC.Communication.Core.Transfer
{
    /// <summary>
    ///     字节倒序的转换类
    /// </summary>
    public class ReverseBytesTransform : ByteTransformBase
    {
        #region Get Value From Bytes

        /// <summary>
        ///     从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public override short TransInt16(byte[] buffer, int index)
        {
            var tmp = new byte[2];
            tmp[0] = buffer[1 + index];
            tmp[1] = buffer[0 + index];
            return BitConverter.ToInt16(tmp, 0);
        }

        /// <summary>
        ///     从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public override ushort TransUInt16(byte[] buffer, int index)
        {
            var tmp = new byte[2];
            tmp[0] = buffer[1 + index];
            tmp[1] = buffer[0 + index];
            return BitConverter.ToUInt16(tmp, 0);
        }

        /// <summary>
        ///     从缓存中提取int结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>int对象</returns>
        public override int TransInt32(byte[] buffer, int index)
        {
            var tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToInt32(ByteTransDataFormat4(tmp), 0);
        }

        /// <summary>
        ///     从缓存中提取uint结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>uint对象</returns>
        public override uint TransUInt32(byte[] buffer, int index)
        {
            var tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToUInt32(ByteTransDataFormat4(tmp), 0);
        }


        /// <summary>
        ///     从缓存中提取long结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>long对象</returns>
        public override long TransInt64(byte[] buffer, int index)
        {
            var tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToInt64(ByteTransDataFormat8(tmp), 0);
        }

        /// <summary>
        ///     从缓存中提取ulong结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ulong对象</returns>
        public override ulong TransUInt64(byte[] buffer, int index)
        {
            var tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToUInt64(ByteTransDataFormat8(tmp), 0);
        }

        /// <summary>
        ///     从缓存中提取float结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>float对象</returns>
        public override float TransSingle(byte[] buffer, int index)
        {
            var tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToSingle(ByteTransDataFormat4(tmp), 0);
        }


        /// <summary>
        ///     从缓存中提取double结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>double对象</returns>
        public override double TransDouble(byte[] buffer, int index)
        {
            var tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToDouble(ByteTransDataFormat8(tmp), 0);
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
            if (values == null) return null;

            var buffer = new byte[values.Length * 2];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                tmp.CopyTo(buffer, 2 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(ushort[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 2];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                tmp.CopyTo(buffer, 2 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     int数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(int[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 4];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     uint数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(uint[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 4];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     long数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(long[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 8];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     ulong数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(ulong[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 8];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }

        /// <summary>
        ///     float数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(float[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 4];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }


        /// <summary>
        ///     double数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(double[] values)
        {
            if (values == null) return null;

            var buffer = new byte[values.Length * 8];
            for (var i = 0; i < values.Length; i++)
            {
                var tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte(string value, Encoding encoding)
        {
            // S71500 字符串存在两个位置的偏差
            // 第一位：表示字符串最大大小
            // 第二位：表示实际大小
            if (value == null) return null;
            var temp = encoding.GetBytes(value);
            var datas = new byte[temp.Length + 2];
            //datas[0] = 0xff;
            datas[0] = (byte)temp.Length;
            datas[1] = (byte)temp.Length;
            Array.Copy(temp, 0, datas, 2, temp.Length);
            return datas;
        }
        #endregion
    }
}