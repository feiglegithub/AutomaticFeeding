//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Platform.DataDistribute
//   项目名称：NJIS.FPZWS.Platform.DataDistribute.UI
//   文 件 名：ViewBase.cs
//   创建时间：2018-10-23 8:50
//   作    者：
//   说    明：
//   修改时间：2018-10-23 8:50
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace NJIS.FPZWS.UI.Common.Controls
{
    public partial class GridViewBase : UserControl
    {
        /// <summary>
        /// 行号的列名
        /// </summary>
        private const string RowCountColumnName = ".RowCountColumnName";
        /// <summary>
        /// 勾选行的列名
        /// </summary>
        private const string CheckColumnName = ".CheckColumnName";
        private readonly List<GridViewRowInfo> _currNewAddRows = new List<GridViewRowInfo>();
        private readonly List<GridViewRowInfo> _currDeleteRows = new List<GridViewRowInfo>();
        private readonly List<object> _currDeleteRowsData = new List<object>();
        private readonly List<GridViewRowInfo> _currEidtedRows = new List<GridViewRowInfo>();

        private ChangeRowCollection _changeRows = null;
        private readonly GridViewCheckBoxColumn _checkColumn = new GridViewCheckBoxColumn() { HeaderText = "", Name = CheckColumnName, MaxWidth = 30, AllowSort = false, ThreeState = false, ReadOnly = false };
        private Expression<Func<GridViewRowInfo, bool>> DefaultSelectRowsMatchExpression { get; set; }

        //private Expression<Func<GridViewRowInfo, Color>> RowsDefaultForeColor { get; set; }

        private readonly List<Tuple<CellInfo, object>> _editCells = new List<Tuple<CellInfo, object>>();
        private readonly GridViewTextBoxColumn _rowCountColumn = new GridViewTextBoxColumn()
        {
            AllowFiltering = false,
            AllowHide = false,
            AllowSearching = false,
            AllowSort = false,
            HeaderText = "",
            AutoSizeMode = BestFitColumnMode.AllCells,
            Name = RowCountColumnName,
            TextAlignment = ContentAlignment.MiddleCenter,
            Width = 30,
            MaxWidth = 30,
            ReadOnly = true
        };
        private readonly WaitControl _waitControl = new WaitControl();

        /// <summary>
        /// 是否允许勾选框可以排序
        /// </summary>
        public bool AllowCheckSort
        {
            get
            {
                return _checkColumn.AllowSort;
            }

            set
            {
                _checkColumn.AllowSort = value;
            }
        }

        private bool _AllowSelectAll = true;
        /// <summary>
        /// 是否允许全选
        /// </summary>
        public bool AllowSelectAll
        {
            get
            {
                return _AllowSelectAll;
            }

            set
            {
                if (value)
                {
                    if (!_AllowSelectAll)
                    {
                        radGridView1.CellClick += SelectAll_CellClick;
                    }
                }
                else
                {
                    if (_AllowSelectAll)
                    {
                        radGridView1.CellClick -= SelectAll_CellClick;
                    }
                }

                _AllowSelectAll = value;
            }

        }

        public ChangeRowCollection ChangeRows => _changeRows ??
                                                 (_changeRows = new ChangeRowCollection(_currNewAddRows, _currDeleteRows, _currDeleteRowsData, _currEidtedRows));
        /// <summary>
        /// 单元格值变化后字体的颜色
        /// </summary>
        public Color ChangedValueForeColor { get; set; } = Color.DeepPink;
        /// <summary>
        /// 单元格值默认字体颜色
        /// </summary>
        public Color DefaultValueForeColor { get; set; } = Color.Black;

        /// <summary>
        /// 新增行的字体颜色
        /// </summary>
        public Color NewAddRowForeColor { get; set; } = Color.Fuchsia;
        protected internal class CellInfo
        {
            public CellInfo(GridViewRowInfo row, GridViewColumn column)
            {
                Column = column;
                Row = row;
            }
            public GridViewColumn Column { get; }
            public GridViewRowInfo Row { get; }

            public bool CompareTo(CellInfo other)
            {
                if (other != null)
                {
                    return other.Column.Equals(Column) && other.Row.Equals(Row);
                }

                return false;
            }
        }

        public GridViewBase()
        {
            InitializeComponent();
            radGridView1.SelectionChanged += RadGridView1_SelectionChanged;
            radGridView1.RowsChanged += RadGridView1_RowsChanged;
            radGridView1.RowsChanging += RadGridView1_RowsChanging;
            radGridView1.CellBeginEdit += RadGridView1_CellBeginEdit;
            radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
            radGridView1.PastingCellClipboardContent += RadGridView1_PastingCellClipboardContent;
            radGridView1.Pasting += RadGridView1_Pasting;
            if (AllowSelectAll)
            {
                radGridView1.CellClick += SelectAll_CellClick;
            }

            ShowRowHeaderColumn = false;
            ShowCheckBox = false;
            ShowRowNumber = true;

        }

        private void SelectAll_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex == -1)
            {
                radGridView.SelectAll();
                if (ShowCheckBox)
                {
                    foreach (var row in Rows)
                    {
                        row.Cells[CheckColumnName].Value = true;
                    }
                }
            }
        }

        public List<T> GetSelectItemsData<T>()
        {
            List<T> datas = new List<T>();
            foreach (var row in SelectedRows)
            {
                if (row.DataBoundItem is T)
                {
                    datas.Add((T)row.DataBoundItem);
                }
            }

            return datas;
        }

        private int pasteCount = 0;
        private void RadGridView1_Pasting(object sender, GridViewClipboardEventArgs e)
        {
            if (pasteCount == 1)
            {
                pasteCount = 0;
                return;
            }
            var s = e.DataObject.GetText();
            var ret = s.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int rowIndex = radGridView1.CurrentRow.Index;
            radGridView1.BeginUpdate();
            if (rowIndex == -1)
            {
                for (int i = 0; i < ret.Length; i++)
                {
                    radGridView1.Rows.AddNew();
                }
            }
            else
            {
                var rowCount = rowIndex + ret.Length;
                if (rowCount > radGridView1.Rows.Count)
                {
                    int newCount = rowCount - radGridView1.Rows.Count;
                    for (int i = 0; i < newCount; i++)
                    {
                        radGridView1.Rows.AddNew();
                    }
                }
            }

            radGridView1.EndUpdate();
            radGridView1.TableElement.RowScroller.ScrollToItem(radGridView1.CurrentRow);
            pasteCount = 1;
        }

        private bool ContainsCellInfo(GridViewCellInfo cell)
        {
            if (cell == null) return false;
            var cellInfo = new CellInfo(cell.RowInfo, cell.ColumnInfo);
            return _editCells.Exists(item => item.Item1.CompareTo(cellInfo));
        }

        private Tuple<CellInfo, object> GetCellInfos(GridViewCellInfo cell)
        {
            if (ContainsCellInfo(cell))
            {
                var cellInfo = new CellInfo(cell.RowInfo, cell.ColumnInfo);
                return _editCells.Find(item => item.Item1.CompareTo(cellInfo));
            }

            return null;
        }

        private void RemoveCellInfo(GridViewCellInfo cell)
        {
            if (cell == null) return;
            var ci = new CellInfo(cell.RowInfo, cell.ColumnInfo);
            if (ContainsCellInfo(cell))
            {
                _editCells.RemoveAll(tuple => tuple.Item1.CompareTo(ci));
            }
            //检查删除的单元格所在的行是否还有发生变更的数据
            var changeList = _editCells.FindAll(tuple => tuple.Item1.Row == cell.RowInfo);
            if (changeList.Count == 0)
            {
                //RemoveChangeRow(cell.RowInfo.Index);
                RemoveChangeRow(cell.RowInfo);
            }
        }
        private void AddCellInfo(GridViewDataRowInfo row, GridViewCollectionChangingEventArgs e)
        {
            var cell = row.Cells[e.PropertyName];
            if (cell == null) return;
            var ci = new CellInfo(cell.RowInfo, cell.ColumnInfo);
            if (!ContainsCellInfo(cell))
            {
                object obj = e.OldValue == null ? null : Convert.ChangeType(cell.Value, cell.Value.GetType());
                _editCells.Add(new Tuple<CellInfo, object>(ci, obj));
            }
        }

        private void AddCellInfo(GridViewCellInfo cell)
        {
            if (cell == null) return;
            var ci = new CellInfo(cell.RowInfo, cell.ColumnInfo);
            if (!ContainsCellInfo(cell))
            {
                object obj = cell.Value == null ? null : Convert.ChangeType(cell.Value, cell.Value.GetType());
                _editCells.Add(new Tuple<CellInfo, object>(ci, obj));
            }
        }

        private void RadGridView1_PastingCellClipboardContent(object sender, GridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var cell = e.RowInfo.Cells[e.ColumnIndex];
            if (ShowCheckBox)
            {
                if (e.Column.Name == CheckColumnName)
                    return;
            }

            if (ShowRowNumber)
            {
                if (e.Column.Name == RowCountColumnName)
                    return;
            }
            AddCellInfo(cell);
            if (!_currNewAddRows.Contains(radGridView1.Rows[cell.RowInfo.Index]))
            {
                cell.Style.ForeColor = CheckCellValueChanged(cell, e.Value) ? ChangedValueForeColor : DefaultValueForeColor;
            }
        }

        private bool CheckCellValueChanged(GridViewCellInfo cell, object cellNewValue)
        {
            if (ContainsCellInfo(cell))
            {
                var tuple = GetCellInfos(cell);
                object obj = tuple.Item2;
                if (obj == null)
                {
                    if (cellNewValue != null)
                    {
                        //AddChangeRow(cell.RowInfo.Index);
                        return true;
                    }
                }
                else
                {
                    if (!obj.Equals(cellNewValue))
                    {
                        //AddChangeRow(cell.RowInfo.Index);
                        return true;
                    }
                }

                //RemoveCellInfo(cell);
            }
            return false;
        }

        public bool ShowRowHeaderColumn
        {
            get { return radGridView1.ShowRowHeaderColumn; }
            set { radGridView1.ShowRowHeaderColumn = value; }
        }

        /// <summary>
        /// 是否显示行号
        /// </summary>
        public bool ShowRowNumber
        {
            get { return radGridView1.Columns.Contains(_rowCountColumn); }
            set
            {
                if (value)
                {
                    if (ShowRowNumber)
                    {
                        return;
                    }
                    radGridView1.Columns.Insert(ShowCheckBox ? 1 : 0, _rowCountColumn);
                    radGridView1.CellFormatting += Gv_CellFormatting;
                }
                else
                {
                    if (ShowRowNumber)
                    {
                        radGridView1.Columns.Remove(_rowCountColumn);
                        radGridView1.CellFormatting -= Gv_CellFormatting;
                    }
                }
            }
        }

        private void Gv_CellFormatting(object sender, CellFormattingEventArgs e)
        {

            if (e.Column.Equals(_rowCountColumn))
                e.CellElement.Text = (e.CellElement.RowIndex + 1).ToString();
            var count = e.CellElement.ViewInfo.Rows.Count;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var cell = e.Row.Cells[e.ColumnIndex];
            if (ShowCheckBox)
            {
                if (cell.ColumnInfo.Index == 0)
                {
                    e.Row.IsSelected = Convert.ToBoolean(cell.Value);

                    return;
                }
            }

            var isChanged = CheckCellValueChanged(cell, e.Value);
            if (isChanged)
            {
                //AddChangeRow(cell.RowInfo.Index);
                AddChangeRow(cell.RowInfo);
            }
            else
            {
                RemoveCellInfo(cell);
            }
            if (!_currNewAddRows.Contains(radGridView1.Rows[cell.RowInfo.Index]))
            {
                cell.Style.ForeColor = CheckCellValueChanged(cell, e.Value) ? ChangedValueForeColor : DefaultValueForeColor;
            }

        }

        /// <summary>
        /// 是否显示勾选项
        /// </summary>
        public bool ShowCheckBox
        {
            get { return radGridView.Columns.Contains(_checkColumn); }
            set
            {
                if (value)
                {
                    radGridView.Columns.Insert(0, _checkColumn);
                }
                else
                {
                    if (radGridView.Columns.Contains(_checkColumn))
                    {
                        radGridView.Columns.Remove(_checkColumn);
                    }
                }
            }
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var cell = e.Row.Cells[e.ColumnIndex];
            if (ShowCheckBox)
            {
                if (cell.ColumnInfo.Index == 0)
                {

                    return;
                }
            }
            AddCellInfo(cell);

        }


        private void AddNewRow(int newRowIndex)
        {
            GridViewRowInfo newRowInfo = radGridView1.Rows[newRowIndex];
            if (!_currNewAddRows.Exists(row => row.Equals(newRowInfo)))
            {
                foreach (GridViewCellInfo cell in newRowInfo.Cells)
                {
                    if (cell.ColumnInfo.Equals(_checkColumn) || cell.ColumnInfo.Equals(_rowCountColumn))
                        continue;
                    cell.Style.ForeColor = NewAddRowForeColor;

                }
                _currNewAddRows.Add(newRowInfo);
            }
        }

        private void AddDeleteRow(int deleteRowIndex)
        {
            GridViewRowInfo deleteRowInfo = radGridView1.Rows[deleteRowIndex];
            if (_currNewAddRows.Exists(row => row.Equals(deleteRowInfo)))
            {
                _currNewAddRows.Remove(deleteRowInfo);
                return;
            }
            if (!_currDeleteRows.Exists(row => row.Equals(deleteRowInfo)))
            {
                _currDeleteRows.Add(deleteRowInfo);
                _currDeleteRowsData.Add(deleteRowInfo.DataBoundItem);
            }
        }

        /// <summary>
        /// 弃用，用户筛选数据以后，行定位错误
        /// </summary>
        /// <param name="changedRowIndex"></param>
        private void RemoveChangeRow(int changedRowIndex)
        {
            GridViewRowInfo editRowInfo = radGridView1.Rows[changedRowIndex];
            if (_currEidtedRows.Exists(row => row.Equals(editRowInfo)))
            {
                _currEidtedRows.Remove(editRowInfo);
            }
        }

        /// <summary>
        /// 移除改变的行
        /// </summary>
        /// <param name="gridViewRowInfo"></param>
        private void RemoveChangeRow(GridViewRowInfo gridViewRowInfo)
        {
            //GridViewRowInfo editRowInfo = radGridView1.Rows[changedRowIndex];
            if (_currEidtedRows.Exists(row => row.Equals(gridViewRowInfo)))
            {
                _currEidtedRows.Remove(gridViewRowInfo);
            }
        }
        
        /// <summary>
        /// 弃用，用户筛选数据以后，行定位错误
        /// </summary>
        /// <param name="changedRowIndex"></param>
        private void AddChangeRow(int changedRowIndex)
        {
            GridViewRowInfo editRowInfo = radGridView1.Rows[changedRowIndex];
            if (_currNewAddRows.Exists(row => row.Equals(editRowInfo)))
            {
                return;
            }
            if (!_currEidtedRows.Exists(row => row.Equals(editRowInfo)))
            {
                _currEidtedRows.Add(editRowInfo);
            }
        }

        /// <summary>
        /// 新增改变的行
        /// </summary>
        /// <param name="gridViewRowInfo"></param>
        private void AddChangeRow(GridViewRowInfo gridViewRowInfo)
        {
            //GridViewRowInfo editRowInfo = radGridView1.Rows[changedRowIndex];
            if (_currNewAddRows.Exists(row => row.Equals(gridViewRowInfo)))
            {
                return;
            }
            if (!_currEidtedRows.Exists(row => row.Equals(gridViewRowInfo)))
            {
                _currEidtedRows.Add(gridViewRowInfo);
            }
        }

        public GridViewRowCollection Rows => radGridView1.Rows;
        public List<GridViewRowInfo> SelectedRows
        {
            get
            {
                List<GridViewRowInfo> lst = new List<GridViewRowInfo>();
                if (ShowCheckBox)
                {
                    foreach (GridViewRowInfo row in Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            lst.Add(row);
                        }
                    }
                }
                else
                {
                    lst = lst.Concat(radGridView1.SelectedRows.ToList()).ToList();
                }
                return lst;
            }
        }


        public int PageSize
        {
            get { return radGridView1.PageSize; }
            set { radGridView1.PageSize = value; }
        }
        public bool EnablePaging
        {
            get { return radGridView1.EnablePaging; }
            set { radGridView1.EnablePaging = value; }
        }
        public bool ReadOnly
        {
            get { return radGridView1.ReadOnly; }
            set { radGridView1.ReadOnly = value; }
        }
        public bool AllowDeleteRow { get { return radGridView1.AllowDeleteRow; } set { radGridView1.AllowDeleteRow = value; } }
        public bool AllowAddNewRow
        {
            get { return radGridView1.AllowAddNewRow; }
            set { radGridView1.AllowAddNewRow = value; }
        }
        public bool AllowEditRow
        {
            get { return radGridView1.AllowEditRow; }
            set { radGridView1.AllowEditRow = value; }
        }

        private void RadGridView1_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddNewRow(e.NewStartingIndex);
            }


        }

        private void RadGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                AddDeleteRow(e.OldStartingIndex);
            }

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var row = sender as GridViewDataRowInfo;
                var cell = row.Cells[e.PropertyName];
                if (cell == null) return;
                if (cell.RowInfo.Index == -1) return;
                AddCellInfo(row, e);
                var isChanged = CheckCellValueChanged(cell, e.NewValue);
                if (isChanged)
                {
                    //AddChangeRow(cell.RowInfo.Index);
                    AddChangeRow(cell.RowInfo);
                }
                else
                {
                    RemoveCellInfo(cell);
                }
                if (!_currNewAddRows.Contains(radGridView1.Rows[cell.RowInfo.Index]))
                {
                    cell.Style.ForeColor = isChanged ? ChangedValueForeColor : DefaultValueForeColor;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
        }

        /// <summary>
        /// 提交当前所有行的更改
        /// </summary>
        public void AcceptChanges()
        {
            foreach (var row in _currNewAddRows)
            {
                foreach (GridViewCellInfo cell in row.Cells)
                {
                    cell.Style.ForeColor = DefaultValueForeColor;
                }
            }
            foreach (var row in _currEidtedRows)
            {
                foreach (GridViewCellInfo cell in row.Cells)
                {
                    cell.Style.ForeColor = DefaultValueForeColor;
                }
            }
            _currNewAddRows.Clear();
            _currDeleteRows.Clear();
            _currDeleteRowsData.Clear();
            _currEidtedRows.Clear();

        }



        private void RadGridView1_SelectionChanged(object sender, System.EventArgs e)
        {

            if (ShowCheckBox)
            {

                var cell = radGridView1.CurrentCell;
                if (cell == null)
                {
                    //foreach (var row in Rows)
                    //{
                    //    row.Cells[0].Value = false;
                    //    row.IsSelected = false;
                    //}
                }
                else
                {
                    if (cell.ColumnIndex == 0) return;
                    foreach (var row in Rows)
                    {
                        row.Cells[0].Value = false;
                    }
                    foreach (var row in radGridView1.SelectedRows)
                    {
                        row.Cells[0].Value = true;
                    }
                }
            }
            lbSelectedRowsCount.Text = this.SelectedRows.Count.ToString();
        }


        //public void SetDefaultForeColor(Expression<Func<GridViewRowInfo, Color>> colorMatchExpression)
        //{
        //    RowsDefaultForeColor = colorMatchExpression;
        //}

        /// <summary>
        /// 设置默认选中行
        /// </summary>
        /// <param name="selectMatchExpression"></param>
        public void SetDefaultSelectRows(Expression<Func<GridViewRowInfo, bool>> selectMatchExpression)
        {
            DefaultSelectRowsMatchExpression = selectMatchExpression;
        }

        public RadGridView radGridView => radGridView1;


        public object DataSource
        {
            get { return radGridView1.DataSource; }
            set
            {
                radGridView1.DataSource = value;

                _currNewAddRows.Clear();
                _currDeleteRows.Clear();
                _currDeleteRowsData.Clear();
                _currEidtedRows.Clear();
                lbCount.Text = radGridView1.Rows.Count.ToString();
                if (DefaultSelectRowsMatchExpression != null)
                {
                    foreach (var row in Rows)
                    {
                        row.IsSelected = DefaultSelectRowsMatchExpression.Compile()(row);
                        if (ShowCheckBox)
                        {
                            row.Cells[0].Value = row.IsSelected;
                        }
                    }
                }
                this.EndWait();
            }
        }

        private void GView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            var column = e.CellElement.ColumnInfo;
            if (column.Name == GridViewHelper.RowNumberNameStr)
            {
                lbCount.Text = (e.RowIndex + 1).ToString();
            }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="gridViewColumns"></param>
        public void AddColumns(params GridViewDataColumn[] gridViewColumns)
        {
            int count = gridViewColumns.Length;
            if (count == 0)
            {
                return;
            }
            radGridView1.MasterTemplate.Columns.AddRange(gridViewColumns);

        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="columnsInfos"></param>
        public void AddColumns(List<ColumnInfo> columnsInfos)
        {
            var count = columnsInfos.Count;
            if (count < 1)
            {
                return;
            }

            var width = radGridView1.Width / count;
            var gridViewDataColumns = new GridViewDataColumn[count];
            for (var i = 0; i < count; i++)
            {
                var column = CreatedGridViewDataColumn(columnsInfos[i]);
                column.Width = width;
                column.Width += columnsInfos[i].Width;
                if (column.Width < 1)
                {
                    column.Width = 2;
                }
                gridViewDataColumns[i] = column;
            }
            radGridView1.MasterTemplate.Columns.AddRange(gridViewDataColumns);
        }

        private GridViewDataColumn CreatedGridViewDataColumn(ColumnInfo columnInfo)
        {
            if (columnInfo.ColumnType == EColumnType.ComboBox)
            {
                return CreatedComboBoxColumn(columnInfo);
            }
            else
            {
                return CreatedTextBoxColumn(columnInfo);
            }


        }

        private GridViewComboBoxColumn CreatedComboBoxColumn(ColumnInfo columnInfo)
        {
            GridViewComboBoxColumn comboBoxColumn = new GridViewComboBoxColumn();
            comboBoxColumn.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            comboBoxColumn.HeaderText = string.IsNullOrWhiteSpace(columnInfo.HeaderText)
                ? columnInfo.FieldName
                : columnInfo.HeaderText; ;
            comboBoxColumn.FieldName = columnInfo.FieldName;
            comboBoxColumn.Name = columnInfo.FieldName;
            comboBoxColumn.ReadOnly = columnInfo.ReadOnly;
            var t = columnInfo.DataType.IsGenericType
                ? columnInfo.DataType.GenericTypeArguments[0]
                : columnInfo.DataType;
            comboBoxColumn.DataType = t;
            comboBoxColumn.DataSource = columnInfo.DataSource;
            comboBoxColumn.DisplayMember = columnInfo.DisplayMember;
            comboBoxColumn.ValueMember = columnInfo.ValueMember;
            comboBoxColumn.TextAlignment = columnInfo.TextAlignment;
            return comboBoxColumn;


        }

        private GridViewTextBoxColumn CreatedTextBoxColumn(ColumnInfo columnInfo)
        {
            var gridViewTextBoxColumn = new GridViewTextBoxColumn();
            gridViewTextBoxColumn.HeaderText = string.IsNullOrWhiteSpace(columnInfo.HeaderText)
                ? columnInfo.FieldName
                : columnInfo.HeaderText;
            gridViewTextBoxColumn.FieldName = columnInfo.FieldName;
            gridViewTextBoxColumn.Name = columnInfo.FieldName;
            gridViewTextBoxColumn.ReadOnly = columnInfo.ReadOnly;
            var t = columnInfo.DataType.IsGenericType
                ? columnInfo.DataType.GenericTypeArguments[0]
                : columnInfo.DataType;
            gridViewTextBoxColumn.TextAlignment = columnInfo.TextAlignment;
            gridViewTextBoxColumn.DataType = t;

            return gridViewTextBoxColumn;
        }

        /// <summary>
        /// 开始等待动画
        /// </summary>
        public void BeginWait()
        {
            _waitControl.Location = new Point(0, 0);
            _waitControl.Size = Size;
            Controls.Add(_waitControl);
            _waitControl.Show();
            _waitControl.BringToFront();
            _waitControl.StarWaiting();
        }

        /// <summary>
        /// 结束等待动画
        /// </summary>
        public void EndWait()
        {
            Controls.Remove(_waitControl);
            _waitControl.StopWaiting();
            _waitControl.Hide();
        }
        /// <summary>
        /// 更新等待提示
        /// </summary>
        /// <param name="tips">提示信息</param>
        public void UpdateWaitTips(string tips)
        {
            _waitControl.UpdateTips(tips);
        }

        /// <summary>
        /// 是否允许筛选
        /// </summary>
        public bool EnableFiltering
        {
            get
            {
                return radGridView1.EnableFiltering; ;
            }
            set
            {
                radGridView1.EnableFiltering = value;
            }
        }

        /// <summary>
        /// 是否允许排序
        /// </summary>
        public bool EnableSorting
        {
            get
            {
                return radGridView1.EnableSorting; ;
            }
            set
            {
                radGridView1.EnableSorting = value;
            }
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            //int count = radGridView1.DisplayedRowCount(true);
        }
    }

    public class ChangeRowCollection
    {
        public ChangeRowCollection(List<GridViewRowInfo> currNewAddRows, List<GridViewRowInfo> currDeleteRows, List<object> currDeleteRowsData, List<GridViewRowInfo> currEidtedRows)
        {
            CurrDeleteRows = currDeleteRows;
            CurrEidtedRows = currEidtedRows;
            CurrNewAddRows = currNewAddRows;
            CurrDeleteRowsData = currDeleteRowsData;
        }
        /// <summary>
        /// 新增的行
        /// </summary>
        public List<GridViewRowInfo> CurrNewAddRows { get; private set; }
        /// <summary>
        /// 删除的行
        /// </summary>
        private List<GridViewRowInfo> CurrDeleteRows { get; set; }
        /// <summary>
        /// 被修改的行
        /// </summary>
        public List<GridViewRowInfo> CurrEidtedRows { get; private set; }
        /// <summary>
        /// 被删除行的数据
        /// </summary>
        public List<object> CurrDeleteRowsData { get; private set; }

    }

    [XmlInclude(typeof(ColumnInfo))]
    [XmlInclude(typeof(string))]
    [Serializable]
    public class ColumnInfo
    {
        public ColumnInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnType">列的样式</param>
        public ColumnInfo(EColumnType columnType = EColumnType.TextBox)
        {
            PropertiesInit();
            ColumnType = columnType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">在默认均分的基础上加减宽度</param>
        /// <param name="columnType">列的样式</param>
        public ColumnInfo(int width = 0, EColumnType columnType = EColumnType.TextBox)
        {
            PropertiesInit();
            Width = width;
            ColumnType = columnType;
        }
        private void PropertiesInit()
        {
            Width = 0;
            ColumnType = EColumnType.TextBox;
        }

        public ContentAlignment TextAlignment { get; set; } = ContentAlignment.BottomCenter;
        public string HeaderText { get; set; }
        public string FieldName { get; set; }
        public Type DataType { get; set; }
        public EColumnType ColumnType { get; private set; }
        public int Width { get; private set; }
        public bool ReadOnly { get; set; } = true;
        public object DataSource { get; set; }
        public string ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }

    public enum EColumnType
    {
        TextBox,
        ComboBox
    }
}