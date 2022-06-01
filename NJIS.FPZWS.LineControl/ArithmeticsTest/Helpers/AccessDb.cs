using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace ArithmeticsTest.Helpers
{
    public class AccessDb : IDisposable
    {
        private const string _ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5";

        private string ConnectionString = "";
        private OleDbConnection conn = null;
        private string AccessFileName = string.Empty;

        public AccessDb(string accessFileName)
        {
            this.AccessFileName = accessFileName;
            ConnectionString = string.Format(_ConnectionString, accessFileName);
            conn = new OleDbConnection(ConnectionString);

            selectCommandTexts.Add(new Tuple<string, string>(nameof(BOARDS), BOARDS));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(CUTS), CUTS));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(HEADER), HEADER));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(JOBS), JOBS));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(MATERIALS), MATERIALS));

            selectCommandTexts.Add(new Tuple<string, string>(nameof(NOTES), NOTES));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(OFFCUTS), OFFCUTS));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(PARTS_DST), PARTS_DST));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(PARTS_REQ), PARTS_REQ));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(PARTS_UDI), PARTS_UDI));
            selectCommandTexts.Add(new Tuple<string, string>(nameof(PATTERNS), PATTERNS));
            InitDataAdapter();

        }

        List<Tuple<string, string>> selectCommandTexts = new List<Tuple<string, string>>();

        private const string BOARDS = "SELECT * FROM BOARDS ";
        private const string CUTS = "SELECT * FROM CUTS";
        private const string HEADER = "SELECT * FROM HEADER";
        private const string JOBS = "SELECT * FROM JOBS";
        private const string MATERIALS = "SELECT * FROM MATERIALS";

        private const string NOTES = "SELECT * FROM NOTES";
        private const string OFFCUTS = "SELECT * FROM OFFCUTS";
        private const string PARTS_DST = "SELECT * FROM PARTS_DST";
        private const string PARTS_REQ = "SELECT * FROM PARTS_REQ";
        private const string PARTS_UDI = "SELECT * FROM PARTS_UDI";
        private const string PATTERNS = "SELECT * FROM PATTERNS";



        private List<Tuple<string, OleDbDataAdapter>> adapters = new List<Tuple<string, OleDbDataAdapter>>();

        private void InitDataAdapter()
        {
            foreach (var tuple in selectCommandTexts)
            {
                var adapter = new OleDbDataAdapter(tuple.Item2, conn);

                adapters.Add(new Tuple<string, OleDbDataAdapter>(tuple.Item1, adapter));

            }
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetDatas()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DataSet dataSet = new DataSet();
            foreach (var tuple in adapters)
            {
                var adapter = tuple.Item2;
                var tableName = tuple.Item1;
                adapter.Fill(dataSet, tableName);
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
                commandBuilder.QuotePrefix = "[";
                commandBuilder.QuoteSuffix = "]";
                adapter.InsertCommand = commandBuilder.GetInsertCommand();
                if (tuple.Item1 != nameof(HEADER))
                {
                    adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                    adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                }
            }
            dataSet.AcceptChanges();
            return dataSet;
        }

        /// <summary>
        /// 创建MDB文件
        /// </summary>
        /// <param name="mdbSourceFile">源mdb文件</param>
        /// <param name="mdbNames">拆分后的mdb文件名</param>
        /// <returns></returns>
        private bool CreatedMDBFile(string mdbSourceFile, List<string> mdbNames)
        {
            List<string> CreadtedFiles = new List<string>();
            foreach (var mdbName in mdbNames)
            {
                try
                {
                    File.Copy(mdbSourceFile, mdbName);
                    CreadtedFiles.Add(mdbName);
                }
                catch (Exception e)
                {
                    foreach (var item in CreadtedFiles)
                    {
                        if (File.Exists(item))
                        {
                            File.Delete(item);
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        private bool IsComplete(string mdbSourceFile)
        {
            return true;
        }

        public bool SpiltDatas(string mdbSourceFile, List<string> mdbNames)
        {
            if (!IsComplete(mdbSourceFile))
            {
                return false;
            }
            if (!CreatedMDBFile(mdbSourceFile, mdbNames))
            {
                return false;
            }

            return true;
        }

        public void Update(DataSet dataSet)
        {
            var t = new List<int>();
            foreach (DataRow row in dataSet.Tables[7].Rows)
            {
                t.Add(Convert.ToInt32(row["PART_INDEX"].ToString()));
            }

            var s1 = t.GroupBy(item => item);
            var s2 =s1.FirstOrDefault(item => item.Count() > 1);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            var tran = conn.BeginTransaction();
            string s = string.Empty;
            try
            {
                foreach (var tuple in adapters)
                {
                    var adapter = tuple.Item2;
                    s = tuple.Item1;
                    if (tuple.Item1 != nameof(HEADER))
                    {
                        adapter.DeleteCommand.Transaction = tran;
                        adapter.UpdateCommand.Transaction = tran;
                    }
                    adapter.InsertCommand.Transaction = tran;
                    var a = dataSet.GetChanges();
                    adapter.Update(dataSet, tuple.Item1);
                }
                tran.Commit();
                dataSet.AcceptChanges();

            }
            catch (Exception e)
            {
                tran.Rollback();
                dataSet.RejectChanges();
                throw e;
            }
            finally
            {
                conn.Close();
                CompressAccessDB();
            }
        }

        /// <summary>
        /// 压缩数据库
        /// </summary>
        public void CompressAccessDB()
        {
            JRO.JetEngine jt = new JRO.JetEngine();
            string tmpName = Path.Combine(new FileInfo(AccessFileName).DirectoryName, "Compact_tmp.mdb");
            jt.CompactDatabase(ConnectionString, string.Format(_ConnectionString, tmpName));
            File.Delete(AccessFileName);
            File.Move(tmpName, this.AccessFileName);
        }

        public void Dispose()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }
    }
}
