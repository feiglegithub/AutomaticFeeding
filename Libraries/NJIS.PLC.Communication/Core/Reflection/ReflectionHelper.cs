//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：ReflectionHelper.cs
//   创建时间：2019-08-19 13:14
//   作    者：
//   说    明：
//   修改时间：2019-08-19 13:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Linq.Expressions;
using System.Reflection;
using NJIS.PLC.Communication.Core.Net;
using NJIS.PLC.Communication.Core.Types;

namespace NJIS.PLC.Communication.Core.Reflection
{
    /// <summary>
    ///     反射的辅助类
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        ///     从设备里读取支持Hsl特性的数据内容，该特性为<see cref="DeviceAddressAttribute" />，详细参考论坛的操作说明。
        /// </summary>
        /// <typeparam name="T">自定义的数据类型对象</typeparam>
        /// <param name="readWrite">读写接口的实现</param>
        /// <returns>包含是否成功的结果对象</returns>
        public static OperateResult<T> Read<T>(IReadWriteNet readWrite) where T : class, new()
        {
            var type = typeof(T);
            // var constrcuor = type.GetConstructors( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic );
            var obj = type.Assembly.CreateInstance(type.FullName);

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(DeviceAddressAttribute), false);
                if (attribute == null) continue;

                DeviceAddressAttribute hAttribute = null;
                for (var i = 0; i < attribute.Length; i++)
                {
                    var tmp = (DeviceAddressAttribute) attribute[i];
                    if (tmp.deviceType != null && tmp.deviceType == readWrite.GetType())
                    {
                        hAttribute = tmp;
                        break;
                    }
                }

                if (hAttribute == null)
                {
                    for (var i = 0; i < attribute.Length; i++)
                    {
                        var tmp = (DeviceAddressAttribute) attribute[i];
                        if (tmp.deviceType == null)
                        {
                            hAttribute = tmp;
                            break;
                        }
                    }
                }

                if (hAttribute == null) continue;

                var propertyType = property.PropertyType;
                if (propertyType == typeof(short))
                {
                    var valueResult = readWrite.ReadInt16(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(short[]))
                {
                    var valueResult = readWrite.ReadInt16(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(ushort))
                {
                    var valueResult = readWrite.ReadUInt16(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(ushort[]))
                {
                    var valueResult = readWrite.ReadUInt16(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(int))
                {
                    var valueResult = readWrite.ReadInt32(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(int[]))
                {
                    var valueResult = readWrite.ReadInt32(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(uint))
                {
                    var valueResult = readWrite.ReadUInt32(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(uint[]))
                {
                    var valueResult = readWrite.ReadUInt32(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(long))
                {
                    var valueResult = readWrite.ReadInt64(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(long[]))
                {
                    var valueResult = readWrite.ReadInt64(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(ulong))
                {
                    var valueResult = readWrite.ReadUInt64(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(ulong[]))
                {
                    var valueResult = readWrite.ReadUInt64(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(float))
                {
                    var valueResult = readWrite.ReadFloat(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(float[]))
                {
                    var valueResult = readWrite.ReadFloat(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(double))
                {
                    var valueResult = readWrite.ReadDouble(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(double[]))
                {
                    var valueResult = readWrite.ReadDouble(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(string))
                {
                    var valueResult = readWrite.ReadString(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(byte[]))
                {
                    var valueResult = readWrite.Read(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(bool))
                {
                    var valueResult = readWrite.ReadBool(hAttribute.address);
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
                else if (propertyType == typeof(bool[]))
                {
                    var valueResult = readWrite.ReadBool(hAttribute.address,
                        (ushort) (hAttribute.length > 0 ? hAttribute.length : 1));
                    if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                    property.SetValue(obj, valueResult.Content, null);
                }
            }

            return OperateResult.CreateSuccessResult((T) obj);
        }


        /// <summary>
        ///     从设备里读取支持Hsl特性的数据内容，该特性为<see cref="DeviceAddressAttribute" />，详细参考论坛的操作说明。
        /// </summary>
        /// <typeparam name="T">自定义的数据类型对象</typeparam>
        /// <param name="data">自定义的数据对象</param>
        /// <param name="readWrite">数据读写对象</param>
        /// <returns>包含是否成功的结果对象</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OperateResult Write<T>(T data, IReadWriteNet readWrite) where T : class, new()
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var type = typeof(T);
            var obj = data;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(DeviceAddressAttribute), false);
                if (attribute == null) continue;

                DeviceAddressAttribute hAttribute = null;
                for (var i = 0; i < attribute.Length; i++)
                {
                    var tmp = (DeviceAddressAttribute) attribute[i];
                    if (tmp.deviceType != null && tmp.deviceType == readWrite.GetType())
                    {
                        hAttribute = tmp;
                        break;
                    }
                }

                if (hAttribute == null)
                {
                    for (var i = 0; i < attribute.Length; i++)
                    {
                        var tmp = (DeviceAddressAttribute) attribute[i];
                        if (tmp.deviceType == null)
                        {
                            hAttribute = tmp;
                            break;
                        }
                    }
                }

                if (hAttribute == null) continue;


                var propertyType = property.PropertyType;
                if (propertyType == typeof(short))
                {
                    var value = (short) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(short[]))
                {
                    var value = (short[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(ushort))
                {
                    var value = (ushort) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(ushort[]))
                {
                    var value = (ushort[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(int))
                {
                    var value = (int) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(int[]))
                {
                    var value = (int[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(uint))
                {
                    var value = (uint) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(uint[]))
                {
                    var value = (uint[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(long))
                {
                    var value = (long) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(long[]))
                {
                    var value = (long[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(ulong))
                {
                    var value = (ulong) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(ulong[]))
                {
                    var value = (ulong[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(float))
                {
                    var value = (float) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(float[]))
                {
                    var value = (float[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(double))
                {
                    var value = (double) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(double[]))
                {
                    var value = (double[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(string))
                {
                    var value = (string) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(byte[]))
                {
                    var value = (byte[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(bool))
                {
                    var value = (bool) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
                else if (propertyType == typeof(bool[]))
                {
                    var value = (bool[]) property.GetValue(obj, null);

                    var writeResult = readWrite.Write(hAttribute.address, value);
                    if (!writeResult.IsSuccess) return writeResult;
                }
            }

            return OperateResult.CreateSuccessResult(obj);
        }

        /// <summary>
        ///     使用表达式树的方式来给一个属性赋值
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <param name="obj">对象信息</param>
        /// <param name="objValue">实际的值</param>
        public static void SetPropertyExp<T, K>(PropertyInfo propertyInfo, T obj, K objValue)
        {
            // propertyInfo.SetValue( obj, objValue, null );  下面就是实现这句话
            var invokeObjExpr = Expression.Parameter(typeof(T), "obj");
            var propValExpr = Expression.Parameter(propertyInfo.PropertyType, "objValue");
            var setMethodExp = Expression.Call(invokeObjExpr, propertyInfo.GetSetMethod(), propValExpr);
            var lambda = Expression.Lambda<Action<T, K>>(setMethodExp, invokeObjExpr, propValExpr);
            lambda.Compile()(obj, objValue);
        }
    }
}