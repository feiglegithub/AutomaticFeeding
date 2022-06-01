// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlGenerator.InitProperties.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:29
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using NJIS.Dapper.Repositories.Extensions;
using NJIS.Model.Attributes;
using NJIS.Model.Attributes.Joins;

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        private void InitProperties()
        {
            var entityType = typeof(TEntity);
            var entityTypeInfo = entityType.GetTypeInfo();
            var tableAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            TableName = tableAttribute != null ? tableAttribute.Name : entityTypeInfo.Name;
            TableSchema = tableAttribute != null ? tableAttribute.Schema : string.Empty;

            AllProperties = entityType.FindClassProperties().Where(q => q.CanWrite).ToArray();

            var props = AllProperties.Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();

            var joinProperties = AllProperties.Where(p => p.GetCustomAttributes<JoinAttributeBase>().Any()).ToArray();

            SqlJoinProperties = GetJoinPropertyMetadata(joinProperties);

            // Filter the non stored properties
            SqlProperties = props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                .Select(p => new SqlPropertyMetadata(p)).ToArray();

            // Filter key properties
            KeySqlProperties = props.Where(p => p.GetCustomAttributes<KeyAttribute>().Any())
                .Select(p => new SqlPropertyMetadata(p)).ToArray();

            // Use identity as key pattern
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            IdentitySqlProperty = identityProperty != null ? new SqlPropertyMetadata(identityProperty) : null;

            var dateChangedProperty =
                props.FirstOrDefault(p => p.GetCustomAttributes<UpdatedAtAttribute>().Count() == 1);
            if (dateChangedProperty != null && (dateChangedProperty.PropertyType == typeof(DateTime) ||
                                                dateChangedProperty.PropertyType == typeof(DateTime?)))
            {
                UpdatedAtProperty = props.FirstOrDefault(p => p.GetCustomAttributes<UpdatedAtAttribute>().Any());
                UpdatedAtPropertyMetadata = new SqlPropertyMetadata(UpdatedAtProperty);
            }
        }
    }
}