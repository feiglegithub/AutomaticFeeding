using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.Platform.Edgebanding.Tool
{
    public static class GridViewHelper
    {
        /// <summary>
        ///     RadGridView 转DataTable
        /// </summary>
        /// <param name="gv"></param>
        /// <returns></returns>
        public static DataTable GridView2DataTable(RadGridView gv)
        {
            var dt = new DataTable();
            for (var count = 0; count < gv.Columns.Count; count++)
            {
                var dc = new DataColumn(gv.Columns[count].HeaderText);
                dt.Columns.Add(dc);
            }
            // 循环行
            for (var count = 0; count < gv.Rows.Count; count++)
            {
                var dr = dt.NewRow();
                for (var countsub = 0; countsub < gv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(gv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static void DataBind_bk<T>(this RadGridView gv, T obj)
        {
            gv.Invoke((Action)(() => { gv.DataSource = obj; }));
        }

        public static void ShowRowNumberOfNew(this RadGridView gv)
        {
            var gridViewTextBoxColumn1 = new GridViewTextBoxColumn();

            gridViewTextBoxColumn1.AllowFiltering = false;
            gridViewTextBoxColumn1.AllowHide = false;
            gridViewTextBoxColumn1.AllowSearching = false;
            gridViewTextBoxColumn1.AllowSort = false;
            gridViewTextBoxColumn1.HeaderText = "#";
            gridViewTextBoxColumn1.AutoSizeMode = BestFitColumnMode.AllCells;
            gridViewTextBoxColumn1.Name = "gvdgcRowNumber";

            gv.Columns.Insert(0, gridViewTextBoxColumn1);
            gv.Rows.CollectionChanged += Rows_CollectionChanged;
        }

        private static void Rows_CollectionChanged(object sender, Telerik.WinControls.Data.NotifyCollectionChangedEventArgs e)
        {
            var rgbMain =
            ((MasterGridViewTemplate)((GridViewRowCollection)sender)
                .Owner).Owner;

            if (e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Reset || e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Add ||
                e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Remove)
            {
                for (var i = 0; i < rgbMain.Rows.Count; i++)
                {
                    rgbMain.Rows[i].Cells[0].Value = i + 1;
                }
            }
        }
    }
}
