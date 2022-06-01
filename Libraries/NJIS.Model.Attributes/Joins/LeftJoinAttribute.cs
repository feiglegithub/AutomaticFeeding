// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform
//  项目名称：NJIS.Model.Attributes
//  文 件 名：LeftJoinAttribute.cs
//  创建时间：2017-08-16 22:57
//  作    者：
//  说    明：
//  修改时间：2017-08-16 22:58
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.Model.Attributes.Joins
{
    /// <summary>
    ///     Generate LEFT JOIN
    /// </summary>
    public class LeftJoinAttribute : JoinAttributeBase
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public LeftJoinAttribute()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tableName">Name of external table</param>
        /// <param name="key">ForeignKey of this table</param>
        /// <param name="externalKey">Key of external table</param>
        public LeftJoinAttribute(string tableName, string key, string externalKey)
            : base(tableName, key, externalKey, string.Empty,string.Empty)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tableName">Name of external table</param>
        /// <param name="key">ForeignKey of this table</param>
        /// <param name="externalKey">Key of external table</param>
        /// <param name="tableSchema">Name of external table schema</param>
        public LeftJoinAttribute(string tableName, string key, string externalKey, string tableSchema)
            : base(tableName, key, externalKey, tableSchema, string.Empty)
        {
        }
        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tableName">Name of external table</param>
        /// <param name="key">ForeignKey of this table</param>
        /// <param name="externalKey">Key of external table</param>
        /// <param name="tableSchema">Name of external table schema</param>
        /// <param name="tableAbbreviation">External table alias</param>
        public LeftJoinAttribute(string tableName, string key, string externalKey, string tableSchema, string tableAbbreviation)
            : base(tableName, key, externalKey, tableSchema, tableAbbreviation)
        {
        }
    }
}