// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：CollectionExtensions.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2019-08-30 8:47
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System.Collections.Generic;

namespace NJIS.Dapper.Repositories.Extensions
{
    internal static class CollectionExtensions
    {
        /// <summary>
        ///     AddRange ICollection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="addCollection"></param>
        public static void AddRange<TInput>(this ICollection<TInput> collection, IEnumerable<TInput> addCollection)
        {
            if (collection == null || addCollection == null)
                return;

            foreach (var item in addCollection)
            {
                collection.Add(item);
            }
        }
    }
}