using System;
using System.Collections.Generic;
using System.Data;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.MDB
{
    internal class MdbRebuild
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
        /// <summary>
        /// 重构MDB数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="patternList">锯切图的顺序</param>
        /// <returns></returns>
        public static Tuple<TimeSpan, Dictionary<int, int>> Rebuild(DataSet data,List<int> patternList /*,List<CuttingStackList> cuttingStackList*/)
        {
            DataTable tableJOBS = data.Tables[JOBS];
            DataTable tableBOARDS = data.Tables[BOARDS];
            DataTable tablePATTERNS = data.Tables[PATTERNS];
            DataTable tableMATERIALS = data.Tables[MATERIALS];
            DataTable tableCUTS = data.Tables[CUTS];
            DataTable tablePARTS_UDI = data.Tables[PARTS_UDI];
            DataTable tablePARTS_REQ = data.Tables[PARTS_REQ];
            DataTable tablePARTS_DST = data.Tables[PARTS_DST];
            DataTable tableOFFCUTS = data.Tables[OFFCUTS];

            tablePATTERNS.Columns.Add(ROW_INDEX, typeof(int));
            tableCUTS.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_UDI.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_REQ.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_DST.Columns.Add(ROW_INDEX, typeof(int));
            tableOFFCUTS.Columns.Add(ROW_INDEX, typeof(int));



            for (int i = 0; i < tableBOARDS.Rows.Count; i++)
            {
                var BOARDS_ROW = tableBOARDS.Rows[i];
                string initalValueBRD_INDEX = BOARDS_ROW[BRD_INDEX].ToString().Trim();
                BOARDS_ROW[BRD_INDEX] = i + 1;
                foreach (DataRow row in tablePATTERNS.Rows)
                {
                    if (initalValueBRD_INDEX == row[BRD_INDEX].ToString().Trim())
                    {
                        row[ROW_INDEX] = BOARDS_ROW[BRD_INDEX];
                    }
                }
            }

            for (int i = 0; i < tablePATTERNS.Rows.Count; i++)
            {
                DataRow row = tablePATTERNS.Rows[i];
                row[BRD_INDEX] = row[ROW_INDEX];
                //row[ROW_INDEX] = ++i;
            }

            //var groups = cuttingStackList.GroupBy(item => item.PatternName);
            //foreach (var group in groups)
            //{
            //    var lst = group.ToList();
            //    int newPTN_INDEX = Convert.ToInt32(group.Key);
            //    int cycle = 0;
            //    if (lst.Count > 4)
            //    {
            //        cycle = lst.Count / 4 + (lst.Count % 4 > 0 ? 1 : 0);
            //    }
            //    else
            //    {
            //        cycle = 1;
            //    }
            //    foreach (DataRow row in tablePATTERNS.Rows)
            //    {
            //        if (Convert.ToInt32(row[PTN_INDEX].ToString().Trim()) == newPTN_INDEX)
            //        {
            //            row["QTY_RUN"] = lst.Count;
            //            row["QTY_CYCLES"] = cycle;
            //            row["TOTAL_TIME"] = cycle * Convert.ToInt32(row["CYCLE_TIME"].ToString());
            //            break;
            //        }
            //    }

            //    if (lst.Count > 1)
            //    {
            //        for (int i = 1; i < lst.Count; i++)
            //        {
            //            cuttingStackList.Remove(lst[i]);
            //        }
            //    }
            //}

            //cuttingStackList.Sort((x, y) => x.StackIndex.CompareTo(y.StackIndex));

            Dictionary<int, int> dic = new Dictionary<int, int>();

            for (int i = 0; i < patternList.Count; i++)
            {
                int newPTN_INDEX = patternList[i];//Convert.ToInt32(cuttingStackList[i].PatternName);
                foreach (DataRow row in tablePATTERNS.Rows)
                {
                    if (Convert.ToInt32(row[PTN_INDEX].ToString().Trim()) == newPTN_INDEX)
                    {
                        row[ROW_INDEX] = i + 1;
                        dic.Add(i + 1, newPTN_INDEX);
                        break;
                    }
                }
            }

            for (int i = 0; i < tableMATERIALS.Rows.Count;)
            {
                DataRow row = tableMATERIALS.Rows[i];
                var resultRows = tablePARTS_REQ.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                var boardRows = tableBOARDS.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                var offcutsRows = tableOFFCUTS.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                row[MAT_INDEX] = ++i;
                foreach (var resultRow in resultRows)
                {
                    resultRow[MAT_INDEX] = row[MAT_INDEX];
                    
                }

                foreach (var boradRow in boardRows)
                {
                    boradRow[MAT_INDEX] = row[MAT_INDEX];
                }

                foreach (var offcutsRow in offcutsRows)
                {
                    offcutsRow[MAT_INDEX] = row[MAT_INDEX];
                }
            }

            //设置 CUTS 序号，确保和 PATTERNS 一致
            for (int i = 0; i < tablePATTERNS.Rows.Count; i++)
            {
                DataRow row = tablePATTERNS.Rows[i];
                string strPTN_INDEX = row[PTN_INDEX].ToString();
                var resultRows = tableCUTS.Select($"{PTN_INDEX}={strPTN_INDEX}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }
            // 设置 PARTS_UDI 序号
            tablePARTS_UDI.DefaultView.Sort = $"{PART_INDEX} ASC";
            for (int i = 0; i < tablePARTS_UDI.DefaultView.Count;)
            {
                DataRowView dataRow = tablePARTS_UDI.DefaultView[i];
                dataRow[ROW_INDEX] = ++i;
            }

            for (int i = 0; i < tablePARTS_UDI.Rows.Count; i++)
            {
                DataRow row = tablePARTS_UDI.Rows[i];
                var resultRows = tablePARTS_REQ.Select($"{PART_INDEX}={Convert.ToInt16(row[PART_INDEX])}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }

            for (int i = 0; i < tablePARTS_UDI.Rows.Count; i++)
            {
                DataRow row = tablePARTS_UDI.Rows[i];
                var resultRows = tablePARTS_DST.Select($"{PART_INDEX}={row[PART_INDEX]}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }
            tableOFFCUTS.DefaultView.Sort = $"{OFC_INDEX} ASC";
            for (int i = 0; i < tableOFFCUTS.DefaultView.Count;)
            {
                DataRowView dataRow = tableOFFCUTS.DefaultView[i];
                dataRow[ROW_INDEX] = ++i;
            }

            foreach (DataRow row in tablePATTERNS.Rows)
            {
                row[PTN_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tableCUTS.Rows)
            {
                row[PTN_INDEX] = row[ROW_INDEX];
                string strPART_INDEX = row[PART_INDEX].ToString().Trim();
                if (strPART_INDEX == "0")
                {
                    row[PART_INDEX] = 0;
                    continue;
                }
                if (strPART_INDEX.Contains("X"))
                {
                    continue;
                }
                foreach (DataRow PARTS_UDIRow in tablePARTS_UDI.Rows)
                {
                    if (strPART_INDEX == PARTS_UDIRow[PART_INDEX].ToString().Trim())
                    {
                        row[ROW_INDEX] = Convert.ToInt32(PARTS_UDIRow[ROW_INDEX]);
                        break;
                    }
                }
                row[PART_INDEX] = row[ROW_INDEX];
            }

            foreach (DataRow row in tablePARTS_UDI.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tablePARTS_REQ.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tablePARTS_DST.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }

            foreach (DataRow row in tableCUTS.Rows)
            {
                string strPART_INDEX = row[PART_INDEX].ToString().Trim().ToUpper();
                strPART_INDEX = strPART_INDEX.Replace(" ", "");
                if (strPART_INDEX.StartsWith("X"))
                {
                    foreach (DataRow OFFCUTSRow in tableOFFCUTS.Rows)
                    {
                        string strOFF_PART_INDEX = "X" + OFFCUTSRow[OFC_INDEX].ToString().Trim();
                        if (strOFF_PART_INDEX == strPART_INDEX)
                        {
                            row[PART_INDEX] = "X" + OFFCUTSRow[ROW_INDEX].ToString().Trim();
                            break;
                        }
                    }
                }
            }

            foreach (DataRow row in tableOFFCUTS.Rows)
            {
                row[OFC_INDEX] = row[ROW_INDEX];
            }

            int totalTime = 0;
            foreach (DataRow row in tablePATTERNS.Rows)
            {
                totalTime += Convert.ToInt32(row["TOTAL_TIME"]);
            }

            foreach (DataRow row in tableJOBS.Rows)
            {
                int JOB_INDEX = Convert.ToInt32(row[nameof(JOB_INDEX)].ToString().Trim());
                int jobTotalTime = 0;
                foreach (DataRow pRow in tablePATTERNS.Rows)
                {
                    int pJob_Index = Convert.ToInt32(pRow[nameof(JOB_INDEX)].ToString().Trim());
                    if (pJob_Index == JOB_INDEX)
                    {
                        jobTotalTime += Convert.ToInt32(pRow["TOTAL_TIME"]);
                    }
                }

                row["CUT_TIME"] = jobTotalTime;
            }

            RebuildUsed(data);
            DateTime tmpTime = DateTime.Now;
            var ts = tmpTime.AddSeconds(totalTime) - tmpTime;

            tablePATTERNS.Columns.Remove(ROW_INDEX);
            tableCUTS.Columns.Remove(ROW_INDEX);
            tablePARTS_UDI.Columns.Remove(ROW_INDEX);
            tablePARTS_REQ.Columns.Remove(ROW_INDEX);
            tablePARTS_DST.Columns.Remove(ROW_INDEX);
            tableOFFCUTS.Columns.Remove(ROW_INDEX);
            foreach (DataRow row in tableCUTS.Rows)
            {
                string s = row[PART_INDEX].ToString();
                int addSpaceCount = 5 - s.Length;
                for (int i = 0; i < addSpaceCount; i++)
                {
                    s = " " + s;
                }

                row[PART_INDEX] = s;
            }
            return new Tuple<TimeSpan, Dictionary<int, int>>(ts, dic);
            //return ts;
        }

        /// <summary>
        /// 重构MDB数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="patternList">锯切图的顺序</param>
        /// <returns></returns>
        public static void Rebuild(DataSet data)
        {
            DataTable tableJOBS = data.Tables[JOBS];
            DataTable tableBOARDS = data.Tables[BOARDS];
            DataTable tablePATTERNS = data.Tables[PATTERNS];
            DataTable tableMATERIALS = data.Tables[MATERIALS];
            DataTable tableCUTS = data.Tables[CUTS];
            DataTable tablePARTS_UDI = data.Tables[PARTS_UDI];
            DataTable tablePARTS_REQ = data.Tables[PARTS_REQ];
            DataTable tablePARTS_DST = data.Tables[PARTS_DST];
            DataTable tableOFFCUTS = data.Tables[OFFCUTS];

            tablePATTERNS.Columns.Add(ROW_INDEX, typeof(int));
            tableCUTS.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_UDI.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_REQ.Columns.Add(ROW_INDEX, typeof(int));
            tablePARTS_DST.Columns.Add(ROW_INDEX, typeof(int));
            tableOFFCUTS.Columns.Add(ROW_INDEX, typeof(int));



            for (int i = 0; i < tableBOARDS.Rows.Count; i++)
            {
                var BOARDS_ROW = tableBOARDS.Rows[i];
                string initalValueBRD_INDEX = BOARDS_ROW[BRD_INDEX].ToString().Trim();
                BOARDS_ROW[BRD_INDEX] = i + 1;
                foreach (DataRow row in tablePATTERNS.Rows)
                {
                    if (initalValueBRD_INDEX == row[BRD_INDEX].ToString().Trim())
                    {
                        row[ROW_INDEX] = BOARDS_ROW[BRD_INDEX];
                    }
                }
            }

            for (int i = 0; i < tablePATTERNS.Rows.Count; i++)
            {
                DataRow row = tablePATTERNS.Rows[i];
                row[BRD_INDEX] = row[ROW_INDEX];
                //row[ROW_INDEX] = ++i;
            }

            

            //Dictionary<int, int> dic = new Dictionary<int, int>();

            //for (int i = 0; i < patternList.Count; i++)
            //{
            //    int newPTN_INDEX = patternList[i];//Convert.ToInt32(cuttingStackList[i].PatternName);
            //    foreach (DataRow row in tablePATTERNS.Rows)
            //    {
            //        if (Convert.ToInt32(row[PTN_INDEX].ToString().Trim()) == newPTN_INDEX)
            //        {
            //            row[ROW_INDEX] = i + 1;
            //            dic.Add(i + 1, newPTN_INDEX);
            //            break;
            //        }
            //    }
            //}

            for (int i = 0; i < tableMATERIALS.Rows.Count;)
            {
                DataRow row = tableMATERIALS.Rows[i];
                var resultRows = tablePARTS_REQ.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                var boardRows = tableBOARDS.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                var offcutsRows = tableOFFCUTS.Select($"{MAT_INDEX}={row[MAT_INDEX]}");
                row[MAT_INDEX] = ++i;
                foreach (var resultRow in resultRows)
                {
                    resultRow[MAT_INDEX] = row[MAT_INDEX];

                }

                foreach (var boradRow in boardRows)
                {
                    boradRow[MAT_INDEX] = row[MAT_INDEX];
                }

                foreach (var offcutsRow in offcutsRows)
                {
                    offcutsRow[MAT_INDEX] = row[MAT_INDEX];
                }
            }

            //设置 CUTS 序号，确保和 PATTERNS 一致
            for (int i = 0; i < tablePATTERNS.Rows.Count; i++)
            {
                DataRow row = tablePATTERNS.Rows[i];
                string strPTN_INDEX = row[PTN_INDEX].ToString();
                var resultRows = tableCUTS.Select($"{PTN_INDEX}={strPTN_INDEX}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }
            // 设置 PARTS_UDI 序号
            tablePARTS_UDI.DefaultView.Sort = $"{PART_INDEX} ASC";
            for (int i = 0; i < tablePARTS_UDI.DefaultView.Count;)
            {
                DataRowView dataRow = tablePARTS_UDI.DefaultView[i];
                dataRow[ROW_INDEX] = ++i;
            }

            for (int i = 0; i < tablePARTS_UDI.Rows.Count; i++)
            {
                DataRow row = tablePARTS_UDI.Rows[i];
                var resultRows = tablePARTS_REQ.Select($"{PART_INDEX}={Convert.ToInt16(row[PART_INDEX])}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }

            for (int i = 0; i < tablePARTS_UDI.Rows.Count; i++)
            {
                DataRow row = tablePARTS_UDI.Rows[i];
                var resultRows = tablePARTS_DST.Select($"{PART_INDEX}={row[PART_INDEX]}");
                foreach (var resultRow in resultRows)
                {
                    resultRow[ROW_INDEX] = row[ROW_INDEX];
                }
            }
            tableOFFCUTS.DefaultView.Sort = $"{OFC_INDEX} ASC";
            for (int i = 0; i < tableOFFCUTS.DefaultView.Count;)
            {
                DataRowView dataRow = tableOFFCUTS.DefaultView[i];
                dataRow[ROW_INDEX] = ++i;
            }

            foreach (DataRow row in tablePATTERNS.Rows)
            {
                row[PTN_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tableCUTS.Rows)
            {
                row[PTN_INDEX] = row[ROW_INDEX];
                string strPART_INDEX = row[PART_INDEX].ToString().Trim();
                if (strPART_INDEX == "0")
                {
                    row[PART_INDEX] = 0;
                    continue;
                }
                if (strPART_INDEX.Contains("X"))
                {
                    continue;
                }
                foreach (DataRow PARTS_UDIRow in tablePARTS_UDI.Rows)
                {
                    if (strPART_INDEX == PARTS_UDIRow[PART_INDEX].ToString().Trim())
                    {
                        row[ROW_INDEX] = Convert.ToInt32(PARTS_UDIRow[ROW_INDEX]);
                        break;
                    }
                }
                row[PART_INDEX] = row[ROW_INDEX];
            }

            foreach (DataRow row in tablePARTS_UDI.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tablePARTS_REQ.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }
            foreach (DataRow row in tablePARTS_DST.Rows)
            {
                row[PART_INDEX] = row[ROW_INDEX];
            }

            foreach (DataRow row in tableCUTS.Rows)
            {
                string strPART_INDEX = row[PART_INDEX].ToString().Trim().ToUpper();
                strPART_INDEX = strPART_INDEX.Replace(" ", "");
                if (strPART_INDEX.StartsWith("X"))
                {
                    foreach (DataRow OFFCUTSRow in tableOFFCUTS.Rows)
                    {
                        string strOFF_PART_INDEX = "X" + OFFCUTSRow[OFC_INDEX].ToString().Trim();
                        if (strOFF_PART_INDEX == strPART_INDEX)
                        {
                            row[PART_INDEX] = "X" + OFFCUTSRow[ROW_INDEX].ToString().Trim();
                            break;
                        }
                    }
                }
            }

            foreach (DataRow row in tableOFFCUTS.Rows)
            {
                row[OFC_INDEX] = row[ROW_INDEX];
            }

            int totalTime = 0;
            foreach (DataRow row in tablePATTERNS.Rows)
            {
                totalTime += Convert.ToInt32(row["TOTAL_TIME"]);
            }

            foreach (DataRow row in tableJOBS.Rows)
            {
                int JOB_INDEX = Convert.ToInt32(row[nameof(JOB_INDEX)].ToString().Trim());
                int jobTotalTime = 0;
                foreach (DataRow pRow in tablePATTERNS.Rows)
                {
                    int pJob_Index = Convert.ToInt32(pRow[nameof(JOB_INDEX)].ToString().Trim());
                    if (pJob_Index == JOB_INDEX)
                    {
                        jobTotalTime += Convert.ToInt32(pRow["TOTAL_TIME"]);
                    }
                }

                row["CUT_TIME"] = jobTotalTime;
            }

            RebuildUsed(data);
            DateTime tmpTime = DateTime.Now;
            var ts = tmpTime.AddSeconds(totalTime) - tmpTime;

            tablePATTERNS.Columns.Remove(ROW_INDEX);
            tableCUTS.Columns.Remove(ROW_INDEX);
            tablePARTS_UDI.Columns.Remove(ROW_INDEX);
            tablePARTS_REQ.Columns.Remove(ROW_INDEX);
            tablePARTS_DST.Columns.Remove(ROW_INDEX);
            tableOFFCUTS.Columns.Remove(ROW_INDEX);
            foreach (DataRow row in tableCUTS.Rows)
            {
                string s = row[PART_INDEX].ToString();
                int addSpaceCount = 5 - s.Length;
                for (int i = 0; i < addSpaceCount; i++)
                {
                    s = " " + s;
                }

                row[PART_INDEX] = s;
            }
            
        }

        /// <summary>
        /// 重算板件数
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private static void RebuildUsed(DataSet Data)
        {
            var data = Data;
            //int totallyTime = 0;
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
                for (; j < PATTERNSView.Count; j++)
                {

                    DataRowView PATTERNSRowView = PATTERNSView[j];
                    int intPATTERNS_BRD_INDEX = Convert.ToInt32(PATTERNSRowView[BRD_INDEX]);
                    if (intPATTERNS_BRD_INDEX == intBOARDS_BRD_INDEX)
                    {
                        PartCount += Convert.ToInt32(PATTERNSRowView["QTY_RUN"]);
                        //totallyTime+= Convert.ToInt32(PATTERNSRowView["TOTAL_TIME"]);
                        //j++;
                    }
                    //else
                    //{
                    //    int t = 10;
                    //}
                }
                BOARDSRowView["QTY_USED"] = PartCount;
            }

            //return totallyTime;
        }



    }
}
