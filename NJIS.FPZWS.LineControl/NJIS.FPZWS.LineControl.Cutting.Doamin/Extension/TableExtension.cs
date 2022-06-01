using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Extension
{
    internal static class TableExtension
    {
        /// <summary>
        /// 获取指定的行数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowMatch"></param>
        /// <param name="deleteSource">是否删除源数据</param>
        /// <returns></returns>
        public static object[]  FindRowFirstOrDefault(this DataTable table,Expression<Func<DataRow,bool>> rowMatch,bool deleteSource)
        {
            for (int i = 0; i < table.Rows.Count; )
            {
                DataRow row = table.Rows[i];
                if (rowMatch.Compile()(row))
                {
                    var data = row.ItemArray;
                    if (deleteSource)
                    {
                        row.Delete();
                        table.AcceptChanges();
                    }
                    return data;
                }

                i++;
            }
            return null;
        }

        /// <summary>
        /// 查找指定的行集合
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowMatch"></param>
        /// <param name="deleteSource">是否删除源数据</param>
        /// <returns></returns>
        public static List<object[]> FindAllRows(this DataTable table, Expression<Func<DataRow, bool>> rowMatch, bool deleteSource)
        {
            List<object[]> list = new List<object[]>();
            for (int i = 0; i < table.Rows.Count; )
            {
                DataRow row = table.Rows[i];
                if (rowMatch.Compile()(row))
                {
                    list.Add(row.ItemArray);
                    if (deleteSource)
                    {
                        row.Delete();
                        table.AcceptChanges();
                        continue;
                    }  
                }
                i++;
            }
            return list;
        }

        public static List<T> ConvertAll<T>(this DataTable table, Converter<DataRow,T> rowConverter)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(rowConverter(row));
            }

            return list;
        }
    }
}
