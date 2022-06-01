// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：UCFilter.cs
//  创建时间：2018-07-23 18:00
//  作    者：
//  说    明：
//  修改时间：2018-05-17 8:18
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Extension;
using Telerik.WinControls;
using Telerik.WinControls.UI;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    public sealed partial class UcFilter : UserControl
    {
        public delegate void FilterSearchEventHandler(object sender, string expression);


        public UcFilter()
        {
            InitializeComponent();
            //Filter.BackColor = Color.Transparent;
            DoubleBuffered = true;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public bool ShowExportButton
        {
            get { return Btn_Export.Visible; }
            set { Btn_Export.Visible = value; }
        }

        public event FilterSearchEventHandler SearchEvent;
        public event EventHandler ExportEvent;

        /// <summary>
        ///     加载过滤条件
        ///     var expression = $"[创建时间] >= '{new DateTime(currTime.Year, currTime.Month, currTime.Day)}' AND [创建时间] =  '{new
        ///     DateTime(currTime.Year, currTime.Month, currTime.Day).AddDays(1)}' ";
        /// </summary>
        /// <param name="filterParams">过滤参数</param>
        /// <param name="expression">初始化默认条件</param>
        public void InitFilter(List<FilterParam> filterParams, string expression)
        {
            MyFilter.SuspendUpdate();

            MyFilter.SizeChanged += Filter_SizeChanged;
            MyFilter.Paint += Filter_Paint;

            try
            {
                MyFilter.Descriptors.Clear();
            }
            catch
            {
                // ignored
            }

            foreach (var item in filterParams)
                MyFilter.Descriptors.Add(
                    new DataFilterDescriptorItem(item.FilterDisplayName, item.ValueType) {Tag = item.FilterDbName});

            MyFilter.Expression = expression;
            try
            {
                MyFilter.AutoSize = true;
            }
            catch (Exception)
            {
                throw new Exception($"过滤器属性不包含初始化的条件[{expression}] ");
            }

            MyFilter.BringToFront();
            MyFilter.ResumeUpdate();
            MyFilter.Update();
        }


        /// <summary>
        ///     加载过滤条件
        ///     new DataFilterDescriptorItem(filterParam.FilterDisplayName, filterParam.ValueType) { filterParam =
        ///     item.FilterDbName }
        ///     var expression = $"[创建时间] >= '{new DateTime(currTime.Year, currTime.Month, currTime.Day)}' AND [创建时间] =  '{new
        ///     DateTime(currTime.Year, currTime.Month, currTime.Day).AddDays(1)}' ";
        /// </summary>
        /// <param name="filterParams">过滤参数</param>
        /// <param name="expression">初始化默认条件</param>
        public void InitFilter(List<DataFilterDescriptorItem> filterParams, string expression)
        {
            MyFilter.SuspendUpdate();

            MyFilter.SizeChanged += Filter_SizeChanged;
            MyFilter.Paint += Filter_Paint;

            try
            {
                MyFilter.Descriptors.Clear();
            }
            catch
            {
                // ignored
            }

            foreach (var item in filterParams) MyFilter.Descriptors.Add(item);

            MyFilter.Expression = expression;
            try
            {
                MyFilter.AutoSize = true;
            }
            catch (Exception)
            {
                throw new Exception($"过滤器属性不包含初始化的条件[{expression}] ");
            }

            MyFilter.BringToFront();
            MyFilter.ResumeUpdate();
            MyFilter.Update();
        }


        /// <summary>
        ///     加载过滤条件
        ///     var expression = $"[创建时间] >= '{new DateTime(currTime.Year, currTime.Month, currTime.Day)}' AND [创建时间] =  '{new
        ///     DateTime(currTime.Year, currTime.Month, currTime.Day).AddDays(1)}' ";
        /// </summary>
        /// <param name="gridView">过滤参数源</param>
        /// <param name="expression">初始化默认条件</param>
        public void InitFilter(RadGridView gridView, string expression)
        {
            MyFilter.SuspendUpdate();

            MyFilter.SizeChanged -= Filter_SizeChanged;
            MyFilter.Paint -= Filter_Paint;
            MyFilter.SizeChanged += Filter_SizeChanged;
            MyFilter.Paint += Filter_Paint;

            try
            {
                MyFilter.Descriptors.Clear();
            }
            catch (Exception)
            {
                // ignored
            }

            foreach (var item in gridView.Columns)
            {
                if (item.Name == GridViewHelper.RowNumberNameStr) continue;
                try
                {
                    MyFilter.Descriptors.Add(
                        new DataFilterDescriptorItem(item.HeaderText, item.DataType) {Tag = item.Name});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            MyFilter.Expression = expression;
            try
            {
                MyFilter.AutoSize = true;
            }
            catch (Exception)
            {
                throw new Exception($"过滤器属性不包含初始化的条件[{expression}] ");
            }

            MyFilter.BringToFront();
            MyFilter.ResumeUpdate();
            MyFilter.Update();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            MyFilter.SelectedNode = MyFilter.Nodes[0];
            var searchStr = MyFilter.GetExpressionStr();
            SearchEvent?.Invoke(this, searchStr);
        }


        private void Filter_Paint(object sender, PaintEventArgs e)
        {
            var currFilter = sender as RadDataFilter;
            if (currFilter == null) return;
            currFilter.HScrollBar.Enabled = false;
            currFilter.HScrollBar.Visibility = ElementVisibility.Hidden;
            //var p = new Pen(Color.White, 3);
            //e.Graphics.DrawRectangle(p, 0, 0, currFilter.Width, currFilter.Height);
        }

        private void Filter_NodeAdded(object sender, RadTreeViewEventArgs e)
        {
            var currFilter = sender as RadDataFilter;
            if (currFilter == null) return;
            currFilter.VScrollBar.Visibility = currFilter.Height > currFilter.Parent.Height
                ? ElementVisibility.Visible
                : ElementVisibility.Hidden;
        }

        private void Filter_NodeRemoved(object sender, RadTreeViewEventArgs e)
        {
            var currFilter = sender as RadDataFilter;
            if (currFilter == null) return;
            currFilter.VScrollBar.Visibility = currFilter.Height > currFilter.Parent.Height
                ? ElementVisibility.Visible
                : ElementVisibility.Hidden;
        }

        private void Filter_SizeChanged(object sender, EventArgs e)
        {
            Height = MyFilter.Height;
        }

        private void MyFilter_Click(object sender, EventArgs e)
        {
            try
            {
                MyFilter.ApplyFilter();
            }
            catch
            {
                // ignored
            }
        }

        private void MyFilter_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Btn_Export_Click(object sender, EventArgs e)
        {
            ExportEvent?.Invoke(sender, e);
        }
    }

    public class FilterParam
    {
        public FilterParam(string filterDisplayNamem, string filterDbName, Type valueType)
        {
            FilterDbName = filterDbName;
            ValueType = valueType;
            FilterDisplayName = filterDisplayNamem;
        }

        /// <summary>
        ///     过滤器显示名称
        /// </summary>
        public string FilterDisplayName { get; }

        /// <summary>
        ///     数据库字段名称
        /// </summary>
        public string FilterDbName { get; }

        /// <summary>
        ///     字段类型
        /// </summary>
        public Type ValueType { get; }
    }
}