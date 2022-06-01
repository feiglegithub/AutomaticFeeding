// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：QueryParameterExpression.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 8:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.Dapper.Repositories.SqlGenerator.QueryExpressions
{
    /// <summary>
    ///     Class that models the data structure in coverting the expression tree into SQL and Params.
    ///     `Parameter` Query Expression
    /// </summary>
    internal class QueryParameterExpression : QueryExpression
    {
        public QueryParameterExpression()
        {
            NodeType = QueryExpressionType.Parameter;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryParameterExpression " /> class.
        /// </summary>
        /// <param name="linkingOperator">The linking operator.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="nestedProperty">Signilize if it is nested property.</param>
        internal QueryParameterExpression(string linkingOperator,
            string propertyName, object propertyValue,
            string queryOperator, bool nestedProperty) : this()
        {
            LinkingOperator = linkingOperator;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            QueryOperator = queryOperator;
            NestedProperty = nestedProperty;
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public string QueryOperator { get; set; }
        public bool NestedProperty { get; set; }

        public override string ToString()
        {
            return
                $"[{base.ToString()}, PropertyName:{PropertyName}, PropertyValue:{PropertyValue}, QueryOperator:{QueryOperator}, NestedProperty:{NestedProperty}]";
        }
    }
}