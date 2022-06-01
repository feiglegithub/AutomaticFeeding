using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.MDB
{
    public class SplitMDB
    {

        private const string BOARDS = nameof(BOARDS);
        private const string CUTS = nameof(CUTS);
        private const string HEADER = nameof(HEADER);
        private const string JOBS = nameof(JOBS);
        private const string MATERIALS = nameof(MATERIALS);
        private const string NOTES = nameof(NOTES);
        private const string OFFCUTS = nameof(OFFCUTS);
        private const string PARTS_DST = nameof(PARTS_DST);
        private const string PARTS_REQ = nameof(PARTS_REQ);
        private const string PARTS_UDI = nameof(PARTS_UDI);
        private const string PATTERNS = nameof(PATTERNS);
        private const string ROW_INDEX = nameof(ROW_INDEX);
        private const string BRD_INDEX = nameof(BRD_INDEX);
        private const string PTN_INDEX = nameof(PTN_INDEX);
        private const string PART_INDEX = nameof(PART_INDEX);
        private const string OFC_INDEX = nameof(OFC_INDEX);
        private const string MAT_INDEX = nameof(MAT_INDEX);

        private const string MDBPath = "MDBs";//Path.Combine(System.IO.Directory.GetCurrentDirectory(), "MDBs");

        private class SpiltDatas
        {
            /// <summary>
            /// MDB文件名（ItemName(堆垛名)）
            /// </summary>
            public string MDBName { get; set; }
            /// <summary>
            /// 拆分结果
            /// </summary>
            //public SpiltMDBResult Result { get; set; }
            /// <summary>
            /// 单个MDB的所有数据
            /// </summary>
            public DataSet Datas { get; set; }
        }

        private const string tmpMDBName = @"MDBTempalte\Template.mdb";

        #region 创建临时文件

        /// <summary>
        /// 创建空MDB临时副本文件
        /// </summary>
        /// <param name="SourceMDB"></param>
        /// <returns></returns>
        private bool CreatedTmpMDB(string SourceMDB)
        {
            try
            {
                if (File.Exists(tmpMDBName))
                {
                    File.Delete(tmpMDBName);
                }
                File.Copy(SourceMDB, tmpMDBName);
                ClearDatas(tmpMDBName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void ClearDatas(string MDBName)
        {
            AccessDB access = new AccessDB(MDBName);
            var datas = access.GetDatas();
            ClearDatas(datas);
            access.Update(datas);
            access.Dispose();
        }

        private void ClearDatas(DataSet datas)
        {
            for (int i = 0; i < datas.Tables.Count; i++)
            {
                var dt = datas.Tables[i];
                ClearDatas(dt);
            }
        }

        private void ClearDatas(DataTable table)
        {
            int count = table.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                table.Rows[i].Delete();
            }
        }

        #endregion



        /// <summary>
        /// 创建空MDB文件
        /// </summary>
        /// <param name="DestFileMDBNames">MDB文件</param>
        /// <returns></returns>
        private bool CreatedMDBs(/*List<string> DestFileMDBNames*/ string MDBFullName)
        {
            if (!Directory.Exists(MDBPath))
            {
                Directory.CreateDirectory(MDBPath);
            }

            string path = Path.GetDirectoryName(MDBFullName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(MDBFullName))
            {
                try
                {
                    File.Delete(MDBFullName);
                }
                catch (Exception e)
                {
                    return false;
                }
                
            }
            File.Copy(tmpMDBName, MDBFullName);

            //List<string> CreadtedFiles = new List<string>();
            //foreach (var item in DestFileMDBNames)
            //{
            //    //string mdbName = Path.Combine(MDBPath,item + ".mdb");
            //    try
            //    {
            //        if (File.Exists(item))
            //        {
            //            File.Delete(item);
            //        }
            //        File.Copy(tmpMDBName, item);
            //        CreadtedFiles.Add(item);
            //    }
            //    catch (Exception e)
            //    {
            //        foreach (var file in CreadtedFiles)
            //        {
            //            if (File.Exists(file))
            //            {
            //                File.Delete(file);
            //            }
            //        }
            //        return false;
            //    }
            //}
            return true;
        }

        #region 数据获取与更新
        //private Dictionary<string, AccessDB> Accesss = new Dictionary<string, AccessDB>();

        //private DataSet GetDatas(string SourceMDB)
        //{
        //    if (Accesss.ContainsKey(SourceMDB))
        //    {
        //        return Accesss[SourceMDB].GetDatas();
        //    }
        //    AccessDB access = new AccessDB(SourceMDB);
        //    Accesss.Add(SourceMDB, access);
        //    var ds = access.GetDatas();
        //    return ds;
        //}

        //private void UpDate(List<SpiltDatas> SpiltDatas)
        //{
        //    foreach (var item in SpiltDatas)
        //    {
        //        UpDate(item.MDBName, item.Datas);
        //    }
        //}

        //private void UpDate(string MDB, DataSet data)
        //{
        //    if (Accesss.ContainsKey(MDB))
        //    {
        //        Accesss[MDB].Update(data);
        //    }
        //}
        #endregion

        public bool UpdateDatas(/*List<Tuple<string, DataSet, SpiltMDBResult>> tuples*/ string MDBFullName,DataSet data)
        {
            if (!File.Exists(tmpMDBName))
            {
                return false;
            }

            //List<SpiltDatas> tmpSpiltDatas = new List<SpiltDatas>();

            //var mdbNames = tuples.ConvertAll(item => item.Item1);
            if (!CreatedMDBs(MDBFullName))
            {
                return false;
            }

            AccessDB accessDb = new AccessDB(MDBFullName);
            var MDBData = accessDb.GetDatas();
            DataSetCopy(data, MDBData);
            accessDb.Update(MDBData);

            #region Old Code

            //List<SpiltDatas> SpiltDatas = new List<SpiltDatas>();
            //foreach (var tuple in tuples)
            //{
            //    var datas = tuple.Item2;
            //    SpiltDatas.Add(new SpiltDatas() { MDBName = tuple.Item1,/* Result = tuple.Item3,*/Datas = GetDatas(tuple.Item1) });
            //    //tmpSpiltDatas.Add(new SplitMDB.SpiltDatas() { MDBName = tuple.Item1, Datas = datas.Clone() });
            //}

            //#region Old Code




            ////foreach (var item in tuples)
            ////{
            ////    var data = item.Item2;

            ////    DataTable TableBOARDS = data.Tables[BOARDS];
            ////    DataTable TablePATTERNS = data.Tables[PATTERNS];
            ////    DataTable TableMATERIALS = data.Tables[MATERIALS];
            ////    DataTable TableCUTS = data.Tables[CUTS];
            ////    DataTable TablePARTS_UDI = data.Tables[PARTS_UDI];
            ////    DataTable TablePARTS_REQ = data.Tables[PARTS_REQ];
            ////    DataTable TablePARTS_DST = data.Tables[PARTS_DST];
            ////    DataTable TableOFFCUTS = data.Tables[OFFCUTS];


            ////    TablePATTERNS.Columns.Add(ROW_INDEX, typeof(int));
            ////    TableCUTS.Columns.Add(ROW_INDEX, typeof(int));
            ////    TablePARTS_UDI.Columns.Add(ROW_INDEX, typeof(int));
            ////    TablePARTS_REQ.Columns.Add(ROW_INDEX, typeof(int));
            ////    TablePARTS_DST.Columns.Add(ROW_INDEX, typeof(int));
            ////    TableOFFCUTS.Columns.Add(ROW_INDEX, typeof(int));


            ////    for (int i = 0; i < TableBOARDS.Rows.Count; i++)
            ////    {
            ////        var BOARDS_ROW = TableBOARDS.Rows[i];
            ////        string strBRD_INDEX = BOARDS_ROW[BRD_INDEX].ToString().Trim();
            ////        BOARDS_ROW[BRD_INDEX] = i + 1;
            ////        foreach (DataRow row in TablePATTERNS.Rows)
            ////        {
            ////            if (strBRD_INDEX == row[BRD_INDEX].ToString().Trim())
            ////            {
            ////                row[ROW_INDEX] = BOARDS_ROW[BRD_INDEX];
            ////            }
            ////        }
            ////    }

            ////    for (int i = 0; i < TablePATTERNS.Rows.Count;)
            ////    {
            ////        DataRow row = TablePATTERNS.Rows[i];
            ////        row[BRD_INDEX] = row[ROW_INDEX];
            ////        row[ROW_INDEX] = ++i;
            ////    }

            ////    for (int i = 0; i < TableMATERIALS.Rows.Count;)
            ////    {
            ////        DataRow row = TableMATERIALS.Rows[i];
            ////        var resultRows = TablePARTS_REQ.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
            ////        row[MAT_INDEX] = ++i;
            ////        foreach (var resultRow in resultRows)
            ////        {
            ////            resultRow[MAT_INDEX] = row[MAT_INDEX];
            ////        }
            ////    }

            ////    //设置 CUTS 序号，确保和 PATTERNS 一致
            ////    for (int i = 0; i < TablePATTERNS.Rows.Count; i++)
            ////    {
            ////        DataRow row = TablePATTERNS.Rows[i];
            ////        string strPTN_INDEX = row[PTN_INDEX].ToString();
            ////        var resultRows = TableCUTS.Select($"{PTN_INDEX}={strPTN_INDEX}");
            ////        foreach (var resultRow in resultRows)
            ////        {
            ////            resultRow[ROW_INDEX] = row[ROW_INDEX];
            ////        }
            ////    }
            ////    // 设置 PARTS_UDI 序号
            ////    TablePARTS_UDI.DefaultView.Sort = $"{PART_INDEX} ASC";
            ////    for (int i = 0; i < TablePARTS_UDI.DefaultView.Count;)
            ////    {
            ////        DataRowView dataRow = TablePARTS_UDI.DefaultView[i];
            ////        dataRow[ROW_INDEX] = ++i;
            ////    }

            ////    for (int i = 0; i < TablePARTS_UDI.Rows.Count; i++)
            ////    {
            ////        DataRow row = TablePARTS_UDI.Rows[i];
            ////        var resultRows = TablePARTS_REQ.Select($"{PART_INDEX}={row[PART_INDEX]}");
            ////        foreach (var resultRow in resultRows)
            ////        {
            ////            resultRow[ROW_INDEX] = row[ROW_INDEX];
            ////        }
            ////    }

            ////    for (int i = 0; i < TablePARTS_UDI.Rows.Count; i++)
            ////    {
            ////        DataRow row = TablePARTS_UDI.Rows[i];
            ////        var resultRows = TablePARTS_DST.Select($"{PART_INDEX}={row[PART_INDEX]}");
            ////        foreach (var resultRow in resultRows)
            ////        {
            ////            resultRow[ROW_INDEX] = row[ROW_INDEX];
            ////        }
            ////    }
            ////    TableOFFCUTS.DefaultView.Sort = $"{OFC_INDEX} ASC";
            ////    for (int i = 0; i < TableOFFCUTS.DefaultView.Count;)
            ////    {
            ////        DataRowView dataRow = TableOFFCUTS.DefaultView[i];
            ////        dataRow[ROW_INDEX] = ++i;
            ////    }



            ////    foreach (DataRow row in TablePATTERNS.Rows)
            ////    {
            ////        row[PTN_INDEX] = row[ROW_INDEX];
            ////    }
            ////    foreach (DataRow row in TableCUTS.Rows)
            ////    {
            ////        row[PTN_INDEX] = row[ROW_INDEX];
            ////        string strPART_INDEX = row[PART_INDEX].ToString().Trim();
            ////        foreach (DataRow PARTS_UDIRow in TablePARTS_UDI.Rows)
            ////        {
            ////            if (strPART_INDEX == PARTS_UDIRow[PART_INDEX].ToString().Trim())
            ////            {
            ////                row[ROW_INDEX] = PARTS_UDIRow[ROW_INDEX];
            ////                break;
            ////            }
            ////        }
            ////        if (strPART_INDEX.Contains("X"))
            ////        {
            ////            continue;
            ////        }
            ////        row[PART_INDEX] = row[ROW_INDEX];
            ////    }

            ////    foreach (DataRow row in TablePARTS_UDI.Rows)
            ////    {
            ////        row[PART_INDEX] = row[ROW_INDEX];
            ////    }
            ////    foreach (DataRow row in TablePARTS_REQ.Rows)
            ////    {
            ////        row[PART_INDEX] = row[ROW_INDEX];
            ////    }
            ////    foreach (DataRow row in TablePARTS_DST.Rows)
            ////    {
            ////        row[PART_INDEX] = row[ROW_INDEX];
            ////    }

            ////    foreach (DataRow row in TableCUTS.Rows)
            ////    {
            ////        string strPART_INDEX = row[PART_INDEX].ToString().Trim().ToUpper();
            ////        if (strPART_INDEX.StartsWith("X"))
            ////        {
            ////            foreach (DataRow OFFCUTSRow in TableOFFCUTS.Rows)
            ////            {
            ////                string strOFF_PART_INDEX = "X" + OFFCUTSRow[OFC_INDEX].ToString().Trim();
            ////                if (strOFF_PART_INDEX == strPART_INDEX)
            ////                {
            ////                    row[PART_INDEX] = "X" + OFFCUTSRow[ROW_INDEX].ToString().Trim();
            ////                    break;
            ////                }
            ////            }
            ////        }
            ////    }
            ////    foreach (DataRow row in TableOFFCUTS.Rows)
            ////    {
            ////        row[OFC_INDEX] = row[ROW_INDEX];
            ////    }
            ////    RebuildUSED(data);

            ////    TablePATTERNS.Columns.Remove(ROW_INDEX);
            ////    TableCUTS.Columns.Remove(ROW_INDEX);
            ////    TablePARTS_UDI.Columns.Remove(ROW_INDEX);
            ////    TablePARTS_REQ.Columns.Remove(ROW_INDEX);
            ////    TablePARTS_DST.Columns.Remove(ROW_INDEX);
            ////    TableOFFCUTS.Columns.Remove(ROW_INDEX);

            ////}



            //////foreach (var item in SpiltDatas)
            //////{
            //////    RebuildUSED(item.Datas);
            //////}
            //#endregion

            //foreach (var tmpItem in tuples)
            //{
            //    var tmpDatas = tmpItem.Item2;
            //    foreach (var item in SpiltDatas)
            //    {
            //        if (tmpItem.Item1 == item.MDBName)
            //        {
            //            DataSetCopy(tmpDatas, item.Datas);
            //            break;
            //        }
            //    }
            //}
            ////UpDate(SpiltDatas);
            //UpDate(SpiltDatas);
            #endregion

            return true;
        }

        private void RebuildUSED(DataSet Data)
        {
            var data = Data;
            int PartCount = 0;
            Dictionary<int, int> USED = new Dictionary<int, int>();
            var BOARDSTable = data.Tables[BOARDS];

            var PATTERNSTable = data.Tables[PATTERNS];
            BOARDSTable.DefaultView.Sort = BRD_INDEX + " ASC";

            PATTERNSTable.DefaultView.Sort = BRD_INDEX + " ASC";
            var BOARDSView = BOARDSTable.DefaultView;
            var PATTERNSView = PATTERNSTable.DefaultView;

            for (int i = 0; i < BOARDSView.Count; i++)
            {
                DataRowView BOARDSRowView = BOARDSView[i];
                int intBOARDS_BRD_INDEX = Convert.ToInt32(BOARDSRowView[BRD_INDEX]);
                int j = 0;
                PartCount = 0;
                for (; j < PATTERNSView.Count;)
                {

                    DataRowView PATTERNSRowView = PATTERNSView[i];
                    int intPATTERNS_BRD_INDEX = Convert.ToInt32(PATTERNSRowView[BRD_INDEX]);
                    if (intPATTERNS_BRD_INDEX == intBOARDS_BRD_INDEX)
                    {
                        PartCount += Convert.ToInt32(PATTERNSRowView["QTY_RUN"]);
                        j++;
                    }
                }
                BOARDSRowView["QTY_USED"] = PartCount;
            }
        }

        //public bool Spilt(string SourceMDB, List<string> DestFileMDBNames)
        //{
        //    int count = DestFileMDBNames.Count;
        //    //创建临时mdb文件
        //    if (!CreatedTmpMDB(SourceMDB))
        //    {
        //        return false;
        //    }
        //    //创建副本列表
        //    if (!CreatedMDBs(DestFileMDBNames))
        //    {
        //        return false;
        //    }
        //    var datas = GetDatas(SourceMDB);
        //    List<SpiltDatas> SpiltDatas = new List<SpiltDatas>();
        //    List<SpiltDatas> tmpSpiltDatas = new List<SpiltDatas>();
        //    foreach (var item in DestFileMDBNames)
        //    {
        //        SpiltDatas.Add(new SplitMDB.SpiltDatas() { MDBName = item, Datas = GetDatas(item) });
        //        tmpSpiltDatas.Add(new SplitMDB.SpiltDatas() { MDBName = item, Datas = datas.Clone() });
        //    }

        //    //拆分
        //    SpiltJOBS(datas, tmpSpiltDatas);
        //    SpiltNOTES(datas, tmpSpiltDatas);
        //    SpiltPATTERNS(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltBoards(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltMaterials(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltCuts(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltParts_UDI(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltParts_DST(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltParts_Req(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);
        //    SpiltOFFCUTS(datas, tmpSpiltDatas);
        //    //UpDate(SpiltDatas);

        //    //重构 BOARDS 与 PATTERNS 表的 BRD_INDEX


        //    foreach (var item in tmpSpiltDatas)
        //    {
        //        var ds = item.Datas;

        //        DataTable TableBOARDS = ds.Tables[BOARDS];
        //        DataTable TablePATTERNS = ds.Tables[PATTERNS];
        //        DataTable TableMATERIALS = ds.Tables[MATERIALS];
        //        DataTable TableCUTS = ds.Tables[CUTS];
        //        DataTable TablePARTS_UDI = ds.Tables[PARTS_UDI];
        //        DataTable TablePARTS_REQ = ds.Tables[PARTS_REQ];
        //        DataTable TablePARTS_DST = ds.Tables[PARTS_DST];
        //        DataTable TableOFFCUTS = ds.Tables[OFFCUTS];


        //        TablePATTERNS.Columns.Add(ROW_INDEX, typeof(int));
        //        TableCUTS.Columns.Add(ROW_INDEX, typeof(int));
        //        TablePARTS_UDI.Columns.Add(ROW_INDEX, typeof(int));
        //        TablePARTS_REQ.Columns.Add(ROW_INDEX, typeof(int));
        //        TablePARTS_DST.Columns.Add(ROW_INDEX, typeof(int));
        //        TableOFFCUTS.Columns.Add(ROW_INDEX, typeof(int));


        //        for (int i = 0; i < TableBOARDS.Rows.Count; i++)
        //        {
        //            var BOARDS_ROW = TableBOARDS.Rows[i];
        //            string strBRD_INDEX = BOARDS_ROW[BRD_INDEX].ToString().Trim();
        //            BOARDS_ROW[BRD_INDEX] = i + 1;
        //            foreach (DataRow row in TablePATTERNS.Rows)
        //            {
        //                if (strBRD_INDEX == row[BRD_INDEX].ToString().Trim())
        //                {
        //                    row[ROW_INDEX] = BOARDS_ROW[BRD_INDEX];
        //                }
        //            }
        //        }

        //        for (int i = 0; i < TablePATTERNS.Rows.Count;)
        //        {
        //            DataRow row = TablePATTERNS.Rows[i];
        //            row[BRD_INDEX] = row[ROW_INDEX];
        //            row[ROW_INDEX] = ++i;
        //        }

        //        for (int i = 0; i < TableMATERIALS.Rows.Count;)
        //        {
        //            DataRow row = TableMATERIALS.Rows[i];
        //            var resultRows = TablePARTS_REQ.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
        //            row[MAT_INDEX] = ++i;
        //            foreach (var resultRow in resultRows)
        //            {
        //                resultRow[MAT_INDEX] = row[MAT_INDEX];
        //            }
        //        }

        //        //设置 CUTS 序号，确保和 PATTERNS 一致
        //        for (int i = 0; i < TablePATTERNS.Rows.Count; i++)
        //        {
        //            DataRow row = TablePATTERNS.Rows[i];
        //            string strPTN_INDEX = row[PTN_INDEX].ToString();
        //            var resultRows = TableCUTS.Select($"{PTN_INDEX}={strPTN_INDEX}");
        //            foreach (var resultRow in resultRows)
        //            {
        //                resultRow[ROW_INDEX] = row[ROW_INDEX];
        //            }
        //        }
        //        // 设置 PARTS_UDI 序号
        //        TablePARTS_UDI.DefaultView.Sort = $"{PART_INDEX} ASC";
        //        for (int i = 0; i < TablePARTS_UDI.DefaultView.Count;)
        //        {
        //            DataRowView dataRow = TablePARTS_UDI.DefaultView[i];
        //            dataRow[ROW_INDEX] = ++i;
        //        }

        //        for (int i = 0; i < TablePARTS_UDI.Rows.Count; i++)
        //        {
        //            DataRow row = TablePARTS_UDI.Rows[i];
        //            var resultRows = TablePARTS_REQ.Select($"{PART_INDEX}={row[PART_INDEX]}");
        //            foreach (var resultRow in resultRows)
        //            {
        //                resultRow[ROW_INDEX] = row[ROW_INDEX];
        //            }
        //        }

        //        for (int i = 0; i < TablePARTS_UDI.Rows.Count; i++)
        //        {
        //            DataRow row = TablePARTS_UDI.Rows[i];
        //            var resultRows = TablePARTS_DST.Select($"{PART_INDEX}={row[PART_INDEX]}");
        //            foreach (var resultRow in resultRows)
        //            {
        //                resultRow[ROW_INDEX] = row[ROW_INDEX];
        //            }
        //        }
        //        TableOFFCUTS.DefaultView.Sort = $"{OFC_INDEX} ASC";
        //        for (int i = 0; i < TableOFFCUTS.DefaultView.Count;)
        //        {
        //            DataRowView dataRow = TableOFFCUTS.DefaultView[i];
        //            dataRow[ROW_INDEX] = ++i;
        //        }



        //        foreach (DataRow row in TablePATTERNS.Rows)
        //        {
        //            row[PTN_INDEX] = row[ROW_INDEX];
        //        }
        //        foreach (DataRow row in TableCUTS.Rows)
        //        {
        //            row[PTN_INDEX] = row[ROW_INDEX];
        //            string strPART_INDEX = row[PART_INDEX].ToString().Trim();
        //            foreach (DataRow PARTS_UDIRow in TablePARTS_UDI.Rows)
        //            {
        //                if (strPART_INDEX == PARTS_UDIRow[PART_INDEX].ToString().Trim())
        //                {
        //                    row[ROW_INDEX] = PARTS_UDIRow[ROW_INDEX];
        //                    break;
        //                }
        //            }
        //            if (strPART_INDEX.Contains("X"))
        //            {
        //                continue;
        //            }
        //            row[PART_INDEX] = row[ROW_INDEX];
        //        }

        //        foreach (DataRow row in TablePARTS_UDI.Rows)
        //        {
        //            row[PART_INDEX] = row[ROW_INDEX];
        //        }
        //        foreach (DataRow row in TablePARTS_REQ.Rows)
        //        {
        //            row[PART_INDEX] = row[ROW_INDEX];
        //        }
        //        foreach (DataRow row in TablePARTS_DST.Rows)
        //        {
        //            row[PART_INDEX] = row[ROW_INDEX];
        //        }

        //        foreach (DataRow row in TableCUTS.Rows)
        //        {
        //            string strPART_INDEX = row[PART_INDEX].ToString().Trim().ToUpper();
        //            if (strPART_INDEX.StartsWith("X"))
        //            {
        //                foreach (DataRow OFFCUTSRow in TableOFFCUTS.Rows)
        //                {
        //                    string strOFF_PART_INDEX = "X" + OFFCUTSRow[OFC_INDEX].ToString().Trim();
        //                    if (strOFF_PART_INDEX == strPART_INDEX)
        //                    {
        //                        row[PART_INDEX] = "X" + OFFCUTSRow[ROW_INDEX].ToString().Trim();
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        foreach (DataRow row in TableOFFCUTS.Rows)
        //        {
        //            row[OFC_INDEX] = row[ROW_INDEX];
        //        }


        //        TablePATTERNS.Columns.Remove(ROW_INDEX);
        //        TableCUTS.Columns.Remove(ROW_INDEX);
        //        TablePARTS_UDI.Columns.Remove(ROW_INDEX);
        //        TablePARTS_REQ.Columns.Remove(ROW_INDEX);
        //        TablePARTS_DST.Columns.Remove(ROW_INDEX);
        //        TableOFFCUTS.Columns.Remove(ROW_INDEX);

        //    }


        //    foreach (var tmpItem in tmpSpiltDatas)
        //    {
        //        var tmpDatas = tmpItem.Datas;
        //        foreach (var item in SpiltDatas)
        //        {
        //            if (tmpItem.MDBName == item.MDBName)
        //            {
        //                DataSetCopy(tmpDatas, item.Datas);
        //                var changes = item.Datas.GetChanges();
        //                break;
        //            }
        //        }
        //    }
        //    UpDate(SpiltDatas);

        //    return true;
        //}

        private void DataSetCopy(DataSet sourceDatas, DataSet destDatas)
        {
            foreach (DataTable sourceTable in sourceDatas.Tables)
            {
                string TableName = sourceTable.TableName;
                var destTable = destDatas.Tables[TableName];
                foreach (DataRow row in sourceTable.Rows)
                {
                    destTable.Rows.Add(row.ItemArray);
                }
            }
        }

        #region 拆分MDB文件

        private void SpiltJOBS(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            var JOBSTable = data.Tables[JOBS];
            foreach (var item in SpiltDatas)
            {
                var ds = item.Datas;
                foreach (DataRow row in JOBSTable.Rows)
                {
                    //row["ORD_DATE"] = row["CUT_DATE"] = DBNull.Value;

                    ds.Tables[JOBS].Rows.Add(row.ItemArray);
                }

            }
        }

        private void SpiltNOTES(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            var NOTESTable = data.Tables[NOTES];
            foreach (var item in SpiltDatas)
            {
                var ds = item.Datas;
                foreach (DataRow row in NOTESTable.Rows)
                {
                    ds.Tables[NOTES].Rows.Add(row.ItemArray);
                }

            }
        }

        private void SpiltPATTERNS(DataSet data, List<SpiltDatas> SpiltDatas)
        {

            int SpiltCount = SpiltDatas.Count;
            DataTable Sourcedt = data.Tables[PATTERNS];

            int RowCount = Sourcedt.Rows.Count;
            Sourcedt.DefaultView.Sort = $"{BRD_INDEX} ASC";
            if (RowCount < SpiltCount)//拆分数量大于当前数据的行数
            {
                return;
            }
            int[] SpiltRowCount = new int[SpiltCount];
            int AVGRowCount = RowCount / SpiltCount;
            int surpleRows = RowCount % SpiltCount;
            for (int i = 0; i < SpiltCount; i++)
            {
                SpiltRowCount[i] = AVGRowCount;
            }
            if (surpleRows > 0)
            {
                SpiltRowCount[SpiltCount - 1] = AVGRowCount + surpleRows;
            }
            int CurrSpiltRowIndex = 0;
            for (int i = 0; i < SpiltCount; i++)
            {
                var item = SpiltRowCount[i];
                int MaxRowIndex = CurrSpiltRowIndex + item;
                var dt = SpiltDatas[i].Datas.Tables[PATTERNS];//Sourcedt.Clone();
                for (; CurrSpiltRowIndex < MaxRowIndex; CurrSpiltRowIndex++)
                {
                    dt.Rows.Add(Sourcedt.DefaultView[CurrSpiltRowIndex].Row.ItemArray);
                }
            }

        }

        private void SpiltBoards(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string BOARDS = nameof(BOARDS);
            //string PATTERNS = nameof(PATTERNS);
            DataTable SourceTable = data.Tables[BOARDS];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string strBRD_INDEX = System.Convert.ToString(SourceTable.Rows[i][BRD_INDEX]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable TablePATTERNS = ds.Tables[PATTERNS];

                    var result = TablePATTERNS.Select($"{BRD_INDEX}='{strBRD_INDEX}'");
                    if (result.Length > 0)
                    {
                        ds.Tables[BOARDS].Rows.Add(SourceTable.Rows[i].ItemArray);
                    }
                }
            }

        }

        private void SpiltMaterials(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string MATERIALS = nameof(MATERIALS);
            //string BOARDS = nameof(BOARDS);
            DataTable SourceTable = data.Tables[MATERIALS];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string CODE = System.Convert.ToString(SourceTable.Rows[i][nameof(CODE)]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable BOARDS_Table = ds.Tables[BOARDS];

                    var result = BOARDS_Table.Select($"{nameof(CODE)}='{CODE}'");
                    if (result.Length > 0)
                    {
                        ds.Tables[MATERIALS].Rows.Add(SourceTable.Rows[i].ItemArray);
                    }
                }
            }
        }

        private void SpiltCuts(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string CUTS = nameof(CUTS);
            //string PATTERNS = nameof(PATTERNS);
            DataTable SourceTable = data.Tables[CUTS];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string strPTN_INDEX = System.Convert.ToString(SourceTable.Rows[i][PTN_INDEX]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable TablePATTERNS = ds.Tables[PATTERNS];

                    var result = TablePATTERNS.Select($"{PTN_INDEX}='{strPTN_INDEX}'");
                    if (result.Length > 0)
                    {
                        ds.Tables[CUTS].Rows.Add(SourceTable.Rows[i].ItemArray);
                    }
                }
            }
        }

        private void SpiltParts_UDI(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string PARTS_UDI = nameof(PARTS_UDI);
            //string CUTS = nameof(CUTS);
            DataTable SourceTable = data.Tables[PARTS_UDI];
            foreach (var SpiltData in SpiltDatas)
            {
                DataSet ds = SpiltData.Datas;
                DataTable TableCUTS = ds.Tables[CUTS];
                foreach (DataRow dataRow in TableCUTS.Rows)
                {
                    string value = dataRow[PART_INDEX].ToString().Trim();
                    int intPART_INDEX = 0;
                    try
                    {
                        intPART_INDEX = Convert.ToInt32(value);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    var result = SourceTable.Select($"{PART_INDEX}={intPART_INDEX}");
                    if (result.Length > 0)
                    {
                        foreach (var item in result)
                        {
                            ds.Tables[PARTS_UDI].Rows.Add(item.ItemArray);
                        }
                    }
                }
            }
        }

        private void SpiltParts_DST(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string PARTS_DST = nameof(PARTS_DST);
            //string PARTS_UDI = nameof(PARTS_UDI);
            DataTable SourceTable = data.Tables[PARTS_DST];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string strPART_INDEX = System.Convert.ToString(SourceTable.Rows[i][PART_INDEX]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable TableCUTS = ds.Tables[PARTS_UDI];

                    var result = TableCUTS.Select($"{PART_INDEX}='{strPART_INDEX}'");
                    if (result.Length > 0)
                    {
                        ds.Tables[PARTS_DST].Rows.Add(SourceTable.Rows[i].ItemArray);
                    }
                }
            }
        }

        private void SpiltParts_Req(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string PARTS_REQ = nameof(PARTS_REQ);
            //string PARTS_UDI = nameof(PARTS_UDI);
            DataTable SourceTable = data.Tables[PARTS_REQ];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string strPART_INDEX = System.Convert.ToString(SourceTable.Rows[i][PART_INDEX]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable TablePARTS_UDI = ds.Tables[PARTS_UDI];

                    var result = TablePARTS_UDI.Select($"{PART_INDEX}='{strPART_INDEX}'");
                    if (result.Length > 0)
                    {
                        ds.Tables[PARTS_REQ].Rows.Add(SourceTable.Rows[i].ItemArray);
                    }
                }
            }
        }

        private void SpiltOFFCUTS(DataSet data, List<SpiltDatas> SpiltDatas)
        {
            //string OFFCUTS = nameof(OFFCUTS);
            //string CUTS = nameof(CUTS);
            DataTable SourceTable = data.Tables[OFFCUTS];
            for (int i = 0; i < SourceTable.Rows.Count; i++)
            {
                string strPART_INDEX = "X" + System.Convert.ToString(SourceTable.Rows[i][OFC_INDEX]);
                foreach (var SpiltData in SpiltDatas)
                {
                    DataSet ds = SpiltData.Datas;
                    DataTable CUTS_Table = ds.Tables[CUTS];
                    var OFFCUTSDatas = CUTS_Table.Select($"{PART_INDEX} LIKE '%X%'");
                    foreach (DataRow row in OFFCUTSDatas)
                    {
                        if (row[PART_INDEX].ToString().Trim() == strPART_INDEX.Trim())
                        {
                            ds.Tables[OFFCUTS].Rows.Add(SourceTable.Rows[i].ItemArray);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
