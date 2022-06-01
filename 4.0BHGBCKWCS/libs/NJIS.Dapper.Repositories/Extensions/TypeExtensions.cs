// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：TypeExtensions.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

#endregion

namespace NJIS.Dapper.Repositories.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> ReflectionPropertyCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] FindClassProperties(this Type objectType)
        {
            if (ReflectionPropertyCache.ContainsKey(objectType))
                return ReflectionPropertyCache[objectType];

            var result = objectType.GetProperties().ToArray();

            ReflectionPropertyCache.TryAdd(objectType, result);

            return result;
        }


        public static bool IsGenericType(this Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        public static bool IsBool(this Type type)
        {
            return type == typeof(bool);
        }
    }
}