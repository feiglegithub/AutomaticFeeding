using ADOX;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.UI
{
    public class AccessDBHelper
    {

        private const string _ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5";

        private string ConnectionString = "";

        private string fileName = "";

        public AccessDBHelper(string fileName)
        {
            ConnectionString = string.Format(_ConnectionString, fileName);
            this.fileName = fileName;
        }

        public void CreatedAccessDB(/*string fileName*/)
        {
            Catalog clog = new Catalog();
            if (!File.Exists(fileName))
            {
                clog.Create(string.Format(_ConnectionString, fileName));
            }
        }

        public void Open()
        {
            using (OleDbConnection oleDb = new OleDbConnection(ConnectionString))
            {
                if(oleDb.State== System.Data.ConnectionState.Closed)
                    oleDb.Open();
                //var dt = oleDb.GetSchema("Tables");//包含系统表

                var dt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                List<string> tables = new List<string>();

                ADODB.Connection conn = new ADODB.Connection();
                conn.Open(ConnectionString);
                //clog.ActiveConnection = conn;
                for (int i=0;i< dt.Rows.Count;i++)
                {
                    string tableName = dt.Rows[i]["TABLE_NAME"].ToString();
                    tables.Add(tableName);
                    var fileddt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                    var schema = conn.OpenSchema(ADODB.SchemaEnum.adSchemaTables);
                }

                conn.Close();
                oleDb.Close();
            }
        }

        public static List<string> GetTables(string SourceFile)
        {
            string ConnectionString = string.Format(_ConnectionString, SourceFile);
            List<string> tables = new List<string>();
            using (OleDbConnection oleDb = new OleDbConnection(ConnectionString))
            {
                if (oleDb.State == System.Data.ConnectionState.Closed)
                    oleDb.Open();
                var dt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tableName = dt.Rows[i]["TABLE_NAME"].ToString();
                    tables.Add(tableName);
                    //var fileddt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                }
                oleDb.Close();
            }
            return tables;
        }

        public static void Copy(string SourceFile,string DestFile)
        {
            string ConnectionString = string.Format(_ConnectionString, DestFile);
            
            Catalog clog = new Catalog();
            if (!File.Exists(DestFile))
            {
                File.Copy(SourceFile,DestFile);
                
            }
            var tables = GetTables(SourceFile);
            //string tableName = "BOARDS";
            string sql = @"SELECT * INTO tmp{0} FROM [;database={1};].{0}";

            string sql1 = @"TRUNCATE TABLE {0}";
            string sql2 = @"INSERT {0} SELECT * FORM tmp{0}";

            using (OleDbConnection oleDb = new OleDbConnection(ConnectionString))
            {
                if (oleDb.State == System.Data.ConnectionState.Closed)
                    oleDb.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction tran = oleDb.BeginTransaction();
                cmd.Connection = oleDb;
                cmd.Transaction = tran;
                //tran.Begin();
                try
                {
                    foreach (var tableName in tables)
                    {
                        cmd.CommandText = string.Format(sql, tableName, SourceFile);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format(sql1, tableName);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format(sql2, tableName);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
                catch(Exception e)
                {
                    tran.Rollback();
                }
                
                oleDb.Close();
            }
        }
        
    }
}
