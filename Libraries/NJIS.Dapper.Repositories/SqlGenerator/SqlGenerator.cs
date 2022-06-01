// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlGenerator.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NJIS.Dapper.Repositories.Extensions;
using NJIS.Model.Attributes.Joins;

#endregion

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    public partial class SqlGenerator<TEntity> : ISqlGenerator<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator()
            : this(new SqlGeneratorConfig {SqlProvider = SqlProvider.MSSQL, UseQuotationMarks = false})
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlProvider sqlProvider, bool useQuotationMarks = false)
            : this(new SqlGeneratorConfig {SqlProvider = sqlProvider, UseQuotationMarks = useQuotationMarks})
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlGeneratorConfig sqlGeneratorConfig)
        {
            // Order is important
            InitProperties();
            InitConfig(sqlGeneratorConfig);
            InitLogicalDeleted();
        }

        public bool HasCreatedAt => CreatedAtProperty != null;


        public PropertyInfo CreatedAtProperty { get; protected set; }


        public PropertyInfo[] AllProperties { get; protected set; }


        public bool HasUpdatedAt => UpdatedAtProperty != null;


        public PropertyInfo UpdatedAtProperty { get; protected set; }

        public SqlPropertyMetadata UpdatedAtPropertyMetadata { get; protected set; }


        public bool IsIdentity => IdentitySqlProperty != null;


        public string TableName { get; protected set; }


        public string TableSchema { get; protected set; }


        public SqlPropertyMetadata IdentitySqlProperty { get; protected set; }


        public SqlPropertyMetadata[] KeySqlProperties { get; protected set; }


        public SqlPropertyMetadata[] SqlProperties { get; protected set; }


        public SqlJoinPropertyMetadata[] SqlJoinProperties { get; protected set; }


        public SqlGeneratorConfig Config { get; protected set; }


        public bool LogicalDelete { get; protected set; }


        public string StatusPropertyName { get; protected set; }


        public object LogicalDeleteValue { get; protected set; }


        public virtual SqlQuery GetInsert(TEntity entity)
        {
            var properties =
                (IsIdentity
                    ? SqlProperties.Where(
                        p =>
                            !p.PropertyName.Equals(IdentitySqlProperty.PropertyName,
                                StringComparison.OrdinalIgnoreCase))
                    : SqlProperties).ToList();

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.Now);
            if (HasCreatedAt)
                CreatedAtProperty.SetValue(entity, DateTime.Now);

            var query = new SqlQuery(entity);

            query.SqlBuilder.Append(
                "INSERT INTO " + TableName
                               + " ([" + string.Join("],[", properties.Select(p => p.ColumnName)) + "])" // columNames
                               + " VALUES (" + string.Join(", ", properties.Select(p => "@" + p.PropertyName)) +
                               ")"); // values

            if (IsIdentity)
                switch (Config.SqlProvider)
                {
                    case SqlProvider.MSSQL:
                        query.SqlBuilder.Append(" SELECT SCOPE_IDENTITY() AS " + IdentitySqlProperty.ColumnName);
                        break;

                    case SqlProvider.MySQL:
                        query.SqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " +
                                                IdentitySqlProperty.ColumnName);
                        break;

                    case SqlProvider.PostgreSQL:
                        query.SqlBuilder.Append(" RETURNING " + IdentitySqlProperty.ColumnName);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            return query;
        }


        public virtual SqlQuery GetBulkInsert(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties =
                (IsIdentity
                    ? SqlProperties.Where(
                        p =>
                            !p.PropertyName.Equals(IdentitySqlProperty.PropertyName,
                                StringComparison.OrdinalIgnoreCase))
                    : SqlProperties).ToList();

            var query = new SqlQuery();

            var values = new List<string>();
            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < entitiesArray.Length; i++)
            {
                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.Now);

                if (HasCreatedAt)
                    CreatedAtProperty.SetValue(entitiesArray[i], DateTime.Now);

                foreach (var property in properties)
                    parameters.Add(property.PropertyName + i,
                        entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                values.Add("(" + string.Join(", ", properties.Select(p => "@" + p.PropertyName + i)) + ")");
            }

            query.SqlBuilder.Append(
                "INSERT INTO " + TableName
                               + " (" + string.Join(", ", properties.Select(p => p.ColumnName)) + ")" // columNames
                               + " VALUES " + string.Join(",", values)); // values

            query.SetParam(parameters);

            return query;
        }

        public virtual SqlQuery GetBulkUpdate(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties =
                SqlProperties.Where(
                    p =>
                        !KeySqlProperties.Any(
                            k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) &&
                        !p.IgnoreUpdate).ToArray();

            var query = new SqlQuery();

            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < entitiesArray.Length; i++)
            {
                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.Now);

                query.SqlBuilder.Append(" UPDATE " + TableName + " SET " +
                                        string.Join(", ",
                                            properties.Select(p => p.ColumnName + " = @" + p.PropertyName + i))
                                        + " WHERE " +
                                        string.Join(" AND ",
                                            KeySqlProperties.Where(p => !p.IgnoreUpdate)
                                                .Select(p => p.ColumnName + " = @" + p.PropertyName + i)));

                foreach (var property in properties)
                {
                    parameters.Add(property.PropertyName + i,
                        entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));
                }

                foreach (var property in KeySqlProperties.Where(p => !p.IgnoreUpdate))
                {
                    parameters.Add(property.PropertyName + i,
                        entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));
                }
            }

            query.SetParam(parameters);

            return query;
        }


        public virtual SqlQuery GetDeleteAll(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlQuery = new SqlQuery();

            sqlQuery.SqlBuilder.Append("DELETE FROM " + TableName + " ");

            return AppendWhereQuery(sqlQuery, predicate);
        }

        private SqlQuery AppendWhereQuery(SqlQuery sqlQuery, Expression<Func<TEntity, bool>> predicate)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            if (predicate != null)
            {
                // WHERE
                var queryProperties = new List<QueryParameter>();
                FillQueryProperties(predicate.Body, ExpressionType.Default, ref queryProperties);

                sqlQuery.SqlBuilder.Append("WHERE ");

                for (var i = 0; i < queryProperties.Count; i++)
                {
                    var item = queryProperties[i];
                    var tableName = TableName;
                    string columnName;
                    if (item.NestedProperty)
                    {
                        var joinProperty = SqlJoinProperties.First(x => x.PropertyName == item.PropertyName);
                        tableName = joinProperty.TableName;
                        columnName = joinProperty.ColumnName;
                    }
                    else
                    {
                        columnName = SqlProperties.First(x => x.PropertyName == item.PropertyName).ColumnName;
                    }

                    if (!string.IsNullOrEmpty(item.LinkingOperator) && i > 0)
                        sqlQuery.SqlBuilder.Append(item.LinkingOperator + " ");

                    var vi = 0;
                    var pv = item.PropertyName;
                    while (dictionary.ContainsKey(pv))
                    {
                        pv = item.PropertyName + vi;
                    }

                    ;

                    if (item.PropertyValue == null)
                        sqlQuery.SqlBuilder.Append(tableName + ".[" + columnName + "] " +
                                                   (item.QueryOperator == "=" ? "IS" : "IS NOT") + " NULL ");
                    else
                        sqlQuery.SqlBuilder.Append(tableName + ".[" + columnName + "] " + item.QueryOperator + " @" +
                                                   pv + " ");
                    dictionary[pv] = item.PropertyValue;
                }

                if (LogicalDelete)
                    sqlQuery.SqlBuilder.Append("AND " + TableName + "." + StatusPropertyName + " != " +
                                               LogicalDeleteValue + " ");
            }
            else
            {
                if (LogicalDelete)
                    sqlQuery.SqlBuilder.Append("WHERE " + TableName + "." + StatusPropertyName + " != " +
                                               LogicalDeleteValue + " ");
            }

            sqlQuery.SetParam(dictionary);

            return sqlQuery;
        }

        /// <summary>
        ///     Get join/nested properties
        /// </summary>
        /// <returns></returns>
        private static SqlJoinPropertyMetadata[] GetJoinPropertyMetadata(PropertyInfo[] joinPropertiesInfo)
        {
            // Filter and get only non collection nested properties
            var singleJoinTypes = joinPropertiesInfo.Where(p => !p.PropertyType.IsConstructedGenericType).ToArray();

            var joinPropertyMetadatas = new List<SqlJoinPropertyMetadata>();

            foreach (var propertyInfo in singleJoinTypes)
            {
                var joinInnerProperties =
                    propertyInfo.PropertyType.GetProperties()
                        .Where(q => q.CanWrite)
                        .Where(ExpressionHelper.GetPrimitivePropertiesPredicate())
                        .ToArray();

                joinPropertyMetadatas.AddRange(
                    joinInnerProperties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                        .Select(p => new SqlJoinPropertyMetadata(propertyInfo, p))
                        .ToArray());
            }

            return joinPropertyMetadatas.ToArray();
        }

        private static string GetTableNameWithSchemaPrefix(string tableName, string tableSchema,
            string startQuotationMark = "", string endQuotationMark = "")
        {
            return !string.IsNullOrEmpty(tableSchema)
                ? startQuotationMark + tableSchema + endQuotationMark + "." + startQuotationMark + tableName +
                  endQuotationMark
                : startQuotationMark + tableName + endQuotationMark;
        }


        private string AppendJoinToSelect(SqlQuery originalBuilder, params Expression<Func<TEntity, object>>[] includes)
        {
            var joinBuilder = new StringBuilder();

            foreach (var include in includes)
            {
                var joinProperty = AllProperties.First(q => q.Name == ExpressionHelper.GetPropertyName(include));
                var declaringType = joinProperty.DeclaringType.GetTypeInfo();
                var tableAttribute = declaringType.GetCustomAttribute<TableAttribute>();
                var tableName = tableAttribute != null ? tableAttribute.Name : declaringType.Name;

                var attrJoin = joinProperty.GetCustomAttribute<JoinAttributeBase>();

                if (attrJoin == null)
                    continue;

                var joinString = "";
                if (attrJoin is LeftJoinAttribute)
                    joinString = "LEFT JOIN";
                else if (attrJoin is InnerJoinAttribute)
                    joinString = "INNER JOIN";
                else if (attrJoin is RightJoinAttribute)
                    joinString = "RIGHT JOIN";

                var joinType = joinProperty.PropertyType.IsGenericType()
                    ? joinProperty.PropertyType.GenericTypeArguments[0]
                    : joinProperty.PropertyType;
                var properties = joinType.FindClassProperties()
                    .Where(ExpressionHelper.GetPrimitivePropertiesPredicate());
                var props =
                    properties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                        .Select(p => new SqlPropertyMetadata(p))
                        .ToArray();

                if (Config.UseQuotationMarks)
                    switch (Config.SqlProvider)
                    {
                        case SqlProvider.MSSQL:
                            tableName = "[" + tableName + "]";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema,
                                "[", "]");
                            attrJoin.Key = "[" + attrJoin.Key + "]";
                            attrJoin.ExternalKey = "[" + attrJoin.ExternalKey + "]";
                            foreach (var prop in props)
                                prop.ColumnName = "[" + prop.ColumnName + "]";
                            break;

                        case SqlProvider.MySQL:
                            tableName = "`" + tableName + "`";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema,
                                "`", "`");
                            attrJoin.Key = "`" + attrJoin.Key + "`";
                            attrJoin.ExternalKey = "`" + attrJoin.ExternalKey + "`";
                            foreach (var prop in props)
                                prop.ColumnName = "`" + prop.ColumnName + "`";
                            break;

                        case SqlProvider.PostgreSQL:
                            tableName = "\"" + tableName + "\"";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema,
                                "\"", "\"");
                            attrJoin.Key = "\"" + attrJoin.Key + "\"";
                            attrJoin.ExternalKey = "\"" + attrJoin.ExternalKey + "\"";
                            foreach (var prop in props)
                                prop.ColumnName = "\"" + prop.ColumnName + "\"";
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(Config.SqlProvider));
                    }
                else
                    attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema);

                originalBuilder.SqlBuilder.Append(", " + GetFieldsSelect(attrJoin.TableName, props));
                joinBuilder.Append(joinString + " " + attrJoin.TableName + " ON " + tableName + "." + attrJoin.Key +
                                   " = " + attrJoin.TableName + "." + attrJoin.ExternalKey + " ");
            }

            return joinBuilder.ToString();
        }


        private static string GetFieldsSelect(string tableName, IEnumerable<SqlPropertyMetadata> properties)
        {
            Func<SqlPropertyMetadata, string> selector = p =>
            {
                return !string.IsNullOrEmpty(p.Alias)
                    ? tableName + ".[" + p.ColumnName + "] AS [" + p.PropertyName + "]"
                    : tableName + ".[" + p.ColumnName + "]";
            };
            return string.Join(", ", properties.Select(selector));
        }


        /// <summary>
        ///     Fill query properties
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <param name="linkingType">Type of the linking.</param>
        /// <param name="queryProperties">The query properties.</param>
        private void FillQueryProperties(Expression expr, ExpressionType linkingType,
            ref List<QueryParameter> queryProperties)
        {
            var body = expr as MethodCallExpression;
            if (body != null)
            {
                var innerBody = body;
                var methodName = innerBody.Method.Name;
                switch (methodName)
                {
                    case "Contains":
                    {
                        bool isNested;
                        var propertyName = ExpressionHelper.GetPropertyNamePath(innerBody, out isNested);

                        if (!SqlProperties.Select(x => x.PropertyName).Contains(propertyName) &&
                            !SqlJoinProperties.Select(x => x.PropertyName).Contains(propertyName))
                            throw new NotImplementedException("predicate can't parse");

                        var propertyValue = ExpressionHelper.GetValuesFromCollection(innerBody);
                        var opr = ExpressionHelper.GetMethodCallSqlOperator(methodName);
                        var link = ExpressionHelper.GetSqlOperator(linkingType);
                        queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr, isNested));
                        break;
                    }

                    default:
                        throw new NotImplementedException($"'{methodName}' method is not implemented");
                }
            }
            else if (expr is BinaryExpression)
            {
                var innerbody = (BinaryExpression) expr;
                if (innerbody.NodeType != ExpressionType.AndAlso && innerbody.NodeType != ExpressionType.OrElse)
                {
                    bool isNested;
                    var propertyName = ExpressionHelper.GetPropertyNamePath(innerbody, out isNested);
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        if (!SqlProperties.Select(x => x.PropertyName).Contains(propertyName) &&
                            !SqlJoinProperties.Select(x => x.PropertyName).Contains(propertyName))
                            throw new NotImplementedException("predicate can't parse");

                        var propertyValue = ExpressionHelper.GetValue(innerbody.Right);
                        var opr = ExpressionHelper.GetSqlOperator(innerbody.NodeType);
                        var link = ExpressionHelper.GetSqlOperator(linkingType);

                        queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr, isNested));
                    }
                }
                else
                {
                    FillQueryProperties(innerbody.Left, innerbody.NodeType, ref queryProperties);
                    FillQueryProperties(innerbody.Right, innerbody.NodeType, ref queryProperties);
                }
            }
            else
            {
                FillQueryProperties(ExpressionHelper.GetBinaryExpression(expr), linkingType, ref queryProperties);
            }
        }

        private enum QueryType
        {
            Select,
            Delete,
            Update
        }
    }
}