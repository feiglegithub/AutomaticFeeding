// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：RadGridViewHelper.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-05-17 8:18
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using CellFormattingEventArgs = Telerik.WinControls.UI.CellFormattingEventArgs;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    /// <summary>
    ///     GridView帮助类
    /// </summary>
    public static class GridViewHelper
    {
        /// <summary>
        ///     行号列名称
        /// </summary>
        public static readonly string RowNumberNameStr = "gvdgcRowNumber";

        private static RadLabelElement _lblCount;
        private static RadGridView _radGridView;

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
                    dr[countsub] = Convert.ToString(gv.Rows[count].Cells[countsub].Value);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static void EditValueChanageColor(this RadGridView gv)
        {
            EditValueChanageColor(gv, Color.Red);
        }

        public static void EditValueChanageColor(this RadGridView gv, Color color)
        {
            gv.RowsChanged += (sender, e) =>
            {
                if (e.NewStartingIndex < 0) return;
                foreach (GridViewCellInfo gridViewCellInfo in gv.Rows[e.NewStartingIndex].Cells)
                    gridViewCellInfo.Style.ForeColor = color;
            };
        }

        public static void DataBind<T>(this RadGridView gv, T obj)
        {
            gv.Invoke((Action)(() => { gv.DataSource = obj; }));
        }

        public static void ShowRowNumber(this RadGridView gv, RadLabelElement lblCount = null, string headerText = "")
        {
            gv.ShowRowHeaderColumn = false;
            var TextBoxColumn = new GridViewTextBoxColumn(){Name="rgvccc"};
            TextBoxColumn.AllowFiltering = false;
            TextBoxColumn.AllowHide = false;
            TextBoxColumn.AllowSearching = false;
            TextBoxColumn.AllowSort = false;
            TextBoxColumn.HeaderText = headerText;
            TextBoxColumn.AutoSizeMode = BestFitColumnMode.AllCells;
            TextBoxColumn.Name = RowNumberNameStr;
            TextBoxColumn.TextAlignment = ContentAlignment.MiddleCenter;
            TextBoxColumn.Width = 30;
            if (!gv.Columns.Contains(TextBoxColumn.Name))
            {
                gv.Columns.Insert(0, TextBoxColumn);
                gv.CellFormatting += Gv_CellFormatting;
            }
            _lblCount = lblCount;
            _radGridView = gv;
        }

        /// <summary>
        /// 删除GridView记录行
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="row">保留的数据行数</param>
        /// <param name="diection">false:从前面的行开始清除，true:从后面的行开始清除</param>
        public static void DeleteRecordRow(this RadGridView gv, int row = 0, bool diection = false)
        {
            gv.InvokeExecute(() =>
            {
                if (gv.Rows.Count > row)
                {
                    while (gv.Rows.Count > row)
                    {
                        var r = row;
                        if (!diection)
                        {
                            r = 0;
                        }
                        gv.Rows.RemoveAt(r);

                    }
                }
            });

        }

        public static void ExportExcel(this RadGridView gv, string fileName = "fileName")
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMdd")}";
            dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var renderer = new SpreadExportRenderer();
            var export = new GridViewSpreadExport(gv) { ExportFormat = SpreadExportFormat.Xlsx };
            renderer.WorkbookCreated += renderer_WorkbookCreated;
            export.RunExport(dialog.FileName, renderer);
        }

        private static void renderer_WorkbookCreated(object sender, WorkbookCreatedEventArgs e)
        {
            var worksheet = e.Workbook.ActiveWorksheet;
            worksheet.Columns[worksheet.UsedCellRange].AutoFitWidth();
        }

        private static void Gv_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name == RowNumberNameStr)
                e.CellElement.Text = (e.CellElement.RowIndex + 1).ToString();
            var count = e.CellElement.ViewInfo.Rows.Count;
            if (_lblCount != null) _lblCount.Text = $"计数：{count}";
        }
    }
}