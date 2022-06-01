// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：TableSchema.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2019-01-07 7:47
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NJIS.Dapper.Repositories.Extensions
{
    public static class TableSchema
    {
        #region 批量插入数据库

        /// <summary>
        ///     批量插入数据
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="tableName">表名</param>
        /// <param name="dataTable">需要插入表数据</param>
        /// <returns></returns>
        public static bool BulkInsert(string connectionString, string tableName, DataTable dataTable)
        {
            //SqlTransaction sqlTransaction = null;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var bulkCopy = new SqlBulkCopy(conn);
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dataTable.Rows.Count;
                    conn.Open();
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //sqlTransaction = conn.BeginTransaction();
                        bulkCopy.WriteToServer(dataTable);
                    }

                    //if(sqlTransaction!=null)
                    //{
                    //    sqlTransaction.Commit();
                    //}
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                //if (sqlTransaction != null)
                //{
                //    sqlTransaction.Rollback();
                //}
                return false;
            }

            return true;
        }

        #endregion

        #region 生成连接字符串

        /// <summary>
        ///     创建SqlServer连接字符串
        /// </summary>
        /// <param name="computerNameOrIp">主机名或者Ip</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">口令（密码）</param>
        /// <param name="dataBaseName">数据库名</param>
        /// <returns></returns>
        public static string CreatedDBConnectionString(string computerNameOrIp, string userName, string password,
            string dataBaseName)
        {
            var strComputerNameOrIpPart = $@"Data Source={computerNameOrIp};";
            var strUserPart = $@"User Id={userName};Password={password};";

            var strDataBasePart = $@"Initial Catalog={dataBaseName};";

            var ConnectionString = strComputerNameOrIpPart + strUserPart + strDataBasePart;

            return ConnectionString;
        }

        /// <summary>
        ///     创建SqlServer连接字符串
        /// </summary>
        /// <param name="computerNameOrIp">主机名或者Ip</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">口令（密码）</param>
        /// <returns></returns>
        public static string CreatedDBConnectionString(string computerNameOrIp, string userName, string password)
        {
            var strComputerNameOrIpPart = $@"Data Source={computerNameOrIp};";
            var strUserPart = $@"User Id={userName};Password={password};";
            var ConnectionString = strComputerNameOrIpPart + strUserPart;

            return ConnectionString;
        }

        #endregion

        #region 获取数据库表结构 sql

        private const string TableName = nameof(TableName);
        private const string id = nameof(id);
        private const string FieldName = nameof(FieldName);
        private const string FieldDescription = nameof(FieldDescription);
        private const string FieldType = nameof(FieldType);
        private const string IsNullable = nameof(IsNullable);

        private const string _sql = @"SELECT  CASE WHEN col.colorder = 1 THEN obj.name
                  ELSE ''
             END AS {0},--TableName,
        col.colorder AS {1},--id ,
        col.name AS {2},--FieldName ,
        col.isnullable as {3}, -- IsNullable,
        ISNULL(ep.[value], '')AS {4},--FieldDescription,
        t.name AS {5}--FiledType

FROM dbo.syscolumns col
        LEFT  JOIN dbo.systypes t ON col.xtype = t.xusertype
        inner JOIN dbo.sysobjects obj ON col.id = obj.id
                                         AND obj.xtype = 'U'
                                         AND obj.status >= 0
        LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
        LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id
                                                      AND col.colid = ep.minor_id
                                                      AND ep.name = 'MS_Description'
        LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id
                                                         AND epTwo.minor_id = 0
                                                         AND epTwo.name = 'MS_Description'
