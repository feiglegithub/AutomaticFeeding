// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlGenerator.GetDelete.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:26
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
        public virtual SqlQuery GetDelete(TEntity entity)
        {
            var sqlQuery = new SqlQuery();
            var whereAndSql =
                string.Join(" AND ",
                    KeySqlProperties.Select(p =>
                        string.Format("{0}.{1} = @{2}", TableName, p.ColumnName, p.PropertyName)));

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder
                    .Append("DELETE FROM ")
                    .Append(TableName)
                    .Append(" WHERE ")
                    .Append(whereAndSql);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append("UPDATE ")
                    .Append(TableName)
                    .Append(" SET ")
                    .Append(StatusPropertyName)
                    .Append(" = ")
                    .Append(LogicalDeleteValue);

                if (HasUpdatedAt)
                {
                    UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

                    sqlQuery.SqlBuilder
                        .Append(", ")
                        .Append(UpdatedAtPropertyMetadata.ColumnName)
                        .Append(" = @")
                        .Append(UpdatedAtPropertyMetadata.PropertyName);
                }

                sqlQuery.SqlBuilder
                    .Append(" WHERE ")
                    .Append(whereAndSql);
            }

            sqlQuery.SetParam(entity);
            return sqlQuery;
        }


        public virtual SqlQuery GetDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlQuery = new SqlQuery();

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder
                    .Append("DELETE FROM ")
                    .Append(TableName);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append("UPDATE ")
                    .Append(TableName)
                    .Append(" SET ")
                    .Append(StatusPropertyName)
                    .Append(" = ")
                    .Append(LogicalDeleteValue);

                if (HasUpdatedAt)
                    sqlQuery.SqlBuilder
                        .Append(", ")
                        .Append(UpdatedAtPropertyMetadata.ColumnName)
                        .Append(" = @")
                        .Append(UpdatedAtPropertyMetadata.PropertyName);
            }

            sqlQuery.SqlBuilder.Append(" ");
            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Delete);
            return sqlQuery;
        }
    }
}