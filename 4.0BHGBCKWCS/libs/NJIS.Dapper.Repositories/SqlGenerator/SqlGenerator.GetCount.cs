// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlGenerator.GetCount.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 8:40
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        public virtual SqlQuery GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlQuery = new SqlQuery();

            sqlQuery.SqlBuilder
                .Append("SELECT COUNT(*) FROM ")
                .Append(TableName)
                .Append(" ");

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select);

            return sqlQuery;
        }


        public virtual SqlQuery GetCount(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> distinctField)
        {
            var propertyName = ExpressionHelper.GetPropertyName(distinctField);
            var property = SqlProperties.First(x => x.PropertyName == propertyName);
            var sqlQuery = InitBuilderCountWithDistinct(property);

            sqlQuery.SqlBuilder
                .Append(" FROM ")
                .Append(TableName)
                .Append(" ");

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select);

            return sqlQuery;
        }

        private SqlQuery InitBuilderCountWithDistinct(SqlPropertyMetadata sqlProperty)
        {
            var query = new SqlQuery();
            query.SqlBuilder.Append("SELECT COUNT(DISTINCT ");

            query.SqlBuilder
                .Append(TableName)
                .Append(".")
                .Append(sqlProperty.ColumnName)
                .Append(")");

            if (sqlProperty.Alias != null)
                query.SqlBuilder
                    .Append(" AS ")
                    .Append(sqlProperty.PropertyName);

            return query;
        }
    }
}