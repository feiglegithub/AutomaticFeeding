// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：SqlQuery.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Text;

#endregion

namespace NJIS.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     A object with the generated sql and dynamic params.
    /// </summary>
    public class SqlQuery
    {
        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        public SqlQuery()
        {
            SqlBuilder = new StringBuilder();
        }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="param">The param.</param>
        public SqlQuery(object param)
            : this()
        {
            Param = param;
        }

        /// <summary>
        ///     SqlBuilder
        /// </summary>
        public StringBuilder SqlBuilder { get; }

        /// <summary>
        ///     Gets the param
        /// </summary>
        public object Param { get; private set; }

        /// <summary>
        ///     Gets the SQL.
        /// </summary>
        public string GetSql()
        {
            return SqlBuilder.ToString().Trim();
        }

        /// <summary>
        ///     Set alternative param
        /// </summary>
        /// <param name="param">The param.</param>
        public void SetParam(object param)
        {
            Param = param;
        }
    }
}