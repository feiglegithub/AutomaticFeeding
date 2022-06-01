// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：DictionaryExtensionMethodClass.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace NJIS.FPZWS.Common
{
    public static class DictionaryExtensionMethodClass
    {
        /// <summary>
        ///     尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue value)
        {
            if (dict.ContainsKey(key) == false)
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        ///     将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrPeplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        ///     获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        ///     向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }

            return dict;
        }

        #region Dictionary<TaskQueue, DateTime> 专用

        /// <summary>
        ///     将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换        此扩展方法适用于 Equals 被重写的 Tkey, 如 Dictionary<TaskQueue, DateTime>
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrPeplaceTasks<TKey, TValue>(this Dictionary<TKey, TValue> dict,
            TKey key, TValue value)
        {
            var tk = dict.Keys.FirstOrDefault(m => m.GetHashCode().Equals(key.GetHashCode()));
            if (tk != null)
            {
                dict.Remove(tk);
            }

            dict[key] = value;
            return dict;
        }

        /// <summary>
        ///     获取与指定的键相关联的值，如果没有则返回输入的默认值        此扩展方法适用于 Equals 被重写的 Tkey, 如 Dictionary<TaskQueue, DateTime>
        /// </summary>
        public static TValue GetValueTasks<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue)
        {
            var tk = dict.Keys.FirstOrDefault(m => m.GetHashCode().Equals(key.GetHashCode()));

            return tk != null ? dict[tk] : defaultValue;
        }

        #endregion
    }
}