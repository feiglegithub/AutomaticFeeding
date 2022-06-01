//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.Common
//   文 件 名：ObjectExtension.cs
//   创建时间：2018-11-22 11:16
//   作    者：
//   说    明：
//   修改时间：2018-11-22 11:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Diagnostics;
using System.Linq;

namespace NJIS.FPZWS.Common
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInteger(this int obj)
        {
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInteger(this object obj)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(int), e);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static int? ToIntegerNullable(this object obj, int? nullValue = null)
        {
            if (IsNull(obj)) return nullValue;
            return ToInteger(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(DateTime), e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static DateTime? ToeDateTimeNullabl(this object obj, DateTime? nullValue = null)
        {
            if (IsNull(obj) || obj as string == "") return nullValue;
            return ToDateTime(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object obj)
        {
            try
            {
                return bool.Parse(obj.ToString());
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(bool), e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return decimal.Parse(obj.ToString());
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(decimal), e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this object obj, decimal? nullValue = null)
        {
            if (IsNull(obj)) return nullValue;
            return ToDecimal(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj)
        {
            try
            {
                return float.Parse(obj.ToString());
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(float), e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CastTo<T>(this object obj)
        {
            try
            {
                return (T) obj;
            }
            catch (Exception e)
            {
                throw new ConvertException(obj, typeof(T), e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] arguments)
        {
            return arguments.Any(e => e.Equals(t));
        }
    }

    public class ConvertException : Exception
    {
        private object _object;
        private new StackTrace _stackTrace;
        private Type Type;

        public ConvertException(object obj, Type type, Exception innerException)
            : base(
                "Не удалось преобразовать объект(" +
                (obj.IsNull() ? "<null>" : string.IsNullOrWhiteSpace(obj.ToString()) ? "<empty>" : obj) + ") к типу " +
                type.Name, innerException)
        {
            _object = obj;
            Type = type;
            _stackTrace = new StackTrace();
        }
    }
}