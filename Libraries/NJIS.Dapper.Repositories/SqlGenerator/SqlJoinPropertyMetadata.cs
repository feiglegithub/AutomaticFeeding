// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlJoinPropertyMetadata.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Reflection;
using NJIS.Model.Attributes.Joins;

#endregion

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    /// </summary>
    public class SqlJoinPropertyMetadata : SqlPropertyMetadata
    {
        /// <summary>
        ///     Metadata for join property info
        /// </summary>
        /// <param name="joinPropertyInfo">Table property info</param>
        /// <param name="propertyInfo">Table column property info</param>
        public SqlJoinPropertyMetadata(PropertyInfo joinPropertyInfo, PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            var joinAtttribute = joinPropertyInfo.GetCustomAttribute<JoinAttributeBase>();
            JoinPropertyInfo = joinPropertyInfo;
            TableSchema = joinAtttribute.TableSchema;
            TableName = joinAtttribute.TableName;
            TableAlias = joinAtttribute.TableAlias;
        }

        /// <summary>
        ///     Table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     Table alias
        /// </summary>
        public string TableAlias { get; set; }

        /// <summary>
        ///     Schema name
        /// </summary>
        public string TableSchema { get; set; }

        /// <summary>
        ///     Original join property info
        /// </summary>
        public PropertyInfo JoinPropertyInfo { get; set; }

        /// <summary>
        ///     Join property name
        /// </summary>
        public string JoinPropertyName => JoinPropertyInfo.Name;

        /// <summary>
        ///     Full property name
        /// </summary>
        public override string PropertyName => JoinPropertyName + base.PropertyName;
    }
}