WHERE obj.name = '{6}'-- TableName
ORDER BY col.colorder ;";

        #endregion

        #region 获取数据库表结构 方法

        /// <summary>
        ///     获取数据库表结构
        /// </summary>
        /// <param name="connectionString">数据库连接（包含数据库名）</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public static DataTable GetDBTableSchema(string connectionString, string TableName)
        {
            var sql = string.Format(_sql, TableSchema.TableName, id, FieldName, IsNullable, FieldDescription, FieldType,
                TableName);
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = sql;
                    var result = command.ExecuteReader();
                    var TableSchema = _GetSchemaTable(result);
                    TableSchema.TableName = TableName;
                    conn.Close();
                    return TableSchema;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        ///     获取数据库表结构
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static DataTable GetDBTableSchema<TEntity>(this DapperRepository<TEntity> rep)
            where TEntity : class
        {
            var strTableName = typeof(TEntity).Name;
            var sql = string.Format(_sql, TableName, id, FieldName, IsNullable, FieldDescription, FieldType,
                strTableName);
            var DbConnection = rep.Connection;
            DbConnection.Open();
            var DbCommand = DbConnection.CreateCommand();
            try
            {
                DbCommand.CommandText = sql;
                var result = DbCommand.ExecuteReader();

                var TableSchema = _GetSchemaTable(result);
                TableSchema.TableName = strTableName;
                return TableSchema;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private const string SelectSqlFormat = "SELECT * FROM {0} WHERE 1=1 ";

        /// <summary>
        ///     查询数据
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        /// <param name="TableName">查询表名</param>
        /// <param name="condiction">查询条件（WHERE后的sql（不包含where））</param>
        /// <returns></returns>
        public static DataTable GetInfos(string connectionString, string TableName, string condiction)
        {
            var sql = string.Format(SelectSqlFormat, TableName) + condiction;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = sql;
                    var result = command.ExecuteReader();
                    var data = FillData(result);
                    conn.Close();
                    return data;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static DataTable FillData(IDataReader result)
        {
            var dt = result.GetSchemaTable();
            var data = new DataTable();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var columnName = dt.Rows[i][0].ToString();
                var dataType = Type.GetType(dt.Rows[i]["DataType"].ToString());
                data.Columns.Add(new DataColumn(columnName, dataType));
            }

            while (result.Read())
            {
                var dr = data.NewRow();
                for (var i = 0; i < data.Columns.Count; i++)
                {
                    var name = data.Columns[i].ColumnName;
                    dr[name] = result[name];
                }

                data.Rows.Add(dr.ItemArray);
            }

            return data;
        }

        private static DataTable _GetSchemaTable(IDataReader result)
        {
            var dt = result.GetSchemaTable();
            var TableSchema = new DataTable();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var columnName = dt.Rows[i][0].ToString();
                var dataType = Type.GetType(dt.Rows[i]["DataType"].ToString());
                var t = columnName == FieldType ? typeof(Type) : typeof(string);
                TableSchema.Columns.Add(new DataColumn(columnName, dataType));
            }

            while (result.Read())
            {
                var dr = TableSchema.NewRow();
                for (var i = 0; i < TableSchema.Columns.Count; i++)
                {
                    var name = TableSchema.Columns[i].ColumnName;
                    if (name == FieldDescription)
                    {
                        if (string.IsNullOrWhiteSpace(result[name].ToString()))
                        {
                            dr[name] = result[FieldName];
                            continue;
                        }

                        dr[name] = result[name];
                    }
                    else if (name == FieldType)
                    {
                        var index = result.GetOrdinal(FieldType);
                        var nullAbleindex = result.GetOrdinal(IsNullable);
                        var Nullable = Convert.ToBoolean(result[nullAbleindex]);
                        var obj = result[index];
                        var typeString = result[index].ToString().ToLower();
                        var t = GetDataType(typeString, Nullable);
                        dr[name] = t;
                    }
                    else
                    {
                        dr[name] = result[name];
                    }
                }

                TableSchema.Rows.Add(dr);
            }

            return TableSchema;
        }

        private static Type GetDataType(string strDataType, bool IsNullable)
        {
            Type type = null;
            switch (strDataType)
            {
                case "bit":
                    type = IsNullable ? typeof(bool?) : typeof(bool);
                    break;
                case "tinyint":
                    type = IsNullable ? typeof(byte?) : typeof(byte);
                    break;
                case "smallint":
                    type = IsNullable ? typeof(short?) : typeof(short);
                    break;
                case "int":
                    type = IsNullable ? typeof(int?) : typeof(int);
                    break;
                case "bigint":
                    type = IsNullable ? typeof(long?) : typeof(long);
                    break;
                case "smallmoney":
                    type = IsNullable ? typeof(decimal?) : typeof(decimal);
                    break;
                case "money":
                    type = IsNullable ? typeof(decimal?) : typeof(decimal);
                    break;
                case "numeric":
                    type = IsNullable ? typeof(decimal?) : typeof(decimal);
                    break;
                case "decimal":
                    type = IsNullable ? typeof(decimal?) : typeof(decimal);
                    break;
                case "float":
                    type = IsNullable ? typeof(double?) : typeof(double);
                    break;
                case "real":
                    type = IsNullable ? typeof(float?) : typeof(float);
                    break;
                case "smalldatetime":
                    type = IsNullable ? typeof(DateTime?) : typeof(DateTime);
                    break;
                case "datetime":
                    type = IsNullable ? typeof(DateTime?) : typeof(DateTime);
                    break;
                case "timestamp":
                    type = IsNullable ? typeof(DateTime?) : typeof(DateTime);
                    break;
                case "char":
                    type = typeof(string);
                    break;
                case "text":
                    type = typeof(string);
                    break;
                case "varchar":
                    type = typeof(string);
                    break;
                case "nchar":
                    type = typeof(string);
                    break;
                case "ntext":
                    type = typeof(string);
                    break;
                case "nvarchar":
                    type = typeof(string);
                    break;
                case "binary":
                    type = typeof(byte[]);
                    break;
                case "varbinary":
                    type = typeof(byte[]);
                    break;
                case "image":
                    type = typeof(byte[]);
                    break;
                case "uniqueidentifier":
                    type = IsNullable ? typeof(Guid?) : typeof(Guid);
                    break;
                case "Variant":
                    type = typeof(object);
                    break;
                default:
                    type = typeof(string);
                    break;
            }

            return type;
        }

        /// <summary>
        ///     将数据库表结构信息从DataTable类型转换为TableSchemaInfo类型
        /// </summary>
        /// <param name="dataTable">存放数据库表结构信息的DataTable</param>
        /// <returns></returns>
        public static TableSchemaInfo ToTableSchemaInfo(this DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
            {
                var tableSchemaInfo = new TableSchemaInfo();
                tableSchemaInfo.TableName = dataTable.Rows[0][TableName].ToString();

                var FiledInfos = new List<FiledInfo>();
                var count = dataTable.Rows.Count;
                for (var i = 0; i < count; i++)
                {
                    var row = dataTable.Rows[i];
                    var fieldDescription = row[FieldDescription].ToString();
                    var filedName = row[FieldName].ToString();

                    var t = row[FieldType] as Type;
                    if (t == null)
                    {
                        var strtype = row[FieldType].ToString();
                        t = Type.GetType(strtype);
                    }

                    var filedType = t;
                    FiledInfos.Add(new FiledInfo
                        {FieldDescription = fieldDescription, FiledName = filedName, FiledType = filedType});
                }

                tableSchemaInfo.FiledInfos = FiledInfos;
                return tableSchemaInfo;
            }

            return null;
        }

        private static DataTable CreateDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn(TableName, typeof(string)));
            dt.Columns.Add(new DataColumn(FieldName, typeof(string)));
            dt.Columns.Add(new DataColumn(FieldType, typeof(Type)));
            dt.Columns.Add(new DataColumn(FieldDescription, typeof(string)));
            return dt;
        }

        #endregion

        #region 尝试连接数据库

        /// <summary>
        ///     尝试连接数据库
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static bool TryConnect<TEntity>(this DapperRepository<TEntity> rep)
            where TEntity : class
        {
            try
            {
                var DbConnection = rep.Connection;
                DbConnection.Open();
                DbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        ///     尝试连接数据库
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static bool TryConnect(string connectionString)
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion

        #region 获取数据库列表

        #region 获取数据列表sql

        private const string DatabaseName = nameof(DatabaseName);

        private const string Sql_GetDatabases = @"SELECT name as {0} FROM sys.databases;";

        #endregion

        /// <summary>
        ///     获取数据库列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<string> GetDatabases(string connectionString)
        {
            var sql = string.Format(Sql_GetDatabases, DatabaseName);
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var sqlDataAdapter = new SqlDataAdapter(sql, conn);
                    sqlDataAdapter.Fill(dt);
                    conn.Close();
                }

                var count = dt.Rows.Count;
                if (count > 0)
                {
                    var list = new List<string>();

                    for (var i = 0; i < count; i++)
                    {
                        list.Add(dt.Rows[i][DatabaseName].ToString());
                    }

                    return list;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 获取表列表

        /// <summary>
        ///     获取数据库中用户表和表注释列表sql
        /// </summary>
        private const string Sql_GetTablesAndConnect = @"SELECT
TableName = d.name,
CONNECT= ISNULL(f.value, d.name)
        FROM syscolumns a
        inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
        left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0 and f.name='MS_Description'
WHERE a.colorder=1 ORDER BY TableName;";

        /// <summary>
        ///     获取数据库的表列表
        /// </summary>
        /// <param name="ConnectionString">数据库连接字符串（包含数据库名）</param>
        /// <returns></returns>
        public static DataTable GetDBTables(string ConnectionString)
        {
            var sql = Sql_GetTablesAndConnect;
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var sqlDataAdapter = new SqlDataAdapter(sql, conn);
                    sqlDataAdapter.Fill(dt);
                    conn.Close();
                }

                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region 批量删除数据

        private static List<string> GetTablePKNames(string ConnectionString, string TableName)
        {
            var PKNames = new List<string>();
            var sql = "sp_pkeys";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@table_name", SqlDbType.VarChar).Value = TableName;
                    var dt = FillData(cmd.ExecuteReader());
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            PKNames.Add(dt.Rows[i]["COLUMN_NAME"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return PKNames;
        }

        /// <summary>
        ///     批量数据删除---只支持数据中有主键数据的删除
        /// </summary>
        /// <param name="ConnectionString">数据库连接</param>
        /// <param name="TableName">表名</param>
        /// <param name="dataTable">数据</param>
        public static bool BulkDelete(string ConnectionString, string TableName, DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return false;
            }

            var pkNames = GetTablePKNames(ConnectionString, TableName);
            if (pkNames.Count == 0)
            {
                return false;
            }

            SqlTransaction sqlTransaction = null;

            var delSql = $"DELETE FROM {TableName} WHERE ";
            var sqlFormat = "[{0}]='{1}'";
            var pkName = string.Empty;

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand();
                    foreach (var item in pkNames)
                    {
                        if (dataTable.Columns.Contains(item))
                        {
                            pkName = item;
                            break;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(pkName))
                    {
                        return false;
                    }

                    var sqlBuilder = new StringBuilder();
                    var count = dataTable.Rows.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var row = dataTable.Rows[i];
                        sqlBuilder.AppendFormat(sqlFormat, pkName, row[pkName].ToString().Replace("'", "''"));
                        if (i < count - 1)
                        {
                            sqlBuilder.Append(" OR ");
                        }
                    }

                    delSql = delSql + sqlBuilder;
                    cmd.CommandText = delSql;
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Transaction = sqlTransaction = conn.BeginTransaction();
                    var ret = cmd.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                return false;
            }
        }

        #endregion
    }


    public class TableSchemaInfo
    {
        /// <summary>
        ///     表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     所有字段信息
        /// </summary>
        public List<FiledInfo> FiledInfos { get; set; }
    }

    /// <summary>
    ///     字段信息
    /// </summary>
    public class FiledInfo
    {
        /// <summary>
        ///     字段名
        /// </summary>
        public string FiledName { get; set; }

        /// <summary>
        ///     字段描述
        /// </summary>
        public string FieldDescription { get; set; }

        /// <summary>
        ///     字段数据类型
        /// </summary>
        public Type FiledType { get; set; }
    }
}