using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.Domain.Extension;
using NJIS.FPZWS.LineControl.Cutting.Domain.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.MDB
{
    internal class MdbOperator
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

        private const string PTN_INDEX = nameof(PTN_INDEX);

        private const string JOB_INDEX = nameof(JOB_INDEX);
        private const string BRD_INDEX = nameof(BRD_INDEX);
        private const string MAT_INDEX = nameof(MAT_INDEX);
        private const string PART_INDEX = nameof(PART_INDEX);
        private const string OFC_INDEX = nameof(OFC_INDEX);
        private const string LENGTH = nameof(LENGTH);
        private const string WIDTH = nameof(WIDTH);
        private const string THICK = nameof(THICK);
        private const string CODE = nameof(CODE);
        private const string MAX_BOOK = nameof(MAX_BOOK);
        private const string TITLE = nameof(TITLE);
        private const string OFC_QTY = nameof(OFC_QTY);

        private const string TOTAL_TIME = nameof(TOTAL_TIME);
        private const string QTY_RUN = nameof(QTY_RUN);
        private const string INFO30 = nameof(INFO30);
        private const string INFO23 = nameof(INFO23);
        private const string INFO24 = nameof(INFO24);
        private const string NOTE_INDEX = nameof(NOTE_INDEX);
        private const string QTY_PARTS = nameof(QTY_PARTS);
        private const string QTY_RPT = nameof(QTY_RPT);
        private const string QTY_CYCLES = nameof(QTY_CYCLES);
        private const string CUT_INDEX = nameof(CUT_INDEX);
        private const string TemplateMdbName = @"MDBTemplate\Template.mdb";

        public static List<PatternAllInfo> GetMdbData(string mdbName)
        {
            List<PatternAllInfo> patternAllInfos = new List<PatternAllInfo>();
            AccessDB db = new AccessDB(mdbName);
            try
            {
                var ds = db.GetDatas();
                db.Dispose();
                patternAllInfos.AddRange(GetPatternAllInfos(ds));
                return patternAllInfos;
            }
            catch (Exception e)
            {
                db.Dispose();
                throw;
            }

            
        }


        public static List<PatternAllInfo> GetPatternAllInfos(DataSet ds, bool deleteSource = false)
        {
            List<PatternAllInfo> patternAllInfos = new List<PatternAllInfo>();

            List<Tuple<int, DataSet>> tuples = GetPatternsDatas(ds, deleteSource);
            tuples.ForEach(tuple => patternAllInfos.Add(ConvertPatternAllInfo(tuple.Item2)));
            return patternAllInfos;
        }

        /// <summary>
        /// 提取每个锯切图的DataSet
        /// </summary>
        /// <param name="ds"> 总mdb的DataSet</param>
        /// <returns></returns>
        public static List<Tuple<int, DataSet>> GetPatternsDatas(DataSet ds, bool deleteSource = false)
        {
            List<Tuple<int, DataSet>> tuples = new List<Tuple<int, DataSet>>();
            var patternlist = ds.Tables[PATTERNS].ConvertAll<int>(row => Convert.ToInt32(row[PTN_INDEX]));
            patternlist.ForEach(item => tuples.Add(new Tuple<int, DataSet>(item, GetPatternDatas(item, ds, deleteSource))));
            return tuples;
        }

        /// <summary>
        /// 提取锯切图的DataSet
        /// </summary>
        /// <param name="pattern">锯切图索引</param>
        /// <param name="ds">mdb数据</param>
        /// <param name="deleteSource">提取完是否删除</param>
        /// <returns></returns>
        public static DataSet GetPatternDatas(int pattern, DataSet ds, bool deleteSource = false)
        {
            var cloneDataSet = ds.Clone();
            //获取 Pattern 的行数据
            var patternRowData = ds.Tables[PATTERNS]
                .FindRowFirstOrDefault(row => pattern == Convert.ToInt32(row[PTN_INDEX]), deleteSource);
            //if (patternRowData == null) return cloneDataSet;
            var patternRow = cloneDataSet.Tables[PATTERNS].Rows.Add(patternRowData);


            int jobIndex = Convert.ToInt32(patternRow[JOB_INDEX]);
            int brdIndex = Convert.ToInt32(patternRow[BRD_INDEX]);

            //获取 Board 的行数据
            var boardRowData = ds.Tables[BOARDS].FindRowFirstOrDefault(row =>
                jobIndex == Convert.ToInt32(row[JOB_INDEX])
                && brdIndex == Convert.ToInt32(row[BRD_INDEX])
                , false);
            //if (boardRowData == null) return cloneDataSet;
            var boardRow = cloneDataSet.Tables[BOARDS].Rows.Add(boardRowData);


            var matIndex = Convert.ToInt32(boardRow[MAT_INDEX]);
            //获取 Material 的行数据(不需要删除)
            var materialRowData = ds.Tables[MATERIALS].FindRowFirstOrDefault(row =>
                jobIndex == Convert.ToInt32(row[JOB_INDEX])
                && matIndex == Convert.ToInt32(row[MAT_INDEX]), false);
            var materialRow = cloneDataSet.Tables[MATERIALS].Rows.Add(materialRowData);


            //获取 Header 的行数据(数据不需要删除)
            var headerRowData = ds.Tables[HEADER].FindRowFirstOrDefault(row => true, false);
            var headerRow = cloneDataSet.Tables[HEADER].Rows.Add(headerRowData);

            //获取 Jobs 的行数据(不需要删除)
            var jobRowData = ds.Tables[JOBS].FindRowFirstOrDefault(row => jobIndex == Convert.ToInt32(row[JOB_INDEX]), false);
            var jobRow = cloneDataSet.Tables[JOBS].Rows.Add(jobRowData);



            //获取 Notes 的行数据(不需要删除)
            var notesRowData = ds.Tables[NOTES].FindRowFirstOrDefault(row =>
                jobIndex == Convert.ToInt32(row[JOB_INDEX]), false);
            //if (notesRowData == null) return cloneDataSet;
            var notesRow = cloneDataSet.Tables[NOTES].Rows.Add(notesRowData);

            //获取 Cuts 的行数据
            var cutsRowDatas = ds.Tables[CUTS].FindAllRows(row =>
                jobIndex == Convert.ToInt32(row[JOB_INDEX])
                && pattern == Convert.ToInt32(row[PTN_INDEX]), deleteSource);
            cutsRowDatas.ForEach(rowData => cloneDataSet.Tables[CUTS].Rows.Add(rowData));

            //获取工件清单的索引
            List<int> partIndexs = cloneDataSet.Tables[CUTS].ConvertAll<int>(row =>
            {
                string partIndexStr = row[PART_INDEX].ToString().Trim();
                if (!partIndexStr.Contains("X".ToUpper()))
                {
                    return Convert.ToInt32(partIndexStr);
                }

                return 0;
            });
            //移除等于0的索引项
            partIndexs.RemoveAll(item => item == 0);
            //获取余板清单的索引
            List<Tuple<int, int>> tuples = cloneDataSet.Tables[CUTS].ConvertAll<Tuple<int, int>>(row =>
            {
                int offcutIndex = 0;
                int offcutCount = 0;
                string partIndexStr = row[PART_INDEX].ToString().Trim();
                if (partIndexStr.Contains("X".ToUpper()))
                {
                    offcutIndex = Convert.ToInt32(partIndexStr.Replace("X".ToUpper(), ""));
                    offcutCount = Convert.ToInt32(row[QTY_PARTS].ToString());
                }
                Tuple<int, int> tuple = new Tuple<int, int>(offcutIndex, offcutCount);
                return tuple;
            });
            tuples.RemoveAll(tuple => tuple.Item1 == 0);
            List<int> offPartIndexs = tuples.ConvertAll<int>(tuple => tuple.Item1);
            offPartIndexs.RemoveAll(item => item == 0);

            //获取 Offcuts 的行的数据
            var offcutsRowDatas = ds.Tables[OFFCUTS].FindAllRows(row =>
                offPartIndexs.Contains(Convert.ToInt32(row[OFC_INDEX]))
                && jobIndex == Convert.ToInt32(row[JOB_INDEX])
                && matIndex == Convert.ToInt32(row[MAT_INDEX]), false);

            offcutsRowDatas.ForEach(rowData => cloneDataSet.Tables[OFFCUTS].Rows.Add(rowData));



            //获取 Part_DST 的行的数据
            var partDstRowDatas = ds.Tables[PARTS_DST].FindAllRows(row =>
                partIndexs.Contains(Convert.ToInt32(row[PART_INDEX]))
                && jobIndex == Convert.ToInt32(row[JOB_INDEX])
                , deleteSource);
            partDstRowDatas.ForEach(rowData => cloneDataSet.Tables[PARTS_DST].Rows.Add(rowData));

            //获取 Part_REQ 的行的数据
            var partReqRowDatas = ds.Tables[PARTS_REQ].FindAllRows(row =>
                partIndexs.Contains(Convert.ToInt32(row[PART_INDEX]))
                && jobIndex == Convert.ToInt32(row[JOB_INDEX])
                && matIndex == Convert.ToInt32(row[MAT_INDEX]), deleteSource);
            partReqRowDatas.ForEach(rowData => cloneDataSet.Tables[PARTS_REQ].Rows.Add(rowData));

            //获取 Part_UDI 的行的数据
            var partUdiRowDatas = ds.Tables[PARTS_UDI].FindAllRows(row =>
                partIndexs.Contains(Convert.ToInt32(row[PART_INDEX]))
                && jobIndex == Convert.ToInt32(row[JOB_INDEX])
                , deleteSource);
            partUdiRowDatas.ForEach(rowData => cloneDataSet.Tables[PARTS_UDI].Rows.Add(rowData));

            // 更新提取的Offcut表的余板数量
            //计算局切图的各种余板数量
            foreach (DataRow row in cloneDataSet.Tables[OFFCUTS].Rows)
            {
                var index = Convert.ToInt32(row[OFC_INDEX].ToString());
                row[OFC_QTY] = tuples.Find(tuple => tuple.Item1 == index).Item2;
            }

            return cloneDataSet;
        }

        /// <summary>
        /// 获取锯切图的统计数据
        /// </summary>
        /// <param name="ds">锯切图的DataSet</param>
        /// <returns></returns>
        private static PatternAllInfo ConvertPatternAllInfo(DataSet ds)
        {
            try
            {
                string materialCode = ds.Tables[MATERIALS].Rows[0][CODE].ToString().Trim();
                double bookThick = Convert.ToDouble(ds.Tables[MATERIALS].Rows[0][THICK]);
                string batchName = Convert.ToString(ds.Tables[HEADER].Rows[0][TITLE]);
                PatternAllInfo patternAllInfo = new PatternAllInfo();
                var partInfoList = ds.Tables[PARTS_UDI].ConvertAll(row =>
                    new WorkpieceInfo
                    {
                        PartId = row[INFO30].ToString().Trim(),
                        IsOffPart = false,
                        Thickness = bookThick,
                        Length = Convert.ToDouble(row[INFO23].ToString().Trim()),
                        Width = Convert.ToDouble(row[INFO24].ToString().Trim()),
                        PartCount = 1
                    });

                var offPartInfoList = ds.Tables[OFFCUTS].ConvertAll(row =>
                    new WorkpieceInfo
                    {
                        PartId = row[CODE].ToString().Trim(),
                        IsOffPart = true,
                        Thickness = bookThick,
                        Length = Convert.ToDouble(row[LENGTH].ToString().Trim()),
                        Width = Convert.ToDouble(row[WIDTH].ToString().Trim()),
                        PartCount = Convert.ToInt32(row[OFC_QTY].ToString().Trim())
                    });
                patternAllInfo.WorkpieceInfos = partInfoList.Concat(offPartInfoList).ToList();

                patternAllInfo.PatternInfo = new PatternInfo()
                {
                    Pattern = Convert.ToInt32(ds.Tables[PATTERNS].Rows[0][PTN_INDEX]),
                    BookLength = Convert.ToDouble(ds.Tables[BOARDS].Rows[0][LENGTH]),
                    BookWidth = Convert.ToDouble(ds.Tables[BOARDS].Rows[0][WIDTH]),
                    BookThick = bookThick,
                    MaterialCode = materialCode,
                    TotalBookCount = Convert.ToInt32(ds.Tables[PATTERNS].Rows[0][QTY_RUN]),
                    NormalPartCount = ds.Tables[PARTS_UDI].Rows.Count,
                    OffPartCount = ds.Tables[OFFCUTS].ConvertAll(row => Convert.ToInt32(row[OFC_QTY].ToString())).Sum(),
                    NormalArea = partInfoList.Sum(item => item.Area),
                    OffArea = offPartInfoList.Sum(item => item.Area),
                    BatchName = batchName,
                    TotalTime = Convert.ToInt32(ds.Tables[PATTERNS].Rows[0][TOTAL_TIME]),
                };
                return patternAllInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// 检查mdb格式是否需要转换
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static bool CheckNeedConvert(DataSet ds)
        {
            foreach (DataRow row in ds.Tables[PATTERNS].Rows)
            {
                var bookNum = Convert.ToInt32(row[QTY_RUN].ToString());
                var maxBookNum = Convert.ToInt32(row[MAX_BOOK].ToString());
                if (bookNum > maxBookNum) return true;
            }
            return false;
        }

        /// <summary>
        /// mdb数据格式转换
        /// </summary>
        /// <param name="mdbName"></param>
        public static void ConvertMdb(string mdbName)
        {

            AccessDB db = new AccessDB(mdbName);
            try
            {
                var ds = db.GetDatas();
                db.Dispose();
                if (!CheckNeedConvert(ds)) return;
                var tuples = GetPatternsDatas(ds);
                //锯切图格式转换
                var tmpList = ConvertMdbFormat(tuples.ConvertAll(tuple => tuple.Item2));
                //生成新锯切图索引
                List<int> convertIndexList = new List<int>();
                for (int i = 1; i <= tmpList.Count; i++)
                {
                    convertIndexList.Add(i);
                }
                //合并新格式Mdb
                var convertCombineMdb = CombineMdbs(tmpList);
                //重构新格式Mdb
                MdbRebuild.Rebuild(convertCombineMdb, convertIndexList);
                //保存新格式Mdb数据
                string newFileName = string.Empty;
                UpdateDatas(convertCombineMdb, out newFileName);
                File.Move(mdbName, mdbName.Replace(".mdb","源.mdb"));
                File.Move(newFileName, mdbName);
            }
            catch (Exception e)
            {
                db.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 锯切图的数据格式转换(堆叠拆解)
        /// </summary>
        /// <param name="destDataSets"></param>
        /// <returns></returns>
        private static List<DataSet> ConvertMdbFormat(List<DataSet> destDataSets)
        {
            List<Tuple<int, DataSet>> list = new List<Tuple<int, DataSet>>();
            foreach (var ds in destDataSets)
            {
                try
                {
                    list.AddRange(MdbFormatConverter(ds));
                }
                catch (Exception e)
                {
                    throw;
                }

            }
            //destDataSets.ForEach(ds=> list.AddRange(MdbFormatConverter(ds)));
            list.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            for (int i = 0; i < list.Count; i++)
            {
                UpdatePtnIndex(list[i].Item2, (short)(i + 1));
            }

            return list.ConvertAll(tuple => tuple.Item2);
        }

        /// <summary>
        /// 更新锯切图索引(锯切图号)
        /// </summary>
        /// <param name="dataSetUnity"></param>
        /// <param name="nPtnIndex"></param>
        /// <returns></returns>
        public static bool UpdatePtnIndex(DataSet dataSetUnity, short nPtnIndex)
        {
            foreach (DataRow row in dataSetUnity.Tables[CUTS].Rows)
            {
                row[PTN_INDEX] = nPtnIndex;
            }
            foreach (DataRow row in dataSetUnity.Tables[PATTERNS].Rows)
            {
                row[PTN_INDEX] = nPtnIndex;
            }
            return true;
        }

        /// <summary>
        /// 锯切图的数据格式转换(堆叠拆解)
        /// </summary>
        /// <param name="destDataSet"></param>
        /// <param name="maxBook"></param>
        /// <returns></returns>
        public static List<Tuple<int, DataSet>> MdbFormatConverter(DataSet destDataSet,int maxBook=0)
        {
            List<DataSet> dataSets = new List<DataSet>();
            int ptnIndex = Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][PTN_INDEX].ToString());
            var bookCount = Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
            maxBook = maxBook<=0? Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][MAX_BOOK].ToString()): maxBook;
            //var maxBook = 4;
            if (bookCount <= maxBook)
            {
                dataSets.Add(destDataSet.Copy());
            }
            else
            {
                //var tmpDataSet = destDataSet.Copy();
                var cycle = bookCount / maxBook + (bookCount % maxBook > 0 ? 1 : 0);
                for (int i = 0; i < cycle; i++)
                {
                    int unUsedBookCount = bookCount - i * maxBook;
                    int curCycleBookCount = unUsedBookCount > maxBook ? maxBook : unUsedBookCount;
                    var ds = destDataSet.Copy();
                    ds.Tables[CUTS].Clear();
                    ds.Tables[CUTS].AcceptChanges();
                    ds.Tables[PATTERNS].Rows[0][QTY_RUN] = curCycleBookCount;
                    ds.Tables[PATTERNS].Rows[0][QTY_CYCLES] = 1;
                    ds.Tables[PATTERNS].AcceptChanges();
                    dataSets.Add(ds);
                }
                var copyDataSet = destDataSet.Copy();
                int curDataSetIndex = 0;
                for (int j = 0; j < copyDataSet.Tables[CUTS].Rows.Count;)
                {

                    var row = copyDataSet.Tables[CUTS].Rows[j];
                    if (j + 1 < copyDataSet.Tables[CUTS].Rows.Count)
                    {
                        var nextRow = copyDataSet.Tables[CUTS].Rows[j + 1];
                        var qtyrpt = Convert.ToInt32(row[QTY_RPT]);
                        var partIndexsStr = Convert.ToString(row[PART_INDEX]).Trim();
                        var qtyparts = Convert.ToInt32(row[QTY_PARTS]);
                        var nextqtyrpt = Convert.ToInt32(nextRow[QTY_RPT]);
                        var nextpartIndexsStr = Convert.ToString(nextRow[PART_INDEX]).Trim();
                        var nextqtyparts = Convert.ToInt32(nextRow[QTY_PARTS]);
                        //if (qtyrpt != 0 && partIndexsStr != "0" && nextqtyrpt == 0 && nextpartIndexsStr != "0")
                        if (qtyparts != 0 && !partIndexsStr.Contains("X".ToUpper()))
                        {
                            var ds = dataSets[curDataSetIndex++];
                            ds.Tables[CUTS].Rows.Add(row.ItemArray);
                            var curBookCount = Convert.ToInt32(ds.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
                            for (int dataCount = 1; dataCount < curBookCount; dataCount++)
                            {
                                var tmpRow = copyDataSet.Tables[CUTS].Rows[j + 1];
                                var data = tmpRow.ItemArray;
                                ds.Tables[CUTS].Rows.Add(data);
                                tmpRow.Delete();
                                copyDataSet.Tables[CUTS].AcceptChanges();
                            }
                            if (j + 1 == copyDataSet.Tables[CUTS].Rows.Count) break;
                            ;
                            if (curDataSetIndex == dataSets.Count)
                            {
                                curDataSetIndex = 0;
                                j++;
                            }
                            else
                            {
                                nextRow = copyDataSet.Tables[CUTS].Rows[j + 1];
                                row[PART_INDEX] = nextRow[PART_INDEX];
                                row[QTY_RPT] = 1;
                                nextRow.Delete();
                                copyDataSet.Tables[CUTS].AcceptChanges();

                            }
                            continue;
                        }

                    }

                    dataSets.ForEach(item => item.Tables[CUTS].Rows.Add(row.ItemArray));
                    j++;
                }
                //dataSets.Add(copyDataSet);

                //更新余板数量
                dataSets.ForEach(item =>
                {
                    var tBookCount = Convert.ToInt32(item.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
                    Dictionary<int, int> dictionaryOffcut = new Dictionary<int, int>();
                    var cutsRows = item.Tables[CUTS].Rows;
                    for (int i = 0; i < cutsRows.Count; i++)
                    {
                        var row = cutsRows[i];
                        row[CUT_INDEX] = (short)(i + 1);
                        string offcutIndexStr = row[PART_INDEX].ToString().Trim().ToUpper();
                        if (offcutIndexStr.Contains("X"))
                        {
                            row[QTY_PARTS] = tBookCount;
                            int offcutIndex = Convert.ToInt32(offcutIndexStr.Replace("X", ""));
                            if (dictionaryOffcut.ContainsKey(offcutIndex))
                            {
                                dictionaryOffcut[offcutIndex] += tBookCount;
                            }
                            else
                            {
                                dictionaryOffcut.Add(offcutIndex, tBookCount);
                            }
                        }
                    }

                    var offcutsRows = item.Tables[OFFCUTS].Rows;
                    for (int i = 0; i < offcutsRows.Count; i++)
                    {
                        var row = offcutsRows[i];
                        var offIndex = Convert.ToInt32(row[OFC_INDEX].ToString());
                        row[OFC_QTY] = dictionaryOffcut[offIndex];
                    }
                    item.Tables[CUTS].AcceptChanges();
                    item.Tables[OFFCUTS].AcceptChanges();

                });
            }

            List<Tuple<int, DataSet>> tuples = new List<Tuple<int, DataSet>>();

            dataSets.ForEach(item => tuples.Add(new Tuple<int, DataSet>(ptnIndex, item)));
            return tuples;
        }

        #region old code

        ///// <summary>
        ///// 拆解单个锯切图
        ///// </summary>
        ///// <param name="destDataSet"></param>
        ///// <returns></returns>
        //private static List<Tuple<int, DataSet>> MdbFormatConverter(DataSet destDataSet)
        //{
        //    List<DataSet> dataSets = new List<DataSet>();
        //    int ptnIndex = Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][PTN_INDEX].ToString());
        //    var bookCount = Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
        //    var maxBook = Convert.ToInt32(destDataSet.Tables[PATTERNS].Rows[0][MAX_BOOK].ToString());
        //    //var maxBook = 4;
        //    if (bookCount <= maxBook)
        //    {
        //        dataSets.Add(destDataSet.Copy());
        //    }
        //    else
        //    {
        //        //var tmpDataSet = destDataSet.Copy();
        //        var cycle = bookCount / maxBook + (bookCount % maxBook > 0 ? 1 : 0);
        //        for (int i = 0; i < cycle; i++)
        //        {
        //            int unUsedBookCount = bookCount - i * maxBook;
        //            int curCycleBookCount = unUsedBookCount > maxBook ? maxBook : unUsedBookCount;
        //            var ds = destDataSet.Copy();
        //            ds.Tables[CUTS].Clear();
        //            ds.Tables[CUTS].AcceptChanges();
        //            ds.Tables[PATTERNS].Rows[0][QTY_RUN] = curCycleBookCount;
        //            ds.Tables[PATTERNS].Rows[0][QTY_CYCLES] = 1;
        //            ds.Tables[PATTERNS].AcceptChanges();
        //            dataSets.Add(ds);
        //        }
        //        var copyDataSet = destDataSet.Copy();
        //        int curDataSetIndex = 0;
        //        for (int j = 0; j < copyDataSet.Tables[CUTS].Rows.Count;)
        //        {

        //            var row = copyDataSet.Tables[CUTS].Rows[j];
        //            if (j + 1 < copyDataSet.Tables[CUTS].Rows.Count)
        //            {
        //                var nextRow = copyDataSet.Tables[CUTS].Rows[j + 1];
        //                var qtyrpt = Convert.ToInt32(row[QTY_RPT]);
        //                var partIndexsStr = Convert.ToString(row[PART_INDEX]).Trim();
        //                var nextqtyrpt = Convert.ToInt32(nextRow[QTY_RPT]);
        //                var nextpartIndexsStr = Convert.ToString(nextRow[PART_INDEX]).Trim();

        //                if (qtyrpt != 0 && partIndexsStr != "0" && nextqtyrpt == 0 && nextpartIndexsStr != "0")
        //                {
        //                    var ds = dataSets[curDataSetIndex++];
        //                    ds.Tables[CUTS].Rows.Add(row.ItemArray);
        //                    var curBookCount = Convert.ToInt32(ds.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
        //                    for (int dataCount = 1; dataCount < curBookCount; dataCount++)
        //                    {
        //                        var tmpRow = copyDataSet.Tables[CUTS].Rows[j + 1];
        //                        var data = tmpRow.ItemArray;
        //                        ds.Tables[CUTS].Rows.Add(data);
        //                        tmpRow.Delete();
        //                        copyDataSet.Tables[CUTS].AcceptChanges();
        //                    }
        //                    if (j + 1 == copyDataSet.Tables[CUTS].Rows.Count) break;
        //                    ;
        //                    if (curDataSetIndex == dataSets.Count)
        //                    {
        //                        curDataSetIndex = 0;
        //                        j++;
        //                    }
        //                    else
        //                    {
        //                        nextRow = copyDataSet.Tables[CUTS].Rows[j + 1];
        //                        row[PART_INDEX] = nextRow[PART_INDEX];
        //                        nextRow.Delete();
        //                        copyDataSet.Tables[CUTS].AcceptChanges();

        //                    }
        //                    continue;
        //                }

        //            }

        //            dataSets.ForEach(item => item.Tables[CUTS].Rows.Add(row.ItemArray));
        //            j++;
        //        }
        //        //dataSets.Add(copyDataSet);

        //        //更新余板数量
        //        dataSets.ForEach(item =>
        //        {
        //            var tBookCount = Convert.ToInt32(item.Tables[PATTERNS].Rows[0][QTY_RUN].ToString());
        //            Dictionary<int, int> dictionaryOffcut = new Dictionary<int, int>();
        //            var cutsRows = item.Tables[CUTS].Rows;
        //            for (int i = 0; i < cutsRows.Count; i++)
        //            {
        //                var row = cutsRows[i];
        //                row[CUT_INDEX] = (short)(i + 1);
        //                string offcutIndexStr = row[PART_INDEX].ToString().Trim().ToUpper();
        //                if (offcutIndexStr.Contains("X"))
        //                {
        //                    row[QTY_PARTS] = tBookCount;
        //                    int offcutIndex = Convert.ToInt32(offcutIndexStr.Replace("X", ""));
        //                    if (dictionaryOffcut.ContainsKey(offcutIndex))
        //                    {
        //                        dictionaryOffcut[offcutIndex] += tBookCount;
        //                    }
        //                    else
        //                    {
        //                        dictionaryOffcut.Add(offcutIndex, tBookCount);
        //                    }
        //                }
        //            }

        //            var offcutsRows = item.Tables[OFFCUTS].Rows;
        //            for (int i = 0; i < offcutsRows.Count; i++)
        //            {
        //                var row = offcutsRows[i];
        //                var offIndex = Convert.ToInt32(row[OFC_INDEX].ToString());
        //                row[OFC_QTY] = dictionaryOffcut[offIndex];
        //            }
        //            item.Tables[CUTS].AcceptChanges();
        //            item.Tables[OFFCUTS].AcceptChanges();

        //        });
        //    }

        //    List<Tuple<int, DataSet>> tuples = new List<Tuple<int, DataSet>>();

        //    dataSets.ForEach(item => tuples.Add(new Tuple<int, DataSet>(ptnIndex, item)));
        //    return tuples;
        //}


        #endregion




        /// <summary>
        /// 合并mdb
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        private static DataSet CombineMdbs(List<DataSet> dataSets)
        {
            if (dataSets == null || dataSets.Count == 0) throw new Exception("mdb数据不能为空");
            DataSet newDataSet = dataSets[0].Clone();
            newDataSet.Tables[HEADER].Rows.Add(dataSets[0].Tables[HEADER].Rows[0].ItemArray);
            foreach (var dataSet in dataSets)
            {
                var cutIndex = dataSets.FindIndex(item => item.Equals(dataSet));
                //合并 BOARDS 表 
                var boardlist = newDataSet.Tables[BOARDS].ConvertAll(dataRow => new Tuple<int, int, int>(Convert.ToInt32(dataRow[JOB_INDEX]),
                    Convert.ToInt32(dataRow[BRD_INDEX]), Convert.ToInt32(dataRow[MAT_INDEX])));
                var newBoardDatas = dataSet.Tables[BOARDS].FindAllRows(dataRow =>
                     !boardlist.Exists(tuple =>
                         Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1 &&
                         Convert.ToInt32(dataRow[BRD_INDEX]) == tuple.Item2 &&
                         Convert.ToInt32(dataRow[MAT_INDEX]) == tuple.Item3)
                , false);
                newBoardDatas.ForEach(data => newDataSet.Tables[BOARDS].Rows.Add(data));
                newDataSet.Tables[BOARDS].AcceptChanges();

                //合并 CUTS 表 
                var cutsList = newDataSet.Tables[CUTS].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[PTN_INDEX])));
                var newCutsDatas = dataSet.Tables[CUTS].FindAllRows(dataRow =>
                        !cutsList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1 &&
                            Convert.ToInt32(dataRow[PTN_INDEX]) == tuple.Item2)
                    , false);
                newCutsDatas.ForEach(data => newDataSet.Tables[CUTS].Rows.Add(data));

                //合并 JOBS 表 
                var jobsList = newDataSet.Tables[JOBS].ConvertAll(dataRow =>
                    new Tuple<int>(Convert.ToInt32(dataRow[JOB_INDEX])));
                var newJobDatas = dataSet.Tables[JOBS].FindAllRows(dataRow =>
                        !jobsList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1)
                    , false);
                newJobDatas.ForEach(data => newDataSet.Tables[JOBS].Rows.Add(data));

                //合并 MATERIALS 表 
                var materialList = newDataSet.Tables[MATERIALS].ConvertAll(dataRow =>
                    new Tuple<int, int, string>(Convert.ToInt32(dataRow[JOB_INDEX])
                        , Convert.ToInt32(dataRow[MAT_INDEX])
                        , Convert.ToString(dataRow[CODE])));

                var newMaterialDatas = dataSet.Tables[MATERIALS].FindAllRows(dataRow =>
                        !materialList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[MAT_INDEX]) == tuple.Item2
                            /*&& Convert.ToString(dataRow[CODE]) == tuple.Item3*/)
                    , false);
                newMaterialDatas.ForEach(data => newDataSet.Tables[MATERIALS].Rows.Add(data));

                //合并 NOTES 表 
                var noteList = newDataSet.Tables[NOTES].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[NOTE_INDEX])));
                var newNoteDatas = dataSet.Tables[NOTES].FindAllRows(dataRow =>
                        !noteList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[NOTE_INDEX]) == tuple.Item2)
                    , false);
                newNoteDatas.ForEach(data => newDataSet.Tables[NOTES].Rows.Add(data));

                //合并 OFFCUTS 表 
                var offcutsList = newDataSet.Tables[OFFCUTS].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[OFC_INDEX])));
                //获取旧的余板
                var oldOffcutsDatas = dataSet.Tables[OFFCUTS].FindAllRows(dataRow =>
                        offcutsList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[OFC_INDEX]) == tuple.Item2)
                    , false);
                //获取新的余板
                var newOffcutsDatas = dataSet.Tables[OFFCUTS].FindAllRows(dataRow =>
                        !offcutsList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[OFC_INDEX]) == tuple.Item2)
                    , false);
                var ofcIndexColumbIndex = newDataSet.Tables[OFFCUTS].Columns.IndexOf(OFC_INDEX);
                var ofcqtyIndexColumnIndex = newDataSet.Tables[OFFCUTS].Columns.IndexOf(OFC_QTY);
                newOffcutsDatas.ForEach(data =>
                {
                    newDataSet.Tables[OFFCUTS].Rows.Add(data);
                });
                oldOffcutsDatas.ForEach(data =>
                {
                    foreach (DataRow row in newDataSet.Tables[OFFCUTS].Rows)
                    {
                        var index = Convert.ToInt32(row[OFC_INDEX].ToString());
                        if (index == Convert.ToInt32(data[ofcIndexColumbIndex].ToString()))
                        {
                            row[OFC_QTY] = Convert.ToInt32(row[OFC_QTY].ToString()) +
                                           Convert.ToInt32(data[ofcqtyIndexColumnIndex]);
                            return;
                        }
                    }
                });

                //合并 PARTS_DST 表 
                var partDstList = newDataSet.Tables[PARTS_DST].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[PART_INDEX])));
                var newPartDstDatas = dataSet.Tables[PARTS_DST].FindAllRows(dataRow =>
                        !partDstList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[PART_INDEX]) == tuple.Item2)
                    , false);
                newPartDstDatas.ForEach(data => newDataSet.Tables[PARTS_DST].Rows.Add(data));

                //合并 PARTS_REQ 表 
                var partReqList = newDataSet.Tables[PARTS_REQ].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[PART_INDEX])));
                var newPartReqDatas = dataSet.Tables[PARTS_REQ].FindAllRows(dataRow =>
                        !partReqList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[PART_INDEX]) == tuple.Item2)
                    , false);
                newPartReqDatas.ForEach(data => newDataSet.Tables[PARTS_REQ].Rows.Add(data));

                //合并 PARTS_UDI 表 
                var partUdiList = newDataSet.Tables[PARTS_UDI].ConvertAll(dataRow =>
                    new Tuple<int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[PART_INDEX])));
                var newPartUdiDatas = dataSet.Tables[PARTS_UDI].FindAllRows(dataRow =>
                        !partUdiList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[PART_INDEX]) == tuple.Item2)
                    , false);
                newPartUdiDatas.ForEach(data => newDataSet.Tables[PARTS_UDI].Rows.Add(data));

                //合并 PARTS_UDI 表 
                var patternList = newDataSet.Tables[PATTERNS].ConvertAll(dataRow =>
                    new Tuple<int, int, int>(Convert.ToInt32(dataRow[JOB_INDEX]), Convert.ToInt32(dataRow[PTN_INDEX]), Convert.ToInt32(dataRow[BRD_INDEX])));
                var newPatternDatas = dataSet.Tables[PATTERNS].FindAllRows(dataRow =>
                        !patternList.Exists(tuple =>
                            Convert.ToInt32(dataRow[JOB_INDEX]) == tuple.Item1
                            && Convert.ToInt32(dataRow[PTN_INDEX]) == tuple.Item2
                            && Convert.ToInt32(dataRow[BRD_INDEX]) == tuple.Item3)
                    , false);
                newPatternDatas.ForEach(data => newDataSet.Tables[PATTERNS].Rows.Add(data));

            }

            return newDataSet;
        }

        /// <summary>
        /// 更新标题
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool UpdatedTitle(DataSet data,string title)
        {
            data.Tables[HEADER].Rows[0][TITLE] = title;
            return true;
        }

        /// <summary>
        /// 保存mdb数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public static bool UpdateDatas(DataSet data, out string newFileName)
        {
            newFileName = "";
            var s = Path.GetFullPath(TemplateMdbName);
            if (!File.Exists(s))
            {
                return false;
            }

            if (!CreatedMdbFile(out newFileName))
            {
                return false;
            }

            AccessDB accessDb = new AccessDB(newFileName);
            var mdbData = accessDb.GetDatas();
            DataSetCopy(data, mdbData);
            accessDb.Update(mdbData);

            return true;
        }

        /// <summary>
        /// 创建空MDB文件
        /// </summary>
        /// <param name="newFileName">MDB文件</param>
        /// <returns></returns>
        private static bool CreatedMdbFile(out string newFileName)
        {
            newFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".mdb");
            if (File.Exists(newFileName)) File.Delete(newFileName);
            File.Copy(TemplateMdbName, newFileName);
            return true;
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="sourceDatas"></param>
        /// <param name="destDatas"></param>
        private static void DataSetCopy(DataSet sourceDatas, DataSet destDatas)
        {
            foreach (DataTable sourceTable in sourceDatas.Tables)
            {
                string tableName = sourceTable.TableName;
                var destTable = destDatas.Tables[tableName];
                foreach (DataRow row in sourceTable.Rows)
                {
                    destTable.Rows.Add(row.ItemArray);
                }
            }
        }

    }

    internal class PatternAllInfo
    {
        /// <summary>
        /// 锯切图信息
        /// </summary>
        public PatternInfo PatternInfo { get; set; }
        /// <summary>
        /// 工件信息
        /// </summary>
        public List<WorkpieceInfo> WorkpieceInfos { get; set; }
    }

   
}
