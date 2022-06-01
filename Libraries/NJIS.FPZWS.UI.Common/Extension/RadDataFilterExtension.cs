// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：RadDataFilterExtension.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-04-30 7:37
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Linq;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.UI.Common.Extension
{
    public static class RadDataFilterExtension
    {
        /// <summary>
        ///     获取数据库查询条件
        ///     过滤器组件DataFilterDescriptorItem的Tag必填且对应数据库字段
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>数据库查询条件</returns>
        public static string GetExpressionStr(this RadDataFilter parent)
        {
            try
            {
                return parent.Descriptors.Cast<DataFilterDescriptorItem>().Aggregate(parent.Expression,
                    (current, item) => current.Replace(item.DescriptorName, item.Tag.ToString())).Replace("#", "\'");
            }
            catch
            {
                throw new Exception("控件过滤条件Tag属性必须赋值为数据库字段");
            }

            //var res = parent.Expression;
            //if (string.IsNullOrEmpty(res))
            //{
            //    return "";
            //}
            //res = res.Replace("#", "\'");
            //foreach (var radItem in parent.Descriptors)
            //{
            //    if ((string)radItem.Tag == GridViewHelper.RowNumberNameStr)
            //    {
            //        continue;
            //    }
            //    var item = (DataFilterDescriptorItem)radItem;
            //    res = res.Replace(item.DescriptorName, item.Tag.ToString());
            //}
            //return res;
        }
    }
}