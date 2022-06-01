using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using NJIS.Common.Data;

namespace NJIS.Common.Extenssion
{
    /// <summary>
    ///     扩展方法
    /// </summary>
    public static class Extenssions
    {
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperties().FirstOrDefault(m => m.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            object obj1;
            if ((object)propertyInfo == null)
            {
                obj1 = null;
            }
            else
            {
                object obj2 = obj;
                obj1 = propertyInfo.GetValue(obj2);
            }
            return (T)obj1;
        }

        /// <summary>
        ///     从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            var descripts = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return descripts.FirstOrDefault() as T;
        }


        /// <summary>
        /// 获取枚举变量值的 Description 属性
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string ToDescription(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                var enumType = obj.GetType();
                var fi = enumType.GetField(Enum.GetName(enumType, obj));
                var dna = (DescriptionAttribute) Attribute.GetCustomAttribute(
                    fi, typeof(DescriptionAttribute));
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
                // ignored
            }
            return obj.ToString();
        }

        /// <summary>
        ///     从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        ///     获取成员元数据的Description特性描述信息
        ///     支持
        ///     DescriptionAttribute、DisplayNameAttribute、DisplayAttribute
        ///     优先级
        ///     DescriptionAttribute、DisplayNameAttribute、DisplayAttribute
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string ToDescription(this MemberInfo member, bool inherit = false)
        {
            var desc = member.GetAttribute<DescriptionAttribute>(inherit);
            if (desc != null)
            {
                return desc.Description;
            }
            var displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            var display = member.GetAttribute<DisplayAttribute>(inherit);
            if (display != null)
            {
                return display.Name;
            }
            return member.Name;
        }

        public static string ToDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var memInfo = type.GetMember(enumeration.ToString());
            if (memInfo.Length > 0)
            {
                return memInfo[0].ToDescription();
            }
            return enumeration.ToString();
        }

        /// <summary>
        ///     将业务操作结果转ajax操作结果
        /// </summary>
        public static AjaxResult ToAjaxResult<T>(this OperationResult<T> result)
        {
            var content = result.Message ?? result.ResultType.ToDescription();
            AjaxResultType type = result.ResultType.ToAjaxResultType();
            return new AjaxResult(content, type, result.Data);
        }

        /// <summary>
        ///     将业务操作结果转ajax操作结果
        /// </summary>
        public static AjaxResult ToAjaxResult(this OperationResult result)
        {
            var content = result.Message ?? result.ResultType.ToDescription();
            AjaxResultType type = result.ResultType.ToAjaxResultType();
            return new AjaxResult(content, type);
        }

        /// <summary>
        ///     把业务结果类型<see cref="OperationResultType" />转换为Ajax结果类型<see cref="AjaxResultType" />
        /// </summary>
        public static AjaxResultType ToAjaxResultType(this OperationResultType resultType)
        {
            switch (resultType)
            {
                case OperationResultType.Success:
                    return AjaxResultType.Success;
                case OperationResultType.NoChanged:
                    return AjaxResultType.Info;
                default:
                    return AjaxResultType.Error;
            }
        }

        /// <summary>
        ///     判断业务结果类型是否是Error结果
        /// </summary>
        public static bool IsError(this OperationResultType resultType)
        {
            return resultType == OperationResultType.QueryNull || resultType == OperationResultType.ValidError
                   || resultType == OperationResultType.Error;
        }
    }
}