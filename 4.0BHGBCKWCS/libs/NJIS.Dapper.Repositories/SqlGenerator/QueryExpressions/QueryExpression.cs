// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：QueryExpression.cs
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
    ///     Abstract Query Expression
    /// </summary>
    internal abstract class QueryExpression
    {
        /// <summary>
        ///     Query Expression Node Type
        /// </summary>
        public QueryExpressionType NodeType { get; set; }

        /// <summary>
        ///     Operator OR/AND
        /// </summary>
        public string LinkingOperator { get; set; }

        public override string ToString()
        {
            return $"[NodeType:{NodeType}, LinkingOperator:{LinkingOperator}]";
        }
    }
}