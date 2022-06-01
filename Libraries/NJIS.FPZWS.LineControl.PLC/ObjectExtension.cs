//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：ObjectExtension.cs
//   创建时间：2018-11-22 11:22
//   作    者：
//   说    明：
//   修改时间：2018-11-22 11:22
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    public static class ObjectExtension
    {
        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this object obj)
        {
            if (obj is bool)
            {
                return (bool) obj;
            }

            return bool.Parse(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj)
        {
            if (obj is double)
            {
                return (double) obj;
            }

            return double.Parse(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this object obj)
        {
            if (obj is int)
            {
                return (int) obj;
            }

            return int.Parse(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj)
        {
            if (obj is float)
            {
                return (float) obj;
            }

            return float.Parse(obj.ToString());
        }


        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToLong(this object obj)
        {
            if (obj is long)
            {
                return (long) obj;
            }

            return long.Parse(obj.ToString());
        }


        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static short ToShort(this object obj)
        {
            if (obj is short)
            {
                return (short) obj;
            }

            return short.Parse(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte ToByte(this object obj)
        {
            if (obj is byte)
            {
                return (byte)obj;
            }

            return byte.Parse(obj.ToString());
        }
    }
}