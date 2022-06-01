// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlGenerator.GetUpdate.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:29
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NJIS.Dapper.Repositories.Extensions;

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        public virtual SqlQuery GetUpdate(TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) &&
                !p.IgnoreUpdate).ToArray();
            if (!properties.Any())
                throw new ArgumentException("Can't update without [Key]");

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder.Append(" WHERE ");

            query.SqlBuilder.Append(string.Join(" AND ", KeySqlProperties.Where(p => !p.IgnoreUpdate)
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            return query;
        }


        public virtual SqlQuery GetUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) &&
                !p.IgnoreUpdate).ToArray();

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder
                .Append(" ");

            AppendWherePredicateQuery(query, predicate, QueryType.Update);

            var parameters = new Dictionary<string, object>();
            var entityType = entity.GetType();
            foreach (var property in properties)
                parameters.Add(property.PropertyName,
                    entityType.GetProperty(property.PropertyName).GetValue(entity, null));

            if (query.Param is Dictionary<string, object> whereParam)
                parameters.AddRange(whereParam);

            query.SetParam(parameters);

            return query;
        }
    }
}